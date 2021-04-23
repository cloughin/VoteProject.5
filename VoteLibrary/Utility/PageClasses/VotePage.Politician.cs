using System;
using System.Collections.Generic;
using DB.Vote;
using static System.String;

namespace Vote
{
  // Public methods and properties relation to politicians
  public partial class VotePage
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static readonly Dictionary<string, string> AlternatePoliticianTabLabels =
      new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
      {
        {"ALLBio111111", "General<br />Philosophy"},
        {"ALLBio222222", "Personal<br />and Family"},
        {"ALLBio333333", "Professional<br />Experience"},
        {"ALLBio444444", "Civic<br />Involvement"},
        {"ALLBio555555", "Political<br />Experience"},
        {"ALLBio666666", "Religious<br />Affiliation"},
        {"ALLBio777777", "Accomplishments<br />and Awards"},
        {"ALLBio888888", "Educational<br />Background"},
        {"ALLBio999999", "Military<br />Service"}
      };

    public static readonly Dictionary<string, string> AlternatePoliticianTabLabels2 =
      new Dictionary<string, string>()
      {
        {Issues.QuestionId.GeneralPhilosophy.ToInt().ToString(), "General<br />Philosophy"},
        {Issues.QuestionId.PersonalAndFamily.ToInt().ToString(), "Personal<br />and Family"},
        {Issues.QuestionId.ProfessionalExperience.ToInt().ToString(), "Professional<br />Experience"},
        {Issues.QuestionId.CivicInvolvement.ToInt().ToString(), "Civic<br />Involvement"},
        {Issues.QuestionId.PoliticalExperience.ToInt().ToString(), "Political<br />Experience"},
        {Issues.QuestionId.ReligiousAffiliation.ToInt().ToString(), "Religious<br />Affiliation"},
        {Issues.QuestionId.AccomplishmentsAndAwards.ToInt().ToString(), "Accomplishments<br />and Awards"},
        {Issues.QuestionId.EducationalBackground.ToInt().ToString(), "Educational<br />Background"},
        {Issues.QuestionId.MilitaryService.ToInt().ToString(), "Military<br />Service"}
      };

    public static string GetPoliticianImageUrl(
      string politicianKey, string size, bool noCache = false)
    {
      var url = Format(
        "/Image.aspx?Id={0}&Col={1}&Def={1}", politicianKey, size);
      if (noCache)
        url = InsertNoCacheIntoUrl(url);
      return url;
    }

    public static string GetPoliticianImageUrl(
      string politicianKey, int width, bool noCache = false)
    {
      var columnName = ImageManager.GetPoliticianImageColumnNameByWidth(width);
      if (IsNullOrWhiteSpace(columnName))
        throw new ArgumentException("Invalid width");
      return GetPoliticianImageUrl(politicianKey, columnName, noCache);
    }


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}