namespace Vote
{
// ReSharper disable InconsistentNaming
  public static partial class db
// ReSharper restore InconsistentNaming
  {

    //public static DateTime Elections_Date(
    //  string ElectionKey
    //  , string Column)
    //{
    //  if (ElectionKey != string.Empty)
    //    return db.Single_Key_Date("Elections", Column, "ElectionKey", ElectionKey);
    //  else
    //    return DateTime.MinValue;
    //}

    //#region Referendums SELECT
    //public static string Referendums(string ElectionKey, string ReferendumKey, string Column)
    //{
    //  if (db.Is_Valid_Election(ElectionKey))
    //  {
    //    if (ReferendumKey != string.Empty)
    //      return db.Double_Key_Str("Referendums", Column, "ElectionKey", ElectionKey, "ReferendumKey", ReferendumKey);
    //    else
    //      return string.Empty;
    //  }
    //  else
    //  {
    //    return string.Empty;
    //  }
    //}
    //#endregion

    //public static void Double_Key_Update_Int(string Table, string Column, int ColumnValue, string KeyName1, string KeyValue1, string KeyName2, string KeyValue2)
    //{
    //  string UpdateSQL = "UPDATE " + Table
    //    + " SET " + Column + " = " + ColumnValue.ToString()
    //    + " WHERE " + KeyName1 + " = " + db.SQLLit(KeyValue1)
    //    + " AND " + KeyName2 + " = " + db.SQLLit(KeyValue2);
    //  db.ExecuteSQL(UpdateSQL);
    //}

    //public static void Double_Key_Update_Bool(string Table, string Column, bool ColumnValue, string KeyName1, string KeyValue1, string KeyName2, string KeyValue2)
    //{
    //  string UpdateSQL = "UPDATE " + Table
    //    + " SET " + Column + " = " + Convert.ToUInt16(ColumnValue).ToString()
    //    + " WHERE " + KeyName1 + " = " + db.SQLLit(KeyValue1)
    //    + " AND " + KeyName2 + " = " + db.SQLLit(KeyValue2);
    //  db.ExecuteSQL(UpdateSQL);
    //}

    //public static int Double_Key_Int(string Table, string Column, string KeyName1, string KeyValue1, string KeyName2, string KeyValue2)
    //{
    //  string SQL = "SELECT " + Column.Trim() + " FROM " + Table.Trim()
    //    + " WHERE " + KeyName1.Trim() + " = " + db.SQLLit(KeyValue1.Trim())
    //    + " AND " + KeyName2.Trim() + " = " + db.SQLLit(KeyValue2.Trim());
    //  DataTable table = db.Table(SQL);
    //  if (table.Rows.Count == 1)
    //  {
    //    return (int) table.Rows[0][Column];
    //  }
    //  else
    //  {
    //    throw new ApplicationException(db.DBErrorMsg(SQL, "Did not find a single row"));
    //  }
    //}

    //public static bool Double_Key_Bool(string Table, string Column, string KeyName1, string KeyValue1, string KeyName2, string KeyValue2)
    //{
    //  string SQL = "SELECT " + Column.Trim() + " FROM " + Table.Trim()
    //    + " WHERE " + KeyName1.Trim() + " = " + db.SQLLit(KeyValue1.Trim())
    //    + " AND " + KeyName2.Trim() + " = " + db.SQLLit(KeyValue2.Trim());
    //  DataRow Row = db.Row(SQL);
    //  if (Row != null)
    //  {
    //    return (bool) Row[Column];
    //  }
    //  else
    //  {
    //    throw new ApplicationException(db.DBErrorMsg(SQL, "Did not find a single row"));
    //  }
    //}
    //public static bool Is_Election_Previous(
    //  string electionKey)
    //{
    //  if (db.Elections_Date(electionKey, "ElectionDate") < DateTime.Now)
    //    return true;
    //  else
    //    return false;
    //}

    //public static DateTime Elections_ElectionDate(
    // string ElectionKey)
    //{
    //  return db.Elections_Date(ElectionKey, "ElectionDate");
    //}
    //public static string ElectionKey_Local(string ElectionKey, string StateCode, string CountyCode, string LocalCode)
    //{
    //  if ((ElectionKey != string.Empty) && (StateCode != string.Empty) && (CountyCode != string.Empty) && (LocalCode != string.Empty))
    //  {
    //    return db.ElectionKey_State(ElectionKey, StateCode) + CountyCode + LocalCode;
    //  }
    //  else
    //    return string.Empty;
    //}
    //public static string ElectionKey_County(string ElectionKey, string StateCode, string CountyCode)
    //{
    //  if (
    //    (ElectionKey != string.Empty)
    //    && (StateCode != string.Empty)
    //    && (CountyCode != string.Empty)
    //    )
    //  {
    //    return db.ElectionKey_State(ElectionKey, StateCode) + CountyCode;
    //  }
    //  else
    //    return string.Empty;
    //}

    //#region Referendums UPDATE
    //public static void ReferendumsUpdate(string ElectionKey, string ReferendumKey, string Column, string ColumnValue)
    //{
    //  db.Double_Key_Update_Str("Referendums", Column, ColumnValue, "ElectionKey", ElectionKey, "ReferendumKey", ReferendumKey);
    //}
    //public static void ReferendumsUpdateBool(string ElectionKey, string ReferendumKey, string Column, bool ColumnValue)
    //{
    //  db.Double_Key_Update_Bool("Referendums", Column, ColumnValue, "ElectionKey", ElectionKey, "ReferendumKey", ReferendumKey);
    //}
    //public static void ReferendumsUpdateInt(string ElectionKey, string ReferendumKey, string Column, int ColumnValue)
    //{
    //  db.Double_Key_Update_Int("Referendums", Column, ColumnValue, "ElectionKey", ElectionKey, "ReferendumKey", ReferendumKey);
    //}
    //#endregion
    //public static bool ReferendumsBool(string ElectionKey, string ReferendumKey, string Column)
    //{
    //  if (db.Is_Valid_Election(ElectionKey))
    //  {
    //    if (ReferendumKey != string.Empty)
    //      return db.Double_Key_Bool("Referendums", Column, "ElectionKey", ElectionKey, "ReferendumKey", ReferendumKey);
    //    else
    //      return false;
    //  }
    //  else
    //  {
    //    return false;
    //  }
    //}
    //public static int ReferendumsInt(string ElectionKey, string ReferendumKey, string Column)
    //{
    //  if (db.Is_Valid_Election(ElectionKey))
    //  {
    //    if (ReferendumKey != string.Empty)
    //      //return db.Single_Key_Int("Referendums", Column, "ReferendumKey", ReferendumKey);
    //      return db.Double_Key_Int("Referendums", Column, "ElectionKey", ElectionKey, "ReferendumKey", ReferendumKey);
    //    else
    //      return 0;
    //  }
    //  else
    //  {
    //    return 0;
    //  }
    //}
    //public static string Anchor_Admin_Referendums(string electionKey,
    //  string referendumKey)
    //{
    //  string anchor = string.Empty;
    //  anchor += "<a href=";
    //  anchor += "\"";
    //  anchor += "/Admin/Referendum.aspx";
    //  anchor += "?Election=" + electionKey;
    //  anchor += "&Referendum=" + referendumKey;
    //  anchor += "\"";

    //  anchor += " title=";
    //  anchor += "\"";
    //  anchor += db.Referendums(electionKey, referendumKey, "ReferendumTitle");
    //  anchor += " Description, Details and Full Text";
    //  anchor += "\"";

    //  anchor += ">";

    //  anchor += db.Referendums(electionKey, referendumKey, "ReferendumTitle");
    //  anchor += "</a>";
    //  return anchor;
    //}
    ////------------------------
    //#region Email Addresses & Anchors

    //public static string Politician_Email_Addr_Any_For_Textbox(PageCache cache,
    //  string politicianKey)
    //{
    //  string Email_Addr =
    //    cache.Politicians.GetPublicEmail(politicianKey);
    //  if (!string.IsNullOrEmpty(Email_Addr))
    //  {
    //    Email_Addr = db.Str_Remove_Http(Email_Addr);
    //    Email_Addr = db.Str_Remove_MailTo(Email_Addr);
    //  }
    //  return Email_Addr;
    //}
    //#endregion Email Addresses & Anchors
    ////------------------------
    //public const string No_Email = "no email";

    //public static string Politician_Email_Addr_Any_For_Label_Anchor_Mailto(
    //  PageCache cache, string politicianKey)
    //{
    //  string emailAddress = db.Politician_Email_Addr_Any_For_Textbox(cache,
    //    politicianKey);

    //  if (!string.IsNullOrEmpty(emailAddress))
    //    return db.Anchor_Mailto_Email(emailAddress);
    //  else
    //    return db.No_Email;
    //}

    //public static bool Is_Party_Code_Empty(PageCache cache, string partyKey)
    //{
    //  string partyCode = cache.Parties.GetPartyCode(partyKey);
    //  return string.IsNullOrEmpty(partyCode);
    //  //if (
    //  //  (!db.Is_Valid_Party(PartyKey))
    //  //  || (string.IsNullOrEmpty(db.Parties_PartyCode(PartyKey)))
    //  //  )
    //  //  return true;
    //  //else
    //  //  return false;
    //}

    //public static bool Triple_Key_Bool(string Table, string Column, string KeyName1, string KeyValue1, string KeyName2, string KeyValue2, string KeyName3, string KeyValue3)
    //{
    //  string SQL = "SELECT " + Column.Trim() + " FROM " + Table.Trim()
    //    + " WHERE " + KeyName1.Trim() + " = " + db.SQLLit(KeyValue1.Trim())
    //    + " AND " + KeyName2.Trim() + " = " + db.SQLLit(KeyValue2.Trim())
    //  + " AND " + KeyName3.Trim() + " = " + db.SQLLit(KeyValue3.Trim());
    //  DataRow Row = db.Row(SQL);
    //  if (Row != null)
    //  {
    //    return (bool) Row[Column];
    //  }
    //  else
    //  {
    //    throw new ApplicationException(db.DBErrorMsg(SQL, "Did not find a single row"));
    //  }
    //}
    ////=====================
    //#region /Admin/OfficeWinner.aspx Urls

    //public static string Url_Admin_OfficeWinner()
    //{
    //  return "/Admin/OfficeWinner.aspx";
    //}
    //#endregion /Admin/OfficeWinner.aspx Urls
    ////=====================

    //#region /Admin/OfficeContest.aspx URLs

    //public static string Url_Admin_OfficeContest()
    //{
    //  return "/Admin/OfficeContest.aspx";
    //}

    //#endregion /Admin/OfficeContest.aspx URLs

    //#region ElectionsPoliticians SELECT
    //public static bool ElectionsPoliticians_Bool(
    //  string ElectionKey
    //  , string OfficeKey
    //  , string PoliticianKey
    //  , string Column)
    //{
    //  return db.Triple_Key_Bool("ElectionsPoliticians",
    //    Column,
    //    "ElectionKey", ElectionKey,
    //    "OfficeKey", OfficeKey,
    //    "PoliticianKey", PoliticianKey
    //    );
    //}


    //#endregion
    //public static void ElectionsOffices_Update_Bool(string ElectionKey, string OfficeKey, string Column, bool ColumnValue)
    //{
    //  db.Double_Key_Update_Bool("ElectionsOffices", Column, ColumnValue, "ElectionKey", ElectionKey, "OfficeKey", OfficeKey);
    //}

    //public static bool Is_Election_Upcoming(PageCache cache, string electionKey)
    //{
    //  return cache.Elections.GetElectionDate(electionKey) >= DateTime.Today;
    //}

    //public static string Politician_Email_Addr_Any_For_Label_Anchor_Mailto(
    //  string politicianKey)
    //{
    //  return Politician_Email_Addr_Any_For_Label_Anchor_Mailto(
    //    VotePage.GetPageCache(), politicianKey);
    //}

    //public static string Url_Admin_OfficeWinner(
    //  string electionKey
    //  , string officeKey
    //  , string politicianKey
    //  )
    //{
    //  string url = db.Url_Admin_OfficeWinner();

    //  if (!string.IsNullOrEmpty(electionKey))
    //    url += "&Election=" + electionKey;

    //  if (!string.IsNullOrEmpty(officeKey))
    //    url += "&Office=" + officeKey;

    //  if (!string.IsNullOrEmpty(politicianKey))
    //    url += "&Id=" + politicianKey;

    //  //Url += db.Url_Add_Security_Codes(PoliticianKey);
    //  return db.Fix_Url_Parms(url);
    //}

    //public static string Url_Admin_OfficeContest(
    //  string electionKey
    //  , string officeKey
    //  , string politicianKey
    //  , string mode
    //  )
    //{
    //  string url = db.Url_Admin_OfficeContest();

    //  if (!string.IsNullOrEmpty(electionKey))
    //    url += "&Election=" + electionKey;

    //  if (!string.IsNullOrEmpty(officeKey))
    //    url += "&Office=" + officeKey;

    //  if (!string.IsNullOrEmpty(politicianKey))
    //    url += "&Id=" + politicianKey;

    //  if (!string.IsNullOrEmpty(mode))
    //    url += "&Mode=" + mode;

    //  //Url += db.Url_Add_Security_Codes(PoliticianKey);
    //  return db.Fix_Url_Parms(url);
    //}

    //#region /Parties.aspx URLs & Anchors (ok)

    //public static string Anchor_Party(PageCache cache, string partyKey, string target)
    //{
    //  string anchor;
    //  //if (
    //  //  (PartyKey.ToUpper() == "X")//No Party
    //  //  || (PartyKey.ToUpper() == "N")//Non-Partisan
    //  //  || (PartyKey.Trim() == string.Empty)
    //  //  )
    //  if (db.Is_Party_Code_Empty(cache, partyKey))
    //    // space if X Party Code or empty code
    //    anchor = db.No_PartyCode;
    //  else if (!cache.Parties.Exists(partyKey))
    //    //space if there is no PartyKey
    //    anchor = db.No_PartyCode;
    //  else if (cache.Parties.GetPartyUrl(partyKey).Trim() == string.Empty)//no website url
    //    //if no party url use partyKey as text
    //    anchor = cache.Parties.GetPartyCode(partyKey);
    //  else
    //  {
    //    #region commented out -return an anchor
    //    //Anchor = "<a href=";
    //    //Anchor += "\"";
    //    //Anchor += db.Http();
    //    //Anchor += db.Parties_Str(PartyKey, "PartyURL");
    //    //Anchor += "\"";

    //    //Anchor += "title=";
    //    //Anchor += "\"";
    //    //Anchor += db.Parties_Str(PartyKey, "PartyName") + " Website";
    //    //Anchor += "\"";

    //    //Anchor += " target=_self";
    //    //Anchor += ">";

    //    //Anchor += db.Parties_Str(PartyKey, "PartyCode");

    //    //Anchor += "</a>";
    //    #endregion commented out -return an anchor

    //    anchor = "<a href=";
    //    anchor += "\"";
    //    anchor += VotePage.NormalizeUrl(cache.Parties.GetPartyUrl(partyKey));
    //    anchor += "\"";

    //    anchor += "title=";
    //    anchor += "\"";
    //    anchor += cache.Parties.GetPartyName(partyKey) + " Website";
    //    anchor += "\"";

    //    if (target != string.Empty)
    //    {
    //      anchor += " target=";
    //      anchor += "\"";
    //      anchor += target;
    //      anchor += "\"";
    //    }

    //    anchor += ">";

    //    anchor += cache.Parties.GetPartyCode(partyKey);

    //    anchor += "</a>";

    //    return anchor;
    //  }
    //  return anchor;
    //}
    //#endregion

    //#region Report_Election_Update Methods
    //public static void Add_Td_Party_Code(
    //  PageCache cache,
    //  ref HtmlTableRow htmlTrPolitician,
    //  string politicianKey,
    //  bool isRunningMate)
    //{
    //  string partyCode;
    //  //Running Mate Pary Code always shows because
    //  //some States have Lt Governors of different parties
    //  //Commented out to reinstate
    //  //Running Mates don't show party
    //  //if (!IsRunningMate)
    //  partyCode = db.Anchor_Party(cache,
    //    cache.Politicians.GetPartyKey(politicianKey), "view");
    //  if (string.IsNullOrEmpty(partyCode))
    //    partyCode = "&nbsp";
    //  //To force border on table cell
    //  db.Add_Td_To_Tr(
    //    htmlTrPolitician
    //    , partyCode
    //    , "tdReportDetail"
    //    , "center"
    //    );
    //}

    //public static void Add_Td_Phone(
    //  ref HtmlTableRow htmlTrPolitician
    //  , string politicianKey
    //  , db.ReportUser reportUser
    //  )
    //{
    //  //string Phone = string.Empty;
    //  //if (ReportUser == db.ReportUser.Public)
    //  //  Phone = db.Politician_Phone_Candidate_Or_NA(PoliticianKey);
    //  //else
    //  //  Phone = db.Politician_Phone_State_Or_NA(PoliticianKey);
    //  //Phone = db.Br_First_Blank_After_Position(Phone, db.Position_Break_Phone);//<br> after 20 positions
    //  string phone = db.Politician_Phone_Any_For_Label(
    //    politicianKey
    //  );
    //  db.Add_Td_To_Tr(htmlTrPolitician
    //    , phone
    //    , "tdReportDetail"
    //    //, "center"
    //    );
    //}
    //public static void Add_Td_Address(
    //  ref HtmlTableRow htmlTrPolitician
    //  , string politicianKey
    //  , db.ReportUser reportUser
    //  )
    //{
    //  string streetAddress;
    //  string cityStateZip;
    //  if (reportUser == db.ReportUser.Public)
    //  {
    //    streetAddress = db.Politician_Address_Any_For_Label(
    //      politicianKey
    //      );
    //    cityStateZip = db.Politician_CityStateZip_Any_For_Label(
    //      politicianKey
    //      );
    //  }
    //  else
    //  {
    //    streetAddress = db.Politician_Address_Any_For_Label(
    //      politicianKey
    //      );
    //    cityStateZip = db.Politician_CityStateZip_Any_For_Label(
    //      politicianKey
    //      );
    //  }

    //  db.Add_Td_To_Tr(htmlTrPolitician, streetAddress
    //    + cityStateZip, "tdReportDetail");
    //}

    //public static void Add_Td_Email_WebAddress(
    //  ref HtmlTableRow htmlTrPolitician
    //  , string politicianKey
    //  , db.ReportUser reportUser
    //  )
    //{
    //  #region Email Address

    //  string emailAddr = db.Politician_Email_Addr_Any_For_Label_Anchor_Mailto(
    //    politicianKey);
    //  #endregion Email Address

    //  #region Web Address

    //  string webAddressAnchor = db.Politician_WebAddress_Public_Anchor(
    //    politicianKey
    //    , string.Empty
    //    , Politicians.GetFormattedName(politicianKey) + " Website"
    //    , "view"
    //    );

    //  #endregion Web Address

    //  if (reportUser == db.ReportUser.Public)
    //    db.Add_Td_To_Tr(htmlTrPolitician
    //      , emailAddr + "<br><br>" + webAddressAnchor
    //      , "tdReportDetailBold"
    //      );
    //  else
    //    db.Add_Td_To_Tr(htmlTrPolitician
    //      , emailAddr + "<br>" + webAddressAnchor
    //      , "tdReportDetail"
    //      );
    //}

    //#endregion Report_Election_Update Methods

    //#region 1 Politician
    //public static void Add_Td_Politician_Name(
    //  ref HtmlTableRow htmlTrPolitician
    //  , db.ReportUser reportUser
    //  , string politicianAnchor
    //  )
    //{
    //  if (reportUser == db.ReportUser.Public)
    //    db.Add_Td_To_Tr(
    //    htmlTrPolitician
    //    , politicianAnchor
    //    , "tdReportDetailLargeBold"
    //    );
    //  else
    //    db.Add_Td_To_Tr(
    //    htmlTrPolitician
    //    , politicianAnchor
    //    , "tdReportDetail"
    //    );
    //}

    //#endregion

    //public static void Update_ElectionsOffices_Is_All_Winners_Identified(string ElectionKey, string OfficeKey)
    //{
    //  //True if the number of office positions 
    //  //equals the number of elected officials.
    //  int officePositions = db.Offices_Int(OfficeKey, "Incumbents");
    //  string sql = string.Empty;
    //  sql += "OfficesOfficials";
    //  sql += " WHERE OfficeKey =" + db.SQLLit(OfficeKey);
    //  int electedOfficials = db.Rows_Count_From(sql);

    //  if (officePositions == electedOfficials)
    //    db.ElectionsOffices_Update_Bool(
    //      ElectionKey,
    //      OfficeKey,
    //      "IsWinnerIdentified",
    //      true
    //      );
    //  else
    //    db.ElectionsOffices_Update_Bool(
    //      ElectionKey,
    //      OfficeKey,
    //      "IsWinnerIdentified",
    //      false
    //      );
    //}
    //public static bool Is_Election_Previous_Most_Recent(
    //  string electionKey)
    //{
    //  string SQL = string.Empty;
    //  SQL += "SELECT";
    //  SQL += " ElectionKey";
    //  SQL += " FROM vote.Elections";
    //  SQL += " WHERE ElectionDate < " + db.SQLLit(Db.DbToday);
    //  SQL += " AND StateCode = " + db.SQLLit(Elections.GetStateCodeFromKey(electionKey));
    //  SQL += " ORDER BY ElectionDate DESC";
    //  DataRow rowElection = db.Row_First_Optional(SQL);
    //  if (rowElection["ElectionKey"].ToString() == electionKey)
    //    return true;
    //  else
    //    return false;
    //}

    //public static bool Is_Election_Upcoming(string electionKey)
    //{
    //  return Is_Election_Upcoming(VotePage.GetPageCache(), electionKey);
    //}

    //public static string ElectionKey_State_From_ElectionKey_Federal(string ElectionKey, string StateCode)
    //{
    //  #region replaced
    //  //#region Note
    //  ////Converts Federal ElectionKey_Federal with U1, U2, U3 as StateCode 
    //  ////to State Election ElectionKey_Federal
    //  //#endregion Note
    //  //if (
    //  //  (ElectionKey_Federal != string.Empty)
    //  //  && (db.ElectionKey_Length_Federal == ElectionKey_Federal.Length)
    //  //  && (StateCode.Length == db.ElectionKey_Length_StateCode)
    //  //  )
    //  //{
    //  //  return StateCode
    //  //    + ElectionKey_Federal.Substring(
    //  //      db.ElectionKey_Length_StateCode
    //  //      , ElectionKey_Federal.Length - db.ElectionKey_Length_StateCode);
    //  //}
    //  //else
    //  //  return string.Empty;
    //  #endregion replaced

    //  #region Note
    //  //Converts any ElectionKey into a State ElectionKey
    //  #endregion Note

    //  return StateCode + ElectionKey.Remove(0, 2);
    //}

    //#region Update OfficesOfficials Incumbent
    //public static int Office_ElectedOfficials(string officeKey)
    //{
    //  string sql = " SELECT PoliticianKey FROM OfficesOfficials";
    //  sql += " WHERE OfficeKey = " + db.SQLLit(officeKey);
    //  return db.Rows(sql);
    //}
    //#endregion

    //#region /Admin/OfficeWinner.aspx Anchors
    //public static string Anchor_Admin_OfficeWinner(
    //  string electionKey
    //  , string officeKey
    //  , string anchorText
    //  , string target
    //  , string politicianKey
    //  )
    //{
    //  string Anchor = string.Empty;
    //  Anchor += "<a href=";
    //  Anchor += "\"";
    //  Anchor += db.Url_Admin_OfficeWinner(electionKey, officeKey, politicianKey);
    //  Anchor += "\"";
    //  Anchor += " target=";
    //  Anchor += "\"";
    //  if (target != string.Empty)
    //    Anchor += target;
    //  else
    //    Anchor += "_officewinner";
    //  Anchor += "\"";

    //  if (!string.IsNullOrEmpty(politicianKey))
    //  {
    //    Anchor += " title=";
    //    Anchor += "\"";
    //    Anchor += Politicians.GetFormattedName(politicianKey) + " administration data edit form";
    //    Anchor += "\"";
    //  }

    //  Anchor += ">";

    //  Anchor += anchorText;

    //  Anchor += "</a>";
    //  return Anchor;
    //}

    //#endregion /Admin/OfficeWinner.aspx Anchors
    ////=====================
    //public static string Url_Admin_OfficeContest(
    //  string electionKey
    //  , string officeKey
    //  )
    //{
    //  return db.Url_Admin_OfficeContest(electionKey, officeKey, string.Empty, string.Empty);
    //}

    //public static string Anchor_Admin_Office_UPDATE_Office(
    //  string officeKey
    //  , string anchorText
    //  )
    //{
    //  return db.Anchor_Admin_Office_UPDATE_Office(officeKey, anchorText, string.Empty);
    //}

    //public static bool Is_Incumbent_For_Election(
    //  string politicianKey,
    //  string officeKey,
    //  string electionKey
    //  )
    //{
    //  return db.ElectionsPoliticians_Bool(
    //    electionKey,
    //    officeKey,
    //    politicianKey,
    //    "IsIncumbent");
    //}

    //public static int ElectionsPoliticians_Rows(string ElectionKey, string OfficeKey, string PoliticianKey)
    //{
    //  string SQL = "ElectionsPoliticians";
    //  SQL += " WHERE ElectionKey = " + db.SQLLit(ElectionKey);
    //  SQL += " AND OfficeKey = " + db.SQLLit(OfficeKey);
    //  SQL += " AND PoliticianKey = " + db.SQLLit(PoliticianKey);
    //  return db.Rows_Count_From(SQL);
    //}
    //public static void ElectionsPoliticians_Delete(string ElectionKey, string OfficeKey, string PoliticianKey)
    //{
    //  string SQL = "DELETE FROM ElectionsPoliticians";
    //  SQL += " WHERE ElectionKey = " + db.SQLLit(ElectionKey);
    //  SQL += " AND OfficeKey = " + db.SQLLit(OfficeKey);
    //  SQL += " AND PoliticianKey = " + db.SQLLit(PoliticianKey);
    //  db.ExecuteSQL(SQL);
    //}
    //public static string RunningMateKey(string ElectionKey, string OfficeKey, string PoliticianKey)
    //{
    //  string theRunningMateKey = string.Empty;
    //  if (Offices.IsRunningMateOffice(OfficeKey))
    //  {
    //    string SQL = string.Empty;
    //    SQL += " SELECT ";
    //    SQL += " ElectionsPoliticians.RunningMateKey";
    //    SQL += " FROM ElectionsPoliticians ";
    //    //SQL += " WHERE ElectionsPoliticians.Election = " + db.SQLLit(ElectionKey);
    //    SQL += " WHERE ElectionsPoliticians.ElectionKey = " + db.SQLLit(ElectionKey);
    //    SQL += " AND ElectionsPoliticians.OfficeKey = " + db.SQLLit(OfficeKey);
    //    SQL += " AND ElectionsPoliticians.PoliticianKey = " + db.SQLLit(PoliticianKey);
    //    SQL += " AND ElectionsPoliticians.RunningMateKey != ''";
    //    DataRow ElectionsPoliticiansRow = db.Row_First_Optional(SQL);
    //    if (ElectionsPoliticiansRow != null)
    //      theRunningMateKey = ElectionsPoliticiansRow["RunningMateKey"].ToString();
    //  }
    //  return theRunningMateKey;
    //}

    //public static void Log_ElectionsPoliticians_Changes(string addOrDelete,
    //  string electionKey, string officeKey, string politicianKey)
    //{
    //  DB.VoteLog.LogElectionPoliticianAddsDeletes.Insert(
    //    DateTime.Now,
    //    addOrDelete.Trim(),
    //    SecurePage.UserSecurityClass,
    //    SecurePage.UserName,
    //    electionKey,
    //    politicianKey,
    //    Offices.GetStateCodeFromKey(electionKey),
    //    string.Empty,
    //    string.Empty,
    //    officeKey,
    //    0);
    //}

    //#region ElectionsPoliticians INSERT
    //public static void ElectionsPoliticians_INSERT(
    //  string ElectionKey
    //  , string OfficeKey
    //  , string PoliticianKey
    //  , string DistrictCode
    //  , int OrderOnBallot
    //  )
    //{
    //  #region DistrictCode column needs to be deleted in table
    //  //if (db.Is_Electoral_District(OfficeKey))
    //  //{
    //  //  #region District Offices only
    //  //  if (string.IsNullOrEmpty(DistrictCode))
    //  //    throw new ApplicationException(
    //  //      "The office class requires DistrictCode");
    //  //  #endregion District Offices only
    //  //}
    //  //else
    //  //{
    //  //  if (!string.IsNullOrEmpty(DistrictCode))
    //  //    throw new ApplicationException(
    //  //      "The office class has an empty string DistrictCode");
    //  //}
    //  #endregion DistrictCode

    //  #region Incumbent flag set when office contest is created

    //  var isIncumbent = db.Is_Incumbent(
    //    PoliticianKey,
    //    OfficeKey);

    //  #endregion Incumbent flag set when office contest is created

    //  string SQL = "INSERT INTO ElectionsPoliticians";
    //  SQL += "(";
    //  SQL += " ElectionKey";
    //  SQL += ",OfficeKey";
    //  SQL += ",PoliticianKey";
    //  SQL += ",RunningMateKey";
    //  SQL += ",ElectionKeyState";
    //  SQL += ",ElectionKeyFederal";
    //  SQL += ",ElectionKeyCounty";
    //  SQL += ",ElectionKeyLocal";
    //  SQL += ",StateCode";
    //  SQL += ",CountyCode";
    //  SQL += ",LocalCode";
    //  SQL += ",DistrictCode";
    //  SQL += ",OrderOnBallot";
    //  SQL += ",IsIncumbent";
    //  SQL += ",IsWinner";
    //  SQL += ")";
    //  SQL += " VALUES(";
    //  SQL += db.SQLLit(ElectionKey);
    //  SQL += "," + db.SQLLit(OfficeKey);
    //  SQL += "," + db.SQLLit(PoliticianKey);
    //  SQL += "," + db.SQLLit(db.RunningMateKey(ElectionKey, OfficeKey, PoliticianKey));
    //  SQL += "," + db.SQLLit(db.ElectionKey_State(ElectionKey));
    //  SQL += "," + db.SQLLit(db.ElectionKey_Federal(ElectionKey, OfficeKey));
    //  SQL += "," + db.SQLLit(db.ElectionKey_County(ElectionKey, OfficeKey));
    //  SQL += "," + db.SQLLit(db.ElectionKey_Local(ElectionKey, OfficeKey));
    //  SQL += "," + db.SQLLit(Elections.GetStateCodeFromKey(ElectionKey));
    //  SQL += "," + db.SQLLit(Elections.GetCountyCodeFromKey(ElectionKey));
    //  SQL += "," + db.SQLLit(Elections.GetLocalCodeFromKey(ElectionKey));
    //  SQL += "," + db.SQLLit(DistrictCode);
    //  SQL += "," + OrderOnBallot.ToString();
    //  if (isIncumbent)
    //    SQL += ",1";
    //  else
    //    SQL += ",0";
    //  SQL += ",0";
    //  SQL += ")";
    //  db.ExecuteSQL(SQL);

    //  db.Log_ElectionsPoliticians_Changes(
    //    "A"
    //  , ElectionKey
    //  , OfficeKey
    //  , PoliticianKey
    //  );
    //}
    //#endregion ElectionsPoliticians INSERT
    //public static void ElectionsPoliticians_Delete_Transaction(string ElectionKey, string OfficeKey, string PoliticianKey)
    //{
    //  if (db.ElectionsPoliticians_Rows(ElectionKey, OfficeKey, PoliticianKey) > 0)
    //  {
    //    db.ElectionsPoliticians_Delete(ElectionKey, OfficeKey, PoliticianKey);

    //    db.Log_ElectionsPoliticians_Changes(
    //      "D"
    //        , ElectionKey
    //        , OfficeKey
    //        , PoliticianKey
    //        );

    //    //db.ElectionsPoliticians_Invalidate(ElectionKey, OfficeKey, PoliticianKey);

    //    //db.Report_Election_Set_Current_Not(ElectionKey);
    //  }
    //}

    //#region ElectionPoliticians Ballot Order

    //public static int BallotOrder_Candidate_Next(string ElectionKey, string OfficeKey)
    //{
    //  DataRow ElectionsPoliticiansRow = db.Row_First_Optional(
    //    sql.Elections_Politicians_Office_DESC(
    //      ElectionKey
    //      , OfficeKey)
    //    );
    //  if (ElectionsPoliticiansRow != null)
    //    return Convert.ToInt16(ElectionsPoliticiansRow["OrderOnBallot"].ToString()) + 10;
    //  else
    //    return 10;
    //}
    //#endregion ElectionPoliticians Ballot Order
    //public static void ElectionsPoliticians_INSERT_And_Whether_Incumbent(
    //  string ElectionKey
    //  , string OfficeKey
    //  , string PoliticianKey
    //  , int OrderOnBallot)
    //{
    //  db.ElectionsPoliticians_Delete_Transaction(
    //    ElectionKey
    //    , OfficeKey
    //    , PoliticianKey)
    //    ;

    //  db.ElectionsPoliticians_INSERT(
    //    ElectionKey
    //    , OfficeKey
    //    , PoliticianKey
    //    , string.Empty
    //    , OrderOnBallot
    //    );

    //  db.Log_ElectionsPoliticians_Changes(
    //    "A"
    //      , ElectionKey
    //      , OfficeKey
    //      , PoliticianKey
    //      );

    //  //db.ElectionsPoliticians_Invalidate(ElectionKey, OfficeKey, PoliticianKey);

    //  //db.Report_Election_Set_Current_Not(ElectionKey);
    //}
    //public static void ElectionsPoliticians_INSERT_And_Whether_Incumbent(
    //  string ElectionKey
    //  , string OfficeKey
    //  , string PoliticianKey
    //  , string Ballot_Order)
    //{
    //  #region checks
    //  if (string.IsNullOrEmpty(ElectionKey))
    //    throw new ApplicationException("The ElectionKey is empty.");
    //  if (string.IsNullOrEmpty(OfficeKey))
    //    throw new ApplicationException("The OfficeKey is empty.");
    //  if (string.IsNullOrEmpty(PoliticianKey))
    //    throw new ApplicationException("The PoliticianKey is empty.");
    //  #endregion checks

    //  if (!string.IsNullOrEmpty(Ballot_Order))
    //  {
    //    #region Use Ballot order passed
    //    db.ElectionsPoliticians_INSERT_And_Whether_Incumbent(
    //      ElectionKey
    //      , OfficeKey
    //      , PoliticianKey
    //      , Convert.ToInt16(Ballot_Order)
    //      );
    //    #endregion Use Ballot order  passed
    //  }
    //  else
    //  {
    //    #region Next Ballot Order
    //    int BallotOrder = db.BallotOrder_Candidate_Next(ElectionKey, OfficeKey);

    //    db.ElectionsPoliticians_INSERT_And_Whether_Incumbent(
    //      ElectionKey
    //      , OfficeKey
    //      , PoliticianKey
    //      , BallotOrder
    //      );
    //    #endregion Next Ballot Order
    //  }

    //  db.Politician_Update_Transaction_Str(PoliticianKey,
    //    Politician_Column.TemporaryOfficeKey, OfficeKey);
    //}

    //public static void Triple_Key_Update_Int(string Table, string Column, int ColumnValue, string KeyName1, string KeyValue1, string KeyName2, string KeyValue2, string KeyName3, string KeyValue3)
    //{
    //  string UpdateSQL = "UPDATE " + Table
    //    + " SET " + Column + " = " + ColumnValue.ToString()
    //    + " WHERE " + KeyName1 + " = " + db.SQLLit(KeyValue1)
    //    + " AND " + KeyName2 + " = " + db.SQLLit(KeyValue2)
    //  + " AND " + KeyName3 + " = " + db.SQLLit(KeyValue3);
    //  db.ExecuteSQL(UpdateSQL);
    //}

    //public static string Triple_Key_Str_Optional(string Table, string Column, string KeyName1, string KeyValue1, string KeyName2, string KeyValue2, string KeyName3, string KeyValue3)
    //{
    //  string SQL = "SELECT " + Column.Trim() + " FROM " + Table.Trim()
    //  + " WHERE " + KeyName1.Trim() + " = " + db.SQLLit(KeyValue1.Trim())
    //  + " AND " + KeyName2.Trim() + " = " + db.SQLLit(KeyValue2.Trim())
    //  + " AND " + KeyName3.Trim() + " = " + db.SQLLit(KeyValue3.Trim());
    //  DataTable table = db.Table(SQL);
    //  switch (table.Rows.Count)
    //  {
    //    case 1://KeyValue
    //      return (string) table.Rows[0][Column].ToString().Trim();
    //    case 0://no row
    //      return string.Empty;
    //    default:
    //      return string.Empty;
    //  }
    //}

    //#region Election Report Columns
    //public const int Columns_Without_BallotOrder_Or_Pic = 5;
    //public const int Columns_With_BallotOrder_Or_Pic = 6;
    //public const int Columns_Officials = 6;
    ////public const int Columns_Officials = 5;

    //#endregion Election Report Columns

    //#region Election Report CURRENT

    //public static int Columns_Report_Election(db.ReportUser reportUser, string electionKey)
    //{
    //  if (reportUser == db.ReportUser.Public)
    //  {
    //    //return db.Columns_Without_BallotOrder_Or_Pic;
    //    return db.Columns_With_BallotOrder_Or_Pic;
    //  }
    //  else
    //  {
    //    //Only Admin and Master Users for upcoming election have ballot order
    //    if (db.Is_Election_Upcoming(electionKey))
    //      return db.Columns_With_BallotOrder_Or_Pic;
    //    else
    //      return db.Columns_Without_BallotOrder_Or_Pic;
    //  }
    //}
    //#endregion Election Report CURRENT

    //public static void ElectionsPoliticians_INSERT_And_Whether_Incumbent(
    //  string ElectionKey
    //  , string OfficeKey
    //  , string PoliticianKey)
    //{
    //  db.ElectionsPoliticians_INSERT_And_Whether_Incumbent(
    //    ElectionKey
    //    , OfficeKey
    //    , PoliticianKey
    //    , string.Empty
    //    );
    //}
    //public static void ElectionsPoliticians_Delete_Log_Invalidate(string ElectionKey, string OfficeKey, string PoliticianKey)
    //{
    //  string SQL = "DELETE FROM ElectionsPoliticians";
    //  SQL += " WHERE ElectionKey = " + db.SQLLit(ElectionKey);
    //  SQL += " AND OfficeKey = " + db.SQLLit(OfficeKey);
    //  SQL += " AND PoliticianKey = " + db.SQLLit(PoliticianKey);
    //  db.ExecuteSQL(SQL);

    //  db.Log_ElectionsPoliticians_Changes(
    //    "D"
    //  , ElectionKey
    //  , OfficeKey
    //  , PoliticianKey
    //  );

    //  //ElectionsPoliticians_Invalidate(
    //  //ElectionKey
    //  //, OfficeKey
    //  //, PoliticianKey
    //  //  );
    //}
    //public static void ElectionsPoliticians_Update_Str(string ElectionKey, string OfficeKey, string PoliticianKey, string Column, string ColumnValue)
    //{
    //  db.Triple_Key_Update_Str("ElectionsPoliticians", Column, ColumnValue, "ElectionKey", ElectionKey, "OfficeKey", OfficeKey, "PoliticianKey", PoliticianKey);
    //}

    //public static void ElectionsPoliticians_Update_Int(string ElectionKey, string OfficeKey, string PoliticianKey, string Column, int ColumnValue)
    //{
    //  db.Triple_Key_Update_Int("ElectionsPoliticians", Column, ColumnValue, "ElectionKey", ElectionKey, "OfficeKey", OfficeKey, "PoliticianKey", PoliticianKey);
    //}

    //public static string ElectionsPoliticians_Optional(
    //  string ElectionKey
    //  , string OfficeKey
    //  , string PoliticianKey
    //  , string Column)
    //{
    //  return db.Triple_Key_Str_Optional("ElectionsPoliticians", Column, "ElectionKey", ElectionKey, "OfficeKey", OfficeKey, "PoliticianKey", PoliticianKey);
    //}
    //public static string ElectionsPoliticians_Str(
    //  string ElectionKey
    //  , string OfficeKey
    //  , string PoliticianKey
    //  , string Column)
    //{
    //  return db.Triple_Key_Str("ElectionsPoliticians",
    //    Column,
    //    "ElectionKey", ElectionKey,
    //    "OfficeKey", OfficeKey,
    //    "PoliticianKey", PoliticianKey
    //    );
    //}

    //public static string Anchor_Admin_OfficeContest(
    //  string electionKey
    //  , string officeKey
    //  , string anchorText
    //  , string target
    //  , string politicianKey
    //  , string mode
    //  )
    //{
    //  string anchor = string.Empty;
    //  anchor += "<a href=";
    //  anchor += "\"";
    //  anchor += db.Url_Admin_OfficeContest(electionKey, officeKey, politicianKey, mode);
    //  anchor += "\"";
    //  anchor += " target=";
    //  anchor += "\"";
    //  if (target != string.Empty)
    //    anchor += target;
    //  else
    //    anchor += "_officecontest";
    //  anchor += "\"";

    //  anchor += " title=";
    //  anchor += "\"";
    //  anchor += Politicians.GetFormattedName(politicianKey) + " administration data edit form";
    //  anchor += "\"";

    //  anchor += ">";

    //  anchor += anchorText;

    //  anchor += "</a>";
    //  return anchor;
    //}
    //public static void Ballot_Order_Check(string ballotOrder)
    //{
    //  if (
    //    (ballotOrder.Trim() != string.Empty)
    //    && (!db.Is_Valid_Integer(ballotOrder.Trim()))
    //    )
    //    throw new ApplicationException("The Ballot Order needs to be a whole number.");
    //}

    //public static string Insert_BR(string str, int at)
    //{
    //  //if (str.Length > at)
    //  //  str = str.Insert(at, "<br>");
    //  //if (str.Length > 2* at)
    //  //  str = str.Insert(2* at, "<br>");
    //  //if (str.Length > 3 * at)
    //  //  str = str.Insert(3 * at, "<br>");
    //  //return str;

    //  int i = 1;
    //  while (str.Length > at * i)
    //  {
    //    str = str.Insert(at * i, "<br>");
    //    i++;
    //  }
    //  return str;
    //}

    //public static string Str_Remove_Last_Char(string str2Fix)
    //{
    //  if (str2Fix.Trim().EndsWith(","))
    //    return str2Fix.Trim().Remove(str2Fix.Trim().Length - 1, 1);
    //  else
    //    return str2Fix;
    //}

    //public static bool Is_Valid_ElectionsPoliticians(string electionKey, string officeKey, string politicianKey)
    //{
    //  string sql = string.Empty;
    //  sql += "ElectionsPoliticians ";
    //  sql += " WHERE ElectionKey = " + db.SQLLit(electionKey);
    //  sql += " AND OfficeKey = " + db.SQLLit(officeKey);
    //  sql += " AND (PoliticianKey = " + db.SQLLit(politicianKey);
    //  sql += " OR RunningMateKey = " + db.SQLLit(politicianKey) + ")";

    //  return db.Rows_Count_From(sql) != 0;
    //}

    //public static DateTime No_Date()
    //{
    //  return Convert.ToDateTime("1900-01-01 00:00:00");
    //}

    //public static bool Is_Election_Upcoming_Viewable(string ElectionKey)
    //{
    //  string SQL = string.Empty;
    //  SQL += " Elections";
    //  SQL += " WHERE ElectionKey = " + db.SQLLit(ElectionKey);
    //  SQL += " AND IsViewable = 1";
    //  int x = db.Rows_Count_From(SQL);
    //  if (db.Rows_Count_From(SQL) == 1)
    //    return true;
    //  else
    //    return false;
    //}

    //#region Elections UPDATE
    ////int
    //public static void Elections_Update_Int(
    //  string ElectionKey
    //  , string Column
    //  , int ColumnValue)
    //{
    //  db.Single_Key_Update_Int("Elections", Column, ColumnValue, "ElectionKey", ElectionKey);
    //}

    ////date
    //public static void Elections_Update_Date(
    //  string ElectionKey
    //  , string Column
    //  , DateTime ColumnValue)
    //{
    //  db.Single_Key_Update_Date("Elections", Column, ColumnValue, "ElectionKey", ElectionKey);
    //}
    //#endregion


    //public static bool Elections_Bool(string ElectionKey, string Column)
    //{
    //  return db.Single_Key_Bool("Elections", Column, "ElectionKey", ElectionKey);
    //}
    //public static string Elections_Date_Str(
    //  string Election_Key
    //  , string Column)
    //{
    //  if (db.Elections_Date(Election_Key, Column)
    //      != db.No_Date())
    //    return db.Elections_Date(Election_Key, Column).ToString();
    //  else
    //    return string.Empty;
    //}

    //public static string PartyCode4ElectionKey(string ElectionKey)
    //{
    //  //if (db.Is_Valid_Election(ElectionKey))
    //  //{
    //  string PartyCode;
    //  if (
    //    Elections.GetElectionTypeFromKey(ElectionKey) == "O"
    //    || Elections.GetElectionTypeFromKey(ElectionKey) == "S"
    //    )
    //    PartyCode = "ALL";
    //  else//ElectionType = B or P for Primary
    //    PartyCode = Elections.GetStateCodeFromKey(ElectionKey) +
    //      Elections.GetNationalPartyCodeFromKey(ElectionKey);
    //  return PartyCode;
    //}
    //public static void States_Update_Date(string StateCode, string Column, DateTime ColumnValue)
    //{
    //  db.Single_Key_Update_Date("States", Column, ColumnValue, "StateCode", StateCode);
    //}
    //public static void States_Update_Int(string StateCode, string Column, int ColumnValue)
    //{
    //  db.Single_Key_Update_Int("States", Column, ColumnValue, "StateCode", StateCode);
    //}

    ////Date
    //public static void Master_Update_Date(string Column, DateTime ColumnValue)
    //{
    //  string SQL = "UPDATE Master "
    //    + " SET " + Column + " = " + db.SQLLit(Db.DbDateTime(ColumnValue))
    //    + " WHERE ID = 'MASTER' ";
    //  db.ExecuteSQL(SQL);
    //}
    ////Int
    //public static void Master_Update_Int(string Column, int ColumnValue)
    //{
    //  string SQL = "UPDATE Master "
    //    + " SET " + Column + " = " + Convert.ToUInt16(ColumnValue).ToString()
    //    + " WHERE ID = 'MASTER' ";
    //  db.ExecuteSQL(SQL);
    //}
    //public static string Master_Date_Str(string Column)
    //{
    //  if (db.Master_Date(Column)
    //      != db.No_Date())
    //    return db.Master_Date(Column).ToString();
    //  else
    //    return string.Empty;
    //}
    //public static bool Is_Has_Http(string str2Check)
    //{
    //  if (
    //    (str2Check.ToLower().IndexOf("http://", 0, str2Check.Length) != -1)
    //    || (str2Check.ToLower().IndexOf("https://", 0, str2Check.Length) != -1)
    //    )
    //    return true;
    //  else
    //    return false;
    //}

    //#region /Admin/ElectionCreate.aspx URLs & Anchors
    //public static string Url_Admin_ElectionCreate()
    //{
    //  //return db.Url_Admin_ElectionCreate(string.Empty);
    //  return "/Admin/ElectionCreate.aspx";
    //}
    //#endregion /Admin/ElectionCreate.aspx URLs & Anchors

    //#region ElectionKey Build to convert from old format
    //public static string ElectionKey_Build(
    //  string StateCode
    //  , string CountyCode
    //  , string LocalCode
    //  , string Date
    //  , string ElectionType
    //  , string NationalPartyCode
    //  )
    //{
    //  #region Election Key Construction
    //  //12 chars if State or Federal Election
    //  //15 chars if County Election
    //  //17 chars if Local Election

    //  //2 char - StateCode or FederalCode  0:2
    //  //        Federal Codes are: UU, US,U1,U2,U3,u4 
    //  //        UU indicates all 51 states, one state at a time
    //  //        US is for a national presidential contest, one big country wide state
    //  //        U1,U2,U3,u4 are used when generating national reports
    //  //            U1=US President, U2=US Senate, U3=US House, U4=Governors
    //  //            so they can be treated as a national election
    //  //        StateCode is used for State election reports
    //  //8 char - Date (YYYYMMDD) 2:8
    //  //1 char - Election Type (either A, B, G, O, S, or P) 10:1
    //  //        National Contest/Election:
    //  //            A - National Presidential Party Contest
    //  //                       only presidential candidates still in race in a particular party
    //  //                       don't need an address for ballot
    //  //                        not reflection of ballot
    //  //                        only 1 election row created
    //  //        State Elections:
    //  //            B - State Presidential Party Primary 
    //  //                        all Pres candidates on ballot whether still in race or not
    //  //                        don't need an address for ballot
    //  //                        51 different elections
    //  //                        only 1 party
    //  //            G - National General Election with All States and All parties
    //  //                (need an address)
    //  //                    All Election Types Created at State level in ADMIN/Default.aspx
    //  //            O - Off Year Scheduled State Election
    //  //                (need an address, all office levels except US President)
    //  //            S - Special Unscheduled State Election
    //  //                (need an address, all office levels except US President, only a couple of offices)
    //  //            P - General Statewide Party Primary
    //  //                (need an address, all office levels, only 1 party)

    //  //1 char - National Party Code 11:1
    //  //         Used as the code shown on ballots
    //  //        (A = All parties, G=Green, L= Libertarian, X= Nonpartisan, R=Republican, D=Democratic)
    //  //3 digits or empty- County Code if it is a County Election 12:3
    //  //2 digits or empty- LocalCode or TownCode if it is a Local or Town election 15:2      
    //  #endregion Election Key Construction

    //  #region checks
    //  if (StateCode.Trim() == string.Empty)
    //    throw new ApplicationException("StateCode Missing!");
    //  if (Date.Trim() == string.Empty)
    //    throw new ApplicationException("The election date Missing!");
    //  if (!db.Is_Valid_Date(Date.Trim()))//bad date
    //    throw new ApplicationException("Making ElectionKey: Date is bad!");
    //  if (ElectionType.Trim() == string.Empty)
    //    throw new ApplicationException("Election Type is empty!");
    //  if (NationalPartyCode.Trim() == string.Empty)
    //    throw new ApplicationException("NationalPartyCode is empty!");
    //  #endregion checks


    //  string ElectionKey = string.Empty;
    //  //StateCode
    //  ElectionKey += StateCode.Trim();
    //  //YYYYMMDD
    //  ElectionKey += Convert.ToDateTime(Date).ToString("yyyyMMdd");
    //  //Type
    //  ElectionKey += ElectionType;
    //  //Party
    //  ElectionKey += NationalPartyCode;
    //  //CountyCode
    //  if (CountyCode.Trim() != string.Empty)
    //    ElectionKey += CountyCode.Trim();
    //  //LocalCode
    //  if (LocalCode.Trim() != string.Empty)
    //    ElectionKey += LocalCode.Trim();

    //  return ElectionKey;
    //}
    //#endregion ElectionKey


    //public static string Url_Admin_ElectionCreate(
    //  string stateCode
    //  , string electionType
    //  , string partyCode
    //  , DateTime electionDate
    //  , string description
    //  )
    //{
    //  string url = db.Url_Admin_ElectionCreate();
    //  url += "&State=" + stateCode;
    //  url += "&Type=" + electionType;
    //  url += "&Party=" + partyCode;
    //  url += "&Date=" + electionDate.ToString("MM/dd/yyyy");
    //  url += "&Desc=" + description;
    //  return db.Fix_Url_Parms(url);
    //}

    //public static string Anchor(string url, string anchorText)
    //{
    //  return db.Anchor(url, anchorText, string.Empty, string.Empty);
    //}
    //public static string Str_Election_Description(
    //  string electionType
    //  , string partyCode
    //  , string electionDate
    //  , string stateCode
      //)
    // Elections.FormatElectionDescription
    //{
    //  if (electionDate == null) throw new ArgumentNullException("electionDate");
    //  electionDate = Convert.ToDateTime(electionDate).ToString("MMMM d, yyyy");

    //  string stateName = string.Empty;
    //  if (StateCache.IsValidStateCode(stateCode))
    //    stateName += " " + StateCache.GetStateName(stateCode);

    //  string partyName = string.Empty;
    //  if (
    //    (electionType.ToUpper() != "G") //General Election
    //    && (partyCode.ToUpper() != "A") //All Parties
    //    && (!string.IsNullOrEmpty(Parties.GetNationalPartyDescription(partyCode)))
    //    )
    //    partyName = " " + Parties.GetNationalPartyDescription(partyCode);

    //  return electionDate + stateName + partyName
    //    + " " + db.Str_Election_Type_Description(electionType, stateCode);
    //}

    //#region (mode: UPDATE ElectionsOffices Table)
    //public static string Url_Admin_Office_UPDATE_Election(string electionKey, string officeKey, string politicianKey)
    //{
    //  string url = db.Ur4AdminOffice();
    //  url += "&Election=" + electionKey;
    //  url += "&Office=" + officeKey;
    //  if (politicianKey != string.Empty)
    //    url += "&Politician=" + politicianKey;
    //  //url += db.xUrl_Add_ViewState_DataCodes_OfficeKey(officeKey);
    //  return db.Fix_Url_Parms(url);
    //}
    //#endregion (mode: UPDATE ElectionsOffices Table)
    //public static string Url_Admin_Office_UPDATE_Election(string electionKey, string officeKey)
    //{
    //  return Url_Admin_Office_UPDATE_Election(electionKey, officeKey, string.Empty);
    //}
    //public static void ElectionsOffices_INSERT(
    //  string ElectionKey
    //  , string OfficeKey
    //  )
    //{
    //  //DistrictCode
    //  db.ElectionsOffices_INSERT(
    //    ElectionKey
    //    , OfficeKey
    //    //, string.Empty
    //    //, db.xDistrictCode_In_OfficeKey(OfficeKey)
    //    , Offices.GetValidDistrictCode(OfficeKey)
    //    );
    //}

    //#region Elections INSERT
    //public static void Elections_Insert_And_Report_Election_Update(string stateCode,
    //  string countyCode, string localCode, string electionDate,
    //  string electionType, string nationalPartyCode, string partyCode,
    //  string electionDesc, string electionAdditionalInfo, string ballotInstructions)
    //{
    //  string electionKey = db.ElectionKey_Build(
    //    stateCode
    //  , countyCode
    //  , localCode
    //  , electionDate
    //  , electionType
    //  , nationalPartyCode
    //  );

    //  if (db.Is_Valid_Election(electionKey))
    //    throw new ApplicationException("An election already exists with ElectionKey: "
    //      + electionKey);

    //  string SQL = string.Empty;
    //  SQL += "INSERT INTO Elections(";
    //  SQL += "ElectionKey";
    //  SQL += ",StateCode";
    //  SQL += ",CountyCode";
    //  SQL += ",LocalCode";
    //  SQL += ",ElectionDate";
    //  SQL += ",ElectionYYYYMMDD";
    //  SQL += ",ElectionType";
    //  SQL += ",NationalPartyCode";
    //  SQL += ",PartyCode";
    //  SQL += ",ElectionStatus";
    //  SQL += ",ElectionDesc";
    //  SQL += ",ElectionAdditionalInfo";
    //  SQL += ",ElectionResultsSource";
    //  SQL += ",ElectionResultsDate";
    //  SQL += ",BallotInstructions";
    //  SQL += ",EmailsDateElectionRoster";
    //  SQL += ",EmailsDateElectionCompletion";
    //  SQL += ",EmailsDateCandidatesLogin";
    //  SQL += ",EmailsDatePartiesLogin";
    //  SQL += ",ElectionKeyCanonical";
    //  SQL += ")VALUES(";
    //  //SQL += db.SQLLit(db.ElectionKey_Build(
    //  //            StateCode
    //  //          , CountyCode
    //  //          , LocalCode
    //  //          , ElectionDate
    //  //          , ElectionType
    //  //          , NationalPartyCode
    //  //          ));
    //  SQL += db.SQLLit(electionKey);
    //  SQL += "," + db.SQLLit(stateCode);
    //  SQL += "," + db.SQLLit(countyCode);
    //  SQL += "," + db.SQLLit(localCode);
    //  SQL += "," + db.SQLLit(Db.DbDateTime(Convert.ToDateTime(electionDate)));
    //  SQL += "," + db.SQLLit(Convert.ToDateTime(electionDate).ToString("yyyyMMdd"));
    //  SQL += "," + db.SQLLit(electionType);
    //  SQL += "," + db.SQLLit(nationalPartyCode);
    //  SQL += "," + db.SQLLit(partyCode);
    //  SQL += ",''";
    //  SQL += "," + db.SQLLit(electionDesc);
    //  SQL += "," + db.SQLLit(electionAdditionalInfo);
    //  SQL += ",''";
    //  SQL += ",'1900-01-01'";
    //  SQL += "," + db.SQLLit(ballotInstructions);
    //  SQL += ",'1900-01-01'";
    //  SQL += ",'1900-01-01'";
    //  SQL += ",'1900-01-01'";
    //  SQL += ",'1900-01-01'";
    //  SQL += ",''";
    //  SQL += ")";

    //  db.ExecuteSQL(SQL);

    //  //db.Report_Election_Update(cache, electionKey);
    //}
    //#endregion Elections INSERT
    //public static string ElectionKey_County(string ElectionKey)
    //{
    //  if (
    //    (!string.IsNullOrEmpty(ElectionKey))
    //    && (ElectionKey.Length >= Elections.ElectionKeyLengthCounty)
    //    )
    //    return ElectionKey.Substring(0, Elections.ElectionKeyLengthCounty);
    //  else
    //    return string.Empty;
    //}

    //#region (mode: UPDATE ElectionsOffices Table)
    //public static string Anchor_Admin_Office_UPDATE_Election(string electionKey,
    //  string officeKey, string anchorText)
    //{
    //  string anchor = string.Empty;
    //  anchor += "<a href=";
    //  anchor += "\"";
    //  anchor += db.Url_Admin_Office_UPDATE_Election(electionKey, officeKey);//(mode: EditOffice4Election)
    //  anchor += "\"";

    //  anchor += " target=";
    //  anchor += "\"";
    //  //Anchor += "_edit";
    //  anchor += "_office";
    //  anchor += "\"";

    //  anchor += ">";

    //  //Anchor += "<nobr>" + AnchorText + "</nobr>";
    //  anchor += anchorText;
    //  anchor += "</a>";
    //  return anchor;
    //}
    //#endregion (mode: UPDATE ElectionsOffices Table)

    //public static void ElectionsPoliticians_INSERT(
    //  string ElectionKey
    //  , string OfficeKey
    //  , string PoliticianKey
    //  , int OrderOnBallot
    //  )
    //{
    //  db.ElectionsPoliticians_INSERT(
    //   ElectionKey
    //   , OfficeKey
    //   , PoliticianKey
    //   , string.Empty
    //   , OrderOnBallot
    //   );
    //}
    ////string
    //public static void Elections_Update_Str(
    //  string ElectionKey
    //  , string Column
    //  , string ColumnValue)
    //{
    //  db.Single_Key_Update_Str("Elections", Column, ColumnValue, "ElectionKey", ElectionKey);
    //}
    //public static bool Is_StateCode_All_51_States(string stateCode)
    //{
    //  return stateCode.ToUpper() == "UU";
    //}

    //#region Election Types
    ////State Elections
    //public const string Election_Type_StateGeneral_G = "G";
    //public const string Election_Type_StateOffYear_O = "O";
    //public const string Election_Type_StateSpecial_S = "S";
    //public const string Election_Type_StatePartyPrimary_P = "P";
    //public const string Election_Type_StatePresidentialPartyPrimary_B = "B";
    //#endregion Election Types
    //public static string Path_OrEmpty_(string Path_Full)
    //{
    //  //use when server full path is known
    //  if (File.Exists(Path_Full))
    //  {
    //    return Path_Full;
    //  }
    //  else
    //    return string.Empty;
    //}
    ////css
    //public static string Path_Part_Css_Custom(string DesignCode, string FileNameWithExtn)
    //{
    //  //return db.Path_PartCssCustomNoFile(DesignCode) + FileNameWithExtn;
    //  return @"css\Designs\" + DesignCode + @"\" + FileNameWithExtn;
    //}
    //public static string Path_OrEmpty(string Path_Part)
    //{
    //  return db.Path_OrEmpty_(VotePage.GetServerPath() + Path_Part);
    //}
    ////css
    //public static string Url_Css_Custom(string DesignCode, string CssFile)
    //{
    //  return @"/css/Designs/" + DesignCode + @"/" + CssFile;
    //}

    //#region /Admin/Emails.aspx
    //public static string Url_Admin_Emails()
    //{
    //  return @"/Admin/Emails.aspx";
    //}

    //#endregion /Admin/Emails.aspx

    //public static string Anchor_Admin_OfficeWinner(
    //  string electionKey
    //  , string officeKey
    //  , string anchorText
    //  , string target
    //  )
    //{
    //  return db.Anchor_Admin_OfficeWinner(
    //   electionKey
    //  , officeKey
    //  , anchorText
    //  , target
    //  , string.Empty
    //  );
    //}
    //public static void ElectionsPoliticians_INSERT(
    //  string ElectionKey
    //  , string OfficeKey
    //  , string PoliticianKey
    //  )
    //{
    //  db.ElectionsPoliticians_INSERT(
    //    ElectionKey
    //    , OfficeKey
    //    , PoliticianKey
    //    , string.Empty
    //    , db.BallotOrder_Candidate_Next(
    //          ElectionKey, OfficeKey)
    //    );
    //}
    ////bool
    //public static void Elections_Update_Bool(
    //  string ElectionKey
    //  , string Column
    //  , bool ColumnValue)
    //{
    //  db.Single_Key_Update_Bool("Elections", Column, ColumnValue, "ElectionKey", ElectionKey);
    //}

    //public static int Elections_Int(string ElectionKey, string Column)
    //{
    //  if (db.Is_Valid_Election(ElectionKey))
    //    return db.Single_Key_Int("Elections", Column, "ElectionKey", ElectionKey);
    //  else
    //    return 0;
    //}

    //#region CountyCode LocalCode in Elections Row

    //public static string CountyCode_In_Elections_Row(string ElectionKey)
    //{
    //  if (ElectionKey != string.Empty)
    //  {
    //    string StateCode = db.Elections_Str(ElectionKey, "StateCode");
    //    string CountyCode = db.Elections_Str(ElectionKey, "CountyCode");

    //    if (CountyCache.CountyExists(StateCode, CountyCode))
    //      return CountyCode;
    //    else
    //      return string.Empty;
    //  }
    //  else
    //    return string.Empty;
    //}

    //#endregion CountyCode LocalCode in Elections Row
    //public static int Referendums_In_Election(string ElectionKey)
    //{
    //  string sql = string.Empty;
    //  sql += "Referendums";
    //  sql += " WHERE ElectionKey = " + db.SQLLit(ElectionKey);
    //  return db.Rows_Count_From(sql);
    //}

    //#region Path - css
    //public static void Path_Css_Custom_Href_Set(ref HtmlLink Href, string DesignCode, string CssFile)
    //{
    //  //string a = db.Path_OrEmpty(db.Path_Part_Css_Custom(DesignCode, CssFile));
    //  //string b = db.Url_Css_Custom(DesignCode, CssFile);
    //  if (db.Path_OrEmpty(db.Path_Part_Css_Custom(DesignCode, CssFile)) != string.Empty)
    //    Href.Href = db.Url_Css_Custom(DesignCode, CssFile);
    //}
    //#endregion Path - css
    //public static string Url_Admin_Emails(
    //  string StateCode
    //  , string ElectionKey)
    //{
    //  string Url = db.Url_Admin_Emails();
    //  if (StateCode != string.Empty)
    //    Url += "&State=" + StateCode;
    //  if (ElectionKey != string.Empty)
    //    Url += "&Election=" + ElectionKey;
    //  return db.Fix_Url_Parms(Url);
    //}
    //public static string Anchor_Admin_OfficeWinner(
    //  string electionKey
    //  , string officeKey
    //  , string anchorText
    //  )
    //{
    //  return db.Anchor_Admin_OfficeWinner(
    //   electionKey
    //  , officeKey
    //  , anchorText
    //  , string.Empty
    //  );
    //}

    //#region Counties Table

    //public static void CountiesUpdate(string StateCode, string CountyCode, string Column, string ColumnValue)
    //{
    //  db.Double_Key_Update_Str("Counties", Column, ColumnValue, "StateCode", StateCode, "CountyCode", CountyCode);
    //}

    //#endregion

    //#region Either States, Counties, LocalDistricts Tables
    //public static void States_Counties_LocalDistricts_Update(
    //  string StateCode
    //  , string CountyCode
    //  , string LocalCode
    //  , string Column
    //  , string Value
    //  , string FromValue
    //  , bool IsLogUpdate
    //  )
    //{
    //  if (!string.IsNullOrEmpty(StateCode))
    //  {
    //    if (!string.IsNullOrEmpty(LocalCode))
    //    {
    //      db.LocalDistrictsUpdate(
    //        StateCode
    //        , CountyCode
    //        , LocalCode
    //        , Column
    //        , Value
    //        );
    //    }
    //    else if (!string.IsNullOrEmpty(CountyCode))
    //    {
    //      db.CountiesUpdate(
    //        StateCode
    //        , CountyCode
    //        , Column
    //        , Value
    //        );
    //    }
    //    else
    //    {
    //      db.States_Update_Str(
    //        StateCode
    //        , Column
    //        , Value
    //        );
    //    }
    //  }
    //  //--------
    //  if (IsLogUpdate)
    //    DB.VoteLog.LogAdminData.Insert(
    //      DateTime.Now
    //      , SecurePage.UserSecurityClass
    //      , SecurePage.UserName
    //      , Column
    //      , FromValue
    //      , Value
    //      );

    //}
    //#endregion Either States, Counties, LocalDistricts Tables

    //public static string GetLocalDistricts(string StateCode, string CountyCode, string LocalCode, string Column)
    //{
    //  db.LocalDistricts_Row_Insert_Empty(StateCode, CountyCode, LocalCode);
    //  return db.Triple_Key_Str("LocalDistricts", Column, "StateCode", StateCode, "CountyCode", CountyCode, "LocalCode", LocalCode);
    //}

    //public static string Counties(string StateCode, string CountyCode, string Column)
    //{
    //  return db.Double_Key_Str("Counties", Column, "StateCode", StateCode, "CountyCode", CountyCode);
    //  //Double_Key_Str_Optional
    //}

    //#region /Organization/ URLs and Anchors
    //public static string Url_OrganizationDefault(string OrganizationCode)
    //{
    //  return "/Organization/Default.aspx?Organization=" + OrganizationCode;
    //}
    //#endregion /Organization/ URLs and Anchors
    //public static string Url_Party_Default(string PartyKey)
    //{
    //  string Url = @"/Party/Default.aspx";
    //  Url += "?Party=" + PartyKey;
    //  return Url;
    //}
    //public static string Anchor_OrganizationDefault(string OrganizationCode)
    //{
    //  string Anchor = string.Empty;
    //  Anchor += "<a href=";
    //  Anchor += "\"";
    //  Anchor += db.Url_OrganizationDefault(OrganizationCode);
    //  Anchor += "\"";
    //  Anchor += ">";
    //  Anchor += "<nobr>" + OrganizationCode + "</nobr>";
    //  Anchor += "</a>";
    //  return Anchor;
    //}
    //public static string Anchor_AdminQuestionAnswers(string anchorText,
    // string issueLevel, string stateCode)
    //{
    //  string anchor = string.Empty;
    //  anchor += "<a href=";
    //  anchor += "\"";
    //  //Anchor += "/Admin/QuestionAnswers.aspx";
    //  //Anchor += "?IssueLevel=" + IssueLevel;
    //  //Anchor += "&StateCode=" + StateCode;
    //  anchor += db.Url_Admin_QuestionAnswers(issueLevel, stateCode);
    //  anchor += "\"";
    //  anchor += ">";
    //  anchor += "<nobr>" + anchorText + "</nobr>";
    //  //Anchor += AnchorText;
    //  anchor += "</a>";
    //  return anchor;
    //}
    //public static string Anchor_Admin_Issues(string stateCode, string anchorText)
    //{
    //  return db.Anchor_Admin_Issues(stateCode, string.Empty, anchorText);
    //}
    //public static string Url_Admin_LocalDistricts(string countyCode, string localCode)
    //{
    //  string url = "/Admin/Districts.aspx";
    //  //if (StateCode != string.Empty)
    //  //  Url += "&State=" + StateCode;
    //  if (countyCode != string.Empty)
    //    url += "&County=" + countyCode;
    //  if (localCode != string.Empty)
    //    url += "&Local=" + localCode;
    //  return db.Fix_Url_Parms(url);
    //}

    //public static bool Is_Valid_Organization(string organizationCode)
    //{
    //  return db.Rows("Organizations", "OrganizationCode", organizationCode) == 1;
    //}
    //public static string Anchor_Admin_Parties(
    //  string stateCode
    //  , string anchorText
    //  )
    //{
    //  return db.Anchor_Admin_Parties(
    //   stateCode
    //  , string.Empty
    //  , string.Empty
    //  , anchorText
    //  );
    //}
    //public static string Anchor_Party_Default(string PartyKey)
    //{
    //  string stateName = StateCache.GetStateName(Parties.GetStateCodeFromKey(PartyKey));
    //  string Anchor = string.Empty;
    //  Anchor += "<a href=";
    //  Anchor += "\"";
    //  Anchor += db.Url_Party_Default(PartyKey);
    //  Anchor += "\"";
    //  Anchor += " target=";
    //  Anchor += "\"";
    //  Anchor += "_self";
    //  Anchor += "\"";
    //  Anchor += ">";
    //  Anchor += stateName;
    //  Anchor += "</a>";
    //  return Anchor;
    //}

    //#region Multiple URLs and Anchors
    //public static string OrganizationCodeLinks()
    //{
    //  string OrganizationCodeLinks = string.Empty;
    //  DataTable OrganizationCodeTable = db.Table(sql.OrganizationCodes());
    //  foreach (DataRow OrganizationRow in OrganizationCodeTable.Rows)
    //  {
    //    OrganizationCodeLinks += " | ";
    //    OrganizationCodeLinks += db.Anchor_OrganizationDefault(
    //      OrganizationRow["OrganizationCode"].ToString().Trim());
    //  }
    //  OrganizationCodeLinks += " | ";
    //  return OrganizationCodeLinks;
    //}
    //public static string Issue_Anchors()
    //{
    //  string Issue_Links = string.Empty;
    //  Issue_Links += db.Anchor_Admin_Issues("LL", "ALL Candidates");
    //  Issue_Links += " | ";
    //  Issue_Links += db.Anchor_Admin_Issues("US", "National");
    //  DataTable StatesTable = db.Table(sql.States_51());
    //  foreach (DataRow StateURLRow in StatesTable.Rows)
    //  {
    //    string StateName = StateURLRow["State"].ToString().Trim();
    //    string StateCode = StateURLRow["StateCode"].ToString().Trim();
    //    Issue_Links += " | ";
    //    Issue_Links += db.Anchor_Admin_Issues(StateCode, StateName);
    //  }
    //  return Issue_Links;
    //}
    //public static string IssueReportsLinks()
    //{
    //  string IssueReportsLinks = string.Empty;
    //  IssueReportsLinks += db.Anchor_AdminQuestionAnswers("National", "B", "US");
    //  DataTable StatesTable = db.Table(sql.States_51());
    //  foreach (DataRow StateURLRow in StatesTable.Rows)
    //  {
    //    string StateName = StateURLRow["State"].ToString().Trim();
    //    string StateCode = StateURLRow["StateCode"].ToString().Trim();
    //    IssueReportsLinks += " | ";
    //    IssueReportsLinks += db.Anchor_AdminQuestionAnswers(StateName, "C", StateCode);
    //  }
    //  return IssueReportsLinks;
    //}
    //#endregion Multiple URLs and Anchors

    //#region Dates - DateTime

    //public static string YYYY_MM_DD(string date)
    //{
    //  return Convert.ToDateTime(date).ToString("yyyy-MM-dd");
    //}

    //#endregion Dates - DateTime


    //// renamed from Is_Digit for clarity
    //public static bool Is_Single_Digit(string char2Check)
    //{
    //  return char2Check.Length == 1 && char.IsDigit(char2Check, 0);
    //}

    //public static string Str_Run_Time(TimeSpan runTime)
    //{
    //  return string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
    //                          runTime.Hours,
    //                          runTime.Minutes,
    //                          runTime.Seconds);
    //}
    //public static DateTime States_HomePageUpdated(
    //  string StateCode)
    //{
    //  return db.States_Date(
    //   StateCode
    //  , "HomePageUpdated");
    //}

    //#region Elections Bool
    //public static bool Is_Election_Upcoming_Viewable_State(string StateCode)
    //{
    //  if (db.Rows(sql.Elections_Upcoming_Viewable_G_O_S_P(StateCode)) >= 1)//there is at least 1 upcoming election
    //    return true;
    //  else
    //    return false;
    //}

    //#endregion

    //public static bool Is_Election_Has_Office_Contests(string ElectionKey)
    //{
    //  if (db.Rows_ElectionsOffices(ElectionKey) > 0)
    //    return true;
    //  else
    //    return false;
    //}

    //#region ElectionsOffices Table
    //public static int Rows_ElectionsOffices(string ElectionKey)
    //{
    //  string SQL = "ElectionsOffices";
    //  SQL += " WHERE ElectionKey = " + db.SQLLit(ElectionKey);
    //  return db.Rows_Count_From(SQL);
    //}

    //#endregion ElectionsOffices Table

    //public static string Html_From_Db_For_Page(string dbText)
    //{
    //  return HttpUtility.HtmlEncode(dbText).ReplaceNewLinesWithBreakTags();
    //  //return db.Str_CrLf_To_Br(DbText);
    //  //return HttpUtility.HtmlEncode(DbText);
    //}

    //public static string Anchor_Master_AnswersEdit(string PoliticianKey, string QuestionKey, string AnchorText, string Target)
    //{
    //  string Anchor = string.Empty;
    //  Anchor += "<a href=";
    //  Anchor += "\"";
    //  Anchor += db.Url_Master_AnswersEdit(PoliticianKey, QuestionKey);
    //  Anchor += "\"";

    //  if (Target != string.Empty)
    //  {
    //    Anchor += " target=";
    //    Anchor += Target;
    //    Anchor += " ";
    //  }

    //  Anchor += ">";

    //  Anchor += "<nobr>" + AnchorText + "</nobr>";
    //  //Anchor += AnchorText;
    //  Anchor += "</a>";
    //  return Anchor;
    //}

    //public static string Domains(string DomainServerName, string Column)
    //{
    //  if (DomainServerName != string.Empty)
    //    return db.Single_Key_Str("Domains", Column, "DomainServerName", DomainServerName);
    //  //return db.Single_Key_Str_Optional("Domains", Column, "DomainServerName", DomainServerName);
    //  else
    //    return string.Empty;
    //}
    //public static void DomainsUpdate(string DomainServerName, string Column, string ColumnValue)
    //{
    //  db.Single_Key_Update_Str("Domains", Column, ColumnValue, "DomainServerName", DomainServerName);
    //}

    //#region Organizations Table
    //public static string Organizations(string OrganizationCode, string Column)
    //{
    //  if (OrganizationCode != string.Empty)
    //    return db.Single_Key_Str_Optional("Organizations", Column, "OrganizationCode", OrganizationCode);
    //  else
    //    return string.Empty;
    //}
    //public static void OrganizationsUpdate(string OrganizationCode, string Column, string ColumnValue)
    //{
    //  db.Single_Key_Update_Str("Organizations", Column, ColumnValue, "OrganizationCode", OrganizationCode);
    //}
    //#endregion

    //public static DataTable Table_Offices(int Office_Class)
    //{
    //  return db.Table(Rows_Offices_Sql(Office_Class));
    //}

    //public static string Politician_Current_Office_And_Status(string politicianKey)
    //{
    //  return Politician_Current_Office_And_Status(VotePage.GetPageCache(),
    //    politicianKey);
    //}

    //public static string Politician_Current_Election(string politicianKey)
    //{
    //  return Politician_Current_Election(VotePage.GetPageCache(), politicianKey);
    //}

    //public static string PoliticianName2Part4Google(string PoliticianKey)
    //{
    //  //Just two parts. 
    //  //Part 1 - either a complete first name, or complete middle name, or complete nickname
    //  //Part 2 - Last Name
    //  //DataRow PoliticianRow = db.Row_Optional(sql.PoliticianName(PoliticianKey));
    //  //if (PoliticianRow != null)
    //  if (Politicians.IsValid(PoliticianKey))
    //  {
    //    bool boolPart1 = false;
    //    string Part1 = string.Empty;
    //    string thePoliticianName;
    //    if (Politicians.GetFirstName(PoliticianKey, string.Empty) != string.Empty)
    //    {
    //      //thePoliticianName += db.Politician_FName(PoliticianKey);
    //      string FirstName = Politicians.GetFirstName(PoliticianKey, string.Empty).Replace(".", string.Empty);
    //      if (FirstName.Length > 1)
    //      {
    //        boolPart1 = true;
    //        Part1 = FirstName;
    //      }
    //    }
    //    if (!boolPart1)
    //    {
    //      string MiddleName = Politicians.GetMiddleName(PoliticianKey, string.Empty).Replace(".", string.Empty);
    //      if (MiddleName.Length > 1)
    //      {
    //        boolPart1 = true;
    //        Part1 = MiddleName;
    //      }
    //    }
    //    if (!boolPart1)
    //    {
    //      string Nickname =
    //        Politicians.GetNickname(PoliticianKey, string.Empty)
    //        .Replace("\"", string.Empty);
    //      if (Nickname.Length > 1)
    //      {
    //        Part1 = Nickname;
    //      }
    //    }
    //    thePoliticianName = Part1;

    //    string politicianName = Politicians.GetLastName(PoliticianKey, string.Empty);
    //    if (politicianName != string.Empty)
    //      thePoliticianName += " " + politicianName;

    //    return thePoliticianName;
    //  }
    //  else
    //    return string.Empty;
    //}

    //public static string PoliticianNameNickName4Google(string PoliticianKey)
    //{
    //  //If there is a nickname use just the Nickname and last name
    //  //DataRow PoliticianRow = db.Row_Optional(sql.PoliticianName(PoliticianKey));
    //  //if (PoliticianRow != null)
    //  string nickname =
    //    Politicians.GetNickname(PoliticianKey, string.Empty)
    //    .Replace("\"", string.Empty);
    //  if (Politicians.IsValid(PoliticianKey))
    //  {
    //    if (nickname.Length > 1)
    //    {
    //      return nickname + " " + Politicians.GetLastName(PoliticianKey, string.Empty);
    //    }
    //    else
    //      return string.Empty;
    //  }
    //  else
    //    return string.Empty;
    //}

    //public static string Politician_Address_Any_For_Textbox(
    //  string politicianKey)
    //{
    //  return Politician_Address_Any_For_Textbox(VotePage.GetPageCache(), politicianKey);
    //}

    //public static string Politician_CityStateZip_Any_For_Textbox(string politicianKey)
    //{
    //  return Politician_CityStateZip_Any_For_Textbox(VotePage.GetPageCache(), politicianKey);
    //}

    //public static string Answer_Issue_Question(string PoliticianKey, string QuestionKey)
    //{
    //  return db.Answer_Issue_Question(PoliticianKey, QuestionKey, false, false, false);
    //}

    //#region Misc
    //public static string PageTitleDesign(string PageDesc, string UserDesignCode)
    //{
    //  string Title = PageDesc;
    //  if (SecurePage.IsMasterUser)//only show DesignCode and StateCode4Data for Masters
    //  {
    //    //Title += "<br>for Design Code: " + db.DomainDesigns_Str(UserDesignCode, "DomainStateDataCode");
    //    Title += "<br>for Design Code: " + UserDesignCode;
    //  }
    //  return Title;
    //}
    //#endregion Misc

    //public static HtmlTableRow Add_Tr_To_Table_Return_Tr(HtmlTable htmlTable)
    //{
    //  return db.Add_Tr_To_Table_Return_Tr(htmlTable, string.Empty, string.Empty);
    //}

    //public static void Html_Table_Attributes_Report(ref HtmlTable htmlTable
    //  , db.ReportUser reportUser)
    //{
    //  db.Html_Table_Attributes_Report(ref htmlTable);
    //  if (reportUser == db.ReportUser.Public)
    //    htmlTable.Attributes["class"] = "tablePage";
    //  else
    //    htmlTable.Attributes["class"] = "tableAdmin";
    //}

    //public static string sql_Offices_ByOrder_ByDistrictCode(string stateCode, int officeClass)
    //{
    //  string sql = sql_Offices_Select();
    //  sql += " WHERE Offices.StateCode = " + db.SQLLit(stateCode);
    //  sql += " AND Offices.OfficeLevel = " + officeClass.ToString();
    //  sql += " ORDER BY Offices.OfficeOrderWithinLevel";
    //  //SQL += " ,CONVERT(int,Offices.DistrictCode)";
    //  sql += ",Offices.DistrictCode";
    //  return sql;
    //}

    //public static string sql_Offices_ByDistrictCode(string stateCode, int officeClass)
    //{
    //  string sql = sql_Offices_Select();
    //  sql += " WHERE Offices.StateCode = " + db.SQLLit(stateCode);
    //  sql += " AND Offices.OfficeLevel = " + officeClass.ToString();
    //  //SQL += " ORDER BY CONVERT(int,Offices.DistrictCode)";
    //  sql += " ORDER BY Offices.DistrictCode";
    //  return sql;
    //}

    //public static string sql_Offices_ByOrder(string stateCode, int officeClass)
    //{
    //  string sql = sql_Offices_Select();
    //  sql += " WHERE Offices.StateCode = " + db.SQLLit(stateCode);
    //  sql += " AND Offices.OfficeLevel = " + officeClass.ToString();
    //  sql += " ORDER BY Offices.OfficeOrderWithinLevel";
    //  return sql;
    //}

    //public static bool Is_Office_Class_Open(string stateCode, OfficeClass officeClass)
    //{
    //  return db.Is_Office_Class_Open(stateCode, string.Empty, string.Empty,
    //    officeClass);
    //}

    //#region enum

    //public enum ReportUser
    //{
    //  Public,
    //  Admin,
    //  Master
    //}

    //#endregion

    //#region /Master/AnswersEdit.aspx URLs & Anchors
    //public static string Url_Master_AnswersEdit(string PoliticianKey, string QuestionKey)
    //{
    //  return "/Master/AnswersEdit.aspx?Id=" + PoliticianKey
    //    + "&Question=" + QuestionKey;
    //}
    //#endregion

    //#region /Master/PoliticianFind.aspx URLs & Anchors
    //#endregion

    //public static string Rows_Offices_Sql(int Office_Class)
    //{
    //  return db.Rows_Offices_Sql(string.Empty, Office_Class);
    //}

    //public static string Politician_Current_Election(PageCache cache,
    //  string politicianKey)
    //{
    //  if (!string.IsNullOrEmpty(politicianKey) &&
    //    db.Is_Politician_In_Election_Upcoming_Viewable(cache, politicianKey))
    //  {
    //    return db.Elections_Str(cache.ElectionsPoliticians
    //      .GetFutureElectionKeyByPoliticianKey(politicianKey, true),
    //      "ElectionDesc");
    //  }
    //  else
    //    return string.Empty;
    //}

    //public static string Answer_Issue_Question(string politicianKey,
    //  string questionKey, bool includeLName, bool includeSource, bool includeDateStamp)
    //{
    //  const int maxLengthSource = 45;

    //  string answer = db.Answer(politicianKey, questionKey);

    //  if (answer != string.Empty)
    //  {
    //    #region Answer in database
    //    if (db.SCRIPT_NAME().ToLower() == "/politicianissue.aspx" ||
    //      db.SCRIPT_NAME().ToLower() == "/issue.aspx")
    //    {
    //      #region public pages
    //      answer = db.Answer_Truncate_To_Max_Chars(answer);

    //      #region Name
    //      if (includeLName)
    //        answer =
    //          "<span class=\"last-name\">" +
    //          //db.Politicians_Str(politicianKey, "LName") +
    //          Politicians.GetLastName(politicianKey, string.Empty) +
    //          ":</span> " +
    //          answer;
    //      #endregion Name

    //      #region Source
    //      string source = db.Answer_Source(politicianKey, questionKey);
    //      if (source != string.Empty && includeSource)
    //      {
    //        #region Note
    //        //IE won't break in middle of a long string 
    //        //with no delimiters, but Firefox will.
    //        //causing an issue table to exceed 940px
    //        //So breaks are forced at the / when the
    //        //source is too long.
    //        #endregion Note

    //        //If Source url longer then MaxLengthSource chars
    //        if (source.Length > maxLengthSource)
    //        {
    //          if (source.IndexOf("/", 0) != -1)
    //          {
    //            //If there is a / break at /
    //            int Loc = source.IndexOf("/", 0);
    //            source = source.Insert(Loc + 1, "<br>");
    //          }
    //          else
    //          {
    //            //break at Max length
    //            source = source.Insert(maxLengthSource, "<br>");
    //          }
    //        }

    //        answer += "<br><span class=\"source\"><span>Source:</span> " +
    //          source + "</span>";
    //      }
    //      #endregion Source

    //      #region Date
    //      //DateTime Date = Convert.ToDateTime(AnswerRow["DateStamp"]);
    //      DateTime date = db.Answer_Date(politicianKey, questionKey);
    //      //int Hour = DateTest.Hour;

    //      //if ((Convert.ToDateTime(AnswerRow["DateStamp"]) != Convert.ToDateTime("1/1/1900"))
    //      if (date != Convert.ToDateTime("1/1/1900") && includeDateStamp)
    //      {
    //        answer += " Date: ";
    //        //if (Convert.ToInt16(Date.Hour) == 0) //no HH:MM:SS
    //        answer += date.ToString("MM/dd/yyyy");
    //        //else
    //        //  Answer += Date.ToString();
    //      }
    //      #endregion Date

    //      return answer;
    //      #endregion public pages
    //    }
    //    else
    //    {
    //      #region  /Politician/Answer.aspx
    //      return answer;
    //      #endregion  /Politician/Answer.aspx
    //    }
    //    #endregion Answer in database
    //  }
    //  else
    //  {
    //    #region No Answer in database

    //    if (politicianKey != "NoRunningMate" &&
    //      (db.SCRIPT_NAME().ToLower() == "/politicianissue.aspx" ||
    //      db.SCRIPT_NAME().ToLower() == "/issue.aspx"))
    //    {
    //      #region public pages
    //      if (includeLName)
    //        return
    //          "<span class=\"last-name\">" +
    //          //db.Politicians_Str(politicianKey, "LName") +
    //          Politicians.GetLastName(politicianKey, string.Empty) +
    //          ":<span> " + db.No_Response;
    //      else
    //        return db.No_Response;
    //      #endregion public pages
    //    }
    //    else
    //    {
    //      #region  /Politician/Answer.aspx
    //      return string.Empty;
    //      #endregion  /Politician/Answer.aspx
    //    }
    //    #endregion No Answer in database
    //  }
    //}

    //#region Election Reports

    //#region Html Table Attributes

    //public static void Html_Table_Attributes_Report(ref HtmlTable htmlTable)
    //{
    //  htmlTable.Attributes["cellspacing"] = "0";
    //  htmlTable.Attributes["cellpadding"] = "0";
    //  htmlTable.Attributes["border"] = "0";
    //}

    //#endregion Html Table Attributes

    //#endregion Election Reports

    //public static string sql_Offices_Select()
    //{
    //  string sql = string.Empty;
    //  sql += " SELECT ";
    //  sql += " Offices.OfficeKey ";
    //  sql += ",Offices.StateCode ";
    //  sql += ",Offices.OfficeLevel ";
    //  sql += ",Offices.OfficeOrderWithinLevel ";
    //  sql += ",Offices.OfficeLine1 ";
    //  sql += ",Offices.OfficeLine2 ";
    //  sql += ",Offices.DistrictCode ";
    //  sql += ",Offices.DistrictCodeAlpha ";
    //  sql += ",Offices.CountyCode ";
    //  sql += ",Offices.LocalCode ";
    //  sql += ",Offices.OfficeOrderWithinLevel ";
    //  sql += ",Offices.IsRunningMateOffice ";
    //  sql += ",Offices.Incumbents ";
    //  sql += ",Offices.VoteInstructions ";
    //  sql += ",Offices.VoteForWording ";
    //  sql += ",Offices.WriteInInstructions ";
    //  sql += ",Offices.WriteInWording ";
    //  sql += ",Offices.WriteInLines ";
    //  sql += ",Offices.IsInactive ";
    //  sql += " FROM Offices ";
    //  //SQL += " WHERE Offices.IsOfficeTagForDeletion = 0";
    //  return sql;
    //}

    //public static bool Is_Office_Class_Open(string stateCode, string countyCode,
    //  string localCode, OfficeClass officeClass)
    //{
    //  return db.Rows_Offices(stateCode, countyCode, localCode, officeClass) > 0 ||
    //    !OfficesAllIdentified.GetIsOfficesAllIdentified(stateCode,
    //    officeClass.ToInt(), countyCode, localCode);
    //}

    //public static int Rows_Offices(string stateCode, string countyCode,
    //  string localCode, OfficeClass officeClass)
    //{
    //  var sql = string.Empty;
    //  sql += " Offices ";
    //  sql += " WHERE Offices.StateCode = " + SQLLit(stateCode);
    //  if (!string.IsNullOrEmpty(countyCode))
    //    sql += " AND Offices.CountyCode = " + SQLLit(countyCode);
    //  if (!string.IsNullOrEmpty(localCode))
    //    sql += " AND Offices.LocalCode = " + SQLLit(localCode);
    //  if (officeClass != OfficeClass.Undefined)
    //    sql += " AND Offices.OfficeLevel = " + officeClass.ToInt();
    //  return Rows_Count_From(sql);
    //}

    //public static string Rows_Offices_Sql(string State_Code, int Office_Class)
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT";
    //  SQL += " Offices.OfficeKey";
    //  SQL += ",Offices.StateCode";
    //  SQL += ",Offices.OfficeLevel";
    //  SQL += ",Offices.OfficeOrderWithinLevel";
    //  SQL += ",Offices.OfficeLine1";
    //  SQL += ",Offices.OfficeLine2";
    //  SQL += ",Offices.DistrictCode";
    //  SQL += ",Offices.DistrictCodeAlpha";
    //  SQL += ",Offices.CountyCode";
    //  SQL += ",Offices.LocalCode";
    //  SQL += ",Offices.OfficeOrderWithinLevel";
    //  SQL += ",Offices.IsRunningMateOffice";
    //  SQL += ",Offices.Incumbents";
    //  SQL += ",Offices.VoteInstructions";
    //  SQL += ",Offices.VoteForWording";
    //  SQL += ",Offices.WriteInInstructions";
    //  SQL += ",Offices.WriteInWording";
    //  SQL += ",Offices.WriteInLines";
    //  SQL += ",Offices.IsInactive";
    //  SQL += " FROM Offices";
    //  SQL += " WHERE Offices.OfficeLevel = " + Office_Class.ToString();
    //  if (!string.IsNullOrEmpty(State_Code))
    //    SQL += " AND Offices.StateCode = " + db.SQLLit(State_Code);
    //  SQL += " ORDER BY Offices.OfficeOrderWithinLevel";
    //  return SQL;
    //}
    //public const string No_Response = "no response";

    //public static string Answer(string PoliticianKey, string QuestionKey)
    //{
    //  return db.Answers_Optional(PoliticianKey, QuestionKey, "Answer");
    //}
    //public static string Answer_Source(string PoliticianKey, string QuestionKey)
    //{
    //  return db.Answers_Optional(PoliticianKey, QuestionKey, "Source");
    //}
    //public static DateTime Answer_Date(
    //  string PoliticianKey
    //  , string QuestionKey)
    //{
    //  return db.AnswersDate_Optional(
    //    PoliticianKey
    //    , QuestionKey, "DateStamp");
    //}


    //public static string Answer_Truncate_To_Max_Chars(string Answer)
    //{
    //  if (Answer.Length > db.Max_Answer_Length_2000)
    //  {
    //    Answer = Answer.Remove(db.Max_Answer_Length_2000);
    //    Answer += " [Response was truncated to maximum response length of "
    //      + db.Max_Answer_Length_2000.ToString()
    //      + " characters.] ";
    //  }
    //  return Answer;
    //}

    //public static string Answers_Optional(
    //  string PoliticianKey
    //  , string QuestionKey
    //  , string Column)
    //{
    //  if (db.Rows("Answers"
    //    , "PoliticianKey", PoliticianKey
    //    , "QuestionKey", QuestionKey) == 1)
    //    return db.Answers(PoliticianKey, QuestionKey, Column);
    //  else
    //    return string.Empty;
    //}

    //public static DateTime AnswersDate_Optional(
    //  string PoliticianKey
    //  , string QuestionKey, string Column)
    //{
    //  if (db.Rows("Answers", "PoliticianKey", PoliticianKey, "QuestionKey", QuestionKey) == 1)
    //    return db.AnswersDate(PoliticianKey, QuestionKey, Column);
    //  else
    //    return DateTime.MinValue;
    //}

    //#region Answers Table
    ////public const int Max_Answer_Length_2000 = 2000;
    ////public const int Max_Answer_Length_1000 = 1000;

    //public static string Answers(string PoliticianKey, string QuestionKey, string Column)
    //{
    //  return db.Double_Key_Str("Answers", Column, "PoliticianKey", PoliticianKey, "QuestionKey", QuestionKey);
    //}
    //public static DateTime AnswersDate(string PoliticianKey, string QuestionKey, string Column)
    //{
    //  if ((PoliticianKey != string.Empty) && (QuestionKey != string.Empty))
    //    return db.Double_Key_Date("Answers", Column, "PoliticianKey", PoliticianKey, "QuestionKey", QuestionKey);
    //  else
    //    return DateTime.MinValue;
    //}


    //#endregion Answers Table

    //public static string Double_Key_Str(string Table, string Column, string KeyName1, string KeyValue1, string KeyName2, string KeyValue2)
    //{
    //  string SQL = "SELECT " + Column.Trim() + " FROM " + Table.Trim()
    //    + " WHERE " + KeyName1.Trim() + " = " + db.SQLLit(KeyValue1.Trim())
    //    + " AND " + KeyName2.Trim() + " = " + db.SQLLit(KeyValue2.Trim());
    //  DataTable table = db.Table(SQL);
    //  if (table.Rows.Count == 1)
    //  {
    //    return (string) table.Rows[0][Column].ToString().Trim();
    //  }
    //  else
    //  {
    //    throw new ApplicationException(db.DBErrorMsg(SQL, "Did not find a single row"));
    //  }
    //}
    //public static bool Double_Key_Bool(string Table, string Column, string KeyName1, string KeyValue1, string KeyName2, int KeyValue2)
    //{
    //  string SQL = "SELECT " + Column.Trim() + " FROM " + Table.Trim()
    //    + " WHERE " + KeyName1.Trim() + " = " + db.SQLLit(KeyValue1.Trim())
    //    + " AND " + KeyName2.Trim() + " = " + KeyValue2.ToString();
    //  DataRow Row = db.Row(SQL);
    //  if (Row != null)
    //  {
    //    return (bool)Row[Column];
    //  }
    //  else
    //  {
    //    throw new ApplicationException(db.DBErrorMsg(SQL, "Did not find a single row"));
    //  }
    //}

    //public static DateTime Double_Key_Date(string Table, string Column, string KeyName1, string KeyValue1, string KeyName2, string KeyValue2)
    //{
    //  string SQL = "SELECT " + Column.Trim() + " FROM " + Table.Trim()
    //    + " WHERE " + KeyName1.Trim() + " = " + db.SQLLit(KeyValue1.Trim())
    //    + " AND " + KeyName2.Trim() + " = " + db.SQLLit(KeyValue2.Trim());
    //  DataRow Row = db.Row(SQL);
    //  if (Row != null)
    //  {
    //    return (DateTime) Row[Column];
    //  }
    //  else
    //  {
    //    throw new ApplicationException(db.DBErrorMsg(SQL, "Did not find a single row"));
    //  }
    //}
    //public static bool Is_Valid_Office_Politician(string OfficeKey, string PoliticianKey)
    //{
    //  if (db.Rows(sql.OfficesOfficials_Office_Politician(OfficeKey, PoliticianKey)) > 0)
    //    return true;
    //  else
    //    return false;
    //}

    //#region OfficesOfficials SELECT
    //public static string OfficesOfficials_Str(string OfficeKey, string Column)
    //{
    //  return db.Single_Key_Str_Optional("OfficesOfficials", Column, "OfficeKey", OfficeKey);
    //}
    //#endregion

    //#region OfficesOfficials UPDATE

    //public static void OfficesOfficialsUpdate(string OfficeKey, string PoliticianKey, string Column, string ColumnValue)
    //{
    //  db.Double_Key_Update_Str("OfficesOfficials", Column, ColumnValue, "OfficeKey", OfficeKey, "PoliticianKey", PoliticianKey);
    //}
    //#endregion

    //public static void Log_OfficesOfficials_Change(string politicianKey,
    //  string officeKey, string stateCode, string dataItem,
    //  string dataFrom, string dataTo)
    //{
    //  DB.VoteLog.LogOfficeOfficialChanges.Insert(
    //    DateTime.Now,
    //    SecurePage.UserSecurityClass,
    //    SecurePage.UserName,
    //    politicianKey,
    //    officeKey,
    //    stateCode,
    //    string.Empty,
    //    string.Empty,
    //    dataItem,
    //    dataFrom,
    //    dataTo);
    //}

    //#region OfficesOfficials INSERT
    //public static void OfficesOfficials_INSERT(string OfficeKey, string PoliticianKey)
    //{
    //  string SQL = string.Empty;
    //  SQL += " INSERT INTO OfficesOfficials ";
    //  SQL += "(";
    //  SQL += "OfficeKey";
    //  SQL += ",StateCode";
    //  SQL += ",PoliticianKey";
    //  SQL += ",DataLastUpdated";
    //  SQL += ",UserSecurity";
    //  SQL += ",UserName";
    //  SQL += ")";
    //  SQL += " VALUES ";
    //  SQL += "(";
    //  SQL += db.SQLLit(OfficeKey);
    //  SQL += "," + db.SQLLit(Offices.GetStateCodeFromKey(OfficeKey));
    //  SQL += "," + db.SQLLit(PoliticianKey);
    //  SQL += "," + db.SQLLit(Db.DbNow);
    //  SQL += "," + db.SQLLit(SecurePage.UserSecurityClass);
    //  SQL += "," + db.SQLLit(SecurePage.UserName);
    //  SQL += ")";

    //  //db.ExecuteSQL(sql.OfficesOfficials_Insert(
    //  //  OfficeKey
    //  //  , PoliticianKey));
    //  db.ExecuteSql(SQL);

    //  db.Log_OfficesOfficials_Add_Or_Delete(
    //    "A"
    //    , OfficeKey
    //    , PoliticianKey
    //    );
    //}
    //#endregion


    //public static void Double_Key_Update_Str(string Table, string Column, string ColumnValue, string KeyName1, string KeyValue1, string KeyName2, string KeyValue2)
    //{
    //  string UpdateSQL = "UPDATE " + Table
    //    + " SET " + Column + " = " + db.SQLLit(ColumnValue)
    //    + " WHERE " + KeyName1 + " = " + db.SQLLit(KeyValue1)
    //    + " AND " + KeyName2 + " = " + db.SQLLit(KeyValue2);
    //  db.ExecuteSql(UpdateSQL);
    //}

    //#region Office Type
    //public const int Type_All = 0;
    //public const int Type_Executive = 1;
    //public const int Type_Legislative = 2;
    //public const int Type_SchoolBoard = 3;
    //public const int Type_Commission = 4;
    //public const int Type_Judicial = 5;
    //public const int Type_Party = 6;
    //#endregion Office Type

    //public const int Electoral_Undefined = 99;
    //public const int Electoral_All = 0;
    //public const int Electoral_Federal = 1;
    //public const int Electoral_State = 2;
    //public const int Electoral_Multi_Counties = 3;
    //public const int Electoral_County = 4;
    //public const int Electoral_Multi_Locals = 5;
    //public const int Electoral_Local = 6;

    //public static string State_Code_Security_Login()
    //{
    //  if (SecurePage.IsMasterUser)
    //    //StateCode passed as query string in /Master/Default.aspx
    //    //return User_Security_StateCode();
    //    return db.User_StateCode();
    //  else
    //    //return HttpContext.Current.Session["UserStateCode"].ToString();
    //    return db.Security_Str(SecurePage.UserName, "UserStateCode");
    //}
    //public static string County_Code_Security_Login()
    //{
    //  return db.Security_Str(SecurePage.UserName, "UserCountyCode");
    //}
    //public static string Local_Code_Security_Login()
    //{
    //  return db.Security_Str(SecurePage.UserName, "UserLocalCode");
    //}

    //public static string PoliticianKey_ViewState()
    //{
    //  #region Note:
    //  //Politician, Master, Admin Interns, and Party users
    //  //obtain the PoliticianKey used if ViewState["PoliticianKey"]
    //  //for all pages in the Politician Folder here
    //  #endregion Note:
    //  if (
    //    (SecurePage.IsMasterUser)
    //    || (db.Is_User_Admin_DataEntry())
    //    || (SecurePage.IsPartyUser)
    //    )
    //  {
    //    #region Special Users who can enter Politician Data
    //    if (!string.IsNullOrEmpty(VotePage.QueryId))
    //    {
    //      return VotePage.QueryId;
    //    }
    //    else
    //    {
    //      return string.Empty;
    //    }
    //    #endregion Special Users who can enter Politician Data
    //  }
    //  else
    //  {
    //    #region Politician
    //    return SecurePage.UserName;
    //    #endregion Politician
    //  }
    //}

    //public static string Name_Electoral_Plus_Text(
    //  string textBeforeName
    //  , string textAfterName
    //  )
    //{
    //  return db.Name_Electoral_Plus_Text(
    //         textBeforeName
    //        , textAfterName
    //        , true
    //        );
    //}
    //public static bool Is_Domain_SingleState()
    //{
    //  return StateCache.IsValidStateCode(db.Domain_DataCode_This());
    //}

    //#region Is Page

    //public static bool Is_Page_Issues()
    //{
    //  switch (db.SCRIPT_NAME().ToLower())
    //  {
    //    case "/admin/issues.aspx":
    //      return true;
    //    case "/admin/issue.aspx":
    //      return true;
    //    case "/admin/issuesreport.aspx":
    //      return true;
    //    case "/admin/issuesquestions.aspx":
    //      return true;
    //    default:
    //      return false;
    //  }
    //}
    //#endregion  Is Page

    //#region Anchor Utilities

    //public static string Center(string text2Center)
    //{
    //  return "<center>" + text2Center + "</center>";
    //}

    //#endregion Anchor Utilities
    //public static string Anchor(string url, string anchorText, string toolTip)
    //{
    //  return db.Anchor(url, anchorText, toolTip, string.Empty);
    //}

    //public static string Anchor(Uri uri, string anchorText, string toolTip,
    //  string target)
    //{
    //  return Anchor(uri.ToString(), anchorText, toolTip, target);
    //}


    //#region / root urls & anchors Multi pages

    //#region /Issue.aspx or PoliticianIssue.aspx URLs & Anchors (ok)
    //public static string Url_Issue_Or_PoliticianIssue(string issueKey, string politicianKey)
    //{
    //  if (db.Is_Politician_In_Election_Upcoming_Viewable(politicianKey))
    //  {
    //    //to Issue.aspx
    //    //return db.Url_Issue(
    //    //  db.Politician_Election_Upcoming_Viewable_Row(PoliticianKey)["ElectionKey"].ToString()
    //    //  , Politicians.GetOfficeKey(PoliticianKey, string.Empty)
    //    //  , IssueKey
    //    //  );
    //    string officeKey =
    //      VotePage.GetPageCache().Politicians.GetOfficeKey(politicianKey);
    //    return db.Url_Issue(
    //      ElectionsPoliticians.GetFutureOfficeKeyInfoByPoliticianKey(politicianKey, true).ElectionKey,
    //        officeKey, issueKey);
    //  }
    //  else
    //  {
    //    //### changed
    //    //to PoliticianIssue.aspx
    //    //return db.Url_PoliticianIssue_ALLPersonal_If_Issues_List_Key(
    //    //  IssueKey
    //    //  );
    //    return db.Url_PoliticianIssue_ALLPersonal_If_Issues_List_Key(
    //      politicianKey, issueKey);
    //  }
    //}
    //#endregion

    //#endregion / root urls & anchors Multi pages
    //public static string Anchor_Admin_Default(string stateCode, string countyCode,
    //  string localCode, string anchorText)
    //{
    //  return db.Anchor_Admin_Default(
    //    stateCode
    //    , countyCode
    //    , localCode
    //    , anchorText
    //    , string.Empty);
    //}
    //public static string Url_Admin_Elections()
    //{
    //  return db.Url_Admin_Elections(string.Empty);
    //}
    //public static string Url_Admin_Election(
    // string electionKey)
    //{
    //  //string Url = db.Url_Admin_Election();
    //  //Url += "&Election=" + ElectionKey;
    //  //Url += db.Url_Add_State_County_Local_Codes();
    //  //return db.Fix_Url_Parms(Url);

    //  return db.Url_Admin_Election(
    //   electionKey
    //  , true);

    //}
    //public static string Url_Admin_Election_Offices(string electionKey)
    //{
    //  string Url = db.Url_Admin_Election_Offices();
    //  Url += "&Election=" + electionKey;
    //  Url += db.Url_Add_State_County_Local_Codes();
    //  return db.Fix_Url_Parms(Url);
    //}
    //public static string Url_Admin_Offices(int officeClass, string stateCode)
    //{
    //  string Url = Url_Admin_Offices();
    //  Url += "&Class=" + officeClass;
    //  Url += db.Url_Add_State_County_Local_Codes();
    //  return db.Fix_Url_Parms(Url);
    //}

    //public static string Url_Admin_Politicians(OfficeClass officeClass, string sort)
    //{
    //  string Url = db.Url_Admin_Politicians();
    //  Url += "&Class=" + officeClass.ToInt();
    //  Url += "&Sort=" + sort;
    //  Url += db.Url_Add_State_County_Local_Codes();
    //  return db.Fix_Url_Parms(Url);
    //}

    //public static string Ur4AdminReferendums(string electionKey)
    //{
    //  string url = db.Ur4AdminReferendums();
    //  url += "&Election=" + electionKey;
    //  url += db.Url_Add_State_County_Local_Codes();
    //  return db.Fix_Url_Parms(url);
    //}

    //#region /Master/ URLs & Anchors

    //#region /Master/Default.aspx URLs & Anchors
    //public static string Url_Master_Default()
    //{
    //  return @"/Master/Default.aspx";
    //}

    //#endregion

    //#region x/Master/Parties.aspx URLs & Anchors

    //#endregion x/Master/Parties.aspx URLs & Anchors

    //#endregion
    //public static string Url_Politician_Default()
    //{
    //  //string Url = @"/Politician/Default.aspx";
    //  //Url += db.Politician_Id_Add_To_QueryString_Master_User();
    //  //return db.Fix_Url_Parms(Url);
    //  return db.Url_Politician_Default(string.Empty);
    //}
    //public static string Url_Politician_IssueQuestions(string issueKey)
    //{
    //  return db.Url_Politician_IssueQuestions(
    //   issueKey
    //  , string.Empty
    //  );
    //}

    //#region /Politician/SearchEngineSubmit.aspx URLs & Anchors
    //public static string Url_Politician_SearchEngineSubmitCompare(
    //  string politicianKey
    //  , string issueKey
    //  )
    //{
    //  string Url;
    //  if (db.Is_Politician_In_Election_Upcoming_Viewable(politicianKey))
    //  {
    //    Url = @"/Politician/SearchEngineSubmit.aspx";
    //    //+ "?Id=" + PoliticianKey
    //    Url += "&Page=Issue";
    //  }
    //  else
    //  {
    //    //Url = @"/Politician/SearchEngineSubmit.aspx?Page=PoliticianIssue";
    //    Url = @"/Politician/SearchEngineSubmit.aspx";
    //    //+ "?Id=" + PoliticianKey
    //    Url += "&Page=PoliticianIssue";
    //  }

    //  if (!string.IsNullOrEmpty(issueKey))
    //    Url += "&Issue=" + issueKey;

    //  Url += db.Politician_Id_Add_To_QueryString_Master_User();

    //  return db.Fix_Url_Parms(Url);
    //}

    //public static string Url_Politician_SearchEngineSubmit_Intro_Page()
    //{
    //  string Url = @"/Politician/SearchEngineSubmit.aspx";
    //  Url += "&Page=Intro";
    //  Url += db.Politician_Id_Add_To_QueryString_Master_User();
    //  return db.Fix_Url_Parms(Url);
    //}
    //#endregion

    //#region Urls - Url or Empty
    //public static string Url_ImageBannerOrEmpty(string DesignCode)
    //{
    //  string Extn;
    //  if (VotePage.IsPublicPage)
    //  {
    //    Extn = db.ExtnImageOrEmpty(db.Path_Part_Image_Banner_NoExtn(DesignCode));
    //    if (Extn != string.Empty)
    //      return db.Url_Image_Banner(DesignCode, Extn);
    //    else
    //      return string.Empty;
    //  }
    //  else
    //  {
    //    Extn = db.ExtnImageOrEmpty(db.Path_Part_Image_Banner_Admin_NoExtn(DesignCode));
    //    if (Extn != string.Empty)
    //      return db.Url_Image_Banner_Admin(DesignCode, Extn);//AdminTable Banner
    //    else
    //    {
    //      //Public Banner
    //      Extn = db.ExtnImageOrEmpty(db.Path_Part_Image_Banner_NoExtn(DesignCode));
    //      if (Extn != string.Empty)
    //        return db.Url_Image_Banner(DesignCode, Extn);
    //      else
    //        return string.Empty;
    //    }
    //  }
    //}
    //public static string Url_ImageTagLineOrEmpty(string DesignCode)
    //{
    //  //return db.Url_Image_OrEmpty(db.Path_Part_Image_TagLine_NoExtn(DesignCode), db.Url_ImageTagLine_NoExtn(DesignCode));

    //  string Extn = db.ExtnImageOrEmpty(db.Path_Part_Image_TagLine_NoExtn(DesignCode));
    //  if (Extn != string.Empty)
    //    return db.Url_Image_TagLine(DesignCode, Extn);
    //  else
    //    return string.Empty;
    //}
    //#endregion Urls - Url or Empty
    //public static string DesignCode_Domain_This()
    //{
    //  return db.Domain_DesignCode_This();
    //}

    //public static bool Is_Include_Banner_This()
    //{
    //  return db.DomainDesigns_Bool_This("IsIncludedBannerAllPages");
    //}


    //public static string Elections4Navbar(string ElectionKey, string Column)
    //{
    //  //For cases where a County Elections row does not exist
    //  if (db.Is_Valid_Election(ElectionKey))
    //    return db.Single_Key_Str("Elections", Column, "ElectionKey", ElectionKey);
    //  else
    //    return db.Single_Key_Str("Elections", Column, "ElectionKey", db.ElectionKey_State(ElectionKey));
    //}

    //public static string Issues_List_Office(string officeKey)
    //{
    //  return Issues_List_Office(VotePage.GetPageCache(), officeKey);
    //}

    //public static bool Is_User_Admin_DataEntry()
    //{
    //  return db.User() == db.UserType.AdminIntern;
    //}

    //public static string Url_Issue(
    //  string electionKey
    //  , string officeKey
    //  , string issueKey
    //  )
    //{
    //  return db.Url_Issue(
    //    electionKey
    //    , officeKey
    //    , issueKey
    //    , string.Empty
    //    );
    //}

    //public static string Url_PoliticianIssue_ALLPersonal_If_Issues_List_Key(
    //  string politicianKey
    //  , string issueKey
    //  )
    //{
    //  if (
    //    (issueKey != string.Empty)
    //    && (issueKey != db.Issues_List(issueKey))
    //    )
    //    return db.Url_PoliticianIssue(politicianKey, issueKey);
    //  else
    //    #region Note
    //    //There is no Biographical Issue on the /PoliticianIssue.aspx page
    //    //However THERE IS a Biographical comparison on the /Issue.aspx page
    //    #endregion Note
    //    return db.Url_PoliticianIssue(politicianKey, "ALLPersonal");
    //  //return db.Url_PoliticianIssue("ALLBio");
    //}
    //------------------

    //Anchors
    //public static string Anchor_Admin_Default(
    //  string stateCode
    //  , string countyCode
    //  , string localCode
    //  , string anchorText
    //  //, string ToolTip
    //  , string Target
    //  )
    //{
    //  string anchor = string.Empty;
    //  anchor += "<a href=";

    //  anchor += "\"";
    //  //if (LocalCode != string.Empty)
    //  anchor += db.Url_Admin_Default(stateCode, countyCode, localCode);
    //  //else if (CountyCode != string.Empty)
    //  //  Anchor += db.Url_Admin_Default(StateCode, CountyCode);
    //  //else
    //  //  Anchor += db.Url_Admin_Default(StateCode);
    //  anchor += "\"";

    //  //if (ToolTip != string.Empty)
    //  //{
    //  //  Anchor += " title=";
    //  //  Anchor += "\"";
    //  //  Anchor += ToolTip;
    //  //  Anchor += "\"";
    //  //}
    //  anchor += " target=";
    //  anchor += "\"";
    //  if (Target != string.Empty)
    //    anchor += Target;
    //  else
    //    anchor += "_self";

    //  anchor += "\"";

    //  anchor += ">";

    //  //Anchor += "<center>";
    //  if (anchorText != string.Empty)
    //  {
    //    anchor += anchorText;
    //  }
    //  else
    //  {
    //    if (localCode != string.Empty)
    //      anchor += VotePage.GetPageCache()
    //        .LocalDistricts
    //        .GetLocalDistrict(stateCode, countyCode, localCode);
    //    else if (countyCode != string.Empty)
    //      anchor += CountyCache.GetCountyName(stateCode, countyCode);
    //    else
    //      anchor += StateCache.GetStateName(stateCode);
    //  }
    //  //Anchor += "</center>";

    //  anchor += "</a>";
    //  return anchor;
    //}

    //----
    //public static string Url_Admin_Election(
    //  string electionKey
    //  , bool isAddStateCountyLocalCodes)
    //{
    //  string url = db.Url_Admin_Election();
    //  url += "&Election=" + electionKey;
    //  if (isAddStateCountyLocalCodes)
    //    url += db.Url_Add_State_County_Local_Codes();
    //  return db.Fix_Url_Parms(url);
    //}

    //#region /Admin/ElectionOffices.aspx URLs & Anchors
    //public static string Url_Admin_Election_Offices()
    //{
    //  return "/Admin/ElectionOffices.aspx";
    //}

    //#endregion

    //#region /Admin/Referendum.aspx URLs & Anchors
    //public static string Ur4AdminReferendums()
    //{
    //  return "/Admin/Referendum.aspx";
    //}

    //#endregion
    //public static string Url_Politician_Default(string politicianKey)
    //{
    //  string Url = @"/Politician/Default.aspx";
    //  if (!string.IsNullOrEmpty(politicianKey))
    //    Url += "&Id=" + politicianKey;
    //  else
    //    Url += db.Politician_Id_Add_To_QueryString_Master_User();
    //  return db.Fix_Url_Parms(Url);
    //}

    //#region Urls - Simple Image and css
    ////Image
    //public static string Url_Image_Banner(string DesignCode, string Extn)
    //{
    //  return @"/images/Designs/" + DesignCode + @"/" + "Banner" + Extn;
    //}
    //public static string Url_Image_Banner_Admin(string DesignCode, string Extn)
    //{
    //  return @"/images/Designs/" + DesignCode + @"/" + "BannerAdmin" + Extn;
    //}
    //public static string Url_Image_TagLine(string DesignCode, string Extn)
    //{
    //  return @"/images/Designs/" + DesignCode + @"/" + "TagLine" + Extn;
    //}

    //#endregion Urls - Simple
    //public static string ExtnImageOrEmpty(string Path_Part_NoExtn)
    //{
    //  return ExtnImageOrEmpty_(Path_Part_NoExtn);
    //}

    //#region Path Part - Image and css
    ////image
    //public static string Path_Part_Image_Banner_NoExtn(string DesignCode)
    //{
    //  return @"images\Designs\" + DesignCode + @"\" + "Banner";
    //}
    //public static string Path_Part_Image_Banner_Admin_NoExtn(string DesignCode)
    //{
    //  return @"images\Designs\" + DesignCode + @"\" + "BannerAdmin";
    //}
    //public static string Path_Part_Image_TagLine_NoExtn(string DesignCode)
    //{
    //  return @"images\Designs\" + DesignCode + @"\" + "Tagline";
    //}
    //#endregion Path Part

    //public static bool Is_Politician_In_Election_Upcoming_Viewable(string politicianKey)
    //{
    //  return Is_Politician_In_Election_Upcoming_Viewable(VotePage.GetPageCache(), politicianKey);
    //}

    //public static string Issues_List_Office(PageCache cache, string officeKey)
    //{
    //  string issueLevel = db.IssueLevel_Office(cache, officeKey);
    //  string stateCode;
    //  if (issueLevel == "B")
    //    stateCode = "US";
    //  else //State Level office
    //    stateCode = db.StateCode4OfficeKey4Domain(cache, officeKey);//US or StateCode
    //  return issueLevel + stateCode + "IssuesList";
    //}

    //#region /Issue.aspx URLs & Anchors (ok)
    //public static string Url_Issue(
    //  string electionKey
    //  , string officeKey
    //  , string issueKey
    //  , string stateCode
    //  )
    //{
    //  #region Note: Definition of IssueKeys
    //  //IssueKeys:
    //  //IssueLevel[0,1]
    //  //IssueGroup[1,2]
    //  //CountyCode[2,3] not implemented
    //  //IssueAbreviation[3,x] any short description of the issue
    //  //
    //  //IssueLevel
    //  //A = All Offices
    //  //B = National Issues for President, US Senate, US Congress
    //  //C = State Issues for Statewide, State Senate, State House Offices (with State Code)
    //  //D = County Issues (with State Code and County Name)
    //  //E = Local Issues (with State Code, County Name, Local District)
    //  //
    //  //StateCode
    //  //For Level A = LL - indicating all politicians
    //  //For Level B = US - National US Issues
    //  //For Level C and higher = StateCode
    //  //
    //  //Examples IssueKeys (depends on issue level):
    //  //
    //  //ALLPersonal -> Personal and Biographical Questions to all politicians
    //  //BUSFood -> Federal offices for Food Issues
    //  //CPAEducation -> Pennsylvania offices for Education Issues
    //  //DVA059Transportation -> FairfaxCounty, Virginia office for Transportaion issues
    //  //DVA05911Transportation -> Hunter Mill District, FairfaxCounty, Virginia office for Transportaion issues
    //  //
    //  //Special IssueKeys not in the IssuesTable in database as an IssueKey but treated like an IssueKey:
    //  //
    //  //BUSIssuesList -> List of all issues for federal offices (US President, Senate and House)
    //  //CVAIssuesList -> List of all issues for Virginia
    //  #endregion Note: Definition of IssueKeys

    //  #region StateCode used for State= query string parameter
    //  if (string.IsNullOrEmpty(stateCode))
    //    stateCode = Offices.GetStateCodeFromKey(officeKey);
    //  if (string.IsNullOrEmpty(stateCode))
    //    stateCode = Elections.GetStateCodeFromKey(electionKey);
    //  #endregion StateCode used for State= query string parameter

    //  //if (db.Is_StateCode_State(stateCode))
    //  if (db.Is_StateCode_State_Or_National_Party_Contest(stateCode))
    //  {
    //    #region replaced
    //    //string Url = string.Empty;
    //    //Url += db.Domain_State_Or_USA(State_Code);
    //    ////### Removed /
    //    ////Url += @"/Issue.aspx";
    //    //Url += @"Issue.aspx";

    //    //Url += "&State=" + State_Code;

    //    //if (ElectionKey != string.Empty)
    //    //  Url += "&Election=" + ElectionKey;

    //    //if (OfficeKey != string.Empty)
    //    //  Url += "&Office=" + OfficeKey;

    //    //if (IssueKey != string.Empty)
    //    //  Url += "&Issue=" + HttpUtility.UrlEncode(IssueKey);
    //    //else
    //    //  //Url += "&Issue=ALLPersonal";
    //    //  Url += "&Issue=ALLBio";
    //    #endregion replaced

    //    if (string.IsNullOrWhiteSpace(issueKey))
    //      issueKey = "ALLBio";

    //    //Uri uri = UrlManager.GetIssuePageUri(
    //    //  stateCode, electionKey, officeKey, issueKey);

    //    Uri uri = UrlManager.GetCompareCandidatesPageUri(
    //      stateCode, electionKey, officeKey);

    //    return uri.ToString();
    //  }
    //  else
    //    return string.Empty;
    //}


    //#endregion
    //public static string Url_PoliticianIssue(string politicianKey, string issueKey)
    //{
    //  return db.Url_PoliticianIssue(politicianKey, issueKey, string.Empty);
    //}

    //#region /Admin/Election.aspx URLs & Anchors
    //public static string Url_Admin_Election()
    //{
    //  return "/Admin/Election.aspx";
    //}
    //#endregion

    //#region Path Utilities
    //public static string ExtnImageOrEmpty_(string Path_Part_NoExtn)
    //{
    //  string Extn;
    //  if (File.Exists(VotePage.GetServerPath() + Path_Part_NoExtn + ".jpg"))
    //    Extn = ".jpg";
    //  else if (File.Exists(VotePage.GetServerPath() + Path_Part_NoExtn + ".gif"))
    //    Extn = ".gif";
    //  else if (File.Exists(VotePage.GetServerPath() + Path_Part_NoExtn + ".png"))
    //    Extn = ".png";
    //  else
    //    Extn = string.Empty;
    //  return Extn;
    //}
    //#endregion Path Utilities

    //public static string StateCode4OfficeKey4Domain(PageCache cache, string officeKey)
    //{
    //  switch (Offices.GetStateCodeFromKey(officeKey))
    //  {
    //    case "U1":
    //      return "US";
    //    case "U2":
    //      return "US";
    //    case "U3":
    //      return "US";
    //    case "U4":
    //      return "US";
    //    default:
    //      return Offices.GetStateCodeFromKey(officeKey);
    //  }
    //}

    //public static string IssueLevel_Office(PageCache cache, string officeKey)
    //{
    //  if (Offices.GetOfficeClass(cache, officeKey) <= OfficeClass.USHouse)
    //    return "B";
    //  return Offices.GetOfficeClass(cache, officeKey) <= OfficeClass.StateHouse
    //    ? "C"
    //    : "D";
    //}

    //public static bool Is_StateCode_State_Or_National_Party_Contest(string stateCode)
    //{
    //  return StateCache.IsValidStateCode(stateCode)
    //    || db.Is_StateCode_National_Party_Contest(stateCode);
    //}

    //#region /PoliticianIssue.aspx URLs & Anchors
    //public static string Url_PoliticianIssue(
    //  string politicianKey
    //  , string issueKey
    //  , string stateCode
    //  )
    //{
    //  //### Changed 
    //  //if (string.IsNullOrEmpty(State_Code))
    //  //  State_Code = db.StateCode_In_PoliticianKey(PoliticianKey);
    //  //if (string.IsNullOrEmpty(State_Code))
    //  //  State_Code = db.StateCode_In_IssueKey(IssueKey);
    //  if (string.IsNullOrEmpty(stateCode))
    //  {
    //    if (!string.IsNullOrEmpty(politicianKey))
    //      stateCode = Politicians.GetStateCodeFromKey(politicianKey);
    //  }
    //  //----

    //  if (!string.IsNullOrEmpty(stateCode))
    //  {
    //    return UrlManager.GetPoliticianIssuePageUri(stateCode,
    //      politicianKey, issueKey).ToString();
    //  }
    //  else
    //    return string.Empty;
    //}


    //#endregion

    //#region StateCode and DataCode for NavbarState and Domains

    //public static bool Is_StateCode_National_Party_Contest(string stateCode)
    //{
    //  return stateCode == "US" || stateCode == "PP";
    //}

    //#endregion StateCode and DataCode for Domains

    //public static bool Is_User_Master_Restricted()
    //{
    //  return !db.Is_User_Master_Unrestricted();
    //}

    //public static bool Is_User_Master_Unrestricted()
    //{
    //  string sql = "Security";
    //  sql += " WHERE UserName = " + db.SQLLit(SecurePage.UserName);
    //  sql += " AND UserSecurity = 'MASTER'";
    //  sql += " AND UserStateCode = ''";
    //  sql += " AND UserCountyCode = ''";
    //  int testRows = db.Rows_Count_From(sql);
    //  return db.Rows_Count_From(sql) == 1;
    //}

    //#region Security for MASTER, ADMIN, PUBLIC users

    //public static bool Is_User_Security_Ok()
    //{
    //  #region Notes
    //  //Insures a valid login user
    //  //Then security check insures that each user type 
    //  //has the valid code or codes required of the user type, specifically:
    //  //A MASTER user only needs any valid StateCode of FederalCode
    //  //A State ADMIN user only needs the StateCode assigned in the Security Table
    //  //because it has authority to manage any county or local district in that State
    //  //A COUNTY user need a valid StateCode and County code 
    //  //which restricts user to that county and its local districts.
    //  //A LOCAL user needs a valid StateCode, CountyCode and LocalCode 
    //  //restricting it to only that particular local district.
    //  #endregion Notes

    //  if (SecurePage.IsSuperUser) return true;

    //  var userStateCode = db.User_StateCode();
    //  if (SecurePage.IsMasterUser)
    //  {
    //    return StateCache.IsValidStateOrFederalCode(userStateCode)
    //      || db.User_StateCode() == "LL"
    //      || db.User_StateCode() == "PP";
    //  }

    //  if (SecurePage.UserSecurityClass != db.User_Security_Default())
    //    //Security in Session from login must be same as in Security Table
    //    return false;

    //  // Admin Users
    //  if (SecurePage.IsStateAdminUser)
    //  {
    //    // State Admin
    //    return (StateCache.IsValidStateOrFederalCode(userStateCode, false))
    //      && db.Security_Str(SecurePage.UserName, "UserStateCode") == db.User_StateCode();
    //  }

    //  if (SecurePage.IsCountyAdminUser)
    //  {
    //    // County Admin
    //    return StateCache.IsValidStateOrFederalCode(userStateCode, false)
    //      && CountyCache.CountyExists(db.User_StateCode(), db.User_CountyCode())
    //      && db.Security_Str(SecurePage.UserName, "UserStateCode") == db.Domain_StateCode_This()
    //      && db.Security_Str(SecurePage.UserName, "UserCountyCode") == db.User_CountyCode();
    //  }

    //  if (SecurePage.IsLocalAdminUser)
    //  {
    //    // Local Admin
    //    return (StateCache.IsValidStateOrFederalCode(userStateCode, false))
    //      && CountyCache.CountyExists(db.User_StateCode(), db.User_CountyCode())
    //      && LocalDistricts.IsValid(db.User_StateCode(), db.User_CountyCode(), db.User_LocalCode())
    //      && db.Security_Str(SecurePage.UserName, "UserStateCode") == db.Domain_StateCode_This()
    //      && db.Security_Str(SecurePage.UserName, "UserCountyCode") == db.User_CountyCode()
    //      && db.Security_Str(SecurePage.UserName, "UserLocalCode") == db.User_LocalCode();
    //  }

    //  return false;
    //}
    //#endregion Security for MASTER, ADMIN, PUBLIC users

    //public static string User_Security_Default()
    //{
    //  switch (db.User())
    //  {
    //    case SecurePage.SecurityClass.Master:
    //      return "MASTER";
    //    case SecurePage.SecurityClass.Admin:
    //      return "ADMIN";
    //    case SecurePage.SecurityClass.County:
    //      return "COUNTY";
    //    case SecurePage.SecurityClass.Local:
    //      return "LOCAL";
    //    case SecurePage.SecurityClass.Politician:
    //      return "POLITICIAN";
    //    default:
    //      return string.Empty;
    //  }
    //}

    //#region User Security
    ////public enum Security

    //public static SecurePage.SecurityClass User()
    //{
    //  if (string.IsNullOrEmpty(SecurePage.UserSecurityClass))
    //    return SecurePage.SecurityClass.Anonymous;

    //  if (Politicians.IsValid(SecurePage.UserName))
    //    return SecurePage.SecurityClass.Politician;

    //  if (db.Is_Valid_PartiesEmails(SecurePage.UserName))
    //    return SecurePage.SecurityClass.Party;

    //  if (db.Security_Str_Optional(SecurePage.UserName, "UserSecurity").ToUpper() == "MASTER")
    //    return SecurePage.SecurityClass.Master;

    //  switch (db.Security_Str_Optional(SecurePage.UserName, "UserSecurity").ToUpper())
    //  {
    //    case "ADMIN":
    //      return SecurePage.SecurityClass.Admin;
    //    case "COUNTY":
    //      return SecurePage.SecurityClass.County;
    //    case "LOCAL":
    //      return SecurePage.SecurityClass.Local;
    //    default:
    //      return SecurePage.SecurityClass.Unknown;
    //  }
    //}

    //#endregion User Security

    //public enum SecurityClass
    //{
    //  Master,
    //  Admin,
    //  County,
    //  Local,
    //  Politician,
    //  Party,
    //  Anonymous,
    //  Unknown,
    //}

    //public static string Security_Str_Optional(string userName, string column)
    //{
    //  if (userName != string.Empty)
    //    return db.Single_Key_Str_Optional("Security", column, "UserName", userName);
    //  else
    //    return string.Empty;
    //}

    //public static bool Is_Valid_PartiesEmails(string PartyEmail)
    //{
    //  if (!string.IsNullOrEmpty(PartyEmail))
    //  {
    //    if (db.Rows("PartiesEmails", "PartyEmail", PartyEmail) == 1)
    //      return true;
    //    else
    //      return false;
    //  }
    //  else
    //    return false;
    //}

    //public static string Page_Url()
    //{
    //  return UrlManager.CurrentHostName
    //    + db.SCRIPT_NAME()
    //    + "?"
    //    + HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
    //}
    //#region SCRIPT_NAME /Default.aspx
    //public static string SCRIPT_NAME()// like: /Default.aspx (with /preceeding)"
    //{
    //  return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
    //}
    //#endregion SCRIPT_NAME /Default.aspx

    //#region /Admin/Elections.aspx Urls and Anchors
    //public static string Url_Admin_Elections(string StateCode)
    //{
    //  string url = "/Admin/Elections.aspx";
    //  if (!string.IsNullOrEmpty(StateCode))
    //    url += "&State=" + StateCode;
    //  else
    //    url += db.Url_Add_State_County_Local_Codes();
    //  return db.Fix_Url_Parms(url);
    //}
    //#endregion /Admin/Elections.aspx Urls and Anchors

    //#region /Admin/Officials.aspx URLs & Anchors
    //public static string Url_Admin_Officials()
    //{
    //  //return "/Admin/Officials.aspx";
    //  string Url = "/Admin/Officials.aspx";
    //  Url += db.Url_Add_State_County_Local_Codes();
    //  return db.Fix_Url_Parms(Url);
    //}

    //#endregion /Admin/Officials.aspx URLs & Anchors



    //#region /Intro.aspx URLs & Anchors

    //public static string Anchor_Intro(
    //  string politicianKey
    //  , string anchorText
    //  , string title
    //  , string target)
    //{
    //  string anchor = string.Empty;

    //  anchor += "<a";

    //  #region href
    //  anchor += " href=";
    //  anchor += "\"";

    //  #region Note
    //  //Anchor += db.Url_Intro(PoliticianKey);
    //  //Changed this relative url to absolute to avoid having 
    //  //duplicate page content for different domains.
    //  //i.e. Vote-USA.org/Intro.aspx?Id="ILObama"
    //  //and Vote-IL.org/Intro.aspx?Id="ILObama"
    //  #endregion Note
    //  //Anchor += db.Domain_State_Or_USA(
    //  //            db.StateCode_In_PoliticianKey(PoliticianKey));
    //  anchor += UrlManager.GetIntroPageUri(politicianKey).ToString();

    //  anchor += "\"";
    //  #endregion href

    //  #region title
    //  if (title != string.Empty)
    //  {
    //    anchor += " title=";
    //    anchor += "\"";
    //    anchor += db.Str_Remove_Single_And_Double_Quotes(title);//nicknames have quotes
    //    anchor += "\"";
    //  }
    //  #endregion title

    //  #region target
    //  //if (Target != string.Empty)
    //  //{
    //  //  Anchor += " target=";
    //  //  Anchor += "\"";
    //  //  Anchor += Target;
    //  //  Anchor += "\"";
    //  //}

    //  anchor += " target=";
    //  anchor += "\"";

    //  if (target != string.Empty)
    //    anchor += target;
    //  else
    //    anchor += "_self";

    //  anchor += "\"";
    //  #endregion target

    //  anchor += ">";

    //  anchor += anchorText;

    //  anchor += "</a>";

    //  return anchor;
    //}
    //#endregion /Intro.aspx URLs & Anchors (ok)
    //public static string Url_Admin_Offices(int officeClass)
    //{
    //  string url = Url_Admin_Offices();
    //  url += "&Class=" + officeClass;
    //  return db.Fix_Url_Parms(url);
    //}

    //#region Image constants
    ////public const int Image_Size_500_Profile = 500;
    ////public const int Image_Size_400_Profile = 400;
    //public const int Image_Size_300_Profile = 300;
    //public const int Image_Size_200_Profile = 200;
    //public const int Image_Size_100_Headshot = 100;
    //public const int Image_Size_75_Headshot = 75;
    //public const int Image_Size_50_Headshot = 50;
    //public const int Image_Size_35_Headshot = 35;
    //public const int Image_Size_25_Headshot = 25;
    //public const int Image_Size_20_Headshot = 20;
    //public const int Image_Size_15_Headshot = 15;

    //#endregion Image constants

    //public static string Politician_Key(
    //  string stateCode
    //  , string lName
    //  , string fName
    //  , string middle
    //  , string suffix
    //  )
    //{
    //  // We can safely simplify these because the later filtering will remove
    //  // all non-ascii and non-alphas.
    //  //LName = db.ToUpper1stChar(db.Str_Remove_Non_Key_Chars(LName));
    //  //FName = db.ToUpper1stChar(db.Str_Remove_Non_Key_Chars(FName));
    //  //Middle = db.ToUpper1stChar(db.Str_Remove_Non_Key_Chars(Middle));
    //  //Suffix = db.Str_Remove_Non_Key_Chars(Suffix);
    //  lName = db.ToUpper1stChar(lName);
    //  fName = db.ToUpper1stChar(fName);
    //  middle = db.ToUpper1stChar(middle);

    //  string thePoliticianKey =
    //    stateCode
    //    + lName
    //    + fName
    //    + middle
    //    + suffix;

    //  // These changes insure that the key is all ascii alpha characters.
    //  // I.e, no numerics, no accented characters and no punctuation.
    //  thePoliticianKey = thePoliticianKey.ToAscii();
    //  thePoliticianKey = // replace all non-letters with empty string
    //    (new Regex(@"\P{L}")).Replace(thePoliticianKey, match => String.Empty);

    //  return thePoliticianKey;
    //}

    //#region Insert Politician
    //public static void Politician_Insert(
    //  string PoliticianKey
    //  , string OfficeKey
    //  , string StateCode
    //  , string FName
    //  , string MName
    //  , string LName
    //  , string Suffix
    //  , string AddOn
    //  , string Nickname
    //  , string PartyKey
    //  )
    //{
    //  #region Fix Name Parts
    //  //First
    //  //FName = db.Str_Fix_Name(FName);
    //  FName = Validation.FixGivenName(FName);

    //  //Middle
    //  //if (!string.IsNullOrEmpty(MName))
    //  //  MName = db.Str_Fix_Name(MName);
    //  MName = Validation.FixGivenName(MName);

    //  //Nick name
    //  //if (!string.IsNullOrEmpty(Nickname))
    //  //  Nickname = db.Str_Fix_Name(Nickname);
    //  Nickname = Validation.FixNickname(Nickname);

    //  //Last
    //  //LName = db.Str_Fix_Name(LName);
    //  LName = Validation.FixLastName(LName);

    //  //Suffix
    //  //if (!string.IsNullOrEmpty(Suffix))
    //  //  Suffix = db.Str_Fix_Name_Suffix(Suffix);
    //  Suffix = Validation.FixNameSuffix(Suffix);
    //  #endregion Fix Name Parts

    //  string Unique_Password = db.MakeUniquePassword();

    //  string SQL = string.Empty;

    //  Politicians.Insert(
    //    politicianKey: PoliticianKey,
    //    password: Unique_Password,
    //    passwordHint: string.Empty,
    //    temporaryOfficeKey: OfficeKey,
    //    liveOfficeKey: string.Empty,
    //    liveOfficeStatus: string.Empty,
    //    liveElectionKey: string.Empty,
    //    stateCode: Politicians.GetStateCodeFromKey(PoliticianKey),
    //    firstName: FName,
    //    middleName: MName,
    //    nickname: Nickname,
    //    lastName: LName,
    //    alphaName: LName.StripAccents(),
    //    vowelStrippedName: LName.StripVowels(),
    //    suffix: Suffix,
    //    addOn: AddOn,
    //    emailVoteUSA: string.Empty,
    //    email: null,
    //    stateEmail: string.Empty,
    //    lastEmailCode: string.Empty,
    //    webAddress: null,
    //    stateWebAddress: string.Empty,
    //    phone: null,
    //    statePhone: string.Empty,
    //    gender: string.Empty,
    //    partyKey: PartyKey,
    //    address: null,
    //    cityStateZip: null,
    //    stateAddress: string.Empty,
    //    stateCityStateZip: string.Empty,
    //    campaignName: string.Empty,
    //    campaignAddress: string.Empty,
    //    campaignCityStateZip: string.Empty,
    //    campaignPhone: string.Empty,
    //    campaignEmail: string.Empty,
    //    stateData: string.Empty,
    //    //LDSStateCode: string.Empty, 
    //    //LDSTypeCode: string.Empty, 
    //    //LDSDistrictCode: string.Empty, 
    //    //LDSLegIDNum: string.Empty, 
    //    //LDSPoliticianName: string.Empty, 
    //    //LDSEmail: string.Empty, 
    //    //LDSWebAddress: string.Empty, 
    //    //LDSPhone: string.Empty, 
    //    //LDSGender: string.Empty, 
    //    //LDSPartyCode: string.Empty, 
    //    //LDSAddress: string.Empty, 
    //    //LDSCityStateZip: string.Empty, 
    //    //LDSVersion: string.Empty, 
    //    //LDSUpdateDate: VoteDb.DateTimeMin, 
    //    introLetterSent: VoteDb.DateTimeMin,
    //    //generalStatement: string.Empty, 
    //    //personal: string.Empty, 
    //    //education: string.Empty, 
    //    //profession: string.Empty, 
    //    //military: string.Empty, 
    //    //civic: string.Empty, 
    //    //political: string.Empty, 
    //    //religion: string.Empty, 
    //    //accomplishments: string.Empty, 
    //    //isHasBioData: false, 
    //    isNotRespondedEmailSent: false,
    //    dataLastUpdated: VoteDb.DateTimeMin,
    //    dataUpdatedCount: 0,
    //    datePictureUploaded: VoteDb.DateTimeMin,
    //    //isLDSIncumbent: false, 
    //    answers: 0, dateOfBirth:
    //    VoteDb.DateTimeMin,
    //    facebookWebAddress: string.Empty,
    //    wikipediaWebAddress: string.Empty,
    //    youTubeWebAddress: string.Empty,
    //    flickrWebAddress: string.Empty,
    //    twitterWebAddress: string.Empty,
    //    RSSFeedWebAddress: string.Empty,
    //    vimeoWebAddress: string.Empty,
    //    googlePlusWebAddress: string.Empty,
    //    linkedInWebAddress: string.Empty,
    //    pinterestWebAddress: string.Empty,
    //    bloggerWebAddress: string.Empty,
    //    webstagramWebAddress: string.Empty,
    //    ballotPediaWebAddress: string.Empty,
    //    youTubeDescription: string.Empty,
    //    youTubeRunningTime: default(TimeSpan),
    //    youTubeDate: VoteDb.DateTimeMin,
    //    youTubeAutoDisable: string.Empty,
    //    youTubeVideoVerified: false);

    //  DB.VoteLog.LogPoliticianAdds.Insert(
    //    DateTime.Now,
    //    SecurePage.UserSecurityClass,
    //    SecurePage.UserName,
    //    PoliticianKey,
    //    Unique_Password,
    //    OfficeKey,
    //    StateCode,
    //    string.Empty,
    //    string.Empty,
    //    string.Empty,
    //    FName,
    //    MName,
    //    LName,
    //    Suffix,
    //    Nickname,
    //    string.Empty,
    //    string.Empty,
    //    string.Empty,
    //    string.Empty,
    //    string.Empty,
    //    string.Empty);
    //}
    //#endregion Insert Politician

    //public static string ToUpper1stChar(string name)
    //{
    //  if (name.Trim() == string.Empty) return string.Empty;
    //  string newCleanName = name.ToLower()
    //    .Trim(); //all lower chars
    //  char firstChar = newCleanName[0]; //1st char upper
    //  firstChar = Char.ToUpper(firstChar);
    //  int len = name.Length;
    //  string remainingChars = newCleanName.Substring(1, len - 1);
    //  remainingChars = remainingChars.ToLower();
    //  return Char.ToString(firstChar) + remainingChars;
    //}

    //#region /Admin/Offices.aspx URLs
    //public static string Url_Admin_Offices()
    //{
    //  return "/Admin/Offices.aspx";
    //  //Url += db.Url_Add_State_County_Local_Codes();
    //  //return db.Fix_Url_Parms(Url);

    //  //return "/Admin/Offices.aspx";
    //}
    //#endregion /Admin/Offices.aspx URLs

    //#region /Admin/Politicians.aspx URLs
    //public static string Url_Admin_Politicians()
    //{
    //  return "/Admin/Politicians.aspx";
    //  //Url += db.Url_Add_State_County_Local_Codes();
    //  //return db.Fix_Url_Parms(Url);
    //}

    //#endregion /Admin/Politicians.aspx URLs

    //public static bool Is_Electoral_County(string officeKey)
    //{
    //  return db.Is_Electoral_County(Offices.GetOfficeClass(officeKey));
    //}

    //public static bool Is_Electoral_Local(string officeKey)
    //{
    //  return db.Is_Electoral_Local(Offices.GetOfficeClass(officeKey));
    //}

    //public static string LegislativeDistricts()
    //{
    //  string legislativeDistrictsAddress;

    //  if (!StateCache.IsValidStateCode(State_Code()))
    //    legislativeDistrictsAddress = "for any address in the United States";
    //  else
    //  {
    //    legislativeDistrictsAddress = "<span class=\"districtsHead\">for address in:</span>"
    //      + LegislativeDistrictsWithBRs();
    //  }
    //  return legislativeDistrictsAddress;
    //}

    //public static string Url_Vote_XX_org_Default()
    //{
    //  return Url_Vote_XX_org_Page("Default.aspx");
    //}

    //public static string Url_Admin_Default_Login()
    //{
    //  return Url_Admin_Default(
    //    State_Code()
    //    , User_CountyCode()
    //    , User_LocalCode()
    //    );
    //}

    //public static string Url_Politician_IssueQuestions(
    //  string issueKey
    //  , string PoliticianKey
    //  )
    //{
    //  string Url;
    //  //if (IssueKey != db.Issues_List(IssueKey))
    //  if (!Is_IssuesList(issueKey))
    //    Url = Url_Politician_IssueQuestions()
    //      + "&Issue=" + issueKey;
    //  else
    //    Url = Url_Politician_IssueQuestions()
    //      + "&Issue=" + "ALLPersonal";
    //  //+ "&Issue=" + "ALLBio";
    //  if (!string.IsNullOrEmpty(PoliticianKey))
    //    Url += "&Id=" + PoliticianKey;
    //  else
    //    Url += Politician_Id_Add_To_QueryString_Master_User();

    //  return Fix_Url_Parms(Url);
    //}

    //public static string Url_Image_Politician_Or_NoPhoto(string politicianKey,
    //  int imageWidth)
    //{
    //  // This handles the NoPhoto case without having to hit the db for every img href
    //  // on the page
    //  //if (db.Is_Valid_PoliticiansImages(PoliticianKey))
    //  //  return db.Url_Image_Politician(PoliticianKey, Width_Image);
    //  //else
    //  //  return db.Url_Image_NoPhoto(Width_Image);
    //  return Url_Image_Politician(politicianKey, imageWidth, imageWidth);
    //}

    //public static bool Is_Include_First_Footer_This()
    //{
    //  return DomainDesigns_Bool_This("IsIncludedFirstFooterAllPages");
    //}

    //public static bool Is_Include_Second_Footer_This()
    //{
    //  return DomainDesigns_Bool_This("IsIncludedSecondFooterAllPages");
    //}

    //public static bool Is_Include_Email_Us_This()
    //{
    //  return DomainDesigns_Bool_This("IsIncludedEmailUsAllPages");
    //}

    //public static bool Is_Include_Powered_By_This()
    //{
    //  return DomainDesigns_Bool_This("IsIncludedPoweredByAllPages");
    //}

    //public static bool Is_Include_Donate_This()
    //{
    //  return DomainDesigns_Bool_This("IsIncludedDonateAllPages");
    //}

    //#region PartiesEmails Table

    //public static string PartiesEmails_Str(
    //  string Party_Email
    //  , string Column
    //  )
    //{
    //  if (Party_Email != string.Empty)
    //    return Single_Key_Str_Optional(
    //      "PartiesEmails"
    //      , Column
    //      , "PartyEmail"
    //      , Party_Email
    //      );
    //  return string.Empty;
    //}

    //#endregion PartiesEmails Table

    //#region ElectionKey

    //public static string ElectionKey_New_Format(string ElectionKeyOld)
    //{
    //  //Converts old ElectionKey format to new ElectionKey format
    //  if (string.IsNullOrEmpty(ElectionKeyOld))
    //  {
    //    return string.Empty;
    //  }

    //  #region Note

    //  //If first two chars are a State or Federal Code it is in the new ElectionKey format
    //  //Old ElectionKey format looks like: 20081104GSC000000ALL

    //  #endregion Note

    //  string ElectionKeyNew;
    //  if (!StateCache.IsValidStateOrFederalCode(ElectionKeyOld.Substring(0, 2).ToUpper(), false))
    //  {
    //    if (ElectionKeyOld.Length >= Elections.ElectionKeyLengthStateOrFederal)
    //    {
    //      ElectionKeyNew = ElectionKeyOld.Substring(9, 2).ToUpper(); //StateCode
    //      ElectionKeyNew += ElectionKeyOld.Substring(0, 8); //YYYYMMDD
    //      ElectionKeyNew += ElectionKeyOld.Substring(8, 1).ToUpper(); //Type

    //      if (ElectionKeyOld.Length == 18)
    //        ElectionKeyNew += ElectionKeyOld.Substring(17, 1).ToUpper(); //Nationa Party Code
    //      else if (ElectionKeyOld.Length >= 21)
    //      {
    //        if (ElectionKeyOld.Substring(17, 3).ToUpper() == "ALL")
    //          ElectionKeyNew += "A";
    //        else
    //          ElectionKeyNew += ElectionKeyOld.Substring(19, 1).ToUpper();
    //      }
    //      else
    //        ElectionKeyNew += "A";

    //      if (ElectionKeyOld.Length >= 17)
    //      {
    //        #region CountyCode

    //        if (ElectionKeyOld.Substring(11, 3) != "000")
    //        {
    //          ElectionKeyNew += ElectionKeyOld.Substring(11, 3);
    //          //LocalCode
    //          if (ElectionKeyOld.Substring(14, 2) != "00")
    //            ElectionKeyNew += ElectionKeyOld.Substring(14, 2);
    //        }

    //        #endregion CountyCode
    //      }

    //      //if (db.Is_Valid_Election(ElectionKeyNew))
    //      return ElectionKeyNew.ToUpper();
    //      //else
    //      //  return string.Empty;
    //    }
    //    return string.Empty;
    //  }
    //  return ElectionKeyOld; //Valid ElectionKey - Old ElectionKey is the New ElectionKey
    //}

    //#endregion ElectionKey Build, convert from old format

    //public static void Add_Td_To_Tr(
    //  HtmlTableRow htmlTr
    //  , string text
    //  , string tdClass
    //  , string align
    //  , string width
    //  , int colspan
    //  )
    //{
    //  //<td Class="TdClass">
    //  var htmlTableCell = new HtmlTableCell { InnerHtml = text };
    //  if (tdClass != string.Empty)
    //    htmlTableCell.Attributes["class"] = tdClass;
    //  if (align != string.Empty)
    //    htmlTableCell.Attributes["align"] = align;
    //  if (width != string.Empty)
    //    htmlTableCell.Attributes["width"] = width;
    //  if (colspan != 0)
    //    htmlTableCell.Attributes["colspan"] = colspan.ToString(CultureInfo.InvariantCulture);
    //  //</td>
    //  htmlTr.Cells.Add(htmlTableCell);
    //}

    //#region Edits

    //public static string LegislativeDistrictsWithBRs()
    //{
    //  return "<br />" + VotePage.FormatLegislativeDistrictsFromQueryString();
    //  //string theLegislativeDistricts = string.Empty;
    //  ////if (db.QueryString("State")//db.State_Code() 
    //  //if (db.State_Code()
    //  //  != "DC")
    //  //{
    //  //  if (db.CongressDistrict_Code_QueryString() != "000")
    //  //    theLegislativeDistricts += "<br>US House District "
    //  //      + db.Str_Remove_Leading_0s(db.CongressDistrict_Code_QueryString())
    //  //      //+ " " + db.Name_State(db.QueryString("State"));//db.State_Code());
    //  //      + " " + StateCache.GetStateName(db.State_Code());
    //  //  if (db.StateSenate_Code_QueryString() != "000")
    //  //    theLegislativeDistricts += "<br>"
    //  //      //+ db.Name_State(db.QueryString("State"))//db.State_Code())
    //  //      + StateCache.GetStateName(db.State_Code())
    //  //      + " Senate District "
    //  //      + db.Str_Remove_Leading_0s(db.StateSenate_Code_QueryString());
    //  //  if (db.StateHouse_Code_QueryString() != "000")
    //  //    theLegislativeDistricts += "<br>"
    //  //      //+ db.Name_State(db.QueryString("State"))//db.State_Code())
    //  //      + db.State_Code()
    //  //      + " House District "
    //  //      + db.Str_Remove_Leading_0s(db.StateHouse_Code_QueryString());
    //  //  if (db.QueryString("County") != string.Empty)
    //  //    theLegislativeDistricts += "<br>" + CountyCache.GetCountyName(
    //  //      db.State_Code(), db.QueryString("County"));
    //  //}
    //  //else
    //  //{
    //  //  if (db.StateSenate_Code_QueryString() != "000")
    //  //    theLegislativeDistricts += "<br>"
    //  //      + "Ward "
    //  //      + db.Str_Remove_Leading_0s(db.StateSenate_Code_QueryString());
    //  //}
    //  //return theLegislativeDistricts;
    //}

    //#endregion

    //#region Vote-XX.org Urls and Anchors

    //public static string Url_Vote_XX_org_Page(string page)
    //{
    //  return UrlManager.GetStateUri(State_Code(),
    //    page, "State=" + State_Code()).ToString();
    //}

    //#endregion Vote-XX.org Urls and Anchors

    //#region /Admin/Default.aspx URLs & Anchors

    ////Urls
    //public static string Url_Admin_Default(string stateCode, string countyCode,
    //  string localCode)
    //{
    //  string url = "/Admin/Default.aspx";
    //  if (stateCode != string.Empty)
    //    url += "&State=" + stateCode;
    //  if (countyCode != string.Empty)
    //    url += "&County=" + countyCode;
    //  if (localCode != string.Empty)
    //    url += "&Local=" + localCode;
    //  return Fix_Url_Parms(url);
    //}

    //#endregion /Admin/Admin.aspx URLs & Anchors

    //public static string Politician_Id_Add_To_QueryString_Master_User()
    //{
    //  #region Note

    //  //A Master User passes the same PoliticianKey in a Id QueryString from page to page
    //  //A Politician User never passes the Id in a QueryString

    //  #endregion Note

    //  if (
    //    (
    //      (SecurePage.IsMasterUser)
    //        || (SecurePage.IsPartyUser)
    //      )
    //      && (!string.IsNullOrEmpty(VotePage.QueryId))
    //    )
    //    return "&Id=" + VotePage.QueryId;
    //  return string.Empty;
    //}

    //#region /Politician/IssueQuestions.aspx URLs & Anchors

    //public static string Url_Politician_IssueQuestions()
    //{
    //  return @"/Politician/IssueQuestions.aspx";
    //}

    //#endregion

    //public static bool DomainDesigns_Bool_This(string Column)
    //{
    //  return DomainDesigns_Bool(Domain_DesignCode_This(), Column);
    //}

    //public static bool Is_IssuesList(string IssueKey)
    //{
    //  if (IssueKey == Issues_List(IssueKey))
    //    return true;
    //  return false;
    //}

    //public static string Domain_DesignCode_This()
    //{
    //  //string test = db.SERVER_NAME();
    //  if (!string.IsNullOrEmpty(VotePage.QueryDesign))
    //    return VotePage.QueryDesign;
    //  return UrlManager.CurrentDomainDesignCode;
    //}

    //#region DomainDesigns Table

    ////Strings

    ////bool
    //public static bool DomainDesigns_Bool(string DomainDesignCode, string Column)
    //{
    //  return Single_Key_Bool("DomainDesigns", Column, "DomainDesignCode", DomainDesignCode);
    //}

    //#endregion

    //public static void Politician_Delete(string politicianKey)
    //{
    //  #region Politicians Table

    //  string sqlDelete = "DELETE FROM Politicians WHERE PoliticianKey ="
    //    + SQLLit(politicianKey);
    //  ExecuteSql(sqlDelete);

    //  #endregion Politicians Table

    //  #region PoliticiansImages Table

    //  //SQLDELETE = "DELETE FROM PoliticiansImages WHERE PoliticianKey ="
    //  //  + db.SQLLit(PoliticianKey);
    //  //db.ExecuteSQL(SQLDELETE);
    //  PoliticiansImagesData.DeleteByPoliticianKey(politicianKey);
    //  PoliticiansImagesBlobs.DeleteByPoliticianKey(politicianKey);
    //  CommonCacheInvalidation.ScheduleInvalidation("politicianimage", politicianKey);

    //  #endregion PoliticiansImages Table

    //  #region Answers Table

    //  sqlDelete = "DELETE FROM Answers WHERE PoliticianKey ="
    //    + SQLLit(politicianKey);
    //  ExecuteSql(sqlDelete);

    //  #endregion Answers Table

    //  sqlDelete = "DELETE FROM ElectionsPoliticians WHERE PoliticianKey ="
    //    + SQLLit(politicianKey);
    //  ExecuteSql(sqlDelete);

    //  sqlDelete = "DELETE FROM OfficesOfficials WHERE PoliticianKey ="
    //    + SQLLit(politicianKey);
    //  ExecuteSql(sqlDelete);

    //  LogElectionPoliticianAddsDeletes.DeleteByPoliticianKey(politicianKey);

    //  LogPoliticianAdds.DeleteByPoliticianKey(politicianKey);

    //  LogPoliticianChanges.DeleteByPoliticianKey(politicianKey);

    //  LogPoliticianAnswers.DeleteByPoliticianKey(politicianKey);
    //}

    //#region OfficesOfficials LOG

    //public static void Log_OfficesOfficials_Add_Or_Delete(string addOrDelete,
    //  string officeKey, string politicianKey)
    //{
    //  //string SQL = "INSERT INTO LogOfficeOfficialAddsDeletes"
    //  //  + "("
    //  //  + "DateStamp"
    //  //  + ",AddOrDelete"
    //  //  + ",UserSecurity"
    //  //  + ",UserName"
    //  //  + ",PoliticianKey"
    //  //  + ",StateCode"
    //  //  + ",OfficeKey"
    //  //  + ")"
    //  //  + " VALUES("
    //  //  + db.SQLLit(Db.DbNow)
    //  //  + "," + db.SQLLit(addOrDelete)
    //  //  + "," + db.SQLLit(db.User_Security())
    //  //  + "," + db.SQLLit(db.User_Name())
    //  //  + "," + db.SQLLit(politicianKey)
    //  //  //+ "," + db.SQLLit(db.StateCode4OfficeKey(OfficeKey))
    //  //  + "," + db.SQLLit(db.StateCode_In_OfficeKey(officeKey))
    //  //  + "," + db.SQLLit(officeKey)
    //  //  + ")";
    //  //db.ExecuteSQL(SQL);
    //  LogOfficeOfficialAddsDeletes.Insert(
    //    DateTime.Now,
    //    addOrDelete,
    //    SecurePage.UserSecurityClass,
    //    VotePage.UserName,
    //    politicianKey,
    //    Offices.GetStateCodeFromKey(officeKey),
    //    string.Empty,
    //    string.Empty,
    //    officeKey);
    //}

    //#endregion

    //#region Offices LOG

    //public static void LogOfficeChange(string officeKey, string dataItem,
    //  string dataFrom, string dataTo)
    //{
    //  LogOfficeChanges.Insert(
    //    DateTime.Now,
    //    SecurePage.UserSecurityClass,
    //    VotePage.UserName,
    //    officeKey,
    //    dataItem,
    //    dataFrom.Trim().ReplaceBreakTagsWithNewLines(),
    //    dataTo.Trim());
    //}

    //public static void LogOfficeChange(string officeKey, string dataItem,
    //  int dataFrom, int dataTo)
    //{
    //  LogOfficeChange(officeKey, dataItem, dataFrom.ToString(), dataTo.ToString());
    //}

    //#endregion

    //---------------------------------------
    //public enum PoliticianStatuses
    //{
    //  InElectionUpcomingViewable,
    //  InElectionUpcomingNotViewable,
    //  InElectionUpcomingViewableRunningMate,
    //  InElectionUpcomingNotViewableRunningMate,
    //  InElectionUpcomingNotCreated,
    //  Incumbent,
    //  IncumbentRunningMate,
    //  InElectionPrevious,
    //  InElectionPreviousRunningMate,
    //  Unknown
    //}

    //public static DataRow Politician_Incumbent_RunningMate_Row(string politicianKey)
    //{
    //  var sql = string.Empty;
    //  sql += " SELECT ";
    //  sql += " Offices.OfficeKey";
    //  sql += " FROM OfficesOfficials,Offices ";
    //  sql += " WHERE OfficesOfficials.RunningMateKey = " + SQLLit(politicianKey);
    //  sql += " AND OfficesOfficials.OfficeKey = Offices.OfficeKey";
    //  var rowPolitician = Row_First_Optional(sql);
    //  return rowPolitician;
    //}

    //public static DataRow Politician_Election_Previous_Row(string politicianKey)
    //{
    //  var sql = string.Empty; //xx
    //  sql += " SELECT";
    //  sql += " ElectionsPoliticians.ElectionKey";
    //  sql += " FROM ElectionsPoliticians,Elections";
    //  sql += " WHERE ElectionsPoliticians.PoliticianKey = " + SQLLit(politicianKey);
    //  sql += " AND ElectionsPoliticians.ElectionKey = Elections.ElectionKey";
    //  sql += " AND Elections.ElectionDate < " + SQLLit(Db.DbToday);
    //  sql += " ORDER BY Elections.ElectionDate DESC";
    //  var rowPolitician = Row_First_Optional(sql);
    //  return rowPolitician;
    //}

    //public static DataRow Politician_Election_Previous_RunningMate_Row(string politicianKey)
    //{
    //  var sql = string.Empty; //xx
    //  sql += " SELECT";
    //  sql += " ElectionsPoliticians.ElectionKey";
    //  sql += " FROM ElectionsPoliticians,Elections";
    //  sql += " WHERE ElectionsPoliticians.RunningMateKey = " + SQLLit(politicianKey);
    //  sql += " AND ElectionsPoliticians.ElectionKey = Elections.ElectionKey";
    //  sql += " AND Elections.ElectionDate < " + SQLLit(Db.DbToday);
    //  sql += " ORDER BY Elections.ElectionDate DESC";
    //  var rowPolitician = Row_First_Optional(sql);
    //  return rowPolitician;
    //}

    //public static DataRow Politician_Election_Upcoming_Not_Created_Row(string politicianKey)
    //{
    //  var sql = string.Empty;
    //  sql += " SELECT ";
    //  sql += " TemporaryOfficeKey";
    //  sql += " FROM Politicians";
    //  sql += " WHERE PoliticianKey = " + SQLLit(politicianKey);
    //  var rowPolitician = Row_First_Optional(sql);
    //  return rowPolitician;
    //}

    //private static PoliticianStatuses Politician_Current_Status_Type(
    //  PageCache cache, string politicianKey)
    //{
    //  if (
    //    !string.IsNullOrEmpty(
    //      cache.ElectionsPoliticians.GetFutureElectionKeyByPoliticianKey(politicianKey, true)))
    //    return PoliticianStatuses.InElectionUpcomingViewable;
    //  if (
    //    !string.IsNullOrEmpty(
    //      cache.ElectionsPoliticians.GetFutureElectionKeyByPoliticianKey(politicianKey, false)))
    //    return PoliticianStatuses.InElectionUpcomingNotViewable;
    //  if (
    //    !string.IsNullOrEmpty(
    //      cache.ElectionsPoliticians.GetFutureElectionKeyByRunningMateKey(politicianKey, true)))
    //    return PoliticianStatuses.InElectionUpcomingViewableRunningMate;
    //  if (
    //    !string.IsNullOrEmpty(
    //      cache.ElectionsPoliticians.GetFutureElectionKeyByRunningMateKey(politicianKey, false)))
    //    return PoliticianStatuses.InElectionUpcomingNotViewableRunningMate;
    //  if (
    //    !string.IsNullOrEmpty(
    //      cache.OfficesOfficials.GetIncumbentOfficeKeyByPoliticianKey(politicianKey)))
    //    return PoliticianStatuses.Incumbent;

    //  if (Politician_Incumbent_RunningMate_Row(politicianKey) != null)
    //    return PoliticianStatuses.IncumbentRunningMate;

    //  if (Politician_Election_Previous_Row(politicianKey) != null)
    //    return PoliticianStatuses.InElectionPrevious;

    //  if (Politician_Election_Previous_RunningMate_Row(politicianKey) != null)
    //    return PoliticianStatuses.InElectionPreviousRunningMate;

    //  if (Politician_Election_Upcoming_Not_Created_Row(politicianKey) != null)
    //    return PoliticianStatuses.InElectionUpcomingNotCreated;

    //  return PoliticianStatuses.Unknown;
    //}

    //public static string Politician_Current_Office_And_Status(PageCache cache,
    //  string politicianKey)
    //{
    //  var officeStatus = cache.Politicians.GetOfficeStatus(politicianKey);
    //  if (!Offices.IsValid(officeStatus.OfficeKey)) return string.Empty;
    //  var officeName = Offices.GetLocalizedOfficeNameWithElectoralClass(cache,
    //    officeStatus.OfficeKey);
    //  return officeStatus.PoliticianStatus.GetOfficeStatusDescription(officeName);
    //}

    //public static bool Is_Politician_In_Election_Upcoming_Viewable(PageCache cache,
    //  string politicianKey)
    //{
    //  switch (Politician_Current_Status_Type(cache, politicianKey))
    //  {
    //    case PoliticianStatuses.InElectionUpcomingViewable:
    //      return true;
    //    case PoliticianStatuses.InElectionUpcomingNotViewable:
    //      return false;
    //    case PoliticianStatuses.InElectionUpcomingViewableRunningMate:
    //      return true;
    //    case PoliticianStatuses.InElectionUpcomingNotViewableRunningMate:
    //      return false;
    //    case PoliticianStatuses.InElectionUpcomingNotCreated:
    //      return false;
    //    case PoliticianStatuses.Incumbent:
    //      return false;
    //    case PoliticianStatuses.IncumbentRunningMate:
    //      return false;
    //    case PoliticianStatuses.InElectionPrevious:
    //      return false;
    //    case PoliticianStatuses.InElectionPreviousRunningMate:
    //      return false;
    //    default:
    //      return false;
    //  }
    //}

  }
}