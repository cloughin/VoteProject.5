namespace DbClassTool
{
  partial class MainForm
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
      if (disposing && components != null)
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
      this.PathTextBox = new System.Windows.Forms.TextBox();
      this.BrowsePathButton = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.DataBaseComboBox = new System.Windows.Forms.ComboBox();
      this.GenerateCodeButton = new System.Windows.Forms.Button();
      this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(67, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Project path:";
      // 
      // PathTextBox
      // 
      this.PathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PathTextBox.Location = new System.Drawing.Point(76, 12);
      this.PathTextBox.Name = "PathTextBox";
      this.PathTextBox.Size = new System.Drawing.Size(429, 20);
      this.PathTextBox.TabIndex = 1;
      this.PathTextBox.Text = "D:\\Users\\Curt\\Dropbox\\Documents\\AmazonWorkspace\\VoteProject.5\\VoteLibrary\\DB";
      // 
      // BrowsePathButton
      // 
      this.BrowsePathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BrowsePathButton.Location = new System.Drawing.Point(511, 10);
      this.BrowsePathButton.Name = "BrowsePathButton";
      this.BrowsePathButton.Size = new System.Drawing.Size(75, 23);
      this.BrowsePathButton.TabIndex = 2;
      this.BrowsePathButton.Text = "Browse...";
      this.BrowsePathButton.UseVisualStyleBackColor = true;
      this.BrowsePathButton.Click += new System.EventHandler(this.BrowsePathButton_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 52);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(56, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Database:";
      // 
      // DataBaseComboBox
      // 
      this.DataBaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.DataBaseComboBox.FormattingEnabled = true;
      this.DataBaseComboBox.Items.AddRange(new object[] {
            "",
            "Vote",
            "VoteCache",
            "VoteCacheLocal",
            "VoteImagesLocal",
            "VoteLog",
            "VoteTemp",
            "VoteZipNew",
            "VoteZipNewLocal"});
      this.DataBaseComboBox.Location = new System.Drawing.Point(76, 49);
      this.DataBaseComboBox.Name = "DataBaseComboBox";
      this.DataBaseComboBox.Size = new System.Drawing.Size(189, 21);
      this.DataBaseComboBox.TabIndex = 4;
      // 
      // GenerateCodeButton
      // 
      this.GenerateCodeButton.Location = new System.Drawing.Point(76, 90);
      this.GenerateCodeButton.Name = "GenerateCodeButton";
      this.GenerateCodeButton.Size = new System.Drawing.Size(91, 23);
      this.GenerateCodeButton.TabIndex = 5;
      this.GenerateCodeButton.Text = "Generate Code";
      this.GenerateCodeButton.UseVisualStyleBackColor = true;
      this.GenerateCodeButton.Click += new System.EventHandler(this.GenerateCodeButton_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(598, 123);
      this.Controls.Add(this.GenerateCodeButton);
      this.Controls.Add(this.DataBaseComboBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.BrowsePathButton);
      this.Controls.Add(this.PathTextBox);
      this.Controls.Add(this.label1);
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(1400, 162);
      this.MinimumSize = new System.Drawing.Size(400, 162);
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "DbClassTool";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox PathTextBox;
    private System.Windows.Forms.Button BrowsePathButton;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox DataBaseComboBox;
    private System.Windows.Forms.Button GenerateCodeButton;
    private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
  }
}

