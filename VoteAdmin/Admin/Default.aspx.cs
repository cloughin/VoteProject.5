using System;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote.Admin
{
  public partial class Default : SecureAdminPage, IAllowEmptyStateCode
  {
    #region legacy

    private static string Ok(string msg)
    {
      return $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";
    }

    private static string Fail(string msg)
    {
      return $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";
    }

    #endregion legacy

    #region Private

    private void LoadCountyLinks()
    {
      if (IsMasterUser || IsStateAdminUser)
      {
        CountyLinkContainer.Visible = true;
        CountyLinks.AddDefaultCountyLinks(CountyLinkList, StateCode);
      }
    }

    private void LoadLocalLinks()
    {
      if (IsMasterUser || IsStateAdminUser || IsCountyAdminUser)
      {
        LocalLinksContainer.Visible = true;
        LocalLinks.AddDefaultLocalLinks(LocalLinkList, StateCode, CountyCode);
      }
    }

    private void RecordElectionStatusData()
    {
      States.UpdateOfficesStatusStatewide(TextBoxOfficesStatusStatewide.Text.Trim(), StateCode);
      States.UpdateOfficesStatusJudicial(TextBoxOfficesStatusJudicial.Text.Trim(), StateCode);
      States.UpdateOfficesStatusCounties(TextBoxOfficesStatusCounties.Text.Trim(), StateCode);
      Msg.Text = Ok("The Election Authority Data was Recorded.");
    }

    #endregion Private

    #region Event handlers and overrides

    private void EnableAddDeleteLocals()
    {
      AddOrDeleteDistricts.Visible = true;
      AddOrDeleteDistricts.NavigateUrl =
        GetUpdateJurisdictionsPageUrl(StateCode, CountyCode);
    }

    protected void ButtonRecordOfficesStaus_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(TextBoxOfficesStatusStatewide,
          TextBoxOfficesStatusJudicial, TextBoxOfficesStatusCounties);

        RecordElectionStatusData();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (IsLocalAdminUser) LocalMessage.Visible = true;
        if (IsNullOrWhiteSpace(StateCode))
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateStateLinks("/admin/?state={StateCode}");
          NoJurisdiction.ShowHeadOptional(false);
          NoJurisdiction.Visible = true;
          return;
        }

        try
        {
          CountyLinkContainer.Visible = false;
          TableParties.Visible = false;
          TableBallotDesign.Visible = false;
          LocalLinksContainer.Visible = false;
          AddOrDeleteDistricts.Visible = false;
          TableNotes.Visible = false;

          MasterOnlySection.Visible = IsMasterUser;

          var pageTitle = Empty;

          if (StateCache.IsValidStateCode(StateCode))
          {
            pageTitle = Offices.GetElectoralClassDescription(StateCode,
              CountyCode, LocalKey);

            // Counties & Locals Links
            switch (Offices.GetElectoralClass(StateCode, CountyCode, LocalKey))
            {
              case ElectoralClass.USPresident:
              case ElectoralClass.USSenate:
              case ElectoralClass.USHouse:
              case ElectoralClass.USGovernors:
                break;

              case ElectoralClass.State:
                LoadCountyLinks();
                TableBallotDesign.Visible = true;

                if (IsMasterUser)
                  TableParties.Visible = true;

                break;

              case ElectoralClass.County:
              case ElectoralClass.Local:
                LoadCountyLinks();
                LoadLocalLinks();
                EnableAddDeleteLocals();

                if (IsMasterUser)
                  TableParties.Visible = true;
                break;
            }

            if (Offices.IsElectoralClassStateCountyOrLocal(StateCode, CountyCode,
              LocalKey))
            {
              // Political Parties
              HyperLinkPoliticalParties.NavigateUrl =
                GetPartiesPageUrl(StateCode);
              HyperLinkPoliticalParties.Text =
                Offices.GetElectoralClassDescription(GetStateCode(),
                  Empty, Empty) + " Political Parties";

              // Ballot Design
              HyperLinkBallotDesign.NavigateUrl = /*Url_Admin_Ballot()*/
                GetBallotPageUrl(StateCode);
              HyperLinkBallotDesign.Text =
                Offices.GetElectoralClassDescription(StateCode, CountyCode,
                  LocalKey) + " Ballot Design and Content";
            }
          }
          else
            switch (StateCode)
            {
              case "US":
                pageTitle = Elections.GetElectionTypeDescription("A", "US");
                break;

              case "PP":
                pageTitle = Elections.GetElectionTypeDescription("A", "PP");
                break;

              default:
                if (StateCache.IsValidFederalCode(StateCode) &&
                  !StateCache.IsUS(StateCode))
                {
                  switch (StateCode)
                  {
                    case "U1":
                      pageTitle =
                        "U.S. President or State-By-State Elections";
                      break;
                    case "U2":
                      pageTitle =
                        "U.S. Senate Officials or State-By-State Elections";
                      break;
                    case "U3":
                      pageTitle =
                        "U.S. House of Representatives Officials or State-By-State Elections";
                      break;
                    case "U4":
                      pageTitle =
                        "Current Governors or State-By-State Elections";
                      break;
                  }
                }
                break;
            }
          Page.Title = H1.InnerHtml = pageTitle + " Administration";
          if (AdminPageLevel == AdminPageLevel.Local)
            FormatOtherCountiesMessage(MuliCountyMessage);

          if (IsSuperUser && StateCache.IsValidStateCode(StateCode))
          {
            // Master Only
            TableMasterOnly.Visible = true;

            if (Offices.GetElectoralClass(StateCode, CountyCode, LocalKey) ==
              ElectoralClass.State)
            {
              // Status Notes of Statewide, Judicial and County Elected Offices and Incumbents
              TableNotes.Visible = true;
              TextBoxOfficesStatusStatewide.Text =
                States.GetOfficesStatusStatewide(StateCode);
              TextBoxOfficesStatusJudicial.Text =
                States.GetOfficesStatusJudicial(StateCode);
              TextBoxOfficesStatusCounties.Text =
                States.GetOfficesStatusCounties(StateCode);
            }
          }
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          LogAdminError(ex);
        }
      }
    }

    #endregion Event handlers and overrides
  }
}