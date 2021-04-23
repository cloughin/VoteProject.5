using System;
using DB.Vote;

namespace Vote.Master
{
  public partial class MaintOfficesPage : SecurePage, ISuperUser
  {
//    #region from db

//    private static void ElectionsOffices_Update_OfficeLevel(string officeKey, int officeLevel)
//    {
//      var sqlupdate = "UPDATE ElectionsOffices"
//      + " SET OfficeLevel =" + officeLevel
//      + " WHERE OfficeKey =" + db.SQLLit(officeKey);
//      db.ExecuteSql(sqlupdate);
//    }

//    private static bool Is_Electoral_District(OfficeClass officeClass)
//    {
//      //Need a DistrictCode
//      return (db.Is_Electoral_District_Multi_Counties(officeClass))
//        || (db.Is_Electoral_District_Multi_Partial_Counties(officeClass));
//    }

//    private static bool Is_Electoral_Federal_State_District(string officeKey)
//    {
//      //Only needs a StateCode
//      return (db.Is_Electoral_Federal(officeKey))
//        || (Is_Electoral_State(officeKey))
//        || (db.Is_Electoral_District_Multi_Counties(officeKey))
//        || (db.Is_Electoral_District_Multi_Partial_Counties(officeKey));
//    }

//    private static bool Is_Electoral_State(string officeKey)
//    {
//      return Is_Electoral_State(Offices.GetOfficeClass(officeKey));
//    }

//    private static bool Is_Electoral_State(OfficeClass officeClass)
//    {
//      switch (officeClass)
//      {
//        case OfficeClass.StateWide:
//        case OfficeClass.StateSenate:
//        case OfficeClass.StateHouse:
//          return true;
//        default:
//          return false;
//      }
//    }

//    private static bool Is_Electoral_Federal_State_District(OfficeClass officeClass)
//    {
//      //Only needs a StateCode
//      return (db.Is_Electoral_Federal(officeClass))
//        || (Is_Electoral_State(officeClass))
//        || (db.Is_Electoral_District_Multi_Counties(officeClass))
//        || (db.Is_Electoral_District_Multi_Partial_Counties(officeClass));
//    }

//    private static bool Is_Electoral_Class_Can_Add_Offices(OfficeClass officeClass)
//    {
//      switch (officeClass)
//      {
//        case OfficeClass.StateWide:
//        case OfficeClass.StateDistrictMultiCounties:
//        case OfficeClass.CountyExecutive:
//        case OfficeClass.LocalExecutive:
//        case OfficeClass.CountyLegislative:
//        case OfficeClass.LocalLegislative:
//        case OfficeClass.CountySchoolBoard:
//        case OfficeClass.LocalSchoolBoard:
//        case OfficeClass.CountyCommission:
//        case OfficeClass.LocalCommission:
//        case OfficeClass.StateJudicial:
//        case OfficeClass.StateDistrictMultiCountiesJudicial:
//        case OfficeClass.CountyJudicial:
//        case OfficeClass.LocalJudicial:
//        case OfficeClass.StateParty:
//        case OfficeClass.StateDistrictMultiCountiesParty:
//        case OfficeClass.CountyParty:
//        case OfficeClass.LocalParty:
//          return true;
//        default:
//          return false;
//      }
//    }
    
//    #endregion from db

//    private void OfficeKey_Update_Tables_Except_Offices()
//    {
//      //db.Is_Valid_ElectionsOffices_Election_Office_Class("",0)
//      var sql = string.Empty;
//      sql += " UPDATE ElectionsOffices";
//      sql += " SET OfficeKey = " + db.SQLLit(TextBox_OfficeKey2.Text.Trim());
//      sql += " WHERE OfficeKey = " + db.SQLLit(TextBox_OfficeKey1.Text.Trim());
//      db.ExecuteSql(sql);

//      sql = string.Empty;
//      sql += " UPDATE ElectionsPoliticians";
//      sql += " SET OfficeKey = " + db.SQLLit(TextBox_OfficeKey2.Text.Trim());
//      sql += " WHERE OfficeKey = " + db.SQLLit(TextBox_OfficeKey1.Text.Trim());
//      db.ExecuteSql(sql);

//      sql = string.Empty;
//      sql += " UPDATE OfficesOfficials";
//      sql += " SET OfficeKey = " + db.SQLLit(TextBox_OfficeKey2.Text.Trim());
//      sql += " WHERE OfficeKey = " + db.SQLLit(TextBox_OfficeKey1.Text.Trim());
//      db.ExecuteSql(sql);

//      sql = string.Empty;
//      sql += " UPDATE Politicians";
//      sql += " SET OfficeKey = " + db.SQLLit(TextBox_OfficeKey2.Text.Trim());
//      sql += " WHERE OfficeKey = " + db.SQLLit(TextBox_OfficeKey1.Text.Trim());
//      db.ExecuteSql(sql);
//    }

//    private void OfficeKey_Update_Tables_All()
//    {
//      OfficeKey_Update_Tables_Except_Offices();

//      if (!Offices.OfficeKeyExists(TextBox_OfficeKey2.Text.Trim()))
//      {
//        var sql = string.Empty;
//        sql += " UPDATE Offices";
//        sql += " SET OfficeKey = " + db.SQLLit(TextBox_OfficeKey2.Text.Trim());
//        sql += " WHERE OfficeKey = " + db.SQLLit(TextBox_OfficeKey1.Text.Trim());
//        db.ExecuteSql(sql);
//      }
//    }

//    private void OfficeKey_Update_Tables_Consolidate()
//    {
//      OfficeKey_Update_Tables_Except_Offices();

//      db.Office_Delete_All_Tables_All_Rows(TextBox_OfficeKey1.Text.Trim(), "C");
//    }

////--

//    private void Check_Is_Can_Change()
//    {
//      //TextBox_OfficeKey2.Text.Trim() may not exist when changing office keys
//      if (
//        (Offices.GetStateCodeFromKey(TextBox_OfficeKey1.Text.Trim()).ToUpper() != "DC")
//        && (!Is_Electoral_Class_Can_Add_Offices(
//        Offices.GetOfficeClass(TextBox_OfficeKey1.Text.Trim())))
//        )
//        throw new ApplicationException(
//          "The Offices are completely defined and can not be added, changed or deleted.");
//    }

//    private void Check_Electoral_Office()
//    {
//      var electoralOfficeFrom = db.Electoral_Class(
//             Offices.GetStateCodeFromKey(TextBox_OfficeKey1.Text.Trim())
//            , Offices.GetCountyCodeFromKey(TextBox_OfficeKey1.Text.Trim())
//            , Offices.GetLocalCodeFromKey(TextBox_OfficeKey1.Text.Trim())
//       );
//      var electoralOfficeTo = db.Electoral_Class(
//        //db.StateCode_In_OfficeKey_New(TextBox_OfficeKey2.Text.Trim())
//             Offices.GetStateCodeFromKey(TextBox_OfficeKey2.Text.Trim())
//            , Offices.GetCountyCodeFromKey(TextBox_OfficeKey2.Text.Trim())
//            , Offices.GetLocalCodeFromKey(TextBox_OfficeKey2.Text.Trim())
//       );
//      if (electoralOfficeFrom != electoralOfficeTo)
//        throw new ApplicationException(
//          "The OfficeKey being changed TO must be same Electoral Level"
//          + " because the StateCode, CountyCode and LocalCodes must be the same.");
//    }

//    private void Check_StateCodes_Match()
//    {
//      if (Offices.GetStateCodeFromKey(TextBox_OfficeKey1.Text.Trim()) !=
//        //db.StateCode_In_OfficeKey_New(TextBox_OfficeKey2.Text.Trim()))
//        Offices.GetStateCodeFromKey(TextBox_OfficeKey2.Text.Trim()))
//        throw new ApplicationException("The StateCodes are different.");
//    }

//    private void Check_CountyCodes_Match()
//    {
//      if (Offices.GetCountyCodeFromKey(TextBox_OfficeKey1.Text.Trim()) !=
//        Offices.GetCountyCodeFromKey(TextBox_OfficeKey2.Text.Trim()))
//        throw new ApplicationException("The CountyCodes are different.");
//    }

//    private void Check_LocalCodes_Match()
//    {
//      if (Offices.GetLocalCodeFromKey(TextBox_OfficeKey1.Text.Trim()) !=
//        Offices.GetLocalCodeFromKey(TextBox_OfficeKey2.Text.Trim()))
//        throw new ApplicationException("The LocalCodes are different.");
//    }

//    private void Check_Electoral_Classs()
//    {
//      Check_Is_Can_Change();
//      Check_Electoral_Office();
//      Check_StateCodes_Match();
//      Check_CountyCodes_Match();
//      Check_LocalCodes_Match();
//    }

//    protected void ButtonDelete_Click1(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Office_Delete_All_Tables_All_Rows(TextBox_OfficeKey1.Text);

//        Msg.Text = db.Ok("The 1st Office has been DELETED in all Tables.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }

//    }


//    protected void Button_View_Office1_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        //Office_Data_Get(
//        //  ref TextBox_OfficeKey1
//        //  , ref Label_Office_Title1
//        //  , ref Label_Office1
//        //  , ref Label_StateCode1
//        //  , ref Label_CountyCode1
//        //  , ref Label_LocalCode1
//        //  , ref Label_Office_Desc1
//        //  );
//        Label_Office1.Text = db.Office_Data(TextBox_OfficeKey1.Text.Trim());
//        Msg.Text = db.Ok("The office data is presented under the OfficeKey Textbox.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }

//    protected void Button_View_Office2_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        Label_Office2.Text = db.Office_Data(TextBox_OfficeKey2.Text.Trim());
//        Msg.Text = db.Ok("The office data is presented under the OfficeKey Textbox.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }

//    }

//    protected void Button_Consolidate_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        #region checks
//        //Office being consolidated into must exist
//        if (!Offices.IsValid(TextBox_OfficeKey2.Text.Trim()))
//          throw new ApplicationException(
//            "The OfficeKey being consolidated into does not exist.");

//        Check_Electoral_Classs();

//        //db.OfficeKey_Update_Tables_Except_Offices(
//        //  TextBox_OfficeKey1.Text.Trim()
//        //  , TextBox_OfficeKey2.Text.Trim()
//        //  );

//        ////Also invalidates the office
//        //db.Office_Delete_All_Tables_All_Rows(TextBox_OfficeKey1.Text.Trim(), "C");

//        OfficeKey_Update_Tables_Consolidate();
//        #endregion checks

//        Label_Office1.Text = db.Office_Data(TextBox_OfficeKey1.Text.Trim());
//        Label_Office2.Text = db.Office_Data(TextBox_OfficeKey2.Text.Trim());


//        //db.Invalidate_Office(TextBox_OfficeKey1.Text.Trim());

//        Msg.Text = db.Ok("The Office with OfficeKey "
//          + TextBox_OfficeKey1.Text.Trim()
//          + " has been CONSOLIDATED into the Office with OfficeKey "
//          + TextBox_OfficeKey2.Text.Trim()
//          + " and office "
//          + TextBox_OfficeKey1.Text.Trim()
//          + " has been deleted in all Tables.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }

//    protected void Button_Change_OfficeKey_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        if (Offices.IsValid(TextBox_ChangeOfficeKey_To.Text.Trim()))
//          throw new ApplicationException("The OfficeKey being changed TO must NOT exist."
//            + " Try consolidating the offices.");

//        Check_Electoral_Classs();

//        //db.OfficeKey_Update_Tables_Except_Offices(
//        //  TextBox_ChangeOfficeKey_From.Text.Trim()
//        //  , TextBox_ChangeOfficeKey_To.Text.Trim()
//        //  );

//        //string SQL = string.Empty;
//        //SQL += " UPDATE Offices";
//        //SQL += " SET OfficeKey = " + db.SQLLit(TextBox_ChangeOfficeKey_To.Text.Trim());
//        //SQL += " WHERE OfficeKey = " + db.SQLLit(TextBox_ChangeOfficeKey_From.Text.Trim());
//        //db.ExecuteSQL(SQL);

//        OfficeKey_Update_Tables_All();

//        Label_Office1.Text = db.Office_Data(TextBox_OfficeKey1.Text.Trim());

//        //db.Invalidate_Office(TextBox_ChangeOfficeKey_To.Text.Trim());

//        Msg.Text = db.Ok("The OfficeKey has been changed and is shown directly above.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }

//    protected void TextBox_Office_TextChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        #region Checks

//        if (!db.Is_Valid_Integer(TextBox_Office_Class.Text.Trim()))
//          throw new ApplicationException("Class is not an integer.");

//        if (Is_Electoral_Federal_State_District(TextBox_Office_Class.Text.Trim()
//          .ToOfficeClass()) !=
//          Is_Electoral_Federal_State_District(
//            TextBox_OfficeKey_Change.Text.Trim()))
//          throw new ApplicationException(
//            "Both offices classes need to require only a StateCode.");

//        if (db.Is_Electoral_County(TextBox_Office_Class.Text.Trim()
//          .ToOfficeClass()) !=
//          db.Is_Electoral_County(TextBox_OfficeKey_Change.Text.Trim()))
//          throw new ApplicationException(
//            "Both offices classes need to require a CountyCode.");
//        if (db.Is_Electoral_Local(TextBox_Office_Class.Text.Trim()
//          .ToOfficeClass()) !=
//          db.Is_Electoral_Local(TextBox_OfficeKey_Change.Text.Trim()))
//          throw new ApplicationException(
//            "Both offices classes need to require a LocalCode.");

//        if (Is_Electoral_District(TextBox_Office_Class.Text.Trim()
//          .ToOfficeClass()))
//          throw new ApplicationException(
//            "This office class require a DistrictCode.");

//        #endregion Checks

//        #region Update Offices

//        db.Offices_Update_Int(TextBox_OfficeKey_Change.Text.Trim(), "OfficeLevel",
//          Convert.ToUInt16(TextBox_Office_Class.Text.Trim()));
//        if (!db.Is_Electoral_District(TextBox_OfficeKey_Change.Text.Trim()))
//          Offices.UpdateDistrictCode(string.Empty, TextBox_OfficeKey_Change.Text.Trim());
//          //db.Offices_Update_Str(TextBox_OfficeKey_Change.Text.Trim(),
//          //  "DistrictCode", string.Empty);

//        #endregion Update Offices

//        #region Update ElectionsOffices

//        ElectionsOffices_Update_OfficeLevel(
//          TextBox_OfficeKey_Change.Text.Trim(),
//          Convert.ToUInt16(TextBox_Office_Class.Text.Trim()));

//        #endregion Update ElectionsOffices

//        if (!db.Is_Electoral_District(TextBox_OfficeKey_Change.Text.Trim()))
//          db.ElectionsOffices_Update_DistrictCode(
//            TextBox_OfficeKey_Change.Text.Trim(), string.Empty);
//        Msg.Text = db.Ok("Office Class Has been chaged.");
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
//      //if (!SecurePage.IsMasterUser)
//      if (!IsSuperUser)
//        HandleSecurityException();

//      Title = H1.InnerText = "Offices Maintenance";

//      try
//      {
//        if (!IsPostBack)
//        {
//          if (!string.IsNullOrEmpty(QueryOffice))
//          {
//            if (!Offices.OfficeKeyExists(QueryOffice))
//              throw new ApplicationException("No OfficeID in Offices Table: "
//                + QueryOffice);

//            TextBox_OfficeKey1.Text = QueryOffice;
//          }
//        }
//      }

//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }


    #region Dead code

    //protected void xUpdate_Office_Rows_In_Tables()
    //{
    //  string SQL = string.Empty;

    //  SQL = string.Empty;
    //  SQL += " UPDATE ElectionsOffices";
    //  SQL += " SET OfficeKey = " + db.SQLLit(TextBox_OfficeKey2.Text.Trim());
    //  SQL += " WHERE OfficeKey = " + db.SQLLit(TextBox_OfficeKey1.Text.Trim());
    //  db.ExecuteSQL(SQL);

    //  SQL = string.Empty;
    //  SQL += " UPDATE ElectionsPoliticians";
    //  SQL += " SET OfficeKey = " + db.SQLLit(TextBox_OfficeKey2.Text.Trim());
    //  SQL += " WHERE OfficeKey = " + db.SQLLit(TextBox_OfficeKey1.Text.Trim());
    //  db.ExecuteSQL(SQL);

    //  SQL = string.Empty;
    //  SQL += " UPDATE OfficesOfficials";
    //  SQL += " SET OfficeKey = " + db.SQLLit(TextBox_OfficeKey2.Text.Trim());
    //  SQL += " WHERE OfficeKey = " + db.SQLLit(TextBox_OfficeKey1.Text.Trim());
    //  db.ExecuteSQL(SQL);

    //  SQL = string.Empty;
    //  SQL += " UPDATE Politicians";
    //  SQL += " SET OfficeKey = " + db.SQLLit(TextBox_OfficeKey2.Text.Trim());
    //  SQL += " WHERE OfficeKey = " + db.SQLLit(TextBox_OfficeKey1.Text.Trim());
    //  db.ExecuteSQL(SQL);
    //}
    //protected void xDelete_Office_Rows_In_Tables(string OfficeKey, string D_Or_C)
    //{
    //  #region Delete all rows in Offices, ElectionsOffices, ElectionsPoliticians, OfficesOfficials
    //  string SQL = string.Empty;

    //  #region LogOfficeAddsDeletes
    //  SQL = "INSERT INTO LogOfficeAddsDeletes ";
    //  SQL += "(";
    //  SQL += "DateStamp";
    //  SQL += ",AddOrDelete";
    //  SQL += ",UserSecurity";
    //  SQL += ",UserName";
    //  SQL += ",OfficeKey";
    //  SQL += ",StateCode";
    //  SQL += ",OfficeLevel";
    //  SQL += ",OfficeLine1";
    //  SQL += ",OfficeLine2";
    //  SQL += ")";
    //  SQL += " VALUES";
    //  SQL += "(";
    //  SQL += db.SQLLit(Db.DbNow);
    //  SQL += ",'" + D_Or_C.ToUpper() + "'";
    //  SQL += "," + db.SQLLit(db.User_Security());
    //  SQL += "," + db.SQLLit(db.User_Name());
    //  SQL += "," + db.SQLLit(OfficeKey);
    //  SQL += "," + db.SQLLit(db.StateCode_In_OfficeKey(OfficeKey));
    //  SQL += "," + db.Office_Class(OfficeKey);
    //  SQL += "," + db.SQLLit(db.Offices_Str(OfficeKey, "OfficeLine1"));
    //  SQL += "," + db.SQLLit(db.Offices_Str(OfficeKey, "OfficeLine2"));
    //  SQL += ")";
    //  db.ExecuteSQL(SQL);
    //  #endregion

    //  SQL = " DELETE FROM ElectionsOffices";
    //  SQL += " WHERE OfficeKey = " + db.SQLLit(OfficeKey);
    //  db.ExecuteSQL(SQL);

    //  SQL = " DELETE FROM ElectionsPoliticians";
    //  SQL += " WHERE OfficeKey = " + db.SQLLit(OfficeKey);
    //  db.ExecuteSQL(SQL);

    //  SQL = " DELETE FROM OfficesOfficials";
    //  SQL += " WHERE OfficeKey = " + db.SQLLit(OfficeKey);
    //  db.ExecuteSQL(SQL);

    //  SQL = " DELETE FROM Offices";
    //  SQL += " WHERE OfficeKey = " + db.SQLLit(OfficeKey);
    //  db.ExecuteSQL(SQL);
    //  #endregion

    //  //db.Invalidate_Office(OfficeKey);
    //}

    #endregion Dead code


  }
}
