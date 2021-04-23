using System;
using System.Data;
using System.Globalization;
using DB.Vote;

namespace Vote.Admin
{
  public partial class ReferendumPage : VotePage
  {
    //#region checked

    //private void PageTitle_()
    //{
    //  PageTitle.Text = string.Empty;
    //  PageTitle.Text +=
    //    Offices.GetElectoralClassDescription(ViewState["StateCode"].ToString(),
    //      ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString());

    //  PageTitle.Text += "<br>";
    //  PageTitle.Text += db.PageTitle_Election(ViewState["ElectionKey"].ToString());

    //  PageTitle.Text += "<br>";
    //  PageTitle.Text += "REFERENDUMS AND BALLOT MEASURES IN THIS ELECTION";
    //}

    //private void HtmlTablesVisible4Entry(int referendumCount)
    //{
    //  TableUpdate.Visible = false; //[Update This Referendum]
    //  TableDelete.Visible = false; //Delete This Referendum:
    //  TableAdd.Visible = false; //[Add This Referendum]
    //  TableSelect4Update.Visible = false; //Edit or Delete a Referendum
    //  TableAddNew.Visible = false; //[Clear Texboxes to Add Another Referendum]

    //  if (referendumCount > 0) TableSelect4Update.Visible = true;
    //  switch (ViewState["FormMode"].ToString())
    //  {
    //    case "ReferendumsADD":
    //      TableAdd.Visible = true;
    //      break;
    //    case "ReferendumsUPDATE":
    //      TableUpdate.Visible = true;
    //      TableAddNew.Visible = true;
    //      TableDelete.Visible = true;
    //      break;
    //  }
    //}

    //private void HtmlTablesVisible4NextButton(int referendumCount)
    //{
    //  TableUpdate.Visible = false; //[Update This Referendum]
    //  TableDelete.Visible = false; //Delete This Referendum:
    //  TableAdd.Visible = false; //[Add This Referendum]
    //  TableSelect4Update.Visible = false; //Edit or Delete a Referendum
    //  TableAddNew.Visible = false; //[Clear Texboxes to Add Another Referendum]

    //  if (referendumCount > 0) TableSelect4Update.Visible = true;
    //  TableAdd.Visible = true;
    //}

    //private void HtmlTablesVisible4AddNewButton(int referendumCount)
    //{
    //  TableUpdate.Visible = false; //[Update This Referendum]
    //  TableDelete.Visible = false; //Delete This Referendum:
    //  TableAdd.Visible = false; //[Add This Referendum]
    //  TableSelect4Update.Visible = false; //Edit or Delete a Referendum
    //  TableAddNew.Visible = false; //[Clear Texboxes to Add Another Referendum]

    //  if (referendumCount > 0) TableSelect4Update.Visible = true;
    //  TableAdd.Visible = true;
    //}

    //private void CheckTextBoxes4IlleagelText()
    //{
    //  db.Throw_Exception_TextBox_Script(OrderOnBallot);
    //  db.Throw_Exception_TextBox_Html(OrderOnBallot);

    //  db.Throw_Exception_TextBox_Script(ReferendumTitle);
    //  db.Throw_Exception_TextBox_Html(ReferendumTitle);

    //  db.Throw_Exception_TextBox_Script(ReferendumDesc);
    //  db.Throw_Exception_TextBox_Html(ReferendumDesc);

    //  db.Throw_Exception_TextBox_Script(TextBoxDetailLink);
    //  db.Throw_Exception_TextBox_Html(TextBoxDetailLink);

    //  db.Throw_Exception_TextBox_Script(TextBoxFullTextLink);
    //  db.Throw_Exception_TextBox_Html(TextBoxFullTextLink);

    //  db.Throw_Exception_TextBox_Script(ReferendumDetail);
    //  db.Throw_Exception_TextBox_Html(ReferendumDetail);

    //  db.Throw_Exception_TextBox_Script(ReferendumFullText);
    //  db.Throw_Exception_TextBox_Html(ReferendumFullText);
    //}

    //private void CheckBallotOrder()
    //{
    //  if (!db.Is_Valid_Integer(OrderOnBallot.Text.Trim()))
    //    throw new ApplicationException(
    //      "The Ballot Order needs to be a whole number.");
    //}

    //private void CheckDescription()
    //{
    //  if (ReferendumDesc.Text.Trim() == string.Empty) throw new ApplicationException("The Referendum Description is required.");
    //}

    //private int BallotOrderReferendumNext()
    //{
    //  var sql = string.Empty;
    //  sql += " SELECT ";
    //  sql += " Referendums.ElectionKey ";
    //  sql += " ,Referendums.ReferendumKey";
    //  sql += " ,Referendums.ReferendumTitle ";
    //  sql += " ,Referendums.ReferendumDesc ";
    //  sql += " ,Referendums.OrderOnBallot";
    //  sql += " ,Referendums.IsReferendumTagForDeletion";
    //  sql += " FROM Referendums ";

    //  switch (db.Electoral_Class_Election(ViewState["ElectionKey"].ToString()))
    //  {
    //    case db.ElectoralClass.State:
    //      sql += " WHERE Referendums.ElectionKeyState = " +
    //        db.SQLLit(ViewState["ElectionKey"].ToString());
    //      break;
    //    case db.ElectoralClass.County:
    //      sql += " WHERE Referendums.ElectionKeyCounty = " +
    //        db.SQLLit(ViewState["ElectionKey"].ToString());
    //      break;
    //    case db.ElectoralClass.Local:
    //      sql += " WHERE Referendums.ElectionKeyLocal = " +
    //        db.SQLLit(ViewState["ElectionKey"].ToString());
    //      break;
    //  }
    //  sql += " ORDER BY OrderOnBallot DESC";
    //  var referendumRow = db.Row_First_Optional(sql);
    //  if (referendumRow != null) return Convert.ToInt16(referendumRow["OrderOnBallot"].ToString()) + 10;
    //  return 10;
    //}

    //private int ShowReferendumList()
    //{
    //  var sqlReferendums = string.Empty;
    //  sqlReferendums += " SELECT ";
    //  sqlReferendums += " Referendums.ElectionKey";
    //  sqlReferendums += ",Referendums.ReferendumKey";
    //  sqlReferendums += ",Referendums.ReferendumTitle";
    //  sqlReferendums += ",Referendums.ReferendumDesc";
    //  sqlReferendums += ",Referendums.OrderOnBallot";
    //  sqlReferendums += ",Referendums.IsReferendumTagForDeletion";
    //  sqlReferendums += ",Referendums.IsResultRecorded";
    //  sqlReferendums += ",Referendums.IsPassed";
    //  sqlReferendums += " FROM Referendums";
    //  sqlReferendums += " WHERE Referendums.ElectionKey = " +
    //    db.SQLLit(ViewState["ElectionKey"].ToString());
    //  sqlReferendums += " AND Referendums.IsReferendumTagForDeletion = 0";
    //  sqlReferendums += " ORDER BY OrderOnBallot ASC";
    //  var referendumTable = db.Table(sqlReferendums);

    //  var theReferendumHtmlTable = string.Empty;
    //  theReferendumHtmlTable += "<table cellspacing=0 cellpadding=0>";

    //  theReferendumHtmlTable += "<tr class=trReportGroupHeading>";
    //  theReferendumHtmlTable +=
    //    "<td align=center class=tdReportGroupHeading colspan=2>";
    //  theReferendumHtmlTable += "Ballot Measures";
    //  theReferendumHtmlTable += "</td>";
    //  theReferendumHtmlTable += "</tr>";

    //  theReferendumHtmlTable += "<tr class=trReportDetailHeading>";

    //  theReferendumHtmlTable += "<td align=center class=tdReportDetailHeading>";

    //  theReferendumHtmlTable += "Ballot<br>Order";
    //  theReferendumHtmlTable += "</td>";
    //  theReferendumHtmlTable +=
    //    "<td align=center width=700px class=tdReportDetailHeading>";
    //  theReferendumHtmlTable += "Referendum or Ballot Measure Title";
    //  theReferendumHtmlTable += "</td>";
    //  theReferendumHtmlTable += "</tr>";

    //  if (referendumTable.Rows.Count > 0)
    //    foreach (DataRow referendumRow in referendumTable.Rows)
    //    {
    //      theReferendumHtmlTable += "<tr class=trReportDetail>";

    //      theReferendumHtmlTable += "<td valign=top class=tdReportDetail>";
    //      theReferendumHtmlTable += referendumRow["OrderOnBallot"].ToString();
    //      theReferendumHtmlTable += "</td>";

    //      var referendumAnchor =
    //        db.Anchor_Admin_Referendums(referendumRow["ElectionKey"].ToString(),
    //          referendumRow["ReferendumKey"].ToString());
    //      theReferendumHtmlTable += "<td valign=top class=tdReportDetail>";
    //      theReferendumHtmlTable += referendumAnchor;

    //      if (Convert.ToBoolean(referendumRow["IsReferendumTagForDeletion"])) theReferendumHtmlTable += " [Tagged for Deletion]";

    //      if (db.Is_Election_Previous(ViewState["ElectionKey"].ToString()))
    //        if (!Convert.ToBoolean(referendumRow["IsResultRecorded"])) theReferendumHtmlTable += " - Not Recorded";
    //        else if (Convert.ToBoolean(referendumRow["IsPassed"])) theReferendumHtmlTable += " - Passed";
    //        else theReferendumHtmlTable += " - Failed";

    //      theReferendumHtmlTable += "</td>";

    //      theReferendumHtmlTable += "</tr>";
    //    }
    //  else
    //  {
    //    theReferendumHtmlTable += "<tr>";
    //    theReferendumHtmlTable += "<td colspan=2>";
    //    theReferendumHtmlTable += "<span class=tdReportDetailColor><center>";
    //    theReferendumHtmlTable +=
    //      "No Referndum or Ballot Measures in the database.</center>";
    //    theReferendumHtmlTable += "</span>";
    //    theReferendumHtmlTable += "</td>";
    //    theReferendumHtmlTable += "</tr>";
    //  }
    //  theReferendumHtmlTable += "</table>";
    //  ReferendumHTMLTable.Text = theReferendumHtmlTable;

    //  return referendumTable.Rows.Count;
    //}

    //private void GetReferendumShowInTextboxes(string electionKey,
    //  string referendumKey)
    //{
    //  OrderOnBallot.Text = db.ReferendumsInt(electionKey, referendumKey,
    //    "OrderOnBallot")
    //    .ToString(CultureInfo.InvariantCulture);
    //  RadiobuttonlistDelete.SelectedValue = db.ReferendumsBool(electionKey,
    //    referendumKey, "IsReferendumTagForDeletion")
    //    ? "Yes"
    //    : "No";
    //  ReferendumTitle.Text = db.Referendums(electionKey, referendumKey,
    //    "ReferendumTitle");
    //  ReferendumDesc.Text = db.Referendums(electionKey, referendumKey,
    //    "ReferendumDesc");
    //  ReferendumDetail.Text = db.Referendums(electionKey, referendumKey,
    //    "ReferendumDetail");
    //  ReferendumFullText.Text = db.Referendums(electionKey, referendumKey,
    //    "ReferendumFullText");
    //  TextBoxDetailLink.Text = db.Referendums(electionKey, referendumKey,
    //    "ReferendumDetailUrl");
    //  TextBoxFullTextLink.Text = db.Referendums(electionKey, referendumKey,
    //    "ReferendumFullTextUrl");
    //}

    //private void ClearTextBoxes()
    //{
    //  OrderOnBallot.Text = BallotOrderReferendumNext()
    //    .ToString(CultureInfo.InvariantCulture);
    //  RadiobuttonlistDelete.SelectedValue = "No";
    //  ReferendumTitle.Text = string.Empty;
    //  ReferendumDesc.Text = string.Empty;
    //  ReferendumDetail.Text = string.Empty;
    //  ReferendumFullText.Text = string.Empty;
    //  TextBoxDetailLink.Text = string.Empty;
    //  TextBoxFullTextLink.Text = string.Empty;
    //}

    //protected void RadiobuttonlistDelete_SelectedIndexChanged(object sender,
    //  EventArgs e)
    //{
    //  try
    //  {
    //    db.ReferendumsUpdateBool(ViewState["ElectionKey"].ToString(),
    //      ViewState["ReferendumKey"].ToString(), "IsReferendumTagForDeletion",
    //      RadiobuttonlistDelete.SelectedValue == "Yes");

    //    ShowReferendumList();

    //    GetReferendumShowInTextboxes(ViewState["ElectionKey"].ToString(),
    //      ViewState["ReferendumKey"].ToString());

    //    Msg.Text = db.Ok(ReferendumTitle.Text + " was UPDATED.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonNext_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    var referendumCount = ShowReferendumList();
    //    HtmlTablesVisible4NextButton(referendumCount);

    //    Msg.Text =
    //      db.Msg(
    //        "To add another referendum enter the referendum information in the text boxes provided and click 'Add This'.");

    //    ClearTextBoxes();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonReCase_Click(object sender, EventArgs e)
    //{
    //  CheckTextBoxes4IlleagelText();
    //  ReferendumTitle.Text = db.Str_ReCase(ReferendumTitle.Text);
    //}

    //protected void ButtonDescRemoveCrLf_Click(object sender, EventArgs e)
    //{
    //  CheckTextBoxes4IlleagelText();
    //  ReferendumDesc.Text = ReferendumDesc.Text.ReplaceNewLinesWithSpaces();
    //}

    //protected void ButtonDetailRemoveCrLf_Click(object sender, EventArgs e)
    //{
    //  CheckTextBoxes4IlleagelText();
    //  ReferendumDetail.Text = ReferendumDetail.Text.ReplaceNewLinesWithSpaces();
    //}

    //protected void ButtonFullRemoveCrLf_Click(object sender, EventArgs e)
    //{
    //  CheckTextBoxes4IlleagelText();
    //  ReferendumFullText.Text = ReferendumFullText.Text.ReplaceNewLinesWithSpaces();
    //}

    //protected void ButtonRecaseDescription_Click(object sender, EventArgs e)
    //{
    //  CheckTextBoxes4IlleagelText();
    //  ReferendumDesc.Text = db.Str_ReCase(ReferendumDesc.Text);
    //}

    //protected void ButtonReCaseFullText_Click(object sender, EventArgs e)
    //{
    //  CheckTextBoxes4IlleagelText();
    //  ReferendumDetail.Text = db.Str_ReCase(ReferendumDetail.Text);
    //}

    //protected void ButtonUpdate_Click1(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxes4IlleagelText();
    //    CheckDescription();
    //    CheckBallotOrder();

    //    var stateCode = ViewState["StateCode"].ToString();
    //    var electionKey = ViewState["ElectionKey"].ToString();
    //    var electionDate = db.Elections_ElectionDate(electionKey);

    //    var referendumKey = ViewState["ReferendumKey"].ToString();

    //    //Update all primary elections on the same date
    //    var tableElectionKeys = DataTable_ElectionKeys(stateCode, electionDate);
    //    foreach (DataRow rowElectionKey in tableElectionKeys.Rows)
    //    {
    //      electionKey = rowElectionKey["ElectionKey"].ToString();
    //      referendumKey = Modify_ReferendumKey(electionKey, referendumKey);

    //      db.ReferendumsUpdateInt(electionKey, referendumKey, "OrderOnBallot",
    //        Convert.ToUInt16(OrderOnBallot.Text));
    //      db.ReferendumsUpdate(electionKey, referendumKey, "ReferendumTitle",
    //        ReferendumTitle.Text);
    //      db.ReferendumsUpdate(electionKey, referendumKey, "ReferendumDesc",
    //        ReferendumDesc.Text);
    //      db.ReferendumsUpdate(electionKey, referendumKey, "ReferendumDetail",
    //        ReferendumDetail.Text);
    //      db.ReferendumsUpdate(electionKey, referendumKey, "ReferendumDetailUrl",
    //        db.Str_Remove_Http(TextBoxDetailLink.Text.Trim()));
    //      db.ReferendumsUpdate(electionKey, referendumKey, "ReferendumFullText",
    //        ReferendumFullText.Text);
    //      db.ReferendumsUpdate(electionKey, referendumKey, "ReferendumFullTextUrl",
    //        db.Str_Remove_Http(TextBoxFullTextLink.Text.Trim()));
    //    }

    //    GetReferendumShowInTextboxes(electionKey, referendumKey);

    //    ShowReferendumList();

    //    Msg.Text = db.Ok(ReferendumTitle.Text + " was UPDATED.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //private string Make_ReferendumKey(string electionKey)
    //{
    //  var referendumKey = electionKey;
    //  referendumKey += db.Str_Remove_Non_Key_Chars(ReferendumTitle.Text.Trim());
    //  referendumKey = referendumKey.ReplaceNewLinesWithEmptyString();
    //  if (referendumKey.Length > 148) referendumKey = referendumKey.Substring(0, 148);
    //  return referendumKey;
    //}

    //private static string Modify_ReferendumKey(string electionKey,
    //  string referendumKey)
    //{
    //  referendumKey = referendumKey.Remove(0, electionKey.Length);
    //  referendumKey = referendumKey.Insert(0, electionKey);

    //  referendumKey = referendumKey.ReplaceNewLinesWithEmptyString();
    //  if (referendumKey.Length > 148) referendumKey = referendumKey.Substring(0, 148);
    //  return referendumKey;
    //}

    //private static DataTable DataTable_ElectionKeys(string stateCode,
    //  DateTime electionDate)
    //{
    //  var sql = string.Empty;
    //  sql += "SELECT ElectionKey";
    //  sql += " FROM Elections";
    //  sql += " WHERE StateCode = " + db.SQLLit(stateCode);
    //  sql += " AND ElectionDate = " + db.SQLLit(Convert.ToDateTime(electionDate)
    //    .ToString("yyyyMMdd"));

    //  return db.Table(sql);
    //}

    //protected void ButtonAdd_Click1(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxes4IlleagelText();

    //    if (ReferendumTitle.Text.Trim() == string.Empty) throw new ApplicationException("The Referendum Title is required.");

    //    var stateCode = ViewState["StateCode"].ToString();
    //    var countyCode = ViewState["CountyCode"].ToString();
    //    var localCode = ViewState["LocalCode"].ToString();
    //    var electionKey = ViewState["ElectionKey"].ToString();
    //    var electionDate = db.Elections_ElectionDate(electionKey);
    //    var referendumKey = Make_ReferendumKey(electionKey);

    //    if (Referendums.IsValid(ViewState["ElectionKey"].ToString(), referendumKey))
    //      throw new ApplicationException(
    //        "This Referendum already exists. Change the Short Title if this is truly a new Referendum.");
    //    CheckDescription();
    //    CheckBallotOrder();

    //    int ballotOrder;
    //    if (OrderOnBallot.Text != string.Empty) ballotOrder = Convert.ToInt16(OrderOnBallot.Text);
    //    else if (BallotOrderReferendumNext() != 0) ballotOrder = BallotOrderReferendumNext();
    //    else ballotOrder = 10;

    //    //Update all primary elections on the same date
    //    var tableElectionKeys = DataTable_ElectionKeys(stateCode, electionDate);
    //    foreach (DataRow rowElectionKey in tableElectionKeys.Rows)
    //    {
    //      electionKey = rowElectionKey["ElectionKey"].ToString();
    //      referendumKey = Make_ReferendumKey(electionKey);

    //      var electionKeyState = db.ElectionKey_State(electionKey, stateCode);
    //      var electionKeyCounty = db.ElectionKey_County(electionKey, stateCode,
    //        countyCode);
    //      var electionKeyLocal = db.ElectionKey_Local(electionKey, stateCode,
    //        countyCode, localCode);

    //      var insertSQL = "INSERT INTO Referendums" + "(" + " ElectionKey" +
    //        ",ReferendumKey" + ",ElectionKeyState" + ",ElectionKeyCounty" +
    //        ",ElectionKeyLocal" + ",StateCode" + ",CountyCode" + ",LocalCode" +
    //        ",OrderOnBallot" + ",ReferendumTitle" + ",ReferendumDesc" +
    //        ",ReferendumDetail" + ",ReferendumDetailUrl" + ",ReferendumFullText" +
    //        ",ReferendumFullTextUrl" + ")" + " VALUES(" +
    //        db.SQLLit(electionKeyState) + "," + db.SQLLit(referendumKey) + "," +
    //        db.SQLLit(electionKeyState) + "," + db.SQLLit(electionKeyCounty) + "," +
    //        db.SQLLit(electionKeyLocal) + "," + db.SQLLit(stateCode) + "," +
    //        db.SQLLit(countyCode) + "," + db.SQLLit(localCode) + "," + ballotOrder +
    //        "," + db.SQLLit(ReferendumTitle.Text.Trim()) + "," +
    //        db.SQLLit(ReferendumDesc.Text.Trim()) + "," +
    //        db.SQLLit(ReferendumDetail.Text.Trim()) + "," +
    //        db.SQLLit(db.Str_Remove_Http(TextBoxDetailLink.Text.Trim())) + "," +
    //        db.SQLLit(ReferendumFullText.Text.Trim()) + "," +
    //        db.SQLLit(db.Str_Remove_Http(TextBoxFullTextLink.Text.Trim())) + ")";
    //      db.ExecuteSQL(insertSQL);
    //    }

    //    HtmlTablesVisible4AddNewButton(ShowReferendumList());

    //    Msg.Text = db.Ok(ReferendumTitle.Text + " was ADDED");

    //    ClearTextBoxes();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //  ReferendumTitle.Text = ReferendumTitle.Text.ReplaceNewLinesWithSpaces();
    //}

    //protected void ReferendumTitle_TextChanged(object sender, EventArgs e) { }

    //protected void RadioButtonListPassFail_SelectedIndexChanged(object sender,
    //  EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxes4IlleagelText();

    //    switch (RadioButtonListPassFail.SelectedValue)
    //    {
    //      case "Passed":
    //        db.ReferendumsUpdateBool(ViewState["ElectionKey"].ToString(),
    //          ViewState["ReferendumKey"].ToString(), "IsPassed", true);
    //        break;
    //      case "Failed":
    //        db.ReferendumsUpdateBool(ViewState["ElectionKey"].ToString(),
    //          ViewState["ReferendumKey"].ToString(), "IsPassed", false);
    //        break;
    //    }

    //    db.ReferendumsUpdateBool(ViewState["ElectionKey"].ToString(),
    //      ViewState["ReferendumKey"].ToString(), "IsResultRecorded", true);

    //    HtmlTablesVisible4Entry(ShowReferendumList());
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //private void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    ViewState["ElectionKey"] = string.Empty;
    //    ViewState["ReferendumKey"] = string.Empty;

    //    if (!string.IsNullOrEmpty(QueryElection)) ViewState["ElectionKey"] = QueryElection;
    //    if (!string.IsNullOrEmpty(QueryReferendum)) ViewState["ReferendumKey"] = QueryReferendum;

    //    //The Session UserStateCode, UserCountyCode, UserLocalCode can be changed
    //    //by a higher administration level using query strings
    //    //This is done in db.State_Code(), db.County_Code(), db.Local_Code()
    //    //
    //    //Using ViewState variables insures these values won't
    //    //change on any postbacks or in different tab or browser Sessions.
    //    //
    //    //ViewState["StateCode"] can be a StateCode or U1, u2, u3 for FederalCode

    //    ViewState["StateCode"] = db.State_Code();
    //    ViewState["CountyCode"] = db.County_Code();
    //    ViewState["LocalCode"] = db.Local_Code();
    //    if (!db.Is_User_Security_Ok()) SecurePage.HandleSecurityException();

    //    try
    //    {
    //      if (db.Is_Election_Previous(ViewState["ElectionKey"].ToString()))
    //      {
    //        TablePassFail.Visible = true;
    //        RadioButtonListPassFail.Enabled =
    //          !string.IsNullOrEmpty(ViewState["ReferendumKey"].ToString());
    //      }
    //      else TablePassFail.Visible = false;

    //      if (ViewState["ReferendumKey"].ToString() == string.Empty) ViewState["FormMode"] = "ReferendumsADD";
    //      else //a referendum slected from the list of referendums
    //        ViewState["FormMode"] = "ReferendumsUPDATE";

    //      if (ViewState["ReferendumKey"].ToString() != string.Empty)
    //        if (
    //          !Referendums.IsValid(ViewState["ElectionKey"].ToString(),
    //            ViewState["ReferendumKey"].ToString()))
    //          throw new ApplicationException(
    //            "No ReferendumKey in Referendums Table: " +
    //              ViewState["ReferendumKey"]);

    //      if (ViewState["ElectionKey"].ToString() != string.Empty)
    //        if (!db.Is_Valid_Election(ViewState["ElectionKey"].ToString()))
    //          throw new ApplicationException(
    //            "No ElectionID in Elections Table:  " + ViewState["ElectionKey"]);

    //      HtmlTablesVisible4Entry(ShowReferendumList());

    //      //ThePageTitle();
    //      PageTitle_();

    //      if (ViewState["FormMode"].ToString() == "ReferendumsUPDATE")
    //        GetReferendumShowInTextboxes(ViewState["ElectionKey"].ToString(),
    //          ViewState["ReferendumKey"].ToString()); //Update
    //      else ClearTextBoxes();
    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}

    protected override void OnInit(EventArgs e)
    {
      LegacyRedirect(SecureAdminPage.GetUpdateElectionsPageUrl(QueryState,
        QueryCounty, QueryLocal));
      base.OnInit(e);
    }

    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    if (Session["ErrNavBarAdmin"].ToString() != string.Empty) Msg.Text = db.Fail(Session["ErrNavBarAdmin"].ToString());
    //  }
    //  catch /*(Exception ex)*/ { }
    //}

    //#endregion checked

    #region Dead code

    #endregion Dead code
  }
}