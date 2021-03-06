namespace LoadDerivedZipTables
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
      this.label2 = new System.Windows.Forms.Label();
      this.ErrorsTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.StartButton = new System.Windows.Forms.Button();
      this.BrowseOutputFileButton = new System.Windows.Forms.Button();
      this.OutputFileTextBox = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.OutputFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.SuspendLayout();
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 339);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(37, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Errors:";
      // 
      // ErrorsTextBox
      // 
      this.ErrorsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ErrorsTextBox.Location = new System.Drawing.Point(15, 355);
      this.ErrorsTextBox.MaxLength = 327670;
      this.ErrorsTextBox.Multiline = true;
      this.ErrorsTextBox.Name = "ErrorsTextBox";
      this.ErrorsTextBox.ReadOnly = true;
      this.ErrorsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.ErrorsTextBox.Size = new System.Drawing.Size(458, 260);
      this.ErrorsTextBox.TabIndex = 6;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 52);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(40, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Status:";
      // 
      // BackgroundWorker
      // 
      this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      // 
      // StatusTextBox
      // 
      this.StatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.StatusTextBox.Location = new System.Drawing.Point(15, 68);
      this.StatusTextBox.MaxLength = 327670;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.StatusTextBox.Size = new System.Drawing.Size(458, 260);
      this.StatusTextBox.TabIndex = 4;
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(398, 621);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 7;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // BrowseOutputFileButton
      // 
      this.BrowseOutputFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BrowseOutputFileButton.Location = new System.Drawing.Point(405, 23);
      this.BrowseOutputFileButton.Name = "BrowseOutputFileButton";
      this.BrowseOutputFileButton.Size = new System.Drawing.Size(75, 23);
      this.BrowseOutputFileButton.TabIndex = 2;
      this.BrowseOutputFileButton.Text = "Browse...";
      this.BrowseOutputFileButton.UseVisualStyleBackColor = true;
      this.BrowseOutputFileButton.Click += new System.EventHandler(this.BrowseOutputFileButton_Click);
      // 
      // OutputFileTextBox
      // 
      this.OutputFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.OutputFileTextBox.Location = new System.Drawing.Point(15, 25);
      this.OutputFileTextBox.Name = "OutputFileTextBox";
      this.OutputFileTextBox.Size = new System.Drawing.Size(384, 20);
      this.OutputFileTextBox.TabIndex = 1;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(113, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Path to csv output file:";
      // 
      // OutputFileDialog
      // 
      this.OutputFileDialog.DefaultExt = "csv";
      this.OutputFileDialog.Filter = "csv files|*.csv|All files|*.*";
      // 
      // CsvMainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(492, 656);
      this.Controls.Add(this.BrowseOutputFileButton);
      this.Controls.Add(this.OutputFileTextBox);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.ErrorsTextBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.StartButton);
      this.Name = "CsvMainForm";
      this.ShowIcon = false;
      this.Text = "Load Streets to CSV";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox ErrorsTextBox;
    private System.Windows.Forms.Label label1;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
    private System.Windows.Forms.TextBox StatusTextBox;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.Button BrowseOutputFileButton;
    private System.Windows.Forms.TextBox OutputFileTextBox;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.SaveFileDialog OutputFileDialog;
  }
}