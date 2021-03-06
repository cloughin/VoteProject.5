using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Vote
{
  public class MixedNumericComparer : IComparer<string>
  {
    private static MixedNumericComparer _Instance;

    public static MixedNumericComparer Instance => _Instance ?? (_Instance = new MixedNumericComparer());

    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    static extern int StrCmpLogicalW(String x, String y);

    public int Compare(string x, string y)
    {
      return StrCmpLogicalW(x, y);
    }

  }
 
  public sealed class SimpleListItem
  {
    public string Text;
    public string Value;

    public SimpleListItem() {}

    public SimpleListItem(string value, string text)
    {
      Text = text;
      Value = value;
    }
  }

  public sealed class StringPair : IComparable<StringPair>
  {
    public readonly string String1;
    public readonly string String2;

    public StringPair(string string1, string string2)
    {
      String1 = string1;
      String2 = string2;
    }

    public override bool Equals(object obj)
    {
      var o = obj as StringPair;
      // ReSharper disable once RedundantCast
      if ((object) o == null) return false;
      return String2 == o.String2 && String1 == o.String1;
    }

    public override int GetHashCode()
    {
      return String1.GetHashCode() ^ String2.GetHashCode();
    }

    public int CompareTo(StringPair other)
    {
      if (String1.IsLt(other.String1)) return -1;
      if (String1.IsGt(other.String1)) return 1;
      if (String2.IsLt(other.String2)) return -1;
      if (String2.IsGt(other.String2)) return 1;
      return 0;
    }
  }

  public sealed class StringPairIgnoreCase : IComparable<StringPairIgnoreCase>
  {
    public readonly string String1;
    public readonly string String2;

    public StringPairIgnoreCase(string string1, string string2)
    {
      String1 = string1;
      String2 = string2;
    }

    public int CompareTo(StringPairIgnoreCase other)
    {
      if (String1.IsLtIgnoreCase(other.String1)) return -1;
      if (String1.IsGtIgnoreCase(other.String1)) return 1;
      if (String2.IsLtIgnoreCase(other.String2)) return -1;
      if (String2.IsGtIgnoreCase(other.String2)) return 1;
      return 0;
    }

    public override bool Equals(object obj)
    {
      var o = obj as StringPairIgnoreCase;
      // ReSharper disable once RedundantCast
      if (o == null) return false;
      return string.Compare(String1, o.String1, StringComparison.OrdinalIgnoreCase) == 0 &&
        string.Compare(String2, o.String2, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public override int GetHashCode()
    {
      return String1.GetHashCode() ^ String2.GetHashCode();
    }
  }

  public class HtmlBreak : Literal
  {
    public HtmlBreak(int n = 1)
    {
      n = Math.Max(1, n);
      switch (n)
      {
        case 1:
          Text = "<br />";
          break;
        case 2:
          Text = "<br /><br />";
          break;
        default:
        {
          var sb = new StringBuilder();
          for (var x = 1; x <= n; x++) sb.Append("<br />");
          Text = sb.ToString();
        }
          break;
      }
    }
  }

  public class HtmlDiv : HtmlGenericControl
  {
    public HtmlDiv() : base("div") { }
  }

  public class HtmlEm : HtmlGenericControl
  {
    public HtmlEm() : base("em") { }
  }

  public class HtmlH1 : HtmlGenericControl
  {
    public HtmlH1() : base("h1") { }
  }

  public class HtmlH2 : HtmlGenericControl
  {
    public HtmlH2() : base("h2") { }
  }

  public class HtmlH3 : HtmlGenericControl
  {
    public HtmlH3() : base("h3") { }
  }

  public class HtmlH4 : HtmlGenericControl
  {
    public HtmlH4() : base("h4") { }
  }

  public class HtmlH5 : HtmlGenericControl
  {
    public HtmlH5() : base("h5") { }
  }

  public class HtmlH6 : HtmlGenericControl
  {
    public HtmlH6() : base("h6") { }
  }

  public class HtmlHr : HtmlGenericControl
  {
    public HtmlHr() : base("hr") { }
  }

  public class HtmlLabel : HtmlGenericControl
  {
    public HtmlLabel() : base("label") { }
  }

  public class HtmlLi : HtmlGenericControl
  {
    public HtmlLi() : base("li") { }
  }

  public class HtmlNbsp : Literal
  {
    public HtmlNbsp()
    {
      Text = "&nbsp;";
    }
  }

  public class HtmlP : HtmlGenericControl
  {
    public HtmlP() : base("p") { }
  }

  public class HtmlSpan : HtmlGenericControl
  {
    public HtmlSpan() : base("span") { }
  }

  public class HtmlStrong : HtmlGenericControl
  {
    public HtmlStrong() : base("strong") { }
  }

  public class HtmlUl : HtmlGenericControl
  {
    public HtmlUl() : base("ul") { }
  }

  public class LocalDate : HtmlGenericControl
  {
    private static readonly long BaseTicks = new DateTime(1970, 1, 1).Ticks;

    public LocalDate(string format = null) : this(DateTime.UtcNow, format) {}

    public LocalDate(DateTime utcDate, string format = null) : base("span")
    {
      Attributes.Add("class", "localdate");
      Attributes.Add("ticks",
        (utcDate.Ticks - BaseTicks).ToString(CultureInfo.InvariantCulture));
      if (!string.IsNullOrWhiteSpace(format)) Attributes.Add("format", format);
    }

    public static string AsString(string format = null)
    {
      return new LocalDate(format).RenderToString();
    }

    public static string AsString(DateTime utcDate, string format = null)
    {
      return new LocalDate(utcDate, format).RenderToString();
    }
  }

  public static class Utility
  {
    public static string GetControlValue(Control control)
    {
      var textBox = control as TextBox;
      if (textBox != null) return textBox.Text;

      var listControl = control as ListControl;
      if (listControl != null) return listControl.SelectedValue;

      var htmlSelect = control as HtmlSelect;
      if (htmlSelect != null) return htmlSelect.Value;

      var htmlInputCheckBox = control as HtmlInputCheckBox;
      if (htmlInputCheckBox != null) return htmlInputCheckBox.Checked.ToString();

      var htmlInputHidden = control as HtmlInputHidden;
      if (htmlInputHidden != null) return htmlInputHidden.Value;

      var htmlGenericControl = control as HtmlGenericControl;
      if (htmlGenericControl != null)
      {
        if (htmlGenericControl.HasClass("radio-container"))
        {
          foreach (var child in htmlGenericControl.Controls)
          {
            var htmlInputRadioButton = child as HtmlInputRadioButton;
            if (htmlInputRadioButton != null && htmlInputRadioButton.Checked)
              return htmlInputRadioButton.Value;
          }
          return string.Empty;
        }
      }

      throw new ApplicationException("Unsupported Control type");
    }

    public static void PopulateEmpty(DropDownList dropDownList,
      string text = "<none>")
    {
      dropDownList.Items.Clear();
      dropDownList.AddItem(text, string.Empty, true);
    }

    public static void PopulateEmpty(HtmlSelect dropDownList,
      string text = "<none>")
    {
      dropDownList.Items.Clear();
      dropDownList.AddItem(text, string.Empty, true);
    }

    public static void PopulateFromList(DropDownList dropDownList,
      IEnumerable<SimpleListItem> list, string valueToSelect = null)
    {
      dropDownList.Items.Clear();
      foreach (var item in list) dropDownList.AddItem(item.Text, item.Value, item.Value == valueToSelect);
    }

    public static void PopulateFromList(HtmlSelect dropDownList,
      IEnumerable<SimpleListItem> list, string valueToSelect = null)
    {
      dropDownList.Items.Clear();
      foreach (var item in list) dropDownList.AddItem(item.Text, item.Value, item.Value == valueToSelect);
    }

    public static void SetControlValue(Control control, string value)
    {
      var textBox = control as TextBox;
      if (textBox != null)
      {
        textBox.Text = value;
        return;
      }

      var listControl = control as ListControl;
      if (listControl != null)
      {
        // Don't throw exception if value not found, just select first
        if (listControl.Items.OfType<ListItem>()
          .Any(i => i.Value == value)) listControl.SelectedValue = value;
        else if (listControl.Items.Count > 0) listControl.SelectedIndex = 0;
        return;
      }

      var htmlSelect = control as HtmlSelect;
      if (htmlSelect != null)
      {
        // Don't throw exception if value not found, just select first
        if (htmlSelect.Items.OfType<ListItem>()
          .Any(i => i.Value == value)) htmlSelect.Value = value;
        else if (htmlSelect.Items.Count > 0) htmlSelect.SelectedIndex = 0;
        return;
      }

      var htmlInputCheckBox = control as HtmlInputCheckBox;
      if (htmlInputCheckBox != null)
      {
        bool isChecked;
        bool.TryParse(value, out isChecked);
        htmlInputCheckBox.Checked = isChecked;
        return;
      }

      var htmlInputHidden = control as HtmlInputHidden;
      if (htmlInputHidden != null)
      {
        htmlInputHidden.Value = value;
        return;
      }

      var htmlGenericControl = control as HtmlGenericControl;
      if (htmlGenericControl != null)
      {
        if (htmlGenericControl.HasClass("radio-container"))
        {
          foreach (var child in htmlGenericControl.Controls)
          {
            var htmlInputRadioButton = child as HtmlInputRadioButton;
            if (htmlInputRadioButton != null)
              htmlInputRadioButton.Checked = htmlInputRadioButton.Value == value;
          }
          return;
        }
      }

      throw new ApplicationException("Unsupported Control type");
    }
  }
}