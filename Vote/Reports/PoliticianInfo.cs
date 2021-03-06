using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class PoliticianInfo : Report
  {
    private DataRow _PoliticianInfo;

    private Control GenerateReport(DataRow politiciaInfo)
    {
      _PoliticianInfo = politiciaInfo;

      var mainTable =
        new HtmlTable {CellSpacing = 0, CellPadding = 0}.AddCssClasses("tablePage");

      var tr = new HtmlTableRow().AddTo(mainTable, "trPoliticianInfoTop");
      var td = new HtmlTableCell().AddTo(tr, "tdPoliticianInfoBio");
      CreateTitleTable()
        .AddTo(td);
      CreateBioTable()
        .AddTo(td);

      td = new HtmlTableCell().AddTo(tr, "tdPoliticianInfoImage");
      new HtmlImage
        {
          Src = VotePage.GetPoliticianImageUrl(_PoliticianInfo.PoliticianKey(), 300)
        }
        .AddTo(td);

      return mainTable;
    }

    private HtmlTable CreateTitleTable()
    {
      var htmlTable = new HtmlTable {CellSpacing = 0, CellPadding = 0, Border = 0};

      var tr = new HtmlTableRow().AddTo(htmlTable,
        "trPoliticianInfoPoliticianTitle");
      new HtmlTableCell
        {
          ColSpan = 2,
          InnerHtml = Politicians.FormatOfficeAndStatus(_PoliticianInfo)
        }.AddTo(tr,
          "tdPoliticianInfoPoliticianTitle tdOfficeName");

      tr = new HtmlTableRow().AddTo(htmlTable, "trPoliticianInfoPoliticianTitle");
      new HtmlTableCell {ColSpan = 2, InnerHtml = GetFutureElectionDescription()}
        .AddTo(tr, "tdPoliticianInfoPoliticianTitle tdElectionName");

      return htmlTable;
    }

    private string GetFutureElectionDescription()
    {
      return _PoliticianInfo.LivePoliticianStatus()
        .IsInFutureViewableElection()
        ? _PoliticianInfo.ElectionDescription()
        : string.Empty;
    }

    private HtmlTable CreateBioTable()
    {
      var htmlTable =
        new HtmlTable {CellSpacing = 0, CellPadding = 0, Border = 0}.AddCssClasses(
          "bioTable");

      // Age
      //
      var tr = new HtmlTableRow().AddTo(htmlTable,
        "trPoliticianInfoContact trPoliticianInfoContactAge");
      var td = new HtmlTableCell().AddTo(tr, "tdPoliticianInfoContactHeading");
      new HtmlSpan {InnerHtml = "Age:"}.AddTo(td);
      new HtmlTableCell {InnerHtml = _PoliticianInfo.Age()}.AddTo(tr,
        "tdPoliticianInfoContactDetail");

      // Party
      //
      tr = new HtmlTableRow().AddTo(htmlTable, "trPoliticianInfoContact");
      td = new HtmlTableCell().AddTo(tr, "tdPoliticianInfoContactHeading");
      new HtmlSpan {InnerHtml = "Party:"}.AddTo(td);
      td = new HtmlTableCell().AddTo(tr, "tdPoliticianInfoContactDetail");
      CreatePartyAnchor()
        .AddTo(td);

      // Phone
      //
      tr = new HtmlTableRow().AddTo(htmlTable, "trPoliticianInfoContact");
      td = new HtmlTableCell().AddTo(tr, "tdPoliticianInfoContactHeading");
      new HtmlSpan {InnerHtml = "Phone:"}.AddTo(td);
      var phone = _PoliticianInfo.PublicPhone();
      new HtmlTableCell
        {
          InnerHtml = string.IsNullOrWhiteSpace(phone) ? "n/a" : phone
        }.AddTo(tr,
          "tdPoliticianInfoContactDetail");

      // Address
      //
      tr = new HtmlTableRow().AddTo(htmlTable, "trPoliticianInfoContact");
      td = new HtmlTableCell().AddTo(tr, "tdPoliticianInfoContactHeading");
      new HtmlSpan {InnerHtml = "Address:"}.AddTo(td);
      var address = _PoliticianInfo.PublicAddress();
      if (!string.IsNullOrWhiteSpace(address))
        address += "<br />";
      address += _PoliticianInfo.PublicCityStateZip();
      new HtmlTableCell
        {
          InnerHtml = string.IsNullOrWhiteSpace(address) ? "n/a" : address
        }.AddTo(
          tr, "tdPoliticianInfoContactDetail");

      // Social media
      //
      tr = new HtmlTableRow().AddTo(htmlTable, "trPoliticianInfoContact");
      td = new HtmlTableCell().AddTo(tr, "tdPoliticianInfoContactHeading");
      new HtmlSpan {InnerHtml = "&nbsp;"}.AddTo(td);
      var webUrl = _PoliticianInfo.PublicWebAddress();
      td = new HtmlTableCell().AddTo(tr,
        "tdPoliticianInfoContactDetail socialMedia");
      new HtmlBreak(2).AddTo(td);
      if (!string.IsNullOrWhiteSpace(webUrl))
      {
        var span = new HtmlSpan().AddTo(td, "TWebsite");
        var title = Politicians.FormatName(_PoliticianInfo) + "'s Website";
        new HtmlAnchor
          {
            HRef = VotePage.NormalizeUrl(webUrl),
            Title = title,
            Target = "view",
            InnerHtml = "Website"
          }.AddTo(span);
        new HtmlBreak(2).AddTo(td);
      }
      SocialMedia.GetAnchors(_PoliticianInfo)
        .AddTo(td);

      return htmlTable;
    }

    private Control CreatePartyAnchor()
    {
      if (_PoliticianInfo.PartyKey() != null)
      {
        if (string.IsNullOrWhiteSpace(_PoliticianInfo.PartyUrl()))
          return new LiteralControl(_PoliticianInfo.PartyName());
        var a = new HtmlAnchor
          {
            HRef = VotePage.NormalizeUrl(_PoliticianInfo.PartyUrl()),
            Title = _PoliticianInfo.PartyName() + " Website",
            Target = "_self",
            InnerHtml = _PoliticianInfo.PartyName()
          };
        return a;
      }
      return new LiteralControl("no party affiliation");
    }

    public static Control GetReport(DataRow politicianInfo)
    {
      var reportObject = new PoliticianInfo();
      return reportObject.GenerateReport(politicianInfo);
    }
  }
}