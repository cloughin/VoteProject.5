using System;
using System.Data;
using System.Web.UI.WebControls;
using DB.Vote;

namespace Vote.Combine2Politicians
{
  public partial class PoliticiansConsolidatePage : SecureAdminPage
  {
    //#region from db

    //private static string Offices_Str_Optional(string officeKey, string column)
    //{
    //  return officeKey != string.Empty
    //    ? db.Single_Key_Str_Optional("Offices", column, "OfficeKey", officeKey)
    //    : string.Empty;
    //}

    //private static void PoliticiansImages_Consolidate_Image_Column(string politicianKeyFrom, 
    //  string politicianKeyTo, string columnName)
    //{
    //  var column = PoliticiansImagesBlobs.GetColumn(columnName);
    //  var imageBlobFrom = PoliticiansImagesBlobs.GetColumn(column, politicianKeyFrom) as byte[];
    //  var imageBlobTo = PoliticiansImagesBlobs.GetColumn(column, politicianKeyTo) as byte[];

    //  //only if FROM has image and TO is null
    //  if (imageBlobFrom == null || imageBlobTo != null) return;

    //  PoliticiansImagesData.GuaranteePoliticianKeyExists(politicianKeyTo);
    //  PoliticiansImagesBlobs.GuaranteePoliticianKeyExists(politicianKeyTo);
    //  PoliticiansImagesBlobs.UpdateColumn(column, imageBlobFrom, politicianKeyTo);
    //}

    //private static DateTime Politicians_Date(string politicianKey, string column)
    //{
    //  return db.Single_Key_Date("Politicians", column, "PoliticianKey", politicianKey);
    //}

    //private static byte[] PoliticiansImagesImage(string politicianKey, string columnName)
    //{
    //  var column = PoliticiansImagesBlobs.GetColumn(columnName);
    //  return PoliticiansImagesBlobs.GetColumn(column, politicianKey) as byte[];
    //}

    //private static byte[] PoliticiansImagesProfileOriginal(string politicianKey)
    //{
    //  return PoliticiansImagesImage(politicianKey, "ProfileOriginal");
    //}

    //private static bool IsValidPoliticiansImages(string politicianKey)
    //{
    //  return PoliticiansImagesData.PoliticianKeyExists(politicianKey);
    //}

    //private static bool IsValidPoliticiansImagesProfileOriginal(string politicianKey)
    //{
    //  return IsValidPoliticiansImages(politicianKey) && 
    //    PoliticiansImagesProfileOriginal(politicianKey) != null;
    //}

    //private static bool IsUpdatePoliticianToData(DateTime fromDate, bool isHasDataFrom, 
    //  DateTime toDate, bool isHasDataTo)
    //{
    //  if (!isHasDataFrom) return false;
    //  return !isHasDataTo || fromDate > toDate;
    //}

    //private static void PoliticiansColumnConsolidate(
    //  string politicianKeyFrom,
    //  string politicianKeyTo,
    //  string column,
    //  bool isEmptySignificant = false 
    //  // for columns like Politician.Address, null indicates no value and
    //  // empty is an actual value that overrides the defaults.
    //  )
    //{
    //  var dataFrom = db.Politicians_Str(politicianKeyFrom, column);
    //  var dataTo = db.Politicians_Str(politicianKeyTo, column);

    //  var dateFrom = Politicians_Date(politicianKeyFrom, "DataLastUpdated");
    //  var dateTo = Politicians_Date(politicianKeyTo, "DataLastUpdated");

    //  var isHasDataFrom = !string.IsNullOrEmpty(dataFrom) || 
    //    isEmptySignificant && dataFrom == string.Empty;

    //  var isHasDataTo = !string.IsNullOrEmpty(dataTo) || 
    //    isEmptySignificant && dataTo == string.Empty;

    //  if (IsUpdatePoliticianToData(dateFrom, isHasDataFrom, dateTo, isHasDataTo))
    //    db.Politicians_Update_Str(politicianKeyTo, column, dataFrom);
    //}

    //private static void ConsolidatePoliticians(string politicianKeyFrom, string politicianKeyTo)
    //{
    //  string sqlupdate;
    //  var stateCodeTo = Politicians.GetStateCode(politicianKeyTo);

    //  // Update Receiving Politician's Data

    //  // Politicians Table
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "EmailAddrVoteUSA");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "EmailAddr", true);
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "StateEmailAddr");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LastEmailCode");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "WebAddr", true);
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "StateWebAddr");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Phone", true);
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "StatePhone");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Gender");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "PartyKey");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Address", true);
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "CityStateZip", true);
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "StateAddress");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "StateCityStateZip");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "CampaignName");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "CampaignAddr");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "CampaignCityStateZip");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "CampaignPhone");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "CampaignEmail");
    //  PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "StateData");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSStateCode");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSTypeCode");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSDistrictCode");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSLegIDNum");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSPoliticianName");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSEmailAddr");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSWebAddr");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSPhone");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSGender");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSPartyCode");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSAddress");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSCityStateZip");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "LDSVersion");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "GeneralStatement");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Personal");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Education");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Profession");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Military");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Civic");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Political");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Religion");
    //  //PoliticiansColumnConsolidate(politicianKeyFrom, politicianKeyTo, "Accomplishments");

    //  // Last Updated
    //  var dateFrom = Politicians_Date(politicianKeyFrom, "DataLastUpdated");
    //  var dateTo = Politicians_Date(politicianKeyTo, "DataLastUpdated");
    //  if (dateFrom > dateTo)
    //    db.Politicians_Update_Date(politicianKeyTo, "DataLastUpdated", dateFrom);

    //  // PoliticiansImages Table
    //  //From politician row must exist in PoliticiansImages Table
    //  //TO politician does not have to exist because it
    //  //is created on the fly.
    //  if (IsValidPoliticiansImages(politicianKeyFrom))
    //  {
    //    // ProfileOriginal
    //    var imageBlobFrom = PoliticiansImagesBlobs.GetProfileOriginal(politicianKeyFrom);
    //    dateFrom = PoliticiansImagesData.GetProfileOriginalDate(politicianKeyFrom, DateTime.MinValue);

    //    byte[] imageBlobTo;
    //    if (IsValidPoliticiansImagesProfileOriginal(politicianKeyTo))
    //    {
    //      imageBlobTo = PoliticiansImagesBlobs.GetProfileOriginal(politicianKeyTo);
    //      dateTo = PoliticiansImagesData.GetProfileOriginalDate(politicianKeyTo, DateTime.MinValue);
    //    }
    //    else
    //    {
    //      imageBlobTo = null;
    //      dateTo = DateTime.MinValue;
    //    }

    //    var hasDataFrom = imageBlobFrom != null;
    //    var isHasDataTo = imageBlobTo != null;

    //    if (IsUpdatePoliticianToData(dateFrom, hasDataFrom, dateTo, isHasDataTo))
    //    {
    //      // Consolidate ProfileOriginal
    //      PoliticiansImages_Consolidate_Image_Column(politicianKeyFrom, politicianKeyTo, 
    //        "ProfileOriginal");

    //      PoliticiansImagesData.GuaranteePoliticianKeyExists(politicianKeyTo);
    //      PoliticiansImagesBlobs.GuaranteePoliticianKeyExists(politicianKeyTo);

    //      var profileOriginalDate =
    //        PoliticiansImagesData.GetProfileOriginalDate(politicianKeyFrom, DateTime.MinValue);
    //      PoliticiansImagesData.UpdateProfileOriginalDate(profileOriginalDate, politicianKeyTo);

    //      //Profile300
    //      PoliticiansImages_Consolidate_Image_Column(politicianKeyFrom, politicianKeyTo, "Profile300");
    //      //Profile200
    //      PoliticiansImages_Consolidate_Image_Column(politicianKeyFrom, politicianKeyTo, "Profile200");
    //    }

    //    // Last Updated
    //    dateFrom = PoliticiansImagesData.GetProfileOriginalDate(politicianKeyFrom, DateTime.MinValue);
    //    dateTo = PoliticiansImagesData.GetProfileOriginalDate(politicianKeyTo, DateTime.MinValue);
    //    if (dateFrom > dateTo)
    //      PoliticiansImagesData.UpdateProfileOriginalDate(dateFrom, politicianKeyTo);

    //    CommonCacheInvalidation.ScheduleInvalidation("politicianimage", politicianKeyTo);
    //  }

    //  // Answers Table
    //  var sql = "SELECT * FROM Answers WHERE PoliticianKey = " + db.SQLLit(politicianKeyFrom);
    //  var table = db.Table(sql);
    //  foreach (DataRow row in table.Rows)
    //  {
    //    //Consolidating into politician has no answer for this question
    //    if (db.Rows("Answers", "PoliticianKey", politicianKeyTo, "QuestionKey", row["QuestionKey"].ToString()) == 0)
    //    {
    //      sqlupdate = "UPDATE Answers"
    //      + " SET PoliticianKey =" + db.SQLLit(politicianKeyTo)
    //      + " WHERE PoliticianKey =" + db.SQLLit(politicianKeyFrom)
    //      + " AND QuestionKey =" + db.SQLLit(row["QuestionKey"].ToString());
    //      db.ExecuteSql(sqlupdate);
    //    }
    //  }

    //  // ElectionsPoliticians Table
    //  sqlupdate = "UPDATE ElectionsPoliticians"
    //   + " SET PoliticianKey =" + db.SQLLit(politicianKeyTo)
    //   + " ,StateCode =" + db.SQLLit(stateCodeTo)
    //   + " WHERE PoliticianKey =" + db.SQLLit(politicianKeyFrom);
    //  db.ExecuteSql(sqlupdate);

    //  sqlupdate = "UPDATE ElectionsPoliticians"
    //   + " SET RunningMateKey =" + db.SQLLit(politicianKeyTo)
    //   + " WHERE RunningMateKey =" + db.SQLLit(politicianKeyFrom);
    //  db.ExecuteSql(sqlupdate);

    //  // OfficesOfficials Table
    //  sqlupdate = "UPDATE OfficesOfficials"
    //   + " SET PoliticianKey =" + db.SQLLit(politicianKeyTo)
    //   + " ,StateCode =" + db.SQLLit(stateCodeTo)
    //   + " WHERE PoliticianKey =" + db.SQLLit(politicianKeyFrom);
    //  db.ExecuteSql(sqlupdate);

    //  sqlupdate = "UPDATE OfficesOfficials"
    //   + " SET RunningMateKey =" + db.SQLLit(politicianKeyTo)
    //   + " WHERE RunningMateKey =" + db.SQLLit(politicianKeyFrom);
    //  db.ExecuteSql(sqlupdate);

    //  DB.VoteLog.LogElectionPoliticianAddsDeletes.UpdateStateCodeByPoliticianKey(stateCodeTo, politicianKeyFrom);
    //  DB.VoteLog.LogElectionPoliticianAddsDeletes.UpdatePoliticianKeyByPoliticianKey(politicianKeyTo, politicianKeyFrom);

    //  DB.VoteLog.LogPoliticianAdds.UpdateStateCodeByPoliticianKey(stateCodeTo, politicianKeyFrom);
    //  DB.VoteLog.LogPoliticianAdds.UpdatePoliticianKeyByPoliticianKey(politicianKeyTo, politicianKeyFrom);

    //  DB.VoteLog.LogPoliticianChanges.UpdatePoliticianKeyByPoliticianKey(politicianKeyTo, politicianKeyFrom);

    //  // LogPoliticianAnswers Table
    //  DB.VoteLog.LogPoliticianAnswers.UpdatePoliticianKeyByPoliticianKey(politicianKeyTo, politicianKeyFrom);

    //  db.Politician_Delete(politicianKeyFrom);
    //}

    //#endregion from db

    //private void Check_Textboxes_For_Consolidate_One()
    //{
    //  // Consolidate and Delete
    //  if (Textbox_PoliticianKey_From.Text.Trim() == string.Empty)
    //    throw new ApplicationException("The textbox for the first politician Id is empty.");
    //  if (!Politicians.IsValid(Textbox_PoliticianKey_From.Text.Trim()))
    //    throw new ApplicationException("The first politician Id is invalid.");

    //  // Into this
    //  if (Textbox_PoliticianKey_To.Text.Trim() == string.Empty)
    //    throw new ApplicationException("The textbox for the first politician Id is empty.");
    //  if (!Politicians.IsValid(Textbox_PoliticianKey_To.Text.Trim()))
    //    throw new ApplicationException("The first politician Id is invalid.");

    //}

    //private void Check_Politicians_In_Same_State_One()
    //{
    //  if (!IsMasterUser)
    //  {
    //    // Same State for Administrators
    //    if (Politicians.GetStateCodeFromKey(Textbox_PoliticianKey_From.Text.Trim())
    //       != Politicians.GetStateCodeFromKey(Textbox_PoliticianKey_To.Text.Trim()))
    //      throw new ApplicationException(
    //        "The politicians are not in the same State."
    //        );


    //    if (Politicians.GetStateCodeFromKey(Textbox_PoliticianKey_From.Text.Trim())
    //    != ViewState["StateCode"].ToString())
    //      throw new ApplicationException(
    //        "The politician does not belong to "
    //        + StateCache.GetStateName(ViewState["StateCode"].ToString())
    //        );
    //  }
    //}

    //private void Check_Textboxes_For_Show_Two()
    //{
    //  // First Politician Id:
    //  if (TextBox_Id_First.Text.Trim() == string.Empty)
    //    throw new ApplicationException("The textbox for the first politician Id is empty.");
    //  if (!Politicians.IsValid(TextBox_Id_First.Text.Trim()))
    //    throw new ApplicationException("The first politician Id is invalid.");

    //  // Second Politician Id:
    //  if (
    //    (TextBox_Id_Second.Text.Trim() != string.Empty)
    //    && (!Politicians.IsValid(TextBox_Id_Second.Text.Trim()))
    //    )
    //    throw new ApplicationException("The second politician Id is invalid.");
    //}

    //private void Check_Politicians_In_Same_State_Two()
    //{
    //  if (!IsMasterUser)
    //  {
    //    // Same State for Administrators
    //    if (TextBox_Id_Second.Text.Trim() != string.Empty)
    //    {
    //      if (Politicians.GetStateCodeFromKey(TextBox_Id_First.Text.Trim())
    //        != Politicians.GetStateCodeFromKey(TextBox_Id_Second.Text.Trim()))
    //        throw new ApplicationException(
    //          "The politicians are not in the same State."
    //          );
    //    }

    //    if (Politicians.GetStateCodeFromKey(TextBox_Id_First.Text.Trim())
    //    != ViewState["StateCode"].ToString())
    //      throw new ApplicationException(
    //        "The politician does not belong to "
    //        + StateCache.GetStateName(ViewState["StateCode"].ToString())
    //        );
    //  }
    //}

    //private void CheckTextBoxs4HtmlAndIlleagalInput()
    //{
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxFirst);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxMiddle);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxNickName);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxLast);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxSuffix);
    //  db.Throw_Exception_TextBox_Html_Or_Script(Textbox_PoliticianKey_From);
    //  db.Throw_Exception_TextBox_Html_Or_Script(Textbox_PoliticianKey_To);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBox_Id_First);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBox_Id_Second);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBox_OfficeKey_New);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBox_FName_New);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBox_Middle_New);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBox_NickName_Last);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBox_LName_New);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBox_Suffix_New);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBox_StateNew);
    //}

    //private void Reset_For_Next_Consolidate()
    //{
    //  TextBoxFirst.Text = string.Empty;
    //  TextBoxMiddle.Text = string.Empty;
    //  TextBoxNickName.Text = string.Empty;
    //  TextBoxLast.Text = string.Empty;
    //  TextBoxSuffix.Text = string.Empty;

    //  Textbox_PoliticianKey_From.Text = string.Empty;
    //  Textbox_PoliticianKey_To.Text = string.Empty;

    //  TextBox_Id_First.Text = string.Empty;
    //  TextBox_Id_Second.Text = string.Empty;

    //  TextBox_OfficeKey_New.Text = string.Empty;
    //  TextBox_FName_New.Text = string.Empty;
    //  TextBox_Middle_New.Text = string.Empty;
    //  TextBox_NickName_Last.Text = string.Empty;
    //  TextBox_LName_New.Text = string.Empty;
    //  TextBox_Suffix_New.Text = string.Empty;

    //  ViewState["ShowConsolidateOne"] = false;
    //  ViewState["ShowConsolidateTwo"] = false;
    //}

    //private string Sql_Select_FROM_Politicians()
    //{
    //  var sql = string.Empty;
    //  sql += " SELECT ";
    //  sql += " Politicians.PoliticianKey ";
    //  sql += " ,Politicians.StateCode ";
    //  sql += " ,Politicians.FName ";
    //  sql += " ,Politicians.MName ";
    //  sql += " ,Politicians.LName ";
    //  sql += " ,Politicians.Suffix ";
    //  sql += " ,Politicians.Nickname ";
    //  sql += " ,Politicians.PartyKey ";
    //  sql += " ,Politicians.TemporaryOfficeKey ";
    //  sql += " ,Politicians.StateAddress ";
    //  sql += " ,Politicians.StateCityStateZip ";
    //  sql += " ,Politicians.StatePhone ";
    //  sql += " ,Politicians.StateEmailAddr ";
    //  sql += " ,Politicians.StateWebAddr ";
    //  sql += " ,Politicians.Address ";
    //  sql += " ,Politicians.CityStateZip ";
    //  sql += " ,Politicians.Phone ";
    //  sql += " ,Politicians.EmailAddr ";
    //  sql += " ,Politicians.WebAddr ";
    //  //sql += " ,Politicians.LDSAddress ";
    //  //sql += " ,Politicians.LDSCityStateZip ";
    //  //sql += " ,Politicians.LDSPhone ";
    //  //sql += " ,Politicians.LDSEmailAddr ";
    //  //sql += " ,Politicians.LDSWebAddr ";
    //  //sql += " ,Politicians.LDSLegIDNum ";
    //  sql += " ,Politicians.DataLastUpdated ";
    //  sql += " FROM Politicians ";

    //  return sql;
    //}

    //private string Sql_Politicians_To_Find()
    //{
    //  var sql = Sql_Select_FROM_Politicians();
    //  sql += " WHERE Politicians.LName = " + db.SQLLit(TextBoxLast.Text.Trim());
    //  if (TextBoxFirst.Text.Trim() != string.Empty)
    //    sql += " AND Politicians.FName = " + db.SQLLit(TextBoxFirst.Text.Trim());
    //  if (TextBoxMiddle.Text.Trim() != string.Empty)
    //    sql += " AND Politicians.MName = " + db.SQLLit(TextBoxMiddle.Text.Trim());
    //  if (TextBoxNickName.Text.Trim() != string.Empty)
    //    sql += " AND Politicians.Nickname = " + db.SQLLit(TextBoxNickName.Text.Trim());
    //  if (TextBoxSuffix.Text.Trim() != string.Empty)
    //    sql += " AND Politicians.Suffix = " + db.SQLLit(TextBoxSuffix.Text.Trim());
    //  else if ((ViewState["StateCode"].ToString() != "U1")
    //      && (ViewState["StateCode"].ToString() != "U2")
    //      && (ViewState["StateCode"].ToString() != "U3"))
    //    sql += " AND Politicians.StateCode = " + db.SQLLit(TextBox_State.Text.Trim());
    //  sql += " ORDER BY Politicians.FName,Politicians.MName";

    //  return sql;
    //}

    //private string Sql_Politicians_To_Consolidate_One()
    //{
    //  var sql = Sql_Select_FROM_Politicians();
    //  sql += " WHERE Politicians.PoliticianKey = " + db.SQLLit(Textbox_PoliticianKey_From.Text.Trim());
    //  sql += " OR Politicians.PoliticianKey = " + db.SQLLit(Textbox_PoliticianKey_To.Text.Trim());

    //  return sql;
    //}

    //private string Sql_Politicians_To_Consolidate_Two()
    //{
    //  var sql = Sql_Select_FROM_Politicians();
    //  sql += " WHERE Politicians.PoliticianKey = " + db.SQLLit(TextBox_Id_First.Text.Trim());

    //  if (TextBox_Id_Second.Text.Trim() != string.Empty)
    //    sql += " OR Politicians.PoliticianKey = " + db.SQLLit(TextBox_Id_Second.Text.Trim());

    //  return sql;
    //}

    //private string Sql_Politicians_Consolidated(string politicianKeyNew)
    //{
    //  var sql = Sql_Select_FROM_Politicians();
    //  sql += " WHERE Politicians.PoliticianKey = " + db.SQLLit(politicianKeyNew);

    //  return sql;
    //}

    //private void Show_Politicians(DataTable politiciansTable)
    //{
    //  var politiciansHTMLTable = string.Empty;
    //  politiciansHTMLTable += "<table cellspacing=0 cellpadding=0>";

    //  // Heading
    //  politiciansHTMLTable += "<tr>";

    //  politiciansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //  politiciansHTMLTable += "ID<br>Politician (Party)<br>Last Updated<br>Picture";
    //  politiciansHTMLTable += "</td>";

    //  politiciansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //  politiciansHTMLTable += "Office";
    //  politiciansHTMLTable += "</td>";

    //  politiciansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //  politiciansHTMLTable += "State Information";
    //  politiciansHTMLTable += "</td>";

    //  politiciansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //  politiciansHTMLTable += "Politician Information";
    //  politiciansHTMLTable += "</td>";

    //  politiciansHTMLTable += "<td align=center class=tdReportGroupHeading>";
    //  politiciansHTMLTable += "Other";
    //  politiciansHTMLTable += "</td>";

    //  politiciansHTMLTable += "</tr>";

    //  if (politiciansTable.Rows.Count > 0)
    //  {
    //    foreach (DataRow politicianRow in politiciansTable.Rows)
    //    {
    //      var politicianKey = politicianRow["PoliticianKey"].ToString();
    //      var officeKey =
    //        PageCache.Politicians.GetOfficeKey(politicianKey);

    //      // Politicians with same last name
    //      politiciansHTMLTable += "<tr>";

    //      politiciansHTMLTable += "<td valign=top class=tdReportDetail>";

    //      politiciansHTMLTable += "<strong>" + politicianKey + "</strong>";

    //      politiciansHTMLTable += "<br>" + db.Anchor_Intro(
    //         politicianKey,
    //         Politicians.GetFormattedName(politicianKey),
    //         Politicians.GetFormattedName(politicianKey) + " Introduction Page", 
    //         "view");

    //      if (politicianRow["PartyKey"].ToString() != string.Empty)
    //        politiciansHTMLTable += " (" + db.Parties_Str(politicianRow["PartyKey"].ToString(), "PartyCode") + ")";

    //      politiciansHTMLTable += "<br>"
    //        + Convert.ToDateTime(politicianRow["DataLastUpdated"]).ToString("MM/dd/yyyy");

    //      politiciansHTMLTable += "<br>" + 
    //        db.AnchorPoliticianImageOrNoPhoto(
    //          UrlManager.GetIntroPageUri(politicianKey).ToString(), 
    //          politicianKey,
    //          db.Image_Size_50_Headshot,
    //          Politicians.GetFormattedName(politicianKey) + " Introduction Page",
    //          //"_edit2"
    //          "_intro");

    //      politiciansHTMLTable += "</td>";

    //      // Office <td>
    //      politiciansHTMLTable += "<td valign=top class=tdReportDetail>";
    //      if (Offices_Str_Optional(officeKey, "OfficeLine1") != string.Empty)
    //      {
    //        politiciansHTMLTable += Offices_Str_Optional(officeKey, "OfficeLine1");
    //        politiciansHTMLTable += " " + Offices_Str_Optional(officeKey, "OfficeLine2");
    //      }
    //      else
    //        politiciansHTMLTable += "No Office Identified for Politician";

    //      //					PoliticiansHTMLTable += " " + PoliticianRow["OfficeLine2"].ToString();
    //      politiciansHTMLTable += "</td>";

    //      // State Data
    //      politiciansHTMLTable += "<td valign=top class=tdReportDetail>";
    //      if (politicianRow["StateAddress"].ToString() != string.Empty)
    //      {
    //        politiciansHTMLTable += politicianRow["StateAddress"] + "<br>";
    //        politiciansHTMLTable += politicianRow["StateCityStateZip"] + "<br>";
    //      }
    //      else
    //        politiciansHTMLTable += "&nbsp;";

    //      if (politicianRow["StatePhone"].ToString() != string.Empty)
    //        politiciansHTMLTable += politicianRow["StatePhone"] + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      if (politicianRow["StateEmailAddr"].ToString() != string.Empty)
    //        politiciansHTMLTable += politicianRow["StateEmailAddr"] + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      if (politicianRow["StateWebAddr"].ToString() != string.Empty)
    //        politiciansHTMLTable += politicianRow["StateWebAddr"] + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      politiciansHTMLTable += "</td>";

    //      // Politician Data
    //      politiciansHTMLTable += "<td valign=top class=tdReportDetail>";
    //      if (politicianRow["Address"].ToString() != string.Empty)
    //      {
    //        politiciansHTMLTable += politicianRow["Address"] + "<br>";
    //        politiciansHTMLTable += politicianRow["CityStateZip"] + "<br>";
    //      }
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      if (politicianRow["Phone"].ToString() != string.Empty)
    //        politiciansHTMLTable += politicianRow["Phone"] + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      if (politicianRow["EmailAddr"].ToString() != string.Empty)
    //        politiciansHTMLTable += politicianRow["EmailAddr"] + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      if (politicianRow["WebAddr"].ToString() != string.Empty)
    //        politiciansHTMLTable += politicianRow["WebAddr"] + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      politiciansHTMLTable += "</td>";

    //      // Other Data
    //      politiciansHTMLTable += "<td valign=top class=tdReportDetail>";
    //      //if (politicianRow["LDSAddress"].ToString() != string.Empty)
    //      //{
    //      //  politiciansHTMLTable += politicianRow["LDSAddress"] + "<br>";
    //      //  politiciansHTMLTable += politicianRow["LDSCityStateZip"] + "<br>";
    //      //}
    //      //else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (politicianRow["LDSPhone"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += politicianRow["LDSPhone"] + "<br>";
    //      //else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (politicianRow["LDSEmailAddr"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += politicianRow["LDSEmailAddr"] + "<br>";
    //      //else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (politicianRow["LDSWebAddr"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += politicianRow["LDSWebAddr"] + "<br>";
    //      //else
    //        politiciansHTMLTable += "&nbsp;";
    //      politiciansHTMLTable += "</td>";

    //      politiciansHTMLTable += "</tr>";
    //    }
    //  }
    //  else
    //  {
    //    // No politicians in the State have the same last name
    //    politiciansHTMLTable += "<tr>";
    //    politiciansHTMLTable += "<td colspan=5>";
    //    politiciansHTMLTable += "<span class=tdReportDetail>";
    //    politiciansHTMLTable += "No politicians in the database have the same last name.";
    //    politiciansHTMLTable += "</span>";
    //    politiciansHTMLTable += "</td>";
    //    politiciansHTMLTable += "</tr>";
    //  }
    //  politiciansHTMLTable += "</table>";
    //  HTMLTablePoliticians.Text = politiciansHTMLTable;
    //}

    //private void Show_Politicians_Found()
    //{
    //  var politiciansTable = db.Table(Sql_Politicians_To_Find());
    //  Show_Politicians(politiciansTable);
    //}

    //private void Show_Politicians_To_Consolidate_One()
    //{
    //  var politiciansTable = db.Table(Sql_Politicians_To_Consolidate_One());
    //  Show_Politicians(politiciansTable);
    //}

    //private void Show_Politicians_To_Consolidate_Two()
    //{
    //  var politiciansTable = db.Table(Sql_Politicians_To_Consolidate_Two());
    //  Show_Politicians(politiciansTable);
    //}

    //private void Show_Politicians_Consolidated(string politicianKeyNew)
    //{
    //  var politiciansTable = db.Table(Sql_Politicians_Consolidated(politicianKeyNew));
    //  Show_Politicians(politiciansTable);
    //}

    //private void Consolidate_Politicians(TextBox textboxPoliticianKeyFrom, 
    //  TextBox textboxPoliticianKeyTo)
    //{
    //  ConsolidatePoliticians(
    //    textboxPoliticianKeyFrom.Text.Trim()
    //    , textboxPoliticianKeyTo.Text.Trim()
    //    );
    //}

    //protected void Button_Show_For_Name_Parts_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxs4HtmlAndIlleagalInput();

    //    if (TextBoxLast.Text.Trim() == string.Empty)
    //      throw new ApplicationException("Politician Last Name needs to be entered in the text box provided.");

    //    Show_Politicians_Found();

    //    Msg.Text = db.Msg("These are the politicians' data meeting the name criterion you entered.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}

    //protected void Button_Show_Consolidate_One_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxs4HtmlAndIlleagalInput();

    //    Check_Textboxes_For_Consolidate_One();

    //    Check_Politicians_In_Same_State_One();

    //    Show_Politicians_To_Consolidate_One();

    //    ViewState["ShowConsolidateOne"] = true;

    //    Msg.Text = db.Msg("These are the two politicians' data for the Id's entered.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Button_Consolidate_One_Into_Another_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxs4HtmlAndIlleagalInput();

    //    Check_Textboxes_For_Consolidate_One();

    //    Check_Politicians_In_Same_State_One();

    //    if (!Convert.ToBoolean(ViewState["ShowConsolidateOne"]))
    //      throw new ApplicationException(
    //        "The Show Both Politicians's Data Button needs to be clicked first.");

    //    Consolidate_Politicians(
    //      Textbox_PoliticianKey_From
    //      , Textbox_PoliticianKey_To
    //      );

    //    Show_Politicians_To_Consolidate_One();

    //    Msg.Text = db.Ok(
    //      "Politician with Id: " + Textbox_PoliticianKey_From.Text.Trim()
    //      + " was consolidated INTO: "
    //      + Politicians.GetFormattedName(Textbox_PoliticianKey_To.Text.Trim())
    //      + " - Id: " + Textbox_PoliticianKey_To.Text.Trim()
    //      + ", as shown in the report below.");

    //    Reset_For_Next_Consolidate();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Button_Show_Consolidate_Two_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxs4HtmlAndIlleagalInput();
    //    Check_Textboxes_For_Show_Two();
    //    Check_Politicians_In_Same_State_Two();

    //    var politicianKey1 = TextBox_Id_First.Text.Trim();

    //    Label_OfficeKey_First.Text = Politicians.GetTemporaryOfficeKey(politicianKey1, string.Empty);
    //    Label_State_First.Text = Politicians.GetStateCodeFromKey(politicianKey1);
    //    Label_FName_First.Text = Politicians.GetFirstName(politicianKey1, string.Empty);
    //    Label_Middle_First.Text = Politicians.GetMiddleName(politicianKey1, string.Empty);
    //    Label_NickName_First.Text = Politicians.GetNickname(politicianKey1, string.Empty);
    //    Label_LName_First.Text = Politicians.GetLastName(politicianKey1, string.Empty);
    //    Label_Suffix_First.Text = Politicians.GetSuffix(politicianKey1, string.Empty);

    //    var politicianKey2 = TextBox_Id_Second.Text.Trim();
    //    if (politicianKey2 != string.Empty)
    //    {
    //      Label_OfficeKey_Second.Text = Politicians.GetTemporaryOfficeKey(politicianKey2, string.Empty);
    //      Label_State_Second.Text = Politicians.GetStateCodeFromKey(politicianKey2);
    //      Label_FName_Second.Text = Politicians.GetFirstName(politicianKey2, string.Empty);
    //      Label_Middle_Second.Text = Politicians.GetMiddleName(politicianKey2, string.Empty);
    //      Label_NickName_Second.Text = Politicians.GetNickname(politicianKey2, string.Empty);
    //      Label_LName_Second.Text = Politicians.GetLastName(politicianKey2, string.Empty);
    //      Label_Suffix_Second.Text = Politicians.GetSuffix(politicianKey2, string.Empty);
    //    }

    //    TextBox_StateNew.Text = Politicians.GetStateCodeFromKey(politicianKey1);

    //    Show_Politicians_To_Consolidate_Two();

    //    ViewState["ShowConsolidateTwo"] = true;

    //    Msg.Text = db.Msg("These are the two politicians' data for the Id's entered.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Button_Consolidate_Two_Into_New_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxs4HtmlAndIlleagalInput();

    //    Check_Textboxes_For_Show_Two();

    //    if (TextBox_OfficeKey_New.Text.Trim() == string.Empty)
    //      throw new ApplicationException("The textbox for the New Politician OfficeKey is empty.");
    //    if (!Offices.OfficeKeyExists(TextBox_OfficeKey_New.Text.Trim()))
    //      throw new ApplicationException("The New Politician OfficeKey is invalid.");

    //    if (!StateCache.IsValidStateCode(TextBox_StateNew.Text.ToUpper()))
    //      throw new ApplicationException("The StateCode is invalid.");

    //    if (TextBox_FName_New.Text.Trim() == string.Empty)
    //      throw new ApplicationException("The textbox for the Politician first name is empty.");
    //    if (TextBox_LName_New.Text.Trim() == string.Empty)
    //      throw new ApplicationException("The textbox for the Politician last name is empty.");

    //    Check_Politicians_In_Same_State_Two();

    //    if (!Convert.ToBoolean(ViewState["ShowConsolidateTwo"]))
    //      throw new ApplicationException(
    //        "The Show Both Politicians's Data Button"
    //        + " needs to be clicked first.");

    //    var politicianKeyNew = db.Politician_Key(TextBox_StateNew.Text.ToUpper(),
    //      TextBox_LName_New.Text.Trim(), TextBox_FName_New.Text.Trim(), TextBox_Middle_New.Text.Trim(), 
    //      TextBox_Suffix_New.Text.Trim());

    //    //Use same party as first politician
    //    var partyKey = Politicians.GetPartyKey(TextBox_Id_First.Text.Trim(), string.Empty);

    //    if (!Politicians.IsValid(politicianKeyNew))
    //    {
    //      //create the new politician
    //      db.Politician_Insert(politicianKeyNew, TextBox_OfficeKey_New.Text.Trim(), 
    //        TextBox_StateNew.Text.ToUpper(), TextBox_FName_New.Text.Trim(), 
    //        TextBox_Middle_New.Text.Trim(), TextBox_LName_New.Text.Trim(), 
    //        TextBox_Suffix_New.Text.Trim(), string.Empty, TextBox_NickName_Last.Text.Trim(), partyKey);
    //    }
    //    else
    //    {
    //      throw new ApplicationException("The consolidated politician with the newly created PoliticianKey"
    //        + " " + politicianKeyNew + " already exists.");
    //    }

    //    //consolidate the first politician into the new politician
    //    ConsolidatePoliticians(TextBox_Id_First.Text.Trim(), politicianKeyNew);

    //    if (TextBox_Id_Second.Text.Trim() != string.Empty)
    //    {
    //      //consolidate the second politician into the new politician
    //      ConsolidatePoliticians(TextBox_Id_Second.Text.Trim(), politicianKeyNew);
    //    }

    //    //delete the first politician
    //    db.Politician_Delete(TextBox_Id_First.Text.Trim());

    //    if (TextBox_Id_Second.Text.Trim() != string.Empty)
    //    {
    //      //delete the second politician
    //      db.Politician_Delete(TextBox_Id_Second.Text.Trim());
    //    }

    //    Show_Politicians_Consolidated(politicianKeyNew);

    //    var message = "Politician with Id: " + TextBox_Id_First.Text.Trim();

    //    if (TextBox_Id_Second.Text.Trim() != string.Empty)
    //      Msg.Text += " and Politician with Id: " + TextBox_Id_Second.Text.Trim();

    //    Msg.Text += " was consolidated INTO: "
    //    + Politicians.GetFormattedName(politicianKeyNew)
    //    + " - Id: " + politicianKeyNew
    //    + ", as shown in the report below.";

    //    Msg.Text = db.Ok(message);

    //    Reset_For_Next_Consolidate();

    //    Label_OfficeKey_First.Text = string.Empty;
    //    Label_State_First.Text = string.Empty;
    //    Label_FName_First.Text = string.Empty;
    //    Label_Middle_First.Text = string.Empty;
    //    Label_NickName_First.Text = string.Empty;
    //    Label_LName_First.Text = string.Empty;
    //    Label_Suffix_First.Text = string.Empty;
    //    Label_OfficeKey_Second.Text = string.Empty;
    //    Label_State_Second.Text = string.Empty;
    //    Label_FName_Second.Text = string.Empty;
    //    Label_Middle_Second.Text = string.Empty;
    //    Label_NickName_Second.Text = string.Empty;
    //    Label_LName_Second.Text = string.Empty;
    //    Label_Suffix_Second.Text = string.Empty;
    //    //Label_State_New.Text = string.Empty;
    //    TextBox_StateNew.Text = string.Empty;
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    ViewState["StateCode"] = db.State_Code();
    //    ViewState["CountyCode"] = db.County_Code();
    //    ViewState["LocalCode"] = db.Local_Code();
    //    //if (!db.Is_User_Security_Ok())
    //    //  HandleSecurityException();

    //    Title = H1.InnerText = "Consolidate Politicians";

    //    try
    //    {
    //      if (IsMasterUser)
    //      {
    //        TextBox_State.Enabled = true;
    //        TextBox_State.Text = string.Empty;
    //        Label_State_First.Text = string.Empty;
    //        Label_State_Second.Text = string.Empty;
    //        TextBox_StateNew.Text = string.Empty;
    //      }
    //      else
    //      {
    //        TextBox_State.Enabled = false;
    //        TextBox_State.Text = ViewState["StateCode"].ToString();
    //        Label_State_First.Text = ViewState["StateCode"].ToString();
    //        Label_State_Second.Text = ViewState["StateCode"].ToString();
    //        TextBox_StateNew.Text = ViewState["StateCode"].ToString();
    //      }

    //      ViewState["ShowConsolidateOne"] = false;
    //      ViewState["ShowConsolidateTwo"] = false;

    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }

    //}

    //protected void Textbox_PoliticianKey_From_TextChanged(object sender, EventArgs e)
    //{

    //}
  }
}
