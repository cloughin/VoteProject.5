using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vote;

namespace VoteNew.Master
{
  public partial class Default : System.Web.UI.Page
  {
    #region Ajax handlers

    [WebMethod]
    public static DynaTreeNode[] GetStateNodes(int option)
    {
      List<DynaTreeNode> list = new List<DynaTreeNode>();
      switch (option)
      {
        case 0: // states only
          break;

        case 1: // include federal
          foreach (string code in StateCache.AllFederalCodes)
            list.Add(new DynaTreeNode()
            {
              title = StateCache.GetStateName(code),
              isFolder = true,
              isLazy = true,
              ajaxMethod = "/admin/default.aspx/GetNodesForState",
              ajaxData = new Dictionary<string, object>() { {"stateCode", code} }
            });
          break;

        case 2: // include All Candidates and National
          list.Add(new DynaTreeNode()
          {
            title = "All Candidates",
            isFolder = true,
            isLazy = true,
            ajaxMethod = "/admin/default.aspx/GetNodesForState",
            ajaxData = new Dictionary<string, object>() { { "stateCode", "LL" } }
          });
          list.Add(new DynaTreeNode()
          {
            title = "National",
            isFolder = true,
            isLazy = true,
            ajaxMethod = "/admin/default.aspx/GetNodesForState",
            ajaxData = new Dictionary<string, object>() { { "stateCode", "US" } }
          });
          break;
      }
      foreach (string code in StateCache.AllStateCodes)
        list.Add(new DynaTreeNode()
        {
          title = StateCache.GetStateName(code),
          isFolder = true,
          isLazy = true,
          ajaxMethod = "/admin/default.aspx/GetNodesForState",
          ajaxData = new Dictionary<string, object>() { { "stateCode", code } }
        });
      return list.ToArray();
    }

    #endregion Ajax handlers

    #region Event handlers

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion Event handlers
  }
}