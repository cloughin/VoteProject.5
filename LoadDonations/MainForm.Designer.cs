namespace LoadDonations
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
      this.OptionsFileBrowseButton = new System.Windows.Forms.Button();
      this.SpreadsheetTextBox = new System.Windows.Forms.TextBox();
      this.OpenOptionsFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.label2 = new System.Windows.Forms.Label();
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.LiveServerRadioButton = new System.Windows.Forms.RadioButton();
      this.TestServerRadioButton = new System.Windows.Forms.RadioButton();
      this.label1 = new System.Windows.Forms.Label();
      this.ServerGroupBox = new System.Windows.Forms.GroupBox();
      this.StartButton = new System.Windows.Forms.Button();
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
      // OptionsFileBrowseButton
      // 
      this.OptionsFileBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.OptionsFileBrowseButton.Location = new System.Drawing.Point(591, 79);
      this.OptionsFileBrowseButton.Name = "OptionsFileBrowseButton";
      this.OptionsFileBrowseButton.Size = new System.Drawing.Size(75, 23);
      this.OptionsFileBrowseButton.TabIndex = 29;
      this.OptionsFileBrowseButton.Text = "Browse...";
      this.OptionsFileBrowseButton.UseVisualStyleBackColor = true;
      this.OptionsFileBrowseButton.Click += new System.EventHandler(this.SpreadsheetBrowseButton_Click);
      // 
      // SpreadsheetTextBox
      // 
      this.SpreadsheetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SpreadsheetTextBox.Location = new System.Drawing.Point(14, 81);
      this.SpreadsheetTextBox.Name = "SpreadsheetTextBox";
      this.SpreadsheetTextBox.Size = new System.Drawing.Size(571, 20);
      this.SpreadsheetTextBox.TabIndex = 28;
      this.SpreadsheetTextBox.Text = "D:\\Users\\Curt\\Dropbox\\Documents\\Vote\\Mantis\\940 Click n Pledge\\2018 Donations Usi" +
    "ng Click & Pledge.xlsx";
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
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(11, 109);
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
      this.StatusTextBox.Location = new System.Drawing.Point(14, 125);
      this.StatusTextBox.MaxLength = 1000000;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.StatusTextBox.Size = new System.Drawing.Size(652, 306);
      this.StatusTextBox.TabIndex = 26;
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
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(11, 65);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(67, 13);
      this.label1.TabIndex = 27;
      this.label1.Text = "Spreadsheet";
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
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(591, 437);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 23;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(676, 472);
      this.Controls.Add(this.UpdateDatabaseCheckBox);
      this.Controls.Add(this.OptionsFileBrowseButton);
      this.Controls.Add(this.SpreadsheetTextBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.ServerGroupBox);
      this.Controls.Add(this.StartButton);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "Load Donations";
      this.ServerGroupBox.ResumeLayout(false);
      this.ServerGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox UpdateDatabaseCheckBox;
    private System.Windows.Forms.Button OptionsFileBrowseButton;
    private System.Windows.Forms.TextBox SpreadsheetTextBox;
    private System.Windows.Forms.OpenFileDialog OpenOptionsFileDialog;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox StatusTextBox;
    private System.Windows.Forms.RadioButton LiveServerRadioButton;
    private System.Windows.Forms.RadioButton TestServerRadioButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox ServerGroupBox;
    private System.Windows.Forms.Button StartButton;
  }
}

