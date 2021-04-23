namespace TestRedirect
{
  partial class BuildZipSingleUSZD
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
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.StartButton = new System.Windows.Forms.Button();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.ServerGroupBox = new System.Windows.Forms.GroupBox();
      this.LiveServerRadioButton = new System.Windows.Forms.RadioButton();
      this.TestServerRadioButton = new System.Windows.Forms.RadioButton();
      this.SuppressUpdateCheckBox = new System.Windows.Forms.CheckBox();
      this.StartAtTextBox = new System.Windows.Forms.TextBox();
      this.StartAtLabel = new System.Windows.Forms.Label();
      this.ServerGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(15, 62);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(40, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Status:";
      // 
      // StatusTextBox
      // 
      this.StatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.StatusTextBox.Location = new System.Drawing.Point(18, 78);
      this.StatusTextBox.MaxLength = 327670;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.StatusTextBox.Size = new System.Drawing.Size(458, 247);
      this.StatusTextBox.TabIndex = 6;
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(401, 331);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 5;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // BackgroundWorker
      // 
      this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      // 
      // ServerGroupBox
      // 
      this.ServerGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ServerGroupBox.Controls.Add(this.LiveServerRadioButton);
      this.ServerGroupBox.Controls.Add(this.TestServerRadioButton);
      this.ServerGroupBox.Location = new System.Drawing.Point(359, 14);
      this.ServerGroupBox.Name = "ServerGroupBox";
      this.ServerGroupBox.Size = new System.Drawing.Size(114, 42);
      this.ServerGroupBox.TabIndex = 25;
      this.ServerGroupBox.TabStop = false;
      this.ServerGroupBox.Text = "Server";
      // 
      // LiveServerRadioButton
      // 
      this.LiveServerRadioButton.AutoSize = true;
      this.LiveServerRadioButton.Location = new System.Drawing.Point(68, 18);
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
      this.TestServerRadioButton.Location = new System.Drawing.Point(7, 18);
      this.TestServerRadioButton.Name = "TestServerRadioButton";
      this.TestServerRadioButton.Size = new System.Drawing.Size(46, 17);
      this.TestServerRadioButton.TabIndex = 0;
      this.TestServerRadioButton.TabStop = true;
      this.TestServerRadioButton.Text = "Test";
      this.TestServerRadioButton.UseVisualStyleBackColor = true;
      this.TestServerRadioButton.CheckedChanged += new System.EventHandler(this.ServerRadioButton_CheckedChanged);
      // 
      // SuppressUpdateCheckBox
      // 
      this.SuppressUpdateCheckBox.AutoSize = true;
      this.SuppressUpdateCheckBox.Location = new System.Drawing.Point(20, 16);
      this.SuppressUpdateCheckBox.Name = "SuppressUpdateCheckBox";
      this.SuppressUpdateCheckBox.Size = new System.Drawing.Size(106, 17);
      this.SuppressUpdateCheckBox.TabIndex = 24;
      this.SuppressUpdateCheckBox.Text = "Suppress update";
      this.SuppressUpdateCheckBox.UseVisualStyleBackColor = true;
      // 
      // StartAtTextBox
      // 
      this.StartAtTextBox.Location = new System.Drawing.Point(68, 40);
      this.StartAtTextBox.Name = "StartAtTextBox";
      this.StartAtTextBox.Size = new System.Drawing.Size(100, 20);
      this.StartAtTextBox.TabIndex = 27;
      // 
      // StartAtLabel
      // 
      this.StartAtLabel.AutoSize = true;
      this.StartAtLabel.Location = new System.Drawing.Point(17, 43);
      this.StartAtLabel.Name = "StartAtLabel";
      this.StartAtLabel.Size = new System.Drawing.Size(44, 13);
      this.StartAtLabel.TabIndex = 26;
      this.StartAtLabel.Text = "Start at:";
      // 
      // BuildZipSingleUSZD
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(492, 366);
      this.Controls.Add(this.StartAtTextBox);
      this.Controls.Add(this.StartAtLabel);
      this.Controls.Add(this.ServerGroupBox);
      this.Controls.Add(this.SuppressUpdateCheckBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.StartButton);
      this.Name = "BuildZipSingleUSZD";
      this.ShowIcon = false;
      this.Text = "Build ZipSingleUSZD";
      this.ServerGroupBox.ResumeLayout(false);
      this.ServerGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox StatusTextBox;
    private System.Windows.Forms.Button StartButton;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
    private System.Windows.Forms.GroupBox ServerGroupBox;
    private System.Windows.Forms.RadioButton LiveServerRadioButton;
    private System.Windows.Forms.RadioButton TestServerRadioButton;
    private System.Windows.Forms.CheckBox SuppressUpdateCheckBox;
    private System.Windows.Forms.TextBox StartAtTextBox;
    private System.Windows.Forms.Label StartAtLabel;

  }
}