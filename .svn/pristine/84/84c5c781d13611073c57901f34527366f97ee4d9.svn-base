using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.Vote;
using Vote;

namespace AnalyzePoliticianNames
{
  static class Program
  {
    static void Main(string[] args)
    {
      var table = Politicians.GetAllNamesData();
      var list = table.Cast<PoliticiansRow>()
        .Where(row => row.LastName.IsNeIgnoreCase(row.LastName.StripAccents()))
        .Select(
          row => new {PoliticianKey = row.PoliticianKey, LastName = row.LastName})
        .ToList();
    }
  }
}
