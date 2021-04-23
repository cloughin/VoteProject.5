namespace LoadZipDatabase
{
  partial class CsvMainForm
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
      this.BrowseZipPlus4PathButton = new System.Windows.Forms.Button();
      this.ZipPlus4PathTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.OutputFileTextBox = new System.Windows.Forms.TextBox();
      this.BrowseOutputFileButton = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.StartButton = new System.Windows.Forms.Button();
      this.ZipPlus4FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.OutputFileSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.SuspendLayout();
      // 
      // BrowseZipPlus4PathButton
      // 
      this.BrowseZipPlus4PathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BrowseZipPlus4PathButton.Location = new System.Drawing.Point(405, 23);
      this.BrowseZipPlus4PathButton.Name = "BrowseZipPlus4PathButton";
      this.BrowseZipPlus4PathButton.Size = new System.Drawing.Size(75, 23);
      this.BrowseZipPlus4PathButton.TabIndex = 5;
      this.BrowseZipPlus4PathButton.Text = "Browse...";
      this.BrowseZipPlus4PathButton.UseVisualStyleBackColor = true;
      this.BrowseZipPlus4PathButton.Click += new System.EventHandler(this.BrowseZipPlus4PathButton_Click);
      // 
      // ZipPlus4PathTextBox
      // 
      this.ZipPlus4PathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ZipPlus4PathTextBox.Location = new System.Drawing.Point(15, 25);
      this.ZipPlus4PathTextBox.Name = "ZipPlus4PathTextBox";
      this.ZipPlus4PathTextBox.Size = new System.Drawing.Size(384, 20);
      this.ZipPlus4PathTextBox.TabIndex = 4;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(103, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Path to Zip+4 folder:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 52);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(58, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Output file:";
      // 
      // OutputFileTextBox
      // 
      this.OutputFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.OutputFileTextBox.Location = new System.Drawing.Point(15, 68);
      this.OutputFileTextBox.Name = "OutputFileTextBox";
      this.OutputFileTextBox.Size = new System.Drawing.Size(384, 20);
      this.OutputFileTextBox.TabIndex = 7;
      // 
      // BrowseOutputFileButton
      // 
      this.BrowseOutputFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BrowseOutputFileButton.Location = new System.Drawing.Point(405, 66);
      this.BrowseOutputFileButton.Name = "BrowseOutputFileButton";
      this.BrowseOutputFileButton.Size = new System.Drawing.Size(75, 23);
      this.BrowseOutputFileButton.TabIndex = 8;
      this.BrowseOutputFileButton.Text = "Browse...";
      this.BrowseOutputFileButton.UseVisualStyleBackColor = true;
      this.BrowseOutputFileButton.Click += new System.EventHandler(this.BrowseOutputFileButton_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 95);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(40, 13);
      this.label3.TabIndex = 14;
      this.label3.Text = "Status:";
      // 
      // StatusTextBox
      // 
      this.StatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.StatusTextBox.Location = new System.Drawing.Point(15, 111);
      this.StatusTextBox.MaxLength = 1000000;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.Size = new System.Drawing.Size(465, 214);
      this.StatusTextBox.TabIndex = 15;
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(405, 331);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 16;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // ZipPlus4FolderBrowserDialog
      // 
      this.ZipPlus4FolderBrowserDialog.Description = "Find Zip+4 folder";
      this.ZipPlus4FolderBrowserDialog.ShowNewFolderButton = false;
      // 
      // BackgroundWorker
      // 
      this.BackgroundWorker.WorkerReportsProgress = true;
      this.BackgroundWorker.WorkerSupportsCancellation = true;
      this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      // 
      // OutputFileSaveFileDialog
      // 
      this.OutputFileSaveFileDialog.DefaultExt = "csv";
      this.OutputFileSaveFileDialog.Filter = "csv files|*.csv|All files|*.*";
      // 
      // CsvMainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(492, 366);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.StartButton);
      this.Controls.Add(this.BrowseOutputFileButton);
      this.Controls.Add(this.OutputFileTextBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.BrowseZipPlus4PathButton);
      this.Controls.Add(this.ZipPlus4PathTextBox);
      this.Controls.Add(this.label1);
      this.Name = "CsvMainForm";
      this.ShowIcon = false;
      this.Text = "Load Zip+4 to CSV";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button BrowseZipPlus4PathButton;
    private System.Windows.Forms.TextBox ZipPlus4PathTextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox OutputFileTextBox;
    private System.Windows.Forms.Button BrowseOutputFileButton;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox StatusTextBox;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.FolderBrowserDialog ZipPlus4FolderBrowserDialog;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
    private System.Windows.Forms.SaveFileDialog OutputFileSaveFileDialog;
  }
}