using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vote;

namespace TestFacebookVideos
{
  static class Program
  {
    private static void Main()
    {
      var info = FacebookVideoUtility.GetVideoInfo("1349045911849193", false);
    }
  }
}
