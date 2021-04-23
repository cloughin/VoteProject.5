using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote
{
  public interface IFeedbackContainerControl
  {
    string CssClass { get; set; }
  }

  public class MixedNumericComparer : IComparer<string>
  {
    private static MixedNumericComparer _Instance;

    public static MixedNumericComparer Instance =>
      _Instance ?? (_Instance = new MixedNumericComparer());

    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    private static extern int StrCmpLogicalW(string x, string y);

    public int Compare(string x, string y)
    {
      return StrCmpLogicalW(x, y);
    }
  }

  public sealed class SimpleListItem
  {
    public string Text;
    public string Value;

    public SimpleListItem()
    {
    }

    public SimpleListItem(string value, string text)
    {
      Text = text;
      Value = value;
    }

    public override string ToString()
    {
      return Text;
    }
  }

  public sealed class StringPairIgnoreCase : IComparable<StringPairIgnoreCase>
  {
    public readonly string String1;
    public readonly string String2;

    public StringPairIgnoreCase(string string1, string string2)
    {
      String1 = string1;
      String2 = string2;
    }

    public int CompareTo(StringPairIgnoreCase other)
    {
      if (String1.IsLtIgnoreCase(other.String1)) return -1;
      if (String1.IsGtIgnoreCase(other.String1)) return 1;
      if (String2.IsLtIgnoreCase(other.String2)) return -1;
      if (String2.IsGtIgnoreCase(other.String2)) return 1;
      return 0;
    }

    public override bool Equals(object obj)
    {
      // ReSharper disable once RedundantCast
      if (!(obj is StringPairIgnoreCase o)) return false;
      return Compare(String1, o.String1, StringComparison.OrdinalIgnoreCase) == 0 &&
        Compare(String2, o.String2, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public override int GetHashCode()
    {
      return String1.GetHashCode() ^ String2.GetHashCode();
    }
  }

  public class HtmlBreak : Literal
  {
    public HtmlBreak(int n = 1)
    {
      n = Math.Max(1, n);
      switch (n)
      {
        case 1:
          Text = "<br />";
          break;
        case 2:
          Text = "<br /><br />";
          break;
        default:
        {
          var sb = new StringBuilder();
          for (var x = 1; x <= n; x++) sb.Append("<br />");
          Text = sb.ToString();
        }
          break;
      }
    }
  }

  public class HtmlDiv : HtmlGenericControl
  {
    public HtmlDiv() : base("div")
    {
    }
  }

  public class HtmlEm : HtmlGenericControl
  {
    public HtmlEm() : base("em")
    {
    }
  }

  public class HtmlH1 : HtmlGenericControl
  {
    public HtmlH1() : base("h1")
    {
    }
  }

  public class HtmlH2 : HtmlGenericControl
  {
    public HtmlH2() : base("h2")
    {
    }
  }

  public class HtmlH3 : HtmlGenericControl
  {
    public HtmlH3() : base("h3")
    {
    }
  }

  public class HtmlH4 : HtmlGenericControl
  {
    public HtmlH4() : base("h4")
    {
    }
  }

  public class HtmlH5 : HtmlGenericControl
  {
    public HtmlH5() : base("h5")
    {
    }
  }

  public class HtmlH6 : HtmlGenericControl
  {
    public HtmlH6() : base("h6")
    {
    }
  }

  public class HtmlHr : HtmlGenericControl
  {
    public HtmlHr() : base("hr")
    {
    }
  }

  public class HtmlLabel : HtmlGenericControl
  {
    public HtmlLabel() : base("label")
    {
    }
  }

  public class HtmlLi : HtmlGenericControl
  {
    public HtmlLi() : base("li")
    {
    }
  }

  public class HtmlNbsp : Literal
  {
    public HtmlNbsp()
    {
      Text = "&nbsp;";
    }
  }

  public class HtmlP : HtmlGenericControl
  {
    public HtmlP() : base("p")
    {
    }
  }

  public class HtmlSpan : HtmlGenericControl
  {
    public HtmlSpan() : base("span")
    {
    }
  }

  public class HtmlStrong : HtmlGenericControl
  {
    public HtmlStrong() : base("strong")
    {
    }
  }

  public class HtmlTableHeadingCell : HtmlTableCell
  {
    public HtmlTableHeadingCell() : base("th")
    {
    }
  }

  public class HtmlTBody : HtmlGenericControl
  {
    public HtmlTBody() : base("tbody")
    {
    }
  }

  public class HtmlTHead : HtmlGenericControl
  {
    public HtmlTHead() : base("thead")
    {
    }
  }

  public class HtmlUl : HtmlGenericControl
  {
    public HtmlUl() : base("ul")
    {
    }
  }

  public class LocalDate : HtmlGenericControl
  {
    private static readonly long BaseTicks = new DateTime(1970, 1, 1).Ticks;

    public LocalDate(string format = null) : this(DateTime.UtcNow, format)
    {
    }

    public LocalDate(DateTime utcDate, string format = null) : base("span")
    {
      Attributes.Add("class", "localdate");
      Attributes.Add("ticks",
        (utcDate.Ticks - BaseTicks).ToString(CultureInfo.InvariantCulture));
      if (!IsNullOrWhiteSpace(format)) Attributes.Add("format", format);
    }

    public static string AsString(string format = null)
    {
      return new LocalDate(format).RenderToString();
    }

    public static string AsString(DateTime utcDate, string format = null)
    {
      return new LocalDate(utcDate, format).RenderToString();
    }
  }

  public static class Utility
  {
    public const bool BannerAdStaging = true;

    public static string GetAdjustedSiteUri(string path = null, string query = null)
    {
      var url = UrlManager.GetSiteUri(path, query).ToString();
      if (BannerAdStaging)
        url = url.Replace("https://vote-usa.org", "http://stage.vote-usa.org");
      return url;
    }

    public static string GetControlValue(Control control)
    {
      if (control is TextBox textBox) return textBox.Text;

      if (control is ListControl listControl) return listControl.SelectedValue;

      if (control is HtmlSelect htmlSelect) return htmlSelect.Value;

      if (control is HtmlInputCheckBox htmlInputCheckBox)
        return htmlInputCheckBox.Checked.ToString();

      if (control is HtmlInputHidden htmlInputHidden) return htmlInputHidden.Value;

      if (control is HtmlGenericControl htmlGenericControl)
      {
        if (htmlGenericControl.HasClass("radio-container"))
        {
          foreach (var child in htmlGenericControl.Controls)
          {
            if (child is HtmlInputRadioButton htmlInputRadioButton &&
              htmlInputRadioButton.Checked)
              return htmlInputRadioButton.Value;
          }

          return Empty;
        }
      }

      throw new ApplicationException("Unsupported Control type");
    }

    public static int Md5HashString(string input, bool abs = false)
    {
      var hashBytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
      var result = 0;
      for (var x = 0; x < 16; x += 4)
      {
        result = result ^ BitConverter.ToInt32(hashBytes, x);
      }
      if (abs) result = Math.Abs(result);
      return result;
    }

    public static void PopulateEmpty(DropDownList dropDownList, string text = "<none>")
    {
      dropDownList.Items.Clear();
      dropDownList.AddItem(text, Empty, true);
    }

    public static void PopulateEmpty(HtmlSelect dropDownList, string text = "<none>")
    {
      dropDownList.Items.Clear();
      dropDownList.AddItem(text, Empty, true);
    }

    public static void PopulateFromList(DropDownList dropDownList,
      IEnumerable<SimpleListItem> list, string valueToSelect = null)
    {
      dropDownList.Items.Clear();
      foreach (var item in list)
        dropDownList.AddItem(item.Text, item.Value, item.Value == valueToSelect);
    }

    public static void PopulateFromList(HtmlSelect dropDownList,
      IEnumerable<SimpleListItem> list, string valueToSelect = null)
    {
      dropDownList.Items.Clear();
      foreach (var item in list)
        dropDownList.AddItem(item.Text, item.Value, item.Value == valueToSelect);
    }

    //public static string RenderAds(string electionKey, string officeKey, string adKey = null)
    //{
    //  var table = ElectionsPoliticians.GetAds(electionKey, officeKey, adKey);
    //  if (table.Rows.Count == 0) return Empty;
    //  var ads = new List<string>();
    //  var cache = VotePage.GetPageCache().Politicians;
    //  foreach (var ad in table.Rows.OfType<DataRow>())
    //  {
    //    var thumbnailId = IsNullOrWhiteSpace(ad.AdThumbnailUrl())
    //      ? ad.AdUrl().GetYouTubeVideoId()
    //      : ad.AdThumbnailUrl().GetYouTubeVideoId();
    //    ads.Add(RenderOneAd(ad.PoliticianKey(), cache.GetPoliticianName(ad.PoliticianKey()),
    //      ad.AdUrl(), thumbnailId, ad.AdSponsor(), ad.AdSponsorUrl(), ad.AdIsCandidateSponsored()));
    //  }

    //  return $"<div class=\"ads-outer\">{Join(Empty, ads)}</div>";
    //}

    //public static string RenderOneAd(string politicianKey, string politicianName, 
    //  string videoUrl, string thumbnailId, string sponsor, string sponsorUrl, bool isCandidateSponsored)
    //{
    //  string paidMessage;
    //  if (isCandidateSponsored)
    //    paidMessage =
    //      "Paid advertisement by <a target=\"_blank\"" + 
    //      $" href=\"{UrlManager.GetIntroPageUri(politicianKey)}\">candidate&rsquo;s campaign</a>";
    //  else if (!IsNullOrWhiteSpace(sponsor))
    //    paidMessage = IsNullOrWhiteSpace(sponsorUrl)
    //      ? $"Paid advertisement by {sponsor}"
    //      : $"Paid advertisement by <a target=\"_blank\" href=\"{VotePage.NormalizeUrl(sponsorUrl)}\">" +
    //        $"{sponsor}</a>";
    //  else
    //    paidMessage = "Paid advertisement";


    //  return "<div class=\"ad-outer\"><div class=\"ad-inner\"><div class=\"ad-copy\">" +
    //    $"<p class=\"ad-name\">{politicianName}</p>" +
    //    $"<p class=\"paid-ad\">{paidMessage}</p></div>" +
    //    $"<a class=\"video-wrapper-outer\" target=\"youtube\" href=\"{VotePage.NormalizeUrl(videoUrl)}\">" +
    //    "<div class=\"video-container\"><div class=\"video-player\"><div>" + 
    //    $"<img class=\"video-thumb\" src=\"http://i.ytimg.com/vi/{thumbnailId}/hqdefault.jpg\"/>" +
    //    "<div class=\"video-play-button\"></div></div></div></div></a>" +
    //    $"<img class=\"ad-profile\" src=\"/Image.aspx?Id={politicianKey}&Col=Headshot100\"/>" +
    //    "<div style=\"clear:both\"></div></div></div>";
    //}

    public static string RenderAds(string electionKey, string officeKey, string adKey = null)
    {
      var table = ElectionsPoliticians.GetAds(electionKey, officeKey, adKey);
      if (table.Rows.Count == 0) return Empty;
      var ads = new List<string>();
      var cache = VotePage.GetPageCache().Politicians;
      foreach (var ad in table.Rows.OfType<DataRow>())
      {
        var thumbnailId = IsNullOrWhiteSpace(ad.AdThumbnailUrl())
          ? ad.AdUrl().GetYouTubeVideoId()
          : ad.AdThumbnailUrl().GetYouTubeVideoId();
        ads.Add(RenderOneAd(electionKey, officeKey, ad.AdType(), ad.PoliticianKey(), 
          cache.GetPoliticianName(ad.PoliticianKey()),  ad.AdUrl(), thumbnailId, 
          ad.AdSponsor(), ad.AdSponsorUrl(), ad.AdIsCandidateSponsored()));
      }

      return $"<div class=\"ads-outer\">{Join(Empty, ads)}</div>";
    }

    public static string RenderOneAd(string electionKey, string officeKey, string adType, 
      string politicianKey, string politicianName, string videoUrl, string thumbnailId, 
      string sponsor, string sponsorUrl, bool isCandidateSponsored)
    {
      string paidMessage;
      if (isCandidateSponsored)
      {
        var url = IsNullOrWhiteSpace(sponsorUrl)
          ? UrlManager.GetIntroPageUri(politicianKey).ToString()
          : VotePage.NormalizeUrl(sponsorUrl);
        paidMessage =
          "Paid advertisement by<br/><a target=\"_blank\"" +
          $" href=\"{url}\">candidate&rsquo;s campaign</a>";
      }
      else if (!IsNullOrWhiteSpace(sponsor))
        paidMessage = IsNullOrWhiteSpace(sponsorUrl)
          ? $"Paid advertisement by<br/>{sponsor}"
          : $"Paid advertisement by<br/><a target=\"_blank\" href=\"{VotePage.NormalizeUrl(sponsorUrl)}\">" +
            $"{sponsor}</a>";
      else
        paidMessage = "Paid advertisement";

      var website = adType == "I"
        ? VotePage.NormalizeUrl(Politicians.GetPublicWebAddress(politicianKey))
        : Empty;

      return adType == "Y"
        ? "<div class=\"ad-outer flex\"><div class=\"ad-inner\"><div class=\"ad-copy\">" +
        $"<p class=\"ad-name\">{politicianName}</p>" +
        $"<p class=\"paid-ad\">{paidMessage}</p></div><div class=\"right-wrapper\">" +
        $"<img class=\"ad-profile\" src=\"/Image.aspx?Id={politicianKey}&Col=Headshot100\"/>" +
        $"<a class=\"video-wrapper-outer\" target=\"youtube\" href=\"{VotePage.NormalizeUrl(videoUrl)}\">" +
        "<div class=\"video-container\"><div class=\"video-player\"><div>" +
        $"<img class=\"video-thumb\" src=\"http://i.ytimg.com/vi/{thumbnailId}/hqdefault.jpg\"/>" +
        "<div class=\"video-play-button\"></div></div></div></div></a>" +
        "<div style=\"clear:both\"></div></div></div></div>"
        : "<div class=\"ad-outer flex\"><div class=\"ad-inner\"><div class=\"ad-copy\">" +
        $"<p class=\"ad-name\">{politicianName}</p>" +
        $"<p class=\"paid-ad\">{paidMessage}</p></div><div class=\"right-wrapper\">" +
        $"<img class=\"ad-profile\" src=\"/Image.aspx?Id={politicianKey}&Col=Headshot100\"/>" +
        $"<div class=\"image-wrapper\"><a class=\"image-link\" target=\"ext\" href=\"{website}\">" +
        $"<img class=\"ad-image\" src=\"/adimage?{electionKey}.{officeKey}.{politicianKey}\" /></a></div>" +
        "</div></div></div></div>";
    }

    //private static string RenderBannerAd(string adType, string stateCode, string electionKey,
    //  string officeKey, bool show)
    //{
    //  //if (UrlManager.IsLive /*&& adType != "H"*/) return Empty;
    //  var adTable = BannerAds.GetRenderInfoData(adType, stateCode, electionKey, officeKey);
    //  if (adTable.Count == 0) return Empty;
    //  var ad = adTable[0];
    //  if (!ad.AdEnabled && !show) return Empty;

    //  return $"<div class=\"banner-ad-outer\"><a href=\"{VotePage.NormalizeUrl(ad.AdUrl)}\"" +
    //    $" target=\"ad\"><img src=\"/banneradimage?{adType}.{stateCode}.{electionKey}.{officeKey}.{DateTime.UtcNow.Ticks}\"" +
    //    " alt=\"Ad Image\"/></a><p class=\"paid-advertisement-notice\">Paid Advertisement</p>" +
    //    "<hr /></div>";
    //}

    //public static string RenderBannerAd(string adType, string stateCode, string electionKey,
    //  string officeKey, bool show, int orgId, bool forAdmin = false)
    //{
    //  //if (UrlManager.IsLive /*&& adType != "H"*/) return Empty;
    //  // If orgID is non zero, the organzation ad will be shown. If the org has no ad, no ad will be shown.
    //  if (orgId == 0)
    //    return RenderBannerAd(adType, stateCode, electionKey, officeKey, show);

    //  var adTable = Organizations.GetAdData(orgId);
    //  if (adTable.Count == 0) return Empty;
    //  var ad = adTable[0];
    //  if (IsNullOrWhiteSpace(ad.AdImageName)) return Empty;
    //  var src = forAdmin 
    //    ? GetAdjustedSiteUri("orgadimage", $"{orgId}.{DateTime.UtcNow.Ticks}") 
    //    : $"/orgadimage?{orgId}.{DateTime.UtcNow.Ticks}";
    //  var onClick = IsNullOrWhiteSpace(ad.AdUrl)
    //    ? "onclick=\"return false;\""
    //    : Empty;

    //  return $"<div class=\"banner-ad-outer\"><a href=\"{VotePage.NormalizeUrl(ad.AdUrl)}\"" +
    //    $" {onClick} target=\"ad\"><img src=\"{src}\"" +
    //    " alt=\"Ad Image\"/></a><p class=\"paid-advertisement-notice\">Paid Advertisement</p>" +
    //    "<hr /></div>";
    //}

    public static string RenderBannerAd2(string adType, string stateCode, string electionKey,
      string officeKey, bool show)
    {
      var adTable = BannerAds.GetRenderInfo2Data(adType, stateCode, electionKey, officeKey);
      if (adTable.Count == 0) return Empty;
      var ad = adTable[0];
      if (!ad.AdEnabled && !show) return Empty;
      var descriptionLines = new List<string>();
      if (!IsNullOrWhiteSpace(ad.AdDescription1))
      {
        descriptionLines.Add($"<div class=\"ad3-description\"><p>{ad.AdDescription1}</p>");
        if (!IsNullOrWhiteSpace(ad.AdDescription2))
          descriptionLines.Add($"<p>{ad.AdDescription2}</p>");
        if (!IsNullOrWhiteSpace(ad.AdDescriptionUrl))
        {
          descriptionLines.Insert(0, $"<a href=\"{VotePage.NormalizeUrl(ad.AdDescriptionUrl)}\" target=\"_blank\">");
          descriptionLines.Add("</a>");
        }
        descriptionLines.Add("</div>");
      }

      var thumbnailId = ad.AdYouTubeUrl.GetYouTubeVideoId();
      var paidAdvertisement = ad.AdIsPaid 
        ? "<p class=\"ad3-paid-advertisement-notice\">Paid Advertisement</p>"
        : Empty;
      var imgSource = IsNullOrWhiteSpace(ad.AdImageName)
        ? $"http://i.ytimg.com/vi/{thumbnailId}/hqdefault.jpg"
        : $"{GetAdjustedSiteUri("banneradimage", $"{ adType}.{ stateCode}.{ electionKey}.{ officeKey}.{ DateTime.UtcNow.Ticks}")}";

      return ad.AdMediaType == "Y"
        ? "<div class=\"ad3-outer ad3-yt\"><div class=\"ad3-inner\"><div class=\"ad3-copy\">" +
        $"{Join(Empty, descriptionLines)}</div>" +
        $"<div class=\"ad3-right-wrapper\"><a class=\"ad3-video-wrapper-outer\" target=\"youtube\" href=\"{VotePage.NormalizeUrl(ad.AdYouTubeUrl)}\">" +
        "<div class=\"ad3-video-container\"><div class=\"ad3-video-player\"><div>" +
        //$"<img class=\"video-thumb\" src=\"http://i.ytimg.com/vi/{thumbnailId}/hqdefault.jpg\"/>" +
        $"<img class=\"ad3-video-thumb\" src=\"{imgSource}\"/>" +
        "<div class=\"video-play-button yt-play-button\"></div></div></div></div></a></div>" +
        $"</div>{paidAdvertisement}</div>"

        : "<div class=\"ad3-outer ad3-img\"><div class=\"ad3-inner\"><div class=\"ad3-copy\">" +
        $"{Join(Empty, descriptionLines)}</div>" +
        $"<div class=\"ad3-right-wrapper\"><div class=\"ad3-image-wrapper\"><a class=\"ad3-image-link\" target=\"ext\" href=\"{VotePage.NormalizeUrl(ad.AdUrl)}\">" +
        //$"<img class=\"ad-image\" src=\"{GetAdjustedSiteUri("banneradimage", $"{adType}.{stateCode}.{electionKey}.{officeKey}.{DateTime.UtcNow.Ticks}")}\" /></a></div></div>" +
        $"<img class=\"ad3-image\" src=\"{imgSource}\" /></a></div></div>" +
        $"</div>{paidAdvertisement}</div>";
    }

    public static string RenderBannerAd(string adType, string stateCode, string electionKey,
      string officeKey, bool show, int orgId, bool forAdmin = false)
    {
      if (UrlManager.IsLive && adType == "H" && orgId == 0) return Empty; // disable home ads for now
      // If orgID is non zero, the organzation ad will be shown. If the org has no ad, no ad will be shown.
      if (orgId == 0)
        return RenderBannerAd2(adType, stateCode, electionKey, officeKey, show);

      var adTable = Organizations.GetAdData(orgId);
      if (adTable.Count == 0) return Empty;
      var ad = adTable[0];
      if (IsNullOrWhiteSpace(ad.AdImageName)) return Empty;
      var src = forAdmin
        ? GetAdjustedSiteUri("orgadimage", $"{orgId}.{DateTime.UtcNow.Ticks}")
        : $"/orgadimage?{orgId}.{DateTime.UtcNow.Ticks}";
      var onClick = IsNullOrWhiteSpace(ad.AdUrl)
        ? "onclick=\"return false;\""
        : Empty;

      return $"<div class=\"banner-ad-outer\"><a href=\"{VotePage.NormalizeUrl(ad.AdUrl)}\"" +
        $" {onClick} target=\"ad\"><img src=\"{src}\"" +
        " alt=\"Ad Image\"/></a><p class=\"paid-advertisement-notice\">Paid Advertisement</p>" +
        "<hr /></div>";
    }

    public static void SetControlValue(Control control, string value)
    {
      if (control is TextBox textBox)
      {
        textBox.Text = value;
        return;
      }

      if (control is ListControl listControl)
      {
        // Don't throw exception if value not found, just select first
        if (listControl.Items.OfType<ListItem>().Any(i => i.Value == value))
          listControl.SelectedValue = value;
        else if (listControl.Items.Count > 0)
          listControl.SelectedIndex = 0;
        return;
      }

      if (control is HtmlSelect htmlSelect)
      {
        // Don't throw exception if value not found, just select first
        if (htmlSelect.Items.OfType<ListItem>().Any(i => i.Value == value))
          htmlSelect.Value = value;
        else if (htmlSelect.Items.Count > 0)
          htmlSelect.SelectedIndex = 0;
        return;
      }

      if (control is HtmlInputCheckBox htmlInputCheckBox)
      {
        bool.TryParse(value, out var isChecked);
        htmlInputCheckBox.Checked = isChecked;
        return;
      }

      if (control is HtmlInputHidden htmlInputHidden)
      {
        htmlInputHidden.Value = value;
        return;
      }

      if (control is HtmlGenericControl htmlGenericControl)
      {
        if (htmlGenericControl.HasClass("radio-container"))
        {
          foreach (var child in htmlGenericControl.Controls)
          {
            if (child is HtmlInputRadioButton htmlInputRadioButton)
              htmlInputRadioButton.Checked = htmlInputRadioButton.Value == value;
          }

          return;
        }
      }

      throw new ApplicationException("Unsupported Control type");
    }

    public static void Signal404()
    {
      var response = HttpContext.Current.Response;
      response.Status = "404 Not Found";
      response.StatusCode = 404;
      response.End();
    }
  }
}