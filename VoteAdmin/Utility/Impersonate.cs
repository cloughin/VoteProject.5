using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

//   I m p e r s o n a t e

public class Impersonate : IDisposable
{
  private bool _Impersonating;
  private readonly WindowsImpersonationContext _Context;
  private readonly WindowsIdentity _OriginalIdentity;

  private const int Logon32LogonInteractive = 2;
  private const int Logon32ProviderDefault = 0;
  private const int SecurityImpersonation = 2;

  [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
  private static extern bool CloseHandle(IntPtr handle);

  [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern int DuplicateToken(
    IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);

  [DllImport("advapi32.dll")]
  private static extern int LogonUserA(
    string lpszUserName, string lpszDomain, string lpszPassword, int dwLogonType,
    int dwLogonProvider, ref IntPtr phToken);

  [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool RevertToSelf();

  protected Impersonate(string domain, string user, string password)
  {
    var token = IntPtr.Zero;
    var tokenDuplicate = IntPtr.Zero;
    var ok = false;

    _OriginalIdentity = WindowsIdentity.GetCurrent();
    try
    {
      if (RevertToSelf())
        if (
          LogonUserA(
            user, domain, password, Logon32LogonInteractive, Logon32ProviderDefault,
            ref token) != 0)
          if (DuplicateToken(token, SecurityImpersonation, ref tokenDuplicate) != 0)
          {
            var tempIdentity = new WindowsIdentity(tokenDuplicate);
            _Context = tempIdentity.Impersonate();
            _Impersonating = _Context != null;
            ok = true;
          }
      if (!ok)
        throw new ApplicationException(
          $"Impersonation of {domain}\\{user} by {Name} failed with Win32 error {Marshal.GetLastWin32Error()}");
    }
    catch
    {
      _OriginalIdentity?.Impersonate();
    }
    finally
    {
      if (token != IntPtr.Zero)
        CloseHandle(token);
      if (tokenDuplicate != IntPtr.Zero)
        CloseHandle(tokenDuplicate);
    }
  }

  private static string Name => WindowsIdentity.GetCurrent().Name;

  private void Undo()
  {
    if (!_Impersonating) return;
    _Context.Undo();
    _OriginalIdentity.Impersonate();
    _Impersonating = false;
  }

  #region IDisposable Members

  public void Dispose()
  {
    Undo();
  }

  #endregion
}

public class ImpersonateGuest : Impersonate
{
  public ImpersonateGuest() : base(Environment.MachineName, "Guest", string.Empty)
  {
  }
}