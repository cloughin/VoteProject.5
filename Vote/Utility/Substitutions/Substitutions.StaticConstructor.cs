using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using DB.Vote;

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
              Fn = (s, o) => StateCache.GetStateName(s.StateCode),
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
              Fn = (s, o) => s.StateCode,
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
              Fn = (s, o) => StateCache.GetBallotStateName(s.StateCode),
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
              Fn = (s, o) => States.GetContact(s.StateCode),
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
              Fn = (s, o) => States.GetContactTitle(s.StateCode),
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
              Fn = (s, o) => StateCache.GetContactEmail(s.StateCode),
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
              Fn = (s, o) => States.GetPhone(s.StateCode),
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
              Fn = (s, o) => States.GetAltContact(s.StateCode),
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
              Fn = (s, o) => States.GetAltContactTitle(s.StateCode),
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
              Fn = (s, o) => States.GetAltEmail(s.StateCode),
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
              Fn = (s, o) => States.GetAltPhone(s.StateCode),
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
              Fn = (s, o) => StateCache.GetElectionsAuthority(s.StateCode),
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
              Fn = (s, o) => StateCache.GetEmail(s.StateCode),
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
              Fn = (s, o) => "Vote-" + s.StateCode + ".org",
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
              Fn = (s, o) => "Vote-" + s.StateCode + ".org",
              Requirements = Requirements.StateCode,
              HtmlDescription = "Alias for [[StateVoteUSADomain]].",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateVoteUSALogoLarge]]",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateImageAnchor(
                    "vote-" + s.StateCode.ToLowerInvariant() + ".org",
                    "vote-" + s.StateCode.ToLowerInvariant() +
                      ".org/images/designs/vote-" + s.StateCode.ToLowerInvariant() +
                      "/lgbanner.png"),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Large image link to the state Vote-XX.org website.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[StateVoteUSALogoSmall]]",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateImageAnchor(
                    "vote-" + s.StateCode.ToLowerInvariant() + ".org",
                    "vote-" + s.StateCode.ToLowerInvariant() +
                      ".org/images/designs/vote-" + s.StateCode.ToLowerInvariant() +
                      "/smbanner.png"),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Small image link to the state Vote-XX.org website.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@StateContactEmail@@",
            new Substitution
            {
              Fn =
                (s, o) =>
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
              Fn = (s, o) => CreateMailToAnchor(States.GetAltEmail(s.StateCode)),
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
              Fn = (s, o) => CreateMailToAnchor(StateCache.GetEmail(s.StateCode)),
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
                (s, o) =>
                  CreateWebAnchor(StateCache.GetUri(s.StateCode),
                    StateCache.GetElectionsAuthority(s.StateCode),
                    "Website of the " + StateCache.GetStateName(s.StateCode) +
                      " Election Authority", "view"),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Hyperlink to the state elections authority.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##StateVoteUSAHomePage##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetStateHostNameAndPort(s.StateCode),
                    "Vote-" + s.StateCode + ".org"),
              Requirements = Requirements.StateCode,
              HtmlDescription =
                "Hyperlink to the state home page on Vote-USA website.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##Vote-XX.org##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetStateHostNameAndPort(s.StateCode),
                    "Vote-" + s.StateCode + ".org"),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Alias for ##StateVoteUSAHomePage##.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##StateSecurePoliticianPage##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetStateHostNameAndPort(s.StateCode) +
                    "/Politician"),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Hyperlink to the secure Politician page.",
              Type = Type.State,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##OfficialsPage##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetOfficialsPageUri(s.StateCode)),
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
              Fn = (s, o) => CountyCache.GetCountyName(s.StateCode, s.CountyCode),
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
              Fn = (s, o) => s.CountyCode,
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
              Fn = (s, o) => Counties.GetContact(s.StateCode, s.CountyCode),
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
              Fn = (s, o) => Counties.GetContactTitle(s.StateCode, s.CountyCode),
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
              Fn = (s, o) => Counties.GetEmail(s.StateCode, s.CountyCode),
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
              Fn = (s, o) => Counties.GetPhone(s.StateCode, s.CountyCode),
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
              Fn = (s, o) => Counties.GetAltContact(s.StateCode, s.CountyCode),
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
              Fn = (s, o) => Counties.GetAltContactTitle(s.StateCode, s.CountyCode),
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
              Fn = (s, o) => Counties.GetAltEmail(s.StateCode, s.CountyCode),
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
              Fn = (s, o) => Counties.GetAltPhone(s.StateCode, s.CountyCode),
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
                (s, o) =>
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
                (s, o) =>
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
              Fn = (s, o) => VotePage.GetPageCache()
                .LocalDistricts.GetLocalDistrict(s.StateCode, s.CountyCode, s.LocalCode),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
              HtmlDescription = "Name or description of the local district.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalCode]]",
            new Substitution
            {
              Fn = (s, o) => s.LocalCode,
              Requirements = Requirements.LocalCode,
              HtmlDescription = "The two digit code for the local district.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[LocalContact]]",
            new Substitution
            {
              Fn =
                (s, o) =>
                  LocalDistricts.GetContact(s.StateCode, s.CountyCode, s.LocalCode),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
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
                (s, o) =>
                  LocalDistricts.GetContactTitle(s.StateCode, s.CountyCode,
                    s.LocalCode),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
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
                (s, o) =>
                  LocalDistricts.GetEmail(s.StateCode, s.CountyCode, s.LocalCode),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
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
                (s, o) =>
                  LocalDistricts.GetPhone(s.StateCode, s.CountyCode, s.LocalCode),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
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
                (s, o) =>
                  LocalDistricts.GetAltContact(s.StateCode, s.CountyCode,
                    s.LocalCode),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
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
                (s, o) =>
                  LocalDistricts.GetAltContactTitle(s.StateCode, s.CountyCode,
                    s.LocalCode),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
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
                (s, o) =>
                  LocalDistricts.GetAltEmail(s.StateCode, s.CountyCode, s.LocalCode),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
              HtmlDescription =
                "Email address of the alternate local contact person.",
              Type = Type.Local,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[LocalAltContactPhone]]",
            new Substitution
            {
              Fn =
                (s, o) =>
                  LocalDistricts.GetAltPhone(s.StateCode, s.CountyCode, s.LocalCode),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
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
                (s, o) =>
                  CreateMailToAnchor(LocalDistricts.GetEmail(s.StateCode,
                    s.CountyCode, s.LocalCode)),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
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
                (s, o) =>
                  CreateMailToAnchor(LocalDistricts.GetAltEmail(s.StateCode,
                    s.CountyCode, s.LocalCode)),
              Requirements =
                Requirements.StateCode | Requirements.CountyCode |
                  Requirements.LocalCode,
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
                (s, o) =>
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
            "[[ElectionDesc]]",
            new Substitution
            {
              Fn = (s, o) => s.PageCache.Elections.GetElectionDesc(s.ElectionKey),
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
              Fn = (s, o) => Join(Elections.GetElectionsOnSameDate(s.ElectionKey)
                .OfType<ElectionsRow>()
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
                  CreateWebAnchor(
                    UrlManager.GetElectionPageUri(
                      Elections.GetStateCodeFromKey(s.ElectionKey), s.ElectionKey),
                    s.PageCache.Elections.GetElectionDesc(s.ElectionKey)),
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
                (s, o) =>
                  CreateWebAnchor(
                    UrlManager.GetElectionPageUri(
                      Elections.GetStateCodeFromKey(s.ElectionKey), s.ElectionKey).ToString() + "&openall=Y",
                    s.PageCache.Elections.GetElectionDesc(s.ElectionKey)),
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
              Fn = (s, o) => Join(Elections.GetElectionsOnSameDate(s.ElectionKey)
                .OfType<ElectionsRow>()
                .Select(
                  row =>
                    CreateWebAnchor(
                      UrlManager.GetElectionPageUri(
                        Elections.GetStateCodeFromKey(row.ElectionKey),
                        row.ElectionKey), row.ElectionDesc)), o),
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
              Fn = (s, o) => Join(Elections.GetElectionsOnSameDate(s.ElectionKey)
                .OfType<ElectionsRow>()
                .Select(
                  row =>
                    CreateWebAnchor(
                      UrlManager.GetElectionPageUri(
                        Elections.GetStateCodeFromKey(row.ElectionKey),
                        row.ElectionKey, true), row.ElectionDesc)), o),
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
              Fn = (s, o) => s.GetLocalizedOfficeName(s.OfficeKey),
              Requirements = Requirements.OfficeKey,
              HtmlDescription =
                "Description of the office, including jurisdiction information for" +
                  "county and local offices.",
              Type = Type.Office,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[OfficeShort]]",
            new Substitution
            {
              Fn =
                (s, o) =>
                  Offices.FormatOfficeName(
                    s.PageCache.Offices.GetOfficeLine1(s.OfficeKey),
                    s.PageCache.Offices.GetOfficeLine2(s.OfficeKey), s.OfficeKey),
              Requirements = Requirements.OfficeKey,
              HtmlDescription =
                "Description of the office without jurisdiction information for" +
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
                (s, o) =>
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
                (s, o) =>
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

          // Politician Type
          {
            "[[Politician]]",
            new Substitution
            {
              Fn =
                (s, o) =>
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
              Fn = (s, o) => s.PageCache.Politicians.GetFirstName(s.PoliticianKey),
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
              Fn = (s, o) => s.PageCache.Politicians.GetLastName(s.PoliticianKey),
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
              Fn = (s, o) => s.PoliticianKey,
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
              Fn = (s, o) => s.PoliticianKey,
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
              Fn = (s, o) => Politicians.GetPassword(s.PoliticianKey),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "Password for the politician.",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PoliticianIntroPage##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetIntroPageUri(s.PoliticianKey)),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription =
                "Hyperlink to the public Intro page for the politician.",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##PoliticianUnsubscribe##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  AddStyleToTag(CreateWebAnchor(
                    string.Format(
                      UrlManager.SiteHostNameAndPort +
                        "/subscribe.aspx?key={0}&op=unpol",
                      s.PoliticianKey),
                    "Unsubscribe from Vote-USA candidate emails"), "font-family:arial;font-size:8pt;color:#aaa"),
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Link to the subscribe page that says \"Unsubscribe from Vote-USA candidate emails\", styled for use in an email footer",
              Type = Type.Politician,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Issue Type
          {
            "[[Issue]]",
            new Substitution
            {
              Fn = (s, o) => s.PageCache.Issues.GetIssue(s.IssueKey),
              Requirements = Requirements.IssueKey,
              HtmlDescription = "Description of the issue.",
              Type = Type.Issue,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##IssuePage##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetCompareCandidatesPageUri(s.StateCode,
                    s.ElectionKey, s.OfficeKey)),
              Requirements =
                Requirements.StateCode | Requirements.ElectionKey |
                  Requirements.OfficeKey | Requirements.IssueKey,
              HtmlDescription = "Hyperlink to the public Issue page.",
              Type = Type.Issue,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Party Type
          {
            "[[Party]]",
            new Substitution
            {
              Fn = (s, o) => s.PageCache.Parties.GetPartyName(s.PartyKey),
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
              Fn = (s, o) => s.PageCache.Parties.GetPartyCode(s.PartyKey),
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
              Fn = (s, o) => s.PartyKey,
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
              Fn = (s, o) => s.PartyEmail,
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
              Fn = (s, o) => "<span style=\"display:none\">@</span>" + s.PartyEmail,
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
              Fn = (s, o) => PartiesEmails.GetPartyPassword(s.PartyEmail),
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
              Fn = (s, o) => PartiesEmails.GetPartyContactFirstName(s.PartyEmail),
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
              Fn = (s, o) => PartiesEmails.GetPartyContactLastName(s.PartyEmail),
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
              Fn = (s, o) => PartiesEmails.GetPartyContactTitle(s.PartyEmail),
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
              Fn = (s, o) => PartiesEmails.GetPartyContactPhone(s.PartyEmail),
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
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetStateHostNameAndPort(s.StateCode) +
                    "/Party"),
              Requirements = Requirements.StateCode,
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
                (s, o) =>
                  AddStyleToTag(CreateWebAnchor(
                    string.Format(
                      UrlManager.SiteHostNameAndPort +
                        "/subscribe.aspx?email={0}&op=unpty",
                      s.PartyEmail),
                    "Unsubscribe from Vote-USA party-related emails"), "font-family:arial;font-size:8pt;color:#aaa"),
              Requirements = Requirements.VisitorId,
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
              Fn = (s, o) => VolunteersView.GetFirstName(s.PartyEmail),
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
              Fn = (s, o) => VolunteersView.GetLastName(s.PartyEmail),
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
              Fn = (s, o) => VolunteersView.GetPhone(s.PartyEmail),
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
              Fn = (s, o) => VolunteersView.GetStateCode(s.PartyEmail).SafeString(),
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
              Fn = (s, o) => StateCache.GetStateName(VolunteersView.GetStateCode(s.PartyEmail)).SafeString(),
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
              Fn = (s, o) => VolunteersView.GetPartyName(s.PartyEmail).SafeString(),
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
              Fn = (s, o) => s.PartyEmail,
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
              Fn = (s, o) => "<span style=\"display:none\">@</span>" + s.PartyEmail,
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
              Fn = (s, o) => VolunteersView.GetPassword(s.PartyEmail),
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
                (s, o) =>
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
              Fn = (s, o) => s.PageCache.Addresses.GetFirstName(s.VisitorId),
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
              Fn = (s, o) => s.PageCache.Addresses.GetLastName(s.VisitorId),
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
              Fn = (s, o) => s.PageCache.Addresses.GetAddress(s.VisitorId),
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
              Fn = (s, o) => s.PageCache.Addresses.GetCity(s.VisitorId),
              Requirements = Requirements.VisitorId,
              HtmlDescription = "The visitor's city.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorZipCode]]", new Substitution
            {
              Fn = (s, o) =>
              {
                var zip = s.PageCache.Addresses.GetZip5(s.VisitorId);
                if (!string.IsNullOrEmpty(zip))
                {
                  var zip4 = s.PageCache.Addresses.GetZip4(s.VisitorId);
                  if (!string.IsNullOrWhiteSpace(zip4)) zip += "-" + zip4;
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) => s.PageCache.Addresses.GetStateSenateDistrict(s.VisitorId),
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
                (s, o) =>
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
                (s, o) => s.PageCache.Addresses.GetStateHouseDistrict(s.VisitorId),
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
                (s, o) =>
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
              Fn = (s, o) => s.PageCache.Addresses.GetSourceCode(s.VisitorId),
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
                (s, o) =>
                  s.PageCache.Addresses.GetSendSampleBallots(s.VisitorId)
                    ? "yes"
                    : "no",
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Whether the visitor requested to be sent sample ballots (yes or no).",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VisitorSubscribeToSampleBallots##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(
                    string.Format(
                      UrlManager.SiteHostNameAndPort +
                        "/subscribe.aspx?email={0}&code={1}&op=ballots",
                      s.GetAdHocSubstitution("[[ContactEmail]]"), s.VisitorId),
                    "Subscribe to future sample ballots via email"),
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Link to the subscribe page that says \"Subscribe to future sample ballots via email\".",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##VisitorUnsubscribe##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  AddStyleToTag(CreateWebAnchor(
                    string.Format(
                      UrlManager.SiteHostNameAndPort +
                        "/subscribe.aspx?email={0}&code={1}&op=unsubscribe",
                      s.GetAdHocSubstitution("[[ContactEmail]]"), s.VisitorId),
                    "Unsubscribe from Vote-USA email"), "font-family:arial;font-size:8pt;color:#aaa"),
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Link to the subscribe page that says \"Unsubscribe from Vote-USA email\", styled for use in an email footer",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[VisitorSampleBallotCount()]]", new Substitution
            {
              Fn = (s, o) =>
              {
                if (
                  string.IsNullOrWhiteSpace(
                    s.PageCache.Addresses.GetCongressionalDistrict(s.VisitorId))) return "0";
                return
                    Elections.GetElectionsForSampleBallotsSubstitutions(s.StateCode, o)
                    .Count()
                    .ToString(CultureInfo.InvariantCulture);
              },
              OptionTypes = OptionTypes.PastFuture,
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Count of links to the sample ballot page for the visitor. " +
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
              Fn = (s, o) =>
              {
                if (
                  string.IsNullOrWhiteSpace(
                    s.PageCache.Addresses.GetCongressionalDistrict(s.VisitorId))) throw new VoteSubstitutionException("Visitor does not have district coding");
                return
                  Join(
                    Elections.GetElectionsForSampleBallotsSubstitutions(s.StateCode, o)
                      .Select(
                        i =>
                          CreateWebAnchor(
                            UrlManager.GetBallotPageUri(i.Value,
                              s.PageCache.Addresses.GetCongressionalDistrict(
                                s.VisitorId),
                              s.PageCache.Addresses.GetStateSenateDistrict(s.VisitorId),
                              s.PageCache.Addresses.GetStateHouseDistrict(s.VisitorId),
                              s.PageCache.Addresses.GetCountyCode(s.VisitorId)), i.Text)),
                    o);
              },
              OptionTypes = OptionTypes.List | OptionTypes.PastFuture,
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "List of links to the sample ballot page for the visitor. " +
                  "Only viewable elections are included. " +
                  "There will only be one link in the list unless this is a primary. " +
                  "This substitution will cause the email to fail if the visitor has no district coding.",
              Type = Type.Visitor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Donor Type
          {
            "[[DonorFirstName]]",
            new Substitution
            {
              Fn = (s, o) => s.PageCache.Donations.GetFirstName(s.DonorId),
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
              Fn = (s, o) => s.PageCache.Donations.GetLastName(s.DonorId),
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
              Fn = (s, o) => s.PageCache.Donations.GetAddress(s.DonorId),
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
              Fn = (s, o) => s.PageCache.Donations.GetCity(s.DonorId),
              Requirements = Requirements.DonorId,
              HtmlDescription = "The donor's city from the donation receipt. Could be different from [[VisitorCity]].",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[DonorZipCode]]", new Substitution
            {
              Fn = (s, o) =>
              {
                var zip = s.PageCache.Donations.GetZip5(s.DonorId);
                if (!string.IsNullOrEmpty(zip))
                {
                  var zip4 = s.PageCache.Donations.GetZip4(s.DonorId);
                  if (!string.IsNullOrWhiteSpace(zip4)) zip += "-" + zip4;
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
              Fn = (s, o) => s.PageCache.Donations.GetPhone(s.DonorId),
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
                (s, o) =>
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
              Fn = (s, o) => s.PageCache.Donations.GetAmount(s.DonorId).ToString("C"),
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
              Fn = (s, o) => Donations.GetTotalAmountByEmail(s.PageCache.Donations.GetEmail(s.DonorId)).ToString("C"),
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
                (s, o) =>
                  AddStyleToTag(CreateWebAnchor(
                    string.Format(
                      UrlManager.SiteHostNameAndPort +
                        "/subscribe.aspx?email={0}&op=undnr",
                      s.PageCache.Donations.GetEmail(s.DonorId)),
                    "Unsubscribe from Vote-USA emails"), "font-family:arial;font-size:8pt;color:#aaa"),
              Requirements = Requirements.VisitorId,
              HtmlDescription =
                "Link to the subscribe page that says \"Unsubscribe from Vote-USA emails\", styled for use in an email footer",
              Type = Type.Donor,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },

          // Miscellaneous Type
          {
            "##DonationImage##",
            new Substitution
            {
              Fn =
                (s, o) =>
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
                (s, o) =>
                  CreateWebAnchor(
                    VotePage.DonateUrl,
                    "100% tax-deductible donation", "Donate to Vote-USA"),
              Requirements = Requirements.None,
              HtmlDescription =
                "Text link to the donation page that says \"100% tax-deductible donation\".",
              Type = Type.Miscellaneous,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[Today()]]",
            new Substitution
            {
              Fn = (s, o) => FormatDate(DateTime.UtcNow, o),
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
              Fn = (s, o) => "Vote-USA.org",
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
              Fn = (s, o) => "Vote-USA.org",
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
                (s, o) =>
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
                (s, o) =>
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
              Fn = (s, o) => CreateMailToAnchor("mgr@Vote-USA.org"),
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
                (s, o) =>
                  CreateWebAnchor(UrlManager.SiteHostNameAndPort, "Vote-USA"),
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
                (s, o) =>
                  CreateWebAnchor(
                    UrlManager.SiteHostNameAndPort + "/forCandidates.aspx",
                    "for Candidates"),
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
                (s, o) =>
                  CreateWebAnchor(
                    UrlManager.SiteHostNameAndPort + "/forPartners.aspx",
                    "for Partners"),
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
                (s, o) =>
                  CreateWebAnchor(
                    UrlManager.SiteHostNameAndPort + "/forVolunteers.aspx",
                    "for Volunteers"),
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
                (s, o) =>
                  CreateWebAnchor(
                    UrlManager.SiteHostNameAndPort + "/SampleBallotButtons.aspx",
                    "Sample Ballot Buttons"),
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
                (s, o) =>
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
              Fn = (s, o) => "@@",
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
              Fn = (s, o) => "[[",
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
              Fn = (s, o) => "]]",
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
              Fn = (s, o) => "##",
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
              Fn = (s, o) => CreateMailToAnchor(States.GetAltEmail(s.StateCode)),
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
              Fn = (s, o) => States.GetAltContact(s.StateCode),
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
              Fn = (s, o) => States.GetAltEmail(s.StateCode),
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
              Fn = (s, o) => States.GetAltPhone(s.StateCode),
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
              Fn = (s, o) => States.GetAltContactTitle(s.StateCode),
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
              Fn = (s, o) => StateCache.GetBallotStateName(s.StateCode),
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
                (s, o) =>
                  CreateWebAnchor("vote-usa.org/SampleBallotButtons.aspx",
                    "Buttons Page"),
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
                (s, o) =>
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
              Fn = (s, o) => States.GetContact(s.StateCode),
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
              Fn = (s, o) => StateCache.GetContactEmail(s.StateCode),
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
              Fn = (s, o) => States.GetPhone(s.StateCode),
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
              Fn = (s, o) => States.GetContactTitle(s.StateCode),
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
              Fn = (s, o) => s.Now.Date.ToString("MMMM d, yyyy"),
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
              Fn = (s, o) => s.PageCache.Elections.GetElectionDesc(s.ElectionKey),
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
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
                (s, o) =>
                  CreateWebAnchor(
                    UrlManager.GetElectionPageUri(
                      Elections.GetStateCodeFromKey(s.ElectionKey), s.ElectionKey),
                    s.PageCache.Elections.GetElectionDesc(s.ElectionKey)),
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
              Fn = (s, o) => Elections.GetElectionsOnSameDate(s.ElectionKey)
                .OfType<ElectionsRow>()
                .Select(
                  row =>
                    CreateWebAnchor(
                      UrlManager.GetElectionPageUri(
                        Elections.GetStateCodeFromKey(row.ElectionKey),
                        row.ElectionKey), row.ElectionDesc))
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
              Fn = (s, o) => s.PageCache.Politicians.GetFirstName(s.PoliticianKey),
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
                (s, o) =>
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
                (s, o) =>
                  string.Join("<br />", s.GetFuturePrimaryElections(s.StateCode)
                    .Select(
                      row =>
                        CreateWebAnchor(
                          UrlManager.GetElectionPageUri(
                            Elections.GetStateCodeFromKey(row.ElectionKey),
                            row.ElectionKey), row.ElectionDesc))),
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
            "##ivn home##",
            new Substitution
            {
              Fn = (s, o) => CreateWebAnchor("ivn.us", "Independent Voter Network"),
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
                (s, o) =>
                  CreateWebAnchor("ivn.us/find-candidates",
                    "Independent Voter Network"),
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
              Fn = (s, o) => s.PageCache.Politicians.GetLastName(s.PoliticianKey),
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
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetOfficialsPageUri(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Hyperlink to the public Officials page.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[org]]",
            new Substitution
            {
              Fn =
                (s, o) =>
                  s.PageCache.Organizations.GetOrganization(s.OrganizationCode),
              Requirements = Requirements.OrganizationCode,
              HtmlDescription = "Name of the organization.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "@@orgemail@@",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateMailToAnchor(
                    s.PageCache.Organizations.GetOrganizationEmail(
                      s.OrganizationCode)),
              Requirements = Requirements.OrganizationCode,
              HtmlDescription = "Mailto link for the organization.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##organchor##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(
                    s.PageCache.Organizations.GetOrganizationUrl(s.OrganizationCode),
                    s.PageCache.Organizations.GetOrganization(s.OrganizationCode)),
              Requirements = Requirements.OrganizationCode,
              HtmlDescription = "Hyperlink to the organization website.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[password]]",
            new Substitution
            {
              Fn = (s, o) => Politicians.GetPassword(s.PoliticianKey),
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
                (s, o) =>
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
              Fn = (s, o) => Elections.GetElectionsOnSameDate(s.ElectionKey)
                .OfType<ElectionsRow>()
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
                (s, o) =>
                  CreateWebAnchor(StateCache.GetUri(s.StateCode),
                    StateCache.GetElectionsAuthority(s.StateCode),
                    "Website of the Election Authority", "view"),
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
              Fn = (s, o) => StateCache.GetElectionsAuthority(s.StateCode),
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
              Fn = (s, o) => StateCache.GetEmail(s.StateCode),
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
              Fn = (s, o) => CreateMailToAnchor(StateCache.GetEmail(s.StateCode)),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Mailto link for the state elections authority.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "[[username]]",
            new Substitution
            {
              Fn = (s, o) => s.PoliticianKey,
              Requirements = Requirements.PoliticianKey,
              HtmlDescription = "The politician key.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          //{
          //  "##video-scrape-bios##",
          //  new Substitution
          //  {
          //    Fn =
          //      (s, o) =>
          //        CreateWebAnchor("Vote-USA.org/Video.aspx?video=introBio",
          //          "Scraping Candidates’ Websites for Biographical" +
          //            " Information, Website Address, and Social Media Links"),
          //    Requirements = Requirements.None,
          //    HtmlDescription = "Hyperlink to the Vote-USA Video page.",
          //    Type = Type.Deprecated,
          //    DisplayOrder = ++SubstitutionDisplayOrder
          //  }
          //},
          {
            "##voteanchor##",
            new Substitution
            {
              Fn = (s, o) => CreateWebAnchor(UrlManager.SiteUri, "Vote-USA"),
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
              Fn = (s, o) => CreateMailToAnchor("mgr@Vote-USA.org"),
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
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetIntroPageUri(s.PoliticianKey)),
              Requirements = Requirements.PoliticianKey,
              HtmlDescription =
                "Hyperlink to the public Intro page for the politician.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##vote-xx.org/issue##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetCompareCandidatesPageUri(s.StateCode,
                    s.ElectionKey, s.OfficeKey)),
              Requirements =
                Requirements.StateCode | Requirements.ElectionKey |
                  Requirements.OfficeKey | Requirements.IssueKey,
              HtmlDescription = "Hyperlink to the public Issue page.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##vote-xx.org/party##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetStateHostNameAndPort(s.StateCode) +
                    "/Party"),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Hyperlink to the secure Party page.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          },
          {
            "##vote-xx.org/politician##",
            new Substitution
            {
              Fn =
                (s, o) =>
                  CreateWebAnchor(UrlManager.GetStateHostNameAndPort(s.StateCode) +
                    "/Politician"),
              Requirements = Requirements.StateCode,
              HtmlDescription = "Hyperlink to the secure Politician page.",
              Type = Type.Deprecated,
              DisplayOrder = ++SubstitutionDisplayOrder
            }
          }
        };
    }

    private static string FormatDate(DateTime date, Options options)
    {
      if (date == DateTime.MinValue || date == VotePage.DefaultDbDate) return string.Empty;
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