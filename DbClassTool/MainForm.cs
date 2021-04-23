using System;
using System.IO;
using System.Windows.Forms;
using GenerateDbClasses;
using static System.String;

namespace DbClassTool
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void BrowsePathButton_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog.SelectedPath = PathTextBox.Text;
      if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
        PathTextBox.Text = FolderBrowserDialog.SelectedPath;
    }

    private void GenerateCodeButton_Click(object sender, EventArgs e)
    {
      if (IsNullOrWhiteSpace(DataBaseComboBox.Text))
      {
        MessageBox.Show("Please select a database");
        return;
      }
      var filePath = Path.Combine(PathTextBox.Text, DataBaseComboBox.Text + ".xml");
      var writer = Generator.GenerateAll(filePath, "DB");
      var generatedCode = writer.ToString();
      // save to designer file
      var designerPath = Path.Combine(PathTextBox.Text, DataBaseComboBox.Text + ".designer.cs");
      File.WriteAllText(designerPath, generatedCode);
      MessageBox.Show("Generated code saved");
    }
  }
}
