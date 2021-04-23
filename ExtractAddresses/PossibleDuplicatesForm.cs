using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtractAddresses
{
  public partial class PossibleDuplicatesForm : Form
  {
    public PossibleDuplicatesForm()
    {
      InitializeComponent();
    }

    public void AddAddress(string address)
    {
      AddressesCheckedListBox.Items.Add(address);
      AddressesCheckedListBox.SetItemChecked(
        AddressesCheckedListBox.Items.Count - 1, true);
    }

    public bool[] GetChecks()
    {
      bool[] checks = new bool[AddressesCheckedListBox.Items.Count];
      for (int inx = 0; inx < AddressesCheckedListBox.Items.Count; inx++)
        checks[inx] = AddressesCheckedListBox.GetItemChecked(inx);
      return checks;
    }

    public bool StopPresenting
    {
      get
      {
        return StopPresentingCheckBox.Checked;
      }
    }

    #region Event handlers

    private void ContinueButton_Click(object sender, EventArgs e)
    {
      Close();
    }

    #endregion Event handlers
  }
}
