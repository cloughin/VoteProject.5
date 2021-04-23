namespace TestRedirect
{
  partial class TestSmtp
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
      this.SendButton = new System.Windows.Forms.Button();
      this.BodyTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.ServerTextBox = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.FromTextBox = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.ToTextBox = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.SubjectTextBox = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // SendButton
      // 
      this.SendButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.SendButton.Location = new System.Drawing.Point(205, 381);
      this.SendButton.Name = "SendButton";
      this.SendButton.Size = new System.Drawing.Size(75, 23);
      this.SendButton.TabIndex = 10;
      this.SendButton.Text = "Send";
      this.SendButton.UseVisualStyleBackColor = true;
      this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
      // 
      // BodyTextBox
      // 
      this.BodyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.BodyTextBox.Location = new System.Drawing.Point(15, 213);
      this.BodyTextBox.Multiline = true;
      this.BodyTextBox.Name = "BodyTextBox";
      this.BodyTextBox.Size = new System.Drawing.Size(265, 155);
      this.BodyTextBox.TabIndex = 9;
      this.BodyTextBox.Text = "Test message.";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 197);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(34, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Body:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(38, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Server";
      // 
      // ServerTextBox
      // 
      this.ServerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.ServerTextBox.Location = new System.Drawing.Point(15, 25);
      this.ServerTextBox.Name = "ServerTextBox";
      this.ServerTextBox.Size = new System.Drawing.Size(265, 20);
      this.ServerTextBox.TabIndex = 1;
      this.ServerTextBox.Text = "localhost";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 56);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(33, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "From:";
      // 
      // FromTextBox
      // 
      this.FromTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.FromTextBox.Location = new System.Drawing.Point(15, 72);
      this.FromTextBox.Name = "FromTextBox";
      this.FromTextBox.Size = new System.Drawing.Size(262, 20);
      this.FromTextBox.TabIndex = 3;
      this.FromTextBox.Text = "ron.kahlow@vote-usa.org";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 103);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(23, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "To:";
      // 
      // ToTextBox
      // 
      this.ToTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.ToTextBox.Location = new System.Drawing.Point(15, 119);
      this.ToTextBox.Name = "ToTextBox";
      this.ToTextBox.Size = new System.Drawing.Size(262, 20);
      this.ToTextBox.TabIndex = 5;
      this.ToTextBox.Text = "curt@loughin.com";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 150);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(46, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Subject:";
      // 
      // SubjectTextBox
      // 
      this.SubjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.SubjectTextBox.Location = new System.Drawing.Point(15, 166);
      this.SubjectTextBox.Name = "SubjectTextBox";
      this.SubjectTextBox.Size = new System.Drawing.Size(262, 20);
      this.SubjectTextBox.TabIndex = 7;
      this.SubjectTextBox.Text = "Test Message";
      // 
      // TestSmtp
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 416);
      this.Controls.Add(this.SubjectTextBox);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.ToTextBox);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.FromTextBox);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.ServerTextBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.BodyTextBox);
      this.Controls.Add(this.SendButton);
      this.Name = "TestSmtp";
      this.ShowIcon = false;
      this.Text = "Test Smtp";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button SendButton;
    private System.Windows.Forms.TextBox BodyTextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox ServerTextBox;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox FromTextBox;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox ToTextBox;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox SubjectTextBox;
  }
}