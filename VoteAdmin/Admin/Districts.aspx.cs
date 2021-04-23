using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Web.UI.HtmlControls;
using DB.Vote;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class DistrictsPage : SecureAdminPage
  {
    #region from db

    public static int Rows(string table, string keyName1, string keyValue1, string keyName2,
        string keyValue2, string keyName3, string keyValue3)
      => Db.Rows(table, keyName1, keyValue1, keyName2, keyValue2, keyName3, keyValue3);

    public static bool Is_Valid_Election(string electionKey)
    {
      if (!string.IsNullOrEmpty(electionKey))
        return G.Rows("Elections", "ElectionKey", electionKey) == 1;
      return false;
    }

    public static string Ok(string msg) => $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    public static string Fail(string msg) => $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    public static string Message(string msg) => $"<span class=\"Msg\">{msg}</span>";

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    public static string Str_Remove_Single_And_Double_Quotes(string str2Modify)
    {
      var str = str2Modify;
      str = str.Replace("\'", string.Empty);
      str = str.Replace("\"", string.Empty);

      return str;
    }

    public static DataRow Row_Last_Optional(string sql)
    {
      var table = G.Table(sql);
      return table.Rows.Count >= 1 ? table.Rows[table.Rows.Count - 1] : null;
    }

    public static void Triple_Key_Update_Str(string table, string column, string columnValue,
      string keyName1, string keyValue1, string keyName2, string keyValue2, string keyName3,
      string keyValue3)
    {
      var updateSql = "UPDATE " + table + " SET " + column + " = " + SqlLit(columnValue) + " WHERE " +
        keyName1 + " = " + SqlLit(keyValue1) + " AND " + keyName2 + " = " + SqlLit(keyValue2) +
        " AND " + keyName3 + " = " + SqlLit(keyValue3);
      G.ExecuteSql(updateSql);
    }

    public static void Office_Delete_All_Tables_All_Rows(string officeKey)
    {
      var sql = " DELETE FROM ElectionsOffices";
      sql += " WHERE OfficeKey = " + SqlLit(officeKey);
      G.ExecuteSql(sql);

      sql = " DELETE FROM ElectionsPoliticians";
      sql += " WHERE OfficeKey = " + SqlLit(officeKey);
      G.ExecuteSql(sql);

      sql = " DELETE FROM OfficesOfficials";
      sql += " WHERE OfficeKey = " + SqlLit(officeKey);
      G.ExecuteSql(sql);

      sql = " DELETE FROM Offices";
      sql += " WHERE OfficeKey = " + SqlLit(officeKey);
      G.ExecuteSql(sql);
    }

    public static string Elections_Str(string electionKey, string columnName)
      => Elections_Str(electionKey, Elections.GetColumn(columnName));

    public static string Elections_Str(string electionKey, Elections.Column column)
    {
      var value = Elections.GetColumn(column, electionKey);
      if (value == null) return string.Empty;
      return value as string;
    }

    public static string Name_Election(string electionKey)
      =>
      (electionKey != string.Empty) && Is_Valid_Election(electionKey)
        ? Elections_Str(electionKey, "ElectionDesc")
        : string.Empty;

    public static string Name_Office_Contest_And_Electoral(string officeKey)
    {
      if (Offices.IsValid(officeKey))
      {
        var nameOfficeContest =
          $"{Offices.FormatOfficeName(officeKey)}, {Offices.GetElectoralClassDescriptionFromOfficeKey(officeKey)}";

        return nameOfficeContest;
      }
      return string.Empty;
    }

    public static string GetOfficeData(string officeKey)
    {
      var officeData = string.Empty;

      if (Offices.IsValid(officeKey))
      {
        officeData += "<br>StateCode:" + Offices.GetStateCodeFromKey(officeKey);
        officeData += ", CountyCode:" + Offices.GetCountyCodeFromKey(officeKey);
        officeData += ", LocalCode:" + Offices.GetLocalCodeFromKey(officeKey);
        officeData += "<br>OfficeClass:" + " (" + Offices.GetOfficeClass(officeKey).ToInt() + ") " +
          Offices.GetOfficeClassDescription(officeKey);

        officeData += "<br>";
        officeData += Offices.GetOfficeClassDescription(Offices.GetOfficeClass(officeKey),
          Offices.GetStateCodeFromKey(officeKey));
        officeData += "<br>";
        officeData += "<strong>" + Name_Office_Contest_And_Electoral(officeKey) + "</strong>";

        officeData += "<br>";
        officeData += "In the following ELECTIONS:";
        var sql = string.Empty;
        sql += " ElectionsOffices";
        sql += " WHERE OfficeKey = " + SqlLit(officeKey);
        if (G.Rows_Count_From(sql) > 0)
        {
          sql = string.Empty;
          sql += " SELECT";
          sql += " ElectionKey";
          sql += " FROM ElectionsOffices";
          sql += " ElectionsOffices";
          sql += " WHERE OfficeKey = " + SqlLit(officeKey);
          var electionsTable = G.Table(sql);
          if (electionsTable != null)
          {
            foreach (DataRow electionsRow in electionsTable.Rows)
              officeData += "<br><strong>" + Name_Election(electionsRow["ElectionKey"].ToString()) +
                "</strong>";
          }
        }
        else
        {
          officeData += "<br><strong>NONE</strong>";
        }

        officeData += "<br>";
        officeData += "POLITICIAN(S) as Incumbnet to this office:";
        sql = string.Empty;
        sql += " OfficesOfficials";
        sql += " WHERE OfficeKey = " + SqlLit(officeKey);
        if (G.Rows_Count_From(sql) > 0)
        {
          sql = string.Empty;
          sql += " SELECT";
          sql += " PoliticianKey";
          sql += " FROM OfficesOfficials";
          sql += " WHERE OfficeKey = " + SqlLit(officeKey);
          var politiciansTable = G.Table(sql);
          if (politiciansTable != null)
          {
            foreach (DataRow politicianRow in politiciansTable.Rows)
              officeData += "<br><strong>" +
                Politicians.GetFormattedName(politicianRow["PoliticianKey"].ToString()) +
                "</strong>";
          }
        }
        else
        {
          officeData += "<br><strong>NONE</strong>";
        }
      }
      else
        officeData += "<br><strong>Office does not exist</strong>";

      return officeData;
    }

    private static string FormatName(string name)
      =>
      (name.Trim() != string.Empty) && !Is_Request_Erase(name.Trim())
        ? name.Trim().ToTitleCase()
        : name;

    private static bool Is_Request_Erase(string dataTo)
      => dataTo.ToUpper().Trim().Replace(" ", string.Empty) == "N/A";

    private static string Str_Remove_SpecialChars_All_Except_Spaces(string str2Modify)
    {
      var str = str2Modify;
      str = str.Trim();
      str = Str_Remove_SpecialChars_Except_Single_And_Double_Quotes_Dash_And_Period(str);
      str = Str_Remove_Single_And_Double_Quotes(str);

      return str;
    }

    private static string Str_Remove_SpecialChars_Except_Single_And_Double_Quotes_Dash_And_Period(
      string str2Modify)
    {
      var str = str2Modify;
      //characters to strip off
      //single ' keep for names like O'Donnell
      //. also kept for Middle Name abreviations 
      //- kept for Hypenated last names
      str = str.Trim();
      str = str.Replace("+", string.Empty);
      str = str.Replace("=", string.Empty);
      str = str.Replace(",", string.Empty);
      str = str.Replace("(", string.Empty);
      str = str.Replace(")", string.Empty);
      str = str.Replace("!", string.Empty);
      str = str.Replace("@", string.Empty);
      str = str.Replace("#", string.Empty);
      str = str.Replace("%", string.Empty);
      str = str.Replace("&", string.Empty);
      str = str.Replace("*", string.Empty);
      str = str.Replace(":", string.Empty);
      str = str.Replace(";", string.Empty);
      str = str.Replace("$", string.Empty);
      str = str.Replace("^", string.Empty);
      str = str.Replace("?", string.Empty);
      str = str.Replace("<", string.Empty);
      str = str.Replace(">", string.Empty);
      str = str.Replace("[", string.Empty);
      str = str.Replace("]", string.Empty);
      str = str.Replace("{", string.Empty);
      str = str.Replace("}", string.Empty);
      str = str.Replace("|", string.Empty);
      str = str.Replace("~", string.Empty);
      str = str.Replace("`", string.Empty);
      str = str.Replace("_", string.Empty);
      str = str.Replace("/", string.Empty);
      return str;
    }

    private static void LocalDistrictsUpdate(string stateCode, string countyCode, string localCode,
      string column, string columnValue)
    {
      LocalDistricts_Row_Insert_Empty(stateCode, countyCode, localCode);
      Triple_Key_Update_Str("LocalDistricts", column, columnValue, "StateCode", stateCode,
        "CountyCode", countyCode, "LocalCode", localCode);
    }

    private static void LocalDistricts_Row_Insert_Empty(string stateCode, string countyCode,
      string localCode)
    {
      if (
        Rows("LocalDistricts", "StateCode", stateCode, "CountyCode", countyCode, "LocalCode",
          localCode) == 0)
      {
        var sql = string.Empty;
        sql += "INSERT INTO LocalDistricts";
        sql += "(";
        sql += "StateCode";
        sql += ",CountyCode";
        sql += ",LocalCode";
        sql += ",LocalDistrict";
        sql += ",StateLocalDistrictCode";
        sql += ",Contact";
        sql += ",ContactTitle";
        sql += ",ContactEmail";
        sql += ",Phone";
        sql += ",AltContact";
        sql += ",AltContactTitle";
        sql += ",AltEMail";
        sql += ",AltPhone";
        sql += ",EMail";
        sql += ",URL";
        sql += ",BallotName";
        sql += ",ElectionsAuthority";
        sql += ",AddressLine1";
        sql += ",AddressLine2";
        sql += ",CityStateZip";
        sql += ",Notes";
        sql += ",EmailPage";
        sql += ",URLDataPage";
        sql += ",IsLocalDistrictTagForDeletion";
        sql += ",ElectionKeyOfficialsReportStatus";
        sql += ")";
        sql += "VALUES";
        sql += "(";
        sql += SqlLit(stateCode);
        sql += "," + SqlLit(countyCode);
        sql += "," + SqlLit(localCode);
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",0";
        sql += ",''";
        sql += ")";
        G.ExecuteSql(sql);
      }
    }

    #endregion from db

    private void Local_Links()
    {
      LabelLocalDistricts.Text =
        LocalLinks.GetDistrictsLocalLinks(StateCode,
            CountyCode, RadioButtonListSort.SelectedValue != "Name")
          .RenderToString();
    }

    private DataTable DataTable_Offices()
    {
      var sql = string.Empty;
      sql += " SELECT OfficeKey";
      sql += " FROM Offices";
      sql += " WHERE StateCode = " + SqlLit(StateCode);
      sql += " AND CountyCode = " + SqlLit(CountyCode);
      sql += " AND LocalCode = " + SqlLit(LocalCode);
      return G.Table(sql);
    }

    protected void ButtonLocalDistrictUpdate_Click(object sender, EventArgs e)
    {
      try
      {
        LocalDistrictsUpdate(StateCode, CountyCode,
          LocalCode, "LocalDistrict", TextBoxLocalDistrictUpdate.Text.Trim());

        Local_Links();

        TextBoxLocalDistrictUpdate.Text = string.Empty;
        TableLocalDistrictUpdate.Visible = false;

        Msg.Text =
          Ok("The local district or town name has been changed and should appear in the list below.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonLocalDistrictAdd_Click(object sender, EventArgs e)
    {
      try
      {
        var codeNext = string.Empty;
        if (TextBoxCode.Text.Trim() != string.Empty)
        {
          if (TextBoxCode.Text.Trim().Length != 2)
            throw new ApplicationException("The code is not 2 digits.");
          if (!TextBoxCode.Text.Trim().IsDigits())
            throw new ApplicationException("The code is not  digits.");
          if (LocalDistricts.IsValid(StateCode,
            CountyCode, TextBoxCode.Text.Trim()))
            throw new ApplicationException("This code already exists.");
        }
        else
        {
          #region Next Sequential Code

          var sql = string.Empty;
          sql += " SELECT ";
          sql += " LocalCode";
          sql += " FROM LocalDistricts ";
          sql += " WHERE StateCode = " + SqlLit(StateCode);
          sql += " AND CountyCode = " + SqlLit(CountyCode);
          sql += " AND LocalCode != '00'";
          sql += " ORDER BY LocalCode ASC";
          var localDistrictRow = Row_Last_Optional(sql);
          if (localDistrictRow != null)
          {
            int codeInt = Convert.ToInt16(localDistrictRow["LocalCode"]);
            if (codeInt < 10)
              codeInt = 10;
            codeInt++;
            codeNext = codeInt.ToString(CultureInfo.InvariantCulture);
          }
          else
          {
            codeNext = "11";
          }

          #endregion Next Sequential Code
        }

        #region Insert Local District

        var insertSql = "INSERT INTO LocalDistricts ";
        insertSql += "(";
        insertSql += "StateCode";
        insertSql += ",CountyCode";
        insertSql += ",LocalCode";
        insertSql += ",LocalDistrict";
        insertSql += ",StateLocalDistrictCode";
        insertSql += ",Contact";
        insertSql += ",ContactTitle";
        insertSql += ",ContactEmail";
        insertSql += ",Phone";
        insertSql += ",AltContact";
        insertSql += ",AltContactTitle";
        insertSql += ",AltEMail";
        insertSql += ",AltPhone";
        insertSql += ",EMail";
        insertSql += ",URL";
        insertSql += ",BallotName";
        insertSql += ",ElectionsAuthority";
        insertSql += ",AddressLine1";
        insertSql += ",AddressLine2";
        insertSql += ",CityStateZip";
        insertSql += ",Notes";
        insertSql += ",EmailPage";
        insertSql += ",URLDataPage";
        insertSql += ",IsLocalDistrictTagForDeletion";
        insertSql += ",ElectionKeyOfficialsReportStatus";
        insertSql += ")";
        insertSql += " VALUES(";
        insertSql += SqlLit(StateCode);
        insertSql += "," + SqlLit(CountyCode);

        if (TextBoxCode.Text.Trim() != string.Empty)
          insertSql += "," + SqlLit(TextBoxCode.Text.Trim());
        else
          insertSql += "," + SqlLit(codeNext);

        insertSql += "," + SqlLit(TextBoxLocalDistrictAdd.Text.Trim());

        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";

        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",0";
        insertSql += ",''";
        insertSql += ")";
        G.ExecuteSql(insertSql);

        #endregion Insert Local District

        #region clear textboxes

        TextBoxLocalDistrictAdd.Text = string.Empty;
        TextBoxCode.Text = string.Empty;

        #endregion clear textboxes

        Local_Links();

        Msg.Text =
          Ok("The local district or town name has been added and should appear in the list below.");
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);

        #endregion
      }
    }

    protected void RadioButtonListSort_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        Local_Links();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonAppendDistrict_Click(object sender, EventArgs e)
      => TextBoxLocalDistrictAdd.Text = TextBoxLocalDistrictAdd.Text.Trim() + " District";

    protected void ButtonAppendTown_Click(object sender, EventArgs e)
      => TextBoxLocalDistrictAdd.Text = TextBoxLocalDistrictAdd.Text.Trim() + " Town";

    protected void ButtonAppendCity_Click(object sender, EventArgs e)
      => TextBoxLocalDistrictAdd.Text = TextBoxLocalDistrictAdd.Text.Trim() + " City";

    protected void Button_Format_Name_Click(object sender, EventArgs e)
    {
      var textbox = sender == Button_Format_Name_Add
        ? TextBoxLocalDistrictAdd
        : TextBoxLocalDistrictUpdate;
      if (!string.IsNullOrWhiteSpace(textbox?.Text))
      {
        textbox.Text = textbox.Text.Trim();
        textbox.Text = Str_Remove_SpecialChars_All_Except_Spaces(textbox.Text);
        textbox.Text = FormatName(textbox.Text);
        Msg.Text = Message("Name has been re-cased.");
      }
    }

    protected void Button_View_Offices_Click(object sender, EventArgs e)
    {
      try
      {
        Label_Offices.Text = string.Empty;
        Label_Offices.Text += "<strong>Offices in this Local District or Town</strong>";

        if (
          Rows("Offices", "StateCode", StateCode, "CountyCode",
            CountyCode, "LocalCode", LocalCode) > 0)
        {
          var officesTable = DataTable_Offices();
          foreach (DataRow officeRow in officesTable.Rows)
          {
            Label_Offices.Text += "<br>----------------";
            Label_Offices.Text += "<br>";
            Label_Offices.Text += GetOfficeData(officeRow["OfficeKey"].ToString());
          }
        }
        else
        {
          Label_Offices.Text = "No Offices in this Local District or Town";
        }
        Msg.Text = Ok("Any office(s) for this local district or town is shown above.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void Button_Delete_Local_Click(object sender, EventArgs e)
    {
      try
      {
        if (TextBoxLocalDistrictUpdate.Text != string.Empty)
        {
          var sql = string.Empty;
          sql += " DELETE FROM LocalDistricts";
          sql += " WHERE StateCode = " + SqlLit(StateCode);
          sql += " AND CountyCode = " + SqlLit(CountyCode);
          sql += " AND LocalCode = " + SqlLit(LocalCode);
          G.ExecuteSql(sql);

          var officesTable = DataTable_Offices();
          foreach (DataRow officeRow in officesTable.Rows)
          {
            Office_Delete_All_Tables_All_Rows(officeRow["OfficeKey"].ToString());
          }

          Msg.Text = Ok(TextBoxLocalDistrictUpdate.Text + " has been deleted.");
          TextBoxLocalDistrictUpdate.Text = string.Empty;

          Local_Links();
        }
        else
        {
          Msg.Text = Fail("No local district or town was identified.");
        }
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Title = H1.InnerText = "Districts";

        var mainForm = Master.FindControl("MainForm") as HtmlControl;
        Debug.Assert(mainForm != null, "mainForm != null");
        mainForm.Attributes.Add("data-state", StateCode);
        mainForm.Attributes.Add("data-county", CountyCode);

        try
        {
          #region Page Title

          PageTitle.Text = Offices.GetElectoralClassDescription(StateCode,
            CountyCode);
          PageTitle.Text += "<br>";


          PageTitle.Text += "ADD & EDIT LOCAL DISTRICTS OR TOWNS";

          #endregion Page Title

          if (string.IsNullOrEmpty(LocalCode))
          {
            #region Setup to Add a Local District

            TableLocalDistrictAdd.Visible = true;
            TableLocalDistrictUpdate.Visible = false;

            #endregion Setup to Add a Local District

            Msg.Text =
              Message("Use the section below to add a local district or town in this county." +
                " Or click one of the links in the Local Districts and Towns Report" +
                " to edit a district or town name.");
          }
          else
          {
            #region Update a Local District

            TableLocalDistrictAdd.Visible = true;
            TableLocalDistrictUpdate.Visible = true;

            #region Load Name in Textbox

            TextBoxLocalDistrictUpdate.Text =
              GetPageCache()
                .LocalDistricts.GetLocalDistrict(StateCode,
                  CountyCode, LocalCode);

            #endregion Load Name in Textbox

            Msg.Text =
              Message("Use the section below to add a local district or town in this county." +
                " Use the next section to edit the district or town shown in the textbox." +
                " Or click one of the links in the Local Districts and Towns Report" +
                " to edit a different district or town name.");

            #endregion Update a Local District
          }

          Local_Links();
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          LogAdminError(ex);
        }
      }
    }
  }
}