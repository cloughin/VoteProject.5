using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using static System.String;

namespace GenerateDbClasses
{
  class DatabaseGenerator
  {
    private readonly XmlElement _DatabaseElement;
    private readonly Provider _Provider;
    private readonly string _DatabaseName;
    private readonly string _ProviderKey;
    private readonly string _ConnectionStringKey;
    private readonly string _MappedDatabaseName;
    private readonly string _DatabaseClassName;
    private readonly int _CommandTimeout;

    private class ColumnSet
    {
      public string Name;
      public IList<ColumnInfo> Columns;
    }

    private static string CreateFormalArgumentList(IEnumerable<ColumnInfo> columnInfos)
    {
      return Join(", ",
        columnInfos.Where(ci => !ci.Skip).Select(
          ci =>
          {
            var isNullable = ci.Type.IsValueType && ci.IsNullable;
            return $"{ci.Type.Name}{(isNullable ? "?" : Empty)} {ci.ParameterName}";
          }));
    }

    public DatabaseGenerator(XmlElement databaseElement)
    {
      _DatabaseElement = databaseElement;

      var providerName = _DatabaseElement.GetAttribute("provider");
      if (IsNullOrWhiteSpace(providerName))
        throw new ApplicationException("'database' element is missing 'provider'.");
      _Provider = Provider.GetProvider(providerName);

      _ProviderKey = _DatabaseElement.GetAttribute("providerKey");

      var connectionString = _DatabaseElement.GetAttribute("connectionString");
      if (IsNullOrWhiteSpace(connectionString))
        throw new ApplicationException("'database' element '" + _DatabaseName +
          "' is missing 'connectionString'.");
      Provider.ConnectionString = connectionString;

      _ConnectionStringKey = _DatabaseElement.GetAttribute("connectionStringKey");

      using (var cn = _Provider.GetConnection())
      {
        _DatabaseName = cn.Database;
        if (IsNullOrWhiteSpace(_DatabaseName))
          throw new ApplicationException("Database name missing from connection ");
      }

      _MappedDatabaseName = _DatabaseElement.GetAttribute("mappedName");
      if (IsNullOrWhiteSpace(_MappedDatabaseName))
        _MappedDatabaseName = _DatabaseName;
      _MappedDatabaseName = _MappedDatabaseName.Trim();
      _DatabaseClassName = _MappedDatabaseName + "Db";

      var commandTimeoutString = _DatabaseElement.GetAttribute("commandTimeout");
      if (!int.TryParse(commandTimeoutString, out _CommandTimeout))
        _CommandTimeout = -1; // -1 = use default
    }

    private void AddCommandParameters(IEnumerable<ColumnInfo> columns)
    {
      foreach (var ci in columns)
        if (!ci.Skip)
          Writer.Write("{0}.AddCommandParameter(cmd, \"{1}\", {2});",
            _DatabaseClassName, ci.MappedName, ci.ParameterName);
    }

    //private void AddCommandParameters(IEnumerable<string> parameters)
    //{
    //  foreach (var parameter in parameters)
    //    Writer.Write("{0}.AddCommandParameter(cmd, \"{1}\", {1});",
    //      _DatabaseClassName, parameter);
    //}

    private void DecorateColumnInfos(IEnumerable<ColumnInfo> columnInfos,
      string tableName)
    {
      if (_DatabaseElement.SelectSingleNode("tables/table[@name=\"" + tableName + "\"]") is XmlElement tableElement)
        foreach (var ci in columnInfos)
        {
          if (tableElement.SelectSingleNode("columns/column[@name=\"" + ci.Name + "\"]") is XmlElement columnElement)
          {
            bool.TryParse(columnElement.GetAttribute("skip"), out var skip);
            ci.Skip = skip;
            var mappedName = columnElement.GetAttribute("mappedName");
            if (!IsNullOrWhiteSpace(mappedName))
              ci.MappedName = mappedName.Trim();
          }
        }
    }

    private IList<IndexInfo> DecorateIndexInfos(IList<IndexInfo> indexInfos,
      string tableName, string columnSetName)
    {
      IList<IndexInfo> result;
      List<IndexInfo> newIndexInfos = null;
      var parentElement =
        _DatabaseElement.SelectSingleNode("tables/table[@name=\"" + tableName + "\"]") as XmlElement;
      if (parentElement != null && columnSetName != null)
        parentElement = parentElement.SelectSingleNode("columnSets/columnSet[@name=\"" + columnSetName + "\"]") as XmlElement;
      if (parentElement?.SelectSingleNode("indexes") is XmlElement indexesElement) 
      {
        bool.TryParse(indexesElement.GetAttribute("skip"), out var skipAll);
        if (skipAll) // clear incoming
          indexInfos = new List<IndexInfo>().AsReadOnly(); // permanent empty list
        var hadDefaultElement = false;

        foreach (var indexElement in indexesElement.OfType<XmlElement>())
        {
          var columns = indexElement.GetAttribute("columns");
          string[] columnArray = null;
          if (!IsNullOrWhiteSpace(columns))
            columnArray = columns.Split(new[] { ',' },
              StringSplitOptions.RemoveEmptyEntries);
          // need a valid column array to continue
          if (columnArray != null && columnArray.Length > 0)
          {
            bool.TryParse(indexElement.GetAttribute("add"), out var add);
            bool.TryParse(indexElement.GetAttribute("skip"), out var skip);
            var name = indexElement.GetAttribute("name");
            var orderBy = indexElement.GetAttribute("orderBy");
            // check for matching column
            var match =
              indexInfos.FirstOrDefault(ii => Join(",", ii.Columns) == columns);
            var isDefault = false;
            if (!hadDefaultElement)
              bool.TryParse(indexElement.GetAttribute("default"), out isDefault);
            hadDefaultElement |= isDefault;
            if (add)
            {
              if (match == null) // don't add if it is a duplicate
              {
                bool.TryParse(indexElement.GetAttribute("unique"), out var isUnique);
                if (newIndexInfos == null)
                  newIndexInfos = new List<IndexInfo>();
                var addedIndex = new IndexInfo(false, isUnique, isDefault, 
                  orderBy) {Name = name};
                addedIndex.AddColumns(columnArray);
                newIndexInfos.Add(addedIndex);
              }
            }
            else if (match != null)
            {
              if (bool.TryParse(indexElement.GetAttribute("unique"), out var isUnique))
                match.IsUnique = isUnique;
              if (skip) match.Skip = true;
              else
              {
                match.OrderBy = orderBy;
                match.Name = name;
              }
              match.IsDefault = isDefault;
            }
          }
        }
      }
      if (newIndexInfos != null)
      {
        newIndexInfos.AddRange(indexInfos);
        result = newIndexInfos.AsReadOnly();
      }
      else result = indexInfos;
      return result;
    }

    private void DecorateTableInfo(TableInfo ti)
    {
      // set emit defaults;
      ti.EmitDelete = !ti.IsView;
      ti.EmitInsert = !ti.IsView;
      ti.EmitSelect = true;
      ti.EmitSelectColumns = true;
      ti.EmitUpdate = !ti.IsView;
      ti.EmitUpdateColumns = !ti.IsView;
      ti.EmitDeleteTable = false;
      ti.EmitTruncateTable = false;
      ti.EmitReader = false;

      if (_DatabaseElement.SelectSingleNode("tables/table[@name=\"" + ti.Name + "\"]") is XmlElement tableElement)
      {
        bool.TryParse(tableElement.GetAttribute("skip"), out var skip);
        ti.Skip = skip;

        bool.TryParse(tableElement.GetAttribute("singleton"), out var isSingleton);
        ti.IsSingleton = isSingleton;

        var mappedName = tableElement.GetAttribute("mappedName");
        if (!IsNullOrWhiteSpace(mappedName))
          ti.MappedName = mappedName.Trim();

        // set emit options
        if (bool.TryParse(tableElement.GetAttribute("delete"), out var emitOption))
          ti.EmitDelete = emitOption;
        if (bool.TryParse(tableElement.GetAttribute("insert"), out emitOption))
          ti.EmitInsert = emitOption;
        if (bool.TryParse(tableElement.GetAttribute("select"), out emitOption))
          ti.EmitSelect = emitOption;
        if (bool.TryParse(tableElement.GetAttribute("selectColumns"), out emitOption))
          ti.EmitSelectColumns = emitOption;
        if (bool.TryParse(tableElement.GetAttribute("update"), out emitOption))
          ti.EmitUpdate = emitOption;
        if (bool.TryParse(tableElement.GetAttribute("updateColumns"), out emitOption))
          ti.EmitUpdateColumns = emitOption;
        if (bool.TryParse(tableElement.GetAttribute("deleteTable"), out emitOption))
          ti.EmitDeleteTable = emitOption;
        if (bool.TryParse(tableElement.GetAttribute("truncateTable"), out emitOption))
          ti.EmitTruncateTable = emitOption;
        if (bool.TryParse(tableElement.GetAttribute("reader"), out emitOption))
          ti.EmitReader = emitOption;
        if (bool.TryParse(tableElement.GetAttribute("allColumnsNullable"), out emitOption))
          ti.AllColumnsNullable = emitOption;
      }
    }

    public void Generate()
    {
      Writer.Write("namespace {0}", _MappedDatabaseName);
      using (new Block(Block.Options.SpacerAfter))
      {
        GenerateDatabaseClass();
        foreach (var ti in _Provider.GetTableInfo())
          GenerateTable(ti);
      }
    }

    private static void GenerateColumnProperties(IEnumerable<ColumnInfo> columnInfos)
    {
      foreach (var ci in columnInfos)
        if (!ci.Skip)
        {
          Writer.Write("public DataColumn {0}Column {{ get {{ return this.Columns[\"{1}\"]; }} }}",
            ci.MappedName, ci.Name);
          Writer.Spacer();
        }
    }

    private static void GenerateColumnProperty(ColumnInfo ci)
    {
      var typeName = ci.Type.Name;
      var isNullable = ci.Type.IsValueType && ci.IsNullable;
      if (ci.Type == typeof(byte[]))
        Writer.Write("[System.Diagnostics.CodeAnalysis.SuppressMessage(\"Microsoft.Performance\", \"CA1819\")]");
      Writer.Write("public {0}{2} {1}", typeName, ci.MappedName,
        isNullable ? "?" : Empty);
      using (new Block(Block.Options.SpacerAfter))
      {
        if (isNullable)
        {
          Writer.Write("get {{ if (this.IsNull(\"{1}\")) return null; else return ({0}) this[\"{1}\"]; }}", typeName,
            ci.Name);
          Writer.Write("set {{ if (value.HasValue) this[\"{0}\"] = value.Value; else this[\"{0}\"] = DBNull.Value; }}", ci.Name);
        }
        else
        {
          if (ci.Type.IsValueType)
          {
            Writer.Write("get {{ return ({0}) this[\"{1}\"]; }}", typeName,
              ci.Name);
            Writer.Write("set {{ this[\"{0}\"] = value; }}", ci.Name);
          }
          else // reference type
          {
            Writer.Write("get {{ return this[\"{1}\"] as {0}; }}", typeName,
              ci.Name);
            Writer.Write("set {{ if (value == null) this[\"{0}\"] = DBNull.Value; else this[\"{0}\"] = value; }}", ci.Name);
          }
        }
      }
    }

    private static void GenerateConstructors(TableInfo ti, string tableClassName, 
      IReadOnlyCollection<ColumnSet> columnSets)
    {
      // ColumnSet enumeration
      var columnSetNames = Join(", ", 
        columnSets.Select(cs => cs.Name));
      Writer.Write("public enum ColumnSet {{ {0} }}", columnSetNames);
      Writer.Spacer();

      // InitSet methods
      foreach (var cs in columnSets)
      {
        Writer.Write("[SuppressMessage(\"Microsoft.Reliability\", \"CA2000\")]");
        Writer.Write("private void Init{0}Columns()", cs.Name);
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("DataColumn _column;");
          foreach (var ci in cs.Columns)
            if (!ci.Skip)
            {
              Writer.Write("_column = new DataColumn(\"{0}\", typeof({1}));",
                ci.Name, ci.Type.Name);
              if (ci.Type == typeof(string) && ci.Size > 0)
                Writer.Write("_column.MaxLength = {0};", ci.Size);
              if (!ci.IsNullable && !ci.IsAutoIncrement && !ti.AllColumnsNullable)
                Writer.Write("_column.AllowDBNull = false;");
              Writer.Write("base.Columns.Add(_column);");
            }
        }
      }

      // The default constructor
      Writer.Write("public {0}() : this(ColumnSet.All) {{ }}", tableClassName);
      Writer.Spacer();

      // The ColumnSet constructor
      Writer.Write("public {0}(ColumnSet columnSet)", tableClassName);
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("this.TableName = \"{0}\";", ti.Name);
        //Writer.Write("this.BeginInit();");
        Writer.Write("switch (columnSet)");
        using (new Block()) foreach (var cs in columnSets)
        {
          Writer.Write("case ColumnSet.{0}:", cs.Name);
          using (new Indent())
          {
            Writer.Write("Init{0}Columns();", cs.Name);
            Writer.Write("break;");
            Writer.Spacer();
          }
        }
        //Writer.Write("this.EndInit();");
      }

      // The serialization constructor
      Writer.Write("protected {0}(SerializationInfo info, StreamingContext context) : base(info, context) {{ }}", tableClassName);
      Writer.Spacer();
    }

    private void GenerateDatabaseClass()
    {
      Writer.Write("#region {0} Database", _DatabaseName);
      Writer.Spacer();

      Writer.Write("public static partial class {0}", _DatabaseClassName);
      using (new Block(Block.Options.SpacerAfter))
      {
        // ConnectionString
        Writer.Write("static string _ConnectionString = @\"{0}\";",
          Provider.ConnectionString);
        Writer.Spacer();
        Writer.Write("public static string ConnectionString");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("get {{ return _ConnectionString; }}");
          Writer.Write("set {{ _ConnectionString = value; }}");
        }

        // Provider
        Writer.Write("static DbProvider _DbProvider = DbProvider.{0};",
          _Provider.ProviderName);
        Writer.Spacer();
        Writer.Write("public static DbProvider DbProvider");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("get {{ return _DbProvider; }}");
          Writer.Write("set {{ _DbProvider = value; }}");
        }

        // static constructor
        Writer.Write("static {0}()", _DatabaseClassName);
        using (new Block(Block.Options.SpacerAfter))
        {
          // Provider
          if (!IsNullOrWhiteSpace(_ProviderKey))
          {
            Writer.Write("DbProvider provider;");
            Writer.Write("if (Enum.TryParse<DbProvider>(WebConfigurationManager.AppSettings[\"{0}\"], out provider))",
              _ProviderKey);
            using (new Indent()) Writer.Write("_DbProvider = provider;");
          }

          // ConnectionString
          if (!IsNullOrWhiteSpace(_ConnectionStringKey))
          {
            Writer.Write("string connectionString = null;",
              _ConnectionStringKey);
            Writer.Write("var cs = ConfigurationManager.ConnectionStrings[\"{0}\"];",
              _ConnectionStringKey);
            Writer.Write("if (cs != null) connectionString = cs.ConnectionString;");
            Writer.Write("if (IsNullOrWhiteSpace(connectionString))");
            using (new Indent()) Writer.Write("connectionString = WebConfigurationManager.AppSettings[\"{0}\"];",
              _ConnectionStringKey);
            Writer.Write("if (!IsNullOrWhiteSpace(connectionString))");
            using (new Indent()) Writer.Write("_ConnectionString = connectionString;");
          }
        }

        // GetConnection
        Writer.Write("public static DbConnection GetConnection()");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("switch (DbProvider)");
          using (new Block())
          {
            if (Generator.SupportMsSql)
            {
              Writer.Write("case DbProvider.MsSql:");
              using (new Indent()) Writer.Write("return new SqlConnection(ConnectionString);");
              Writer.Spacer();
            }
            if (Generator.SupportMySql)
            {
              Writer.Write("case DbProvider.MySql:");
              using (new Indent()) Writer.Write("return new MySqlConnection(ConnectionString);");
              Writer.Spacer();
            }
            Writer.Write("default:");
            using (new Indent()) Writer.Write("return null;");
            Writer.Spacer();
          }
        }

        // GetOpenConnection
        Writer.Write("public static DbConnection GetOpenConnection()");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("DbConnection cn = GetConnection();");
          Writer.Write("if (cn != null) cn.Open();");
          Writer.Write("return cn;");
        }

        // GetCommand (4 overloads)
        Writer.Write("public static DbCommand GetCommand(string cmdText)");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("return GetCommand(cmdText, null, -1);");
        }

        //Writer.Write("public static DbCommand GetCommand(string cmdText, DbConnection cn)");
        //using (new Block(Block.Options.SpacerAfter))
        //{
        //  Writer.Write("return GetCommand(cmdText, cn, -1);");
        //}

        Writer.Write("public static DbCommand GetCommand(string cmdText, int commandTimeout = -1)");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("return GetCommand(cmdText, null, commandTimeout);");
        }

        Writer.Write("[SuppressMessage(\"Microsoft.Reliability\", \"CA2000\")]");
        Writer.Write("[SuppressMessage(\"Microsoft.Security\", \"CA2100\")]");
        Writer.Write("public static DbCommand GetCommand(string cmdText, DbConnection cn, int commandTimeout)");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("switch (DbProvider)");
          using (new Block())
          {
            if (Generator.SupportMsSql)
            {
              Writer.Write("case DbProvider.MsSql:");
              using (new Indent())
              {
                Writer.Write("SqlCommand sqlCommand = new SqlCommand(cmdText, cn as SqlConnection);");
                Writer.Write("if (commandTimeout >= 0) sqlCommand.CommandTimeout = commandTimeout;");
                if (_CommandTimeout >= 0)
                  Writer.Write("else sqlCommand.CommandTimeout = {0};", _CommandTimeout);
                Writer.Write("return sqlCommand;");
              }
              Writer.Spacer();
            }
            if (Generator.SupportMySql)
            {
              Writer.Write("case DbProvider.MySql:");
              using (new Indent())
              {
                Writer.Write("MySqlCommand mySqlCommand = new MySqlCommand(cmdText, cn as MySqlConnection);");
                Writer.Write("if (commandTimeout >= 0) mySqlCommand.CommandTimeout = commandTimeout;");
                if (_CommandTimeout >= 0)
                  Writer.Write("else mySqlCommand.CommandTimeout = {0};", _CommandTimeout);
                Writer.Write("return mySqlCommand;");
              }
              Writer.Spacer();
            }
            Writer.Write("default:");
            using (new Indent()) Writer.Write("return null;");
            Writer.Spacer();
          }
        }

        // GetDataAdapter
        Writer.Write("public static DbDataAdapter GetDataAdapter(DbCommand command)");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("switch (DbProvider)");
          using (new Block())
          {
            if (Generator.SupportMsSql)
            {
              Writer.Write("case DbProvider.MsSql:");
              using (new Indent()) Writer.Write("return new SqlDataAdapter(command as SqlCommand);");
              Writer.Spacer();
            }
            if (Generator.SupportMySql)
            {
              Writer.Write("case DbProvider.MySql:");
              using (new Indent()) Writer.Write("return new MySqlDataAdapter(command as MySqlCommand);");
              Writer.Spacer();
            }
            Writer.Write("default:");
            using (new Indent()) Writer.Write("return null;");
            Writer.Spacer();
          }
        }

        // GetCommandBuilder
        Writer.Write("public static DbCommandBuilder GetCommandBuilder(DbDataAdapter adapter)");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("switch (DbProvider)");
          using (new Block())
          {
            if (Generator.SupportMsSql)
            {
              Writer.Write("case DbProvider.MsSql:");
              using (new Indent()) Writer.Write("return new SqlCommandBuilder(adapter as SqlDataAdapter);");
              Writer.Spacer();
            }
            if (Generator.SupportMySql)
            {
              Writer.Write("case DbProvider.MySql:");
              using (new Indent()) Writer.Write("return new MySqlCommandBuilder(adapter as MySqlDataAdapter);");
              Writer.Spacer();
            }
            Writer.Write("default:");
            using (new Indent()) Writer.Write("return null;");
            Writer.Spacer();
          }
        }

        // AddCommandParameter
        Writer.Write("public static void AddCommandParameter(DbCommand command, string name, object value)");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("if (!name.StartsWith(\"@\")) name = \"@\" + name;");
          Writer.Write("switch (DbProvider)");
          using (new Block())
          {
            if (Generator.SupportMsSql)
            {
              Writer.Write("case DbProvider.MsSql:");
              using (new Indent())
              {
                Writer.Write("(command as SqlCommand).Parameters.AddWithValue(name, value);");
                Writer.Write("break;");
              }
              Writer.Spacer();
            }
            if (Generator.SupportMySql)
            {
              Writer.Write("case DbProvider.MySql:");
              using (new Indent())
              {
                Writer.Write("(command as MySqlCommand).Parameters.AddWithValue(name, value);");
                Writer.Write("break;");
              }
              Writer.Spacer();
            }
          }
        }

        // ExecuteNonQuery
        Writer.Write("public static int ExecuteNonQuery(DbCommand command)");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("using (DbConnection cn = GetOpenConnection())");
          using (new Block())
          {
            Writer.Write("command.Connection = cn;");
            Writer.Write("return command.ExecuteNonQuery();");
          }
        }

        // ExecuteScalar
        Writer.Write("public static object ExecuteScalar(DbCommand command)");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("using (DbConnection cn = GetOpenConnection())");
          using (new Block())
          {
            Writer.Write("command.Connection = cn;");
            Writer.Write("return command.ExecuteScalar();");
          }
        }
      }

      Writer.Write("#endregion {0} Database", _DatabaseName);
      Writer.Spacer();
    }

    private void GenerateDeleteCommand(string tableName,
      string whereClause, IEnumerable<ColumnInfo> indexColumns,
      bool handleCommandTimeout)
    {
      if (IsNullOrWhiteSpace(whereClause))
        Writer.Write("string cmdText = \"DELETE FROM {0}\";",
          tableName);
      else
        Writer.Write("string cmdText = \"DELETE FROM {0} WHERE {1}\";",
          tableName, whereClause);
      Writer.Write("DbCommand cmd = {0}.GetCommand(cmdText, {1});",
        _DatabaseClassName, handleCommandTimeout ? "commandTimeout" : "-1");
      if (indexColumns != null)
        AddCommandParameters(indexColumns);
      Writer.Write($"return {_DatabaseClassName}.ExecuteNonQuery(cmd);");
    }

    private void GenerateGetColumnByIndexMethods(TableInfo ti, ColumnInfo ci,
      string indexName, bool isDefault, IList<ColumnInfo> indexColumns,
      string parameterList, string formalArgumentList, string whereClause)
    {
      if (indexColumns == null) indexColumns = new List<ColumnInfo>();
      // skip any columns that are in the index or marked skip
      var skip = ci.Skip;
      // we now include index columns that are string because of
      // variants considered equal
      if (!skip)
        skip = indexColumns.Contains(ci) && ci.Type != typeof(string);
      //if (!ci.Skip && !indexColumns.Contains(ci))
      if (!skip)
      {
        var typeName = ci.Type.Name;
        var defaultedMethodName = $"Get{ci.MappedName}";
        var methodName = IsNullOrWhiteSpace(indexName)
          ? $"Get{ci.MappedName}"
          : $"Get{ci.MappedName}By{indexName}";

        // The overload without a default. 
        // Returns null if not found (or if null). Uses nullables 
        // for value types. Calls the defaulted overload 
        // (private version for value types)
        if (ci.Type.IsValueType)
        {
          if (IsNullOrWhiteSpace(formalArgumentList))
            Writer.Write("public static {0}{1} {2}()",
              typeName, "?",
              methodName);
          else
            Writer.Write("public static {0}{1} {2}({3})",
              typeName, "?",
              methodName, formalArgumentList);
          using (new Block(Block.Options.SpacerAfter))
          {
            if (IsNullOrWhiteSpace(parameterList))
              Writer.Write("return {0}{1}(null);",
                "_",
                methodName);
            else
              Writer.Write("return {0}{1}({2}, null);",
                "_",
                methodName, parameterList);
          }

          // For default indexes, the wrapper without the explicit index
          if (isDefault)
          {
            if (IsNullOrWhiteSpace(formalArgumentList))
              Writer.Write("public static {0}{1} {2}()",
                typeName, "?",
                defaultedMethodName);
            else
              Writer.Write("public static {0}{1} {2}({3})",
                typeName, "?",
                defaultedMethodName, formalArgumentList);
            using (new Block(Block.Options.SpacerAfter))
            {
              Writer.Write("return {0}({1});",
                methodName, parameterList);
            }
          }
        }        
        
        // The defaulted overload.
        // Does the query for reference types. Calls the private
        // defaulted overload for value types).
        var def = ci.Type.IsValueType
          ? Empty
          : " = null";
        if (IsNullOrWhiteSpace(formalArgumentList))
          Writer.Write("public static {0} {1}({0} defaultValue{2})",
            typeName, methodName, def);
        else
          Writer.Write("public static {0} {1}({2}, {0} defaultValue{3})",
            typeName, methodName, formalArgumentList, def);
        using (new Block(Block.Options.SpacerAfter))
        {
          if (ci.Type.IsValueType)
          {
            if (IsNullOrWhiteSpace(parameterList))
              Writer.Write("return _{0}(defaultValue).Value;",
                methodName);
            else
              Writer.Write("return _{0}({1}, defaultValue).Value;",
                methodName, parameterList);
          }
          else
          {
            Writer.Write("object result;");
            GenerateSelectCommand(ti.Name, ci.Name, whereClause, null, indexColumns,
              false, $"result = {_DatabaseClassName}.ExecuteScalar(cmd);");
            Writer.Write("if (result == null || result == DBNull.Value) return defaultValue;");
            Writer.Write("else return result as {0};", typeName);
          }
        }

        // For default indexes, the wrapper without the explicit index
        if (isDefault)
        {
          if (IsNullOrWhiteSpace(formalArgumentList))
            Writer.Write("public static {0} {1}({0} defaultValue{2})",
              typeName, defaultedMethodName, def);
          else
            Writer.Write("public static {0} {1}({2}, {0} defaultValue{3})",
              typeName, defaultedMethodName, formalArgumentList, def);
          using (new Block(Block.Options.SpacerAfter))
          {
            if (IsNullOrWhiteSpace(parameterList))
              Writer.Write("return {0}(defaultValue);",
                methodName);
            else
              Writer.Write("return {0}({1}, defaultValue);",
                methodName, parameterList);
          }
        }

        if (ci.Type.IsValueType)
        {
          // The private defaulted overload, for value tyes only.
          if (IsNullOrWhiteSpace(formalArgumentList))
            Writer.Write("private static {0}? _{1}({0}? defaultValue)",
             typeName, methodName);
          else
            Writer.Write("private static {0}? _{1}({2}, {0}? defaultValue)",
             typeName, methodName, formalArgumentList);
          using (new Block(Block.Options.SpacerAfter))
          {
            Writer.Write("object result;");
            GenerateSelectCommand(ti.Name, ci.Name, whereClause, null, indexColumns,
              false, $"result = {_DatabaseClassName}.ExecuteScalar(cmd);");
            Writer.Write("if (result == null || result == DBNull.Value) return defaultValue;");
            Writer.Write("else return ({0}) result;", typeName);
          }
        }
      }
    }

    private void GenerateIndexedAccessMethods(TableInfo ti, string tableClassName,
      string readerClassName, IList<ColumnInfo> columnInfos, 
      string columnSetName, IList<ColumnInfo> allColumnInfos)
    {
      var indexInfos = _Provider.GetIndexInfo(ti.Name);
      indexInfos = DecorateIndexInfos(indexInfos, ti.Name, columnSetName);
      var columnNameList = Join(",", ColumnInfo.GetNames(columnInfos));
      var isColumnSet = !IsNullOrWhiteSpace(columnSetName);
      if (!isColumnSet) columnSetName = Empty;
      
      foreach (var ii in indexInfos)
        if (!ii.Skip)
        {
          // convert column name list to ColumnInfo 
          var indexColumns = ii.GetColumnInfos(allColumnInfos);
          var mappedIndexNames = ColumnInfo.GetMappedNames(indexColumns);
          var parameterIndexNames = ColumnInfo.GetParameterNames(indexColumns);

          var indexName = ii.Name;
          if (IsNullOrWhiteSpace(indexName))
            indexName = Join(Empty /*"_"*/, mappedIndexNames);

          var formalArgumentList = Join(", ",
            indexColumns.Where(ci => !ci.Skip).Select(
              ci => ci.Type.Name + " " + ci.ParameterName));

          var parameterList = Join(", ", parameterIndexNames);

          var whereClause = Join(" AND ",
            indexColumns.Select(
              ci => ci.Name + "=@" + ci.MappedName));

          if (!isColumnSet)
          {
            // Count or Exists
            if (ii.IsUnique) // Exists for unique indexes
            {
              Writer.Write("public static bool {0}Exists({1})",
                indexName, formalArgumentList);
              using (new Block(Block.Options.SpacerAfter))
              {
                Writer.Write("object result;");
                GenerateSelectCommand(ti.Name, "COUNT(*)", whereClause, null, indexColumns,
                  false, $"result = {_DatabaseClassName}.ExecuteScalar(cmd);");
                Writer.Write("return Convert.ToInt32(result) != 0;");
              }
            }
            else // Count for each non-unique index (include overload with timeout)
            {
              Writer.Write("public static int CountBy{0}({1}, int commandTimeout = -1)",
                indexName, formalArgumentList);
              using (new Block(Block.Options.SpacerAfter))
              {
                Writer.Write("object result;");
                GenerateSelectCommand(ti.Name, "COUNT(*)", whereClause, null, indexColumns,
                  true, $"result = {_DatabaseClassName}.ExecuteScalar(cmd);");
                Writer.Write("return Convert.ToInt32(result);");
              }
              //Writer.Write("public static int CountBy{0}({1})",
              //  indexName, formalArgumentList);
              //using (new Block(Block.Options.SpacerAfter)) Writer.Write("return CountBy{0}({1}, -1);", indexName, parameterList);
            }

            // Column access for each unique index (GetByKey)
            if (ti.EmitSelectColumns && ii.IsUnique)
              foreach (var ci in columnInfos)
                GenerateGetColumnByIndexMethods(ti, ci, indexName, ii.IsDefault, 
                  indexColumns, parameterList, formalArgumentList, whereClause);

            // GetColumnByKey (weak typed)
            Writer.Write("public static object GetColumnBy{0}(Column _column, {1})",
              indexName, formalArgumentList);
            using (new Block(Block.Options.SpacerAfter))
            {
              Writer.Write("object result;");
              GenerateSelectCommand(ti.Name, "{0}", whereClause, null, indexColumns,
                false, $"result = {_DatabaseClassName}.ExecuteScalar(cmd);");
              Writer.Write("if (result == DBNull.Value) return null;");
              Writer.Write("return result;");
            }

            if (ii.IsDefault) // default wrapper for GetColumn[ByKey]
            {
              Writer.Write("public static object GetColumn(Column _column, {0})",
                formalArgumentList);
              using (new Block(Block.Options.SpacerAfter))
              {
                Writer.Write("return GetColumnBy{0}(_column, {1});",
                  indexName, parameterList);
              }
            }
          }

          // GetDataByKey (strongly-typed DataTable -- 2 overloads)
          if (ti.EmitSelect)
          {
            //Writer.Write("public static {0} Get{3}DataBy{1}({2})",
            //  tableClassName, indexName, formalArgumentList, columnSetName);
            //using (new Block(Block.Options.SpacerAfter)) Writer.Write("return Get{2}DataBy{0}({1}, -1);", indexName,
            //  parameterList, columnSetName);

            Writer.Write("public static {0} Get{3}DataBy{1}({2}, int commandTimeout = -1)",
              tableClassName, indexName, formalArgumentList, columnSetName);
            using (new Block(Block.Options.SpacerAfter))
            {
              var csName = isColumnSet ? columnSetName : "All";
              var finalStatements = SelectTableFinalStatements(tableClassName, csName);
              GenerateSelectCommand(ti.Name, columnNameList, whereClause, ii.OrderBy, indexColumns,
                true, Join("\n", finalStatements));
            }

            if (ii.IsDefault) // default wrappers for GetData[ByKey]
            {
              //Writer.Write("public static {0} Get{2}Data({1})",
              // tableClassName, formalArgumentList, columnSetName);
              //using (new Block(Block.Options.SpacerAfter)) Writer.Write("return Get{2}DataBy{0}({1}, -1);", indexName, parameterList, 
              //  columnSetName);

              Writer.Write("public static {0} Get{2}Data({1}, int commandTimeout = -1)",
                tableClassName, formalArgumentList, columnSetName);
              using (new Block(Block.Options.SpacerAfter))
              {
                Writer.Write("return Get{2}DataBy{0}({1}, commandTimeout);", indexName,
                  parameterList,  columnSetName);
              }
            }
          }

          if (ti.EmitReader)
          {
            //Writer.Write("public static {0} Get{3}DataReaderBy{1}({2})",
            //  readerClassName, indexName, formalArgumentList, columnSetName);
            //using (new Block(Block.Options.SpacerAfter)) Writer.Write("return Get{2}DataReaderBy{0}({1}, -1);", indexName,
            //  parameterList, columnSetName);

            Writer.Write("public static {0} Get{3}DataReaderBy{1}({2}, int commandTimeout = -1)",
              readerClassName, indexName, formalArgumentList, columnSetName);
            using (new Block(Block.Options.SpacerAfter))
            {
              var csName = isColumnSet ? columnSetName : "All";
              var orderByText = Empty;
              if (!IsNullOrWhiteSpace(ii.OrderBy))
                orderByText = $" ORDER BY {ii.OrderBy}";
              if (IsNullOrWhiteSpace(whereClause))
                Writer.Write("string cmdText = GetSelectCommandText({0}.ColumnSet.{1}) + \"{2}\";", tableClassName, csName, orderByText);
              else
                Writer.Write("string cmdText = GetSelectCommandText({0}.ColumnSet.{1}) + \" WHERE {2}{3}\";", tableClassName, csName, whereClause, orderByText);
              Writer.Write("DbConnection cn = {0}.GetOpenConnection();", _DatabaseClassName);
              Writer.Write("DbCommand cmd = {0}.GetCommand(cmdText, cn, commandTimeout);",
                _DatabaseClassName);
              AddCommandParameters(indexColumns);
              Writer.Write("return new {0}(cmd.ExecuteReader(), cn);", readerClassName);
            }
          }

          if (!isColumnSet)
          {
            // UpdateByKey (unique and non-unique)
            if (ti.EmitUpdateColumns)
              foreach (var ci in columnInfos)
                GenerateUpdateColumnByIndexMethods(ti, ci, indexName, ii.IsDefault,
                  indexColumns, parameterList, formalArgumentList, whereClause);

            // UpdateColumnByKey (weak typed)
            if (ti.EmitUpdateColumns)
            {
              var methodName = $"UpdateColumnBy{indexName}";

              // prepend column and newValue to the formalArgumentList
              var newFormalArgumentList =
                $"Column _column, object newValue, {formalArgumentList}";

              Writer.Write("public static int {0}({1})", methodName,
                newFormalArgumentList);
              using (new Block(Block.Options.SpacerAfter)) GenerateUpdateCommand(ti.Name, "{0}", whereClause, indexColumns,
                false);

              if (ii.IsDefault) // default wrapper
              {
                const string defaultedMethodName = "UpdateColumn";

                // prepend column and newValue to the parameterList
                var newParameterList =
                  $"_column, newValue, {parameterList}";

                Writer.Write("public static int {0}({1})", defaultedMethodName,
                  newFormalArgumentList);
                using (new Block(Block.Options.SpacerAfter)) Writer.Write(" return {0}({1});", methodName, newParameterList);
              }
            }

            // DeleteByKey (2 overloads)
            if (ti.EmitDelete)
            {
              //Writer.Write("public static int DeleteBy{0}({1})",
              //  indexName, formalArgumentList);
              //using (new Block(Block.Options.SpacerAfter)) Writer.Write("return DeleteBy{0}({1}, -1);", indexName, parameterList);

              Writer.Write("public static int DeleteBy{0}({1}, int commandTimeout = -1)",
                indexName, formalArgumentList);
              using (new Block(Block.Options.SpacerAfter)) GenerateDeleteCommand(ti.Name, whereClause, indexColumns, true);
            }
          }
        }
    }

    private void GenerateInsertCommand(string tableName,
      IList<ColumnInfo> insertColumns, bool handleCommandTimeout,
      ColumnInfo autoIncrementColumn)
    {
      var columnList = Join(",", ColumnInfo.GetNames(insertColumns));

      var valuesList = Join(",",
        insertColumns
        .Where(ci => !ci.Skip)
        .Select(
          ci => "@" + ci.MappedName));

      Writer.Write("string cmdText = \"INSERT INTO {0} ({1}) VALUES ({2})\";",
        tableName, columnList, valuesList);
      if (autoIncrementColumn != null) // inject code to return autoIncrement value
      {
        Writer.Write("switch ({0}.DbProvider)", _DatabaseClassName);
        using (new Block())
        {
          if (Generator.SupportMySql)
          {
            Writer.Write("case DbProvider.MySql:");
            using (new Indent())
            {
              Writer.Write("cmdText += \"; SELECT LAST_INSERT_ID()\";");
              Writer.Write("break;");
            }
            Writer.Spacer();
          }
          if (Generator.SupportMsSql)
          {
            Writer.Write("case DbProvider.MsSql:");
            using (new Indent())
            {
              Writer.Write("cmdText += \"; SELECT SCOPE_IDENTITY()\";");
              Writer.Write("break;");
            }
            Writer.Spacer();
          }
          Writer.Write("default:");
          using (new Indent()) Writer.Write("throw new ApplicationException(\"Unsupported provider.\");");
          Writer.Spacer();
        }
      }
      Writer.Write("DbCommand cmd = {0}.GetCommand(cmdText, {1});",
        _DatabaseClassName, handleCommandTimeout ? "commandTimeout" : "-1");
      AddCommandParameters(insertColumns);
      if (autoIncrementColumn == null)
        Writer.Write($"{_DatabaseClassName}.ExecuteNonQuery(cmd);");
      else
        switch (autoIncrementColumn.Type.Name)
        {
          case "Int32":
            Writer.Write(
              $"return Convert.To{autoIncrementColumn.Type.Name}({_DatabaseClassName}.ExecuteScalar(cmd));");
            break;

          default:
            Writer.Write(
              $"return ({autoIncrementColumn.Type.Name}) {_DatabaseClassName}.ExecuteScalar(cmd);");
           break;
        }
    }

    private static void GenerateRowClass(IEnumerable<ColumnInfo> columnInfos,
      string rowClassName)
    {
      Writer.Write("public partial class {0} : DataRow", rowClassName);
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("internal {0}(DataRowBuilder rb) : base(rb) {{}}", rowClassName);
        Writer.Spacer();
        foreach (var ci in columnInfos)
          if (!ci.Skip)
            GenerateColumnProperty(ci);
      }
    }

    private void GenerateSelectCommand(string tableName, string columnList,
      string whereClause, string orderByClause, IEnumerable<ColumnInfo> indexColumns,
      bool handleCommandTimeout, string executeStatement)
    {
      var orderByText = Empty;
      if (!IsNullOrWhiteSpace(orderByClause))
        orderByText = $" ORDER BY {orderByClause}";
      if (IsNullOrWhiteSpace(whereClause))
        Writer.Write("string cmdText = \"SELECT {0} FROM {1}{2}\";",
          columnList, tableName, orderByText);
      else
        Writer.Write("string cmdText = \"SELECT {0} FROM {1} WHERE {2}{3}\";",
          columnList, tableName, whereClause, orderByText);
      if (columnList.StartsWith("{")) // do substutution
        Writer.Write("cmdText = Format(cmdText, GetColumnName(_column));");
      Writer.Write("DbCommand cmd = {0}.GetCommand(cmdText, {1});",
        _DatabaseClassName, handleCommandTimeout ? "commandTimeout" : "-1");
      if (indexColumns != null)
        AddCommandParameters(indexColumns);
      Writer.Write(executeStatement);
    }

    private void GenerateSingletonAccessMethods(TableInfo ti,
      IList<ColumnInfo> columnInfos)
    {
      //var columnNameList = Join(",", ColumnInfo.GetNames(columnInfosList));

      // Exists
      Writer.Write("public static bool Exists()");
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("object result;");
        GenerateSelectCommand(ti.Name, "COUNT(*)", null, null, null,
          false, $"result = {_DatabaseClassName}.ExecuteScalar(cmd);");
        Writer.Write("return Convert.ToInt32(result) != 0;");
      }

      // Column access for each unique index (GetByKey)
      if (ti.EmitSelectColumns)
        foreach (var ci in columnInfos)
          GenerateGetColumnByIndexMethods(ti, ci, null, false, null, null, null, null);

      // GetColumn (weak typed)
      Writer.Write("public static object GetColumn(Column _column)");
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("object result;");
        GenerateSelectCommand(ti.Name, "{0}", null, null, null,
          false, $"result = {_DatabaseClassName}.ExecuteScalar(cmd);");
        Writer.Write("if (result == DBNull.Value) return null;");
        Writer.Write("return result;");
      }

      // Update
      if (ti.EmitUpdateColumns)
        foreach (var ci in columnInfos)
          GenerateUpdateColumnByIndexMethods(ti, ci, null, false, null, null, null, null);

      // UpdateColumn (weak typed)
      if (ti.EmitUpdateColumns)
      {
        Writer.Write("public static int UpdateColumn(Column _column, object newValue)");
        using (new Block(Block.Options.SpacerAfter)) GenerateUpdateCommand(ti.Name, "{0}", null, null, false);
      }
    }

    private void GenerateTable(TableInfo ti)
    {
      DecorateTableInfo(ti);
      if (!ti.Skip)
      {
        var columnInfos = _Provider.GetColumnInfo(ti.Name, ti.AllColumnsNullable);
        DecorateColumnInfos(columnInfos, ti.Name);
        var rowClassName = ti.MappedName + "Row";
        Writer.Write("#region {0}.{1}", _DatabaseName, ti.Name);
        Writer.Spacer();

        GenerateRowClass(columnInfos, rowClassName);
        GenerateTableClasses(ti, columnInfos, rowClassName);

        Writer.Write("#endregion {0}.{1}", _DatabaseName, ti.Name);
        Writer.Spacer();
      }
    }

    private void GenerateTableClasses(TableInfo ti, 
      IList<ColumnInfo> columnInfos, string rowClassName)
    {
      var tableClassName = ti.MappedName + "Table";
      var readerClassName = ti.MappedName + "Reader";

      var columnSets = new List<ColumnSet> { new ColumnSet { Name = "All", Columns = columnInfos } };
      // add the "All" column set

      GenerateTypedTable(ti, columnInfos, rowClassName, tableClassName, columnSets);
      GenerateStaticTableClass(ti, columnInfos, tableClassName, readerClassName, columnSets);
      if (ti.EmitReader)
        GenerateTypedReaderClass(columnInfos, readerClassName);
    }

    private void GenerateStaticTableClass(TableInfo ti, IList<ColumnInfo> columnInfos, 
      string tableClassName, string readerClassName, IReadOnlyCollection<ColumnSet> columnSets)
    {
      var tableStaticClassName = ti.MappedName;
      var formalArgumentList = CreateFormalArgumentList(columnInfos);
      var mappedNameString = Join(", ", ColumnInfo.GetMappedNames(columnInfos));
      var columnNameList = Join(",", ColumnInfo.GetNames(columnInfos));

      Writer.Write("public static partial class {0}", tableStaticClassName);
      using (new Block(Block.Options.SpacerAfter))
      {
        GenerateStaticTableName(ti);
        GenerateStaticTableColumnEnum(columnInfos, mappedNameString);
        GenerateStaticTableGetColumn();
        GenerateStaticTableCountTable(ti);
        GenerateStaticTableDeleteTable(ti);
        GenerateStaticTableTruncateTable(ti);
        GenerateStaticTableInsert(ti, columnInfos, formalArgumentList);
        GenerateStaticTableSelectAllCommandText(ti, columnNameList);
        GenerateStaticTableGetAllData(ti, tableClassName, columnNameList);
        GenerateStaticTableGetAllDataReader(ti, readerClassName);
        GenerateStaticTableFillTable(tableClassName);
        GenerateStaticTableUpdateTable(ti, tableClassName);
        GenerateStaticTableColumnNameProperties(columnInfos);
        GenerateStaticTableMaxLengthProperties(columnInfos);

        if (ti.IsSingleton)
          GenerateSingletonAccessMethods(ti, columnInfos);
        else
          GenerateIndexedAccessMethods(ti, tableClassName, readerClassName,
            columnInfos, null, columnInfos);

        GenerateStaticTableColumnSets(ti, columnInfos, tableClassName, readerClassName, columnSets);
        GenerateStaticTableGetSelectCommandText(tableClassName, columnSets);
        GenerateStaticTableConstructor(tableStaticClassName);
      }
    }

    private static void GenerateStaticTableName(TableInfo ti)
    {
      Writer.Write("public const string TableName = \"" + ti.Name + "\";");
      Writer.Spacer();
    }

    private static void GenerateStaticTableColumnEnum(IEnumerable<ColumnInfo> columnInfos, string mappedNameString)
    {
      Writer.Write("public enum Column {{ {0} }}", mappedNameString);
      Writer.Spacer();

      // ColumnDictionary 
      Writer.Write("private static Dictionary<Column, string> ColumnDictionary = new Dictionary<Column, string>() {{");
      using (new Indent()) Writer.Write(Join(",\n",
        columnInfos
          .Where(ci => !ci.Skip)
          .Select(ci => $"{{{{ Column.{ci.MappedName}, \"{ci.Name}\" }}}}")) + " }};");
      Writer.Spacer();

      // ColumnNameDictionary 
      Writer.Write("private static Dictionary<string, Column> ColumnNameDictionary;");
      Writer.Spacer();

      // GetColumnName
      Writer.Write("public static string GetColumnName(Column _column)");
      using (new Block(Block.Options.SpacerAfter)) Writer.Write("return ColumnDictionary[_column];");
    }

    private static void GenerateStaticTableGetColumn()
    {
      Writer.Write("public static Column GetColumn(string columnName)");
      using (new Block(Block.Options.SpacerAfter)) Writer.Write("return ColumnNameDictionary[columnName];");

      // TryGetColumn
      Writer.Write("public static bool TryGetColumn(string columnName, out Column _column)");
      using (new Block(Block.Options.SpacerAfter)) Writer.Write("return ColumnNameDictionary.TryGetValue(columnName, out _column);");

      // GetMappedColumn -- uses mapped name
      Writer.Write("public static Column GetMappedColumn(string columnName)");
      using (new Block(Block.Options.SpacerAfter)) Writer.Write("return (Column) Enum.Parse(typeof(Column), columnName, false);");

      // TryGetMappedColumn
      Writer.Write("public static bool TryGetMappedColumn(string columnName, out Column _column)");
      using (new Block(Block.Options.SpacerAfter)) Writer.Write("return Enum.TryParse(columnName, false, out _column);");
    }

    private void GenerateStaticTableCountTable(TableInfo ti)
    {
      //Writer.Write("public static int CountTable()");
      //using (new Block(Block.Options.SpacerAfter))
      //{
      //  Writer.Write("return CountTable(-1);");
      //}

      Writer.Write("public static int CountTable(int commandTimeout = -1)");
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("object result;");
        GenerateSelectCommand(ti.Name, "COUNT(*)", null, null, null,
          true, $"result = {_DatabaseClassName}.ExecuteScalar(cmd);");
        Writer.Write("return Convert.ToInt32(result);");
      }
    }

    private void GenerateStaticTableDeleteTable(TableInfo ti)
    {
      if (ti.EmitDeleteTable)
      {
        //Writer.Write("public static int DeleteTable()");
        //using (new Block(Block.Options.SpacerAfter))
        //{
        //  Writer.Write("return DeleteTable(-1);");
        //}

        Writer.Write("public static int DeleteTable(int commandTimeout = -1)");
        using (new Block(Block.Options.SpacerAfter))
        {
          GenerateDeleteCommand(ti.Name, null, null, true);
        }
      }
    }

    private void GenerateStaticTableTruncateTable(TableInfo ti)
    {
      if (ti.EmitTruncateTable)
      {
        //Writer.Write("public static void TruncateTable()");
        //using (new Block(Block.Options.SpacerAfter))
        //{
        //  Writer.Write("TruncateTable(-1);");
        //}

        Writer.Write("public static void TruncateTable(int commandTimeout = -1)");
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("string cmdText = \"TRUNCATE TABLE {0}\";", ti.Name);
          Writer.Write("DbCommand cmd = {0}.GetCommand(cmdText, commandTimeout);",
            _DatabaseClassName);
          Writer.Write($"{_DatabaseClassName}.ExecuteNonQuery(cmd);");
        }
      }
    }

    private void GenerateStaticTableInsert(TableInfo ti, IList<ColumnInfo> columnInfos, 
      string formalArgumentList)
    {
      if (ti.EmitInsert)
      {
        var autoIncrementColumn =
          ColumnInfo.GetAutoIncrementColumn(columnInfos);

        var returnTypeName = "void";
        if (autoIncrementColumn != null)
        {
          returnTypeName = autoIncrementColumn.Type.Name;
        }

        //Writer.Write("public static {0} Insert({1})",
        //  returnTypeName, formalArgumentList);
        //using (new Block(Block.Options.SpacerAfter)) Writer.Write("{0}Insert({1}, -1);",
        //  returnStatementLeader, parameterNameString);

        Writer.Write("public static {0} Insert({1}, int commandTimeout = -1)",
          returnTypeName, formalArgumentList);
        using (new Block(Block.Options.SpacerAfter)) GenerateInsertCommand(ti.Name, columnInfos, true, autoIncrementColumn);

        // for tables with an autoincrement, we emit 2 more overloads
        // with the autoincrement column omitted

        if (autoIncrementColumn != null)
        {
          var nonAutoIncrementColumnInfos =
            columnInfos
            .Where(ci => ci != autoIncrementColumn)
            .ToList();
          var nonAutoIncrementFormalArgumentList =
            CreateFormalArgumentList(nonAutoIncrementColumnInfos);

          //Writer.Write("public static {0} Insert({1})",
          //  returnTypeName, nonAutoIncrementFormalArgumentList);
          //using (new Block(Block.Options.SpacerAfter)) Writer.Write("{0}Insert({1}, -1);",
          //  returnStatementLeader, nonAutoIncrementParameterNameString);

          Writer.Write("public static {0} Insert({1}, int commandTimeout = -1)",
            returnTypeName, nonAutoIncrementFormalArgumentList);
          using (new Block(Block.Options.SpacerAfter)) GenerateInsertCommand(ti.Name, nonAutoIncrementColumnInfos,
            true, autoIncrementColumn);
        }
      }
    }

    private static void GenerateStaticTableSelectAllCommandText(TableInfo ti, string columnNameList)
    {
      Writer.Write("public static string SelectAllCommandText");
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("get");
        using (new Block()) Writer.Write("return \"SELECT {0} FROM {1}\";", columnNameList, ti.Name);
      }
    }

    private void GenerateStaticTableGetAllData(TableInfo ti, string tableClassName, string columnNameList)
    {
      if (ti.EmitSelect)
      {
        //Writer.Write("public static {0} GetAllData()", tableClassName);
        //using (new Block(Block.Options.SpacerAfter)) Writer.Write("return GetAllData(-1);");

        Writer.Write("public static {0} GetAllData(int commandTimeout = -1)", tableClassName);
        using (new Block(Block.Options.SpacerAfter))
        {
          var finalStatements = SelectTableFinalStatements(tableClassName, "All");
          GenerateSelectCommand(ti.Name, columnNameList, null, null, null,
            true, Join("\n", finalStatements));
        }
      }
    }

    private void GenerateStaticTableGetAllDataReader(TableInfo ti, string readerClassName)
    {
      if (ti.EmitReader)
      {
        //Writer.Write("public static {0} GetAllDataReader()", readerClassName);
        //using (new Block(Block.Options.SpacerAfter)) Writer.Write("return GetAllDataReader(-1);");

        Writer.Write("public static {0} GetAllDataReader(int commandTimeout = -1)", readerClassName);
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("string cmdText = SelectAllCommandText;");
          Writer.Write("DbConnection cn = {0}.GetOpenConnection();", _DatabaseClassName);
          Writer.Write("DbCommand cmd = {0}.GetCommand(cmdText, cn, commandTimeout);",
            _DatabaseClassName);
          Writer.Write("return new {0}(cmd.ExecuteReader(), cn);",  readerClassName);
        }
      }
    }

    private void GenerateStaticTableFillTable(string tableClassName)
    {
      Writer.Write("public static {0} FillTable(DbCommand command)",
        tableClassName);
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("return FillTable(command, {0}.ColumnSet.All);", tableClassName);
      }

      Writer.Write("[SuppressMessage(\"Microsoft.Reliability\", \"CA2000\")]");
      Writer.Write("public static {0} FillTable(DbCommand command, {0}.ColumnSet columnSet)",
        tableClassName);
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("using (DbConnection cn = {0}.GetOpenConnection())",
          _DatabaseClassName);
        using (new Block())
        {
          Writer.Write("command.Connection = cn;");
          Writer.Write("{0} table = new {0}(columnSet);", tableClassName);
          Writer.Write("DbDataAdapter adapter = {0}.GetDataAdapter(command);",
            _DatabaseClassName);
          Writer.Write("adapter.Fill(table);");
          Writer.Write("return table;");
        }
      }
    }

    private void GenerateStaticTableUpdateTable(TableInfo ti, string tableClassName)
    {
      if (ti.EmitUpdate)
      {
        //Writer.Write("public static void UpdateTable({0} table)",
        //  tableClassName);
        //using (new Block(Block.Options.SpacerAfter))
        //{
        //  Writer.Write("UpdateTable(table, {0}.ColumnSet.All, -1, ConflictOption.CompareAllSearchableValues, false);",
        //    tableClassName);
        //}

        //Writer.Write("public static void UpdateTable({0} table, {0}.ColumnSet columnSet)",
        //  tableClassName);
        //using (new Block(Block.Options.SpacerAfter))
        //{
        //  Writer.Write("UpdateTable(table, columnSet, -1, ConflictOption.CompareAllSearchableValues, false);");
        //}

        //Writer.Write("public static void UpdateTable({0} table, int commandTimeout)",
        //  tableClassName);
        //using (new Block(Block.Options.SpacerAfter))
        //{
        //  Writer.Write("UpdateTable(table, {0}.ColumnSet.All, commandTimeout);",
        //    tableClassName);
        //}

        //Writer.Write("public static void UpdateTable({0} table, {0}.ColumnSet columnSet, int commandTimeout)",
        //  tableClassName);
        //using (new Block(Block.Options.SpacerAfter))
        //{
        //  Writer.Write("UpdateTable(table, columnSet, commandTimeout, ConflictOption.CompareAllSearchableValues, false);");
        //}

        Writer.Write("public static void UpdateTable({0} table, int commandTimeout, ConflictOption conflictOption = ConflictOption.CompareAllSearchableValues, bool continueUpdateOnError = false)",
          tableClassName);
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("UpdateTable(table, {0}.ColumnSet.All, commandTimeout, conflictOption, continueUpdateOnError);",
            tableClassName);
        }

        Writer.Write("public static void UpdateTable({0} table, {0}.ColumnSet columnSet = {0}.ColumnSet.All, int commandTimeout = -1, ConflictOption conflictOption = ConflictOption.CompareAllSearchableValues, bool continueUpdateOnError = false)",
          tableClassName);
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("using (DbConnection cn = {0}.GetOpenConnection())",
            _DatabaseClassName);
          using (new Block())
          {
            Writer.Write("string cmdText = GetSelectCommandText(columnSet);");
            Writer.Write("DbCommand cmd = {0}.GetCommand(cmdText, cn, commandTimeout);",
              _DatabaseClassName);
            Writer.Write("DbDataAdapter adapter = {0}.GetDataAdapter(cmd);",
              _DatabaseClassName);
            Writer.Write("adapter.ContinueUpdateOnError = continueUpdateOnError;");
            Writer.Write("DbCommandBuilder builder = {0}.GetCommandBuilder(adapter);",
              _DatabaseClassName);
            Writer.Write("builder.ConflictOption = conflictOption;");
            Writer.Write("adapter.Update(table);");
          }
        }
      }
    }

    private static void GenerateStaticTableColumnNameProperties(IEnumerable<ColumnInfo> columnInfos)
    {
      foreach (var ci in columnInfos)
        if (!ci.Skip)
        {
          Writer.Write("public static string {0}ColumnName {{ get {{ return \"{1}\"; }} }}",
            ci.MappedName, ci.Name);
          Writer.Spacer();
        }
    }

    private static void GenerateStaticTableMaxLengthProperties(IEnumerable<ColumnInfo> columnInfos)
    {
      foreach (var ci in columnInfos)
        if (!ci.Skip && ci.Type == typeof(string))
        {
          var maxLength = "int.MaxValue";
          if (ci.Size > 0) maxLength = ci.Size.ToString(CultureInfo.InvariantCulture);
          Writer.Write("public static int {0}MaxLength {{ get {{ return {1}; }} }}",
            ci.MappedName, maxLength);
          Writer.Spacer();
        }
    }

    private void GenerateStaticTableColumnSets(TableInfo ti,
      IList<ColumnInfo> columnInfos, string tableClassName,
      string readerClassName, IEnumerable<ColumnSet> columnSets)
    {
      foreach (var cs in columnSets)
        if (cs.Name != "All")
        {
          GenerateTableClassColumnSet(ti, cs.Name, cs.Columns,
            tableClassName, readerClassName, columnInfos);
        }
    }

    private static void GenerateStaticTableGetSelectCommandText(string tableClassName, IEnumerable<ColumnSet> columnSets)
    {
      Writer.Write("public static string GetSelectCommandText({0}.ColumnSet columnSet)",
        tableClassName);
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("switch(columnSet)");
        using (new Block())
        {
          foreach (var cs in columnSets)
          {
            Writer.Write("case {0}.ColumnSet.{1}:", tableClassName, cs.Name);
            using (new Indent())
            {
              Writer.Write("return Select{0}CommandText;", cs.Name);
              Writer.Spacer();
            }
          }
          Writer.Write("default:");
          using (new Indent()) Writer.Write("return null;");
        }
      }
    }

    private static void GenerateStaticTableConstructor(string tableStaticClassName)
    {
      // static constructor (to properly time the initialization of
      // ColumnNameDictionary
      Writer.Write("static {0}()", tableStaticClassName);
      using (new Block(Block.Options.SpacerAfter)) Writer.Write("ColumnNameDictionary = ColumnDictionary.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);");
    }

    private void GenerateTypedTable(TableInfo ti, IList<ColumnInfo> columnInfos, 
      string rowClassName, string tableClassName, List<ColumnSet> columnSets)
    {
      var formalArgumentList = CreateFormalArgumentList(columnInfos);
      var parameterNameString = Join(", ", ColumnInfo.GetParameterNames(columnInfos));
      //var mappedNameString = Join(", ", ColumnInfo.GetMappedNames(columnInfosList));
      //var columnNameList = Join(",", ColumnInfo.GetNames(columnInfosList));

      Writer.Write("[Serializable]");
      Writer.Write("public partial class {0} : TypedTableBase<{1}>",
        tableClassName, rowClassName);
      using (new Block(Block.Options.SpacerAfter))
      {
        // indexer
        Writer.Write("public {0} this[int index] {{ get {{ return ({0})(this.Rows[index]); }} }}",
          rowClassName);
        Writer.Spacer();

        // Count property
        Writer.Write("public int Count {{ get {{ return this.Rows.Count; }} }}");
        Writer.Spacer();

        // AddRow
        Writer.Write("public void AddRow({0} row) {{ this.Rows.Add(row); }}",
          rowClassName);
        Writer.Spacer();

        // CreateInstance
        Writer.Write("protected override DataTable CreateInstance() {{ return new {0}(); }}",
          tableClassName);
        Writer.Spacer();

        // NewRow
        Writer.Write("new public {0} NewRow() {{ return ({0}) (base.NewRow()); }}",
          rowClassName);
        Writer.Spacer();

        // NewRowFromBuilder
        Writer.Write("protected override DataRow NewRowFromBuilder(DataRowBuilder rb)");
        Writer.Write("{{ return new {0}(rb); }}", rowClassName);
        Writer.Spacer();

        // GetRowType
        Writer.Write("protected override Type GetRowType()");
        Writer.Write("{{ return typeof({0}); }}", rowClassName);
        Writer.Spacer();

        // RemoveRow
        Writer.Write("public void RemoveRow({0} row) {{ this.Rows.Remove(row); }}",
          rowClassName);
        Writer.Spacer();

        // NewRow all columns overload
        Writer.Write("public {0} NewRow({1})", rowClassName, formalArgumentList);
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("{0} row = NewRow();", rowClassName);
          foreach (var ci in columnInfos)
            if (!ci.Skip)
              Writer.Write("row.{0} = {1};", ci.MappedName, ci.ParameterName);
          Writer.Write("return row;");
        }

        // AddRow all columns overload
        Writer.Write("public void AddRow({0})", formalArgumentList);
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("{0} row = NewRow({1});", rowClassName, parameterNameString);
          Writer.Write("AddRow(row);");
        }

        // for tables with an autoincrement, we emit another set of overloads
        // with the autoincrement column omitted

        var autoIncrementColumn =
          ColumnInfo.GetAutoIncrementColumn(columnInfos);
        if (autoIncrementColumn != null)
        {
          var nonAutoIncrementColumnInfos =
            columnInfos
              .Where(ci => ci != autoIncrementColumn)
              .ToList();
          var nonAutoIncrementFormalArgumentList =
            CreateFormalArgumentList(nonAutoIncrementColumnInfos);
          var nonAutoIncrementParameterNameString =
            Join(", ", ColumnInfo.GetParameterNames(nonAutoIncrementColumnInfos));

          Writer.Write("public {0} NewRow({1})", rowClassName, nonAutoIncrementFormalArgumentList);
          using (new Block(Block.Options.SpacerAfter))
          {
            Writer.Write("{0} row = NewRow();", rowClassName);
            foreach (var ci in nonAutoIncrementColumnInfos)
              if (!ci.Skip)
                Writer.Write("row.{0} = {1};", ci.MappedName, ci.ParameterName);
            Writer.Write("return row;");
          }

          Writer.Write("public void AddRow({0})",
            nonAutoIncrementFormalArgumentList);
          using (new Block(Block.Options.SpacerAfter))
          {
            Writer.Write("{0} row = NewRow({1});", rowClassName, nonAutoIncrementParameterNameString);
            Writer.Write("AddRow(row);");
          }
        }

        GenerateColumnProperties(columnInfos);
        GetColumnSets(ti, columnInfos, columnSets);
        GenerateConstructors(ti, tableClassName, columnSets);
      }
    }

    private static void GenerateTypedReaderClass(IEnumerable<ColumnInfo> columnInfos,
      string readerClassName)
    {
      Writer.Write("public partial class {0} : DataReaderBase", readerClassName);
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("public {0}(DbDataReader dataReader, DbConnection connection) : base(dataReader, connection) {{ }}",
          readerClassName);
        Writer.Spacer();
        foreach (var ci in columnInfos)
          if (!ci.Skip)
            GenerateTypedReaderColumnProperty(ci);
      }
    }

    private static void GenerateTypedReaderColumnProperty(ColumnInfo ci)
    {
      var typeName = ci.Type.Name;
      var isNullable = ci.Type.IsValueType && ci.IsNullable;
      if (ci.Type == typeof(byte[]))
        Writer.Write("[System.Diagnostics.CodeAnalysis.SuppressMessage(\"Microsoft.Performance\", \"CA1819\")]");
      Writer.Write("public {0}{2} {1}", typeName, ci.MappedName,
        isNullable ? "?" : Empty);
      using (new Block(Block.Options.SpacerAfter))
      {
        if (isNullable)
        {
          Writer.Write("get {{ if (this.IsDBNull(GetOrdinal(\"{1}\"))) return null; else return ({0}) this[\"{1}\"]; }}", 
            typeName, ci.Name);
        }
        else
        {
          Writer.Write(ci.Type.IsValueType
            ? "get {{ return ({0}) this[\"{1}\"]; }}"
            : "get {{ return this[\"{1}\"] as {0}; }}", typeName, ci.Name);
        }
      }
    }

    private void GenerateTableClassColumnSet(TableInfo ti, string columnSetName, 
      IList<ColumnInfo> columnSetInfos, string tableClassName,
      string readerClassName, IList<ColumnInfo> allColumnInfos)
    {
      var columnSetInfosList = columnSetInfos;
      var columnSetNameList = Join(",", ColumnInfo.GetNames(columnSetInfosList));

      // SelectSetCommandText
      Writer.Write("public static string Select{0}CommandText", columnSetName);
      using (new Block(Block.Options.SpacerAfter))
      {
        Writer.Write("get");
        using (new Block()) Writer.Write("return \"SELECT {0} FROM {1}\";", columnSetNameList, ti.Name);
      }

      // GetAllSetData (2 overloads)
      if (ti.EmitSelect)
      {
        //Writer.Write("public static {0} GetAll{1}Data()", tableClassName, columnSetName);
        //using (new Block(Block.Options.SpacerAfter)) Writer.Write("return GetAll{0}Data(-1);", columnSetName);

        Writer.Write("public static {0} GetAll{1}Data(int commandTimeout = -1)",
          tableClassName, columnSetName);
        using (new Block(Block.Options.SpacerAfter))
        {
          var finalStatements = SelectTableFinalStatements(tableClassName, columnSetName);
          GenerateSelectCommand(ti.Name, columnSetNameList, null, null, null,
            true, Join("\n", finalStatements));
        }
      }

      // GetAllSetDataReader (2 overloads)
      if (ti.EmitReader)
      {
        //Writer.Write("public static {0} GetAll{1}DataReader()", readerClassName, columnSetName);
        //using (new Block(Block.Options.SpacerAfter)) Writer.Write("return GetAll{0}DataReader(-1);", columnSetName);

        Writer.Write("public static {0} GetAll{1}DataReader(int commandTimeout = -1)",
          readerClassName, columnSetName);
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("string cmdText = GetSelectCommandText({0}.ColumnSet.{1});", tableClassName, columnSetName);
          Writer.Write("DbConnection cn = {0}.GetOpenConnection();", _DatabaseClassName);
          Writer.Write("DbCommand cmd = {0}.GetCommand(cmdText, cn, commandTimeout);",
            _DatabaseClassName);
          Writer.Write("return new {0}(cmd.ExecuteReader(), cn);", readerClassName);
        }
      }

      // Update (2 overloads)
      if (ti.EmitUpdate)
      {
        Writer.Write("public static void Update{0}({1} table)", columnSetName,
          tableClassName);
        using (new Block(Block.Options.SpacerAfter))
        {
          Writer.Write("Update{0}(table, -1, ConflictOption.CompareAllSearchableValues, false);",
            columnSetName);
        }

        Writer.Write("public static void Update{0}({1} table, int commandTimeout, ConflictOption conflictOption, bool continueUpdateOnError)",
          columnSetName, tableClassName);
        using (new Block(Block.Options.SpacerAfter))
        {
          //string[] finalStatements = UpdateTableFinalStatements(tableClassName, columnSetName);
          //GenerateSelectCommand(ti.Name, columnSetNameList, null, null, null,
          //  true, Join("\n", finalStatements));
          Writer.Write("UpdateTable(table, {0}.ColumnSet.{1}, commandTimeout, conflictOption, continueUpdateOnError);",
            tableClassName, columnSetName);
        }
      }

      GenerateIndexedAccessMethods(ti, tableClassName, readerClassName,
        columnSetInfosList, columnSetName, allColumnInfos);
    }

    private void GenerateUpdateColumnByIndexMethods(TableInfo ti, ColumnInfo ci,
      string indexName, bool isDefault, IEnumerable<ColumnInfo> indexColumns,
      string parameterList, string formalArgumentList, string whereClause)
    {
      // skip any columns that are marked skip (but process index columns)
      if (!ci.Skip)
      {
        var methodName = IsNullOrWhiteSpace(indexName)
          ? $"Update{ci.MappedName}"
          : $"Update{ci.MappedName}By{indexName}";
        var isNullable = ci.Type.IsValueType && ci.IsNullable;

        // prepend newValue to the formalArgumentList
        formalArgumentList = IsNullOrWhiteSpace(formalArgumentList) 
          ? $"{ci.Type.Name}{(isNullable ? "?" : Empty)} newValue" 
          : $"{ci.Type.Name}{(isNullable ? "?" : Empty)} newValue, {formalArgumentList}";

        Writer.Write("public static int {0}({1})", methodName, formalArgumentList);
        using (new Block(Block.Options.SpacerAfter)) GenerateUpdateCommand(ti.Name, ci.Name, whereClause, indexColumns,
          isNullable);

        if (isDefault)
        {
          var defaultedMethodName = $"Update{ci.MappedName}";

          Writer.Write("public static int {0}({1})", defaultedMethodName, 
            formalArgumentList);
          using (new Block(Block.Options.SpacerAfter)) Writer.Write("return {0}(newValue, {1});", methodName, parameterList);
        }
      }
    }

    private void GenerateUpdateCommand(string tableName, string columnName,
      string whereClause, IEnumerable<ColumnInfo> indexColumns,
      bool newValueIsNullable)
    {
      if (IsNullOrWhiteSpace(whereClause))
        Writer.Write("string cmdText = \"UPDATE {0} SET {1}=@newValue\";",
          tableName, columnName);
      else
        Writer.Write("string cmdText = \"UPDATE {0} SET {1}=@newValue WHERE {2}\";",
          tableName, columnName, whereClause);
      if (columnName.StartsWith("{")) // do substutution
        Writer.Write("cmdText = Format(cmdText, GetColumnName(_column));");
      Writer.Write("DbCommand cmd = {0}.GetCommand(cmdText, -1);",
        _DatabaseClassName);
      if (indexColumns != null)
        AddCommandParameters(indexColumns);
      if (newValueIsNullable)
      {
        Writer.Write("object o = null;");
        Writer.Write("if (newValue.HasValue) o = newValue.Value;");
        Writer.Write("{0}.AddCommandParameter(cmd, \"{1}\", {2});",
          _DatabaseClassName, "newValue", "o");
      }
      else
        Writer.Write("{0}.AddCommandParameter(cmd, \"{1}\", {1});",
          _DatabaseClassName, "newValue");
      Writer.Write($"return {_DatabaseClassName}.ExecuteNonQuery(cmd);");
    }

    private void GetColumnSets(TableInfo ti,
      IList<ColumnInfo> columnInfos,
      ICollection<ColumnSet> columnSets)
    {
      if (_DatabaseElement.SelectSingleNode("tables/table[@name=\"" + ti.Name + "\"]/columnSets") is XmlElement columnSetsElement)
        foreach (var columnSetElement in columnSetsElement.OfType<XmlElement>())
        {
          var columnSetName = columnSetElement.GetAttribute("name");
          var columns = columnSetElement.GetAttribute("columns");
          if (!IsNullOrWhiteSpace(columnSetName) &&
            !IsNullOrWhiteSpace(columns))
          {
            var columnArray = columns.Split(',');
            var columnSetInfos = columnInfos.Where(
              ci => !ci.Skip && columnArray.Contains(ci.Name)).ToList().AsReadOnly();
            if (columnSetInfos.Count > 0)
            {
              //GenerateTableClassColumnSet(ti, columnSetName, columnSetInfos,
              //  tableClassName, columnInfos);
              columnSets.Add(new ColumnSet { Name = columnSetName, Columns = columnSetInfos });
            }
          }
        }
    }

    private static string[] SelectTableFinalStatements(string tableClassName,
      string columnSetName)
    {
      var finalStatements = new[]
      {
        $"return FillTable(cmd, {tableClassName}.ColumnSet.{columnSetName});"
      };
      return finalStatements;
    }
  }
}
