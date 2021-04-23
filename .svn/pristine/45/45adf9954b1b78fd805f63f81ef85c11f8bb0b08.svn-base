using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.Vote;
using DB.VoteLog;
using Vote;

namespace AdHoc
{
  class Program
  {
    static void Main(string[] args)
    {
      //var start = DateTime.UtcNow;
      //db2.Report_Officials_Update(PageCache.GetTemporary(), "VA");
      //var elapsed = DateTime.UtcNow - start;

      var allCountyOffices = Offices.CountAllCountyOffices("VA");
      var allLocalOffices = Offices.CountAllLocalOffices("VA", "059");
      var countyCounts = Offices.CountCountyOfficesByCounty("VA");
      var localCounts = Offices.CountLocalOfficesByLocal("VA", "059");
      var counties = CountyCache.GetCountiesByState("VA");
      var locals = Offices.GetLocalNamesWithOffices("VA", "059");
    }
  }
}
