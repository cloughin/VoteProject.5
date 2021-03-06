namespace GetPoliticianImagesFromLogs
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
      this.label2 = new System.Windows.Forms.Label();
      this.StartButton = new System.Windows.Forms.Button();
      this.LiveServerRadioButton = new System.Windows.Forms.RadioButton();
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.ServerGroupBox = new System.Windows.Forms.GroupBox();
      this.TestServerRadioButton = new System.Windows.Forms.RadioButton();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.label1 = new System.Windows.Forms.Label();
      this.PoliticianKeyTextBox = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.FolderTextBox = new System.Windows.Forms.TextBox();
      this.BrowseButton = new System.Windows.Forms.Button();
      this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.ServerGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(11, 131);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "Status:";
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(299, 427);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 12;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // LiveServerRadioButton
      // 
      this.LiveServerRadioButton.AutoSize = true;
      this.LiveServerRadioButton.Checked = true;
      this.LiveServerRadioButton.Location = new System.Drawing.Point(68, 20);
      this.LiveServerRadioButton.Name = "LiveServerRadioButton";
      this.LiveServerRadioButton.Size = new System.Drawing.Size(45, 17);
      this.LiveServerRadioButton.TabIndex = 1;
      this.LiveServerRadioButton.TabStop = true;
      this.LiveServerRadioButton.Text = "Live";
      this.LiveServerRadioButton.UseVisualStyleBackColor = true;
      this.LiveServerRadioButton.CheckedChanged += new System.EventHandler(this.ServerRadioButton_CheckedChanged);
      // 
      // StatusTextBox
      // 
      this.StatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.StatusTextBox.Location = new System.Drawing.Point(14, 147);
      this.StatusTextBox.MaxLength = 1000000;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.StatusTextBox.Size = new System.Drawing.Size(360, 273);
      this.StatusTextBox.TabIndex = 11;
      // 
      // ServerGroupBox
      // 
      this.ServerGroupBox.Controls.Add(this.LiveServerRadioButton);
      this.ServerGroupBox.Controls.Add(this.TestServerRadioButton);
      this.ServerGroupBox.Location = new System.Drawing.Point(14, 12);
      this.ServerGroupBox.Name = "ServerGroupBox";
      this.ServerGroupBox.Size = new System.Drawing.Size(114, 42);
      this.ServerGroupBox.TabIndex = 9;
      this.ServerGroupBox.TabStop = false;
      this.ServerGroupBox.Text = "Server";
      // 
      // TestServerRadioButton
      // 
      this.TestServerRadioButton.AutoSize = true;
      this.TestServerRadioButton.Location = new System.Drawing.Point(7, 20);
      this.TestServerRadioButton.Name = "TestServerRadioButton";
      this.TestServerRadioButton.Size = new System.Drawing.Size(46, 17);
      this.TestServerRadioButton.TabIndex = 0;
      this.TestServerRadioButton.Text = "Test";
      this.TestServerRadioButton.UseVisualStyleBackColor = true;
      this.TestServerRadioButton.CheckedChanged += new System.EventHandler(this.ServerRadioButton_CheckedChanged);
      // 
      // BackgroundWorker
      // 
      this.BackgroundWorker.WorkerReportsProgress = true;
      this.BackgroundWorker.WorkerSupportsCancellation = true;
      this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(11, 65);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(76, 13);
      this.label1.TabIndex = 13;
      this.label1.Text = "Politician Key: ";
      // 
      // PoliticianKeyTextBox
      // 
      this.PoliticianKeyTextBox.Location = new System.Drawing.Point(93, 62);
      this.PoliticianKeyTextBox.Name = "PoliticianKeyTextBox";
      this.PoliticianKeyTextBox.Size = new System.Drawing.Size(100, 20);
      this.PoliticianKeyTextBox.TabIndex = 14;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 93);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(67, 13);
      this.label3.TabIndex = 15;
      this.label3.Text = "Save Folder:";
      // 
      // FolderTextBox
      // 
      this.FolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.FolderTextBox.Location = new System.Drawing.Point(93, 90);
      this.FolderTextBox.Name = "FolderTextBox";
      this.FolderTextBox.Size = new System.Drawing.Size(198, 20);
      this.FolderTextBox.TabIndex = 16;
      // 
      // BrowseButton
      // 
      this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BrowseButton.Location = new System.Drawing.Point(297, 87);
      this.BrowseButton.Name = "BrowseButton";
      this.BrowseButton.Size = new System.Drawing.Size(75, 23);
      this.BrowseButton.TabIndex = 17;
      this.BrowseButton.Text = "Browse...";
      this.BrowseButton.UseVisualStyleBackColor = true;
      this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(384, 462);
      this.Controls.Add(this.BrowseButton);
      this.Controls.Add(this.FolderTextBox);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.PoliticianKeyTextBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.StartButton);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.ServerGroupBox);
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "Get Politician Images From Log";
      this.ServerGroupBox.ResumeLayout(false);
      this.ServerGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.RadioButton LiveServerRadioButton;
    private System.Windows.Forms.TextBox StatusTextBox;
    private System.Windows.Forms.GroupBox ServerGroupBox;
    private System.Windows.Forms.RadioButton TestServerRadioButton;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox PoliticianKeyTextBox;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox FolderTextBox;
    private System.Windows.Forms.Button BrowseButton;
    private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
  }
}

