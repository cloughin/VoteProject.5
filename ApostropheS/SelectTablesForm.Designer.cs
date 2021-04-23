namespace ApostropheS
{
  partial class SelectTablesForm
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
      this.SelectAllButton = new System.Windows.Forms.Button();
      this.SelectNoneButton = new System.Windows.Forms.Button();
      this.OkButton = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.TablesCheckedListBox = new System.Windows.Forms.CheckedListBox();
      this.SuspendLayout();
      // 
      // SelectAllButton
      // 
      this.SelectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.SelectAllButton.Location = new System.Drawing.Point(16, 231);
      this.SelectAllButton.Name = "SelectAllButton";
      this.SelectAllButton.Size = new System.Drawing.Size(75, 23);
      this.SelectAllButton.TabIndex = 9;
      this.SelectAllButton.Text = "Select All";
      this.SelectAllButton.UseVisualStyleBackColor = true;
      this.SelectAllButton.Click += new System.EventHandler(this.SelectTablesSelectAllButton_Click);
      // 
      // SelectNoneButton
      // 
      this.SelectNoneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.SelectNoneButton.Location = new System.Drawing.Point(96, 231);
      this.SelectNoneButton.Name = "SelectNoneButton";
      this.SelectNoneButton.Size = new System.Drawing.Size(75, 23);
      this.SelectNoneButton.TabIndex = 8;
      this.SelectNoneButton.Text = "Select None";
      this.SelectNoneButton.UseVisualStyleBackColor = true;
      this.SelectNoneButton.Click += new System.EventHandler(this.SelectTablesSelectNoneButton_Click);
      // 
      // OkButton
      // 
      this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OkButton.Location = new System.Drawing.Point(176, 231);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 7;
      this.OkButton.Text = "Ok";
      this.OkButton.UseVisualStyleBackColor = true;
      this.OkButton.Click += new System.EventHandler(this.SelectTablesOkButton_Click);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Location = new System.Drawing.Point(13, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(238, 27);
      this.label1.TabIndex = 6;
      this.label1.Text = "Found the following tables with non-primary-key text columns. Select the tables t" +
    "o repair.";
      // 
      // TablesCheckedListBox
      // 
      this.TablesCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TablesCheckedListBox.CheckOnClick = true;
      this.TablesCheckedListBox.FormattingEnabled = true;
      this.TablesCheckedListBox.Location = new System.Drawing.Point(12, 42);
      this.TablesCheckedListBox.Name = "TablesCheckedListBox";
      this.TablesCheckedListBox.Size = new System.Drawing.Size(239, 184);
      this.TablesCheckedListBox.TabIndex = 5;
      // 
      // SelectTablesForm
      // 
      this.AcceptButton = this.OkButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(263, 267);
      this.ControlBox = false;
      this.Controls.Add(this.SelectAllButton);
      this.Controls.Add(this.SelectNoneButton);
      this.Controls.Add(this.OkButton);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.TablesCheckedListBox);
      this.MinimumSize = new System.Drawing.Size(279, 38);
      this.Name = "SelectTablesForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Select Tables";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button SelectAllButton;
    private System.Windows.Forms.Button SelectNoneButton;
    private System.Windows.Forms.Button OkButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckedListBox TablesCheckedListBox;
  }
}