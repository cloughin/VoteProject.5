using System;
using DB.Vote;

namespace Vote.Master
{
  public partial class SetStagingPage : SecurePage, ISuperUser
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Set Staging";
        H1.InnerHtml = "Set Staging";

        var staging = Security.GetIsStaging(UserName, false);
        RadioButtonStagingOn.Checked = staging;
        RadioButtonStagingOff.Checked = !staging;
      }
      else
      {
        Security.UpdateIsStaging(RadioButtonStagingOn.Checked, UserName);
      }
    }
  }
}