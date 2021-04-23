namespace NormalizeNewLines
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
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.ServerGroupBox = new System.Windows.Forms.GroupBox();
      this.LiveServerRadioButton = new System.Windows.Forms.RadioButton();
      this.TestServerRadioButton = new System.Windows.Forms.RadioButton();
      this.StartButton = new System.Windows.Forms.Button();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.RepairLineBreaksCheckBox = new System.Windows.Forms.CheckBox();
      this.RepairRedundantSpacesCheckBox = new System.Windows.Forms.CheckBox();
      this.UpdateCheckBox = new System.Windows.Forms.CheckBox();
      this.ServerGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // StatusTextBox
      // 
      this.StatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.StatusTextBox.Location = new System.Drawing.Point(12, 77);
      this.StatusTextBox.MaxLength = 1000000;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.StatusTextBox.Size = new System.Drawing.Size(360, 243);
      this.StatusTextBox.TabIndex = 4;
      // 
      // ServerGroupBox
      // 
      this.ServerGroupBox.Controls.Add(this.LiveServerRadioButton);
      this.ServerGroupBox.Controls.Add(this.TestServerRadioButton);
      this.ServerGroupBox.Location = new System.Drawing.Point(12, 12);
      this.ServerGroupBox.Name = "ServerGroupBox";
      this.ServerGroupBox.Size = new System.Drawing.Size(114, 42);
      this.ServerGroupBox.TabIndex = 0;
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
      this.LiveServerRadioButton.Click += new System.EventHandler(this.ServerRadioButton_CheckedChanged);
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
      this.TestServerRadioButton.Click += new System.EventHandler(this.ServerRadioButton_CheckedChanged);
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(297, 327);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 5;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // BackgroundWorker
      // 
      this.BackgroundWorker.WorkerReportsProgress = true;
      this.BackgroundWorker.WorkerSupportsCancellation = true;
      this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      // 
      // RepairLineBreaksCheckBox
      // 
      this.RepairLineBreaksCheckBox.AutoSize = true;
      this.RepairLineBreaksCheckBox.Location = new System.Drawing.Point(160, 7);
      this.RepairLineBreaksCheckBox.Name = "RepairLineBreaksCheckBox";
      this.RepairLineBreaksCheckBox.Size = new System.Drawing.Size(116, 17);
      this.RepairLineBreaksCheckBox.TabIndex = 1;
      this.RepairLineBreaksCheckBox.Text = "Repair Line Breaks";
      this.RepairLineBreaksCheckBox.UseVisualStyleBackColor = true;
      // 
      // RepairRedundantSpacesCheckBox
      // 
      this.RepairRedundantSpacesCheckBox.AutoSize = true;
      this.RepairRedundantSpacesCheckBox.Location = new System.Drawing.Point(160, 30);
      this.RepairRedundantSpacesCheckBox.Name = "RepairRedundantSpacesCheckBox";
      this.RepairRedundantSpacesCheckBox.Size = new System.Drawing.Size(152, 17);
      this.RepairRedundantSpacesCheckBox.TabIndex = 2;
      this.RepairRedundantSpacesCheckBox.Text = "Repair Redundant Spaces";
      this.RepairRedundantSpacesCheckBox.UseVisualStyleBackColor = true;
      // 
      // UpdateCheckBox
      // 
      this.UpdateCheckBox.AutoSize = true;
      this.UpdateCheckBox.Location = new System.Drawing.Point(160, 53);
      this.UpdateCheckBox.Name = "UpdateCheckBox";
      this.UpdateCheckBox.Size = new System.Drawing.Size(61, 17);
      this.UpdateCheckBox.TabIndex = 3;
      this.UpdateCheckBox.Text = "Update";
      this.UpdateCheckBox.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(384, 362);
      this.Controls.Add(this.UpdateCheckBox);
      this.Controls.Add(this.RepairRedundantSpacesCheckBox);
      this.Controls.Add(this.RepairLineBreaksCheckBox);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.ServerGroupBox);
      this.Controls.Add(this.StartButton);
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "Normailze Line Breaks";
      this.ServerGroupBox.ResumeLayout(false);
      this.ServerGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox StatusTextBox;
    private System.Windows.Forms.GroupBox ServerGroupBox;
    private System.Windows.Forms.RadioButton LiveServerRadioButton;
    private System.Windows.Forms.RadioButton TestServerRadioButton;
    private System.Windows.Forms.Button StartButton;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
    private System.Windows.Forms.CheckBox RepairLineBreaksCheckBox;
    private System.Windows.Forms.CheckBox RepairRedundantSpacesCheckBox;
    private System.Windows.Forms.CheckBox UpdateCheckBox;
  }
}

