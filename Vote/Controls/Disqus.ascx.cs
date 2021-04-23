using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class Disqus : UserControl
  {
    protected static string CanonicalUrl => 
      VotePage.GetPage<PublicPage>().CanonicalUri.ToString();

    protected static string EmbedFilename => 
      VotePage.IsDebugging
      ? "//voteusadev.disqus.com/embed.js"
      : "//voteusa.disqus.com/embed.js";

    protected string CacheKey { get; } = 
      VotePage.GetPage<CacheablePage>().CacheKey;

    protected void Page_Load(object sender, EventArgs e) => 
      Page.ClientScript.RegisterStartupScript(GetType(), "Disqus",
      VotePage.IsDebugging
        ? "<script id=\"dsq-count-scr\" src=\"//voteusadev.disqus.com/count.js\" async></script>"
        : "<script id=\"dsq-count-scr\" src=\"//voteusa.disqus.com/count.js\" async></script>");
  }
}