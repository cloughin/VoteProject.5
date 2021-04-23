namespace SetUpIssues
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
      this.UpdateDatabaseCheckBox = new System.Windows.Forms.CheckBox();
      this.SaveFolderBrowseButton = new System.Windows.Forms.Button();
      this.SaveFolderTextBox = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.OptionsFileBrowseButton = new System.Windows.Forms.Button();
      this.OptionsFileTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.ServerGroupBox = new System.Windows.Forms.GroupBox();
      this.LiveServerRadioButton = new System.Windows.Forms.RadioButton();
      this.TestServerRadioButton = new System.Windows.Forms.RadioButton();
      this.StartButton = new System.Windows.Forms.Button();
      this.OpenOptionsFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.SaveFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.label4 = new System.Windows.Forms.Label();
      this.AnswersFileTextBox = new System.Windows.Forms.TextBox();
      this.AnswersFileBrowseButton = new System.Windows.Forms.Button();
      this.OpenAnswersFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.ServerGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // UpdateDatabaseCheckBox
      // 
      this.UpdateDatabaseCheckBox.AutoSize = true;
      this.UpdateDatabaseCheckBox.Location = new System.Drawing.Point(162, 22);
      this.UpdateDatabaseCheckBox.Name = "UpdateDatabaseCheckBox";
      this.UpdateDatabaseCheckBox.Size = new System.Drawing.Size(110, 17);
      this.UpdateDatabaseCheckBox.TabIndex = 33;
      this.UpdateDatabaseCheckBox.Text = "Update Database";
      this.UpdateDatabaseCheckBox.UseVisualStyleBackColor = true;
      // 
      // SaveFolderBrowseButton
      // 
      this.SaveFolderBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.SaveFolderBrowseButton.Location = new System.Drawing.Point(591, 189);
      this.SaveFolderBrowseButton.Name = "SaveFolderBrowseButton";
      this.SaveFolderBrowseButton.Size = new System.Drawing.Size(75, 23);
      this.SaveFolderBrowseButton.TabIndex = 32;
      this.SaveFolderBrowseButton.Text = "Browse...";
      this.SaveFolderBrowseButton.UseVisualStyleBackColor = true;
      this.SaveFolderBrowseButton.Click += new System.EventHandler(this.SaveFolderBrowseButton_Click);
      // 
      // SaveFolderTextBox
      // 
      this.SaveFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SaveFolderTextBox.Location = new System.Drawing.Point(14, 191);
      this.SaveFolderTextBox.Name = "SaveFolderTextBox";
      this.SaveFolderTextBox.Size = new System.Drawing.Size(571, 20);
      this.SaveFolderTextBox.TabIndex = 31;
      this.SaveFolderTextBox.Text = "D:\\Users\\Curt\\Dropbox\\Documents\\Vote\\Issues\\csvs";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(11, 175);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(64, 13);
      this.label3.TabIndex = 30;
      this.label3.Text = "Save Folder";
      // 
      // OptionsFileBrowseButton
      // 
      this.OptionsFileBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.OptionsFileBrowseButton.Location = new System.Drawing.Point(591, 79);
      this.OptionsFileBrowseButton.Name = "OptionsFileBrowseButton";
      this.OptionsFileBrowseButton.Size = new System.Drawing.Size(75, 23);
      this.OptionsFileBrowseButton.TabIndex = 29;
      this.OptionsFileBrowseButton.Text = "Browse...";
      this.OptionsFileBrowseButton.UseVisualStyleBackColor = true;
      this.OptionsFileBrowseButton.Click += new System.EventHandler(this.OptionsFileBrowseButton_Click);
      // 
      // OptionsFileTextBox
      // 
      this.OptionsFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.OptionsFileTextBox.Location = new System.Drawing.Point(14, 81);
      this.OptionsFileTextBox.Name = "OptionsFileTextBox";
      this.OptionsFileTextBox.Size = new System.Drawing.Size(571, 20);
      this.OptionsFileTextBox.TabIndex = 28;
      this.OptionsFileTextBox.Text = "D:\\Users\\Curt\\Dropbox\\Documents\\AmazonWorkspace\\VoteProject.4\\SetUpIssues\\Options" +
    ".json";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(11, 65);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(73, 13);
      this.label1.TabIndex = 27;
      this.label1.Text = "Exception File";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(11, 226);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 13);
      this.label2.TabIndex = 25;
      this.label2.Text = "Status:";
      // 
      // StatusTextBox
      // 
      this.StatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.StatusTextBox.Location = new System.Drawing.Point(14, 242);
      this.StatusTextBox.MaxLength = 1000000;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.StatusTextBox.Size = new System.Drawing.Size(652, 244);
      this.StatusTextBox.TabIndex = 26;
      // 
      // ServerGroupBox
      // 
      this.ServerGroupBox.Controls.Add(this.LiveServerRadioButton);
      this.ServerGroupBox.Controls.Add(this.TestServerRadioButton);
      this.ServerGroupBox.Location = new System.Drawing.Point(14, 12);
      this.ServerGroupBox.Name = "ServerGroupBox";
      this.ServerGroupBox.Size = new System.Drawing.Size(114, 42);
      this.ServerGroupBox.TabIndex = 24;
      this.ServerGroupBox.TabStop = false;
      this.ServerGroupBox.Text = "Server";
      // 
      // LiveServerRadioButton
      // 
      this.LiveServerRadioButton.AutoSize = true;
      this.LiveServerRadioButton.Location = new System.Drawing.Point(68, 20);
      this.LiveServerRadioButton.Name = "LiveServerRadioButton";
      this.LiveServerRadioButton.Size = new System.Drawing.Size(45, 17);
      this.LiveServerRadioButton.TabIndex = 1;
      this.LiveServerRadioButton.Text = "Live";
      this.LiveServerRadioButton.UseVisualStyleBackColor = true;
      this.LiveServerRadioButton.CheckedChanged += new System.EventHandler(this.ServerRadioButton_CheckedChanged);
      // 
      // TestServerRadioButton
      // 
      this.TestServerRadioButton.AutoSize = true;
      this.TestServerRadioButton.Checked = true;
      this.TestServerRadioButton.Location = new System.Drawing.Point(7, 20);
      this.TestServerRadioButton.Name = "TestServerRadioButton";
      this.TestServerRadioButton.Size = new System.Drawing.Size(46, 17);
      this.TestServerRadioButton.TabIndex = 0;
      this.TestServerRadioButton.TabStop = true;
      this.TestServerRadioButton.Text = "Test";
      this.TestServerRadioButton.UseVisualStyleBackColor = true;
      this.TestServerRadioButton.CheckedChanged += new System.EventHandler(this.ServerRadioButton_CheckedChanged);
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(591, 492);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 23;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // OpenOptionsFileDialog
      // 
      this.OpenOptionsFileDialog.DefaultExt = "json";
      this.OpenOptionsFileDialog.Filter = "JSON files|*.json";
      // 
      // BackgroundWorker
      // 
      this.BackgroundWorker.WorkerReportsProgress = true;
      this.BackgroundWorker.WorkerSupportsCancellation = true;
      this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(11, 120);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(105, 13);
      this.label4.TabIndex = 34;
      this.label4.Text = "Answer Mapping File";
      // 
      // AnswersFileTextBox
      // 
      this.AnswersFileTextBox.Location = new System.Drawing.Point(14, 136);
      this.AnswersFileTextBox.Name = "AnswersFileTextBox";
      this.AnswersFileTextBox.Size = new System.Drawing.Size(571, 20);
      this.AnswersFileTextBox.TabIndex = 35;
      this.AnswersFileTextBox.Text = "D:\\Users\\Curt\\Dropbox\\Documents\\Vote\\Mantis\\823\\Questions-to-topics.csv";
      // 
      // AnswersFileBrowseButton
      // 
      this.AnswersFileBrowseButton.Location = new System.Drawing.Point(591, 134);
      this.AnswersFileBrowseButton.Name = "AnswersFileBrowseButton";
      this.AnswersFileBrowseButton.Size = new System.Drawing.Size(75, 23);
      this.AnswersFileBrowseButton.TabIndex = 36;
      this.AnswersFileBrowseButton.Text = "Browse...";
      this.AnswersFileBrowseButton.UseVisualStyleBackColor = true;
      this.AnswersFileBrowseButton.Click += new System.EventHandler(this.AnswersFileBrowseButton_Click);
      // 
      // OpenAnswersFileDialog
      // 
      this.OpenAnswersFileDialog.DefaultExt = "csv";
      this.OpenAnswersFileDialog.Filter = "CSV files|*.csv";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(676, 527);
      this.Controls.Add(this.AnswersFileBrowseButton);
      this.Controls.Add(this.AnswersFileTextBox);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.UpdateDatabaseCheckBox);
      this.Controls.Add(this.SaveFolderBrowseButton);
      this.Controls.Add(this.SaveFolderTextBox);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.OptionsFileBrowseButton);
      this.Controls.Add(this.OptionsFileTextBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.ServerGroupBox);
      this.Controls.Add(this.StartButton);
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "Setup Issues";
      this.ServerGroupBox.ResumeLayout(false);
      this.ServerGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox UpdateDatabaseCheckBox;
    private System.Windows.Forms.Button SaveFolderBrowseButton;
    private System.Windows.Forms.TextBox SaveFolderTextBox;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button OptionsFileBrowseButton;
    private System.Windows.Forms.TextBox OptionsFileTextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox StatusTextBox;
    private System.Windows.Forms.GroupBox ServerGroupBox;
    private System.Windows.Forms.RadioButton LiveServerRadioButton;
    private System.Windows.Forms.RadioButton TestServerRadioButton;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.OpenFileDialog OpenOptionsFileDialog;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
    private System.Windows.Forms.FolderBrowserDialog SaveFolderBrowserDialog;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox AnswersFileTextBox;
    private System.Windows.Forms.Button AnswersFileBrowseButton;
    private System.Windows.Forms.OpenFileDialog OpenAnswersFileDialog;
  }
}

