namespace CreateCsvFromLogAddressesGood
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
      this.label1 = new System.Windows.Forms.Label();
      this.OutputFileTextBox = new System.Windows.Forms.TextBox();
      this.BrowseOutputFileButton = new System.Windows.Forms.Button();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.SaveOutputFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.label2 = new System.Windows.Forms.Label();
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.StartButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(58, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Output file:";
      // 
      // OutputFileTextBox
      // 
      this.OutputFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.OutputFileTextBox.Location = new System.Drawing.Point(15, 25);
      this.OutputFileTextBox.Name = "OutputFileTextBox";
      this.OutputFileTextBox.Size = new System.Drawing.Size(344, 20);
      this.OutputFileTextBox.TabIndex = 1;
      // 
      // BrowseOutputFileButton
      // 
      this.BrowseOutputFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BrowseOutputFileButton.Location = new System.Drawing.Point(365, 23);
      this.BrowseOutputFileButton.Name = "BrowseOutputFileButton";
      this.BrowseOutputFileButton.Size = new System.Drawing.Size(75, 23);
      this.BrowseOutputFileButton.TabIndex = 17;
      this.BrowseOutputFileButton.Text = "Browse...";
      this.BrowseOutputFileButton.UseVisualStyleBackColor = true;
      this.BrowseOutputFileButton.Click += new System.EventHandler(this.BrowseOutputFileButton_Click);
      // 
      // BackgroundWorker
      // 
      this.BackgroundWorker.WorkerReportsProgress = true;
      this.BackgroundWorker.WorkerSupportsCancellation = true;
      this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      // 
      // SaveOutputFileDialog
      // 
      this.SaveOutputFileDialog.DefaultExt = "csv";
      this.SaveOutputFileDialog.Filter = "CSV files|*.csv|All files|*.*";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 56);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 13);
      this.label2.TabIndex = 20;
      this.label2.Text = "Status:";
      // 
      // StatusTextBox
      // 
      this.StatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.StatusTextBox.Location = new System.Drawing.Point(15, 72);
      this.StatusTextBox.MaxLength = 1000000;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.Size = new System.Drawing.Size(425, 151);
      this.StatusTextBox.TabIndex = 21;
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(365, 231);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 22;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(452, 266);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.StartButton);
      this.Controls.Add(this.BrowseOutputFileButton);
      this.Controls.Add(this.OutputFileTextBox);
      this.Controls.Add(this.label1);
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "Create CSV From LogAddressesGood";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox OutputFileTextBox;
    private System.Windows.Forms.Button BrowseOutputFileButton;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
    private System.Windows.Forms.SaveFileDialog SaveOutputFileDialog;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox StatusTextBox;
    private System.Windows.Forms.Button StartButton;
  }
}

