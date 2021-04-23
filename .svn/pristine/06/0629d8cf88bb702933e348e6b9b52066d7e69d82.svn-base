using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace AnalyzeSqlLog
{
  public partial class Form1 : Form
  {
    class SelectLine
    {
      public int LineNumber;
      public string Line;
    }

    public Form1()
    {
      InitializeComponent();
    }

    private void BrowseButton_Click(object sender, EventArgs e)
    {
      OpenFileDialog.FileName = FileTextBox.Text;
      if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        FileTextBox.Text = OpenFileDialog.FileName;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      FileInfo fi = new FileInfo(FileTextBox.Text);
      StreamReader fileStream = fi.OpenText();
      Regex selectRegex = new Regex(@".*\t.*\t *\d* *Query *\t(?<select>SELECT .*)");
      int lineNumber = 0;
      List<SelectLine> selectLines = new List<SelectLine>();
      bool onlyShowDuplicates = OnlyShowDuplicatesCheckBox.Checked;

      AnalysisTextBox.Clear();

      while (!fileStream.EndOfStream)
      {
        string line = fileStream.ReadLine();
        lineNumber++;
        Match match = selectRegex.Match(line);
        if (match.Success)
          selectLines.Add(
            new SelectLine() 
            { 
              LineNumber = lineNumber, 
              Line = match.Groups["select"].Captures[0].Value 
            });
      }

      var groups = selectLines.GroupBy(l => l.Line)
        .OrderBy(g => g.Key)
        .ToArray();

      foreach (var group in groups)
      {
        StringBuilder lineNumbers = new StringBuilder();
        int lineNumberCount = 0;
        foreach (var line in group)
        {
          if (lineNumbers.Length != 0)
            lineNumbers.Append(",");
          lineNumbers.Append(line.LineNumber.ToString());
          lineNumberCount++;
        }
        if (!onlyShowDuplicates || lineNumberCount > 1)
        {
          AnalysisTextBox.AppendText("Line numbers: ");
          AnalysisTextBox.AppendText(lineNumbers.ToString());
          AnalysisTextBox.AppendText(Environment.NewLine);
          AnalysisTextBox.AppendText(group.Key);
          AnalysisTextBox.AppendText(Environment.NewLine);
          AnalysisTextBox.AppendText(Environment.NewLine);
        }
      }
      AnalysisTextBox.AppendText("Total selects: ");
      AnalysisTextBox.AppendText(selectLines.Count.ToString());
      AnalysisTextBox.AppendText(Environment.NewLine);
      AnalysisTextBox.AppendText("Unique selects: ");
      AnalysisTextBox.AppendText(groups.Length.ToString());
    }
  }
}
