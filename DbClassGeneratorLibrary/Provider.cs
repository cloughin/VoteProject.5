using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using static System.String;

namespace GenerateDbClasses
{
  abstract class Provider
  {
    public static string ConnectionString { get; set; }

    //public static string DatabaseName { get; set; }

    protected static IList<IndexInfo> FilterUniqueIndexes(
      IEnumerable<IndexInfo> result)
    {
      // Use GroupBy to get Distinct entries (so we can use lambda)
      // Duplicate keys would generate compile errors. Always favor a unique 
      // duplicate.
      return result.GroupBy(
        i => Join(Empty, i.Columns),
        (key, group) =>
        {
          var groupList = group.ToList();
          var unique = groupList.FirstOrDefault(ii => ii.IsUnique);
          if (unique == null)
            return groupList.First();
          return unique;
        }
        )
        .ToList()
        .AsReadOnly();
    }

    public abstract IList<ColumnInfo> GetColumnInfo(string tableName, bool allColumnsNullable);

    public abstract DbConnection GetConnection();

    public abstract IList<IndexInfo> GetIndexInfo(string tableName);

    public static Provider GetProvider(string providerName)
    {
      Provider provider;

      switch (providerName.ToLowerInvariant().Trim())
      {
        case "mssql":
          if (!Generator.SupportMsSql)
            throw new ApplicationException("MsSql support not enabled.");
          provider = new MsSqlProvider();
          break;

        case "mysql":
           if (!Generator.SupportMySql)
            throw new ApplicationException("MySql support not enabled.");
           provider = new MySqlProvider();
           break;

        default:
          throw new ApplicationException("Unknown provider '" + providerName + "'");
      }

      return provider;
    }

    public abstract IEnumerable<TableInfo> GetTableInfo();

    public abstract string ProviderName { get; }
  }
}
