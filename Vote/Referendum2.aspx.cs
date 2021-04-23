using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using static System.String;

namespace Vote
{
  public partial class Referendum2Page : PublicPage
  {
    private readonly string _ElectionKey = QueryElection;
    private readonly string _ReferendumKey = QueryReferendum;
    private string _ElectionDescription;
    private ReferendumsRow _Referendum;

    private const string TitleTag = "{2} | {0} (Ballot Measure / Referendum) | {1}";
    private const string MetaDescriptionTag = "{0} (Ballot Measure / Referendum), {1}: Description with link to full text.";

    private static Control CreateAnchor(string url, string anchorText, string cssClass = "")
    {
      var anchor = new HtmlAnchor
      {
        HRef = NormalizeUrl(url),
        InnerHtml = anchorText,
        Target = "view"
      }.AddCssClasses(cssClass);
      anchor.Attributes["ref"] = "nofollow";
      return anchor;
    }

    private void CreateReport()
    {
      ReferendumTitle.InnerHtml = _Referendum.ReferendumTitle.ReplaceNewLinesWithParagraphs();
      ReferendumSubTitle.InnerHtml = StateCache.GetStateName(Referendums.GetStateCodeFromKey(_ReferendumKey)) +
        " Ballot Measure / Referendum";
      ReferendumSubTitle.Visible = false;
      // Link removed per Mantis 840
      //new HtmlAnchor
      //{
      //  HRef = UrlManager.GetElectionPageUri(_ElectionKey).ToString(),
      //  InnerText = _ElectionDescription
      //}.AddTo(ReferendumElection);
      ReferendumElection.InnerText = _ElectionDescription;

      var referendumDescription = _Referendum.ReferendumDescription;
      if (IsNullOrWhiteSpace(referendumDescription))
      {
        ReferendumDescriptionLabel.Visible = false;
        ReferendumDescription.Visible = false;
      }
      else
      {
        ReferendumDescriptionLabel.InnerHtml = "Description:";
        ReferendumDescription.InnerHtml = referendumDescription.ReplaceNewLinesWithParagraphs();
      }

      var detail = _Referendum.ReferendumDetail;
      var detailUrl = _Referendum.ReferendumDetailUrl;
      if (IsNullOrWhiteSpace(detailUrl) && IsNullOrWhiteSpace(detail))
      {
        ReferendumDetailLabel.Visible = false;
        ReferendumDetail.Visible = false;
      }
      else if (IsNullOrWhiteSpace(detail))
      {
        ReferendumDetailLabel.InnerHtml = CreateAnchor(detailUrl, "Link to Detail", "no-print")
          .RenderToString();
        ReferendumDetail.Visible = false;
      }
      else
      {
        ReferendumDetailLabel.InnerHtml =
          $"Detail {CreateAnchor(detailUrl, "(link to detail)", "parenthetical no-print").RenderToString()}";
        ReferendumDetail.InnerHtml = detail.ReplaceNewLinesWithParagraphs();
      }

      var fullText = _Referendum.ReferendumFullText;
      var fullTextUrl = _Referendum.ReferendumFullTextUrl;
      if (IsNullOrWhiteSpace(fullTextUrl) && IsNullOrWhiteSpace(fullText))
      {
        ReferendumFullTextLabel.Visible = false;
        ReferendumFullText.Visible = false;
      }
      else if (IsNullOrWhiteSpace(fullText))
      {
        ReferendumFullTextLabel.InnerHtml = CreateAnchor(fullTextUrl, "Link to Full Text", "no-print")
          .RenderToString();
        ReferendumFullText.Visible = false;
      }
      else
      {
        ReferendumFullTextLabel.InnerHtml =
          $"Full Text {CreateAnchor(fullTextUrl, "(link to full text)").RenderToString()}";
        ReferendumFullText.InnerHtml = fullText.ReplaceNewLinesWithParagraphs();
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      _ElectionDescription = PageCache.Elections.GetElectionDesc(_ElectionKey);
      var table = Referendums.GetData(_ElectionKey, _ReferendumKey);
      if (table.Count == 1)
        _Referendum = table[0];

      var spacedOutTitle = _Referendum.ReferendumTitle.ReplaceNewLinesWithSpaces();
      Title = Format(TitleTag, spacedOutTitle, _ElectionDescription, PublicMasterPage.SiteName);
      MetaDescription = Format(MetaDescriptionTag, spacedOutTitle, _ElectionDescription);

      if (_Referendum != null)
        CreateReport();
    }
  }
}