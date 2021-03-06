namespace DB.Vote
{
  public sealed class OfficeKeyInfo
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static readonly OfficeKeyInfo Empty = new OfficeKeyInfo
    {
      ElectionKey = string.Empty,
      OfficeKey = string.Empty
    };

    public string ElectionKey;
    public string OfficeKey;

    public bool IsEmpty
    {
      get
      {
        return string.IsNullOrWhiteSpace(ElectionKey) &&
          string.IsNullOrWhiteSpace(OfficeKey);
      }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}