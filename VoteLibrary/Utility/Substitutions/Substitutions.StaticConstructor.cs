using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.HtmlControls;
using DB.Vote;
using static System.String;

namespace Vote
{
  public sealed partial class Substitutions
  {
    private static readonly int SubstitutionDisplayOrder;

    static Substitutions()
    {
      SubstitutionsDictionary =
        new Dictionary<string, Substitution>(StringComparer.OrdinalIgnoreCase)
        {
          // State Type
          {
            "[[State]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetStateName(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "The full name of the state.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateCode]]",
            new Substitution
            {
              Fn = (s, o, t) => s.StateCode,
              Requirements = Requirements.StateCode,
              HtmlDescription = "The standard two character USPS state code.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateBallotName]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetBallotStateName(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "The official name of the state as it should appear on ballots.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateContact]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetContact(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "The main state contact person.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateContactTitle]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetContactTitle(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "The title of the main state contact person.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateContactEmail]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetContactEmail(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Email address of the main state contact person.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateContactPhone]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetPhone(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "The phone number of the main state contact person.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateAltContact]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetAltContact(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "The alternate state contact person.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateAltContactTitle]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetAltContactTitle(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "The title of the alternate state contact person.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateAltContactEmail]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetAltEmail(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Email address of the alternate state contact person.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateAltContactPhone]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetAltPhone(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "The phone number of the alternate state contact person.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateAuthority]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetElectionsAuthority(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Name of the state elections authority.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateAuthorityEmail]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetEmail(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Email address of the state elections authority.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateVoteUSADomain]]",
            new Substitution
            {
              Fn = (s, o, t) => "Vote-" + s.StateCode + ".org",
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Domain name for the state Vote-USA website (like Vote-VA.org).",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[Vote-XX.org]]",
            new Substitution
            {
              Fn = (s, o, t) => "Vote-" + s.StateCode + ".org",
              Requirements = Requirements.StateCode,
              HtmlDescription = "Alias for [[StateVoteUSADomain]].",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@StateContactEmail@@",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateMailToAnchor(StateCache.GetContactEmail(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Mailto link for the main state contact person.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@StateAltContactEmail@@",
            new Substitution
            {
              Fn = (s, o, t) => CreateMailToAnchor(States.GetAltEmail(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Mailto link for the alternate state contact person.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@StateAuthorityEmail@@",
            new Substitution
            {
              Fn = (s, o, t) => CreateMailToAnchor(StateCache.GetEmail(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Mailto link for the state elections authority.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##StateAuthorityPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(StateCache.GetUri(s.StateCode),
                    StateCache.GetElectionsAuthority(s.StateCode),
                    t ?? "Website of the " + StateCache.GetStateName(s.StateCode) +
                      " Election Authority", "view"),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Hyperlink to the state elections authority.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##OfficialsPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(UrlManager.GetOfficialsPageUri(s.StateCode), t ?? $"Current {StateCache.GetStateName(s.StateCode)} Elected Representatives"),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Hyperlink to the public Officials page.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // County Type
          {
            "[[County]]",
            new Substitution
            {
              Fn = (s, o, t) => CountyCache.GetCountyName(s.StateCode, s.CountyCode),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription = "Name of the county.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CountyCode]]",
            new Substitution
            {
              Fn = (s, o, t) => s.CountyCode,
              Requirements = Requirements.CountyCode,
              HtmlDescription = "The three digit county code.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CountyContact]]",
            new Substitution
            {
              Fn = (s, o, t) => Counties.GetContact(s.StateCode, s.CountyCode),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription = "The main county contact person.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CountyContactTitle]]",
            new Substitution
            {
              Fn = (s, o, t) => Counties.GetContactTitle(s.StateCode, s.CountyCode),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription = "The title of the main county contact person.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CountyContactEmail]]",
            new Substitution
            {
              Fn = (s, o, t) => Counties.GetEmail(s.StateCode, s.CountyCode),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription = "Email address of the main county contact person.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CountyContactPhone]]",
            new Substitution
            {
              Fn = (s, o, t) => Counties.GetPhone(s.StateCode, s.CountyCode),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription =
                "The phone number of the main county contact person.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CountyAltContact]]",
            new Substitution
            {
              Fn = (s, o, t) => Counties.GetAltContact(s.StateCode, s.CountyCode),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription = "The alternate county contact person.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CountyAltContactTitle]]",
            new Substitution
            {
              Fn = (s, o, t) => Counties.GetAltContactTitle(s.StateCode, s.CountyCode),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription = "The title of the alternate county contact person.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CountyAltContactEmail]]",
            new Substitution
            {
              Fn = (s, o, t) => Counties.GetAltEmail(s.StateCode, s.CountyCode),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription =
                "Email address of the alternate county contact person.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CountyAltContactPhone]]",
            new Substitution
            {
              Fn = (s, o, t) => Counties.GetAltPhone(s.StateCode, s.CountyCode),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription =
                "The phone number of the alternate county contact person.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@CountyContactEmail@@",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateMailToAnchor(Counties.GetEmail(s.StateCode, s.CountyCode)),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription = "Mailto link for the main county contact person.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@CountyAltContactEmail@@",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateMailToAnchor(Counties.GetAltEmail(s.StateCode, s.CountyCode)),
              Requirements = Requirements.StateCode | Requirements.CountyCode,
              HtmlDescription =
                "Mailto link for the alternate county contact person.",
              Type = Type.County,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Local Type
          {
            "[[Local]]", new Substitution
            {
              Fn = (s, o, t) => 
                VotePage.GetPageCache().LocalDistricts.GetLocalDistrict(s.StateCode, s.LocalKey),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription = "Name or description of the local district.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalKey]]",
            new Substitution
            {
              Fn = (s, o, t) => s.LocalKey,
              Requirements = Requirements.LocalKey,
              HtmlDescription = "The 5 digit key for the local district.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalContact]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  LocalDistricts.GetContactByStateCodeLocalKey(s.StateCode, s.LocalKey),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription = "The local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalContactTitle]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  LocalDistricts.GetContactTitleByStateCodeLocalKey(s.StateCode, s.LocalKey),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription = "The title of the main local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalContactEmail]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  LocalDistricts.GetEmailByStateCodeLocalKey(s.StateCode, s.LocalKey),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription = "Email address of the main local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalContactPhone]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  LocalDistricts.GetPhoneByStateCodeLocalKey(s.StateCode, s.LocalKey),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription =
                "The phone number of the main local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalAltContact]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  LocalDistricts.GetAltContactByStateCodeLocalKey(s.StateCode, s.LocalKey),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription = "The alternate local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalAltContactTitle]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                    LocalDistricts.GetAltContactTitleByStateCodeLocalKey(s.StateCode, s.LocalKey),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription = "The title of the alternate local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalAltContactEmail]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                   LocalDistricts.GetAltEmailByStateCodeLocalKey(s.StateCode, s.LocalKey),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription =
                "Email address of the alternate local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalAltContactPhone]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  LocalDistricts.GetAltPhoneByStateCodeLocalKey(s.StateCode, s.LocalKey),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription =
                "The phone number of the alternate local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@LocalContactEmail@@",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                    CreateMailToAnchor(LocalDistricts.GetEmailByStateCodeLocalKey(s.StateCode, s.LocalKey)),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription = "Mailto link for the main local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@LocalAltContactEmail@@",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                    CreateMailToAnchor(LocalDistricts.GetAltEmailByStateCodeLocalKey(s.StateCode, s.LocalKey)),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalKey,
              HtmlDescription =
                "Mailto link for the alternate local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Election Type

          {
            "[[ElectionDate()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  FormatDate(Elections.GetElectionDateFromKey(s.ElectionKey), o),
              Requirements = Requirements.ElectionKey,
              OptionTypes = OptionTypes.Date,
              HtmlDescription =
                "Date of the election based on the ElectionKey, formatted according to the Date option.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[ElectionKey]]",
            new Substitution
            {
              Fn = (s, o, t) => s.ElectionKey,
              Requirements = Requirements.ElectionKey,
              HtmlDescription =
                "The Election Key, for building URL links.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[ElectionDesc]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Elections.GetElectionDesc(s.ElectionKey),
              Requirements = Requirements.ElectionKey,
              HtmlDescription =
                "Description of the election based on the ElectionKey.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PrimariesList()]]",
            new Substitution
            {
              Fn = (s, o, t) => Join(Elections.GetElectionsOnSameDate(s.ElectionKey)
                .Select(row => row.ElectionDesc), o),
              Requirements = Requirements.ElectionKey,
              OptionTypes = OptionTypes.List,
              HtmlDescription =
                "List of all elections on the same date as the supplied ElectionKey, " +
                  "separated by according to the List option.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LatestElectionDate()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  FormatDate(
                    Elections.GetElectionDateForSubstitutions(s.StateCode, o), o),
              Requirements = Requirements.StateCode,
              OptionTypes =
                OptionTypes.Election | OptionTypes.Viewability | OptionTypes.Date |
                  OptionTypes.PastFuture,
              HtmlDescription =
                "Date of the requested election for the state, formatted according" +
                  " to the Date option. The type or types of election, whether it is" +
                  " viewable, and whether it is past or future are controlled by" +
                  " the options. If no qualifying election is found, the result" +
                  " is empty.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LatestElectionDesc()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  Elections.GetElectionDescriptionForSubstitutions(s.StateCode, o),
              Requirements = Requirements.StateCode,
              OptionTypes =
                OptionTypes.Election | OptionTypes.Viewability |
                  OptionTypes.PastFuture,
              HtmlDescription =
                "Description of the requested election for the state. The type or types of election, whether it is" +
                  " viewable, and whether it is past or future are controlled by" +
                  " the options. If no qualifying election is found, the result" +
                  " is empty.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LatestMajorPartyPrimaryDate()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  FormatDate(
                    Elections.GetElectionDateForSubstitutions(s.StateCode, o,
                      s.GetMajorPartyCodeWithOption(o)), o),
              Requirements = Requirements.StateCode,
              OptionTypes =
                OptionTypes.Viewability | OptionTypes.PastFuture | OptionTypes.Date |
                  OptionTypes.MajorParty,
              HtmlDescription =
                "Date of the requested major party primary for the state, formatted" +
                  " according to the Date option. The major party, whether it is" +
                  " viewable, and whether it is past or future are controlled by" +
                  " the options. If no qualifying election is found, the result" +
                  " is empty.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LatestMajorPartyPrimaryDesc()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  Elections.GetElectionDescriptionForSubstitutions(s.StateCode, o,
                    s.GetMajorPartyCodeWithOption(o)),
              Requirements = Requirements.StateCode,
              OptionTypes =
                OptionTypes.Viewability | OptionTypes.PastFuture |
                  OptionTypes.MajorParty,
              HtmlDescription =
                "Description of the requested major party primary  for the state. The major party, whether it is" +
                  " viewable, and whether it is past or future are controlled by" +
                  " the options. If no qualifying election is found, the result" +
                  " is empty.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LatestPrimariesList()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  Join(
                    Elections.GetPrimaryElectionDescriptionsForSubstitutions(
                      s.StateCode, o), o),
              Requirements = Requirements.StateCode,
              OptionTypes =
                OptionTypes.List | OptionTypes.Viewability | OptionTypes.PastFuture,
              HtmlDescription =
                "List of all primaries on the same date as the request for the state." +
                  " Whether the election is viewable, and whether it is past or future are controlled by" +
                  " the options. If no qualifying elections are found, the result" +
                  " is empty. The list is separated by according to the List option.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[FutureElectionsList()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  Join(
                    Elections.GetFutureElectionDescriptionsForSubstitutions(
                      s.StateCode, o), o),
              Requirements = Requirements.StateCode,
              OptionTypes =
                OptionTypes.List | OptionTypes.Election | OptionTypes.Viewability,
              HtmlDescription =
                "List of all future elections for the state." +
                  " The type or types of elections and whether the elections are viewable is controlled by" +
                  " the options. If no qualifying elections are found, the result" +
                  " is empty. The list is separated by according to the List option.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##ElectionPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    UrlManager.GetElectionPageUri(
                      Elections.GetStateCodeFromKey(s.ElectionKey), s.ElectionKey),
                    t ?? s.PageCache.Elections.GetElectionDesc(s.ElectionKey)),
              Requirements = Requirements.ElectionKey,
              HtmlDescription = "Hyperlink to the public Election page.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##ElectionPageLongList##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    UrlManager.GetElectionPageUri(
                      Elections.GetStateCodeFromKey(s.ElectionKey), s.ElectionKey) + "&openall=Y",
                    t ?? s.PageCache.Elections.GetElectionDesc(s.ElectionKey)),
              Requirements = Requirements.ElectionKey,
              HtmlDescription = "Hyperlink to the public Election page with all accordions opened.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PrimariesElectionPageList()##",
            new Substitution
            {
              Fn = (s, o, t) => Join(Elections.GetElectionsOnSameDate(s.ElectionKey)
                .Select(
                  row =>
                    CreateWebAnchor(
                      UrlManager.GetElectionPageUri(
                        Elections.GetStateCodeFromKey(row.ElectionKey),
                        row.ElectionKey), t ?? row.ElectionDesc)), o),
              Requirements = Requirements.ElectionKey,
              OptionTypes = OptionTypes.List,
              HtmlDescription =
                "List of hyperlinks to the public Election page for all elections" +
                  " on the same date as the supplied ElectionKey, separated" +
                  " according to the List option. Intended for primaries.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PrimariesElectionPageLongList()##",
            new Substitution
            {
              Fn = (s, o, t) => Join(Elections.GetElectionsOnSameDate(s.ElectionKey)
                .Select(
                  row =>
                    CreateWebAnchor(
                      UrlManager.GetElectionPageUri(
                        Elections.GetStateCodeFromKey(row.ElectionKey),
                        row.ElectionKey, true), t ?? row.ElectionDesc)), o),
              Requirements = Requirements.ElectionKey,
              OptionTypes = OptionTypes.List,
              HtmlDescription =
                "List of hyperlinks to the public Election page for all elections" +
                  " on the same date as the supplied ElectionKey with all accordions opened, separated" +
                  " according to the List option. Intended for primaries.",
              Type = Type.Election,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Office Type
          {
            "[[Office]]",
            new Substitution
            {
              Fn = (s, o, t) => s.GetLocalizedOfficeName(s.OfficeKey),
              Requirements = Requirements.OfficeKey,
              HtmlDescription =
                "Description of the office, including jurisdiction information for " +
                  "county and local offices.",
              Type = Type.Office,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OfficeKey]]",
            new Substitution
            {
              Fn = (s, o, t) => s.OfficeKey,
              Requirements = Requirements.OfficeKey,
              HtmlDescription =
                "The Office Key, for building URL links.",
              Type = Type.Office,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OfficeShort]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  Offices.FormatOfficeName(
                    s.PageCache.Offices.GetOfficeLine1(s.OfficeKey),
                    s.PageCache.Offices.GetOfficeLine2(s.OfficeKey), s.OfficeKey),
              Requirements = Requirements.OfficeKey,
              HtmlDescription =
                "Description of the office without jurisdiction information for " +
                  "county and local offices.",
              Type = Type.Office,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CandidateCount]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  ElectionsPoliticians.GetPoliticianCountForOfficeInElection(
                    s.ElectionKey, s.OfficeKey)
                    .ToString(CultureInfo.InvariantCulture),
              Requirements = Requirements.ElectionKey | Requirements.OfficeKey,
              HtmlDescription = "Number of candidates running for the office.",
              Type = Type.Office,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[CandidateList()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  Join(
                    ElectionsPoliticians.GetPoliticiansForOfficeInElection(
                        s.ElectionKey, s.OfficeKey)
                      .Rows.OfType<DataRow>()
                      .Select(row => Politicians.FormatName(row)), o),
              Requirements = Requirements.ElectionKey | Requirements.OfficeKey,
              OptionTypes = OptionTypes.List,
              HtmlDescription =
                "A list of all candidates for the office in the election, separated according" +
                " to the List option.",
              Type = Type.Office,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[AdRate]]",
            new Substitution
            {
              Fn = delegate(Substitutions s, Options o, string t)
              {
                var adRate = Offices.GetAdRate(s.ElectionKey, s.OfficeKey);
                if (s.AdDiscount != null)
                {
                  var discounted = adRate * (1 - s.AdDiscount.Value);
                  decimal round;
                  if (discounted <= 50) round = 5;
                  else if (discounted <= 100) round = 10;
                  else if (discounted <= 2000) round = 25;
                  else round = 100;
                  adRate = Math.Truncate((discounted + round - 1) / round) * round;
                }
                var format = adRate % 1.0m == 0 ? "$#,#" : "C";
                return adRate
                  .ToString(format, CultureInfo.CreateSpecificCulture("en-US"));
              },
              Requirements = Requirements.ElectionKey | Requirements.OfficeKey,
              HtmlDescription =
                "The add rate for this office and election, formatted as xxx.xx. The rate" +
                " can be discounted. For example, if you include [[AdDiscount=33%]] in your" +
                " template, the predefined rate will be discounted by 33%.",
              Type = Type.Office,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##CompareCandidatesPage##",
            new Substitution
            {
              Fn = (s, o, t) => CreateWebAnchor(
                UrlManager.GetCompareCandidatesPageUri(s.StateCode, s.ElectionKey,
                  s.OfficeKey),
                t ??
                $"Candidates for {Offices.FormatOfficeName(s.PageCache.Offices.GetOfficeLine1(s.OfficeKey), s.PageCache.Offices.GetOfficeLine2(s.OfficeKey), s.OfficeKey)}, {s.PageCache.Elections.GetElectionDesc(s.ElectionKey)}"),
              Requirements =
                Requirements.StateCode | Requirements.ElectionKey |
                Requirements.OfficeKey,
              HtmlDescription = "Hyperlink to the public Compare Candidates page.",
              Type = Type.Office,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Politician Type
          {
            "[[Politician]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Politicians.GetPoliticianName(s.PoliticianKey),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "Full formatted name of the politician.",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PoliticianFirstName]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Politicians.GetFirstName(s.PoliticianKey),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "The first name of the politician.",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PoliticianLastName]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Politicians.GetLastName(s.PoliticianKey),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "Last name of the politician.",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PoliticianKey]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PoliticianKey,
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "The politician key.",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PoliticianUserName]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PoliticianKey,
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "Alias for [[PoliticianKey]].",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PoliticianPassword]]",
            new Substitution
            {
              Fn = (s, o, t) => Politicians.GetPassword(s.PoliticianKey),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "Password for the politician.",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PoliticianSigninDate()]]",
            new Substitution
            {
              Fn = (s, o, t) => 
                FormatDate(Politicians.GetLatestSignin(s.PoliticianKey) ?? VotePage.DefaultDbDate, o),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "Latest sign in date for the politician.",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PoliticianIntroPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(UrlManager.GetIntroPageUri(s.PoliticianKey), t ?? $"Candidate Page for {s.PageCache.Politicians.GetPoliticianName(s.PoliticianKey)}"),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription =
                "Hyperlink to the public Intro page for the politician.",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##SecurePoliticianPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(UrlManager.GetAdminUri("/Politician"), t ?? "Vote-USA candidate sign in page"),
              Requirements = Requirements.None,
              HtmlDescription = "Hyperlink to the secure Politician page.",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PoliticianUnsubscribe##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  AddStyleToTag(CreateWebAnchor(
                    Format("https://" +
                      UrlManager.SiteHostNameAndPort +
                      "/subscribe.aspx?key={0}&op=unpol",
                      s.PoliticianKey),
                    t ?? "Unsubscribe from Vote-USA candidate emails"), "font-family:arial;font-size:8pt;color:#aaa"),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription =
                "Link to the subscribe page that says \"Unsubscribe from Vote-USA candidate emails\", styled for use in an email footer",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PoliticianYouTube##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                {
                  var youTubeUrl = Politicians.GetYouTubeWebAddress(s.PoliticianKey);
                  return IsNullOrEmpty(youTubeUrl)
                    ? Empty
                    : CreateWebAnchor(youTubeUrl, t ?? $"YouTube video for {s.PageCache.Politicians.GetPoliticianName(s.PoliticianKey)}");
                },
              Requirements = Requirements.ElectionKey, 
              HtmlDescription =
                "YouTube video or channel URL for the politician",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PoliticianAdYouTube##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                {
                  var youTubeAdUrl = ElectionsPoliticians.GetAdUrl(s.ElectionKey, s.OfficeKey, s.PoliticianKey);
                  return CreateWebAnchor(youTubeAdUrl, t ?? $"YouTube ad video for {s.PageCache.Politicians.GetPoliticianName(s.PoliticianKey)}");
                },
              Requirements = Requirements.PoliticianKey | Requirements.OfficeKey | Requirements.PoliticianKey,
              HtmlDescription =
                "YouTube video used in candidate's ad",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Issue Type
          {
            "[[Issue]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Issues.GetIssue(s.IssueKey),
              Requirements = Requirements.IssueKey,
              HtmlDescription = "Description of the issue.",
              Type = Type.Issue,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Party Type
          {
            "[[Party]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Parties.GetPartyName(s.PartyKey),
              Requirements = Requirements.PartyKey,
              HtmlDescription = "Name of the party.",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PartyCode]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Parties.GetPartyCode(s.PartyKey),
              Requirements = Requirements.PartyKey,
              HtmlDescription =
                "Party code (excluding the state code) for the party",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PartyKey]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PartyKey,
              Requirements = Requirements.PartyKey,
              HtmlDescription =
                "The party key, which includes the state code for state parties.",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PartyEmail]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PartyEmail,
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "Email address of the party contact.",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PartyEmailAsText]]",
            new Substitution
            {
              Fn = (s, o, t) => "<span style=\"display:none\">@</span>" + s.PartyEmail,
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "Email address of the party contact that will not render as a link in Outlook.",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PartyPassword]]",
            new Substitution
            {
              Fn = (s, o, t) => PartiesEmails.GetPartyPassword(s.PartyEmail),
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "Password for the party contact.",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PartyContactFirstName]]",
            new Substitution
            {
              Fn = (s, o, t) => PartiesEmails.GetPartyContactFirstName(s.PartyEmail),
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "First name of the party contact.",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PartyContactLastName]]",
            new Substitution
            {
              Fn = (s, o, t) => PartiesEmails.GetPartyContactLastName(s.PartyEmail),
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "Last name of the party contact.",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PartyContactTitle]]",
            new Substitution
            {
              Fn = (s, o, t) => PartiesEmails.GetPartyContactTitle(s.PartyEmail),
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "Title of the party contact.",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[PartyContactPhone]]",
            new Substitution
            {
              Fn = (s, o, t) => PartiesEmails.GetPartyContactPhone(s.PartyEmail),
              Requirements = Requirements.PartyEmail,
              HtmlDescription =
                "The phone number of the party contact.",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##SecurePartyPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(UrlManager.GetAdminUri("/Party"), t ?? "Vote-USA party sign in page"),
              Requirements = Requirements.None,
              HtmlDescription = "Hyperlink to the secure Party page.",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PartyUnsubscribe##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  AddStyleToTag(CreateWebAnchor(
                    Format("https://" +
                      UrlManager.SiteHostNameAndPort +
                        "/subscribe.aspx?email={0}&op=unpty",
                      s.PartyEmail),
                    t ?? "Unsubscribe from Vote-USA party-related emails"), "font-family:arial;font-size:8pt;color:#aaa"),
              Requirements = Requirements.PartyEmail,
              HtmlDescription =
                "Link to the subscribe page that says \"Unsubscribe from Vote-USA party-related emails\", styled for use in an email footer",
              Type = Type.Party,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Volunteer Tyoe
          {
            "[[VolunteerFirstName]]",
            new Substitution
            {
              Fn = (s, o, t) => VolunteersView.GetFirstName(s.PartyEmail),
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "First name of the volunteer.",
              Type = Type.Volunteer,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VolunteerLastName]]",
            new Substitution
            {
              Fn = (s, o, t) => VolunteersView.GetLastName(s.PartyEmail),
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "Last name of the volunteer.",
              Type = Type.Volunteer,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VolunteerPhone]]",
            new Substitution
            {
              Fn = (s, o, t) => VolunteersView.GetPhone(s.PartyEmail),
              Requirements = Requirements.PartyEmail,
              HtmlDescription =
                "The phone number of the volunteer.",
              Type = Type.Volunteer,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VolunteerStateCode]]",
            new Substitution
            {
              Fn = (s, o, t) => VolunteersView.GetStateCode(s.PartyEmail).SafeString(),
              Requirements = Requirements.PartyEmail,
              HtmlDescription =
                "The state code of the volunteer.",
              Type = Type.Volunteer,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VolunteerState]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetStateName(VolunteersView.GetStateCode(s.PartyEmail)).SafeString(),
              Requirements = Requirements.PartyEmail,
              HtmlDescription =
                "The state name of the volunteer.",
              Type = Type.Volunteer,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VolunteerParty]]",
            new Substitution
            {
              Fn = (s, o, t) => VolunteersView.GetPartyName(s.PartyEmail).SafeString(),
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "The volunteer's political party (blank if none).",
              Type = Type.Volunteer,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VolunteerEmail]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PartyEmail,
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "Email address of the volunteer.",
              Type = Type.Volunteer,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VolunteerEmailAsText]]",
            new Substitution
            {
              Fn = (s, o, t) => "<span style=\"display:none\">@</span>" + s.PartyEmail,
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "Email address of the volunteer that will not render as a link in Outlook.",
              Type = Type.Volunteer,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VolunteerPassword]]",
            new Substitution
            {
              Fn = (s, o, t) => VolunteersView.GetPassword(s.PartyEmail),
              Requirements = Requirements.PartyEmail,
              HtmlDescription = "Password for the volunteer.",
              Type = Type.Volunteer,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VolunteerDate()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  FormatDate(VolunteersView.GetDateStamp(s.PartyEmail, VotePage.DefaultDbDate), o),
              Requirements = Requirements.PartyEmail,
              OptionTypes = OptionTypes.Date,
              HtmlDescription =
                "Date the volunteer was added, formatted according to the Date option.",
              Type = Type.Volunteer,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Website Visitor Type
          {
            "[[VisitorFirstName]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Addresses.GetFirstName(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's first name.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorLastName]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Addresses.GetLastName(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's last name.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorAddress]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Addresses.GetAddress(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's address.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorCity]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Addresses.GetCity(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's city.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorZipCode]]", new Substitution
            {
              Fn = (s, o, t) =>
              {
                var zip = s.PageCache.Addresses.GetZip5(s.VisitorId);
                if (!IsNullOrEmpty(zip))
                {
                  var zip4 = s.PageCache.Addresses.GetZip4(s.VisitorId);
                  if (!IsNullOrWhiteSpace(zip4)) zip += "-" + zip4;
                }
                return zip;
              },
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's zip code.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorDate()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  FormatDate(s.PageCache.Addresses.GetDateStamp(s.VisitorId), o),
              Requirements = Requirements.VisitorId,
              OptionTypes = OptionTypes.Date,
              HtmlDescription =
                "Date the visitor registered on the Vote-USA web site, formatted" +
                  " according to the Date option.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorCongressionalDistrictCode]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Addresses.GetCongressionalDistrict(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's US house district code.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorCongressionalDistrict]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Addresses.GetCongressionalDistrictDesc(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's US house district.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorStateSenateDistrictCode]]",
            new Substitution
            {
              Fn =
                (s, o, t) => s.PageCache.Addresses.GetStateSenateDistrict(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's state senate district code.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorStateSenateDistrict]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Addresses.GetStateSenateDistrictDesc(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's state senate district.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorStateHouseDistrictCode]]",
            new Substitution
            {
              Fn =
                (s, o, t) => s.PageCache.Addresses.GetStateHouseDistrict(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's state house district code.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorStateHouseDistrict]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Addresses.GetStateHouseDistrictDesc(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's state house district.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorSourceCode]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Addresses.GetSourceCode(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "A code indicating the source of the visitor information.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorSendSampleBallots]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Addresses.GetSendSampleBallots(s.VisitorId)
                    ? "yes"
                    : "no",
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Whether the visitor requested to be sent ballot choices (yes or no).",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VisitorSubscribeToSampleBallots##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    Format("https://" +
                      UrlManager.SiteHostNameAndPort +
                        "/subscribe.aspx?email={0}&code={1}&op=ballots",
                      s.GetAdHocSubstitution("[[ContactEmail]]"), s.VisitorId),
                    t ?? "Subscribe to future ballot choices via email"),
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Link to the subscribe page that says \"Subscribe to future ballot choices via email\".",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VisitorUnsubscribe##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  AddStyleToTag(CreateWebAnchor(
                    Format("https://" +
                      UrlManager.SiteHostNameAndPort +
                      "/subscribe.aspx?email={0}&code={1}&op=unsubscribe",
                      s.GetAdHocSubstitution("[[ContactEmail]]"), s.VisitorId),
                    t ?? "Unsubscribe from Vote-USA email"), "font-family:arial;font-size:8pt;color:#aaa"),
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Link to the subscribe page that says \"Unsubscribe from Vote-USA email\", styled for use in an email footer",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VisitorUnsubscribeInline##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    Format("https://" +
                      UrlManager.SiteHostNameAndPort +
                      "/subscribe.aspx?email={0}&code={1}&op=unsubscribe",
                      s.GetAdHocSubstitution("[[ContactEmail]]"), s.VisitorId),
                    t ?? "Unsubscribe from Vote-USA email"),
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Link to the subscribe page that says \"Unsubscribe from Vote-USA email\", unstyled for use in the email body",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorSampleBallotCount()]]", new Substitution
            {
              Fn = (s, o, t) =>
              {
                if (
                  IsNullOrWhiteSpace(
                    s.PageCache.Addresses.GetCongressionalDistrict(s.VisitorId))) return "0";
                return
                    Elections.GetElectionsForSampleBallotsSubstitutions(s.StateCode, o)
                    .Count()
                    .ToString(CultureInfo.InvariantCulture);
              },
              OptionTypes = OptionTypes.PastFuture,
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Count of links to the ballot choices page for the visitor. " +
                  "Only viewable elections are included. " +
                  "The result will be zero or one unless this is a primary. " +
                  "Returns zero if the visitor has no district coding.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VisitorSampleBallotPage()##", new Substitution
            {
              Fn = (s, o, t) =>
              {
                if (
                  IsNullOrWhiteSpace(
                    s.PageCache.Addresses.GetCongressionalDistrict(s.VisitorId)))
                  throw new VoteSubstitutionException("Visitor does not have district coding (##VisitorSampleBallotPage()##)");
                var c = s.PageCache.Addresses;
                var elections =
                  Elections.GetElectionsForSampleBallotsSubstitutions(s.StateCode, o)
                    .Where(i => Elections.HasContestsOrBallotMeasures(i.Value,
                      c.GetCongressionalDistrict(s.VisitorId),
                      c.GetStateSenateDistrict(s.VisitorId),
                      c.GetStateHouseDistrict(s.VisitorId),
                      c.GetCountyCode(s.VisitorId),
                      c.GetDistrict(s.VisitorId),
                      c.GetPlace(s.VisitorId),
                      c.GetElementary(s.VisitorId),
                      c.GetSecondary(s.VisitorId),
                      c.GetUnified(s.VisitorId),
                      c.GetCityCouncil(s.VisitorId),
                      c.GetCountySupervisors(s.VisitorId),
                      c.GetSchoolDistrictDistrict(s.VisitorId)))
                    .ToList();
                if (!elections.Any())
                  throw new VoteSubstitutionException("There are no contests or ballot measures for this visitor (##VisitorSampleBallotPage()##)");
                return
                  Join(
                    elections
                      .Select(
                        i =>
                          CreateWebAnchor(
                            UrlManager.GetBallotPageUri(i.Value,
                              c.GetCongressionalDistrict(
                                s.VisitorId),
                              c.GetStateSenateDistrict(s.VisitorId),
                              c.GetStateHouseDistrict(s.VisitorId),
                              c.GetCountyCode(s.VisitorId),
                              c.GetDistrict(s.VisitorId),
                              c.GetPlace(s.VisitorId),
                              c.GetElementary(s.VisitorId),
                              c.GetSecondary(s.VisitorId),
                              c.GetUnified(s.VisitorId),
                              c.GetCityCouncil(s.VisitorId),
                              c.GetCountySupervisors(s.VisitorId),
                              c.GetSchoolDistrictDistrict(s.VisitorId),
                              s.GetAdHocSubstitution("[[ContactEmail]]")), t ?? i.Text)), o);
              },
              OptionTypes = OptionTypes.List | OptionTypes.PastFuture,
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "List of links to the ballot choices page for the visitor. " +
                  "Only viewable elections are included. " +
                  "There will only be one link in the list unless this is a primary. " +
                  "This substitution will cause the email to fail if the visitor has no " +
                  "district coding or there are no elections with at least one contest or ballot " +
                  "measure for the visitor.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Donor Type
          {
            "[[DonorFirstName]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Donations.GetFirstName(s.DonorId),
              Requirements = Requirements.DonorId,
              HtmlDescription = "The donor's first name from the donation receipt. Could be different from [[VisitorFirstName]].",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[DonorLastName]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Donations.GetLastName(s.DonorId),
              Requirements = Requirements.DonorId,
              HtmlDescription = "The donor's last name from the donation receipt. Could be different from [[VisitorLastName]].",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[DonorAddress]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Donations.GetAddress(s.DonorId),
              Requirements = Requirements.DonorId,
              HtmlDescription = "The donor's address from the donation receipt. Could be different from [[VisitorAddress]].",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[DonorCity]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Donations.GetCity(s.DonorId),
              Requirements = Requirements.DonorId,
              HtmlDescription = "The donor's city from the donation receipt. Could be different from [[VisitorCity]].",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[DonorZipCode]]", new Substitution
            {
              Fn = (s, o, t) =>
              {
                var zip = s.PageCache.Donations.GetZip5(s.DonorId);
                if (!IsNullOrEmpty(zip))
                {
                  var zip4 = s.PageCache.Donations.GetZip4(s.DonorId);
                  if (!IsNullOrWhiteSpace(zip4)) zip += "-" + zip4;
                }
                return zip;
              },
              Requirements = Requirements.DonorId,
              HtmlDescription = "The donor's zip code from the donation receipt. Could be different from [[VisitorZipCode]].",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[DonorPhone]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Donations.GetPhone(s.DonorId),
              Requirements = Requirements.DonorId,
              HtmlDescription = "The donor's phone from the donation receipt.",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[DonorDate()]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  FormatDate(s.PageCache.Donations.GetDate(s.DonorId), o),
              Requirements = Requirements.DonorId,
              OptionTypes = OptionTypes.Date,
              HtmlDescription =
                "Date of the latest donation for this donor, formatted" +
                  " according to the Date option.",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[DonorAmount]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Donations.GetAmount(s.DonorId).ToString("C"),
              Requirements = Requirements.DonorId,
              HtmlDescription = "The amount of the latest donation for this donor.",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[DonorTotal]]",
            new Substitution
            {
              Fn = (s, o, t) => Donations.GetTotalAmountByEmail(s.PageCache.Donations.GetEmail(s.DonorId)).ToString("C"),
              Requirements = Requirements.DonorId,
              HtmlDescription = "The total donation amount for this donor.",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##DonorUnsubscribe##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  AddStyleToTag(CreateWebAnchor(
                    Format("https://" +
                      UrlManager.SiteHostNameAndPort +
                        "/subscribe.aspx?email={0}&op=undnr",
                      s.PageCache.Donations.GetEmail(s.DonorId)),
                    t ?? "Unsubscribe from Vote-USA emails"), "font-family:arial;font-size:8pt;color:#aaa"),
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Link to the subscribe page that says \"Unsubscribe from Vote-USA emails\", styled for use in an email footer",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Organizations
          {
            "[[Organization]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetName(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "Name of the organization",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationShortName]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetOrgAbbreviation(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "Short name of the organization",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationType]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetOrgType(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "Type of the organization",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationSubType]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetOrgSubType(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "Sub type of the organization",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationIdeology]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetIdeology(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "Ideology of the organization",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationContact]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetContact(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "Name of the organization contact",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationAddressLine1]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetAddress1(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "First line of the organization address",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationAddressLine2]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetAddress2(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "Second line of the organization address",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationCity]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetCity(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "City of the organization",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationState]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetStateName(s.PageCache.OrgContacts.GetContact(s.OrgContactId)) ?? Empty,
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "State name of the organization",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationStateCode]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetStateCode(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "State code of the organization",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OrganizationZip]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.OrgContacts.GetZip(s.OrgContactId),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "Zip code of the organization",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@OrganizationContactEmail@@",
            new Substitution
            {
              Fn = (s, o, t) => CreateMailToAnchor(s.PageCache.OrgContacts.GetEmail(s.OrgContactId)),
              Requirements = Requirements.OrgContactId,
              HtmlDescription = "Email of the organization contact",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PublicInterestOrganizationsPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor($"{UrlManager.SiteHostNameAndPort}/PublicInterestOrganizations.aspx", t ?? "Vote-USA | Public Interest Organizations"),
              Requirements = Requirements.None,
              HtmlDescription = "URL of the PublicInterestOrganizations page",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##OrganizationUnsubscribe##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  AddStyleToTag(CreateWebAnchor(
                    Format("https://" +
                      UrlManager.SiteHostNameAndPort +
                      "/subscribe.aspx?email={0}&op=unorg",
                      s.PageCache.OrgContacts.GetEmail(s.OrgContactId)),
                    t ?? "Unsubscribe from Vote-USA emails"), "font-family:arial;font-size:8pt;color:#aaa"),
              Requirements = Requirements.OrgContactId,
              HtmlDescription =
                "Link to the subscribe page that says \"Unsubscribe from Vote-USA emails\", styled for use in an email footer",
              Type = Type.Organization,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Miscellaneous Type
          {
            "[[Image]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                {
                  var path = $"/image/{t}{UploadedImages.GetImageTypeByExternalName(t, Empty)}";
                  return $"<img src=\"{UrlManager.GetSiteUri(path)}\"/>";
                },
              Requirements = Requirements.None,
              HtmlDescription =
                "Insert an uploaded image. Specify the image external name in curly bracket without a file extension, i.e., [[Image{&lt;name&gt;}]].",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##DonationImage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateImageAnchor(
                    VotePage.DonateUrl,
                    "vote-usa.org/images/donateboxemail.png", "Donate to Vote-USA"),
              Requirements = Requirements.None,
              HtmlDescription =
                "Image link to the donation page that says \"Your tax-deductible donations are Our Sole Support. Donations do not support any particular candidate. Donate Today.\"",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##DonationText##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    VotePage.DonateUrl,
                    t ?? "100% tax-deductible donation", "Donate to Vote-USA"),
              Requirements = Requirements.None,
              HtmlDescription =
                "Text link to the donation page that says \"100% tax-deductible donation\".",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PaymentText##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    VotePage.DonateUrl,
                    t ?? "Make Payment", "Make a payment to Vote-USA"),
              Requirements = Requirements.None,
              HtmlDescription =
                "Text link to the donation page that says \"Make Payment\".",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##AdSamples##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  new HtmlImage {Src = "https://vote-usa.org/images/AdSamples.png"}.RenderToString(),
              Requirements = Requirements.None,
              HtmlDescription =
                "Image of the sample ads.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[ToEmailHidden]]",
            new Substitution
            {
              Fn = (s, o, t) =>
              {
                var div = new HtmlDiv {InnerText = s.Substitute("[[contactemail]]").Replace("@", "#")};
                div.Style.Add("color", "#ffffff");
                div.Style.Add("font-size", "4pt");
                return div.RenderToString();
              },
              Requirements = Requirements.None,
              HtmlDescription =
                "Recipient email address with # for @ in white, for tracking redacted email addresses.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[EmailIdHidden]]",
            new Substitution
            {
              Fn = (s, o, t) =>
              {
                var div = new HtmlDiv { InnerText = $"*{s.SourceCode}*" };
                div.Style.Add("color", "#ffffff");
                div.Style.Add("font-size", "4pt");
                return div.RenderToString();
              },
              Requirements = Requirements.None,
              HtmlDescription =
                "An Id that allows us to recover the email address from a spam report",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[Today()]]",
            new Substitution
            {
              Fn = (s, o, t) => FormatDate(DateTime.UtcNow, o),
              Requirements = Requirements.None,
              OptionTypes = OptionTypes.Date,
              HtmlDescription =
                "The current date formatted according to the Date" + " option.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VoteUSADomain]]",
            new Substitution
            {
              Fn = (s, o, t) => "Vote-USA.org",
              Requirements = Requirements.None,
              HtmlDescription = "Domain name for the main Vote-USA website.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[Vote-USA.org]]",
            new Substitution
            {
              Fn = (s, o, t) => "Vote-USA.org",
              Requirements = Requirements.None,
              HtmlDescription = "Alias for [[VoteUSADomain]].",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VoteUSALogoLarge##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateImageAnchor(UrlManager.SiteHostNameAndPort,
                    UrlManager.SiteHostNameAndPort +
                      "/images/designs/vote-usa/lgbanner.png"),
              Requirements = Requirements.None,
              HtmlDescription = "Large image link to the Vote-USA.org website.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VoteUSALogoSmall##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateImageAnchor(UrlManager.SiteHostNameAndPort,
                    UrlManager.SiteHostNameAndPort +
                      "/images/designs/vote-usa/smbanner.png"),
              Requirements = Requirements.None,
              HtmlDescription = "Small image link to the Vote-USA.org website.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@VoteUSAEmail@@",
            new Substitution
            {
              Fn = (s, o, t) => CreateMailToAnchor("mgr@Vote-USA.org"),
              Requirements = Requirements.None,
              HtmlDescription = "Mailto link for mgr@Vote-USA.org.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VoteUSAHomePage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(UrlManager.SiteHostNameAndPort, t ?? "Vote-USA"),
              Requirements = Requirements.None,
              HtmlDescription = "Hyperlink to the Vote-USA website home page.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VoteUSACandidatesPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    UrlManager.SiteHostNameAndPort + "/forCandidates.aspx",
                    t ?? "for Candidates"),
              Requirements = Requirements.None,
              HtmlDescription = "Hyperlink to the Vote-USA forCandidates page.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VoteUSAPartnersPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    UrlManager.SiteHostNameAndPort + "/forPartners.aspx",
                    t ?? "for Partners"),
              Requirements = Requirements.None,
              HtmlDescription = "Hyperlink to the Vote-USA forPartners page.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VoteUSAVolunteersPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    UrlManager.SiteHostNameAndPort + "/forVolunteers.aspx",
                    t ?? "for Volunteers"),
              Requirements = Requirements.None,
              HtmlDescription = "Hyperlink to the Vote-USA forVolunteers page.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VoteUSASampleBallotButtonsPage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    UrlManager.SiteHostNameAndPort + "/SampleBallotButtons.aspx",
                    t ?? "Ballot Choices Buttons"),
              Requirements = Requirements.None,
              HtmlDescription =
                "Hyperlink to the Vote-USA SampleBallotButtons page.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##Twitter##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateImageAnchor("https://twitter.com/VoteUSA1",
                    UrlManager.SiteHostNameAndPort +
                      "/images/twitter32.png", "Follow Vote-USA on Twitter", "twitter"),
              Requirements = Requirements.None,
              HtmlDescription = "Twitter logo link to Vote-USA.org on Twitter.",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
      
          // Undocumented
          {
            "[[EmailSub]]",
            new Substitution
            {
              Fn = (s, o, t) => "@@",
              Requirements = Requirements.None,
              HtmlDescription = "Substitutes @@, for documentation.",
              Type = Type.Undocumented,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[SubBegin]]",
            new Substitution
            {
              Fn = (s, o, t) => "[[",
              Requirements = Requirements.None,
              HtmlDescription = "Substitutes [[, for documentation.",
              Type = Type.Undocumented,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[SubEnd]]",
            new Substitution
            {
              Fn = (s, o, t) => "]]",
              Requirements = Requirements.None,
              HtmlDescription = "Substitutes ]], for documentation.",
              Type = Type.Undocumented,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[WebSub]]",
            new Substitution
            {
              Fn = (s, o, t) => "##",
              Requirements = Requirements.None,
              HtmlDescription = "Substitutes ##, for documentation.",
              Type = Type.Undocumented,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Deprecated
          {
            "@@altcontactemail@@",
            new Substitution
            {
              Fn = (s, o, t) => CreateMailToAnchor(States.GetAltEmail(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Mailto link for the alternate state contact person.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[altcontact]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetAltContact(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "The alternate state contact person.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[altcontactemail]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetAltEmail(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Email address of the alternate state contact person.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[altcontactphone]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetAltPhone(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "The phone number of the alternate state contact person.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[altcontacttitle]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetAltContactTitle(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "The title of the main alternate contact person.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[ballotname]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetBallotStateName(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "The official name of the state as it should appear on ballots.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##buttons##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor("vote-usa.org/SampleBallotButtons.aspx",
                    t ?? "Buttons Page"),
              Requirements = Requirements.None,
              HtmlDescription = "Hyperlink to the SampleBallotButtons page.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@maincontactemail@@",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateMailToAnchor(StateCache.GetContactEmail(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Mailto link for the main state contact person.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[maincontact]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetContact(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "The main state contact person.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[maincontactemail]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetContactEmail(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Email address of the main state contact person.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[maincontactphone]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetPhone(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "The phone number of the main state contact person.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[maincontacttitle]]",
            new Substitution
            {
              Fn = (s, o, t) => States.GetContactTitle(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "The title of the main state contact person.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[date]]",
            new Substitution
            {
              Fn = (s, o, t) => s.Now.Date.ToString("MMMM d, yyyy"),
              Requirements = Requirements.None,
              HtmlDescription = "The current date, formatted like January 5, 2014.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[election]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Elections.GetElectionDesc(s.ElectionKey),
              Requirements = Requirements.ElectionKey,
              HtmlDescription =
                "Description of the election based on the ElectionKey.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[electiondategeneral]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Elections.GetElectionDate(
                    s.PageCache.Elections
                      .GetLatestViewableGeneralElectionKeyByStateCode(s.StateCode))
                    .ToOptionalDate(),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Date of the latest viewable general election for the state," +
                  " formatted like November 5, 2014. It could be past or" +
                  " future.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[electiondescgeneral]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Elections.GetElectionDesc(
                    s.PageCache.Elections
                      .GetLatestViewableGeneralElectionKeyByStateCode(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Description of the latest viewable general election" +
                  " for the state. It could be past or future.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[electiondateoffyear]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Elections.GetElectionDate(
                    s.PageCache.Elections
                      .GetLatestViewableOffYearElectionKeyByStateCode(s.StateCode))
                    .ToOptionalDate(),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Date of the latest viewable off-year election for the" +
                  " state, formatted like November 5, 2014. It could be" +
                  " past or future.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[electiondescoffyear]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Elections.GetElectionDesc(
                    s.PageCache.Elections
                      .GetLatestViewableOffYearElectionKeyByStateCode(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Description of the latest viewable off-year election for" +
                  " the state. It could be past or future.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[electiondateprimary]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Elections.GetElectionDate(
                    s.PageCache.Elections
                      .GetLatestViewablePrimaryElectionKeyByStateCode(s.StateCode))
                    .ToString("MMMM d, yyyy"),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Date of the latest viewable primary election for the" +
                  " state, formatted like November 5, 2014. It could" +
                  " be past or future.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[electiondescprimary]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Elections.GetElectionDesc(
                    s.PageCache.Elections
                      .GetLatestViewablePrimaryElectionKeyByStateCode(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Description of the latest viewable primary election" +
                  " for the state. It could be past or future.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[electiondatespecial]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Elections.GetElectionDate(
                    s.PageCache.Elections
                      .GetLatestViewableSpecialElectionKeyByStateCode(s.StateCode))
                    .ToOptionalDate(),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Date of the latest viewable special election for the" +
                  " state, formatted like November 5, 2014. It could be" +
                  " past or future.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[electiondescspecial]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Elections.GetElectionDesc(
                    s.PageCache.Elections
                      .GetLatestViewableSpecialElectionKeyByStateCode(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Description of the latest viewable special election" +
                  " for the state. It could be past or future.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##electionroster##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(
                    UrlManager.GetElectionPageUri(
                      Elections.GetStateCodeFromKey(s.ElectionKey), s.ElectionKey),
                    t ?? s.PageCache.Elections.GetElectionDesc(s.ElectionKey)),
              Requirements = Requirements.ElectionKey,
              HtmlDescription = "Hyperlink to the public Election page.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##electionrosters##",
            new Substitution
            {
              Fn = (s, o, t) => Elections.GetElectionsOnSameDate(s.ElectionKey)
                .Select(
                  row =>
                    CreateWebAnchor(
                      UrlManager.GetElectionPageUri(
                        Elections.GetStateCodeFromKey(row.ElectionKey),
                        row.ElectionKey), t ?? row.ElectionDesc))
                .JoinText(),
              Requirements = Requirements.ElectionKey,
              HtmlDescription =
                "List of hyperlinks to the public Election page for" +
                  " all elections on the same date as the supplied" +
                  " ElectionKey, separated by commas.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[fname]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Politicians.GetFirstName(s.PoliticianKey),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "The first name of the politician.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[futureprimariesdesc]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  string.Join("<br />", s.GetFuturePrimaryElections(s.StateCode)
                    .Select(row => row.ElectionDesc)),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Description of the next future primary election for" +
                  " the state, or empty if none exists. If more then" +
                  " one primary is on the same date, all are included," +
                  " separated by break tags.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##futureprimariesrosters##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  string.Join("<br />", s.GetFuturePrimaryElections(s.StateCode)
                    .Select(
                      row =>
                        CreateWebAnchor(
                          UrlManager.GetElectionPageUri(
                            Elections.GetStateCodeFromKey(row.ElectionKey),
                            row.ElectionKey), t ?? row.ElectionDesc))),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Hyperlink to the public Election page for the next" +
                  " future primary election for the state, or empty if" +
                  " none exists. If more then one primary is on the same" +
                  " date, all are included, separated by break tags.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##IssuePage##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(UrlManager.GetCompareCandidatesPageUri(s.StateCode,
                    s.ElectionKey, s.OfficeKey), t),
              Requirements =
                Requirements.StateCode | Requirements.ElectionKey |
                Requirements.OfficeKey | Requirements.IssueKey,
              HtmlDescription = "Hyperlink to the public Issue page.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##ivn home##",
            new Substitution
            {
              Fn = (s, o, t) => CreateWebAnchor("ivn.us", t ?? "Independent Voter Network"),
              Requirements = Requirements.None,
              HtmlDescription =
                "Hyperlink to the Independent Voter Network" + " website.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##ivn vote-usa##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor("ivn.us/find-candidates",
                    t ?? "Independent Voter Network"),
              Requirements = Requirements.None,
              HtmlDescription =
                "Hyperlink to the Find-Candidates page on the" +
                  " Independent Voter Network website.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[lname]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PageCache.Politicians.GetLastName(s.PoliticianKey),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "Last name of the politician.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##officialsreport##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(UrlManager.GetOfficialsPageUri(s.StateCode), t),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Hyperlink to the public Officials page.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[password]]",
            new Substitution
            {
              Fn = (s, o, t) => Politicians.GetPassword(s.PoliticianKey),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "Password for the politician.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[politicians]]",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  s.PageCache.Politicians.GetPoliticianListForOffice(
                    s.ElectionKey, s.OfficeKey),
              Requirements = Requirements.ElectionKey | Requirements.OfficeKey,
              HtmlDescription =
                "A comma separated list of all candidates for the office" +
                  " in the election.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[primaries]]",
            new Substitution
            {
              Fn = (s, o, t) => Elections.GetElectionsOnSameDate(s.ElectionKey)
                .Select(row => row.ElectionDesc)
                .JoinText(),
              Requirements = Requirements.ElectionKey,
              HtmlDescription =
                "List of all elections on the same date as the supplied" +
                  " ElectionKey, separated by commas.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##stateanchor##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(StateCache.GetUri(s.StateCode),
                    StateCache.GetElectionsAuthority(s.StateCode),
                    t ?? "Website of the Election Authority", "view"),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Hyperlink to the state elections authority.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[stateelectionauthority]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetElectionsAuthority(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Name of the state elections authority.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[stateemail]]",
            new Substitution
            {
              Fn = (s, o, t) => StateCache.GetEmail(s.StateCode),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Email address of the state elections authority.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@stateemail@@",
            new Substitution
            {
              Fn = (s, o, t) => CreateMailToAnchor(StateCache.GetEmail(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Mailto link for the state elections authority.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##StateSecurePoliticianPage##",
            new Substitution
            {
              //Fn =
              //  (s, o, t) =>
              //    CreateWebAnchor(UrlManager.GetStateHostNameAndPort(s.StateCode) +
              //      "/Politician"),
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(UrlManager.GetAdminUri("/Politician"), t),
              Requirements = Requirements.None,
              HtmlDescription = "Hyperlink to the secure Politician page.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[username]]",
            new Substitution
            {
              Fn = (s, o, t) => s.PoliticianKey,
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "The politician key.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##voteanchor##",
            new Substitution
            {
              Fn = (s, o, t) => CreateWebAnchor(UrlManager.SiteUri, t ?? "Vote-USA"),
              Requirements = Requirements.None,
              HtmlDescription = "Hyperlink to the Vote-USA website home page.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@voteemail@@",
            new Substitution
            {
              Fn = (s, o, t) => CreateMailToAnchor("mgr@Vote-USA.org"),
              Requirements = Requirements.None,
              HtmlDescription = "Mailto link for mgr@Vote-USA.org.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##vote-xx.org/intro##",
            new Substitution
            {
              Fn =
                (s, o, t) =>
                  CreateWebAnchor(UrlManager.GetIntroPageUri(s.PoliticianKey), t),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription =
                "Hyperlink to the public Intro page for the politician.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          }
        };
    }

    private static string FormatDate(DateTime date, Options options)
    {
      if (date.IsDefaultDate()) return Empty;
      if ((options & Options.XmdDate) != 0) return date.ToString("M/d/yyyy");
      if ((options & Options.XmmddDate) != 0) return date.ToString("MM/dd/yyyy");
      if ((options & Options.XmmmdDate) != 0) return date.ToString("MMM d, yyyy");
      return (options & Options.XdddmmmdDate) != 0
        ? date.ToString("ddd, MMM d, yyyy")
        : date.ToString((options & Options.XddddmmmmdDate) != 0
          ? "dddd, MMMM d, yyyy"
          : "MMMM d, yyyy");
    }

    private static string Join(IEnumerable<string> list, Options options)
    {
      if ((options & Options.ParagraphList) != 0) return list.JoinAsParagraphs();
      if ((options & Options.UnorderedList) != 0) return list.JoinAsList();
      if ((options & Options.LinefeedList) != 0) return string.Join("\n", list);
      return (options & Options.BreakList) != 0
        ? string.Join("<br />", list)
        : list.JoinText();
    }
  }
}