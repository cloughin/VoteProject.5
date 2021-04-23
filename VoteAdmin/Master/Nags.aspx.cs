using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB.VoteCache;
using static System.String;

namespace Vote.Master
{
  public partial class NagsPage : SecurePage, ISuperUser
  {
    #region Declarations

    private const string TextBoxMessageNumberId = "TextBoxMessageNumber";
    private const string TextBoxNextMessageNumberId = "TextBoxNextMessageNumber";
    private const string TextBoxMessageTextId = "TextBoxMessageText";

    #endregion Declarations

    #region Utility Methods

    private static int GetMsgNoFromId(string id)
    {
      var match = Regex.Match(id, "[0-9]+");
      if (match.Success)
        return int.Parse(match.Value);
      return -1;
    }

    private static string GetMsgTypeFromId(string id)
    {
      var match = Regex.Match(id, "[a-z]+", RegexOptions.IgnoreCase);
      return match.Success ? match.Value : Empty;
    }

    #endregion Utility Methods

    #region Validation Methods

    private void CheckForDuplicateMsgNo(int msgNo)
    {
      if (!DonationNags.MessageNumberExists(msgNo)) return;
      var tbMessageNumber =
        Master.FindMainContentControl(TextBoxMessageNumberId) as TextBox;
      NagFeedback.PostValidationError(
        tbMessageNumber, "MsgNo {0} already exists", msgNo);
    }

    private int GetMsgNoFromForm()
    {
      var tbMessageNumber =
        Master.FindMainContentControl(TextBoxMessageNumberId) as TextBox;
      return NagFeedback.ValidateInt(tbMessageNumber, out _, "MsgNo", 1);
    }

    private string GetMsgTextFromForm()
    {
      var tbMessageText =
        Master.FindMainContentControl(TextBoxMessageTextId) as TextBox;
      return NagFeedback.ValidateRequired(tbMessageText, "Message text", out _);
    }

    private int? GetNextMsgFromForm()
    {
      var tbNextMessageNumber =
        Master.FindMainContentControl(TextBoxNextMessageNumberId) as TextBox;
      var nextMsgNo = NagFeedback.ValidateIntOptional(
        tbNextMessageNumber, out _, "NextMsg", 0, 1);
      return nextMsgNo == 0 ? (int?) null : nextMsgNo;
    }

    #endregion Validation Methods

    #region BuildHtmlTable

    private void BuildHtmlTable()
    {
      var nagsTable = DonationNags.GetAllData();

      // possible modes: normal (Empty), "Edit", "Insert"
      var editMode = GetMsgTypeFromId(NagEditMode.Value);
      var editMessageNumber = GetMsgNoFromId(NagEditMode.Value);

      var htmlTable = InitializeNagsTable();

      foreach (var nagsRow in nagsTable)
        BuildOneNagsDataRow(editMode, editMessageNumber, htmlTable, nagsRow);

      if (editMode != "Edit")
        BuildInsertRow(editMode, htmlTable);
    }

    private void BuildInsertRow(string editMode, Control htmlTable)
    {
      var trInsert = new TableRow();
      htmlTable.Controls.Add(trInsert);

      // First operation column
      var tdInsertAction1 = new TableCell();
      trInsert.Controls.Add(tdInsertAction1);
      if (editMode == "Insert")
      {
        var insertLinkButton1 = new LinkButton();
        tdInsertAction1.Controls.Add(insertLinkButton1);
        insertLinkButton1.CssClass = "op op-add";
        insertLinkButton1.Text = "Add";
        insertLinkButton1.ID = "Add";
        insertLinkButton1.ToolTip = "Update the database with the new message";
        insertLinkButton1.Click += AddButton_Click;
      }
      else
        tdInsertAction1.Text = "&nbsp;";

      // Second operation column
      var tdInsertAction2 = new TableCell();
      trInsert.Controls.Add(tdInsertAction2);
      LinkButton insertLinkButton2;
      if (editMode == "Insert")
      {
        insertLinkButton2 = new LinkButton();
        tdInsertAction2.Controls.Add(insertLinkButton2);
        insertLinkButton2.CssClass = "op op-cancel";
        insertLinkButton2.Text = "Cancel";
        insertLinkButton2.ID = "Cancel";
        insertLinkButton2.ToolTip =
          "Cancel creation of the new message -- no changes will be made to the database";
        insertLinkButton2.Click += CancelButton_Click;
      }
      else
      {
        insertLinkButton2 = new LinkButton();
        tdInsertAction2.Controls.Add(insertLinkButton2);
        insertLinkButton2.CssClass = "op op-insert";
        insertLinkButton2.Text = "Insert";
        insertLinkButton2.ID = "Insert";
        insertLinkButton2.ToolTip = "Create a new message";
        insertLinkButton2.Click += InsertButton_Click;
      }

      // Create data columns
      var tdInsertMessageNumber = new TableCell();
      trInsert.Controls.Add(tdInsertMessageNumber);
      tdInsertMessageNumber.CssClass = "content";

      var tdInsertNextMessageNumber = new TableCell();
      trInsert.Controls.Add(tdInsertNextMessageNumber);
      tdInsertNextMessageNumber.CssClass = "content";

      //var tdInsertMessageHeading = new TableCell();
      //trInsert.Controls.Add(tdInsertMessageHeading);
      //tdInsertMessageHeading.CssClass = "content";

      var tdInsertMessageText = new TableCell();
      trInsert.Controls.Add(tdInsertMessageText);
      tdInsertMessageText.CssClass = "content";

      // Populate data columns
      // If there is an active insertion, populate with TextBoxes
      if (editMode == "Insert")
      {
        TextBox tbInsertMessageNumber = new TextBoxWithNormalizedLineBreaks();
        tdInsertMessageNumber.Controls.Add(tbInsertMessageNumber);
        tbInsertMessageNumber.ID = TextBoxMessageNumberId;
        tbInsertMessageNumber.CssClass = "text-box message-number";

        TextBox tbInsertNextMessageNumber = new TextBoxWithNormalizedLineBreaks();
        tdInsertNextMessageNumber.Controls.Add(tbInsertNextMessageNumber);
        tbInsertNextMessageNumber.ID = TextBoxNextMessageNumberId;
        tbInsertNextMessageNumber.CssClass = "text-box next-message-number";

        TextBox tbInsertMessageText = new TextBoxWithNormalizedLineBreaks();
        tdInsertMessageText.Controls.Add(tbInsertMessageText);
        tbInsertMessageText.ID = TextBoxMessageTextId;
        tbInsertMessageText.CssClass = "text-box message-text";
      }
      // Empty placeholders
      else
      {
        var p = new HtmlP();
        tdInsertMessageNumber.Controls.Add(p);
        p.InnerHtml = "&nbsp;";

        p = new HtmlP();
        tdInsertNextMessageNumber.Controls.Add(p);
        p.InnerHtml = "&nbsp;";

        //p = new HtmlP();
        //tdInsertMessageHeading.Controls.Add(p);
        //p.InnerHtml = "&nbsp;";

        p = new HtmlP();
        tdInsertMessageText.Controls.Add(p);
        p.InnerHtml = "&nbsp;";
      }
    }

    private void BuildOneNagsDataRow(
      string editMode, int editMessageNumber, Control htmlTable,
      DonationNagsRow nagsRow)
    {
      var msgNo = nagsRow.MessageNumber.ToString(CultureInfo.InvariantCulture);

      var tr = new TableRow();
      htmlTable.Controls.Add(tr);

      // First operation column
      var tdAction1 = new TableCell();
      tr.Controls.Add(tdAction1);
      LinkButton button1;
      if (editMode == Empty)
      {
        button1 = new LinkButton();
        tdAction1.Controls.Add(button1);
        button1.CssClass = "op op-edit";
        button1.Text = "Edit";
        button1.ID = "Edit" + msgNo;
        button1.ToolTip = "Edit Msg No " + msgNo;
        button1.Click += EditButton_Click;
      }
      else if (editMode == "Edit" && editMessageNumber == nagsRow.MessageNumber)
      {
        button1 = new LinkButton();
        tdAction1.Controls.Add(button1);
        button1.CssClass = "op op-update";
        button1.Text = "Update";
        button1.ToolTip = "Save the changes to Msg No " + msgNo;
        button1.ID = "Update" + msgNo;
        button1.Click += UpdateButton_Click;
      }
      else
        tdAction1.Text = "&nbsp;";

      // Second operation column
      var tdAction2 = new TableCell();
      tr.Controls.Add(tdAction2);
      LinkButton button2;
      if (editMode == Empty)
      {
        button2 = new LinkButton();
        tdAction2.Controls.Add(button2);
        button2.CssClass = "op op-delete";
        button2.Text = "Delete";
        button2.ID = "Delete" + msgNo;
        button2.ToolTip = "Delete Msg No " + msgNo;
        button2.Click += DeleteButton_Click;
      }
      else if (editMode == "Edit" && editMessageNumber == nagsRow.MessageNumber)
      {
        button2 = new LinkButton();
        tdAction2.Controls.Add(button2);
        button2.CssClass = "op op-cancel";
        button2.Text = "Cancel";
        button2.ID = "Cancel" + msgNo;
        button2.ToolTip = "Cancel editing Msg No " + msgNo +
          " -- no changes will be made";
        button2.Click += CancelButton_Click;
      }
      else
        tdAction2.Text = "&nbsp;";

      // Create data columns
      var tdMessageNumber = new TableCell();
      tr.Controls.Add(tdMessageNumber);
      tdMessageNumber.ID = "MessageNumber" + msgNo;
      tdMessageNumber.CssClass = "content";

      var tdNextMessageNumber = new TableCell();
      tr.Controls.Add(tdNextMessageNumber);
      tdNextMessageNumber.ID = "NextMessageNumber" + msgNo;
      tdNextMessageNumber.CssClass = "content";

      //var tdMessageHeading = new TableCell();
      //tr.Controls.Add(tdMessageHeading);
      //tdMessageHeading.ID = "MessageHeading" + msgNo;
      //tdMessageHeading.CssClass = "content";

      var tdMessageText = new TableCell();
      tr.Controls.Add(tdMessageText);
      tdMessageText.ID = "MessageText" + msgNo;
      tdMessageText.CssClass = "content";

      // Populate data columns
      // If it's the actrive edit column, populate with TextBoxes
      if (editMode == "Edit" && editMessageNumber == nagsRow.MessageNumber)
      {
        TextBox tbMessageNumber = new TextBoxWithNormalizedLineBreaks();
        tdMessageNumber.Controls.Add(tbMessageNumber);
        tbMessageNumber.ID = TextBoxMessageNumberId;
        tbMessageNumber.CssClass = "text-box message-number";

        tbMessageNumber.Text = msgNo;

        TextBox tbNextMessageNumber = new TextBoxWithNormalizedLineBreaks();
        tdNextMessageNumber.Controls.Add(tbNextMessageNumber);
        tbNextMessageNumber.ID = TextBoxNextMessageNumberId;

        tbNextMessageNumber.CssClass = "text-box next-message-number";

        tbNextMessageNumber.Text = nagsRow.NextMessageNumber == null
          ? Empty
          : nagsRow.NextMessageNumber.ToString();

        TextBox tbMessageText = new TextBoxWithNormalizedLineBreaks();
        tdMessageText.Controls.Add(tbMessageText);
        tbMessageText.ID = TextBoxMessageTextId;

        tbMessageText.CssClass = "text-box message-text";

        tbMessageText.Text = IsNullOrWhiteSpace(nagsRow.MessageText)
          ? Empty
          : nagsRow.MessageText;
      }
      // Display-only columns
      else
      {
        var p = new HtmlP();
        tdMessageNumber.Controls.Add(p);
        p.InnerHtml = msgNo;

        p = new HtmlP();
        tdNextMessageNumber.Controls.Add(p);
        p.InnerHtml = nagsRow.NextMessageNumber == null
          ? "&nbsp;"
          : nagsRow.NextMessageNumber.ToString();

        p = new HtmlP();
        tdMessageText.Controls.Add(p);
        p.InnerHtml = IsNullOrWhiteSpace(nagsRow.MessageText)
          ? "&nbsp;"
          : nagsRow.MessageText;
      }
    }

    private Table InitializeNagsTable()
    {
      // Create the table object and add headings
      //
      NagTableContainer.Controls.Clear();
      var htmlTable = new Table {CellSpacing = 0, CellPadding = 0};

      var trHeading = new TableHeaderRow();
      htmlTable.Controls.Add(trHeading);

      var thAction1 = new TableHeaderCell();
      trHeading.Controls.Add(thAction1);
      thAction1.CssClass = "op1";
      thAction1.Text = "&nbsp;";

      var thAction2 = new TableHeaderCell();
      trHeading.Controls.Add(thAction2);
      thAction2.CssClass = "op2";
      thAction2.Text = "&nbsp;";

      var thMessageNumber = new TableHeaderCell();
      trHeading.Controls.Add(thMessageNumber);
      thMessageNumber.CssClass = "message-number content";
      thMessageNumber.Text = "Msg<br />No";

      var thNextMessageNumber = new TableHeaderCell();
      trHeading.Controls.Add(thNextMessageNumber);
      thNextMessageNumber.CssClass = "next-message-number content";
      thNextMessageNumber.Text = "Next<br />Msg";

      //var thMessageHeading = new TableHeaderCell();
      //trHeading.Controls.Add(thMessageHeading);
      //thMessageHeading.CssClass = "message-heading content";
      //thMessageHeading.Text = "&nbsp;<br />Head";

      var thMessageText = new TableHeaderCell();
      trHeading.Controls.Add(thMessageText);
      thMessageText.CssClass = "message-text content";
      thMessageText.Text = "&nbsp;<br />Message";

      NagTableContainer.Controls.Add(htmlTable);
      return htmlTable;
    }

    #endregion BuildHtmlTable

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Donation Nags and Ballot Choices Request Dialogs Control";
        H1.InnerHtml = "Donation Nags and Ballot Choices Request Dialogs Control";
        SampleBallotControl.SelectedValue =
          DB.Vote.Master.GetPresentGetFutureSampleBallotsDialog(false) ? "T" : "F";
        NagControl.SelectedValue = DonationNagsControl.GetIsNaggingEnabled(false)
          ? "T"
          : "F";
      }

      BuildHtmlTable();
    }

    private void AddButton_Click(object sender, EventArgs e)
    {
      // Adds a new message to the database
      try
      {
        var newMsgNo = GetMsgNoFromForm();
        CheckForDuplicateMsgNo(newMsgNo);
        var nextMsgNo = GetNextMsgFromForm();
        var messageText = GetMsgTextFromForm();

        if (NagFeedback.ValidationErrorCount != 0) return;
        var table = new DonationNagsTable();
        table.AddRow(newMsgNo, null, messageText, nextMsgNo);
        DonationNags.UpdateTable(table);

        var feedback = $"MsgNo {newMsgNo} was successfully added";
        NagFeedback.AddInfo(feedback);

        CommonCacheInvalidation.ScheduleInvalidateNagsAll();
        NagEditMode.Value = Empty;
        BuildHtmlTable();
      }
      catch (Exception ex)
      {
        NagFeedback.AddError("The operation failed due to an unexpected error.");
        NagFeedback.HandleException(ex);
      }
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
      // Handles cancellations -- not much to do
      try
      {
        NagFeedback.AddInfo("The operation was cancelled");
        NagEditMode.Value = Empty;
        BuildHtmlTable();
      }
      catch (Exception ex)
      {
        NagFeedback.AddError("The operation failed due to an unexpected error.");
        NagFeedback.HandleException(ex);
      }
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
      try
      {
        Debug.Assert(sender is Control, "sender is Control");
        var msgNo = GetMsgNoFromId(((Control) sender).ID);
        DonationNags.DeleteByMessageNumber(msgNo);
        var feedback = $"MsgNo {msgNo} was successfully deleted";
        NagFeedback.AddInfo(feedback);
        CommonCacheInvalidation.ScheduleInvalidateNagsAll();
        NagEditMode.Value = Empty;
        BuildHtmlTable();
      }
      catch (Exception ex)
      {
        NagFeedback.AddError("The operation failed due to an unexpected error.");
        NagFeedback.HandleException(ex);
      }
    }

    private void EditButton_Click(object sender, EventArgs e)
    {
      try
      {
        Debug.Assert(sender is Control, "sender is Control");
        var msgNo = GetMsgNoFromId(((Control) sender).ID);
        NagEditMode.Value = "Edit" + msgNo.ToString(CultureInfo.InvariantCulture);
        BuildHtmlTable();
      }
      catch (Exception ex)
      {
        NagFeedback.AddError("The operation failed due to an unexpected error.");
        NagFeedback.HandleException(ex);
      }
    }

    private void InsertButton_Click(object sender, EventArgs e)
    {
      // This method initiates an insert operation.
      // The AddButton_Click method actually does the add.
      try
      {
        NagEditMode.Value = "Insert";
        BuildHtmlTable();
      }
      catch (Exception ex)
      {
        NagFeedback.AddError("The operation failed due to an unexpected error.");
        NagFeedback.HandleException(ex);
      }
    }

    protected void NagControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        string msg;
        if (NagControl.SelectedValue == "T")
        {
          DonationNagsControl.UpdateIsNaggingEnabled(true);
          msg = "The Donation Nag Dialog has been ENABLED.";
        }
        else
        {
          DonationNagsControl.UpdateIsNaggingEnabled(false);
          msg = "The Donation Nag Dialog has been DISABLED.";
        }

        CommonCacheInvalidation.ScheduleInvalidateNagsAll();
        NagControlFeedback.AddInfo(msg);
        NagControlFeedback.AddInfo("It will take effect in 5 to 10 minutes.");
      }
      catch (Exception ex)
      {
        NagControlFeedback.AddError(
          "The operation failed due to an unexpected error.");
        NagControlFeedback.HandleException(ex);
      }
    }

    protected void SampleBallotControl_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      try
      {
        string msg;
        if (SampleBallotControl.SelectedValue == "T")
        {
          DB.Vote.Master.UpdatePresentGetFutureSampleBallotsDialog(true);
          msg =
            "The 'Get Future Ballot Choices Automatically' Dialog has been ENABLED.";
        }
        else
        {
          DB.Vote.Master.UpdatePresentGetFutureSampleBallotsDialog(false);
          msg =
            "The 'Get Future Ballot Choices Automatically' Dialog has been DISABLED.";
        }

        //CommonCacheInvalidation.ScheduleInvalidateBallotAll();
        SampleBallotControlFeedback.AddInfo(msg);
        //SampleBallotControlFeedback.AddInfo(
        //  "Cached Ballot Pages will be cleared in 5 to 10 minutes.");
      }
      catch (Exception ex)
      {
        SampleBallotControlFeedback.AddError(
          "The operation failed due to an unexpected error.");
        SampleBallotControlFeedback.HandleException(ex);
      }
    }

    private void UpdateButton_Click(object sender, EventArgs e)
    {
      try
      {
        // Get the existing record
        Debug.Assert(sender is Control, "sender is Control");
        var msgNo = GetMsgNoFromId(((Control) sender).ID);
        var table = DonationNags.GetDataByMessageNumber(msgNo);

        var newMsgNo = GetMsgNoFromForm();
        if (newMsgNo != msgNo)
          CheckForDuplicateMsgNo(newMsgNo);
        var nextMsgNo = GetNextMsgFromForm();
        var messageText = GetMsgTextFromForm();

        if (NagFeedback.ValidationErrorCount != 0) return;
        table[0].MessageNumber = newMsgNo;
        table[0].NextMessageNumber = nextMsgNo;
        table[0].MessageText = messageText;
        DonationNags.UpdateTable(table);

        var feedback = newMsgNo != msgNo 
          ? $"MsgNo {msgNo} was successfully updated and its MsgNo changed to {newMsgNo}" 
          : $"MsgNo {msgNo} was successfully updated";
        NagFeedback.AddInfo(feedback);

        CommonCacheInvalidation.ScheduleInvalidateNagsAll();
        NagEditMode.Value = Empty;
        BuildHtmlTable();
      }
      catch (Exception ex)
      {
        NagFeedback.AddError("The operation failed due to an unexpected error.");
        NagFeedback.HandleException(ex);
      }
    }

    #endregion Event Handlers
  }
}