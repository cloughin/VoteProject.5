using System;
using System.Collections.Generic;

namespace NormalizeNewLines
{
  public class TableInfo
  {
    public string Name;
    public List<string> PrimaryKeyColumns;
    public List<ColumnInfo> TextColumns;
    public bool Enabled;

    public override string ToString()
    {
      return Name;
    }
  }

  public class ColumnInfo
  {
    public string Name;
    public int Size;
  }
}