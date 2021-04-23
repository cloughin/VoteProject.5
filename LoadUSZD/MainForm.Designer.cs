namespace LoadUSZD
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
      this.label2 = new System.Windows.Forms.Label();
      this.DataFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.StartButton = new System.Windows.Forms.Button();
      this.BrowseDataPathButton = new System.Windows.Forms.Button();
      this.DataPathTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.VersionTextBox = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 58);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 13);
      this.label2.TabIndex = 17;
      this.label2.Text = "Status:";
      // 
      // DataFolderBrowserDialog
      // 
      this.DataFolderBrowserDialog.Description = "Find Data Folder";
      this.DataFolderBrowserDialog.ShowNewFolderButton = false;
      // 
      // StatusTextBox
      // 
      this.StatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.StatusTextBox.Location = new System.Drawing.Point(12, 74);
      this.StatusTextBox.MaxLength = 1000000;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.Size = new System.Drawing.Size(527, 151);
      this.StatusTextBox.TabIndex = 18;
      // 
      // BackgroundWorker
      // 
      this.BackgroundWorker.WorkerReportsProgress = true;
      this.BackgroundWorker.WorkerSupportsCancellation = true;
      this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(464, 231);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 19;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // BrowseDataPathButton
      // 
      this.BrowseDataPathButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BrowseDataPathButton.Location = new System.Drawing.Point(464, 23);
      this.BrowseDataPathButton.Name = "BrowseDataPathButton";
      this.BrowseDataPathButton.Size = new System.Drawing.Size(75, 23);
      this.BrowseDataPathButton.TabIndex = 16;
      this.BrowseDataPathButton.Text = "Browse...";
      this.BrowseDataPathButton.UseVisualStyleBackColor = true;
      this.BrowseDataPathButton.Click += new System.EventHandler(this.BrowseDataPathButton_Click);
      // 
      // DataPathTextBox
      // 
      this.DataPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.DataPathTextBox.Location = new System.Drawing.Point(74, 25);
      this.DataPathTextBox.Name = "DataPathTextBox";
      this.DataPathTextBox.Size = new System.Drawing.Size(384, 20);
      this.DataPathTextBox.TabIndex = 15;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(71, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(100, 13);
      this.label1.TabIndex = 14;
      this.label1.Text = "Path to data  folder:";
      // 
      // VersionTextBox
      // 
      this.VersionTextBox.Location = new System.Drawing.Point(12, 25);
      this.VersionTextBox.Name = "VersionTextBox";
      this.VersionTextBox.Size = new System.Drawing.Size(45, 20);
      this.VersionTextBox.TabIndex = 20;
      this.VersionTextBox.Text = "12.4";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(45, 13);
      this.label3.TabIndex = 21;
      this.label3.Text = "Version:";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(551, 266);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.VersionTextBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.StartButton);
      this.Controls.Add(this.BrowseDataPathButton);
      this.Controls.Add(this.DataPathTextBox);
      this.Controls.Add(this.label1);
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "Load USZD";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.FolderBrowserDialog DataFolderBrowserDialog;
    private System.Windows.Forms.TextBox StatusTextBox;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.Button BrowseDataPathButton;
    private System.Windows.Forms.TextBox DataPathTextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox VersionTextBox;
    private System.Windows.Forms.Label label3;
  }
}

