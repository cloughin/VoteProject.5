using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Vote.Master
{
  public partial class AdminMaster : MasterPage, SecurePage.IAdminMaster
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    //public Control FindHeadContentControl(string id)
    //{
    //  return MasterHeadContent.FindControl(id);
    //}

    public Control FindMainContentControl(string id)
    {
      return MasterMainContent.FindControl(id);
    }

    public HtmlForm MasterForm
    {
      get { return MainForm; }
    }

    public bool HasMenu
    {
      get { return AdminMenu.HasMenu; }
    }

    //public ContentPlaceHolder HeadContentControl { get { return MasterHeadContent; } }

    public void SetJavascriptNotNeeded()
    {
      HtmlTag.AddCssClasses("no-js-ok");
    }

    public void SetNoCacheUrl(string url)
    {
      NoCacheIFrame.Src = url;
    }

    public ContentPlaceHolder MainContentControl
    {
      get { return MasterMainContent; }
    }

    public bool NoHeading
    {
      set
      {
        AdminPage.Visible = !value;
      }
    }

    public bool NoMenu
    {
      set
      {
        AdminMenu.Visible = !value;
      }
    }

    //public void SetMenuVisibility(bool visible)
    //{
    //  AdminMenu.Visible = visible;
    //}


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public

    #region Event handlers and overrides

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      // we do this here to guarantee it comes before any Page_OnLoad's
      Page.IncludeCss("/css/vote/adminMaster.css", "if IE 7", "ie7", "if IE 8", "ie8",
        "if gte IE 9", "ie9");
      var cssFile =
        Path.ChangeExtension(Path.Combine("/css/vote", Request.Path.Substring(1)), "css");
      Page.IncludeCss(cssFile, "if IE", "ie", "if IE 7", "ie7", "if IE 8", "ie8",
        "if gte IE 9", "ie9");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var scriptFile =
        Path.ChangeExtension(Path.Combine("/js/vote", Request.Path.Substring(1)), "js");
      if (File.Exists(Server.MapPath(scriptFile)))
        scriptFile = Path
          .ChangeExtension(Path.Combine("vote", Request.Path.Substring(1)), null)
          .Replace('\\', '/');
      else scriptFile = "vote/init";
      var cs = Page.ClientScript;
      var type = GetType();
      const string scriptName = "app";
      if (cs.IsStartupScriptRegistered(type, scriptName)) return;
      cs.RegisterStartupScript(type, scriptName,
        $"require(['{scriptFile}'], function(){{}});", true);
    }

    #endregion Event handlers and overrides
  }
}