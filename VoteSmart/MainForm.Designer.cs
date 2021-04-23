namespace VoteSmart
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.MissingCandidatesRadioButton = new System.Windows.Forms.RadioButton();
      this.IterateRadioButton = new System.Windows.Forms.RadioButton();
      this.ProcessStoredRadioButton = new System.Windows.Forms.RadioButton();
      this.GetRawRadioButton = new System.Windows.Forms.RadioButton();
      this.ParametersTextBox = new System.Windows.Forms.TextBox();
      this.MethodComboBox = new System.Windows.Forms.ComboBox();
      this.StartButton = new System.Windows.Forms.Button();
      this.CreateCandidatesStatesRadioButton = new System.Windows.Forms.RadioButton();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.CreateCandidatesStatesRadioButton);
      this.groupBox1.Controls.Add(this.MissingCandidatesRadioButton);
      this.groupBox1.Controls.Add(this.IterateRadioButton);
      this.groupBox1.Controls.Add(this.ProcessStoredRadioButton);
      this.groupBox1.Controls.Add(this.GetRawRadioButton);
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(160, 141);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Operation";
      // 
      // MissingCandidatesRadioButton
      // 
      this.MissingCandidatesRadioButton.AutoSize = true;
      this.MissingCandidatesRadioButton.Location = new System.Drawing.Point(6, 88);
      this.MissingCandidatesRadioButton.Name = "MissingCandidatesRadioButton";
      this.MissingCandidatesRadioButton.Size = new System.Drawing.Size(136, 17);
      this.MissingCandidatesRadioButton.TabIndex = 4;
      this.MissingCandidatesRadioButton.TabStop = true;
      this.MissingCandidatesRadioButton.Text = "Get Missing Candidates";
      this.MissingCandidatesRadioButton.UseVisualStyleBackColor = true;
      // 
      // IterateRadioButton
      // 
      this.IterateRadioButton.AutoSize = true;
      this.IterateRadioButton.Location = new System.Drawing.Point(6, 65);
      this.IterateRadioButton.Name = "IterateRadioButton";
      this.IterateRadioButton.Size = new System.Drawing.Size(55, 17);
      this.IterateRadioButton.TabIndex = 3;
      this.IterateRadioButton.TabStop = true;
      this.IterateRadioButton.Text = "Iterate";
      this.IterateRadioButton.UseVisualStyleBackColor = true;
      // 
      // ProcessStoredRadioButton
      // 
      this.ProcessStoredRadioButton.AutoSize = true;
      this.ProcessStoredRadioButton.Location = new System.Drawing.Point(6, 42);
      this.ProcessStoredRadioButton.Name = "ProcessStoredRadioButton";
      this.ProcessStoredRadioButton.Size = new System.Drawing.Size(123, 17);
      this.ProcessStoredRadioButton.TabIndex = 2;
      this.ProcessStoredRadioButton.Text = "Process Stored Data";
      this.ProcessStoredRadioButton.UseVisualStyleBackColor = true;
      // 
      // GetRawRadioButton
      // 
      this.GetRawRadioButton.AutoSize = true;
      this.GetRawRadioButton.Location = new System.Drawing.Point(6, 19);
      this.GetRawRadioButton.Name = "GetRawRadioButton";
      this.GetRawRadioButton.Size = new System.Drawing.Size(93, 17);
      this.GetRawRadioButton.TabIndex = 1;
      this.GetRawRadioButton.Text = "Get Raw Data";
      this.GetRawRadioButton.UseVisualStyleBackColor = true;
      // 
      // ParametersTextBox
      // 
      this.ParametersTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ParametersTextBox.Location = new System.Drawing.Point(11, 186);
      this.ParametersTextBox.Name = "ParametersTextBox";
      this.ParametersTextBox.Size = new System.Drawing.Size(261, 20);
      this.ParametersTextBox.TabIndex = 3;
      // 
      // MethodComboBox
      // 
      this.MethodComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.MethodComboBox.DropDownHeight = 260;
      this.MethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.MethodComboBox.FormattingEnabled = true;
      this.MethodComboBox.IntegralHeight = false;
      this.MethodComboBox.Items.AddRange(new object[] {
            "Address.getCampaign(candidateId*)",
            "Address.getCampaignWebAddress(candidateId*)",
            "Address.getOffice(candidateId*)",
            "Address.getOfficeWebAddress(candidateId*)",
            "Candidates.getByOfficeTypeState(officeTypeId*,stateId*,electionYear*,stageId)",
            "CandidateBio.getBio(candidateId*)",
            "CandidateBio.getAddlBio(candidateId*)",
            "Committee.getCommittee(committeeId*)",
            "Committee.getCommitteeMembers(committeeId*)",
            "Committee.getCommitteesByTypeState(typeId,stateId*)",
            "District.getByOfficeState(officeId*,stateId*)",
            "Election.getElectionByYearState(year*,stateId*)",
            "Election.getStageCandidates(electionId*,stageId*)",
            "Leadership.getOfficials(leadershipId*,stateId*)",
            "Leadership.getPositions(stateId*,officeId)",
            "Local.getCities(stateId*)",
            "Local.getCounties(stateId*)",
            "Local.getOfficials(localId*)",
            "Measure.getMeasure(measureId*)",
            "Measure.getMeasuresByYearState(year*,stateId*)",
            "Office.getOfficesByType(officeTypeId*)",
            "Officials.getStatewide(stateId*)",
            "State.getState(stateId*)",
            "State.getStateIDs()"});
      this.MethodComboBox.Location = new System.Drawing.Point(11, 159);
      this.MethodComboBox.Name = "MethodComboBox";
      this.MethodComboBox.Size = new System.Drawing.Size(261, 21);
      this.MethodComboBox.TabIndex = 2;
      this.MethodComboBox.SelectedIndexChanged += new System.EventHandler(this.MethodComboBox_SelectedIndexChanged);
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(197, 227);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 3;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // CreateCandidatesStatesRadioButton
      // 
      this.CreateCandidatesStatesRadioButton.AutoSize = true;
      this.CreateCandidatesStatesRadioButton.Location = new System.Drawing.Point(6, 111);
      this.CreateCandidatesStatesRadioButton.Name = "CreateCandidatesStatesRadioButton";
      this.CreateCandidatesStatesRadioButton.Size = new System.Drawing.Size(145, 17);
      this.CreateCandidatesStatesRadioButton.TabIndex = 5;
      this.CreateCandidatesStatesRadioButton.TabStop = true;
      this.CreateCandidatesStatesRadioButton.Text = "Create candidates_states";
      this.CreateCandidatesStatesRadioButton.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Controls.Add(this.StartButton);
      this.Controls.Add(this.ParametersTextBox);
      this.Controls.Add(this.MethodComboBox);
      this.Controls.Add(this.groupBox1);
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "VoteSmart";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton ProcessStoredRadioButton;
    private System.Windows.Forms.RadioButton GetRawRadioButton;
    private System.Windows.Forms.TextBox ParametersTextBox;
    private System.Windows.Forms.ComboBox MethodComboBox;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.RadioButton IterateRadioButton;
    private System.Windows.Forms.RadioButton MissingCandidatesRadioButton;
    private System.Windows.Forms.RadioButton CreateCandidatesStatesRadioButton;
  }
}

