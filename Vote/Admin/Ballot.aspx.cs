using System;
//using DB.Vote;

namespace Vote.Admin
{
  public partial class BallotPage : VotePage
  {
    //#region from db

    //private static void States_Update_Bool(string stateCode, string column, bool columnValue)
    //{
    //  db.Single_Key_Update_Bool("States", column, columnValue, "StateCode", stateCode);
    //}

    //private static void States_Update_Str(string stateCode, string column, string columnValue)
    //{
    //  db.Single_Key_Update_Str("States", column, columnValue, "StateCode", stateCode);
    //}
    //#endregion from db

    //private void Visible_Controls()
    //{
    //  var stateCode = ViewState["StateCode"].ToString();
    //  var countyCode = ViewState["CountyCode"].ToString();
    //  var localCode = ViewState["LocalCode"].ToString();

    //  switch (db.Electoral_Class(stateCode, countyCode, localCode))
    //  {
    //    case db.ElectoralClass.State:
    //      #region State
    //      TableBallotDesign.Visible = true;
    //      RadioButtonListIncumbent.SelectedValue = 
    //        States.GetIsIncumbentShownOnBallots(stateCode).ToString();
    //      RadioButtonListEncloseNickname.SelectedValue =
    //        States.GetEncloseNickname(stateCode);
    //      RadioButtonListUnopposed.SelectedValue =
    //        States.GetShowUnopposed(stateCode).ToString();
    //      RadioButtonListWriteIn.SelectedValue =
    //        States.GetShowWriteIn(stateCode).ToString();
    //      #endregion State
    //      break;
    //    case db.ElectoralClass.County:
    //      #region County
    //      TableBallotDesign.Visible = false;
    //      #endregion County
    //      break;
    //    case db.ElectoralClass.Local:
    //      #region Local District
    //      TableBallotDesign.Visible = false;
    //      #endregion Local District
    //      break;
    //  }
    //}

    //private void Page_Title()
    //{
    //  PageTitle.Text = string.Empty;
    //  PageTitle.Text += Offices.GetElectoralClassDescription(
    //     ViewState["StateCode"].ToString()
    //    , ViewState["CountyCode"].ToString()
    //    , ViewState["LocalCode"].ToString());

    //  PageTitle.Text += "<br>";
    //  PageTitle.Text += "BALLOT DESIGN & CONTENT";

    //}

    //#region Event handlers and overrides

    //protected void TextBoxBallotName_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxBallotName);

    //    //if (Get("BallotName") != TextBoxBallotName.Text.Trim())
    //    if (db.States_Str(ViewState["StateCode"].ToString(), "BallotName") != TextBoxBallotName.Text.Trim())
    //    {
    //      //Update("BallotName", TextBoxBallotName.Text.Trim());
    //      States_Update_Str(ViewState["StateCode"].ToString(), "BallotName", TextBoxBallotName.Text.Trim());


    //      //LoadBallotName4Voters();
    //      TextBoxBallotName.Text = db.States_Str(ViewState["StateCode"].ToString(), "BallotName");

    //      Msg.Text = db.Ok("The election name on ballots was recorded.");
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}

    //protected void RadioButtonListIncumbent_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    States_Update_Bool(ViewState["StateCode"].ToString(),
    //      "IsIncumbentShownOnBallots",
    //      RadioButtonListIncumbent.SelectedValue == "True");
    //    Msg.Text = db.Ok("Incumbents shown on ballots indication has been set.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListUnopposed_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    States_Update_Bool(ViewState["StateCode"].ToString(),
    //      "ShowUnopposed",
    //      RadioButtonListUnopposed.SelectedValue == "True");
    //    Msg.Text = db.Ok("Unopposed contests shown on ballots indication has been set.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListWriteIn_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    States_Update_Bool(ViewState["StateCode"].ToString(),
    //      "ShowWriteIn",
    //      RadioButtonListWriteIn.SelectedValue == "True");
    //    Msg.Text = db.Ok("Write-in line shown on ballots indication has been set.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListEncloseNickname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    States_Update_Str(ViewState["StateCode"].ToString(), "EncloseNickname", RadioButtonListEncloseNickname.SelectedValue);
    //    Msg.Text = db.Ok("Nickname indication has been set.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}

    protected override void OnPreInit(EventArgs e)
    {
      var query = Request.QueryString.ToString();
      var url = "/admin/updatejurisdictions.aspx";
      if (!string.IsNullOrWhiteSpace(query))
      {
        url += "?" + query;
        if (string.IsNullOrWhiteSpace(QueryCounty))
          url += "#ballot";
      }
      Response.Redirect(url);
      base.OnPreInit(e);
    }

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    Page.Title = "Ballot";

    //    ViewState["StateCode"] = db.State_Code();
    //    ViewState["CountyCode"] = db.County_Code();
    //    ViewState["LocalCode"] = db.Local_Code();

    //    try
    //    {
    //      Visible_Controls();

    //      Page_Title();

    //      TextBoxBallotName.Text = db.States_Str(ViewState["StateCode"].ToString(), "BallotName") != string.Empty 
    //        ? db.States_Str(ViewState["StateCode"].ToString(), "BallotName") 
    //        : StateCache.GetStateName(ViewState["StateCode"].ToString());

    //      //SetPanelVisibility(new Control[] { Banner });
    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}

    //#endregion Event handlers and overrides
  }
}
