namespace TestRedirect
{
  partial class Instructions
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
      this.MainTabControl = new System.Windows.Forms.TabControl();
      this.ArchivesTabPage = new System.Windows.Forms.TabPage();
      this.ItemGroupBox = new System.Windows.Forms.GroupBox();
      this.ItemTextBox = new System.Windows.Forms.TextBox();
      this.ItemUpdateButton = new System.Windows.Forms.Button();
      this.ItemCheckBox = new System.Windows.Forms.CheckBox();
      this.BallotUpcomingTabPage = new System.Windows.Forms.TabPage();
      this.BallotPreviousTabPage = new System.Windows.Forms.TabPage();
      this.ElectedTabPage = new System.Windows.Forms.TabPage();
      this.ElectionUSPresidentPrimaryTabPage = new System.Windows.Forms.TabPage();
      this.ElectionUSPresidentTabPage = new System.Windows.Forms.TabPage();
      this.ElectionUSSenateTabPage = new System.Windows.Forms.TabPage();
      this.ElectionUSHouseTabPage = new System.Windows.Forms.TabPage();
      this.ElectionGovernorsTabPage = new System.Windows.Forms.TabPage();
      this.ElectionStateTabPage = new System.Windows.Forms.TabPage();
      this.ElectionCountyTabPage = new System.Windows.Forms.TabPage();
      this.ElectionLocalTabPage = new System.Windows.Forms.TabPage();
      this.ServerGroupBox = new System.Windows.Forms.GroupBox();
      this.LiveServerRadioButton = new System.Windows.Forms.RadioButton();
      this.TestServerRadioButton = new System.Windows.Forms.RadioButton();
      this.HideEmptyCheckBox = new System.Windows.Forms.CheckBox();
      this.MainTabControl.SuspendLayout();
      this.ArchivesTabPage.SuspendLayout();
      this.ItemGroupBox.SuspendLayout();
      this.ServerGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // MainTabControl
      // 
      this.MainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.MainTabControl.Controls.Add(this.ArchivesTabPage);
      this.MainTabControl.Controls.Add(this.BallotUpcomingTabPage);
      this.MainTabControl.Controls.Add(this.BallotPreviousTabPage);
      this.MainTabControl.Controls.Add(this.ElectedTabPage);
      this.MainTabControl.Controls.Add(this.ElectionUSPresidentPrimaryTabPage);
      this.MainTabControl.Controls.Add(this.ElectionUSPresidentTabPage);
      this.MainTabControl.Controls.Add(this.ElectionUSSenateTabPage);
      this.MainTabControl.Controls.Add(this.ElectionUSHouseTabPage);
      this.MainTabControl.Controls.Add(this.ElectionGovernorsTabPage);
      this.MainTabControl.Controls.Add(this.ElectionStateTabPage);
      this.MainTabControl.Controls.Add(this.ElectionCountyTabPage);
      this.MainTabControl.Controls.Add(this.ElectionLocalTabPage);
      this.MainTabControl.Location = new System.Drawing.Point(0, 60);
      this.MainTabControl.Name = "MainTabControl";
      this.MainTabControl.SelectedIndex = 0;
      this.MainTabControl.Size = new System.Drawing.Size(643, 246);
      this.MainTabControl.TabIndex = 2;
      this.MainTabControl.SelectedIndexChanged += new System.EventHandler(this.MainTabControl_SelectedIndexChanged);
      // 
      // ArchivesTabPage
      // 
      this.ArchivesTabPage.Controls.Add(this.ItemGroupBox);
      this.ArchivesTabPage.Location = new System.Drawing.Point(4, 22);
      this.ArchivesTabPage.Name = "ArchivesTabPage";
      this.ArchivesTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.ArchivesTabPage.Size = new System.Drawing.Size(635, 220);
      this.ArchivesTabPage.TabIndex = 0;
      this.ArchivesTabPage.Text = "Archives";
      this.ArchivesTabPage.UseVisualStyleBackColor = true;
      // 
      // ItemGroupBox
      // 
      this.ItemGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.ItemGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.ItemGroupBox.Controls.Add(this.ItemTextBox);
      this.ItemGroupBox.Controls.Add(this.ItemUpdateButton);
      this.ItemGroupBox.Controls.Add(this.ItemCheckBox);
      this.ItemGroupBox.Location = new System.Drawing.Point(8, 6);
      this.ItemGroupBox.Name = "ItemGroupBox";
      this.ItemGroupBox.Size = new System.Drawing.Size(619, 100);
      this.ItemGroupBox.TabIndex = 0;
      this.ItemGroupBox.TabStop = false;
      this.ItemGroupBox.Text = "ItemGroupBox";
      // 
      // ItemTextBox
      // 
      this.ItemTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.ItemTextBox.Location = new System.Drawing.Point(6, 42);
      this.ItemTextBox.Multiline = true;
      this.ItemTextBox.Name = "ItemTextBox";
      this.ItemTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.ItemTextBox.Size = new System.Drawing.Size(607, 52);
      this.ItemTextBox.TabIndex = 2;
      this.ItemTextBox.TextChanged += new System.EventHandler(this.ItemTextBox_TextChanged);
      // 
      // ItemUpdateButton
      // 
      this.ItemUpdateButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ItemUpdateButton.Enabled = false;
      this.ItemUpdateButton.Location = new System.Drawing.Point(538, 15);
      this.ItemUpdateButton.Name = "ItemUpdateButton";
      this.ItemUpdateButton.Size = new System.Drawing.Size(75, 23);
      this.ItemUpdateButton.TabIndex = 1;
      this.ItemUpdateButton.Text = "Update";
      this.ItemUpdateButton.UseVisualStyleBackColor = true;
      this.ItemUpdateButton.Click += new System.EventHandler(this.ItemUpdateButton_Click);
      // 
      // ItemCheckBox
      // 
      this.ItemCheckBox.AutoSize = true;
      this.ItemCheckBox.Location = new System.Drawing.Point(6, 19);
      this.ItemCheckBox.Name = "ItemCheckBox";
      this.ItemCheckBox.Size = new System.Drawing.Size(95, 17);
      this.ItemCheckBox.TabIndex = 0;
      this.ItemCheckBox.Text = "ItemCheckBox";
      this.ItemCheckBox.UseVisualStyleBackColor = true;
      this.ItemCheckBox.CheckedChanged += new System.EventHandler(this.ItemCheckBox_CheckedChanged);
      // 
      // BallotUpcomingTabPage
      // 
      this.BallotUpcomingTabPage.Location = new System.Drawing.Point(4, 22);
      this.BallotUpcomingTabPage.Name = "BallotUpcomingTabPage";
      this.BallotUpcomingTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.BallotUpcomingTabPage.Size = new System.Drawing.Size(635, 220);
      this.BallotUpcomingTabPage.TabIndex = 1;
      this.BallotUpcomingTabPage.Text = "Ballot Upcoming";
      this.BallotUpcomingTabPage.UseVisualStyleBackColor = true;
      // 
      // BallotPreviousTabPage
      // 
      this.BallotPreviousTabPage.Location = new System.Drawing.Point(4, 22);
      this.BallotPreviousTabPage.Name = "BallotPreviousTabPage";
      this.BallotPreviousTabPage.Size = new System.Drawing.Size(635, 220);
      this.BallotPreviousTabPage.TabIndex = 2;
      this.BallotPreviousTabPage.Text = "Ballot Previous";
      this.BallotPreviousTabPage.UseVisualStyleBackColor = true;
      // 
      // ElectedTabPage
      // 
      this.ElectedTabPage.Location = new System.Drawing.Point(4, 22);
      this.ElectedTabPage.Name = "ElectedTabPage";
      this.ElectedTabPage.Size = new System.Drawing.Size(635, 220);
      this.ElectedTabPage.TabIndex = 3;
      this.ElectedTabPage.Text = "Elected";
      this.ElectedTabPage.UseVisualStyleBackColor = true;
      // 
      // ElectionUSPresidentPrimaryTabPage
      // 
      this.ElectionUSPresidentPrimaryTabPage.Location = new System.Drawing.Point(4, 22);
      this.ElectionUSPresidentPrimaryTabPage.Name = "ElectionUSPresidentPrimaryTabPage";
      this.ElectionUSPresidentPrimaryTabPage.Size = new System.Drawing.Size(635, 220);
      this.ElectionUSPresidentPrimaryTabPage.TabIndex = 4;
      this.ElectionUSPresidentPrimaryTabPage.Text = "Election Pres Pri";
      this.ElectionUSPresidentPrimaryTabPage.UseVisualStyleBackColor = true;
      // 
      // ElectionUSPresidentTabPage
      // 
      this.ElectionUSPresidentTabPage.Location = new System.Drawing.Point(4, 22);
      this.ElectionUSPresidentTabPage.Name = "ElectionUSPresidentTabPage";
      this.ElectionUSPresidentTabPage.Size = new System.Drawing.Size(635, 220);
      this.ElectionUSPresidentTabPage.TabIndex = 5;
      this.ElectionUSPresidentTabPage.Text = "Election Pres";
      this.ElectionUSPresidentTabPage.UseVisualStyleBackColor = true;
      // 
      // ElectionUSSenateTabPage
      // 
      this.ElectionUSSenateTabPage.Location = new System.Drawing.Point(4, 22);
      this.ElectionUSSenateTabPage.Name = "ElectionUSSenateTabPage";
      this.ElectionUSSenateTabPage.Size = new System.Drawing.Size(635, 220);
      this.ElectionUSSenateTabPage.TabIndex = 6;
      this.ElectionUSSenateTabPage.Text = "Election Senate";
      this.ElectionUSSenateTabPage.UseVisualStyleBackColor = true;
      // 
      // ElectionUSHouseTabPage
      // 
      this.ElectionUSHouseTabPage.Location = new System.Drawing.Point(4, 22);
      this.ElectionUSHouseTabPage.Name = "ElectionUSHouseTabPage";
      this.ElectionUSHouseTabPage.Size = new System.Drawing.Size(635, 220);
      this.ElectionUSHouseTabPage.TabIndex = 7;
      this.ElectionUSHouseTabPage.Text = "Election House";
      this.ElectionUSHouseTabPage.UseVisualStyleBackColor = true;
      // 
      // ElectionGovernorsTabPage
      // 
      this.ElectionGovernorsTabPage.Location = new System.Drawing.Point(4, 22);
      this.ElectionGovernorsTabPage.Name = "ElectionGovernorsTabPage";
      this.ElectionGovernorsTabPage.Size = new System.Drawing.Size(635, 220);
      this.ElectionGovernorsTabPage.TabIndex = 8;
      this.ElectionGovernorsTabPage.Text = "Election Governors";
      this.ElectionGovernorsTabPage.UseVisualStyleBackColor = true;
      // 
      // ElectionStateTabPage
      // 
      this.ElectionStateTabPage.Location = new System.Drawing.Point(4, 22);
      this.ElectionStateTabPage.Name = "ElectionStateTabPage";
      this.ElectionStateTabPage.Size = new System.Drawing.Size(635, 220);
      this.ElectionStateTabPage.TabIndex = 9;
      this.ElectionStateTabPage.Text = "Election State";
      this.ElectionStateTabPage.UseVisualStyleBackColor = true;
      // 
      // ElectionCountyTabPage
      // 
      this.ElectionCountyTabPage.Location = new System.Drawing.Point(4, 22);
      this.ElectionCountyTabPage.Name = "ElectionCountyTabPage";
      this.ElectionCountyTabPage.Size = new System.Drawing.Size(635, 220);
      this.ElectionCountyTabPage.TabIndex = 10;
      this.ElectionCountyTabPage.Text = "Election County";
      this.ElectionCountyTabPage.UseVisualStyleBackColor = true;
      // 
      // ElectionLocalTabPage
      // 
      this.ElectionLocalTabPage.Location = new System.Drawing.Point(4, 22);
      this.ElectionLocalTabPage.Name = "ElectionLocalTabPage";
      this.ElectionLocalTabPage.Size = new System.Drawing.Size(635, 220);
      this.ElectionLocalTabPage.TabIndex = 11;
      this.ElectionLocalTabPage.Text = "Election Local";
      this.ElectionLocalTabPage.UseVisualStyleBackColor = true;
      // 
      // ServerGroupBox
      // 
      this.ServerGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ServerGroupBox.Controls.Add(this.LiveServerRadioButton);
      this.ServerGroupBox.Controls.Add(this.TestServerRadioButton);
      this.ServerGroupBox.Location = new System.Drawing.Point(517, 12);
      this.ServerGroupBox.Name = "ServerGroupBox";
      this.ServerGroupBox.Size = new System.Drawing.Size(114, 42);
      this.ServerGroupBox.TabIndex = 1;
      this.ServerGroupBox.TabStop = false;
      this.ServerGroupBox.Text = "Server";
      // 
      // LiveServerRadioButton
      // 
      this.LiveServerRadioButton.AutoSize = true;
      this.LiveServerRadioButton.Location = new System.Drawing.Point(67, 17);
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
      this.TestServerRadioButton.Location = new System.Drawing.Point(6, 17);
      this.TestServerRadioButton.Name = "TestServerRadioButton";
      this.TestServerRadioButton.Size = new System.Drawing.Size(46, 17);
      this.TestServerRadioButton.TabIndex = 0;
      this.TestServerRadioButton.TabStop = true;
      this.TestServerRadioButton.Text = "Test";
      this.TestServerRadioButton.UseVisualStyleBackColor = true;
      this.TestServerRadioButton.CheckedChanged += new System.EventHandler(this.ServerRadioButton_CheckedChanged);
      // 
      // HideEmptyCheckBox
      // 
      this.HideEmptyCheckBox.AutoSize = true;
      this.HideEmptyCheckBox.Location = new System.Drawing.Point(17, 22);
      this.HideEmptyCheckBox.Name = "HideEmptyCheckBox";
      this.HideEmptyCheckBox.Size = new System.Drawing.Size(136, 17);
      this.HideEmptyCheckBox.TabIndex = 0;
      this.HideEmptyCheckBox.Text = "Hide items with no data";
      this.HideEmptyCheckBox.UseVisualStyleBackColor = true;
      this.HideEmptyCheckBox.CheckedChanged += new System.EventHandler(this.HideEmptyCheckBox_CheckedChanged);
      // 
      // Instructions
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(643, 306);
      this.Controls.Add(this.HideEmptyCheckBox);
      this.Controls.Add(this.ServerGroupBox);
      this.Controls.Add(this.MainTabControl);
      this.Name = "Instructions";
      this.ShowIcon = false;
      this.Text = "Instructions Maintenance";
      this.MainTabControl.ResumeLayout(false);
      this.ArchivesTabPage.ResumeLayout(false);
      this.ItemGroupBox.ResumeLayout(false);
      this.ItemGroupBox.PerformLayout();
      this.ServerGroupBox.ResumeLayout(false);
      this.ServerGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl MainTabControl;
    private System.Windows.Forms.TabPage ArchivesTabPage;
    private System.Windows.Forms.TabPage BallotUpcomingTabPage;
    private System.Windows.Forms.GroupBox ItemGroupBox;
    private System.Windows.Forms.TextBox ItemTextBox;
    private System.Windows.Forms.Button ItemUpdateButton;
    private System.Windows.Forms.CheckBox ItemCheckBox;
    private System.Windows.Forms.GroupBox ServerGroupBox;
    private System.Windows.Forms.RadioButton LiveServerRadioButton;
    private System.Windows.Forms.RadioButton TestServerRadioButton;
    private System.Windows.Forms.CheckBox HideEmptyCheckBox;
    private System.Windows.Forms.TabPage BallotPreviousTabPage;
    private System.Windows.Forms.TabPage ElectedTabPage;
    private System.Windows.Forms.TabPage ElectionUSPresidentPrimaryTabPage;
    private System.Windows.Forms.TabPage ElectionUSPresidentTabPage;
    private System.Windows.Forms.TabPage ElectionUSSenateTabPage;
    private System.Windows.Forms.TabPage ElectionUSHouseTabPage;
    private System.Windows.Forms.TabPage ElectionGovernorsTabPage;
    private System.Windows.Forms.TabPage ElectionStateTabPage;
    private System.Windows.Forms.TabPage ElectionCountyTabPage;
    private System.Windows.Forms.TabPage ElectionLocalTabPage;
  }
}