namespace BallotPediaUpdateLinks
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
      this.label1 = new System.Windows.Forms.Label();
      this.StartButton = new System.Windows.Forms.Button();
      this.InputOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.InputBrowseButton = new System.Windows.Forms.Button();
      this.InputTextBox = new System.Windows.Forms.TextBox();
      this.ServerGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // ServerGroupBox
      // 
      this.ServerGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ServerGroupBox.Controls.Add(this.LiveServerRadioButton);
      this.ServerGroupBox.Controls.Add(this.TestServerRadioButton);
      this.ServerGroupBox.Location = new System.Drawing.Point(360, 16);
      this.ServerGroupBox.Name = "ServerGroupBox";
      this.ServerGroupBox.Size = new System.Drawing.Size(114, 42);
      this.ServerGroupBox.TabIndex = 38;
      this.ServerGroupBox.TabStop = false;
      this.ServerGroupBox.Text = "Server";
      // 
      // LiveServerRadioButton
      // 
      this.LiveServerRadioButton.AutoSize = true;
      this.LiveServerRadioButton.Location = new System.Drawing.Point(67, 19);
      this.LiveServerRadioButton.Name = "LiveServerRadioButton";
      this.LiveServerRadioButton.Size = new System.Drawing.Size(45, 17);
      this.LiveServerRadioButton.TabIndex = 1;
      this.LiveServerRadioButton.Text = "Live";
      this.LiveServerRadioButton.UseVisualStyleBackColor = true;
      this.LiveServerRadioButton.Click += new System.EventHandler(this.ServerRadioButton_CheckedChanged);
      // 
      // TestServerRadioButton
      // 
      this.TestServerRadioButton.AutoSize = true;
      this.TestServerRadioButton.Checked = true;
      this.TestServerRadioButton.Location = new System.Drawing.Point(6, 19);
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
      this.label1.Location = new System.Drawing.Point(11, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(58, 13);
      this.label1.TabIndex = 34;
      this.label1.Text = "Input CSV:";
      // 
      // StartButton
      // 
      this.StartButton.Location = new System.Drawing.Point(399, 331);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 37;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // InputBrowseButton
      // 
      this.InputBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.InputBrowseButton.Location = new System.Drawing.Point(279, 22);
      this.InputBrowseButton.Name = "InputBrowseButton";
      this.InputBrowseButton.Size = new System.Drawing.Size(75, 23);
      this.InputBrowseButton.TabIndex = 36;
      this.InputBrowseButton.Text = "Browse...";
      this.InputBrowseButton.UseVisualStyleBackColor = true;
      this.InputBrowseButton.Click += new System.EventHandler(this.InputBrowseButton_Click);
      // 
      // InputTextBox
      // 
      this.InputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.InputTextBox.Location = new System.Drawing.Point(14, 24);
      this.InputTextBox.Name = "InputTextBox";
      this.InputTextBox.Size = new System.Drawing.Size(259, 20);
      this.InputTextBox.TabIndex = 35;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(484, 362);
      this.Controls.Add(this.ServerGroupBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.StartButton);
      this.Controls.Add(this.InputBrowseButton);
      this.Controls.Add(this.InputTextBox);
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "BallotPedia Update Links";
      this.ServerGroupBox.ResumeLayout(false);
      this.ServerGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox ServerGroupBox;
    private System.Windows.Forms.RadioButton LiveServerRadioButton;
    private System.Windows.Forms.RadioButton TestServerRadioButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.OpenFileDialog InputOpenFileDialog;
    private System.Windows.Forms.Button InputBrowseButton;
    private System.Windows.Forms.TextBox InputTextBox;

  }
}

