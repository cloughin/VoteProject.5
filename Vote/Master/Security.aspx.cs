using System;
using System.Data;
using System.Web.UI.WebControls;
using DB.VoteLog;

namespace Vote.Master
{
  public partial class SecurityPage : SecurePage, ISuperUser
  {
    #region from db

    public static string States_51()
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " StateCode ";
      sql += " ,State ";
      sql += " FROM States ";
      sql += " WHERE StateCode != 'US' ";
      sql += " AND StateCode != 'U1' ";
      sql += " AND StateCode != 'U2' ";
      sql += " AND StateCode != 'U3' ";
      sql += " AND StateCode != 'U4' ";
      sql += " AND StateCode != 'AS' ";
      sql += " AND StateCode != 'GU' ";
      sql += " AND StateCode != 'VI' ";
      sql += " AND StateCode != 'PR' ";
      sql += " AND StateCode != 'PP' ";
      sql += " ORDER BY State";
      return sql;
    }

    public static string Ok(string msg)
    {
      return "<span class=" + "\"" + "MsgOk" + "\"" + ">"
        + "SUCCESS!!! " + msg + "</span>";
    }

    public static string Fail(string msg)
    {
      return "<span class=" + "\"" + "MsgFail" + "\"" + ">"
        + "****FAILURE**** " + msg + "</span>";
    }

    public static string Message(string msg)
    {
      return "<span class=" + "\"" + "Msg" + "\"" + ">"
        + msg + "</span>";
    }

    public static void Log_Error_Admin(Exception ex, string message = null)
    {
      var logMessage = string.Empty;
      var stackTrace = string.Empty;
      if (ex != null)
      {
        logMessage = ex.Message;
        stackTrace = ex.StackTrace;
      }
      if (!string.IsNullOrWhiteSpace(message))
      {
        if (!string.IsNullOrWhiteSpace(logMessage))
          logMessage += " :: ";
        logMessage += message;
      }
      LogErrorsAdmin.Insert(DateTime.Now, UrlManager.GetCurrentPathUri(true).ToString(),
        logMessage, stackTrace);
    }

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    public static string DbErrorMsg(string sql, string err)
    {
      //Write code to log database errors to a DBFailues Table
      return "Database Failure for SQL Statement::" + sql + " :: Error Msg:: " + err;
    }

    public static string Single_Key_Str(string tableName, string column, string keyName,
      string keyValue)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
        + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());

      var table = G.Table(sql);
      if (table.Rows.Count == 1)
      {
        // modified to pass through nulls (Politician.Address etc)
        //return (string)table.Rows[0][Column].ToString().Trim();
        var result = table.Rows[0][column] as string;
        result = result?.Trim();
        return result;
      }
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    public static bool Single_Key_Bool(string tableName, string column, string keyName,
      string keyValue)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
        + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());
      var table = G.Table(sql);
      if (table.Rows.Count == 1)
      {
        return Convert.ToBoolean(table.Rows[0][column]);
      }
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    private static bool Security_Bool(string userName, string column)
    {
      return (userName != string.Empty) && Single_Key_Bool("Security", column, "UserName", userName);
    }

    public static void Single_Key_Update_Str(string table, string column, string columnValue,
      string keyName, string keyValue)
    {
      Db.UpdateColumnByKey(table, column, columnValue, keyName, keyValue);
    }

    public static string Security_Str(string userName, string column)
    {
      return userName != string.Empty
        ? Single_Key_Str("Security", column, "UserName", userName)
        : string.Empty;
    }

    public static bool Security_User_Is_Has_DataEntry_Authority(string userName)
    {
      return Security_Bool(userName, "UserIsHasDataEntryAuthority");
    }

    private static void SecurityUpdate(string userName, string column, string columnValue)
    {
      Single_Key_Update_Str("Security", column, columnValue, "UserName", userName);
    }

    private static bool Is_Valid_Security(string userName)
    {
      if (!string.IsNullOrEmpty(userName))
        return G.Rows("Security", "UserName", userName) == 1;
      return false;
    }

    #endregion from db

    private void Reset_User_Type_Radio_Buttons()
    {
      //RadioButtonList_User_Type.Items[RadioButtonList_User_Type.SelectedIndex].Selected = false;

      for (var i = 0; i <= RadioButtonList_User_Type.Items.Count - 1; i++)
      {
        RadioButtonList_User_Type.Items[i].Selected = false;
      }
    }

    private void Check_User_Name()
    {
      if (string.IsNullOrEmpty(TextBox_User_Name.Text.Trim()))
        throw new ApplicationException("The User Name textbox is empty.");
    }

    private void Check_User_Password()
    {
      if (!IsSuperUser)
      {
        if (string.IsNullOrEmpty(TextBox_User_Password.Text.Trim()))
          throw new ApplicationException("The User Password textbox is empty.");
      }
    }

    private void Check_User_Not_Exist()
    {
      if (Is_Valid_Security(TextBox_User_Name.Text.Trim()))
        throw new ApplicationException("The User Already exists.");
    }

    private void Check_User_Exist()
    {
      if (!Is_Valid_Security(TextBox_User_Name.Text.Trim()))
        throw new ApplicationException("The User Already exists.");
    }

    private void Check_User_Name_Password()
    {
      //if (string.IsNullOrEmpty(TextBox_User_Name.Text.Trim()))
      //  throw new ApplicationException("The User Name textbox is empty.");
      Check_User_Name();

      //if (!SecurePage.IsSuperUser)
      //{
      //  if (string.IsNullOrEmpty(TextBox_User_Password.Text.Trim()))
      //    throw new ApplicationException("The User Password textbox is empty.");
      //}
      Check_User_Password();

      //if (db.Is_Valid_Security(TextBox_User_Name.Text.Trim()))
      //  throw new ApplicationException("The User Already exists.");
      Check_User_Not_Exist();
    }

    private void Controls_Enabled_On_Entry()
    {
      TextBox_User_Name.Enabled = true;
      TextBox_User_Password.Enabled = true;

      RadioButtonList_User_Type.Enabled = true;

      Button_Different_User.Enabled = false;
      Button_Delete.Enabled = false;

      //Only Users enabled on entry
      ListBox_Users.Enabled = true;
      ListBox_Type.Enabled = false;
      ListBox_Design.Enabled = false;
      ListBox_Organization.Enabled = false;
      ListBox_State.Enabled = false;
      ListBox_County.Enabled = false;
      ListBox_Local.Enabled = false;

      Button_Update_UserName_Pasword.Visible = false;
    }

    private void Controls_Enabled_Update_Master()
    {
      TextBox_User_Name.Enabled = true;
      TextBox_User_Password.Enabled = true;

      RadioButtonList_User_Type.Enabled = false;

      Button_Different_User.Enabled = true;
      Button_Delete.Enabled = true;

      ListBox_Users.Enabled = true;
      ListBox_Type.Enabled = true;
      ListBox_Design.Enabled = true;
      ListBox_Organization.Enabled = true;
      ListBox_State.Enabled = true;
      ListBox_County.Enabled = false;
      ListBox_Local.Enabled = false;

      Button_Update_UserName_Pasword.Visible = true;
    }

    private void Controls_Enabled_Update_Admin()
    {
      TextBox_User_Name.Enabled = false;
      TextBox_User_Password.Enabled = false;
      ListBox_Users.Enabled = false;
      RadioButtonList_User_Type.Enabled = false;
      Button_Different_User.Enabled = true;
      Button_Delete.Enabled = true;
      ListBox_Design.Enabled = false;
      ListBox_Organization.Enabled = false;
      ListBox_State.Enabled = true;
      ListBox_County.Enabled = false;
      ListBox_Local.Enabled = false;

      Button_Update_UserName_Pasword.Visible = true;
    }

    private void Controls_Enabled_Update_County()
    {
      TextBox_User_Name.Enabled = false;
      TextBox_User_Password.Enabled = false;
      ListBox_Users.Enabled = false;
      RadioButtonList_User_Type.Enabled = false;
      Button_Different_User.Enabled = true;
      Button_Delete.Enabled = true;
      ListBox_Design.Enabled = false;
      ListBox_Organization.Enabled = false;
      ListBox_State.Enabled = true;
      ListBox_County.Enabled = true;
      ListBox_Local.Enabled = false;

      Button_Update_UserName_Pasword.Visible = true;
    }

    private void Controls_Enabled_Update_Local()
    {
      TextBox_User_Name.Enabled = false;
      TextBox_User_Password.Enabled = false;
      ListBox_Users.Enabled = false;
      RadioButtonList_User_Type.Enabled = false;
      Button_Different_User.Enabled = true;
      Button_Delete.Enabled = true;
      ListBox_Design.Enabled = false;
      ListBox_Organization.Enabled = false;
      ListBox_State.Enabled = true;
      ListBox_County.Enabled = true;
      ListBox_Local.Enabled = true;

      Button_Update_UserName_Pasword.Visible = true;
    }

    private void Controls_Enabled_Update_Design()
    {
      TextBox_User_Name.Enabled = false;
      TextBox_User_Password.Enabled = false;
      ListBox_Users.Enabled = false;
      RadioButtonList_User_Type.Enabled = false;
      Button_Different_User.Enabled = true;
      Button_Delete.Enabled = true;
      ListBox_Design.Enabled = true;
      ListBox_Organization.Enabled = false;
      ListBox_State.Enabled = false;
      ListBox_County.Enabled = false;
      ListBox_Local.Enabled = false;

      Button_Update_UserName_Pasword.Visible = true;
    }

    private void Controls_Enabled_Update_Organization()
    {
      TextBox_User_Name.Enabled = false;
      TextBox_User_Password.Enabled = false;
      ListBox_Users.Enabled = false;
      RadioButtonList_User_Type.Enabled = false;
      Button_Different_User.Enabled = true;
      Button_Delete.Enabled = true;
      ListBox_Design.Enabled = false;
      ListBox_Organization.Enabled = true;
      ListBox_State.Enabled = false;
      ListBox_County.Enabled = false;
      ListBox_Local.Enabled = false;

      Button_Update_UserName_Pasword.Visible = true;
    }

    private void Load_Users()
    {
      ListBox_Users.Items.Clear();
      ListBox_Type.Items.Clear();

      var sql = string.Empty;
      sql += "SELECT ";
      sql += " UserName";
      sql += ",UserSecurity";
      sql += ",UserIsHasDataEntryAuthority";
      sql += " FROM Security";
      sql += " ORDER BY UserName";

      var securityTable = G.Table(sql);
      foreach (DataRow securityRow in securityTable.Rows)
      {
        var listBoxUserItem = new ListItem
        {
          Value = securityRow["UserName"].ToString(),
          Text = securityRow["UserName"].ToString()
        };
        ListBox_Users.Items.Add(listBoxUserItem);

        var listBoxTypeItem = new ListItem
        {
          Value = securityRow["UserSecurity"].ToString(),
          Text = securityRow["UserSecurity"].ToString()
        };
        if (Convert.ToBoolean(securityRow["UserIsHasDataEntryAuthority"]) == false)
          listBoxTypeItem.Text += "-N";
        else
          listBoxTypeItem.Text += "-Y";
        ListBox_Type.Items.Add(listBoxTypeItem);
      }
    }

    private void Load_DesignCodes()
    {
      ListBox_Design.Items.Clear();
      var sql = string.Empty;
      sql += "SELECT ";
      sql += " DomainDesignCode ";
      sql += " FROM DomainDesigns ";
      sql += " ORDER BY DomainDesignCode";

      var domainDesignsTable = G.Table(sql);
      foreach (DataRow domainDesignRow in domainDesignsTable.Rows)
      {
        var listBoxDesignItem = new ListItem
        {
          Value = domainDesignRow["DomainDesignCode"].ToString(),
          Text = domainDesignRow["DomainDesignCode"].ToString()
        };
        ListBox_Design.Items.Add(listBoxDesignItem);
      }
    }

    private void Load_OrganizationCodes()
    {
      ListBox_Organization.Items.Clear();
      var sql = string.Empty;
      sql += "SELECT ";
      sql += " OrganizationCode ";
      sql += " FROM Organizations ";
      sql += " ORDER BY OrganizationCode";

      var organizationTable = G.Table(sql);
      foreach (DataRow organizationRow in organizationTable.Rows)
      {
        var listBoxOrganizationItem = new ListItem
        {
          Value = organizationRow["OrganizationCode"].ToString(),
          Text = organizationRow["OrganizationCode"].ToString()
        };
        ListBox_Organization.Items.Add(listBoxOrganizationItem);
      }
    }

    private void Load_StateCodes()
    {
      ListBox_State.Items.Clear();

      //ListItem ListBox_State_Item = new ListItem();
      //ListBox_State_Item.Value = "US";
      //ListBox_State_Item.Text = "US";
      //ListBox_State.Items.Add(ListBox_State_Item);

      var statesTable = G.Table(States_51());
      foreach (DataRow stateRow in statesTable.Rows)
      {
        var listBoxStateItem = new ListItem
        {
          Value = stateRow["StateCode"].ToString(),
          Text = stateRow["StateCode"].ToString()
        };
        ListBox_State.Items.Add(listBoxStateItem);
      }
    }

    private void Reload_All_Controls(string userName)
    {
      Load_Users();
      Load_DesignCodes();
      Load_OrganizationCodes();
      Load_StateCodes();

      if (!string.IsNullOrEmpty(userName))
      {
        TextBox_User_Name.Text = Security_Str(userName, "UserName");
        TextBox_User_Password.Text = Security_Str(userName, "UserPassword");

        Label_Type.Text = Security_Str(userName, "UserSecurity");
        Label_UserName.Text = Security_Str(userName, "UserName");
        Label_Design.Text = Security_Str(userName, "UserDesignCode");
        Label_Organization.Text = Security_Str(userName, "UserOrganizationCode");
        Label_State.Text = Security_Str(userName, "UserStateCode");
        Label_County.Text = Security_Str(userName, "UserCountyCode");
        Label_Local.Text = Security_Str(userName, "UserLocalCode");

        if (Security_User_Is_Has_DataEntry_Authority(userName))
        {
          CheckBox_Has_Data_Entry_Authority.Checked = true;
          Label_Type.Text += "-Y";
        }
        else
        {
          CheckBox_Has_Data_Entry_Authority.Checked = false;
          Label_Type.Text += "-N";
        }
      }
      else
      {
        TextBox_User_Name.Text = string.Empty;
        TextBox_User_Password.Text = string.Empty;

        Label_Type.Text = string.Empty;
        Label_UserName.Text = string.Empty;
        Label_Design.Text = string.Empty;
        Label_Organization.Text = string.Empty;
        Label_State.Text = string.Empty;
        Label_County.Text = string.Empty;
        Label_Local.Text = string.Empty;

        CheckBox_Has_Data_Entry_Authority.Checked = false;
      }
    }

    private void Reload_All_Controls()
    {
      Reload_All_Controls(string.Empty);
    }

    private void Create_User()
    {
      var sqlSecurityInsert = string.Empty;
      sqlSecurityInsert += "INSERT INTO Security";
      sqlSecurityInsert += "(";
      sqlSecurityInsert += "UserName";
      sqlSecurityInsert += ",UserPassword";
      sqlSecurityInsert += ",UserSecurity";
      sqlSecurityInsert += ",UserIsHasDataEntryAuthority";
      sqlSecurityInsert += ")";
      sqlSecurityInsert += "VALUES";
      sqlSecurityInsert += "(";
      sqlSecurityInsert += SqlLit(TextBox_User_Name.Text.Trim());
      sqlSecurityInsert += "," + SqlLit(TextBox_User_Password.Text.Trim());
      sqlSecurityInsert += "," + SqlLit(RadioButtonList_User_Type.SelectedValue);
      if (CheckBox_Has_Data_Entry_Authority.Checked)
        sqlSecurityInsert += ",1";
      else
        sqlSecurityInsert += ",0";

      sqlSecurityInsert += ")";
      G.ExecuteSql(sqlSecurityInsert);
    }

    protected void Button_Update_UserName_Pasword_Click(object sender, EventArgs e)
    {
      try
      {
        Check_User_Name_Password();

        if (!string.Equals(TextBox_User_Name.Text.Trim(), Label_UserName.Text, StringComparison.OrdinalIgnoreCase)
          && Is_Valid_Security(TextBox_User_Name.Text.Trim())
        )
          throw new ApplicationException("You can't cange the name to a user that already exists.");

        //Need to save for Load_TextBoxes_And_Labels
        var updatedUser = TextBox_User_Name.Text.Trim();

        //password needs to be done first because UserName is the key
        SecurityUpdate(Label_UserName.Text.Trim(), "UserPassword", TextBox_User_Password.Text.Trim());
        SecurityUpdate(Label_UserName.Text.Trim(), "UserName", TextBox_User_Name.Text.Trim());

        Reload_All_Controls(updatedUser);
        //Load_TextBoxes_And_Labels(Updated_User);
        Msg.Text = Ok("The user has been updated.");
      }
      catch (Exception ex)
      {
        Reset_User_Type_Radio_Buttons();
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void RadioButtonList_User_Type_SelectedIndexChanged1(object sender, EventArgs e)
    {
      try
      {
        //Need to save for Reload_All_Controls
        var newUser = TextBox_User_Name.Text;
        Check_User_Name_Password();
        switch (RadioButtonList_User_Type.SelectedValue)
        {
          case "MASTER":
            Create_User();
            Controls_Enabled_Update_Master();
            Msg.Text = Ok("The Master User has been created with maximum capabilities."
              + " You may select a Design Code and/or Organization Code to limit the Mater User"
              + " to a particular design and/or organization.");
            break;
          case "ADMIN":
            Create_User();
            Controls_Enabled_Update_Admin();
            Msg.Text = Message("The State Adminsitrator User has been created."
              + " You MUST now select the State to administer.");
            break;
          case "COUNTY":
            Create_User();
            Controls_Enabled_Update_County();
            Msg.Text = Message("The County Adminsitrator User has been created."
              + " You MUST now select the State and County to administer.");
            break;
          case "LOCAL":
            Create_User();
            Controls_Enabled_Update_Local();
            Msg.Text = Message("The Local District Adminsitrator User has been created."
              + " You MUST now select the State, County and Local District to administer.");
            break;
          case "DESIGN":
            Create_User();
            Controls_Enabled_Update_Design();
            break;
          case "ORGANIZATION":
            Create_User();
            Controls_Enabled_Update_Organization();
            break;
          case "ISSUES":
            Msg.Text = Fail("Administrators for Issues has not yet been implemented.");
            break;
          case "PARTIES":
            Msg.Text = Fail("Administrators for Parties has not yet been implemented.");
            break;
        }
        Reload_All_Controls(newUser);

        Reset_User_Type_Radio_Buttons();
      }
      catch (Exception ex)
      {
        Reset_User_Type_Radio_Buttons();
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void Button_Different_User_Click(object sender, EventArgs e)
    {
      try
      {
        var userType = Security_Str(Label_UserName.Text, "UserSecurity");
        switch (userType)
        {
          case "MASTER":
            break;
          case "ADMIN":
            if (string.IsNullOrEmpty(Security_Str(Label_UserName.Text, "UserStateCode")))
              throw new ApplicationException("You need to select a State before proceeding.");
            break;
          case "COUNTY":
            break;
          case "LOCAL":
            break;
          case "DESIGN":
            break;
          case "ORGANIZATION":
            break;
          case "ISSUES":
            break;
          case "PARTIES":
            break;
        }

        Reload_All_Controls();
        Controls_Enabled_On_Entry();
        Msg.Text = Message("Select a user from the Users List Box to edit an existing user."
          + " Or enter a new User name and password and select a user type to add another user.");
      }
      catch (Exception ex)
      {
        Reset_User_Type_Radio_Buttons();
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void Button_Delete_Click(object sender, EventArgs e)
    {
      try
      {
        var sqlDelete = string.Empty;
        sqlDelete += "DELETE FROM Security";
        sqlDelete += " WHERE UserName = " + SqlLit(Label_UserName.Text);
        G.ExecuteSql(sqlDelete);


        Reload_All_Controls();
        Controls_Enabled_On_Entry();
        Msg.Text = Ok("The user has been deleted.");
      }
      catch (Exception ex)
      {
        Reset_User_Type_Radio_Buttons();
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ListBox_Users_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        var userSecurity =
          Security_Str(ListBox_Users.SelectedItem.ToString(), "UserSecurity").ToUpper();
        switch (userSecurity)
        {
          case "MASTER":
            Controls_Enabled_Update_Master();
            break;
          case "ADMIN":
            Controls_Enabled_Update_Admin();
            break;
          case "COUNTY":
            Controls_Enabled_Update_County();
            break;
          case "LOCAL":
            Controls_Enabled_Update_Local();
            break;
          case "DESIGN":
            Controls_Enabled_Update_Design();
            break;
          case "ORGANIZATION":
            Controls_Enabled_Update_Organization();
            break;
          case "ISSUES":
            break;
          case "PARTIES":
            break;
        }

        Reload_All_Controls(ListBox_Users.SelectedItem.ToString());

        Msg.Text =
          Message(
            "You may change the User Name, Password, or codes limiting the administrators capabilities.");
      }
      catch (Exception ex)
      {
        Reset_User_Type_Radio_Buttons();
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ListBox_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        SecurityUpdate(Label_UserName.Text, "UserSecurity", ListBox_Type.SelectedItem.ToString());

        Reload_All_Controls(Label_UserName.Text);

        Msg.Text = Ok("The User has been updated.");
      }
      catch (Exception ex)
      {
        Reset_User_Type_Radio_Buttons();
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ListBox_Design_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        SecurityUpdate(TextBox_User_Name.Text.Trim(), "UserDesignCode",
          ListBox_Design.SelectedItem.ToString());
        Msg.Text =
          Ok("The user has been updated to permit administration for the Design whose code is: "
            + ListBox_Design.SelectedItem);
        //Needs to come aftger Msg because ListBox_Organization.SelectedItem is reset by Reload_All_Controls
        Reload_All_Controls(Label_UserName.Text);
      }
      catch (Exception ex)
      {
        Reset_User_Type_Radio_Buttons();
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ListBox_Organization_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        SecurityUpdate(TextBox_User_Name.Text.Trim(), "UserOrganizationCode",
          ListBox_Organization.SelectedItem.ToString());
        Msg.Text =
          Ok("The user has been updated to permit administration for the Oranization whose code is: "
            + ListBox_Organization.SelectedItem);
        //Needs to come aftger Msg because ListBox_Organization.SelectedItem is reset by Reload_All_Controls
        Reload_All_Controls(Label_UserName.Text);
      }
      catch (Exception ex)
      {
        Reset_User_Type_Radio_Buttons();
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ListBox_State_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        //if (db.Security_Str(Label_UserName.Text, "UserSecurity") != "ADMIN")
        //  throw new ApplicationException("Only a State Administration User can identify a State to administer.");

        SecurityUpdate(Label_UserName.Text, "UserStateCode", ListBox_State.SelectedItem.ToString());

        Reload_All_Controls(Label_UserName.Text);

        Msg.Text = Ok("The State Administrator User has been updated.");
      }
      catch (Exception ex)
      {
        Reset_User_Type_Radio_Buttons();
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    private void Page_Load(object sender, EventArgs e)
    {
      Response.Redirect("/master/UpdateUsers.aspx");
      Response.End();
      //if (db.User() != db.User_.Master)
      if (!IsSuperUser)
        HandleSecurityException();

      Title = H1.InnerText = "Security";

      try
      {
        if (!IsPostBack)
        {
          Reload_All_Controls();
          Controls_Enabled_On_Entry();
        }
      }

      catch (Exception ex)
      {
        Reset_User_Type_Radio_Buttons();
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void Button_Update_Data_Entry_Authority_Click(object sender, EventArgs e)
    {
      try
      {
        Check_User_Name();
        Check_User_Exist();

        var sql = " UPDATE Security";
        if (CheckBox_Has_Data_Entry_Authority.Checked)
          sql += " SET UserIsHasDataEntryAuthority = 1";
        else
          sql += " SET UserIsHasDataEntryAuthority = 0";
        sql += " WHERE UserName = " + SqlLit(TextBox_User_Name.Text.Trim());
        G.ExecuteSql(sql);

        Reload_All_Controls(TextBox_User_Name.Text.Trim());

        Msg.Text = Ok("The user's authority to enter politician data has been updated.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    #region Dead code

    //protected void xClear_User_Name_Password()
    //{
    //  TextBox_User_Name.Text = string.Empty;
    //  TextBox_User_Password.Text = string.Empty;
    //}

    #endregion Dead code
  }
}