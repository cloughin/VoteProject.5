using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DB.Vote;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class ElectionOffices1 : VotePage
  {
//    private int Offices_In_Group(DataRow Office_Row)
//    {
//      string SQL = string.Empty;
//      SQL += "Offices";
//      SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//      SQL += " AND CountyCode = " + db.SQLLit(ViewState["CountyCode"].ToString());
//      SQL += " AND LocalCode = " + db.SQLLit(ViewState["LocalCode"].ToString());
//      SQL += " AND OfficeLevel = " + Office_Row["OfficeLevel"].ToString();
//      //SQL += " AND IsOfficeTagForDeletion = 0";
//      SQL += " AND IsSpecialOffice = 0";
//      return db.Rows_Count_From(SQL);
//    }
//    private string Msg_Electoral()
//    {
//      switch (db.Electoral_Class(
//         ViewState["StateCode"].ToString()
//        , ViewState["CountyCode"].ToString()
//        , ViewState["LocalCode"].ToString()))
//      {
//        case db.ElectoralClass.State:
//          return " "
//            + Offices.GetElectoralClassDescription(
//              ViewState["StateCode"].ToString()
//              );
//        case db.ElectoralClass.County:
//          return " "
//            + Offices.GetElectoralClassDescription(
//              ViewState["StateCode"].ToString()
//              , ViewState["CountyCode"].ToString()
//              );
//        case db.ElectoralClass.Local:
//          return " "
//            + Offices.GetElectoralClassDescription(
//              ViewState["StateCode"].ToString()
//              , ViewState["CountyCode"].ToString()
//              , ViewState["LocalCode"].ToString()
//              );
//        default:
//          return string.Empty;
//      }
//    }

//    private void Visible_Controls()
//    {
//      #region Nothing Visible
//      TableOfficeGroups.Visible = false;
//      TableAddOffice.Visible = false;
//      TableCountyLinks.Visible = false;
//      TableLocalLinks.Visible = false;
//      #endregion Nothing Visible

//      #region TableCountyLinks, TableLocalLinks
//      switch (db.Electoral_Class(
//             ViewState["StateCode"].ToString()
//            , ViewState["CountyCode"].ToString()
//            , ViewState["LocalCode"].ToString()
//       ))
//      {
//        case db.ElectoralClass.USPresident:
//          break;
//        case db.ElectoralClass.USSenate:
//          break;
//        case db.ElectoralClass.USHouse:
//          break;
//        case db.ElectoralClass.USGovernors:
//          break;
//        case db.ElectoralClass.State:
//          #region State Offices beign selected
//          TableOfficeGroups.Visible = true;
//          TableAddOffice.Visible = true;

//          TableCountyLinks.Visible = true;
//          //LabelCounties.Text = db.County_Links(
//          //  db.Anchor_For.Admin_Election_Offices
//          //  , db.ElectionKey_State(
//          //      ViewState["ElectionKey"].ToString())
//          //  );
//          LabelCounties.Text = 
//            CountyLinks.GetElectionOfficesCountyLinks(
//              ViewState["ElectionKey"].ToString()).RenderToString();
//          #endregion State Offices beign selected
//          break;
//        case db.ElectoralClass.County:
//          TableOfficeGroups.Visible = true;
//          TableAddOffice.Visible = true;

//          #region County Offices beign selected
//          if (
//            (SecurePage.IsMasterUser)
//            || (SecurePage.IsStateAdminUser)
//            )
//          {
//            //Any user who can administer State
//            //has other county links available
//            TableCountyLinks.Visible = true;
//            //LabelCounties.Text = db.County_Links(
//            //  db.Anchor_For.Admin_Election_Offices
//            //  , db.ElectionKey_County(
//            //    ViewState["ElectionKey"].ToString()
//            //    , ViewState["StateCode"].ToString()
//            //    , ViewState["CountyCode"].ToString()
//            //    )
//            //  );
//            LabelCounties.Text =
//             CountyLinks.GetElectionOfficesCountyLinks(
//               ViewState["ElectionKey"].ToString()).RenderToString();
//          }
//          //Any user in a county
//          //has the local links for the county visible
//          TableLocalLinks.Visible = true;
//          //LabelLocalDistricts.Text = db.Local_Links(
//          //    db.Sort_Order.Name
//          //    , db.Anchor_For.Admin_Election_Offices
//          //    , ViewState["StateCode"].ToString()
//          //    , ViewState["CountyCode"].ToString()
//          //    , ViewState["ElectionKey"].ToString()
//          //    );
//          LabelLocalDistricts.Text =
//            LocalLinks.GetElectionOfficesLocalLinks(
//              ViewState["ElectionKey"].ToString(),
//              ViewState["StateCode"].ToString(),
//              ViewState["CountyCode"].ToString()).RenderToString();
//          #endregion County Offices beign selected
//          break;
//        case db.ElectoralClass.Local:
//          TableOfficeGroups.Visible = true;
//          TableAddOffice.Visible = true;

//          #region Local Offices beign selected
//          if (
//            (SecurePage.IsMasterUser)
//            || (SecurePage.IsStateAdminUser)
//            )
//          {
//            //Any user who can administer State
//            //has other county links available
//            //and other local district links in the current county
//            #region County and Local links
//            TableCountyLinks.Visible = true;
//            //LabelCounties.Text = db.County_Links(
//            //  //"Name"//Sort order
//            //  db.Anchor_For.Admin_Election_Offices
//            //  , db.ElectionKey_State(
//            //      ViewState["ElectionKey"].ToString()
//            //      )
//            //  );
//            LabelCounties.Text =
//             CountyLinks.GetElectionOfficesCountyLinks(
//               ViewState["ElectionKey"].ToString()).RenderToString();

//            TableLocalLinks.Visible = true;
//            //LabelLocalDistricts.Text = db.Local_Links(
//            //  db.Sort_Order.Name
//            //  , db.Anchor_For.Admin_Election_Offices
//            //  , ViewState["StateCode"].ToString()
//            //  , ViewState["CountyCode"].ToString()
//            //  , ViewState["ElectionKey"].ToString()
//            //  );
//            LabelLocalDistricts.Text =
//             LocalLinks.GetElectionOfficesLocalLinks(
//               ViewState["ElectionKey"].ToString(),
//               ViewState["StateCode"].ToString(),
//               ViewState["CountyCode"].ToString()).RenderToString();
//            #endregion County and Local links
//          }
//          else if (SecurePage.IsCountyAdminUser)
//          {
//            //Any user who can administer the County
//            //has the other local district links available
//            #region Just local links
//            TableLocalLinks.Visible = true;
//            //LabelLocalDistricts.Text = db.Local_Links(
//            //  db.Sort_Order.Name
//            //  , db.Anchor_For.Admin_Election_Offices
//            //  , ViewState["StateCode"].ToString()
//            //  , ViewState["CountyCode"].ToString()
//            //  , ViewState["ElectionKey"].ToString()
//            //  );
//            LabelLocalDistricts.Text =
//              LocalLinks.GetElectionOfficesLocalLinks(
//                ViewState["ElectionKey"].ToString(),
//                ViewState["StateCode"].ToString(),
//                ViewState["CountyCode"].ToString()).RenderToString();
//            #endregion Just local links
//          }
//          #endregion Local Offices beign selected
//          break;
//        default:
//          break;
//      }
//      #endregion TableCountyLinks, TableLocalLinks

//    }

//    private void Page_Title()
//    {
//      PageTitle.Text = string.Empty;
//      PageTitle.Text += Offices.GetElectoralClassDescription(
//        ViewState["StateCode"].ToString()
//        , ViewState["CountyCode"].ToString()
//        , ViewState["LocalCode"].ToString()
//        );

//      PageTitle.Text += "<br>";

//      if (db.Is_Valid_Election(ViewState["ElectionKey"].ToString()))
//        PageTitle.Text += db.PageTitle_Election(
//          ViewState["ElectionKey"].ToString());
//      else
//        PageTitle.Text += db.PageTitle_Election(
//          db.ElectionKey_State(ViewState["ElectionKey"].ToString()));

//      PageTitle.Text += "<br>";
//      PageTitle.Text += "OFFICE CONTESTS IN THIS ELECTION";
//    }

//    private void Elections_Delete(string ElectionKey)
//    {
//      string SQL = string.Empty;
//      SQL += "DELETE FROM Elections";
//      SQL += " WHERE ElectionKey = " + db.SQLLit(ElectionKey);
//      db.ExecuteSQL(SQL);
//    }

//    //private void ReportsElections_Delete(string ElectionKey)
//    //{
//    //  string SQL = string.Empty;
//    //  SQL += "DELETE FROM ReportsElections";
//    //  SQL += " WHERE ElectionKey = " + db.SQLLit(ElectionKey);
//    //  db.ExecuteSQL(SQL);
//    //}

//    private bool Is_Has_ElectionsOffices_Rows(string ElectionKey)
//    {
//      string SQL = string.Empty;
//      SQL += " ElectionsOffices";
//      SQL += " WHERE ElectionKey =" + db.SQLLit(ElectionKey);
//      if (db.Rows_Count_From(SQL) > 0)
//        return true;
//      else
//        return false;
//    }
//    private bool Is_Has_ElectionsOffices_Rows_Election_State(string ElectionKey_State)
//    {
//      string SQL = string.Empty;
//      SQL += " ElectionsOffices";
//      SQL += " WHERE ElectionKeyState =" + db.SQLLit(ElectionKey_State);
//      if (db.Rows_Count_From(SQL) > 0)
//        return true;
//      else
//        return false;
//    }
//    private bool Is_Has_ElectionsOffices_Rows_Election_County(string ElectionKey_County)
//    {
//      string SQL = string.Empty;
//      SQL += " ElectionsOffices";
//      SQL += " WHERE ElectionKeyCounty =" + db.SQLLit(ElectionKey_County);
//      if (db.Rows_Count_From(SQL) > 0)
//        return true;
//      else
//        return false;
//    }
//    private bool Is_Has_ElectionsOffices_Rows_Election_Local(string ElectionKey_Local)
//    {
//      string SQL = string.Empty;
//      SQL += " ElectionsOffices";
//      SQL += " WHERE ElectionKeyLocal =" + db.SQLLit(ElectionKey_Local);
//      if (db.Rows_Count_From(SQL) > 0)
//        return true;
//      else
//        return false;
//    }

//    protected int County_Offices()
//    {
//      string SQL = string.Empty;
//      SQL += " Offices";
//      SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//      SQL += " AND CountyCode != ''";
//      SQL += " AND LocalCode = ''";
//      return db.Rows_Count_From(SQL);
//    }
//    protected int Local_Offices()
//    {
//      string SQL = string.Empty;
//      SQL += " Offices";
//      SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//      SQL += " AND LocalCode != ''";
//      return db.Rows_Count_From(SQL);
//    }
//    protected int County_Election_Offices()
//    {
//      string SQL = string.Empty;
//      SQL += " ElectionsOffices";
//      SQL += " WHERE ElectionKeyState = " + db.SQLLit(ViewState["ElectionKey"].ToString());
//      SQL += " AND CountyCode != ''";
//      SQL += " AND LocalCode = ''";
//      return db.Rows_Count_From(SQL);
//    }
//    protected int Local_Election_Offices()
//    {
//      string SQL = string.Empty;
//      SQL += " ElectionsOffices";
//      SQL += " WHERE ElectionKeyState = " + db.SQLLit(ViewState["ElectionKey"].ToString());
//      SQL += " AND LocalCode != ''";
//      return db.Rows_Count_From(SQL);
//    }

//    private void Delete_ElectionsOffices(string OfficeKey)
//    {
//      #region Note - State, County and Local Elections and Election Reports
//      //Federal elections are never deleted 
//      //because they are the standard 2 year General elections.
//      //
//      //State elections can only be deleted at the State or Master level
//      //and not here.
//      //
//      //COUNTY and LOCAL elections are deleted on the fly
//      //when no more offices are in the election for the county or local district.
//      //
//      //But a COUNTY election can not be deleted if there are any
//      //local office contests for any of the local districts.
//      #endregion Note - State, County and Local Elections and Election Reports

//      #region Delete LOCAL ElectionsOffices row
//      //string OfficeKey = ViewState["OfficeKey"
//      //  + Index_Checkbox_Changed_Office.ToString()].ToString();
//      string SQL = string.Empty;
//      SQL += "DELETE FROM ElectionsOffices";
//      SQL += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
//      SQL += " AND OfficeKey = " + db.SQLLit(OfficeKey);
//      db.ExecuteSQL(SQL);
//      #endregion Delete LOCAL ElectionsOffices row

//      #region Delete LOCAL Election and LOCAL Election Report
//      //If a LOCAL office and no more local ElectionsOffices rows 
//      //delete LOCAL Election and LOCAL Election Report.
//      //
//      //If after deleting the local ElectionsOffices rows 
//      //there may be no more county ElectionsOffices rows
//      //So delete COUNTY election and report
//      if (Offices.IsLocalOffice(OfficeKey))
//      {
//        if (!Is_Has_ElectionsOffices_Rows_Election_Local(
//              db.ElectionKey_Local(
//                ViewState["ElectionKey"].ToString()
//                , ViewState["StateCode"].ToString()
//                , ViewState["CountyCode"].ToString()
//                , ViewState["LocalCode"].ToString()
//                )))
//        {
//          Elections_Delete(db.ElectionKey_Local(
//            ViewState["ElectionKey"].ToString()
//            , ViewState["StateCode"].ToString()
//            , ViewState["CountyCode"].ToString()
//            , ViewState["LocalCode"].ToString()
//            ));
//          //ReportsElections_Delete(db.ElectionKey_Local(
//          //  ViewState["ElectionKey"].ToString()
//          //  , ViewState["StateCode"].ToString()
//          //  , ViewState["CountyCode"].ToString()
//          //  , ViewState["LocalCode"].ToString()
//          //  ));
//        }
//      }
//      #endregion Delete LOCAL Election and LOCAL Election Report

//      #region Delete COUNTY Election and COUNTY Election Report
//      //If a COUNTY or LOCAL office and no more COUNTY ElectionsOffices rows 
//      //delete the COUNTY Election and COUNTY Election Report.
//      //
//      //Local elections are just a subset of the COUNTY ElectionsOffices rows 
//      //And when they are all deleted the county stuff should be deleted
//      if (
//        Offices.IsCountyOffice(OfficeKey)
//        || Offices.IsLocalOffice(OfficeKey)
//        )
//      {
//        if (!Is_Has_ElectionsOffices_Rows_Election_County(
//          db.ElectionKey_County(
//          ViewState["ElectionKey"].ToString()
//          , ViewState["StateCode"].ToString()
//          , ViewState["CountyCode"].ToString()
//          )))
//        {
//          Elections_Delete(db.ElectionKey_County(
//            ViewState["ElectionKey"].ToString()
//            , ViewState["StateCode"].ToString()
//            , ViewState["CountyCode"].ToString()
//            ));
//          //ReportsElections_Delete(db.ElectionKey_County(
//          //  ViewState["ElectionKey"].ToString()
//          //  , ViewState["StateCode"].ToString()
//          //  , ViewState["CountyCode"].ToString()
//          //  ));
//        }
//      }
//      #endregion Delete COUNTY Election and COUNTY Election Report
//    }
//    protected void Delete_All_County_Election_Tables_Rows()
//    {
//      string SQL = string.Empty;

//      SQL = string.Empty;
//      SQL += " DELETE";
//      SQL += " FROM ElectionsOffices";
//      SQL += " WHERE ElectionKeyState = " + db.SQLLit(ViewState["ElectionKey"].ToString());
//      SQL += " AND CountyCode != ''";
//      SQL += " AND LocalCode = ''";
//      db.ExecuteSQL(SQL);

//      SQL = string.Empty;
//      SQL += " DELETE";
//      SQL += " FROM Elections";
//      SQL += " WHERE Substring(ElectionKey,1,12) = " + db.SQLLit(ViewState["ElectionKey"].ToString());
//      SQL += " AND CountyCode != ''";
//      SQL += " AND LocalCode = ''";
//      db.ExecuteSQL(SQL);

//      //SQL = string.Empty;
//      //SQL += " DELETE";
//      //SQL += " FROM ReportsElections";
//      //SQL += " WHERE Substring(ElectionKey,1,12) = " + db.SQLLit(ViewState["ElectionKey"].ToString());
//      //SQL += " AND CountyCode != ''";
//      //SQL += " AND LocalCode = ''";
//      //db.ExecuteSQL(SQL);

//    }
//    protected void Delete_All_Local_Election_Tables_Rows()
//    {
//      string SQL = string.Empty;

//      SQL = string.Empty;
//      SQL += " DELETE";
//      SQL += " FROM ElectionsOffices";
//      SQL += " WHERE ElectionKeyState = " + db.SQLLit(ViewState["ElectionKey"].ToString());
//      SQL += " AND LocalCode != ''";
//      db.ExecuteSQL(SQL);

//      SQL = string.Empty;
//      SQL += " DELETE";
//      SQL += " FROM Elections";
//      SQL += " WHERE Substring(ElectionKey,1,12) = " + db.SQLLit(ViewState["ElectionKey"].ToString());
//      SQL += " AND LocalCode != ''";
//      db.ExecuteSQL(SQL);

//      //SQL = string.Empty;
//      //SQL += " DELETE";
//      //SQL += " FROM ReportsElections";
//      //SQL += " WHERE Substring(ElectionKey,1,12) = " + db.SQLLit(ViewState["ElectionKey"].ToString());
//      //SQL += " AND LocalCode != ''";
//      //db.ExecuteSQL(SQL);
//    }
//    protected void Insert_All_County_Election_Tables_Rows()
//    {
//      string SQL = string.Empty;

//      #region ElectionsOffices
//      SQL = string.Empty;
//      SQL += " SELECT";
//      SQL += " OfficeKey,CountyCode,LocalCode,OfficeLevel";
//      SQL += " FROM Offices";
//      SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//      SQL += " AND CountyCode != ''";
//      SQL += " AND LocalCode = ''";
//      DataTable Table_Offices = db.Table(SQL);
//      int LocalOffices = Table_Offices.Rows.Count;
//      foreach (DataRow Row_Office in Table_Offices.Rows)
//      {
//        db.ElectionsOffices_INSERT(
//          db.ElectionKey_County(
//            ViewState["ElectionKey"].ToString()
//            , Row_Office["OfficeKey"].ToString()
//            )
//          , Row_Office["OfficeKey"].ToString()
//          , Offices.GetValidDistrictCode(Row_Office["OfficeKey"].ToString())
//          );
//      }
//      #endregion ElectionsOffices

//      #region Elections
//      DataTable Table_Counties = db.Table(sql.Counties(ViewState["StateCode"].ToString()));
//      foreach (DataRow Row_County in Table_Counties.Rows)
//      {
//        #region County
//        #region Note
//        //If a COUNTY or LOCAL office insert a COUNTY election 
//        //and election report if it does not exist
//        #endregion Note
//        string ElectionKey_County = db.ElectionKey_County(
//          ViewState["ElectionKey"].ToString()
//          , ViewState["StateCode"].ToString()
//          , Row_County["CountyCode"].ToString()
//          );
//        string ElectionKey_State = db.ElectionKey_State(ViewState["ElectionKey"].ToString());

//        if (!db.Is_Valid_Election(ElectionKey_County))
//        {
//          db.Elections_Insert_And_Report_Election_Update(ViewState["StateCode"].ToString(),
//             Row_County["CountyCode"].ToString(),
//            string.Empty,
//            db.Elections_Date(ElectionKey_State, "ElectionDate").ToString(),
//            db.Elections_Str(ElectionKey_State, "ElectionType"),
//            db.Elections_Str(ElectionKey_State, "NationalPartyCode"),
//            db.Elections_Str(ElectionKey_State, "PartyCode"),
//            db.Elections_Str(ElectionKey_State, "ElectionDesc"),
//            db.Elections_Str(ElectionKey_State, "ElectionAdditionalInfo"),
//            db.Elections_Str(ElectionKey_State, "BallotInstructions"));
//        }
//        #endregion County
//      }
//      #endregion Elections
//    }
//    protected void Insert_All_Local_Election_Tables_Rows()
//    {
//      string SQL = string.Empty;

//      #region ElectionsOffices
//      SQL = string.Empty;
//      SQL += " SELECT";
//      SQL += " OfficeKey,CountyCode,LocalCode,OfficeLevel";
//      SQL += " FROM Offices";
//      SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//      SQL += " AND LocalCode != ''";
//      DataTable Table_Offices = db.Table(SQL);
//      int LocalOffices = Table_Offices.Rows.Count;
//      foreach (DataRow Row_Office in Table_Offices.Rows)
//      {
//        db.ElectionsOffices_INSERT(
//          db.ElectionKey_Local(
//            ViewState["ElectionKey"].ToString()
//            , Row_Office["OfficeKey"].ToString()
//            )
//          , Row_Office["OfficeKey"].ToString()
//          , Offices.GetValidDistrictCode(Row_Office["OfficeKey"].ToString())
//          );
//      }
//      #endregion ElectionsOffices

//      #region Elections
//      DataTable Table_Counties = db.Table(sql.Counties(ViewState["StateCode"].ToString()));
//      foreach (DataRow Row_County in Table_Counties.Rows)
//      {
//        DataTable Table_Locals = db.Table(sql.LocalDistricts(
//          ViewState["StateCode"].ToString()
//          , Row_County["CountyCode"].ToString()));
//        foreach (DataRow Row_Local in Table_Locals.Rows)
//        {
//          #region Local
//          #region Note
//          //If a COUNTY or LOCAL office insert a COUNTY election 
//          //and election report if it does not exist
//          #endregion Note
//          string ElectionKey_Local = db.ElectionKey_Local(
//            ViewState["ElectionKey"].ToString()
//            , ViewState["StateCode"].ToString()
//            , Row_County["CountyCode"].ToString()
//            , Row_Local["LocalCode"].ToString()
//            );
//          string ElectionKey_State = db.ElectionKey_State(ViewState["ElectionKey"].ToString());

//          if (!db.Is_Valid_Election(ElectionKey_Local))
//          {
//            db.Elections_Insert_And_Report_Election_Update(ViewState["StateCode"].ToString(),
//              Row_County["CountyCode"].ToString(),
//              Row_Local["LocalCode"].ToString(),
//              db.Elections_Date(ElectionKey_State, "ElectionDate").ToString(),
//              db.Elections_Str(ElectionKey_State, "ElectionType"),
//              db.Elections_Str(ElectionKey_State, "NationalPartyCode"),
//              db.Elections_Str(ElectionKey_State, "PartyCode"),
//              db.Elections_Str(ElectionKey_State, "ElectionDesc"),
//              db.Elections_Str(ElectionKey_State, "ElectionAdditionalInfo"),
//              db.Elections_Str(ElectionKey_State, "BallotInstructions"));
//          }
//          #endregion Local
//        }
//      }
//      #endregion Elections

//    }

//    private void Insert_ElectionsOffices(string officeKey)
//    {
//      var stateCode = ViewState["StateCode"].ToString();
//      var countyCode = ViewState["CountyCode"].ToString();
//      var localCode = ViewState["LocalCode"].ToString();
//      var electionKey = ViewState["ElectionKey"].ToString();

//      //A State Election should always exist
//      //It is created in ElectionCreate.aspx
//      //And only State Elections are created by ElectionCreate.aspx
//      //County and local elections are always created here, on the fly, as needed.
//      //
//      //For every election (State, county or local) a corresponding ReportElection 
//      //row should exist and are created and deleted as elections are created and deleted.

//      CreateFederalElectionIfNeeded(electionKey, officeKey, stateCode, countyCode, localCode);
//      CreateCountyElectionIfNeeded(electionKey, stateCode, countyCode, localCode);
//      CreateLocalElectionIfNeeded(electionKey, stateCode, countyCode, localCode);

//      if (!db.Is_Valid_Election_Office(electionKey, officeKey))
//        db.ElectionsOffices_INSERT(electionKey, officeKey);
//    }

//    private void CreateLocalElectionIfNeeded(string electionKey, string stateCode, 
//      string countyCode, string localCode)
//    {
//      var electionKeyState = db.ElectionKey_State(electionKey);
//      if (db.Electoral_Class(stateCode, countyCode, localCode) ==
//        db.ElectoralClass.Local)
//      {
//        var electionKeyLocal = db.ElectionKey_Local(electionKey, stateCode, countyCode,
//          localCode);
//        if (!db.Is_Valid_Election(electionKeyLocal))
//          db.Elections_Insert_And_Report_Election_Update(stateCode,
//            countyCode, ViewState["LocalCode"].ToString(),
//            db.Elections_Date(electionKeyState, "ElectionDate")
//              .ToString(CultureInfo.InvariantCulture),
//            db.Elections_Str(electionKeyState, "ElectionType"),
//            db.Elections_Str(electionKeyState, "NationalPartyCode"),
//            db.Elections_Str(electionKeyState, "PartyCode"),
//            db.Elections_Str(electionKeyState, "ElectionDesc"),
//            db.Elections_Str(electionKeyState, "ElectionAdditionalInfo"),
//            db.Elections_Str(electionKeyState, "BallotInstructions"));
//      }
//    }

//    private void CreateCountyElectionIfNeeded(string electionKey, string stateCode,
//      string countyCode, string localCode)
//    {
//      var electoralClass = db.Electoral_Class(stateCode, countyCode, localCode);
//      if (electoralClass == db.ElectoralClass.County ||
//        electoralClass == db.ElectoralClass.Local)
//      {
//        var electionKeyState = db.ElectionKey_State(electionKey);
//        var electionKeyCounty = db.ElectionKey_County(electionKey, stateCode,
//          countyCode);
//        if (!db.Is_Valid_Election(electionKeyCounty))
//          db.Elections_Insert_And_Report_Election_Update(stateCode,
//            countyCode, string.Empty,
//            db.Elections_Date(electionKeyState, "ElectionDate")
//              .ToString(CultureInfo.InvariantCulture),
//            db.Elections_Str(electionKeyState, "ElectionType"),
//            db.Elections_Str(electionKeyState, "NationalPartyCode"),
//            db.Elections_Str(electionKeyState, "PartyCode"),
//            db.Elections_Str(electionKeyState, "ElectionDesc"),
//            db.Elections_Str(electionKeyState, "ElectionAdditionalInfo"),
//            db.Elections_Str(electionKeyState, "BallotInstructions"));
//      }
//    }

//    private void CreateFederalElectionIfNeeded(string electionKey, string officeKey, 
//      string stateCode, string countyCode, string localCode)
//    {
//      if (db.Electoral_Class(stateCode, countyCode, localCode) ==
//        db.ElectoralClass.State &&
//        (Offices.IsUSPresident(officeKey) || Offices.IsUSSenate(officeKey) ||
//          Offices.IsUSHouse(officeKey)) && Elections.IsGeneralElection(electionKey))
//      {
//        //If the election is a General 2 year election
//        //The first State election 
//        //should cause the Federal election and report to be created.
//        //
//        //If US President office insert U1 Elecion and report rows if they not exist
//        //If US Senate office insert U2 Elecion and report if they do not exist
//        //If US House office insert U3 Elecion and report if they do not exist
//        var electionKeyFederal = string.Empty;

//        var electionDesc = string.Empty;
//        electionDesc += db.Elections_Date(electionKey, "ElectionDate")
//          .ToString("MMMM d, yyyy");
//        electionDesc += " General Election";

//        if (Offices.IsUSPresident(officeKey))
//        {
//          electionKeyFederal = db.ElectionKey_USPres(electionKey);
//          electionDesc += " U.S. President";
//        }
//        else if (Offices.IsUSSenate(officeKey))
//        {
//          electionKeyFederal = db.ElectionKey_USSenate(electionKey);
//          electionDesc += " U.S. Senate";
//        }
//        else if (Offices.IsUSHouse(officeKey))
//        {
//          electionKeyFederal = db.ElectionKey_USHouse(electionKey);
//          electionDesc += " U.S. House of Representatives";
//        }

//        if (!db.Is_Valid_Election(electionKeyFederal))
//        {
//          electionDesc += " State-By-State";

//          db.Elections_Insert_And_Report_Election_Update(Elections.GetValidatedFederalCodeFromKey(electionKeyFederal),
//            string.Empty, string.Empty,
//            db.Elections_Date(electionKey, "ElectionDate")
//              .ToString(CultureInfo.InvariantCulture), "G", "A", "ALL",
//            electionDesc, string.Empty, string.Empty);
//        }
//      }
//    }

//    private DataTable Office_Table_Groups()
//    {
//      #region SQL
//      string SQL = string.Empty;
//      SQL += " SELECT StateCode,OfficeLevel";
//      SQL += " FROM Offices";
//      SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//      SQL += " GROUP BY StateCode,OfficeLevel";
//      #endregion SQL

//      DataTable OfficeGroups_Table = null;
//      if (SQL != string.Empty)
//      {
//        OfficeGroups_Table = db.Table(SQL);
//      }
//      return OfficeGroups_Table;
//    }

//    private DataTable Offices_Table()
//    {
//      #region SQL
//      string SQL = string.Empty;
//      SQL += " SELECT ";
//      SQL += " OfficeKey";
//      //SQL += ",StateCode";
//      //SQL += ",CountyCode";
//      //SQL += ",LocalCode";
//      SQL += " FROM Offices";
//      //SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//      //SQL += " AND IsOfficeTagForDeletion = 0";

//      ////Special Offices are for temporary unexpired office term
//      //SQL += " AND IsSpecialOffice = 0";

//      //DateTime Test = db.Elections_Date(db.ElectionKey_State(
//      //    ViewState["ElectionKey"].ToString())
//      //  , "ElectionDate");
//      //if (
//      //  (db.Is_Election_Type_General(db.ElectionKey_State(ViewState["ElectionKey"].ToString())))
//      //  && (db.Is_US_Presidential_Election_Year(
//      //    db.Elections_Date(db.ElectionKey_State(ViewState["ElectionKey"].ToString())
//      //    , "ElectionDate")))
//      //  )
//      //{
//      //  SQL += " OR StateCode = 'US'";
//      //}

//      switch (db.Electoral_Class(
//             ViewState["StateCode"].ToString()
//            , ViewState["CountyCode"].ToString()
//            , ViewState["LocalCode"].ToString()
//       ))
//      {
//        case db.ElectoralClass.USPresident:
//          //SQL = string.Empty;

//          //Special Case: Comparing Presidential Candidates
//          SQL += " WHERE OfficeKey = 'USPresident'";
//          break;
//        case db.ElectoralClass.USSenate:
//          SQL = string.Empty;
//          break;
//        case db.ElectoralClass.USHouse:
//          SQL = string.Empty;
//          break;
//        case db.ElectoralClass.USGovernors:
//          SQL = string.Empty;
//          break;
//        case db.ElectoralClass.State:
//          #region State
//          SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//          SQL += " AND CountyCode = ''";
//          SQL += " AND LocalCode = ''";
//          //Election type O (Off Year) and S (Special)
//          //Never have US President contest
//          string electionType = db.Elections_Str(
//            ViewState["ElectionKey"].ToString()
//            , "ElectionType"
//            );
//          if (
//            (electionType.ToUpper() != "O")
//            && (electionType.ToUpper() != "S")
//            )
//            SQL += " OR OfficeKey = 'USPresident'";
//          #endregion State
//          break;
//        case db.ElectoralClass.County:
//          #region County
//          SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//          SQL += " AND CountyCode = "
//            + db.SQLLit(ViewState["CountyCode"].ToString());
//          SQL += " AND LocalCode = ''";
//          #endregion County
//          break;
//        case db.ElectoralClass.Local:
//          #region Local District
//          SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//          SQL += " AND CountyCode = "
//            + db.SQLLit(ViewState["CountyCode"].ToString());
//          SQL += " AND LocalCode = "
//            + db.SQLLit(ViewState["LocalCode"].ToString());
//          #endregion Local District
//          break;
//        default:
//          break;
//      }
//      //SQL += " ORDER BY OfficeLevel,CONVERT(int,DistrictCode),OfficeOrderWithinLevel,DistrictCodeAlpha,OfficeLine1";
//      if (!string.IsNullOrEmpty(SQL))
//      {
//        //Special Offices are for temporary unexpired office term
//        SQL += " AND IsSpecialOffice = 0";
//        SQL += " ORDER BY OfficeLevel,DistrictCode,OfficeOrderWithinLevel,DistrictCodeAlpha,OfficeLine1";
//      }
//      #endregion SQL

//      DataTable OfficesTable = null;
//      if (SQL != string.Empty)
//      {
//        OfficesTable = db.Table(SQL);
//      }
//      return OfficesTable;
//    }

//    protected bool Update_ElectionsOffices_And_Internal_Table_Office(int Index_Checkbox_Changed_Office)
//    {
//      string OfficeKey = ViewState["OfficeKey" + Index_Checkbox_Changed_Office.ToString()].ToString();
//      string Office_Name = Offices.FormatOfficeName(OfficeKey);
//      if (CheckBoxList_Offices_In_Election.Items[Index_Checkbox_Changed_Office].Selected)
//      {
//        CheckBoxList_Offices_In_Election.Items[Index_Checkbox_Changed_Office].Text =
//          db.Anchor_Admin_Office_UPDATE_Election(
//              ViewState["ElectionKey"].ToString()
//            , OfficeKey
//            , Office_Name
//            );
//      }


//      if (CheckBoxList_Offices_In_Election.Items[Index_Checkbox_Changed_Office].Selected)
//        //Insert_ElectionsOffices(Index_Checkbox_Changed_Office);
//        Insert_ElectionsOffices(OfficeKey);
//      else
//        Delete_ElectionsOffices(OfficeKey);

//      #region update internal table of checkboxes
//      ViewState["CheckBoxOffice" + Index_Checkbox_Changed_Office.ToString()]
//        = CheckBoxList_Offices_In_Election.Items[Index_Checkbox_Changed_Office].Selected;
//      #endregion update internal table of checkboxes

//      return CheckBoxList_Offices_In_Election.Items[Index_Checkbox_Changed_Office].Selected;
//    }

//    protected bool Update_ElectionsOffices_And_Internal_Table_Group(int Index_Checkbox_Changed_Group)
//    {
//      //All the offices in the group
//      int Office_Class = Convert.ToInt16(ViewState["OfficeLevel" + Index_Checkbox_Changed_Group.ToString()]);
//      string SQL = string.Empty;
//      SQL += " SELECT OfficeKey";
//      SQL += " FROM Offices";
//      SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//      SQL += " AND CountyCode = " + db.SQLLit(ViewState["CountyCode"].ToString());
//      SQL += " AND LocalCode = " + db.SQLLit(ViewState["LocalCode"].ToString());
//      SQL += " AND OfficeLevel = " + Office_Class.ToString();
//      //SQL += " AND IsOfficeTagForDeletion = 0";
//      SQL += " ORDER BY OfficeKey";
//      DataTable Offices_Table = db.Table(SQL);
//      foreach (DataRow Office_Row in Offices_Table.Rows)
//      {
//        string OfficeKey = Office_Row["OfficeKey"].ToString();

//        if (CheckBoxList_Offices_In_Election_Groups.Items[Index_Checkbox_Changed_Group].Selected)
//        {
//          Insert_ElectionsOffices(Office_Row["OfficeKey"].ToString());
//          ViewState["CheckBoxGroup" + Index_Checkbox_Changed_Group.ToString()] = true;
//        }
//        else
//        {
//          Delete_ElectionsOffices(Office_Row["OfficeKey"].ToString());
//          ViewState["CheckBoxGroup" + Index_Checkbox_Changed_Group.ToString()] = false;
//        }
//      }

//      CheckBoxList_Offices_In_Election.Items.Clear();
//      Load_Offices_Checkboxes_And_Internal_Tables();

//      return CheckBoxList_Offices_In_Election_Groups.Items[Index_Checkbox_Changed_Group].Selected;
//    }

//    protected string Anchor_Add_Offices(OfficeClass officeClass)
//    {
//      return
//        db.Anchor_Admin_Office_ADD(
//            officeClass, "Add Offices for "
//              + Offices.GetLocalizedOfficeClassDescription(
//                officeClass
//                , ViewState["StateCode"].ToString()
//                , ViewState["CountyCode"].ToString()
//                , ViewState["LocalCode"].ToString()
//                ));
//    }

//    protected void Check_Or_Uncheck_All_County_Local_Offices()
//    {
//      #region All Offices in All Counties
//      if (County_Election_Offices() == County_Offices())
//        CheckBox_All_County_Offices.Checked = true;
//      else
//        CheckBox_All_County_Offices.Checked = false;
//      #endregion All Offices in All Counties

//      #region All Offices in All Local Districts
//      if (Local_Election_Offices() == Local_Offices())
//        CheckBox_All_Local_Offices.Checked = true;
//      else
//        CheckBox_All_Local_Offices.Checked = false;
//      #endregion All Offices in All Local Districts
//    }
//    protected void Load_All_County_Local_Offices_Counts()
//    {
//      Label_All_County_Offices.Text =
//       County_Election_Offices().ToString()
//       + " / "
//       + County_Offices();
//      Label_All_Local_Offices.Text =
//        Local_Election_Offices()
//        + " / "
//        + Local_Offices();
//    }

//    private void Load_Links_To_Add_Offices()
//    {
//      switch (db.Electoral_Class(
//                ViewState["StateCode"].ToString()
//                , ViewState["CountyCode"].ToString()
//                , ViewState["LocalCode"].ToString()
//                )
//        )
//      {
//        case db.ElectoralClass.State:
//          #region State
//          LabelAddOffices.Text = "<br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(OfficeClass.StateWide);
//          LabelAddOffices.Text += "<br><br>";

//          #region Include later
//#if false
//          LabelAddOffices.Text += Anchor_Add_Offices(db.Office_State_District_Multi_Counties);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(db.Office_State_Judicial);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(db.Office_State_District_Multi_Counties_Judicial);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(db.Office_State_Party);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(db.Office_State_District_Multi_Counties_Party);
//          LabelAddOffices.Text += "<br><br>";
//#endif
//          #endregion Include later

//          #endregion State
//          break;
//        case db.ElectoralClass.County:
//          #region County
//          LabelAddOffices.Text = "<br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(OfficeClass.CountyExecutive);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(OfficeClass.CountyLegislative);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(OfficeClass.CountySchoolBoard);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(OfficeClass.CountyCommission);
//          LabelAddOffices.Text += "<br><br>";

//          #region Include later
//#if false
//          LabelAddOffices.Text += Anchor_Add_Offices(db.Office_County_Judicial);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(db.Office_County_Party);
//          LabelAddOffices.Text += "<br>";
//#endif
//          #endregion Include later

//          #endregion County
//          break;
//        case db.ElectoralClass.Local:
//          #region Local District
//          LabelAddOffices.Text = "<br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(OfficeClass.LocalExecutive);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(OfficeClass.LocalLegislative);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(OfficeClass.LocalSchoolBoard);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(OfficeClass.LocalCommission);
//          LabelAddOffices.Text += "<br><br>";

//          #region Include later
//#if false
//          LabelAddOffices.Text += Anchor_Add_Offices(db.Office_Local_Judicial);
//          LabelAddOffices.Text += "<br><br>";

//          LabelAddOffices.Text += Anchor_Add_Offices(db.Office_Local_Party);
//          LabelAddOffices.Text += "<br><br>";
//#endif
//          #endregion Include later

//          #endregion Local District
//          break;
//        default:
//          break;
//      }
//    }
//    private void Load_Groups_Checkboxes_And_Internal_Tables()
//    {

//      DataTable OfficeGroups_Table = Office_Table_Groups();
//      if (OfficeGroups_Table != null)
//      {
//        int Index = 0;
//        foreach (DataRow Office_Row in OfficeGroups_Table.Rows)
//        {
//          //string SQL = string.Empty;
//          ////SQL += " SELECT StateCode,OfficeLevel";
//          //SQL += "Offices";
//          //SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//          //SQL += " AND CountyCode = " + db.SQLLit(ViewState["CountyCode"].ToString());
//          //SQL += " AND LocalCode = " + db.SQLLit(ViewState["LocalCode"].ToString());
//          //SQL += " AND OfficeLevel = " + Office_Row["OfficeLevel"].ToString();
//          //SQL += " AND IsOfficeTagForDeletion = 0";
//          //int Offices_In_Group = db.Rows_Count_From(SQL);
//          if (Offices_In_Group(Office_Row) > 1)
//          {
//            //Create checkbox
//            CheckBoxList_Offices_In_Election_Groups.Items.Add(new ListItem());

//            #region Set Office Name
//            CheckBoxList_Offices_In_Election_Groups.Items[Index].Text = //"<nobr>"
//            "All " + Offices_In_Group(Office_Row) + " Offices for "
//            + Offices.GetLocalizedOfficeClassDescription(
//               Office_Row["OfficeLevel"].ToOfficeClass()
//                , ViewState["StateCode"].ToString()
//                , ViewState["CountyCode"].ToString()
//                , ViewState["LocalCode"].ToString());
//            //+ "</nobr>";
//            #endregion Set Office Name

//            #region UnCheck Checkbox and save in internal table
//            //If all offices in group are in the election
//            string SQL = string.Empty;
//            SQL += "ElectionsOffices";
//            SQL += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
//            SQL += " AND OfficeLevel = " + Convert.ToInt16(Office_Row["OfficeLevel"]).ToString();
//            int Offices_In_Election = db.Rows_Count_From(SQL);

//            if (Offices_In_Group(Office_Row) == Offices_In_Election)
//            {
//              CheckBoxList_Offices_In_Election_Groups.Items[Index].Selected = true;
//              ViewState["CheckBoxGroup" + Index.ToString()] = true;
//            }
//            else
//            {
//              CheckBoxList_Offices_In_Election_Groups.Items[Index].Selected = false;
//              ViewState["CheckBoxGroup" + Index.ToString()] = false;
//            }
//            #endregion UnCheck Checkbox and save in internal table

//            #region Save OfficeLevel in internal table
//            ViewState["OfficeLevel" + Index.ToString()] = Office_Row["OfficeLevel"].ToString();
//            #endregion Save OfficeLevel in internal table

//            Index++;
//          }
//        }
//      }
//    }
//    private void Load_Offices_Checkboxes_And_Internal_Tables()
//    {
//      DataTable OfficesTable = Offices_Table();
//      if (OfficesTable != null)
//      {
//        int Index = 0;
//        foreach (DataRow Office_Row in OfficesTable.Rows)
//        {
//          #region an office
//          //Create checkbox
//          CheckBoxList_Offices_In_Election.Items.Add(new ListItem());

//          #region Check or UnCheck Checkbox and save in internal table
//          string SQL = " ElectionsOffices";
//          SQL += " WHERE ElectionKey=" + db.SQLLit(ViewState["ElectionKey"].ToString());
//          SQL += " AND OfficeKey=" + db.SQLLit(Office_Row["OfficeKey"].ToString());
//          int ElectionsOffices_Rows = db.Rows_Count_From(SQL);
//          if (ElectionsOffices_Rows > 0)
//          {
//            #region Checked
//            CheckBoxList_Offices_In_Election.Items[Index].Selected = true;
//            ViewState["CheckBoxOffice" + Index.ToString()] = true;
//            #endregion Checked
            
//            #region Office Name anchor to update office
//            CheckBoxList_Offices_In_Election.Items[Index].Text =
//              //"<nobr>"
//              db.Anchor_Admin_Office_UPDATE_Election(
//                  ViewState["ElectionKey"].ToString()
//                  , Office_Row["OfficeKey"].ToString()
//                  , Offices.FormatOfficeName(Office_Row["OfficeKey"].ToString()));
//            //)
//            //+ "</nobr>";
//            #endregion Office Name anchor to update office
//          }
//          else
//          {
//            #region UnChecked
//            CheckBoxList_Offices_In_Election.Items[Index].Selected = false;
//            ViewState["CheckBoxOffice" + Index.ToString()] = false;
//            #region Set Office Name
//            CheckBoxList_Offices_In_Election.Items[Index].Text =
//              //"<nobr>"
//              Offices.FormatOfficeName(Office_Row["OfficeKey"].ToString());
//            //+ "</nobr>";
//            #endregion Set Office Name
//            #endregion UnChecked
//          }
//          #endregion Check or UnCheck Checkbox and save in internal table

//          #region US President disabled for General election

//          // Disallow unchecking of US President 
//          // for General 4 Elections
//          string electionType = db.Elections_Str(
//            ViewState["ElectionKey"].ToString()
//            , "ElectionType"
//            );
//          if (
//            (electionType.ToUpper() == "G")
//            && (Offices.FormatOfficeName(Office_Row["OfficeKey"].ToString()) == "U.S. President")
//            )
//            CheckBoxList_Offices_In_Election.Items[Index].Enabled = false;
//          #endregion US President disabled for General election

//          #region Save OfficeKey in internal table
//          ViewState["OfficeKey" + Index.ToString()] = Office_Row["OfficeKey"].ToString();
//          #endregion Save OfficeKey in internal table

//          Index++;
//          #endregion an office
//        }
//      }
//    }

//    private int Index_Checkbox_That_Changed_Office()
//    {
//      int Checkbox_Index_Office = 0;
//      DataTable OfficesTable = Offices_Table();
//      if (OfficesTable != null)
//      {
//        bool Is_Found_Change = false;
//        int Index = 0;
//        foreach (DataRow Office_Row in OfficesTable.Rows)
//        {
//          if (!Is_Found_Change)
//          {
//            if (Convert.ToBoolean(ViewState["CheckBoxOffice" + Index.ToString()]) !=
//              CheckBoxList_Offices_In_Election.Items[Index].Selected)
//            {
//              Checkbox_Index_Office = Index;
//              Is_Found_Change = true;
//            }
//            Index++;
//          }
//        }
//      }
//      return Checkbox_Index_Office;
//    }

//    protected void CheckBoxList_Offices_In_Election_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        int Index_Checkbox_Changed_Office = Index_Checkbox_That_Changed_Office();

//        bool Is_Checked = Update_ElectionsOffices_And_Internal_Table_Office(Index_Checkbox_Changed_Office);

//        string OfficeKey = ViewState["OfficeKey" + Index_Checkbox_Changed_Office.ToString()].ToString();

//        #region Msg
//        if (Is_Has_ElectionsOffices_Rows(ViewState["ElectionKey"].ToString()))
//        {
//          if (Is_Checked)
//            Msg.Text = db.Ok(Offices.FormatOfficeName(OfficeKey)
//              + " was ADDED to this" + Msg_Electoral() + " Election.");
//          else
//            Msg.Text = db.Ok(Offices.FormatOfficeName(OfficeKey)
//              + " was DELETED from this" + Msg_Electoral() + " Election.");
//        }
//        else
//        {
//          Msg.Text = db.Warn("No office contests are currently identified in this"
//         + Msg_Electoral() + " Election.");
//        }
//        #endregion Msg

//        //db.Invalidate_Election(ViewState["ElectionKey"].ToString());
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    private int Index_Checkbox_That_Changed_Group()
//    {
//      int Checkbox_Index_Group = 0;
//      DataTable OfficeGroups_Table = Office_Table_Groups();
//      if (OfficeGroups_Table != null)
//      {
//        bool Is_Found_Change = false;
//        int Index = 0;
//        foreach (DataRow Office_Row in OfficeGroups_Table.Rows)
//        {
//          //string SQL = string.Empty;
//          ////SQL += " SELECT StateCode,OfficeLevel";
//          //SQL += "Offices";
//          //SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
//          //SQL += " AND CountyCode = " + db.SQLLit(ViewState["CountyCode"].ToString());
//          //SQL += " AND LocalCode = " + db.SQLLit(ViewState["LocalCode"].ToString());
//          //SQL += " AND OfficeLevel = " + Office_Row["OfficeLevel"].ToString();
//          //SQL += " AND IsOfficeTagForDeletion = 0";
//          //int Offices = db.Rows_Count_From(SQL);
//          //if (db.Rows_Count_From(SQL) > 1)
//          if (Offices_In_Group(Office_Row) > 1)
//          {
//            if (!Is_Found_Change)
//            {
//              if (Convert.ToBoolean(ViewState["CheckBoxGroup" + Index.ToString()]) !=
//                CheckBoxList_Offices_In_Election_Groups.Items[Index].Selected)
//              {
//                Checkbox_Index_Group = Index;
//                Is_Found_Change = true;
//              }
//              Index++;
//            }
//          }
//        }
//      }
//      return Checkbox_Index_Group;
//    }

//    protected void CheckBoxList_Offices_In_Election_Groups_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        int Index_Checkbox_Changed_Group = Index_Checkbox_That_Changed_Group();

//        bool Is_Checked = Update_ElectionsOffices_And_Internal_Table_Group(Index_Checkbox_Changed_Group);

//        #region Msg
//        var officeClass = ViewState["OfficeLevel" + 
//          Index_Checkbox_Changed_Group].ToOfficeClass();
//        string Msg_Return = "All Offices for "
//            + Offices.GetLocalizedOfficeClassDescription(
//                officeClass
//                , ViewState["StateCode"].ToString()
//                , ViewState["CountyCode"].ToString()
//                , ViewState["LocalCode"].ToString()) + " have been";
//        if (Is_Checked)
//          Msg_Return += " INCLUDED in";
//        else
//          Msg_Return += " DELETED from";

//        Msg_Return += " this election.";

//        Msg.Text = db.Ok(Msg_Return);
//        #endregion Msg

//        //db.Invalidate_Election(ViewState["ElectionKey"].ToString());
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void CheckBox_All_County_Offices_CheckedChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        Server.ScriptTimeout = 60000;//6000 sec = 100 min

//        if (CheckBox_All_County_Offices.Checked)
//        {
//          Delete_All_County_Election_Tables_Rows();

//          Insert_All_County_Election_Tables_Rows();

//          Msg.Text = db.Ok("All " + County_Offices().ToString()
//            + " County Office Contests are in the election.");
//        }
//        else
//        {
//          Delete_All_County_Election_Tables_Rows();

//          Msg.Text = db.Ok("All " + County_Offices().ToString()
//            + " County Office Contests have been removed the election.");
//        }

//        Load_All_County_Local_Offices_Counts();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }

//    }

//    protected void CheckBox_All_Local_Offices_CheckedChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        Server.ScriptTimeout = 60000;//6000 sec = 100 min

//        if (CheckBox_All_Local_Offices.Checked)
//        {
//          Delete_All_Local_Election_Tables_Rows();

//          Insert_All_Local_Election_Tables_Rows();

//          Msg.Text = db.Ok("All " + Local_Offices().ToString()
//            + " Local District Office Contests are in the election.");
//        }
//        else
//        {
//          Delete_All_Local_Election_Tables_Rows();

//          Msg.Text = db.Ok("All " + Local_Offices().ToString()
//            + " Local District Office Contests have been removed the election.");
//        }

//        Load_All_County_Local_Offices_Counts();

//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }


//    protected void Page_Load(object sender, EventArgs e)
//    {
//      if (!IsPostBack)
//      {
//        #region ViewState Values
//        ViewState["ElectionKey"] = string.Empty;

//        #region ViewState["ElectionKey"]
//        if (!string.IsNullOrEmpty(QueryElection))
//          ViewState["ElectionKey"] = QueryElection;
//        #endregion ViewState["ElectionKey"]
//        #endregion ViewState Values

//        #region Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

//        #region Notes
//        //The Session UserStateCode, UserCountyCode, UserLocalCode can be changed
//        //by a higher administration level using query strings
//        //This is done in db.State_Code(), db.County_Code(), db.Local_Code()
//        //
//        //Using ViewState variables insures these values won't
//        //change on any postbacks or in different tab or browser Sessions.
//        //
//        //ViewState["StateCode"] can be a StateCode or U1, u2, u3 for FederalCode
//        #endregion Notes

//        ViewState["StateCode"] = db.State_Code();
//        ViewState["CountyCode"] = db.County_Code();
//        ViewState["LocalCode"] = db.Local_Code();
//        if (!db.Is_User_Security_Ok())
//          SecurePage.HandleSecurityException();

//        #endregion Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

//        try
//        {
//          Visible_Controls();

//          Page_Title();

//          #region Heading Labels
//          LabelOfficeContests.Text = Offices.GetElectoralClassDescription(
//            ViewState["StateCode"].ToString()
//          , ViewState["CountyCode"].ToString()
//          , ViewState["LocalCode"].ToString()
//          );

//          LabelOfficeContestsGroups.Text = Offices.GetElectoralClassDescription(
//            ViewState["StateCode"].ToString()
//          , ViewState["CountyCode"].ToString()
//          , ViewState["LocalCode"].ToString()
//          );

//          LabelOfficesAdd.Text = Offices.GetElectoralClassDescription(
//            ViewState["StateCode"].ToString()
//          , ViewState["CountyCode"].ToString()
//          , ViewState["LocalCode"].ToString()
//          );
//          #endregion Heading Labels

//          Load_Groups_Checkboxes_And_Internal_Tables();

//          Load_Offices_Checkboxes_And_Internal_Tables();

//          //Links to add new elected offices that are not in database
//          Load_Links_To_Add_Offices();

//          #region All County and/or Local Contest to Include or Exclude
//          if (
//            (SecurePage.IsSuperUser)
//            && (db.Electoral_Class(
//                ViewState["StateCode"].ToString()
//                , ViewState["CountyCode"].ToString()
//                , ViewState["LocalCode"].ToString()
//            ) == db.ElectoralClass.State)
//            )
//          {
//            TableAllCountyAndLocal.Visible = true;
//            Check_Or_Uncheck_All_County_Local_Offices();
//            Load_All_County_Local_Offices_Counts();
//          }
//          else
//            TableAllCountyAndLocal.Visible = false;
//          #endregion All County and/or Local Contest to Include or Exclude

//          #region Msg
//          if (Elections.IsStateElection(ViewState["ElectionKey"].ToString()))
//          {
//            #region State Election
//            if (!Is_Has_ElectionsOffices_Rows_Election_State(
//              db.ElectionKey_State(ViewState["ElectionKey"].ToString())))
//            {
//              Msg.Text = db.Warn("This Election has been created for"
//                + " " + Msg_Electoral()
//                + " but no election contests have been identified."
//                + " Check each Federal and State office contest in this"
//                + " " + Msg_Electoral() + " Election.");
//            }
//            else
//            {
//              Msg.Text = db.Ok("Check each Federal and State office contest in this"
//                + " " + Msg_Electoral() + " Election.");
//            }
//            #endregion State Election
//          }
//          else
//          {
//            #region County or Local Election
//            if (db.Is_Valid_Election(ViewState["ElectionKey"].ToString()))
//            {
//              if (!Is_Has_ElectionsOffices_Rows(
//                db.ElectionKey_County(ViewState["ElectionKey"].ToString())))
//              {
//                Msg.Text = db.Warn("This Election has been created for"
//                  + " " + Msg_Electoral()
//                  + " but no election contests have been identified."
//                  + " Check each office contest in this"
//                  + " " + Msg_Electoral() + " Election.");
//              }
//              else
//              {
//                #region County or Local Election Exists
//                Msg.Text = db.Msg("Check each office contest in this"
//                  + Msg_Electoral() + " Election.");
//                #endregion County or Local Election Exists
//              }
//            }
//            else
//            {
//              #region No County or Local Election Exists
//              Msg.Text = db.Warn("This Election has been created."
//                  + " But no office contests or referendums"
//                  + " have been defined for this"
//                  + Msg_Electoral()
//                  + " Election."
//                  + " Check each office contest in this"
//                  + Msg_Electoral() + " Election.");
//              #endregion No County or Local Election Exists
//            }
//            #endregion County or Local Election
//          }
//          #endregion Msg

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

    protected override void OnInit(EventArgs e)
    {
      LegacyRedirect(SecureAdminPage.GetUpdateElectionsPageUrl(QueryState,
        QueryCounty, QueryLocal));
      base.OnInit(e);
    }

    #region Dead code

    //private int xElectionsOffices_Rows(string ElectionKey)
    //{
    //  string SQL = string.Empty;
    //  SQL += " ElectionsOffices";
    //  SQL += " WHERE ElectionKey =" + db.SQLLit(ElectionKey);
    //  return db.Rows_Count_From(SQL);
    //}

    #endregion Dead code


  }
}
