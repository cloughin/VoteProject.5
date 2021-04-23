using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vote;

namespace VoteNew.Admin
{
  public partial class Default : System.Web.UI.Page
  {
    #region Ajax handlers

    [WebMethod]
    public static DynaTreeNode[] GetNodesForState(string stateCode)
    {
      List<DynaTreeNode> list = new List<DynaTreeNode>();

      list.Add(new DynaTreeNode()
      {
        title = "Elections",
        isFolder = true,
        //isLazy = true,
        //ajaxMethod = "/admin/default.aspx/GetNodesForState",
        //ajaxData = new Dictionary<string, object>() { { "stateCode", "LL" } }
      });

      list.Add(new DynaTreeNode()
      {
        title = "Incumbents",
        isFolder = true,
        //isLazy = true,
        //ajaxMethod = "/admin/default.aspx/GetNodesForState",
        //ajaxData = new Dictionary<string, object>() { { "stateCode", "LL" } }
      });

      list.Add(new DynaTreeNode()
      {
        title = "Offices",
        isFolder = true,
        //isLazy = true,
        //ajaxMethod = "/admin/default.aspx/GetNodesForState",
        //ajaxData = new Dictionary<string, object>() { { "stateCode", "LL" } }
      });

      list.Add(new DynaTreeNode()
      {
        title = "Politicians",
        isFolder = true,
        //isLazy = true,
        //ajaxMethod = "/admin/default.aspx/GetNodesForState",
        //ajaxData = new Dictionary<string, object>() { { "stateCode", "LL" } }
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