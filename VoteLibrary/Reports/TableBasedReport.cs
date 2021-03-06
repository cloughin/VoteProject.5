using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public class TableBasedReport : Report
  {
    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    #endregion ReSharper disable

    //protected const int ImageSize100 = 100;
    protected const int ImageSize75 = 75;

    protected readonly PlaceHolder ReportContainer = new PlaceHolder();
    protected HtmlTable CurrentHtmlTable;
    protected HtmlTableRow CurrentPoliticianRow;

    protected void CreateCategoryHeading(string title)
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportHeading");
      new HtmlTableCell {Align = "center", InnerHtml = title}.AddTo(tr,
        "tdReportHeading");
    }

    protected HtmlTable CreateInitializedHtmlTable()
    {
      return
        new HtmlTable {CellSpacing = 0, CellPadding = 0, Border = 0}.AddCssClasses(
          ReportUser == ReportUser.Public ? "tablePage" : "tableAdmin");
    }

    protected void ReportMissingPolitician()
    {
      var td = new HtmlTableCell().AddTo(CurrentPoliticianRow, "tdReportImage");
      CreatePoliticianImageAnchor(UrlManager.GetIntroPageUri("NoPhoto")
          .ToString(), "NoPhoto", ImageSize100, "No Photo")
        .AddTo(td);
      td = new HtmlTableCell().AddTo(CurrentPoliticianRow, "tdReportDetail");
      new HtmlStrong {InnerHtml = "Vacant or Not Identified"}
        .AddTo(td);
    }

    protected void ReportPolitician(DataRow politician, bool isWinner,
      bool isIncumbent)
    {
      var politicianKey = politician.PoliticianKey();
      var politicianName = Politicians.FormatName(politician);

      Control anchorHeadshot = null;
      switch (ReportUser)
      {
        case ReportUser.Public:
        {
          anchorHeadshot =
            CreatePoliticianImageAnchor(UrlManager.GetIntroPageUri(politicianKey)
                .ToString(), politicianKey, ImageSize100,
              politicianName +
              " biographical information and positions and views on the issues");
          break;
        }

        case ReportUser.Admin:
        {
          anchorHeadshot = new HtmlImage
          {
            Src = VotePage.GetPoliticianImageUrl(politicianKey, ImageSize75)
          };
          break;
        }

        case ReportUser.Master:
        {
          anchorHeadshot =
            CreatePoliticianImageAnchor(
              SecurePoliticianPage.GetUpdateIssuesPageUrl(politicianKey),
              politicianKey, ImageSize75,
               "Edit Issue Topic Responses", "politician");
          break;
        }
      }

      var td = new HtmlTableCell().AddTo(CurrentPoliticianRow, "tdReportImage");
      Debug.Assert(anchorHeadshot != null, "anchorHeadshot != null");
      anchorHeadshot.AddTo(td);

      var politicianCell = new HtmlTableCell().AddTo(CurrentPoliticianRow,
        "tdReportDetail");

      var nameContainer = new HtmlDiv().AddTo(politicianCell,
        "detail name");

      if (isIncumbent)
        new Literal {Text = "* "}.AddTo(nameContainer);

      FormatNameAndPartyTable(politician)
        .AddTo(nameContainer);

      if (isWinner)
      {
        new HtmlBreak().AddTo(nameContainer);
        var strong = new HtmlStrong().AddTo(nameContainer);
        var span =
          new HtmlSpan {InnerHtml = "(winner)"}.AddTo(strong);
        span.Style.Add(HtmlTextWriterStyle.Color, "red");
      }

      var website = FormatPoliticianWebsiteTable(politician, 0);
      var isEmptyWebsite = website as Literal;
      if (IsNullOrWhiteSpace(isEmptyWebsite?.Text))
      {
        var websiteContainer = new HtmlDiv().AddTo(politicianCell,
          "detail website");
        website.AddTo(websiteContainer);
      }

      var socalMediaAnchors = SocialMedia.GetAnchors(politician);
      if (socalMediaAnchors.Controls.Count > 0)
        socalMediaAnchors.AddTo(politicianCell, "detail social");

      var addressLines = new List<string>();
      var streetAddress = politician.PublicAddress();
      var cityStateZip = politician.PublicCityStateZip();
      if (!IsNullOrEmpty(streetAddress) &&
        !IsNullOrEmpty(cityStateZip))
      {
        addressLines.Add(streetAddress);
        addressLines.Add(cityStateZip);
      }
      else addressLines.Add("no address");

      var addressContainer = new HtmlDiv().AddTo(politicianCell,
        "detail address");

      foreach (var line in addressLines)
      {
        var p = new HtmlP().AddTo(addressContainer, "TAddress");
        new Literal {Text = line}.AddTo(p);
      }

      string phone = null;
      switch (ReportUser)
      {
        case ReportUser.Public:
        {
          phone = politician.PublicPhone();
          break;
        }

        case ReportUser.Admin:
        {
          phone = politician.StatePhone();
          if (IsNullOrEmpty(phone)) phone = "no phone";
          break;
        }

        case ReportUser.Master:
        {
          phone = politician.PublicPhone();
          if (IsNullOrEmpty(phone)) phone = "no phone";
          break;
        }
      }

      if (!IsNullOrEmpty(phone))
      {
        var phoneContainer = new HtmlDiv().AddTo(politicianCell,
          "detail phone");
        new HtmlP {InnerHtml = phone}.AddTo(phoneContainer,
          "TPhone");
      }

      var age = politician.Age();
      if (IsNullOrEmpty(age)) return;
      var ageContainer = new HtmlDiv().AddTo(politicianCell,
        "detail age");
      new HtmlP {InnerHtml = age}.AddTo(ageContainer, "TAge");
    }

    protected void StartNewHtmlTable()
    {
      CurrentHtmlTable = CreateInitializedHtmlTable()
        .AddTo(ReportContainer);
    }

    protected sealed class OfficialsSort : ReportDataManager<DataRow>.OrderBy
    {
      public override int Compare(DataRow row1, DataRow row2)
      {
        var result = row1.OfficeLevel()
          .CompareTo(row2.OfficeLevel());
        if (result != 0) return result;
        result = string.Compare(row1.DistrictCode(), row2.DistrictCode(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = row1.OfficeOrderWithinLevel()
          .CompareTo(row2.OfficeOrderWithinLevel());
        if (result != 0) return result;
        result = string.Compare(row1.OfficeLine1(), row2.OfficeLine1(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = string.Compare(row1.AddOn(), row2.AddOn(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        return string.Compare(row1.LastName(), row2.LastName(),
          StringComparison.OrdinalIgnoreCase);
      }
    }

    #region ReSharper restore

    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected
  }
}