using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;

namespace Vote
{
  // Steps to add a new Social Medium
  // ================================
  // • Select an appropriate (probably obvious) name for the medium, like YouTube or 
  //   Blogger. Referred to subsequently as <mediumName>.
  // • Add a column at the end of the Politicians table in both the Live and Test 
  //   databases. The column name should be '<mediumName>WebAddress'. 
  //   Attributes: LONGTEXT, NOT NULL.
  // • Select 'Run Custom Tool' on DB/Vote.xml, then compile. There will probably be 
  //   compile errors on Insert methods because of the new column. These are usually
  //   correctable by adding an additional parameter of string.Empty at the end of 
  //   the parameter list.
  // • In DB/Vote.xml, add the new column at the end of the Cache columnSet definition
  //   for the politicians table.
  //     Node:
  //       <generateDbClasses>
  //         <databases>
  //           <database>
  //             <tables>
  //               <table name="Politicians">
  //                 <columnSets>
  //                   <columnSet name="Cache" columns="PoliticianKey,...">
  // • In the PoliticiansCache object, add a method for the new medium named
  //   Get<memberName>WebAddress. Follow the GetFacebookWebAddress method as a model.
  // • Obtain or create a 15x15 image for the medium.
  // • Add an entry to the SocialMedia.SocialMedium array below (beginning new 
  //   SocialMedium(...). The order of the entries in this table determines the order 
  //   of presentation.
  //
  // To disable a medium, set its 'Enabled' flag to false. To completely remove a medium,
  // the above steps must be reversed.

  public static class SocialMedia
  {
    #region Private

    private static readonly ReadOnlyCollection<SocialMedium>
      SocialMediumReadOnlyList;

    static SocialMedia()
    {
      var socialMedium = new[]
      { // The four parameters:
        // 1. The medium name (also the unique key)
        // 2. The type description, used with the name for a complete
        //    description, like Facebook Page, YouTube Video or RSSFeed.
        // 3. The location of the 15x15 image
        // 4. Enabled flag
        new SocialMedium("YouTube", null, "Channel or Video", "/images/YouTube.png", "/images/youtube.2.png", true),
        new SocialMedium("Facebook", null, "Page", "/images/Facebook.png", "/images/facebook.2.png", true),
        new SocialMedium("Twitter", null, "Page", "/images/Twitter.png", "/images/twitter.2.png", true),
        new SocialMedium("Flickr", null, "Page", "/images/Flicker.png", "/images/flickr.2.png", true),
        new SocialMedium("RSSFeed", "RSS Feed", string.Empty, "/images/RSS.png", "/images/rss.2.png", true),
        new SocialMedium("Wikipedia", null, "Page", "/images/Wikipedia.png", "/images/wikipedia.2.png", true),
        new SocialMedium("Vimeo", null, "Video", "/images/Vimeo.jpg", "/images/vimeo.2.png", true),
        new SocialMedium("GooglePlus", "Google+", "Page", "/images/GooglePlus.jpg", "/images/googleplus.2.png", true),
        new SocialMedium("LinkedIn", null, "Page", "/images/LinkedIn.jpg", "/images/linkedin.2.png", true),
        new SocialMedium("Pinterest", null, "Page", "/images/Pinterest.jpg", "/images/pinterest.2.png", true),
        new SocialMedium("Blogger", null, "Site", "/images/Blogger.jpg", "/images/blogger.2.png", true),
        new SocialMedium("Webstagram", "Instagram", "Page", "/images/Instagram.jpg", "/images/instagram.2.png", true),
        new SocialMedium("BallotPedia", null, "Page", "/images/BallotPedia.png", "/images/ballotpedia.2.png", true)
      };

      // create a read-only list with only enabled media
      SocialMediumReadOnlyList = socialMedium.Where(medium => medium.Enabled)
        .ToList()
        .AsReadOnly();
    }

    private static void CreateOneMediumAnchor(Control ul, string webAddress,
      SocialMedium medium, string politicianName, bool useLargeIcons = false)
    {
      var li = new HtmlLi().AddTo(ul);
      var a =
        new HyperLink
          {
            NavigateUrl = VotePage.NormalizeUrl(webAddress),
            Target = medium.Name.ToLowerInvariant(),
            ToolTip = medium.GetTooltip(politicianName)
          }.AddTo(li);
      new Image { ImageUrl = useLargeIcons ? medium.LargeImageUrl : medium.ImageUrl }.AddTo(a);
    }

    private static void CreateEmailAnchor(Control ul, string email, string politicianName, 
      bool useLargeIcons = false)
    {
      var li = new HtmlLi().AddTo(ul);
      var a =
        new HyperLink
          {
            NavigateUrl = "mailto:" + email,
            ToolTip = "Send Email to " + politicianName + " at: " + email
          }.AddTo(li);
      new Image {ImageUrl = useLargeIcons ? "/images/email.2.png" : "/images/Email.png"}.AddTo(a);
    }

    private static Control FinishAnchors(Control ul, bool addClearBoth = true)
    {
      if (ul.Controls.Count == 0)
        return new PlaceHolder();

      var container = new HtmlDiv();
      ul.AddTo(container);
      if (addClearBoth)
      {
        var clear = new HtmlDiv().AddTo(container);
        clear.Style.Add("clear", "both");
      }
      return container;
    }

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static Control GetAnchors(PageCache cache, string politicianKey)
    {
      var politicianName = cache.Politicians.GetPoliticianName(politicianKey);

      var ul = new HtmlUl();
      ul.Attributes.Add("class", "social-media-anchors");

      foreach (var medium in SocialMediaList)
      {
        var webAddress = medium.GetLink(cache, politicianKey);
        if (string.IsNullOrWhiteSpace(webAddress)) continue;
        CreateOneMediumAnchor(ul, webAddress, medium, politicianName);
      }

      // tack on email as a special case
      var email = cache.Politicians.GetPublicEmail(politicianKey);
      if (!string.IsNullOrWhiteSpace(email))
        CreateEmailAnchor(ul, email, politicianName);

      // enclose the <ul> in a div and add a <div style="clear:both"> 
      // for safety sake

      return FinishAnchors(ul);
    }

    public static Control GetAnchors(DataRow row, bool addClearBoth = true,
      bool useLargeIcons = false)
    {
      int count;
      return GetAnchors(row, out count, addClearBoth, useLargeIcons);
    }

    public static Control GetAnchors(DataRow row, out int count, bool addClearBoth = true, 
      bool useLargeIcons = false)
    {
      var politicianName = Politicians.FormatName(row);
      count = 0;

      var ul = new HtmlUl();
      ul.Attributes.Add("class", "social-media-anchors");

      foreach (var medium in SocialMediaList)
      {
        var webAddress = medium.GetLink(row);
        if (string.IsNullOrWhiteSpace(webAddress)) continue;
        count++;
        CreateOneMediumAnchor(ul, webAddress, medium, politicianName, useLargeIcons);
      }

      // tack on email as a special case
      var email = row.PublicEmail();
      if (!string.IsNullOrWhiteSpace(email))
      {
        count++;
        CreateEmailAnchor(ul, email, politicianName, useLargeIcons);
      }

      // enclose the <ul> in a div and add a <div style="clear:both"> 
      // for safety sake

      return FinishAnchors(ul, addClearBoth);
    }

    public static IEnumerable<SocialMedium> SocialMediaList => SocialMediumReadOnlyList;

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }

  public class SocialMedium
  {
    private readonly string _DisplayName; // if null uses name

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public SocialMedium(string name, string displayName, string type,
      string imageUrl, string largeImageUrl, bool enabled)
    {
      Name = name;
      _DisplayName = displayName;
      MediumType = type;
      ImageUrl = imageUrl;
      LargeImageUrl = largeImageUrl;
      Enabled = enabled;
    }

    public string DatabaseColumn => Name + "WebAddress";

    public string Description => DisplayName + " Address";

    public string DisplayName => _DisplayName ?? Name;

    public bool Enabled { get; }

    public string GetLink(DataRow row)
    {
      return row[DatabaseColumn] as string;
    }

    public string GetLink(PageCache cache, string politicianKey)
    {
      if (cache == null)
        return GetLinkFromDatabase(politicianKey);

      var methodInfo = typeof (PoliticiansCache).GetMethod("Get" + DatabaseColumn);
      return
        methodInfo.Invoke(cache.Politicians, new object[] {politicianKey}) as
          string;
    }

    public string GetLinkFromDatabase(string politicianKey)
    {
      return
        Politicians.GetColumn(Politicians.GetColumn(DatabaseColumn), politicianKey)
          as string;
    }

    public string GetTooltip(string politicialName)
    {
      var type = MediumType;
      if (type.Length != 0) type = ' ' + type;
      return politicialName + "'s " + DisplayName + type;
    }

    public string ImageUrl { get; }

    public string LargeImageUrl { get; }

    public string Name { get; }

    public string MediumType { get; }

    public void UpdateLink(string politicianKey, string newValue)
    {
      Politicians.UpdateColumn(Politicians.GetColumn(DatabaseColumn), newValue,
        politicianKey);
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}