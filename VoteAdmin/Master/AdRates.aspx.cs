using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;
using Vote;

namespace VoteAdmin.Master
{
  public partial class AdRatesPage : SecurePage, ISuperUser
  {
    private void BuildAdRatesTable()
    {
      var officeClassesTable = OfficeClasses.GetAllData();

      var htmlTable = InitializeAdRatesTable();

      foreach (var officeClass in officeClassesTable)
      {
        var tr = new TableRow();
        htmlTable.Controls.Add(tr);
        tr.CssClass = "office-class";

        var tdName = new TableCell();
        tr.Controls.Add(tdName);
        tdName.CssClass = "table-name";
        tdName.Text = officeClass.Name;

        var tdLevel = new TableCell();
        tr.Controls.Add(tdLevel);
        tdLevel.CssClass = "table-level table-mainlevel";
        tdLevel.Text = officeClass.OfficeLevel.ToString();

        var tdSubLevel = new TableCell();
        tr.Controls.Add(tdSubLevel);
        tdSubLevel.CssClass = "table-level table-sublevel";
        tdSubLevel.Text = officeClass.AlternateOfficeLevel.ToString(); 

        var tdGeneralRate = new TableCell();
        tr.Controls.Add(tdGeneralRate);
        tdGeneralRate.CssClass = "table-rate table-general-rate";
        var generalRate = officeClass.GeneralAdRate.ToString(CultureInfo.InvariantCulture);
        var tdGeneralRateInput = new HtmlInputText {Value = generalRate};
        tdGeneralRateInput.Attributes.Add("data-original", generalRate);
        tdGeneralRate.Controls.Add(tdGeneralRateInput);

        var tdPrimaryRate = new TableCell();
        tr.Controls.Add(tdPrimaryRate);
        tdPrimaryRate.CssClass = "table-rate table-primary-rate";
        var primaryRate = officeClass.PrimaryAdRate.ToString(CultureInfo.InvariantCulture);
        var tdPrimaryRateInput = new HtmlInputText { Value = primaryRate };
        tdPrimaryRateInput.Attributes.Add("data-original", primaryRate);
        tdPrimaryRate.Controls.Add(tdPrimaryRateInput);
      }

      var extraAdRates = new[]
      {
        new {Name = "Home Page Ad", Type = "H", Rate = DB.Vote.Master.GetHomeAdRate(0)},
        new {Name = "Ballot Page Ad", Type = "B", Rate = DB.Vote.Master.GetBallotAdRate(0)},
        new {Name = "Elected Page Ad", Type = "E", Rate = DB.Vote.Master.GetElectedAdRate(0)},
        new {Name = "Compare Page Ad", Type = "C", Rate = DB.Vote.Master.GetContestAdRate(0)}
      };

      foreach (var adRate in extraAdRates)
      {
        var tr = new TableRow();
        htmlTable.Controls.Add(tr);
        tr.CssClass = "office-class";

        var tdName = new TableCell();
        tr.Controls.Add(tdName);
        tdName.CssClass = "table-name";
        tdName.Text = adRate.Name;
        tdName.Attributes.Add("data-type", adRate.Type);

        new TableCell().AddTo(tr);
        new TableCell().AddTo(tr);

        var rate = new TableCell();
        tr.Controls.Add(rate);
        rate.CssClass = "table-rate table-general-rate";
        var generalRate = adRate.Rate.ToString(CultureInfo.InvariantCulture);
        var tdGeneralRateInput = new HtmlInputText { Value = generalRate };
        tdGeneralRateInput.Attributes.Add("data-original", generalRate);
        rate.Controls.Add(tdGeneralRateInput);

        new TableCell().AddTo(tr);
      }
    }

    private Table InitializeAdRatesTable()
    {
      // Create the table object and add headings
      //
      AdRatesContainer.Controls.Clear();
      var htmlTable = new Table { CellSpacing = 0, CellPadding = 0 };

      var trHeading = new TableHeaderRow();
      htmlTable.Controls.Add(trHeading);

      var thName = new TableHeaderCell();
      trHeading.Controls.Add(thName);
      thName.CssClass = "table-name";
      thName.Text = "Name";

      var thLevel = new TableHeaderCell();
      trHeading.Controls.Add(thLevel);
      thLevel.CssClass = "table-level table-mainlevel";
      thLevel.Text = "Level";

      var thSubLevel = new TableHeaderCell();
      trHeading.Controls.Add(thSubLevel);
      thSubLevel.CssClass = "table-level table-sublevel";
      thSubLevel.Text = "Sub Level";

      var thGeneralRate = new TableHeaderCell();
      trHeading.Controls.Add(thGeneralRate);
      thGeneralRate.CssClass = "table-rate table-general-rate";
      thGeneralRate.Text = "General Rate";

      var thPrimaryRate = new TableHeaderCell();
      trHeading.Controls.Add(thPrimaryRate);
      thPrimaryRate.CssClass = "table-rate table-primary-rate";
      thPrimaryRate.Text = "Primary Rate";

      AdRatesContainer.Controls.Add(htmlTable);
      return htmlTable;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        H1.InnerHtml = Page.Title = "Update Candidate Ad Rates";
        BuildAdRatesTable();
      }
    }

  }
}