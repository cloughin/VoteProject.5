using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Vote;
using DB.Vote;

namespace TestRedirect
{
  public partial class XmlTest : Form
  {
    public XmlTest()
    {
      InitializeComponent();
    }

    private void XmlTest_Load(object sender, EventArgs e)
    {
      CreateBallotData("VA", "VA20101102GA", "011", "034", "067", "059");
    }

    private static void CreateBallotData(string stateCode, string electionKey, 
      string congressionalDistrict, string stateSenateCode, string stateHouseCode, 
      string countyCode)
    {
      StringBuilder sb = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings();
      using (XmlWriter writer = XmlWriter.Create(sb, settings))
      {
        writer.WriteStartElement("ballot");

        string stateName = StateCache.GetStateName(stateCode);
        if (stateName == null) stateName = string.Empty;
        string countyName = db.Name_County(stateCode, countyCode);

        writer.WriteAttributeString("stateCode", stateCode);
        writer.WriteAttributeString("stateName", stateName);
        writer.WriteAttributeString("countyCode", countyCode);
        writer.WriteAttributeString("countyName", countyName);
        writer.WriteAttributeString("congressionalDistrict", congressionalDistrict);
        writer.WriteAttributeString("stateSenateCode", stateSenateCode);
        writer.WriteAttributeString("stateHouseCode", stateHouseCode);

        WriteElectionData(writer, electionKey);
        writer.WriteEndElement();
        writer.Flush();
      }
    }

    private static void WriteElectionData(XmlWriter writer, string electionKey)
    {
      writer.WriteStartElement("election");
      writer.WriteAttributeString("key", electionKey);

      ElectionsTable table = Elections.GetDataByElectionKey(electionKey);
      if (table.Count == 1)
      {
        ElectionsRow row = table[0];
        writer.WriteAttributeString("description", row.ElectionDesc);
      }

      writer.WriteEndElement();
    }
  }
}
