using System;
using static System.String;

namespace Vote.Admin
{
  public partial class DownloadOfficesCsv : SecureAdminPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Title = H1.InnerText = $"Download Offices CSV for {StateCache.GetStateName(QueryState)}";
      }
    }
  }
}