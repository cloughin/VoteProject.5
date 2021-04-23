using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Vote.LDSNewPoliticiansAdd
{
  public partial class LDSNewPoliticiansAdd : VotePage
  {
//    protected void ShowPoliticians(DataTable SameLastNameDataTable)
//    {
//      string PoliticiansHTMLTable = string.Empty;
//      PoliticiansHTMLTable += "<table cellspacing=0 cellpadding=0>";

//      #region Heading
//      PoliticiansHTMLTable += "<tr>";

//      PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
//      PoliticiansHTMLTable += "Politician";
//      PoliticiansHTMLTable += "</td>";

//      PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
//      PoliticiansHTMLTable += "Office";
//      PoliticiansHTMLTable += "</td>";

//      PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
//      PoliticiansHTMLTable += "State Information";
//      PoliticiansHTMLTable += "</td>";

//      PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
//      PoliticiansHTMLTable += "Politician Information";
//      PoliticiansHTMLTable += "</td>";

//      PoliticiansHTMLTable += "<td align=center class=tdReportGroupHeading>";
//      PoliticiansHTMLTable += "Other";
//      PoliticiansHTMLTable += "</td>";

//      PoliticiansHTMLTable += "</tr>";
//      #endregion

//      if (SameLastNameDataTable.Rows.Count > 0)
//      {
//        foreach (DataRow SameLastNameRow in SameLastNameDataTable.Rows)
//        {
//          #region Politicians with same last name

//          #region Name Party LDSNumber PoliticianKey
//          PoliticiansHTMLTable += "<tr>";

//          PoliticiansHTMLTable += "<td valign=top class=tdReportDetail>";
//          PoliticiansHTMLTable += db.Anchor_Master_LDSPolitician(
//            SameLastNameRow["PoliticianKey"].ToString()
//            //,SameLastNameRow["PoliticianName"].ToString()
//            , db.GetPoliticianName(SameLastNameRow["PoliticianKey"].ToString())
//            );

//          if (SameLastNameRow["PartyKey"].ToString() != string.Empty)
//            PoliticiansHTMLTable += " (" + db.Parties_Str(SameLastNameRow["PartyKey"].ToString(), "PartyCode") + ")";
//          //LDSLegIDNum
//          if (SameLastNameRow["LDSLegIDNum"].ToString().Trim() != string.Empty)
//            PoliticiansHTMLTable += "<br>LDS Id:" + SameLastNameRow["LDSLegIDNum"].ToString();
//          //PoliticianKey
//          PoliticiansHTMLTable += "<br>[" + SameLastNameRow["PoliticianKey"].ToString() + "]";
//          PoliticiansHTMLTable += "</td>";
//          #endregion

//          #region Office
//          PoliticiansHTMLTable += "<td valign=top class=tdReportDetail>";
//          //					PoliticiansHTMLTable += SameLastNameRow["OfficeLine1"].ToString();
//          //					PoliticiansHTMLTable += " " + SameLastNameRow["OfficeLine2"].ToString();
//          if (db.Offices_Str_Optional(SameLastNameRow["OfficeKey"].ToString(), "OfficeLine1") != string.Empty)
//          {
//            PoliticiansHTMLTable += db.Offices_Str_Optional(SameLastNameRow["OfficeKey"].ToString(), "OfficeLine1");
//            PoliticiansHTMLTable += " " + db.Offices_Str_Optional(SameLastNameRow["OfficeKey"].ToString(), "OfficeLine2");
//          }
//          else
//            PoliticiansHTMLTable += "No Office Identified for Politician";

//          //					PoliticiansHTMLTable += " " + SameLastNameRow["OfficeLine2"].ToString();
//          PoliticiansHTMLTable += "</td>";
//          #endregion

//          #region State Data
//          PoliticiansHTMLTable += "<td valign=top class=tdReportDetail>";
//          if (SameLastNameRow["StateAddress"].ToString() != string.Empty)
//          {
//            PoliticiansHTMLTable += SameLastNameRow["StateAddress"].ToString() + "<br>";
//            PoliticiansHTMLTable += SameLastNameRow["StateCityStateZip"].ToString() + "<br>";
//          }
//          else
//            PoliticiansHTMLTable += "&nbsp;";

//          if (SameLastNameRow["StatePhone"].ToString() != string.Empty)
//            PoliticiansHTMLTable += SameLastNameRow["StatePhone"].ToString() + "<br>";
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          if (SameLastNameRow["StateEmailAddr"].ToString() != string.Empty)
//            PoliticiansHTMLTable += SameLastNameRow["StateEmailAddr"].ToString() + "<br>";
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          if (SameLastNameRow["StateWebAddr"].ToString() != string.Empty)
//            PoliticiansHTMLTable += SameLastNameRow["StateWebAddr"].ToString() + "<br>";
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          PoliticiansHTMLTable += "</td>";
//          #endregion

//          #region Politician Data
//          PoliticiansHTMLTable += "<td valign=top class=tdReportDetail>";
//          if (SameLastNameRow["Address"].ToString() != string.Empty)
//          {
//            PoliticiansHTMLTable += SameLastNameRow["Address"].ToString() + "<br>";
//            PoliticiansHTMLTable += SameLastNameRow["CityStateZip"].ToString() + "<br>";
//          }
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          if (SameLastNameRow["Phone"].ToString() != string.Empty)
//            PoliticiansHTMLTable += SameLastNameRow["Phone"].ToString() + "<br>";
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          if (SameLastNameRow["EmailAddr"].ToString() != string.Empty)
//            PoliticiansHTMLTable += SameLastNameRow["EmailAddr"].ToString() + "<br>";
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          if (SameLastNameRow["WebAddr"].ToString() != string.Empty)
//            PoliticiansHTMLTable += SameLastNameRow["WebAddr"].ToString() + "<br>";
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          PoliticiansHTMLTable += "</td>";
//          #endregion

//          #region LDS Data
//          PoliticiansHTMLTable += "<td valign=top class=tdReportDetail>";
//          if (SameLastNameRow["LDSAddress"].ToString() != string.Empty)
//          {
//            PoliticiansHTMLTable += SameLastNameRow["LDSAddress"].ToString() + "<br>";
//            PoliticiansHTMLTable += SameLastNameRow["LDSCityStateZip"].ToString() + "<br>";
//          }
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          if (SameLastNameRow["LDSPhone"].ToString() != string.Empty)
//            PoliticiansHTMLTable += SameLastNameRow["LDSPhone"].ToString() + "<br>";
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          if (SameLastNameRow["LDSEmailAddr"].ToString() != string.Empty)
//            PoliticiansHTMLTable += SameLastNameRow["LDSEmailAddr"].ToString() + "<br>";
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          if (SameLastNameRow["LDSWebAddr"].ToString() != string.Empty)
//            PoliticiansHTMLTable += SameLastNameRow["LDSWebAddr"].ToString() + "<br>";
//          else
//            PoliticiansHTMLTable += "&nbsp;";
//          PoliticiansHTMLTable += "</td>";
//          #endregion

//          PoliticiansHTMLTable += "</tr>";
//          #endregion
//        }
//      }
//      else
//      {
//        #region No politicians in the State have the same last name
//        PoliticiansHTMLTable += "<tr>";
//        PoliticiansHTMLTable += "<td colspan=5>";
//        PoliticiansHTMLTable += "<span class=tdReportDetail>";
//        PoliticiansHTMLTable += "No politicians in the database have the same last name.";
//        PoliticiansHTMLTable += "</span>";
//        PoliticiansHTMLTable += "</td>";
//        PoliticiansHTMLTable += "</tr>";
//        #endregion
//      }
//      PoliticiansHTMLTable += "</table>";
//      SameLastNameHTMLTable.Text = PoliticiansHTMLTable;
//    }
//    protected void ShowNextLDSPolitician()
//    {
//      if (Convert.ToUInt16(Session["RowsInNonProcessedTable"]) > Convert.ToUInt16(Session["CurrentRow"]))
//      {
//        #region Fill LDS Politician Row
//        DataTable LEGIDTable = (DataTable)Session["LEGIDTable"];
//        DataRow LDSPoliticianRow = LEGIDTable.Rows[Convert.ToUInt16(Session["CurrentRow"])];
//        Party.Text = LDSPoliticianRow["PARTY"].ToString();
//        First.Text = LDSPoliticianRow["FIRST_NAME"].ToString();
//        Middle.Text = LDSPoliticianRow["MID_NAME"].ToString();
//        Last.Text = LDSPoliticianRow["LAST_NAME"].ToString();
//        Suffix.Text = LDSPoliticianRow["SUFFIX"].ToString();
//        State.Text = db.State4LDSStateCode(LDSPoliticianRow["STATE"].ToString());
//        Office.Text = db.OfficeName4LDSType(LDSPoliticianRow["TYPE"].ToString());
//        District.Text = LDSPoliticianRow["DISTRICT"].ToString();
//        LDSId.Text = LDSPoliticianRow["LEG_ID_NUM"].ToString();
//        DistrictAddress.Text = LDSPoliticianRow["DIST_ADD1"].ToString()
//          + " " + LDSPoliticianRow["DIST_ADD2"].ToString();
//        DistrictCityStateZip.Text = LDSPoliticianRow["DIST_CITY"].ToString()
//          + " " + LDSPoliticianRow["DIST_STATE"].ToString()
//          + " " + db.Zip5(LDSPoliticianRow["DIST_ZIP"].ToString());
//        DistrictPhone.Text = db.EditPhone(LDSPoliticianRow["DIST_PHONE"].ToString());
//        CapitalAddress.Text = LDSPoliticianRow["CAP_ADD1"].ToString()
//          + " " + LDSPoliticianRow["CAP_ADD2"].ToString();
//        CapitalCityStateZip.Text = LDSPoliticianRow["CAP_CITY"].ToString()
//          + " " + LDSPoliticianRow["CAP_STATE"].ToString()
//          + " " + db.Zip5(LDSPoliticianRow["CAP_ZIP"].ToString());
//        CapitalPhone.Text = db.EditPhone(LDSPoliticianRow["CAP_PHONE"].ToString());
//        Email.Text = LDSPoliticianRow["E_MAIL_ADD"].ToString();
//        Web.Text = LDSPoliticianRow["W_WIDE_WEB"].ToString();

//        #endregion

//        #region Politicians with same last name
//        //DataTable SameLastNameTable = db.Table(sql.SameLastName4State(
//        //  db.StateCode4LDSStateCode(LDSPoliticianRow["STATE"].ToString())
//        //  //				,db.Str_Remove_SpecialChars_All_And_Spaces(LDSPoliticianRow["LAST_NAME"].ToString())));
//        //  , LDSPoliticianRow["LAST_NAME"].ToString()));
//        DataTable SameLastNameTable = db.Table(sql.SameLastName4State(
//          //db.StateCode4OfficeKey(ViewState["OfficeKey"].ToString())
//           db.StateCode4LDSStateCode(LDSPoliticianRow["STATE"].ToString())
//           , LDSPoliticianRow["LAST_NAME"].ToString()
//           , db.Str_Remove_Non_Key_Chars(LDSPoliticianRow["LAST_NAME"].ToString())));
//        ShowPoliticians(SameLastNameTable);
//        #endregion
//      }
//      else
//      {
//        db.Master_Update_Date("LDSDateCompletedPoliticiansAdded", DateTime.Now);

//        db.ExecuteSQL("TRUNCATE TABLE LEGIDYYNotProcessed");
//        Msg.Text += db.Ok("<br><br>ALL New LDS POLITICIANS have been processed in LEGIDYYNotProcessed and the Table has been truncated."
//          + " Use the Back button to complete the final two LDS data update steps.");
//      }
//    }

//    protected void ButtonAddPolitician_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Throw_Exception_TextBox_Html_Or_Script(TextBoxFirst);

//        DataTable LEGIDTable = (DataTable)Session["LEGIDTable"];
//        DataRow LEGIDYYRow = LEGIDTable.Rows[Convert.ToUInt16(Session["CurrentRow"])];

//        #region NewPoliticianKey
//        string NewPoliticianKey = string.Empty;
//        if (
//          (TextBoxFirst.Text.Trim() == string.Empty)
//          && (TextBoxMiddle.Text.Trim() == string.Empty)
//          )
//          //Normal case of a unique key
//          #region Make a unique PoliticianKey - returns string.Empty if key was not unique
//          NewPoliticianKey = db.PoliticianKey4LEGIDYYRow(LEGIDYYRow);
//          #endregion
//        else
//        {
//          #region duplicate key so modify using First name Text Box
//          NewPoliticianKey = db.Politician_Key(
//            db.StateCode4LDSStateCode(LEGIDYYRow["STATE"].ToString().Trim())
//            , db.Str_Remove_Non_Key_Chars(LEGIDYYRow["LAST_NAME"].ToString().Trim().Replace(".", string.Empty))
//            , db.Str_Remove_Non_Key_Chars(TextBoxFirst.Text.Trim().Replace(".", string.Empty))
//            //, LEGIDYYRow["MID_NAME"].ToString().Trim().Replace(".", string.Empty)
//            , db.Str_Remove_Non_Key_Chars(TextBoxMiddle.Text.Trim().Replace(".", string.Empty))
//            , db.Str_Remove_Non_Key_Chars(LEGIDYYRow["SUFFIX"].ToString().Trim()).Replace(".", string.Empty));
//          if (db.Is_Valid_Politician(NewPoliticianKey))//NewPoliticianKey not unique
//            NewPoliticianKey = string.Empty;
//          #endregion
//        }
//        #endregion

//        if (NewPoliticianKey == string.Empty)
//        {
//          #region Report a Duplicate PoliticianKey
//          Done.Text = Convert.ToUInt16(Session["CurrentRow"]).ToString();
//          ToDo.Text = Convert.ToUInt16(Session["RowsInNonProcessedTable"]).ToString();
//          Msg.Text = db.Fail("This name creates a duplicate PoliticianKey: "
//            + db.PoliticianNameLDS(LEGIDYYRow)
//            + ". Change the First Name to make the PoliticianKey unique."
//            + "</span>");
//          #endregion
//        }
//        else
//        {
//          #region Unique PoliticianKey so add the new politician

//          if (!db.Is_Valid_Office(db.OfficeKey4LDS(LEGIDYYRow)))
//          {
//            throw new ApplicationException(
//              "Office Row does not exist for OfficeKey: "
//              + db.OfficeKey4LDS(LEGIDYYRow));
//          }

//          #region Insert Politician
//          db.Politician_Insert(
//             NewPoliticianKey
//            , db.OfficeKey4LDS(LEGIDYYRow)
//            , db.StateCode4LDSStateCode(LEGIDYYRow["STATE"].ToString().Trim())
//            , LEGIDYYRow["FIRST_NAME"].ToString().Trim()
//            , LEGIDYYRow["MID_NAME"].ToString().Trim()
//            , LEGIDYYRow["LAST_NAME"].ToString().Trim()
//            , LEGIDYYRow["SUFFIX"].ToString().Trim()
//            , string.Empty
//            , string.Empty
//            , db.PartyKey(
//                db.StateCode4LDSStateCode(LEGIDYYRow["STATE"].ToString().Trim())
//                ,LEGIDYYRow["PARTY"].ToString().Trim()
//                )
//            );

//          db.Politicians_Update_LDSStateCode(NewPoliticianKey
//            , LEGIDYYRow["STATE"].ToString().Trim());
//          db.Politicians_Update_LDSTypeCode(NewPoliticianKey
//            , LEGIDYYRow["TYPE"].ToString().Trim());
//          db.Politicians_Update_LDSDistrictCode(NewPoliticianKey
//            , LEGIDYYRow["DISTRICT"].ToString().Trim());
//          db.Politicians_Update_LDSLegIDNum(NewPoliticianKey
//            , LEGIDYYRow["LEG_ID_NUM"].ToString().Trim());
//          db.Politicians_Update_LDSPoliticianName(NewPoliticianKey
//            , db.PoliticianNameLDS(LEGIDYYRow));
//          db.Politicians_Update_LDSEmailAddr(NewPoliticianKey
//            , db.Str_Remove_MailTo(LEGIDYYRow["E_MAIL_ADD"].ToString().Trim()));
//          db.Politicians_Update_LDSWebAddr(NewPoliticianKey
//            , db.Str_Remove_Http(LEGIDYYRow["W_WIDE_WEB"].ToString().Trim()));
//          db.Politicians_Update_LDSPhone(NewPoliticianKey
//            , db.PhoneLDS(LEGIDYYRow));
//          db.Politicians_Update_LDSGender(NewPoliticianKey
//            , LEGIDYYRow["GENDER"].ToString().Trim());
//          db.Politicians_Update_LDSAddress(NewPoliticianKey
//            , db.AddressLDS(LEGIDYYRow));
//          db.Politicians_Update_LDSCityStateZip(NewPoliticianKey
//            , db.CityStateZipLDS(LEGIDYYRow));
//          db.Politicians_Update_LDSVersion(NewPoliticianKey
//            , db.Master_Str("LDSVersion"));
//          db.Politicians_Update_LDSUpdateDate(NewPoliticianKey
//            , db.Master_Str("LDSUpdateDate"));
//          #endregion Insert Politician

//          #region Increment to Next LDS Politician
//          Session["CurrentRow"] = Convert.ToUInt16(Session["CurrentRow"]) + 1;
//          Done.Text = Convert.ToUInt16(Session["CurrentRow"]).ToString();
//          ToDo.Text = Convert.ToUInt16(Session["RowsInNonProcessedTable"]).ToString();
//          #endregion

//          TextBoxFirst.Text = string.Empty;
//          TextBoxMiddle.Text = string.Empty;

//          Msg.Text = db.Ok(db.PoliticianNameLDS(LEGIDYYRow)
//            + " SUCCESSFULLY ADDED! "
//            + "[" + NewPoliticianKey + "] to [" + db.OfficeKey4LDS(LEGIDYYRow) + "]");

//          ShowNextLDSPolitician();//the next politician row

//          #region OfficesOfficials commented out - done in next step
//#if false
//          #region Insert into OfficesOfficials
//            string ErrMsg = string.Empty;
//            if (!db.Is_DC_ID_WS_Special_Processing(LEGIDYYRow["STATE"].ToString()
//              , LEGIDYYRow["TYPE"].ToString()
//              , LEGIDYYRow["DISTRICT"].ToString()))
//            {
//              ErrMsg = db.OfficesOfficialsUpdate4Politician(
//               LEGIDYYRow
//               , NewPoliticianKey
//               , db.Master_Str("LDSVersion")
//               , db.Master_Date("LDSUpdateDate"));
//            }
//            else
//            {
//          #region Special Offices for DC StateSenate, ID StateHouse, and WA StateHouse
//              if ((LEGIDYYRow["STATE"].ToString() == "11") && (LEGIDYYRow["TYPE"].ToString() == "3"))
//              {
//          #region DC ANCs - only add to OfficeOfficials because there can be any number in a ANC
//                string OfficeKey = db.DCOfficeKey4LDS(LEGIDYYRow["STATE"].ToString()
//                  , LEGIDYYRow["TYPE"].ToString()
//                  , LEGIDYYRow["DISTRICT"].ToString());
//                db.InsertOfficesOfficialsRow(LEGIDYYRow, OfficeKey, db.Master_Str("LDSVersion"), db.Master_Date("LDSUpdateDate"));
//                #endregion
//              }
//              if ((LEGIDYYRow["STATE"].ToString() == "16") && (LEGIDYYRow["TYPE"].ToString() == "3"))
//              {
//          #region ID StateHouse has a seperate A and B office for the same LDS DISTRICT like: IDStateHouse1A, IDStateHouse1B, IDStateHouse2A,IDStateHouse2B
//                //Only add to the 'A" Office because there is no way of knowing whether the incumbent was elected to the A or B
//                string OfficeKey = db.IDOfficeKey4LDS(LEGIDYYRow["STATE"].ToString()
//                  , LEGIDYYRow["TYPE"].ToString()
//                  , LEGIDYYRow["DISTRICT"].ToString());
//                db.InsertOfficesOfficialsRow(LEGIDYYRow, OfficeKey, db.Master_Str("LDSVersion"), db.Master_Date("LDSUpdateDate"));
//                #endregion
//              }
//              if ((LEGIDYYRow["STATE"].ToString() == "53") && (LEGIDYYRow["TYPE"].ToString() == "3"))
//              {
//          #region WA StateHouse has a seperate P1 and P2 office for the same LDS DISTRICT like: WAStateHouse1P1,WAStateHouse1P2,WAStateHouse2P1,WAStateHouse2P2
//                //Only add to the 'P1' Office because there is no way of knowing whether the incumbent was elected to the P1 or P2
//                string OfficeKey = db.WAOfficeKey4LDS(LEGIDYYRow["STATE"].ToString()
//                  , LEGIDYYRow["TYPE"].ToString()
//                  , LEGIDYYRow["DISTRICT"].ToString());
//                db.InsertOfficesOfficialsRow(LEGIDYYRow, OfficeKey, db.Master_Str("LDSVersion"), db.Master_Date("LDSUpdateDate"));
//                #endregion
//              }
//              #endregion
//            }
//          #endregion

//            if (ErrMsg == string.Empty)
//            {
//          #region OfficesOfficials Update ok
//              TextBoxFirst.Text = string.Empty;

//          #region Increment to Next LDS Politician
//              Session["CurrentRow"] = Convert.ToUInt16(Session["CurrentRow"]) + 1;
//              Done.Text = Convert.ToUInt16(Session["CurrentRow"]).ToString();
//              ToDo.Text = Convert.ToUInt16(Session["RowsInNonProcessedTable"]).ToString();
//              #endregion

//              Msg.Text = db.Ok(db.PoliticianNameLDS(LEGIDYYRow)
//                + " SUCCESSFULLY ADDED! "
//                + "[" + NewPoliticianKey + "]");

//              ShowNextLDSPolitician();//the next politician row

//              #endregion
//            }
//            else
//            {
//              Msg.Text = db.Fail(ErrMsg);
//            }
//#endif
//          #endregion OfficesOfficials commented out - done in next step
//          #endregion
//        }
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }

//    protected void ButtonSkipPolitician_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Throw_Exception_TextBox_Html_Or_Script(TextBoxFirst);
//        //if (!IsEndOfLEGIDYYTable())
//        //{
//        TextBoxFirst.Text = string.Empty;

//        #region Increment to Next LDS Politician
//        Session["CurrentRow"] = Convert.ToUInt16(Session["CurrentRow"]) + 1;
//        Done.Text = Convert.ToUInt16(Session["CurrentRow"]).ToString();
//        ToDo.Text = Convert.ToUInt16(Session["RowsInNonProcessedTable"]).ToString();
//        #endregion

//        Msg.Text = db.Msg("Politician WAS SKIPPED! ");

//        ShowNextLDSPolitician();//the next politician row
//        //}
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }

//    protected void Page_Load(object sender, EventArgs e)
//    {
//      //if (db.User() != db.User_.Master)
//      //  Response.Redirect(UrlManager.CanonicalSiteUri.ToString());
//      if (!SecurePage.IsMasterUser)
//        SecurePage.HandleSecurityException();

//      if (!IsPostBack)
//      {
//        try
//        {
//          #region PoliticianKey
//          if (!string.IsNullOrEmpty(db.QueryString("Id")))
//            ViewState["PoliticianKey"] = db.QueryString("Id").Trim();
//          else
//            ViewState["PoliticianKey"] = string.Empty;
//          #endregion

//          if ((ViewState["PoliticianKey"] as string) == string.Empty)
//          {
//            #region First Row Only

//            #region Create LEGIDYYNotProcessed Table and Rows in Table, set row pointer
//            string SQL = "SELECT * FROM LEGIDYYNotProcessed";
//            Session["LEGIDTable"] = db.Table(SQL);
//            DataTable LEGIDTable = (DataTable)Session["LEGIDTable"];
//            #endregion Create LEGIDYYNotProcessed Table and Rows in Table, set row pointer

//            if (LEGIDTable.Rows.Count == 0)
//              Msg.Text = db.Msg("The LEGIDYYNotProcessed Table is empty. It has probably been processed already.");
//            else
//            {
//              LDSVersion.Text = db.Master_Str("LDSVersion");
//              Session["CurrentRow"] = 0;
//              Done.Text = Convert.ToUInt16(Session["CurrentRow"]).ToString();
//              Session["RowsInNonProcessedTable"] = LEGIDTable.Rows.Count;
//              ToDo.Text = Convert.ToUInt16(Session["RowsInNonProcessedTable"]).ToString();

//              ShowNextLDSPolitician();
//            }
//            #endregion
//          }
//          else
//          {
//            //if (!IsEndOfLEGIDYYTable())
//            //{
//            #region Subsequent Rows when Politician Clicked in 'Same Last Name' Report

//            #region Update Politician - The Politician Clicked in 'Politicians with Same Last Name' Report
//            DataTable LEGIDTable = (DataTable)Session["LEGIDTable"];
//            Session["RowsInNonProcessedTable"] = LEGIDTable.Rows.Count;
//            DataRow LEGIDYYRow = LEGIDTable.Rows[Convert.ToUInt16(Session["CurrentRow"])];

//            #region Update Politician's LEG_ID_NUM and OfficeKey
//            //db.Politicians_Update_Str(ViewState["PoliticianKey"].ToString(), "LDSLegIDNum", LEGIDYYRow["LEG_ID_NUM"].ToString());
//            //db.Politicians_Update_Str(ViewState["PoliticianKey"].ToString(), "LDSVersion", db.Master_Str("LDSVersion"));
//            //db.Politicians_Update_Date(ViewState["PoliticianKey"].ToString(), "LDSUpdateDate", db.Master_Date("LDSUpdateDate"));
//            //db.Politicians_Update_Str(ViewState["PoliticianKey"].ToString(), "OfficeKey", db.OfficeKey4LDS(LEGIDYYRow));

//            //db.xLDS_Data_Politicians_Update(
//            //  LEGIDYYRow
//            //  , ViewState["PoliticianKey"].ToString()
//            //  , db.Master_Str("LDSVersion")
//            //  , db.Master_Date("LDSUpdateDate"));
//            #endregion

//            #region Insert into OfficesOfficials (commented out)
//#if false
//            string ErrMsg = db.OfficesOfficialsUpdate4Politician(
//                LEGIDYYRow
//                , ViewState["PoliticianKey"].ToString()
//                , db.Master_Str("LDSVersion")
//                , db.Master_Date("LDSUpdateDate"));
//#endif            
//            #endregion

//            #endregion

//            #region Increment to Next LDS Politician
//            Session["CurrentRow"] = Convert.ToUInt16(Session["CurrentRow"]) + 1;
//            Done.Text = Convert.ToUInt16(Session["CurrentRow"]).ToString();
//            ToDo.Text = Convert.ToUInt16(Session["RowsInNonProcessedTable"]).ToString();
//            #endregion

//            Msg.Text = db.Ok(db.PoliticianNameLDS(LEGIDYYRow)
//              + " SUCCESSFULLY UPDATED! "
//              + "[" + ViewState["PoliticianKey"].ToString() + "]");

//            ShowNextLDSPolitician();

//            #endregion
//            //}
//          }
//        }
//        catch (Exception ex)
//        {
//          #region
//          Msg.Text = db.Fail(ex.Message);
//          db.Log_Error_Admin(ex);
//          #endregion
//        }
//      }
//    }

  }
}
