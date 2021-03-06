//using System;
//using System.Diagnostics;
//using System.Linq;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using Vote.Controls;

namespace Vote.Politician
{
  public partial class UpdateIntroPage
  {
    //#region Private

    //#region DataItem object

    //[PageInitializer]
    //private class BioTabItem : PoliticianDataItem
    //{
    //  private const string GroupName = "Bio";
    //  private HtmlAnchor _TabLabel;
    //  private HtmlInputHidden _DescriptionTag;
    //  public Button Button;
    //  private string _HtmlDescription;
    //  private HtmlGenericControl _Undo;

    //  private BioTabItem(UpdateIntroPage page) : base(page, GroupName) {}

    //  private static BioTabItem[] GetBioTabInfo(UpdateIntroPage page)
    //  {
    //    var bioTabInfo = new[]
    //      {
    //        new BioTabItem(page)
    //          {
    //            _HtmlDescription = "General<br />Philosophy",
    //            Column = "GeneralStatement"
    //          },
    //        new BioTabItem(page)
    //          {
    //            _HtmlDescription = "Personal<br />and Family",
    //            Column = "Personal"
    //          },
    //        new BioTabItem(page)
    //          {
    //            _HtmlDescription = "Educational<br />Background",
    //            Column = "Education"
    //          },
    //        new BioTabItem(page)
    //          {
    //            _HtmlDescription = "Professional<br />Experience",
    //            Column = "Profession"
    //          },
    //        new BioTabItem(page)
    //          {
    //            _HtmlDescription = "Military<br />Service",
    //            Column = "Military"
    //          },
    //        new BioTabItem(page)
    //          {
    //            _HtmlDescription = "Civic<br />Involvement",
    //            Column = "Civic"
    //          },
    //        new BioTabItem(page)
    //          {
    //            _HtmlDescription = "Political<br />Experience",
    //            Column = "Political"
    //          },
    //        new BioTabItem(page)
    //          {
    //            _HtmlDescription = "Religious<br />Affiliation",
    //            Column = "Religion"
    //          },
    //        new BioTabItem(page)
    //          {
    //            _HtmlDescription = "Accomplishments<br />and Awards",
    //            Column = "Accomplishments"
    //          }
    //      };

    //    foreach (var item in bioTabInfo)
    //    {
    //      item.InitializeItem(page, false);
    //      InitializeGroup(page, GroupName + item.Column);
    //    }
    //    return bioTabInfo;
    //  }

    //  // ReSharper disable UnusedMember.Local
    //  // Invoked via Reflection
    //  private static void Initialize(UpdateIntroPage page)
    //    // ReSharper restore UnusedMember.Local
    //  {
    //    page._BioTabInfo = GetBioTabInfo(page);
    //    page.RegisterUpdateAll(page.UpdateAllBio);
    //    if (!page.IsPostBack)
    //    {
    //      new MainTabItem { TabName = GroupName }.Initialize(page);
    //      page.LoadBioTabData();
    //    }
    //  }

    //  protected override void InitializeItem(TemplateControl owner,
    //    bool addMonitorClasses = true, FeedbackContainerControl feedback = null)
    //  {
    //    var page = owner as SecurePage;

    //    base.InitializeItem(page, addMonitorClasses);

    //    Feedback =
    //      page.Master.FindMainContentControl("FeedbackBio" + Column) as
    //        FeedbackContainerControl;

    //    _TabLabel =
    //      page.Master.FindMainContentControl("TabBio" + Column) as HtmlAnchor;
    //    _Undo =
    //      page.Master.FindMainContentControl("UndoBio" + Column) as
    //        HtmlGenericControl;
    //    Button = page.Master.FindMainContentControl("ButtonBio" + Column) as Button;
    //    _DescriptionTag =
    //      page.Master.FindMainContentControl("DescriptionBio" + Column) as
    //        HtmlInputHidden;

    //    // Build text description from html version
    //    Description = _HtmlDescription.StripHtml(" ")
    //      .StripRedundantWhiteSpace();

    //    // Set control values
    //    Debug.Assert(_TabLabel != null, "_TabLabel != null");
    //    _TabLabel.InnerHtml = _HtmlDescription;
    //    Debug.Assert(_DescriptionTag != null, "_DescriptionTag != null");
    //    _DescriptionTag.Value = Description;
    //    Debug.Assert(Button != null, "Button != null");
    //    Button.ToolTip = "Update " + Description;
    //    Debug.Assert(_Undo != null, "_Undo != null");
    //    var htmlGenericControl = _Undo.Parent as HtmlGenericControl;
    //    if (htmlGenericControl != null)
    //      htmlGenericControl.Attributes.Add("title",
    //        string.Format("Revert your {0} entry to the latest saved version.",
    //          Description));

    //    var li = _TabLabel.Parent as HtmlGenericControl;
    //    var monitor =
    //      page.MonitorFactory.GetMonitorInstance("bio" + Column.ToLowerInvariant());

    //    // add the asterisk and star icons to the bio vertical tabs
    //    CreateAsteriskIndicator("AstBio" + Column, monitor.GetAsteriskClass(null),
    //      string.Format("There are unsaved changed to {0}", Description))
    //      .AddTo(li);

    //    CreateStarIndicator("StarBio" + Column, monitor.GetStarClass(null),
    //      string.Format("We have a response from you for {0}", Description))
    //      .AddTo(li);

    //    // add the clear button
    //    var clear = CreateClearButton("ClearBio" + Column,
    //      monitor.GetClearClass(null), string.Format("Clear {0}", Description));
    //    _Undo.Parent.AddAfter(clear);
    //  }
    //}

    //private BioTabItem[] _BioTabInfo;

    //#endregion DataItem object

    //private void LoadBioTabData()
    //{
    //  foreach (var item in _BioTabInfo)
    //    item.LoadControl();
    //}

    //private int UpdateAllBio(bool showSummary)
    //{
    //  _BioTabInfo.ClearValidationErrors();
    //  var errorCount = 0;
    //  foreach (var item in _BioTabInfo)
    //  {
    //    var status = item.DoUpdate(false);
    //    if (status == UpdateStatus.Failure)
    //      errorCount++;
    //    if (status != UpdateStatus.Unchanged)
    //    {
    //      var updatePanel =
    //        Master.FindMainContentControl("UpdatePanelBio" + item.Column) as
    //          UpdatePanel;
    //      if (updatePanel != null)
    //        updatePanel.Update();
    //    }
    //  }
    //  InvalidatePageCache();
    //  return errorCount;
    //}

    //#endregion Private

    //#region Event handlers and overrides

    //protected void ButtonBio_OnClick(object sender, EventArgs e)
    //{
    //  PoliticianDataItem dataItem = null;

    //  try
    //  {
    //    dataItem = _BioTabInfo.First(item => item.Button == sender);
    //    FeedbackContainerControl.ClearValidationErrors(dataItem.DataControl);
    //    dataItem.DoUpdate(true);
    //    InvalidatePageCache();
    //  }
    //  catch (Exception ex)
    //  {
    //    if (dataItem != null)
    //      dataItem.Feedback.HandleException(ex);
    //  }
    //}

    //#endregion Event handlers and overrides
  }
}