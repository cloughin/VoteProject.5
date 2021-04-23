using System.Collections.Generic;
using System.Linq;

namespace GenerateDbClasses
{
  class IndexInfo
  {
    private readonly List<string> _Columns;

    public IndexInfo(bool isPrimary, bool isUnique, bool isDefault = false, string orderBy = null)
    {
      IsPrimary = isPrimary;
      IsUnique = isUnique;
      IsDefault = isDefault;
      _Columns = new List<string>();
      OrderBy = orderBy;
    }

    public void AddColumn(string name)
    {
      _Columns.Add(name);
    }

    public void AddColumns(IEnumerable<string> columns)
    {
      _Columns.AddRange(columns);
    }

    //public int ColumnCount
    //{
    //  get
    //  {
    //    return _Columns.Count;
    //  }
    //}

    public IEnumerable<string> Columns
    {
      get { return _Columns.AsReadOnly(); }
    }

    public IList<ColumnInfo> GetColumnInfos(IEnumerable<ColumnInfo> cis)
    {
      return _Columns.Select(
        name => cis.Single(ci => ci.Name == name)).ToList().AsReadOnly();
    }

    public bool IsDefault { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool IsPrimary { get; set; }

    public bool IsUnique { get; set; }

    public string Name { get; set; }

    public string OrderBy { get; set; }

    public bool Skip { get; set; }
  }
}
