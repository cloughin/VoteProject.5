namespace GenerateDbClasses
{
  class TableInfo
  {
    public TableInfo(string name, bool isView)
    {
      Name = name;
      MappedName = name;
      IsView = isView;
    }

    public bool AllColumnsNullable { get; set; }

    public bool EmitDelete { get; set; }

    public bool EmitDeleteTable { get; set; }

    public bool EmitInsert { get; set; }

    public bool EmitReader { get; set; }

    public bool EmitSelect { get; set; }

    public bool EmitSelectColumns { get; set; }

    public bool EmitTruncateTable { get; set; }

    public bool EmitUpdate { get; set; }

    public bool EmitUpdateColumns { get; set; }

    public bool IsSingleton { get; set; }

    public bool IsView { get; }

    public string MappedName { get; set; }

    public string Name { get; private set; }

    public bool Skip { get; set; }
  }
}
