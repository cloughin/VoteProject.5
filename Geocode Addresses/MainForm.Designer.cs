namespace Geocode_Addresses
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
      this.ServerGroupBox = new System.Windows.Forms.GroupBox();
      this.LiveServerRadioButton = new System.Windows.Forms.RadioButton();
      this.TestServerRadioButton = new System.Windows.Forms.RadioButton();
      this.MaxAttemptsTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.StartButton = new System.Windows.Forms.Button();
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.ServerGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // ServerGroupBox
      // 
      this.ServerGroupBox.Controls.Add(this.LiveServerRadioButton);
      this.ServerGroupBox.Controls.Add(this.TestServerRadioButton);
      this.ServerGroupBox.Location = new System.Drawing.Point(12, 12);
      this.ServerGroupBox.Name = "ServerGroupBox";
      this.ServerGroupBox.Size = new System.Drawing.Size(114, 42);
      this.ServerGroupBox.TabIndex = 10;
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
      // MaxAttemptsTextBox
      // 
      this.MaxAttemptsTextBox.Location = new System.Drawing.Point(98, 63);
      this.MaxAttemptsTextBox.Name = "MaxAttemptsTextBox";
      this.MaxAttemptsTextBox.Size = new System.Drawing.Size(100, 20);
      this.MaxAttemptsTextBox.TabIndex = 19;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 66);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(77, 13);
      this.label1.TabIndex = 18;
      this.label1.Text = "Max Attempts: ";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(16, 94);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 13);
      this.label2.TabIndex = 15;
      this.label2.Text = "Status:";
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(304, 429);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 17;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // StatusTextBox
      // 
      this.StatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.StatusTextBox.Location = new System.Drawing.Point(19, 110);
      this.StatusTextBox.MaxLength = 1000000;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.StatusTextBox.Size = new System.Drawing.Size(360, 312);
      this.StatusTextBox.TabIndex = 16;
      // 
      // BackgroundWorker
      // 
      this.BackgroundWorker.WorkerReportsProgress = true;
      this.BackgroundWorker.WorkerSupportsCancellation = true;
      this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(384, 462);
      this.Controls.Add(this.MaxAttemptsTextBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.StartButton);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.ServerGroupBox);
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "Geo Code Addresses";
      this.ServerGroupBox.ResumeLayout(false);
      this.ServerGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox ServerGroupBox;
    private System.Windows.Forms.RadioButton LiveServerRadioButton;
    private System.Windows.Forms.RadioButton TestServerRadioButton;
    private System.Windows.Forms.TextBox MaxAttemptsTextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.TextBox StatusTextBox;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
  }
}

