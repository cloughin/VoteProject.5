using System;
using System.Web.UI;
using DB.Vote;

namespace Vote
{
  // This page is for AWS Load Balancer health testing.
  // It should inherit directly from System.Web.UI.Page
  public partial class PingPage : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      using (VoteDb.GetOpenConnection())
      {
        // This tests the health of the DB.
        // It will throw an exception if the DB is unavailable.
      }
    }
  }
}