using System;
using DB.Vote;

namespace Vote
{
  public class SecurePartyPage : SecurePage
  {
    #region Public Properties

    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable MemberCanBeProtected.Global
    public string PartyCode { get; private set; }

    public bool IsPartyMajor { get; private set; }

    public string PartyKey { get; private set; }

    public string PartyName { get; private set; }

    public bool PartyKeyExists { get; private set; }

    public string StateCode { get; private set; }

    public bool StateCodeIsNonState { get; private set; }
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global

    #endregion Public Properties

    #region Event handlers and overrides

    protected override void OnLoad(EventArgs e)
    {
      // Skip OnLoad processing if key is missing, handled in OnPreRender
      if (PartyKeyExists || (string.IsNullOrWhiteSpace(PartyKey) && this is IAllowEmptyPartyKey))
        base.OnLoad(e);
    }

    protected override void OnPreLoad(EventArgs e)
    {
      base.OnPreLoad(e);
      PartyKey = ViewStatePartyKey;
      PartyKeyExists = Parties.PartyKeyExists(PartyKey);
      if (PartyKeyExists)
      {
        PartyName = PageCache.Parties.GetPartyName(PartyKey);
        PartyCode = PageCache.Parties.GetPartyCode(PartyKey);
        StateCode = PageCache.Parties.GetStateCode(PartyKey);
        IsPartyMajor = PageCache.Parties.GetIsPartyMajor(PartyKey);
        StateCodeIsNonState = !StateCache.IsValidStateCode(StateCode);
      }
      else if (!IsSignedIn) // if not signed in, dump to sign in pageName
        RedirectToSignInPage();
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      if (PartyKeyExists || (string.IsNullOrWhiteSpace(PartyKey) && this is IAllowEmptyPartyKey))
        return;
      var mainContent = Master.MainContentControl;
      mainContent.Controls.Clear();
      var text = string.IsNullOrWhiteSpace(PartyKey)
        ? "PartyKey is missing"
        : "PartyKey [" + PartyKey + "] is invalid";
      new HtmlP {InnerHtml = text}.AddTo(mainContent,
        "missing-key");
    }

    #endregion Event handlers and overrides
  }
}


// marker for empty party key code pages
internal interface IAllowEmptyPartyKey
{
}