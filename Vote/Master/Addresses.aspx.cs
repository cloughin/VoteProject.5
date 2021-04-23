using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Master
{
  public partial class AddressesPage : SecurePage, ISuperUser
  {
    #region from db

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

    #endregion from db

    #region Dynamic controls

    // Object to encapsulate info about each possible output column
    [Serializable]
    private class OutputColumnInfo
    {
      public bool Include;
      public string IdName;
      public string LabelName;
      public string HeadingName;
      // If SqlName is null, indicates special processing needed.
      // Ie, it is not a direct transfer from a DB column.
      public string SqlName;

      private OutputColumnInfo Clone()
      {
        return new OutputColumnInfo
        {
          Include = Include,
          IdName = IdName,
          LabelName = LabelName,
          HeadingName = HeadingName,
          SqlName = SqlName
        };
      }

      // Keep this private -- it should only be accessed via
      // GetDefaultOutputColumnInfo(), which clones the underlying
      // object
      private static readonly OutputColumnInfo[] DefaultOutputColumnInfo =
      {
        new OutputColumnInfo
        {
          Include = false,
          IdName = "Name",
          LabelName = "Name",
          HeadingName = "Name",
          SqlName = null
        },
        new OutputColumnInfo
        {
          Include = true,
          IdName = "FirstName",
          LabelName = "First Name",
          HeadingName = "FirstName",
          SqlName = "FirstName"
        },
        new OutputColumnInfo
        {
          Include = true,
          IdName = "LastName",
          LabelName = "Last Name",
          HeadingName = "LastName",
          SqlName = "LastName"
        },
        new OutputColumnInfo
        {
          Include = true,
          IdName = "Address",
          LabelName = "Address",
          HeadingName = "Address",
          SqlName = "Address"
        },
        new OutputColumnInfo
        {
          Include = true,
          IdName = "City",
          LabelName = "City",
          HeadingName = "City",
          SqlName = "City"
        },
        new OutputColumnInfo
        {
          Include = true,
          IdName = "StateCode",
          LabelName = "State Code",
          HeadingName = "State",
          SqlName = "StateCode"
        },
        new OutputColumnInfo
        {
          Include = true,
          IdName = "ZipPlus4",
          LabelName = "Zip+4",
          HeadingName = "Zip",
          SqlName = null
        },
        new OutputColumnInfo
        {
          Include = false,
          IdName = "Zip5",
          LabelName = "Zip5",
          HeadingName = "Zip5",
          SqlName = "Zip5"
        },
        new OutputColumnInfo
        {
          Include = false,
          IdName = "Zip4",
          LabelName = "Zip4",
          HeadingName = "Zip4",
          SqlName = "Zip4"
        },
        new OutputColumnInfo
        {
          Include = true,
          IdName = "Email",
          LabelName = "Email",
          HeadingName = "Email",
          SqlName = "Email"
        },
        new OutputColumnInfo
        {
          Include = true,
          IdName = "Phone",
          LabelName = "Phone",
          HeadingName = "Phone",
          SqlName = "Phone"
        }
      };

      // Return a clone of the DefaultOutputColumnInfo
      internal static OutputColumnInfo[] GetDefaultOutputColumnInfo()
      {
        var result =
          new OutputColumnInfo[DefaultOutputColumnInfo.Length];
        for (var inx = 0; inx < result.Length; inx++)
          result[inx] = DefaultOutputColumnInfo[inx].Clone();
        return result;
      }
    }

    private OutputColumnInfo[] _CurrentOutputColumnInfo;

    private static string GetStateCheckboxId(string stateCode)
    {
      return "include-" + stateCode;
    }

    private void BuildStatesCheckboxTable()
    {
      const int columnCount = 8; // change this to control layout
      var stateCodes = StateCache.All51StateCodes.ToArray();

      var table = new HtmlTable();
      table.Attributes["class"] = "tableAdmin";
      table.CellSpacing = 0;
      table.CellPadding = 0;

      var rowCount = (stateCodes.Length - 1) / columnCount + 1;
      for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
      {
        var row = new HtmlTableRow();
        table.Rows.Add(row);
        for (var columnIndex = 0; columnIndex < columnCount; columnIndex++)
        {
          var cell = new HtmlTableCell();
          cell.Attributes["class"] = "T";
          row.Cells.Add(cell);
          var stateIndex = columnIndex * rowCount + rowIndex;
          if (stateIndex >= stateCodes.Length)
            cell.InnerHtml = "&nbsp";
          else
          {
            var stateCode = stateCodes[stateIndex];
            var checkbox = new HtmlInputCheckBox {ID = GetStateCheckboxId(stateCode)};
            cell.Controls.Add(checkbox);
            cell.Controls.Add(new LiteralControl(StateCache.GetStateName(stateCodes[stateIndex])));
          }
        }
      }

      StatesPlaceHolder.Controls.Add(table);
    }

    private void SetAllStateCheckboxes(bool value)
    {
      foreach (var stateCode in StateCache.All51StateCodes)
      {
        var id = GetStateCheckboxId(stateCode);
        var checkbox = StatesPlaceHolder.FindControl(id)
          as HtmlInputCheckBox;
        if (checkbox != null)
          checkbox.Checked = value;
      }
    }

    private static string GetColumnHeadingId(string idName)
    {
      return "columnHeading-" + idName;
    }

    private static string GetColumnCheckBoxId(string idName)
    {
      return "columnCheck-" + idName;
    }

    // Add a column to the display of re-orderable output columns
    private void AddColumn(HtmlTable table, bool include, string idName, string labelName,
      string headingName)
    {
      var row = new HtmlTableRow();
      table.Rows.Add(row);

      var cell = new HtmlTableCell();
      cell.Attributes["class"] = "T";
      cell.Align = "center";
      row.Cells.Add(cell);
      var imageButton = new ImageButton
      {
        ID = "columnUp-" + idName,
        ImageUrl = "/images/arrowupsmall.gif"
      };
      imageButton.Click += ColumnReorderButton_Click;
      cell.Controls.Add(imageButton);
      var literal = new LiteralControl("&nbsp;");
      cell.Controls.Add(literal);
      imageButton = new ImageButton
      {
        ID = "columnDown-" + idName,
        ImageUrl = "/images/arrowdownsmall.gif"
      };
      imageButton.Click += ColumnReorderButton_Click;
      cell.Controls.Add(imageButton);

      cell = new HtmlTableCell();
      cell.Attributes["class"] = "T";
      row.Cells.Add(cell);
      var checkbox = new HtmlInputCheckBox {ID = GetColumnCheckBoxId(idName), Checked = include};
      cell.Controls.Add(checkbox);
      literal = new LiteralControl(labelName);
      cell.Controls.Add(literal);

      cell = new HtmlTableCell();
      cell.Attributes["class"] = "T";
      row.Cells.Add(cell);
      var textbox = new HtmlInputText {ID = GetColumnHeadingId(idName), Value = headingName};
      cell.Controls.Add(textbox);
    }

    private void AddColumn(HtmlTable table, OutputColumnInfo columnInfo)
    {
      AddColumn(table, columnInfo.Include, columnInfo.IdName,
        columnInfo.LabelName, columnInfo.HeadingName);
    }

    private void BuildOutputColumnTable()
    {
      var table = new HtmlTable();
      table.Attributes["class"] = "tableAdmin";
      table.CellSpacing = 0;
      table.CellPadding = 0;

      // Create the headings
      var row = new HtmlTableRow();
      table.Rows.Add(row);
      var cell = new HtmlTableCell("th");
      row.Cells.Add(cell);
      cell.InnerText = "Reorder";
      cell.Width = "50";
      cell = new HtmlTableCell("th");
      row.Cells.Add(cell);
      cell.InnerText = "Include Column";
      cell.Align = "left";
      cell.Width = "150";
      cell = new HtmlTableCell("th");
      row.Cells.Add(cell);
      cell.InnerText = "Column Heading";
      cell.Align = "left";

      foreach (var columnInfo in _CurrentOutputColumnInfo)
        AddColumn(table, columnInfo);

      OutputColumnPlaceHolder.Controls.Clear();
      OutputColumnPlaceHolder.Controls.Add(table);
    }

    private void UpdateCurrentOutputColumnInfo()
    {
      // Update the CurrentOutputColumnInfo from the form controls
      foreach (var columnInfo in _CurrentOutputColumnInfo)
      {
        var checkbox = OutputColumnPlaceHolder.FindControl(
          GetColumnCheckBoxId(columnInfo.IdName)) as HtmlInputCheckBox;
        if (checkbox != null)
          columnInfo.Include = checkbox.Checked;
        var textbox = OutputColumnPlaceHolder.FindControl(
          GetColumnHeadingId(columnInfo.IdName)) as HtmlInputText;
        if (textbox != null)
          columnInfo.HeadingName = textbox.Value;
      }
    }

    private static void WriteColumnHeadings(SimpleCsvWriter csvWriter,
      TextWriter stringWriter, IEnumerable<OutputColumnInfo> outputColumns)
    {
      // In case the CSV required column headings
      foreach (var column in outputColumns)
        csvWriter.AddField(column.HeadingName);
      csvWriter.Write(stringWriter);
    }

    private static void WriteRow(SimpleCsvWriter csvWriter,
      TextWriter stringWriter, IEnumerable<OutputColumnInfo> outputColumns,
      IDataRecord dataReader)
    {
      // Write a single row of data to the memory-resident CSV
      foreach (var column in outputColumns)
      {
        if (column.SqlName == null) // a special (derived) column
        {
          switch (column.IdName)
          {
            case "Name":
              var firstName = dataReader["FirstName"] as string;
              var lastName = dataReader["LastName"] as string;
              if (string.IsNullOrWhiteSpace(firstName))
                csvWriter.AddField(string.IsNullOrWhiteSpace(lastName)
                  ? string.Empty
                  : lastName);
              else if (string.IsNullOrWhiteSpace(lastName))
                csvWriter.AddField(lastName);
              else
                csvWriter.AddField(firstName + ' ' + lastName);
              break;

            case "ZipPlus4":
              var zip5 = dataReader["Zip5"] as string;
              var zip4 = dataReader["Zip4"] as string;
              if (string.IsNullOrWhiteSpace(zip5))
                csvWriter.AddField(string.Empty);
              else if (string.IsNullOrWhiteSpace(zip4))
                csvWriter.AddField(zip5);
              else
                csvWriter.AddField(zip5 + '-' + zip4);
              break;
          }
        }
        else
          csvWriter.AddField(dataReader[column.SqlName] as string);
      }
      csvWriter.Write(stringWriter);
    }

    #endregion Dynamic controls

    #region Control parsing and editing

    private List<string> GetDistrictCodes(int districtCodeLength)
    {
      // Return a list of district codes as strings, properly zero padded
      List<string> districtCodes = null;
      var districtCodeInput = LegislativeTextBox.Text.Trim();
      if (districtCodeInput != string.Empty)
        districtCodes = districtCodeInput.Split(',')
          .Select(code => code.Trim().ZeroPad(districtCodeLength))
          .ToList();
      return districtCodes;
    }

    private List<OutputColumnInfo> GetSelectedOutputColumns()
    {
      // Return a list of currently-selected output columns
      return _CurrentOutputColumnInfo
        .Where(ci => ci.Include)
        .ToList();
    }

    private List<string> GetSelectedStates()
    {
      // Return a list of currently-selected state codes
      var selectedStates = new List<string>();
      foreach (var stateCode in StateCache.All51StateCodes)
      {
        var id = GetStateCheckboxId(stateCode);
        var checkbox = StatesPlaceHolder.FindControl(id)
          as HtmlInputCheckBox;
        if (checkbox?.Checked == true)
          selectedStates.Add(stateCode);
      }
      return selectedStates;
    }

    #endregion Control parsing and editing

    #region Event handlers

    private void ColumnReorderButton_Click(object sender, ImageClickEventArgs e)
    {
      var imageButton = sender as ImageButton;
      if (imageButton != null)
      {
        // Parse the direction and IdName from the control ID
        // ID format: columnUp-<IdName> or columnDown-<IdName>
        var match = Regex.Match(imageButton.ID,
          "^column(?<direction>Up|Down)-(?<idName>.+)$");
        if (match.Success)
        {
          var direction = match.Groups["direction"].Captures[0].Value;
          var idName = match.Groups["idName"].Captures[0].Value;
          var inx = Array.FindIndex(_CurrentOutputColumnInfo, ci => ci.IdName == idName);
          if (inx >= 0)
            switch (direction)
            {
              case "Up":
                if (inx > 0)
                  Array.Reverse(_CurrentOutputColumnInfo, inx - 1, 2);
                break;

              case "Down":
                if (inx < _CurrentOutputColumnInfo.Length - 1)
                  Array.Reverse(_CurrentOutputColumnInfo, inx, 2);
                break;
            }
        }
      }
    }

    protected void ClearAllStatesButton_Click(object sender, EventArgs e)
    {
      SetAllStateCheckboxes(false);
    }

    protected void DownloadButton_Click(object sender, EventArgs e)
    {
      try
      {
        var selectedStates = GetSelectedStates();
        if (selectedStates.Count == 0)
          throw new VoteException("No states were selected");

        // If all 51 states are selected, we null the list to indicate no
        // state selection is needed
        if (selectedStates.Count == 51) selectedStates = null;

        // A sensibility check
        if (EmailFilterCheckBox.Checked && NoEmailFilterCheckBox.Checked)
          throw new VoteException("Both the 'with' and 'without' email filters are checked");

        // Parse the districts
        var districtColumnName = LegislativeFilterDropDownList.SelectedValue;
        var districtCodeLength = 3;
        if (districtColumnName == "CD")
          districtCodeLength = 2;
        var districtCodes = GetDistrictCodes(districtCodeLength);
        if (LegislativeFilterCheckBox.Checked && (districtCodes == null))
          throw new VoteException("The legislative filter was checked but no districts were entered");

        // Get the dates
        var fromDate = DateTime.MinValue.Date;
        var toDate = DateTime.MaxValue.Date;
        var fromDateString = FromDateTextBox.Text.Trim();
        var toDateString = ToDateTextBox.Text.Trim();
        if (fromDateString != string.Empty)
          if (!DateTime.TryParse(fromDateString, out fromDate))
            throw new VoteException("Invalid 'from' date");
        if (toDateString != string.Empty)
          if (!DateTime.TryParse(toDateString, out toDate))
            throw new VoteException("Invalid 'to' date");
        fromDate = fromDate.Date;
        toDate = toDate.Date;
        if (fromDate > toDate)
          throw new VoteException("The 'from' date exceeds the 'to' date");

        // Get the selected columns
        UpdateCurrentOutputColumnInfo();
        var outputColumns = GetSelectedOutputColumns();
        if (outputColumns.Count == 0)
          throw new VoteException("No output columns were selected");

        // Set up the CSV objects
        var stringWriter = new StringWriter();
        var csvWriter = new SimpleCsvWriter();
        if (OutputHeadingCheckBox.Checked) // column headings requested
          WriteColumnHeadings(csvWriter, stringWriter, outputColumns);

        // If the LegislativeFilterCheckBox is checked, we use the
        // DistrictAddressesView. Otherwise we use the Addresses table.
        if (LegislativeFilterCheckBox.Checked)
        {
          using (var reader =
            DistrictAddressesView.GetDataReaderForAddressExtraction(
              selectedStates, NameFilterCheckBox.Checked,
              AddressFilterCheckBox.Checked, EmailFilterCheckBox.Checked,
              NoEmailFilterCheckBox.Checked, PhoneFilterCheckBox.Checked,
              fromDate, toDate, districtColumnName, districtCodes, 0))
            while (reader.Read())
              WriteRow(csvWriter, stringWriter, outputColumns, reader.DataReader);
        }
        else
        {
          using (var reader =
            Addresses.GetDataReaderForAddressExtraction(
              selectedStates, NameFilterCheckBox.Checked,
              AddressFilterCheckBox.Checked, EmailFilterCheckBox.Checked,
              NoEmailFilterCheckBox.Checked, PhoneFilterCheckBox.Checked,
              fromDate, toDate, 0))
            while (reader.Read())
              WriteRow(csvWriter, stringWriter, outputColumns, reader.DataReader);
        }

        // Send the results back as a download
        var memoryStream = new MemoryStream();
        var data = Encoding.UTF8.GetBytes(stringWriter.ToString());
        memoryStream.Write(data, 0, data.Length);
        memoryStream.Position = 0;

        Response.Clear();
        Response.AddHeader("Content-Disposition", "attachment; filename=addresses.csv");
        Response.AddHeader("Content-Length", data.Length.ToString(CultureInfo.InvariantCulture));
        Response.ContentType = "application/octet-stream";
        Response.Charset = "UTF-8";

        using (memoryStream)
        using (var reader = new BinaryReader(memoryStream))
          Response.BinaryWrite(reader.ReadBytes(data.Length));
      }
      catch (Exception ex)
      {
        Feedback.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void UpdateAddressesFromLogButton_Click(object sender, EventArgs e)
    {
      try
      {
        var totalCount = 0;
        var transferredCount = 0;
        var insufficientContentCount = 0;
        var duplicateCount = 0;
        var done = false; // error recovery retry
        while (!done)
          using (var reader = LogAddressesGoodNew.GetDataReaderByNotTransferredToAddresses(0))
          {
            try
            {
              while (reader.Read())
              {
                var address = reader.ParsedAddress.SafeString().Trim().ToUpperInvariant();
                var city = reader.ParsedCity.SafeString().Trim().ToUpperInvariant();
                var state = reader.ParsedStateCode.SafeString().Trim().ToUpperInvariant();
                var zip5 = reader.ParsedZip5.SafeString().Trim().ToUpperInvariant();
                var zip4 = reader.ParsedZip4.SafeString().Trim().ToUpperInvariant();
                var email = reader.Email.SafeString().Trim();
                totalCount++;
                // We need email, address, city, state, at a minimum
                if (email != string.Empty /*&& address != string.Empty && city != string.Empty &&
                  state != string.Empty*/)
                {
                  //var matchCount = Addresses.CountByEmailAddressCityStateCodeZip5Zip4(
                  //  email, address, city, state, zip5, zip4);
                  var matchCount = Addresses.EmailExists(email) ? 1 : 0;
                  if (matchCount == 0) // it's new
                  {
                    transferredCount++;
                    //FOR CURT
                    Addresses.Insert(string.Empty, string.Empty, address, city, state, zip5, zip4,
                      email, string.Empty, reader.DateStamp.Date, "LOG", false,
                      // since these are log entries, any email address 
                      // indicates an opt-in for sample ballots
                      email != string.Empty, false, VoteDb.DateTimeMin, string.Empty, string.Empty,
                      string.Empty, string.Empty, string.Empty, VoteDb.DateTimeMin, 0, DefaultDbDate,
                      false);
                  }
                  else
                  {
                    Addresses.UpdateSendSampleBallotsByEmail(true, email);
                    duplicateCount++;
                  }
                }
                else insufficientContentCount++;
                LogAddressesGoodNew.UpdateTransferredToAddressesById(true, reader.Id);
              }
              done = true; // no more retries needed
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
          }
        Feedback.Text = Ok("<br />" + 
          $"{totalCount} log rows read<br />" +
          $"{transferredCount} addresses transferred<br />" +
          $"{insufficientContentCount} addresses had insufficient information<br />" +
          $"{duplicateCount} addresses were exact duplicates");
      }
      catch (Exception ex)
      {
        Feedback.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void SelectAllStatesButton_Click(object sender, EventArgs e)
    {
      SetAllStateCheckboxes(true);
    }

    private void Page_Load(object sender, EventArgs e)
    {
      Feedback.Text = string.Empty;
      if (IsPostBack)
      {
        _CurrentOutputColumnInfo = ViewState["OutputColumnInfo"] as OutputColumnInfo[];
      }
      else
      {
        _CurrentOutputColumnInfo = OutputColumnInfo.GetDefaultOutputColumnInfo();
        if (!IsMasterUser)
          HandleSecurityException();

        Title = H1.InnerText = "Addresses";
      }
      BuildStatesCheckboxTable();
      BuildOutputColumnTable();
    }

    protected override void OnLoadComplete(EventArgs e)
    {
      base.OnLoadComplete(e);

      // Save dynamic column info in ViewState
      UpdateCurrentOutputColumnInfo();
      ViewState["OutputColumnInfo"] = _CurrentOutputColumnInfo;
      BuildOutputColumnTable();
    }

    #endregion Event handlers
  }
}