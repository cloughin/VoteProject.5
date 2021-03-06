using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.String;

namespace Vote
{
  [ToolboxData(
     "<{0}:DropDownListWithOptionGroup runat=\"server\"></{0}:DropDownListWithOptionGroup>"
   )]
  public class DropDownListWithOptionGroup : DropDownList
  {
    private const string OptionGroupTag = "optgroup";
    private const string OptionTag = "option";

    protected override void RenderContents(HtmlTextWriter writer)
    {
      var items = Items;
      var count = items.Count;
      if (count <= 0) return;
      var flag = false;
      var optgrouppending = false;
      for (var i = 0; i < count; i++)
      {
        var tag = OptionTag;
        string optgroupLabel = null;
        var item = items[i];
        if (!item.Enabled) continue;
        if (item.Attributes.Count > 0 && item.Attributes[OptionGroupTag] != null)
        {
          tag = OptionGroupTag;
          optgroupLabel = item.Attributes[OptionGroupTag];
          if (optgrouppending) writer.WriteEndTag(OptionGroupTag);
        }
        writer.WriteBeginTag(tag);
        if (!IsNullOrEmpty(optgroupLabel))
          writer.WriteAttribute("label", optgroupLabel);
        else
        {
          if (item.Selected)
          {
            if (flag)
              VerifyMultiSelect();
            flag = true;
            writer.WriteAttribute("selected", "selected");
          }
          writer.WriteAttribute("value", item.Value, true);
          if (item.Attributes.Count > 0)
            item.Attributes.Render(writer);
          Page?.ClientScript.RegisterForEventValidation(UniqueID, item.Value);
        }
        writer.Write('>');
        HttpUtility.HtmlEncode(item.Text, writer);
        if (tag == OptionGroupTag)
          optgrouppending = true;
        else
          writer.WriteEndTag(tag);
        writer.WriteLine();
      }
      if (optgrouppending) writer.WriteEndTag(OptionGroupTag);
    }

    protected override object SaveViewState()
    {
      var state = new object[Items.Count + 1];
      var baseState = base.SaveViewState();
      state[0] = baseState;
      var itemHasAttributes = false;

      for (var i = 0; i < Items.Count; i++)
      {
        if (Items[i].Attributes.Count <= 0) continue;
        itemHasAttributes = true;
        var attributes = new object[Items[i].Attributes.Count * 2];
        var k = 0;

        foreach (string key in Items[i].Attributes.Keys)
        {
          attributes[k] = key;
          k++;
          attributes[k] = Items[i].Attributes[key];
          k++;
        }
        state[i + 1] = attributes;
      }

      return itemHasAttributes ? state : baseState;
    }

    protected override void LoadViewState(object savedState)
    {
      if (savedState == null)
        return;

      if (!(savedState.GetType()
        .GetElementType() == null) && savedState.GetType()
        .GetElementType() == typeof (object))
      {
        var state = (object[]) savedState;
        base.LoadViewState(state[0]);

        for (var i = 1; i < state.Length; i++)
        {
          if (state[i] == null) continue;
          var attributes = (object[]) state[i];
          for (var k = 0; k < attributes.Length; k += 2)
            Items[i - 1].Attributes.Add(
              attributes[k].ToString(), attributes[k + 1].ToString());
        }
      }
      else
        base.LoadViewState(savedState);
    }
  }
}