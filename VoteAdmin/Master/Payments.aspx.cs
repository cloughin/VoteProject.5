using System;
using DB.VoteLog;

namespace Vote.Master
{
  public partial class PaymentsPage : SecurePage, ISuperUser
  {
    #region Private

    #endregion Private

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Payments";
        H1.InnerHtml = "Payments";

        LogDataChange.GetBillingSummary("Ilya.Shambat",
          new DateTime(2014, 1, 1), new DateTime(2014, 4, 30));
      }
    }

    #endregion Event handlers and overrides
  }
}