namespace AnalyzeSqlLog
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.label1 = new System.Windows.Forms.Label();
      this.FileTextBox = new System.Windows.Forms.TextBox();
      this.BrowseButton = new System.Windows.Forms.Button();
      this.AnalysisTextBox = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.StartButton = new System.Windows.Forms.Button();
      this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.OnlyShowDuplicatesCheckBox = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(26, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "File:";
      // 
      // FileTextBox
      // 
      this.FileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.FileTextBox.Location = new System.Drawing.Point(45, 10);
      this.FileTextBox.Name = "FileTextBox";
      this.FileTextBox.Size = new System.Drawing.Size(154, 20);
      this.FileTextBox.TabIndex = 1;
      // 
      // BrowseButton
      // 
      this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BrowseButton.Location = new System.Drawing.Point(205, 8);
      this.BrowseButton.Name = "BrowseButton";
      this.BrowseButton.Size = new System.Drawing.Size(75, 23);
      this.BrowseButton.TabIndex = 2;
      this.BrowseButton.Text = "Browse...";
      this.BrowseButton.UseVisualStyleBackColor = true;
      this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
      // 
      // AnalysisTextBox
      // 
      this.AnalysisTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.AnalysisTextBox.Location = new System.Drawing.Point(12, 82);
      this.AnalysisTextBox.Multiline = true;
      this.AnalysisTextBox.Name = "AnalysisTextBox";
      this.AnalysisTextBox.ReadOnly = true;
      this.AnalysisTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.AnalysisTextBox.Size = new System.Drawing.Size(268, 143);
      this.AnalysisTextBox.TabIndex = 3;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 66);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(48, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Analysis:";
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(205, 231);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 5;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // OpenFileDialog
      // 
      this.OpenFileDialog.DefaultExt = "log";
      this.OpenFileDialog.Filter = "Log files|*.log";
      this.OpenFileDialog.Title = "Select Sql Log File";
      // 
      // OnlyShowDuplicatesCheckBox
      // 
      this.OnlyShowDuplicatesCheckBox.AutoSize = true;
      this.OnlyShowDuplicatesCheckBox.Checked = true;
      this.OnlyShowDuplicatesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.OnlyShowDuplicatesCheckBox.Location = new System.Drawing.Point(16, 36);
      this.OnlyShowDuplicatesCheckBox.Name = "OnlyShowDuplicatesCheckBox";
      this.OnlyShowDuplicatesCheckBox.Size = new System.Drawing.Size(124, 17);
      this.OnlyShowDuplicatesCheckBox.TabIndex = 6;
      this.OnlyShowDuplicatesCheckBox.Text = "OnlyShowDuplicates";
      this.OnlyShowDuplicatesCheckBox.UseVisualStyleBackColor = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 266);
      this.Controls.Add(this.OnlyShowDuplicatesCheckBox);
      this.Controls.Add(this.StartButton);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.AnalysisTextBox);
      this.Controls.Add(this.BrowseButton);
      this.Controls.Add(this.FileTextBox);
      this.Controls.Add(this.label1);
      this.Name = "Form1";
      this.ShowIcon = false;
      this.Text = "Analyze Sql Log";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox FileTextBox;
    private System.Windows.Forms.Button BrowseButton;
    private System.Windows.Forms.TextBox AnalysisTextBox;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.OpenFileDialog OpenFileDialog;
    private System.Windows.Forms.CheckBox OnlyShowDuplicatesCheckBox;
  }
}

