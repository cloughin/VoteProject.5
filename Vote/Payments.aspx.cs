using System;
using static System.String;

namespace Vote
{
  public partial class PaymentsPage : PublicPage
  {
    private const string TitleTag = "{0} | Make a Payment to {0}.org";

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Title = Format(TitleTag, PublicMasterPage.SiteName);
      }
    }
  }
}