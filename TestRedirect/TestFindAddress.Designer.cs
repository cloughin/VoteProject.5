namespace TestRedirect
{
  partial class TestFindAddress
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
      this.AddressTextBox = new System.Windows.Forms.TextBox();
      this.FindButton = new System.Windows.Forms.Button();
      this.ResultsTextBox = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.OptionGroupBox = new System.Windows.Forms.GroupBox();
      this.LookupRadioButton = new System.Windows.Forms.RadioButton();
      this.ParseRadioButton = new System.Windows.Forms.RadioButton();
      this.OptionGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(92, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Address or ZIP+4:";
      // 
      // AddressTextBox
      // 
      this.AddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.AddressTextBox.Location = new System.Drawing.Point(15, 25);
      this.AddressTextBox.Name = "AddressTextBox";
      this.AddressTextBox.Size = new System.Drawing.Size(565, 20);
      this.AddressTextBox.TabIndex = 1;
      this.AddressTextBox.Text = "12219 Folkstone 20171-1818";
      // 
      // FindButton
      // 
      this.FindButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.FindButton.Location = new System.Drawing.Point(505, 51);
      this.FindButton.Name = "FindButton";
      this.FindButton.Size = new System.Drawing.Size(75, 23);
      this.FindButton.TabIndex = 5;
      this.FindButton.Text = "Find";
      this.FindButton.UseVisualStyleBackColor = true;
      this.FindButton.Click += new System.EventHandler(this.FindButton_Click);
      // 
      // ResultsTextBox
      // 
      this.ResultsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.ResultsTextBox.Location = new System.Drawing.Point(15, 127);
      this.ResultsTextBox.Multiline = true;
      this.ResultsTextBox.Name = "ResultsTextBox";
      this.ResultsTextBox.ReadOnly = true;
      this.ResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.ResultsTextBox.Size = new System.Drawing.Size(565, 327);
      this.ResultsTextBox.TabIndex = 4;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 111);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(45, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Results:";
      // 
      // OptionGroupBox
      // 
      this.OptionGroupBox.Controls.Add(this.ParseRadioButton);
      this.OptionGroupBox.Controls.Add(this.LookupRadioButton);
      this.OptionGroupBox.Location = new System.Drawing.Point(15, 51);
      this.OptionGroupBox.Name = "OptionGroupBox";
      this.OptionGroupBox.Size = new System.Drawing.Size(149, 45);
      this.OptionGroupBox.TabIndex = 2;
      this.OptionGroupBox.TabStop = false;
      this.OptionGroupBox.Text = "Option";
      // 
      // LookupRadioButton
      // 
      this.LookupRadioButton.AutoSize = true;
      this.LookupRadioButton.Checked = true;
      this.LookupRadioButton.Location = new System.Drawing.Point(6, 19);
      this.LookupRadioButton.Name = "LookupRadioButton";
      this.LookupRadioButton.Size = new System.Drawing.Size(61, 17);
      this.LookupRadioButton.TabIndex = 0;
      this.LookupRadioButton.TabStop = true;
      this.LookupRadioButton.Text = "Lookup";
      this.LookupRadioButton.UseVisualStyleBackColor = true;
      // 
      // ParseRadioButton
      // 
      this.ParseRadioButton.AutoSize = true;
      this.ParseRadioButton.Location = new System.Drawing.Point(85, 19);
      this.ParseRadioButton.Name = "ParseRadioButton";
      this.ParseRadioButton.Size = new System.Drawing.Size(52, 17);
      this.ParseRadioButton.TabIndex = 1;
      this.ParseRadioButton.Text = "Parse";
      this.ParseRadioButton.UseVisualStyleBackColor = true;
      // 
      // TestFindAddress
      // 
      this.AcceptButton = this.FindButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(592, 466);
      this.Controls.Add(this.OptionGroupBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.ResultsTextBox);
      this.Controls.Add(this.FindButton);
      this.Controls.Add(this.AddressTextBox);
      this.Controls.Add(this.label1);
      this.Name = "TestFindAddress";
      this.ShowIcon = false;
      this.Text = "Test Find Address";
      this.OptionGroupBox.ResumeLayout(false);
      this.OptionGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox AddressTextBox;
    private System.Windows.Forms.Button FindButton;
    private System.Windows.Forms.TextBox ResultsTextBox;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.GroupBox OptionGroupBox;
    private System.Windows.Forms.RadioButton ParseRadioButton;
    private System.Windows.Forms.RadioButton LookupRadioButton;
  }
}