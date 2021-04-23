using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EGIS.ShapeFileLib;

namespace AnalyzeTigerSchools
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private static void Analyze(string stateFips, string tigerFolderPath)
    {
      var countyShapeFile = new ShapeFile(Path.Combine(tigerFolderPath,
        @"tl_2016_us_county\tl_2016_us_county.shp"));
      var unifiedShapeFile = new ShapeFile(Path.Combine(tigerFolderPath,
        @"tl_2016_us_unsd\tl_2016_us_unsd.shp"));
      var unifiedEnumerator = unifiedShapeFile.GetShapeFileEnumerator();
      while (unifiedEnumerator.MoveNext())
      {
        var unifiedFieldValues =
          unifiedShapeFile.GetAttributeFieldValues(unifiedEnumerator.CurrentShapeIndex);
        var st = unifiedFieldValues[0].Trim();
        if (stateFips == null || stateFips == st)
        {
          var unifiedBounds = unifiedShapeFile.GetShapeBoundsD(unifiedEnumerator.CurrentShapeIndex);
          var unifiedData = unifiedShapeFile.GetShapeDataD(unifiedEnumerator.CurrentShapeIndex);
          var countyEnumerator = countyShapeFile.GetShapeFileEnumerator(unifiedBounds);
          var inCounties = new List<string>();
          while (countyEnumerator.MoveNext())
          {
            var countyFieldValues = countyShapeFile.GetAttributeFieldValues(countyEnumerator.CurrentShapeIndex);
            var inCounty = false;
            var countyData =
              countyShapeFile.GetShapeDataD(countyEnumerator.CurrentShapeIndex);
            foreach (var c in countyData)
              foreach (var p in unifiedData)
                if (GeometryAlgorithms.PolygonPolygonIntersect(c, c.Length,
                  p, p.Length))
                  inCounty = true;
            if (inCounty)
              inCounties.Add(countyFieldValues[1]); // fips
          }
        }
      }
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      Analyze("51", @"D:\Users\CurtNew\Dropbox\Documents\Vote\Tiger\2016");
    }
  }
}
