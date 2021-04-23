using System;

namespace Vote.Admin
{
  public partial class OfficesAndCandidatesSelection : SecureAdminPage, IAllowEmptyStateCode
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Title = H1.InnerText = "Elections, Offices, Candidates & Ballot Measures CSV";
    }
  }
}