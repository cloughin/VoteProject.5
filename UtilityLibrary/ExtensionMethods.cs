using System;
using System.Windows.Forms;

namespace UtilityLibrary
{
  public static class ExtensionMethods
  {
    // Call like this:
    //   this.Invoke(() => progressBar1.Value = i);
    // or
    //   string value = this.Invoke(() => button1.Text);

    public static TResult Invoke<T, TResult>(this T control, Func<TResult> code)
      where T : Control
    {
      if (control.InvokeRequired)
        return (TResult) control.Invoke(code);
      return code();
    }

    public static void Invoke(this Control control, Action code)
    {
      if (control.InvokeRequired)
        control.Invoke(code);
      else
        code();
    }
  }
}