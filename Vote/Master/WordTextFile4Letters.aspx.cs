using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web.Mail;
using System.IO;

namespace Vote.Master
{
  public partial class CandidateInfo4Letters : VotePage
  {
    //private void WriteHeadingCandidates(StreamWriter sw)
    //{
    //  WriteOneCandidateDataLine(sw
    //    , "Name"
    //    , "Office"
    //    , "Candidate"
    //    , "Address"
    //    , "CityStateZip"
    //    , "Username"
    //    , "Password"
    //    , "CD"
    //    , "State"
    //    , "Election"
    //    );
    //}
    //private void WriteHeadingPoliticalParties(StreamWriter sw)
    //{
    //  WriteOnePartyDataLine(sw
    //    , "Name"
    //    , "Address"
    //    , string.Empty
    //    , "CityStateZip"
    //    , "StateCode"
    //    );
    //}
    //private void WriteHeadingStates(StreamWriter sw)
    //{
    //  WriteOneStateDataLine(sw
    //    , "StateCode"
    //    , "State"
    //    , "Name"
    //    , string.Empty
    //    , "ElectionsAuthority"
    //    , "AddressLine1"
    //    , "AddressLine2"
    //    , "AddressLine3"
    //    );
    //}
    //private void WriteOneCandidateDataLine
    //  (StreamWriter sw
    //  , string Name
    //  , string Office
    //  , string Candidate
    //  , string Address
    //  , string CityStateZip
    //  , string Username
    //  , string Password
    //  , string StateCode
    //  , string State
    //  , string Election
    //  )
    //{
    //  #region Text File Line
    //  string Line = string.Empty;
    //  Line += Name + "\t";
    //  Line += Office + "\t";
    //  Line += Candidate + "\t";
    //  Line += Address + "\t";
    //  Line += CityStateZip + "\t";
    //  Line += Username + "\t";
    //  Line += Password + "\t";
    //  Line += StateCode + "\t";
    //  Line += State + "\t";
    //  Line += Election + "\t";
    //  #endregion
    //  sw.WriteLine(Line);
    //}
    //private void WriteOnePartyDataLine
    //  (StreamWriter sw
    //  , string Name
    //  , string PartyAddressLine1
    //  , string PartyAddressLine2
    //  , string CityStateZip
    //  , string StateCode
    //  )
    //{
    //  #region Text File Line
    //  string Line = string.Empty;
    //  Line += Name + "\t";
    //  Line += PartyAddressLine1;
    //  if (PartyAddressLine2 != string.Empty)
    //    Line += ", " + PartyAddressLine2;
    //  Line += "\t";
    //  Line += CityStateZip + "\t";
    //  Line += StateCode + "\t";
    //  #endregion
    //  sw.WriteLine(Line);
    //}
    //private void WriteOneStateDataLine
    //  (StreamWriter sw
    //  , string StateCode
    //  , string State
    //  , string Contact
    //  , string ContactTitle
    //  , string ElectionsAuthority
    //  , string AddressLine1
    //  , string AddressLine2
    //  , string CityStateZip
    //  )
    //{
    //  #region Text File Line
    //  string Line = string.Empty;
    //  Line += StateCode + "\t";
    //  Line += State + "\t";
    //  Line += Contact;
    //  if (ContactTitle != string.Empty)
    //    Line += ", " + ContactTitle;
    //  Line += "\t";
    //  Line += ElectionsAuthority + "\t";
    //  Line += AddressLine1 + "\t";
    //  if (AddressLine2 != string.Empty)
    //  {
    //    Line += AddressLine2 + "\t";
    //    Line += CityStateZip + "\t";
    //  }
    //  else
    //  {
    //    AddressLine2 = CityStateZip;
    //    Line += AddressLine2 + "\t";
    //    Line += "\t";
    //  }
    //  #endregion
    //  sw.WriteLine(Line);
    //}
    //private void WriteDataLines4Candidate(StreamWriter sw, DataRow CandidateRow, DataRow OfficeRow)
    //{
    //  WriteOneCandidateDataLine(sw
    //    , "Campaign Manager"
    //    , db.Offices_Str(OfficeRow["OfficeKey"].ToString(), "OfficeLine1")
    //    + " " + db.Offices_Str(OfficeRow["OfficeKey"].ToString(), "OfficeLine2")
    //    , CandidateRow["FName"].ToString() + " " + CandidateRow["LName"].ToString()
    //    , db.Politician_Address_Any_For_Textbox(OfficeRow["PoliticianKey"].ToString())
    //    //, db.Politician_CityStateZip_Candidate_Or_NA(CandidateRow)
    //    , db.Politician_CityStateZip_Any_For_Textbox(OfficeRow["PoliticianKey"].ToString())
    //    , CandidateRow["PoliticianKey"].ToString()
    //    , CandidateRow["Password"].ToString()
    //    , CandidateRow["StateCode"].ToString()
    //    , StateCache.GetStateName(Session["UserStateCode"].ToString())
    //    , db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionDesc")
    //    );
    //  if (
    //    (CandidateRow["CampaignName"].ToString() != string.Empty)
    //    && (CandidateRow["CampaignAddr"].ToString() != string.Empty)
    //    && (CandidateRow["CampaignCityStateZip"].ToString() != string.Empty)
    //    )
    //  {
    //    WriteOneCandidateDataLine(sw
    //      , CandidateRow["CampaignName"].ToString()
    //      , db.Offices_Str(OfficeRow["OfficeKey"].ToString(), "OfficeLine1")
    //      + " " + db.Offices_Str(OfficeRow["OfficeKey"].ToString(), "OfficeLine2")
    //      , CandidateRow["FName"].ToString() + " " + CandidateRow["LName"].ToString()
    //      , CandidateRow["CampaignAddr"].ToString()
    //      , CandidateRow["CampaignCityStateZip"].ToString()
    //      , CandidateRow["PoliticianKey"].ToString()
    //      , CandidateRow["Password"].ToString()
    //      , CandidateRow["StateCode"].ToString()
    //      , StateCache.GetStateName(Session["UserStateCode"].ToString())
    //      , db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionDesc")
    //      );
    //  }
    //}
    //private void WriteDataLines4Party(StreamWriter sw, DataRow PartyRow, string StateCode)
    //{
    //  if (
    //    (PartyRow["PartyName"].ToString() != string.Empty)
    //    && (PartyRow["PartyAddressLine1"].ToString() != string.Empty)
    //    && (PartyRow["PartyCityStateZip"].ToString() != string.Empty)
    //    )
    //  {
    //    WriteOnePartyDataLine(sw
    //      , PartyRow["PartyName"].ToString()
    //      , PartyRow["PartyAddressLine1"].ToString()
    //      , PartyRow["PartyAddressLine2"].ToString()
    //      , PartyRow["PartyCityStateZip"].ToString()
    //      , StateCode
    //      );
    //  }
    //}
    //private void WriteDataLines4State(StreamWriter sw, DataRow StateRow)
    //{
    //  if (
    //    (StateRow["Contact"].ToString() != string.Empty)
    //    && (StateRow["ElectionsAuthority"].ToString() != string.Empty)
    //    && (StateRow["AddressLine1"].ToString() != string.Empty)
    //    && (StateRow["CityStateZip"].ToString() != string.Empty)
    //    )
    //  {
    //    WriteOneStateDataLine(sw
    //      , StateRow["StateCode"].ToString()
    //      , StateRow["State"].ToString()
    //      , StateRow["Contact"].ToString()
    //      , StateRow["ContactTitle"].ToString()
    //      , StateRow["ElectionsAuthority"].ToString()
    //      , StateRow["AddressLine1"].ToString()
    //      , StateRow["AddressLine2"].ToString()
    //      , StateRow["CityStateZip"].ToString()
    //      );
    //  }
    //}

    //protected void ButtonMakeFile_Click1(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    int CandidateCount = 0;
    //    DataTable OfficesTable = null;

    //    string path = string.Empty;
    //    //if (db.Is_Localhost)
    //    //{
    //    //  #region Text File to localhost
    //    //  path = @"D:\Inetpub\wwwroot\Master\CandidateData" + Session["UserStateCode"].ToString() + ".txt";
    //    //  using (StreamWriter sw = new StreamWriter
    //    //    //(Server.MapPath("CandidateData" + Session["UserStateCode"].ToString() + ".txt").ToString())
    //    //    (path)
    //    //   )
    //    //  {
    //    //    WriteHeadingCandidates(sw);

    //    //    OfficesTable = db.Table(sql.Elections_Offices_Text_File(ViewState["ElectionKey"].ToString()));
    //    //    foreach (DataRow OfficeRow in OfficesTable.Rows)
    //    //    {
    //    //      DataTable CandidatesTable = db.Table(sql.Candidates4Letters(
    //    //        OfficeRow["ElectionKey"].ToString()
    //    //        , OfficeRow["OfficeKey"].ToString()));
    //    //      foreach (DataRow CandidateRow in CandidatesTable.Rows)
    //    //      {
    //    //        WriteDataLines4Candidate(sw, CandidateRow, OfficeRow);
    //    //      }
    //    //    }
    //    //  }
    //    //  #endregion
    //    //}
    //    //else
    //    //{
    //    #region Text File to Production Server
    //    //using (Impersonate imp = new Impersonate(Environment.MachineName, "Administrator", "v0+3u$@"))
    //    //{
    //    //path = @"\\vote-1\d\Webhosting\Master\CandidateData" + Session["UserStateCode"].ToString() + ".txt";
    //    //path = db.PathProduction1() + @"Master\CandidateData" + Session["UserStateCode"].ToString() + ".txt";
    //    path = GetServerPath() + @"Master\CandidateData" + Session["UserStateCode"].ToString() + ".txt";
    //    using (StreamWriter sw = new StreamWriter
    //      //(Server.MapPath("CandidateData" + Session["UserStateCode"].ToString() + ".txt").ToString())
    //      (path)
    //     )
    //    {
    //      WriteHeadingCandidates(sw);

    //      OfficesTable = db.Table(sql.Elections_Offices_Text_File(ViewState["ElectionKey"].ToString()));
    //      foreach (DataRow OfficeRow in OfficesTable.Rows)
    //      {
    //        DataTable CandidatesTable = db.Table(sql.Candidates4Letters(
    //          OfficeRow["ElectionKey"].ToString()
    //          , OfficeRow["OfficeKey"].ToString()));
    //        foreach (DataRow CandidateRow in CandidatesTable.Rows)
    //        {
    //          WriteDataLines4Candidate(sw, CandidateRow, OfficeRow);
    //        }
    //      }
    //      //}
    //      //}
    //    #endregion
    //    }

    //    #region Page Report

    //    #region Page Report Heading
    //    string PoliticiansHTMLTable = string.Empty;
    //    PoliticiansHTMLTable += "<table cellspacing=0 cellpadding=0>";
    //    PoliticiansHTMLTable += "<tr>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "Name";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "Office";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "Candidate";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "Address";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "CityStateZip";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "Username";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "Password";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "CD";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "State";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "Election";

    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "</tr>";
    //    #endregion


    //    OfficesTable = db.Table(sql.Elections_Offices_Text_File(ViewState["ElectionKey"].ToString()));
    //    foreach (DataRow OfficeRow in OfficesTable.Rows)
    //    {
    //      DataTable CandidatesTable = db.Table(sql.Candidates4Letters(
    //        OfficeRow["ElectionKey"].ToString()
    //        , OfficeRow["OfficeKey"].ToString()));
    //      foreach (DataRow CandidateRow in CandidatesTable.Rows)
    //      {
    //        #region Report Row
    //        CandidateCount++;

    //        PoliticiansHTMLTable += "<tr>";

    //        PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //        PoliticiansHTMLTable += "Campaign Manager";
    //        PoliticiansHTMLTable += "</td>";


    //        PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //        PoliticiansHTMLTable += db.Offices_Str(OfficeRow["OfficeKey"].ToString(), "OfficeLine1")
    //          + " " + db.Offices_Str(OfficeRow["OfficeKey"].ToString(), "OfficeLine2");
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //        PoliticiansHTMLTable += CandidateRow["FName"].ToString();
    //        PoliticiansHTMLTable += " " + CandidateRow["LName"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //        PoliticiansHTMLTable += db.Politician_Address_Any_For_Textbox(
    //          CandidateRow["PoliticianKey"].ToString());
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //        PoliticiansHTMLTable += db.Politician_CityStateZip_Any_For_Textbox(
    //          CandidateRow["PoliticianKey"].ToString());
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //        PoliticiansHTMLTable += CandidateRow["PoliticianKey"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //        PoliticiansHTMLTable += CandidateRow["Password"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //        PoliticiansHTMLTable += CandidateRow["StateCode"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //        PoliticiansHTMLTable += StateCache.GetStateName(Session["UserStateCode"].ToString());
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //        PoliticiansHTMLTable += db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionDesc");
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "</tr>";
    //        #endregion

    //        if (
    //          (CandidateRow["CampaignName"].ToString() != string.Empty)
    //          && (CandidateRow["CampaignAddr"].ToString() != string.Empty)
    //          && (CandidateRow["CampaignCityStateZip"].ToString() != string.Empty)
    //          )
    //        {
    //          #region Report Row
    //          PoliticiansHTMLTable += "<tr>";

    //          PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //          PoliticiansHTMLTable += CandidateRow["CampaignName"].ToString();
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //          PoliticiansHTMLTable += db.Offices_Str(OfficeRow["OfficeKey"].ToString(), "OfficeLine1")
    //            + " " + db.Offices_Str(OfficeRow["OfficeKey"].ToString(), "OfficeLine2");
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //          PoliticiansHTMLTable += CandidateRow["FName"].ToString();
    //          PoliticiansHTMLTable += " " + CandidateRow["LName"].ToString();
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //          PoliticiansHTMLTable += CandidateRow["CampaignAddr"].ToString();//
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //          PoliticiansHTMLTable += CandidateRow["CampaignCityStateZip"].ToString();
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //          PoliticiansHTMLTable += CandidateRow["PoliticianKey"].ToString();
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //          PoliticiansHTMLTable += CandidateRow["Password"].ToString();
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //          PoliticiansHTMLTable += CandidateRow["StateCode"].ToString();
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //          PoliticiansHTMLTable += StateCache.GetStateName(Session["UserStateCode"].ToString());
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "<td align=center class=tdReportDetail>";
    //          PoliticiansHTMLTable += db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionDesc");
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "</tr>";
    //          #endregion
    //        }

    //        string SQL = "UPDATE Politicians "
    //          + " SET IntroLetterSent = " + db.SQLLit(Db.DbNow)
    //          + " WHERE PoliticianKey = " + db.SQLLit(CandidateRow["PoliticianKey"].ToString());
    //        db.ExecuteSQL(SQL);
    //      }
    //    }

    //    #region Finish Page Report
    //    PoliticiansHTMLTable += "</table>";
    //    LabelCandidateDataTable.Text = PoliticiansHTMLTable;
    //    #endregion

    //    #endregion

    //    Msg.Text = db.Msg("File: /Master/CandidateData" + Session["UserStateCode"].ToString()
    //      + ".txt has " + CandidateCount.ToString() + " Candidates.");
    //  }

    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);

    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}
    //protected void ButtonPoliticalParties_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    int PartyCount = 0;
    //    DataTable PartiesTable = null;

    //    string path = string.Empty;
    //    //if (db.Is_Localhost)
    //    //{
    //    //  #region Text File to localhost
    //    //  path = @"D:\Inetpub\wwwroot\Master\PartyAddresses.txt";
    //    //  using (StreamWriter sw = new StreamWriter
    //    //    (path)
    //    //   )
    //    //  {
    //    //    WriteHeadingPoliticalParties(sw);

    //    //    PartiesTable = db.Table(sql.Parties());
    //    //    foreach (DataRow PartyRow in PartiesTable.Rows)
    //    //    {
    //    //      string StateCode = string.Empty;
    //    //      if (PartyRow["StateCode"].ToString() == "US")
    //    //        StateCode = "USA";
    //    //      else
    //    //        StateCode = PartyRow["StateCode"].ToString();

    //    //      WriteDataLines4Party(sw, PartyRow, StateCode);
    //    //    }
    //    //  }
    //    //  #endregion
    //    //}
    //    //else
    //    //{
    //    #region Text File to Production Server
    //    //using (Impersonate imp = new Impersonate(Environment.MachineName, "Administrator", "v0+3u$@"))
    //    //{
    //    //path = @"\\vote-1\d\Webhosting\Master\PartyAddresses.txt";
    //    //path = db.PathProduction1() + @"Master\PartyAddresses.txt";
    //    path = GetServerPath() + @"Master\PartyAddresses.txt";
    //    using (StreamWriter sw = new StreamWriter
    //      (path)
    //     )
    //    {
    //      WriteHeadingPoliticalParties(sw);

    //      PartiesTable = db.Table(sql.Parties());
    //      foreach (DataRow PartyRow in PartiesTable.Rows)
    //      {
    //        string StateCode = string.Empty;
    //        if (PartyRow["StateCode"].ToString() == "US")
    //          StateCode = "USA";
    //        else
    //          StateCode = PartyRow["StateCode"].ToString();

    //        WriteDataLines4Party(sw, PartyRow, StateCode);
    //      }
    //      //}
    //      //}
    //    #endregion
    //    }

    //    #region Page Report

    //    #region Page Report Heading
    //    string PoliticiansHTMLTable = string.Empty;
    //    PoliticiansHTMLTable += "<table cellspacing=0 cellpadding=0>";
    //    PoliticiansHTMLTable += "<tr>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "Name";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "Address";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "CityStateZip";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "StateCode";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "</tr>";
    //    #endregion


    //    PartiesTable = db.Table(sql.Parties());
    //    foreach (DataRow PartyRow in PartiesTable.Rows)
    //    {
    //      if (
    //        (PartyRow["PartyName"].ToString() != string.Empty)
    //        && (PartyRow["PartyAddressLine1"].ToString() != string.Empty)
    //        && (PartyRow["PartyCityStateZip"].ToString() != string.Empty)
    //        )
    //      {
    //        #region Only Parties with and address
    //        PoliticiansHTMLTable += "<tr>";

    //        PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //        PoliticiansHTMLTable += PartyRow["PartyName"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //        PoliticiansHTMLTable += PartyRow["PartyAddressLine1"].ToString();
    //        if (PartyRow["PartyAddressLine2"].ToString() != string.Empty)
    //          PoliticiansHTMLTable += ", " + PartyRow["PartyAddressLine2"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //        PoliticiansHTMLTable += PartyRow["PartyCityStateZip"].ToString();
    //        PoliticiansHTMLTable += "</td>";


    //        string StateCode = string.Empty;
    //        if (PartyRow["StateCode"].ToString() == "US")
    //          StateCode = "USA";
    //        else
    //          StateCode = PartyRow["StateCode"].ToString();
    //        PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //        PoliticiansHTMLTable += StateCode;
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "</tr>";

    //        PartyCount++;
    //        #endregion
    //      }
    //    }

    //    #region Finish Page Report
    //    PoliticiansHTMLTable += "</table>";
    //    LabelCandidateDataTable.Text = PoliticiansHTMLTable;
    //    #endregion

    //    #endregion

    //    Msg.Text = db.Msg("File: Master/PartyAddresses.txt has "
    //     + PartyCount.ToString() + " Addresses.");
    //  }

    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);

    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }

    //}
    //protected void ButtonStateAuthorities_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    int StatesCount = 0;
    //    DataTable StatesTable = null;

    //    string path = string.Empty;
    //    //if (db.Is_Localhost)
    //    //{
    //    //  #region Text File to localhost
    //    //  path = @"D:\Inetpub\wwwroot\Master\StateAuthorities.txt";
    //    //  using (StreamWriter sw = new StreamWriter
    //    //    (path)
    //    //   )
    //    //  {
    //    //    WriteHeadingStates(sw);

    //    //    StatesTable = db.Table(sql.States51Letters());
    //    //    foreach (DataRow StateRow in StatesTable.Rows)
    //    //    {
    //    //      WriteDataLines4State(sw, StateRow);
    //    //    }
    //    //  }
    //    //  #endregion
    //    //}
    //    //else
    //    //{
    //    #region Text File to Production Server
    //    //using (Impersonate imp = new Impersonate(Environment.MachineName, "Administrator", "v0+3u$@"))
    //    //{
    //    //path = @"\\vote-1\d\Webhosting\Master\StateAuthorities.txt";
    //    //path = db.PathProduction1() + @"Master\StateAuthorities.txt";
    //    path = GetServerPath() + @"Master\StateAuthorities.txt";
    //    using (StreamWriter sw = new StreamWriter
    //        (path)
    //       )
    //    {
    //      WriteHeadingStates(sw);

    //      StatesTable = db.Table(sql.States51Letters());
    //      foreach (DataRow StateRow in StatesTable.Rows)
    //      {
    //        WriteDataLines4State(sw, StateRow);
    //      }
    //      //}
    //      //}
    //    #endregion
    //    }

    //    #region Page Report

    //    #region Page Report Heading
    //    string PoliticiansHTMLTable = string.Empty;
    //    PoliticiansHTMLTable += "<table cellspacing=0 cellpadding=0>";
    //    PoliticiansHTMLTable += "<tr>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "StateCode";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "State";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "Name";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "ElectionsAuthority";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "AddressLine1";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "AddressLine2";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //    PoliticiansHTMLTable += "AddressLine3";
    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "</td>";

    //    PoliticiansHTMLTable += "</tr>";
    //    #endregion


    //    StatesTable = db.Table(sql.States51Letters());
    //    foreach (DataRow StateRow in StatesTable.Rows)
    //    {
    //      if (
    //        (StateRow["Contact"].ToString() != string.Empty)
    //        && (StateRow["ElectionsAuthority"].ToString() != string.Empty)
    //        && (StateRow["AddressLine1"].ToString() != string.Empty)
    //        && (StateRow["CityStateZip"].ToString() != string.Empty)
    //        )
    //      {
    //        #region Only Parties with and address
    //        PoliticiansHTMLTable += "<tr>";

    //        PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //        PoliticiansHTMLTable += StateRow["StateCode"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //        PoliticiansHTMLTable += StateRow["State"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //        PoliticiansHTMLTable += StateRow["Contact"].ToString();
    //        if (StateRow["ContactTitle"].ToString() != string.Empty)
    //          PoliticiansHTMLTable += ", " + StateRow["ContactTitle"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //        PoliticiansHTMLTable += StateRow["ElectionsAuthority"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //        PoliticiansHTMLTable += StateRow["AddressLine1"].ToString();
    //        PoliticiansHTMLTable += "</td>";

    //        if (StateRow["AddressLine2"].ToString() != string.Empty)
    //        {
    //          PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //          PoliticiansHTMLTable += StateRow["AddressLine2"].ToString();
    //          PoliticiansHTMLTable += "</td>";


    //          PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //          PoliticiansHTMLTable += StateRow["CityStateZip"].ToString();
    //          PoliticiansHTMLTable += "</td>";
    //        }
    //        else
    //        {
    //          PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //          PoliticiansHTMLTable += StateRow["CityStateZip"].ToString();
    //          PoliticiansHTMLTable += "</td>";

    //          PoliticiansHTMLTable += "<td align=left class=tdReportDetail>";
    //          PoliticiansHTMLTable += "-";
    //          PoliticiansHTMLTable += "</td>";
    //        }


    //        PoliticiansHTMLTable += "</tr>";

    //        StatesCount++;
    //        #endregion
    //      }
    //    }

    //    #region Finish Page Report
    //    PoliticiansHTMLTable += "</table>";
    //    LabelCandidateDataTable.Text = PoliticiansHTMLTable;
    //    #endregion

    //    #endregion

    //    Msg.Text = db.Msg("File: Master/StateAuthorities.txt has "
    //     + StatesCount.ToString() + " Addresses.");
    //  }

    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);

    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }

    //}

    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    if (!SecurePage.IsMasterUser)
    //      SecurePage.HandleSecurityException();

    //    try
    //    {
    //      #region ViewState["File"] ViewState["ElectionKey"]
    //      if (!string.IsNullOrEmpty(QueryElection))
    //        ViewState["ElectionKey"] = QueryElection;
    //      else
    //        ViewState["ElectionKey"] = string.Empty;

    //      if (!string.IsNullOrEmpty(GetQueryString("File")))
    //        ViewState["File"] = GetQueryString("File");
    //      else
    //        ViewState["File"] = string.Empty;
    //      #endregion

    //      if (ViewState["ElectionKey"].ToString() != string.Empty)
    //      {
    //        TableCandidates.Visible = true;
    //        TablePoliticalParties.Visible = false;
    //        TableStateAuthorities.Visible = false;
    //      }
    //      else if (ViewState["File"].ToString() == "Parties")
    //      {
    //        TableCandidates.Visible = false;
    //        TablePoliticalParties.Visible = true;
    //        TableStateAuthorities.Visible = false;
    //      }
    //      else if (ViewState["File"].ToString() == "States")
    //      {
    //        TableCandidates.Visible = false;
    //        TablePoliticalParties.Visible = false;
    //        TableStateAuthorities.Visible = true;
    //      }
    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}



  }
}
