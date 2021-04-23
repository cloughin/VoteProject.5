using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Text.RegularExpressions;
using DB.Vote;
using System.Web.UI.WebControls;
using DB.VoteLog;
using System.IO;
using System.Data.Common;
using System.Text;
using MySql.Data.MySqlClient;

namespace Vote.Master
{
  public partial class EmailsPage : System.Web.UI.Page
  {
    //#region Dynamic controls

    //private void BuildStatesCheckboxTable()
    //{
    //  int columnCount = 8; // change this to control layout
    //  string[] stateCodes = StateCache.All51StateCodes.ToArray();

    //  HtmlTable table = new HtmlTable();
    //  table.Attributes["class"] = "tableAdmin";
    //  table.CellSpacing = 0;
    //  table.CellPadding = 0;

    //  int rowCount = (stateCodes.Length - 1) / columnCount + 1;
    //  for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
    //  {
    //    HtmlTableRow row = new HtmlTableRow();
    //    table.Rows.Add(row);
    //    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
    //    {
    //      HtmlTableCell cell = new HtmlTableCell();
    //      cell.Attributes["class"] = "T";
    //      row.Cells.Add(cell);
    //      int stateIndex = columnIndex * rowCount + rowIndex;
    //      if (stateIndex >= stateCodes.Length)
    //        cell.InnerHtml = "&nbsp";
    //      else
    //      {
    //        string stateCode = stateCodes[stateIndex];
    //        HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
    //        checkbox.ID = GetStateCheckboxId(stateCode);
    //        cell.Controls.Add(checkbox);
    //        cell.Controls.Add(new LiteralControl(StateCache.GetStateName(stateCodes[stateIndex])));
    //      }
    //    }
    //  }

    //  StatesPlaceHolder.Controls.Add(table);
    //}

    //private void ClearBatchStatistics()
    //{
    //  EmailsInBatchLabel.Text = "Click Refresh to see statistics";
    //  SuccessfullySentLabel.Text = string.Empty;
    //  RejectedLabel.Text = string.Empty;
    //  ToBeSentLabel.Text = string.Empty;
    //}

    //private void ClearSelectionCriteria()
    //{
    //  SetAllStateCheckboxes(false);
    //  LegislativeFilterCheckBox.Checked = false;
    //  LegislativeFilterDropDownList.SelectedIndex = 0;
    //  LegislativeTextBox.Text = string.Empty;
    //  WithNamesCheckBox.Checked = false;
    //  WithoutNamesCheckBox.Checked = false;
    //  WithAddressesCheckBox.Checked = false;
    //  WithoutAddressesCheckBox.Checked = false;
    //  KnownDistrictsCheckBox.Checked = false;
    //  UnknownDistrictsCheckBox.Checked = false;
    //  FromDateTextBox.Text = string.Empty;
    //  ToDateTextBox.Text = string.Empty;
    //}

    //private string GetStateCheckboxId(string stateCode)
    //{
    //  return "include-" + stateCode;
    //}

    //private void InitializeBatchNameDropDownList(int id)
    //{
    //  string selectedValue = string.Empty;
    //  if (id >= 0)
    //    selectedValue = id.ToString();
    //  InitializeBatchNameDropDownList(selectedValue);
    //}

    //private void InitializeBatchNameDropDownList(string selectedValue)
    //{
    //  bool createMode = selectedValue == string.Empty;
    //  BatchNameDropDownList.Items.Clear();
    //  BatchNameDropDownList.AddItem("-- Create New Batch --", string.Empty,
    //    createMode);
    //  var table = EmailBatches.GetOpenBatchesData();
    //  foreach (var row in table)
    //  {
    //    string value = row.Id.ToString();
    //    BatchNameDropDownList.AddItem(row.Name, row.Id.ToString(),
    //      selectedValue == value);
    //  }
    //}

    //private void LoadBatchStatistics(int id)
    //{
    //  int emailsInBatch = 0;
    //  int successfullySent = 0;
    //  int rejected = 0;
    //  int toBeSent = 0;
    //  using (var reader = EmailQueue.GetDataReaderByEmailBatchId(id, 0))
    //    while (reader.Read())
    //    {
    //      emailsInBatch++;
    //      if (reader.SentTime == null)
    //        toBeSent++;
    //      else if (reader.Rejected)
    //        rejected++;
    //      else
    //        successfullySent++;
    //    }
    //  EmailsInBatchLabel.Text = emailsInBatch.ToString();
    //  SuccessfullySentLabel.Text = successfullySent.ToString();
    //  RejectedLabel.Text = rejected.ToString();
    //  ToBeSentLabel.Text = toBeSent.ToString();
    //}

    //private void LoadBatchData(int id)
    //{
    //  var row = EmailBatches.GetDataById(id)[0];
    //  BatchNameTextBox.Text = row.Name;
    //  BatchDescriptionTextBox.Text = row.Description;
    //  BatchFromAddressTextBox.Text = row.FromAddress;
    //  BatchSubjectTextBox.Text = row.Subject;
    //  BatchTemplateTextBox.Text = row.Template;
    //  BatchClosedCheckBox.Checked = row.IsClosed;
    //  BatchCreationTimeLabel.Text = row.CreationTime.ToString();
    //  ClearBatchStatistics();
    //  ClearSelectionCriteria();
    //}

    //private int SelectedBatchId
    //{
    //  get
    //  {
    //    string selectedValue = BatchNameDropDownList.SelectedValue.SafeString();
    //    int result;
    //    if (int.TryParse(selectedValue, out result))
    //      return result;
    //    else
    //      return -1;
    //  }
    //}

    //private void SetAllStateCheckboxes(bool value)
    //{
    //  foreach (var stateCode in StateCache.All51StateCodes)
    //  {
    //    string id = GetStateCheckboxId(stateCode);
    //    HtmlInputCheckBox checkbox = StatesPlaceHolder.FindControl(id)
    //      as HtmlInputCheckBox;
    //    if (checkbox != null)
    //      checkbox.Checked = value;
    //  }
    //}

    //#endregion Dynamic controls

    //#region Control parsing and editing

    //private List<string> GetDistrictCodes(int districtCodeLength)
    //{
    //  // Return a list of district codes as strings, properly zero padded
    //  List<string> districtCodes = null;
    //  string districtCodeInput = LegislativeTextBox.Text.Trim();
    //  if (districtCodeInput != string.Empty)
    //    districtCodes = districtCodeInput.Split(',')
    //      .Select(code => code.Trim().ZeroPad(districtCodeLength))
    //      .ToList();
    //  return districtCodes;
    //}

    //private List<string> GetSelectedStates()
    //{
    //  // Return a list of currently-selected state codes
    //  var selectedStates = new List<string>();
    //  foreach (string stateCode in StateCache.All51StateCodes)
    //  {
    //    string id = GetStateCheckboxId(stateCode);
    //    HtmlInputCheckBox checkbox = StatesPlaceHolder.FindControl(id)
    //      as HtmlInputCheckBox;
    //    if (checkbox != null && checkbox.Checked)
    //      selectedStates.Add(stateCode);
    //  }
    //  return selectedStates;
    //}

    //#endregion Control parsing and editing

    //#region Event handlers

    //protected void AddEmailsButton_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    List<string> selectedStates = GetSelectedStates();
    //    if (selectedStates.Count == 0)
    //      throw new VoteException("No states were selected");

    //    // If all 51 states are selected, we null the list to indicate no
    //    // state selection is needed
    //    if (selectedStates.Count == 51) selectedStates = null;

    //    // Sensibility checks
    //    if (WithNamesCheckBox.Checked && WithoutNamesCheckBox.Checked)
    //      throw new VoteException("Both the 'with' and 'without' names filters are checked");
    //    if (WithAddressesCheckBox.Checked && WithoutAddressesCheckBox.Checked)
    //      throw new VoteException("Both the 'with' and 'without' addresses filters are checked");
    //    if (AppendedEmailsCheckBox.Checked && EnteredEmailsCheckBox.Checked)
    //      throw new VoteException("Both the 'appended' and 'entered' email filters are checked");
    //    if (KnownDistrictsCheckBox.Checked && UnknownDistrictsCheckBox.Checked)
    //      throw new VoteException("Both the 'known' and 'unknown' districts filters are checked");

    //    // Parse the districts
    //    string districtColumnName = LegislativeFilterDropDownList.SelectedValue;
    //    int districtCodeLength = 3;
    //    if (districtColumnName == "CongressionalDistrict")
    //      districtCodeLength = 2;
    //    List<string> districtCodes = GetDistrictCodes(districtCodeLength);
    //    if (LegislativeFilterCheckBox.Checked && districtCodes == null)
    //      throw new VoteException("The legislative filter was checked but no districts were entered");

    //    // Get the dates
    //    DateTime fromDate = DateTime.MinValue.Date;
    //    DateTime toDate = DateTime.MaxValue.Date;
    //    string fromDateString = FromDateTextBox.Text.Trim();
    //    string toDateString = ToDateTextBox.Text.Trim();
    //    if (fromDateString != string.Empty)
    //      if (!DateTime.TryParse(fromDateString, out fromDate))
    //        throw new VoteException("Invalid 'from' date");
    //    if (toDateString != string.Empty)
    //      if (!DateTime.TryParse(toDateString, out toDate))
    //        throw new VoteException("Invalid 'to' date");
    //    fromDate = fromDate.Date;
    //    toDate = toDate.Date;
    //    if (fromDate > toDate)
    //      throw new VoteException("The 'from' date exceeds the 'to' date");

    //    int batchId = SelectedBatchId;

    //    int addressesFound = 0;
    //    int invalid = 0;
    //    int duplicates = 0;
    //    int addressesWritten = 0;
    //    using (var reader =
    //      Addresses.GetDataReaderForEmailBatches(selectedStates,
    //       WithNamesCheckBox.Checked, WithoutNamesCheckBox.Checked,
    //       WithAddressesCheckBox.Checked, WithoutAddressesCheckBox.Checked,
    //       AppendedEmailsCheckBox.Checked, EnteredEmailsCheckBox.Checked,
    //       KnownDistrictsCheckBox.Checked, UnknownDistrictsCheckBox.Checked,
    //       fromDate, toDate, districtColumnName, districtCodes, 0))
    //      while (reader.Read())
    //      {
    //        addressesFound++;
    //        if (!Validation.IsValidEmailAddress(reader.Email))
    //          invalid++;
    //        else if (EmailQueue.EmailBatchIdToAddressExists(batchId, reader.Email))
    //          duplicates++;
    //        else
    //        {
    //          EmailQueue.Insert(
    //            batchId,
    //            reader.Email,
    //            null,
    //            false,
    //            string.Empty,
    //            reader.FirstName,
    //            reader.LastName,
    //            reader.Address,
    //            reader.City,
    //            reader.StateCode,
    //            reader.Zip5,
    //            reader.Zip4,
    //            reader.CongressionalDistrict,
    //            reader.StateSenateDistrict,
    //            reader.StateHouseDistrict,
    //            reader.County);
    //          addressesWritten++;
    //        }
    //      }

    //    ClearBatchStatistics();

    //    Feedback.Text = db.Ok(string.Format("<br />" +
    //      "{0} qualifying addresses found<br />" +
    //      "{1} invalid email addresses<br />" +
    //      "{2} duplicates<br />" +
    //      "{3} addresses written",
    //      addressesFound,
    //      invalid,
    //      duplicates,
    //      addressesWritten));
    //  }
    //  catch (Exception ex)
    //  {
    //    Feedback.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void BatchNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  int id = SelectedBatchId;
    //  bool createMode = id < 0;
    //  NewBatch.Visible = createMode;
    //  ExistingBatch.Visible = !createMode;
    //  if (createMode)
    //    NewBatchNameTextBox.Text = string.Empty;
    //  else
    //    LoadBatchData(id);
    //}

    //protected void ClearAllStatesButton_Click(object sender, EventArgs e)
    //{
    //  SetAllStateCheckboxes(false);
    //}

    //protected void CreateBatchButton_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string newName = NewBatchNameTextBox.Text.Trim();
    //    if (string.IsNullOrWhiteSpace(newName))
    //      throw new VoteException("Batch name is required");
    //    if (EmailBatches.NameExists(newName))
    //      throw new VoteException("Name '{0}' already exists", newName);
    //    int id = EmailBatches.Insert(
    //      newName,
    //      string.Empty,
    //      string.Empty,
    //      string.Empty,
    //      string.Empty,
    //      DateTime.Now,
    //      false);
    //    NewBatch.Visible = false;
    //    ExistingBatch.Visible = true;
    //    InitializeBatchNameDropDownList(id);
    //    LoadBatchData(id);
    //    db.Ok("Batch created");
    //  }
    //  catch (Exception ex)
    //  {
    //    Feedback.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RefreshButton_Click(object sender, EventArgs e)
    //{
    //  LoadBatchStatistics(SelectedBatchId);
    //}

    //protected void SelectAllStatesButton_Click(object sender, EventArgs e)
    //{
    //  SetAllStateCheckboxes(true);
    //}

    //protected void UpdateBatchButton_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    int id = SelectedBatchId;
    //    string oldName = EmailBatches.GetNameById(id);
    //    string newName = BatchNameTextBox.Text.Trim();
    //    if (oldName != newName)
    //    {
    //      if (string.IsNullOrWhiteSpace(newName))
    //        throw new VoteException("Batch name is required");
    //      if (EmailBatches.NameExists(newName))
    //        throw new VoteException("Name '{0}' already exists", newName);
    //      EmailBatches.UpdateNameById(newName, id);
    //    }
    //    EmailBatches.UpdateDescriptionById(BatchDescriptionTextBox.Text, id);
    //    EmailBatches.UpdateFromAddressById(BatchFromAddressTextBox.Text, id);
    //    EmailBatches.UpdateSubjectById(BatchSubjectTextBox.Text, id);
    //    EmailBatches.UpdateTemplateById(BatchTemplateTextBox.Text, id);
    //    EmailBatches.UpdateIsClosedById(BatchClosedCheckBox.Checked, id);
    //    if (BatchClosedCheckBox.Checked)
    //    {
    //      id = -1;
    //      NewBatchNameTextBox.Text = string.Empty; 
    //      NewBatch.Visible = true;
    //      ExistingBatch.Visible = false;
    //    }
    //    if (oldName != newName || BatchClosedCheckBox.Checked)
    //      InitializeBatchNameDropDownList(id);
    //    Feedback.Text = db.Ok("Batch updated");
    //  }
    //  catch (Exception ex)
    //  {
    //    Feedback.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  Feedback.Text = string.Empty;
    //  if (!IsPostBack)
    //  {
    //    InitializeBatchNameDropDownList(string.Empty);
    //    NewBatchNameTextBox.Text = string.Empty; 
    //    NewBatch.Visible = true;
    //    ExistingBatch.Visible = false;
    //    if (!SecurePage.IsMasterUser)
    //      SecurePage.HandleSecurityException();
    //  }
    //  BuildStatesCheckboxTable();
    //}

    //#endregion Event handlers
  }
}