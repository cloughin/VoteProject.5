using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using DB.Vote;

namespace PoliticianSearchTest
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      foreach (var row in Politicians.GetAllData())
        PoliticiansTest.Insert(row.PoliticianKey,
          row.StateCode, row.FirstName, row.MiddleName, row.Nickname, row.LastName,
          row.Suffix, row.LastName.DoubleMetaphone());
    }
  }
}