using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;

namespace Vote.Controls
{
  public partial class ElectionControl : UserControl
  {
    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static string GenerateHtml(bool isVirtual, string stateCode,
      string countyCode, string localCode)
    {
      var placeHolder = new PlaceHolder();
      Generate(placeHolder, stateCode, countyCode, localCode, isVirtual);
      return placeHolder.RenderToString();
    }

    //public static string GenerateHtml(string stateCode, string countyCode,
    //  string localCode)
    //{
    //  return GenerateHtml(true, stateCode, countyCode, localCode);
    //}

    public string Populate(string stateCode, string countyCode = "", string localCode = "") => 
      Generate(PlaceHolder, stateCode, countyCode, localCode);

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public

    #region Private

    private static string Generate(Control control, string stateCode,
      string countyCode, string localCode, bool isVirtual = true)
    {
      var firstElectionKey = string.Empty;
      var byDate = isVirtual
        ? Elections.GetVirtualElectionControlData(stateCode, countyCode, localCode)
        : Elections.GetElectionControlData(stateCode, countyCode, localCode);

      if (byDate.Count == 0)
        new HtmlDiv
        {
          InnerHtml = "No elections have been added for this jurisdiction"
        }.AddTo(
          control, "no-elections");
      else
      {
        foreach (var dateGroup in byDate)
        {
          new HtmlDiv
          {
            InnerHtml = dateGroup.Key.ToString("yyyy MMM d")
          }.AddTo(control,
            "election-date " +
            (dateGroup.Key > DateTime.UtcNow.Date ? "future" : "past"));
          foreach (var election in dateGroup)
          {
            var electionDiv = new HtmlDiv().AddTo(control,
              "election-desc");
            new LiteralControl(RemoveDateFromElectionDesc(election.ElectionDesc))
              .AddTo(electionDiv);
            new HtmlInputHidden {Value = election.ElectionKey}.AddTo(electionDiv,
              "election-key");
          }
        }
        firstElectionKey = byDate[0].First()
          .ElectionKey;
      }

      return firstElectionKey;
    }

    private static string RemoveDateFromElectionDesc(string electionDesc)
    {
      var match = Regex.Match(electionDesc, @" 20\d\d ");
      return match.Success
        ? electionDesc.Substring(match.Index + match.Length)
        : electionDesc;
    }

    #endregion Private

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #endregion Event handlers and overrides
  }
}