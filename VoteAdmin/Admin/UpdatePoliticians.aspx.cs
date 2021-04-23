﻿using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DB.Vote;
using Vote.Controls;

namespace Vote.Admin
{
  [PageInitializers]
  public partial class UpdatePoliticiansPage : SecureAdminPage, IAllowEmptyStateCode
  {
    public override IEnumerable<string> NonStateCodesAllowed => new[] {"US"};

    #region Private

    private ManagePoliticiansPanel _ManagePoliticiansPanel;

    private void SetSubHeading()
    {
      switch (AdminPageLevel)
      {
        case AdminPageLevel.State:
          H2.InnerHtml = "Politicians for " + States.GetName(StateCode);
          break;

        case AdminPageLevel.President:
          H2.InnerHtml = "Politicians for All States";
          break;

        default:
          H2.InnerHtml = "No State Selected";
          break;
      }
    }

    #endregion Private

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsMasterUser) HandleSecurityException();

      _ManagePoliticiansPanel =
        Master.FindMainContentControl("ManagePoliticiansPanel") as ManagePoliticiansPanel;
      if (_ManagePoliticiansPanel != null)
      {
        _ManagePoliticiansPanel.Mode = ManagePoliticiansPanel.DataMode.AddPoliticians;
        _ManagePoliticiansPanel.PageFeedback = FeedbackAddCandidates;
      }

      if (!IsPostBack)
      {
        const string title = "Update Politicians";
        Title = title;
        H1.InnerHtml = title;

        SetSubHeading();

        if (AdminPageLevel != AdminPageLevel.State && AdminPageLevel != AdminPageLevel.President)
          UpdateControls.Visible = false;

        if (AdminPageLevel == AdminPageLevel.Unknown)
        {
          NoJurisdiction.CreateStateLinks("/admin/UpdatePoliticians.aspx?state={StateCode}");
          NoJurisdiction.ShowHeadOptional(false);
          NoJurisdiction.Visible = true;
        }
        else
        {
          ChangeState.Items.Clear();
          ChangeState.Items.Add(new ListItem { Text = "All States", Value = "US" });
          foreach (var stateCode in StateCache.All51StateCodes)
            ChangeState.Items.Add(new ListItem { Text =
              $"{stateCode} - {StateCache.GetStateName(stateCode)}", Value = stateCode });
          ChangeState.SetValue(QueryState);
        }
      }
    }

    #endregion Event handlers and overrides
  }
}