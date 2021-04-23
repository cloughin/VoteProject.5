namespace DB.Vote
{
  public /*partial*/ class PoliticiansImagesView
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

    public static void DeleteByPoliticianKey(
      string politicianKey, int commandTimeout = -1)
    {
      PoliticiansImagesData.DeleteByPoliticianKey(politicianKey, commandTimeout);
      PoliticiansImagesBlobs.DeleteByPoliticianKey(politicianKey, commandTimeout);
    }

    public static void GuaranteePoliticianKeyExists(string politicianKey)
    {
      PoliticiansImagesData.GuaranteePoliticianKeyExists(politicianKey);
      PoliticiansImagesBlobs.GuaranteePoliticianKeyExists(politicianKey);
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}