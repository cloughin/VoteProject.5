using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Vote;

namespace VoteNew
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  [System.ComponentModel.ToolboxItem(false)]
  [System.Web.Script.Services.ScriptService]
  public class WebService : System.Web.Services.WebService
  {
    [WebMethod]
    public AddressFinderResult FindAddress(string input)
    {
      return AddressFinder.Find(input);
    }

    [WebMethod]
    public AddressFinderResult FindState(string input)
    {
      Uri redirectUri = UrlManager.GetVotersPageUri(input);
      //Uri redirectUri = UrlManager.GetOfficialsPageUri(input);
      // This is temp code while developing new site
      redirectUri = Transition.SetLegacyPort(redirectUri);

      AddressFinderResult result = new AddressFinderResult()
      {
        RedirectUrl = redirectUri.ToString(),
        SuccessMessage = "Redirecting to the " + StateCache.GetStateName(input) + " 'for Voters' page"
        //SuccessMessage = "Redirecting to state officials page"
      };

      return result;
    }
  }
}
