namespace ExcelTest
{
  partial class Form1
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
      this.TextBoxFileToRead = new System.Windows.Forms.TextBox();
      this.ButtonBrowseFileToRead = new System.Windows.Forms.Button();
      this.OpenFileToReadDialog = new System.Windows.Forms.OpenFileDialog();
      this.ButtonStart = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(67, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "File to Read:";
      // 
      // TextBoxFileToRead
      // 
      this.TextBoxFileToRead.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TextBoxFileToRead.Location = new System.Drawing.Point(85, 6);
      this.TextBoxFileToRead.Name = "TextBoxFileToRead";
      this.TextBoxFileToRead.Size = new System.Drawing.Size(622, 20);
      this.TextBoxFileToRead.TabIndex = 1;
      // 
      // ButtonBrowseFileToRead
      // 
      this.ButtonBrowseFileToRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ButtonBrowseFileToRead.Location = new System.Drawing.Point(713, 4);
      this.ButtonBrowseFileToRead.Name = "ButtonBrowseFileToRead";
      this.ButtonBrowseFileToRead.Size = new System.Drawing.Size(75, 23);
      this.ButtonBrowseFileToRead.TabIndex = 2;
      this.ButtonBrowseFileToRead.Text = "Browse...";
      this.ButtonBrowseFileToRead.UseVisualStyleBackColor = true;
      this.ButtonBrowseFileToRead.Click += new System.EventHandler(this.ButtonBrowseFileToRead_Click);
      // 
      // ButtonStart
      // 
      this.ButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ButtonStart.Location = new System.Drawing.Point(713, 415);
      this.ButtonStart.Name = "ButtonStart";
      this.ButtonStart.Size = new System.Drawing.Size(75, 23);
      this.ButtonStart.TabIndex = 3;
      this.ButtonStart.Text = "Start";
      this.ButtonStart.UseVisualStyleBackColor = true;
      this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.ButtonStart);
      this.Controls.Add(this.ButtonBrowseFileToRead);
      this.Controls.Add(this.TextBoxFileToRead);
      this.Controls.Add(this.label1);
      this.Name = "Form1";
      this.ShowIcon = false;
      this.Text = "Excel Test";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox TextBoxFileToRead;
    private System.Windows.Forms.Button ButtonBrowseFileToRead;
    private System.Windows.Forms.OpenFileDialog OpenFileToReadDialog;
    private System.Windows.Forms.Button ButtonStart;
  }
}

