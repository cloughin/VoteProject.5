namespace DisqusLoad
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
      if (disposing && components != null)
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
      this.SuppressUpdateCheckBox = new System.Windows.Forms.CheckBox();
      this.OpenXmlFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.label2 = new System.Windows.Forms.Label();
      this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.StatusTextBox = new System.Windows.Forms.TextBox();
      this.ServerGroupBox = new System.Windows.Forms.GroupBox();
      this.LiveServerRadioButton = new System.Windows.Forms.RadioButton();
      this.TestServerRadioButton = new System.Windows.Forms.RadioButton();
      this.StartButton = new System.Windows.Forms.Button();
      this.BrowseXmlPathButton = new System.Windows.Forms.Button();
      this.XmlPathTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.ServerGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // SuppressUpdateCheckBox
      // 
      this.SuppressUpdateCheckBox.AutoSize = true;
      this.SuppressUpdateCheckBox.Location = new System.Drawing.Point(15, 54);
      this.SuppressUpdateCheckBox.Name = "SuppressUpdateCheckBox";
      this.SuppressUpdateCheckBox.Size = new System.Drawing.Size(106, 17);
      this.SuppressUpdateCheckBox.TabIndex = 22;
      this.SuppressUpdateCheckBox.Text = "Suppress update";
      this.SuppressUpdateCheckBox.UseVisualStyleBackColor = true;
      // 
      // OpenXmlFileDialog
      // 
      this.OpenXmlFileDialog.DefaultExt = "xml";
      this.OpenXmlFileDialog.Filter = "XML files|*.xml";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 85);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "Status:";
      // 
      // BackgroundWorker
      // 
      this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      // 
      // StatusTextBox
      // 
      this.StatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.StatusTextBox.Location = new System.Drawing.Point(15, 101);
      this.StatusTextBox.MaxLength = 1000000;
      this.StatusTextBox.Multiline = true;
      this.StatusTextBox.Name = "StatusTextBox";
      this.StatusTextBox.ReadOnly = true;
      this.StatusTextBox.Size = new System.Drawing.Size(465, 226);
      this.StatusTextBox.TabIndex = 20;
      // 
      // ServerGroupBox
      // 
      this.ServerGroupBox.Controls.Add(this.LiveServerRadioButton);
      this.ServerGroupBox.Controls.Add(this.TestServerRadioButton);
      this.ServerGroupBox.Location = new System.Drawing.Point(354, 51);
      this.ServerGroupBox.Name = "ServerGroupBox";
      this.ServerGroupBox.Size = new System.Drawing.Size(114, 42);
      this.ServerGroupBox.TabIndex = 18;
      this.ServerGroupBox.TabStop = false;
      this.ServerGroupBox.Text = "Server";
      // 
      // LiveServerRadioButton
      // 
      this.LiveServerRadioButton.AutoSize = true;
      this.LiveServerRadioButton.Location = new System.Drawing.Point(67, 15);
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
      this.TestServerRadioButton.Location = new System.Drawing.Point(6, 15);
      this.TestServerRadioButton.Name = "TestServerRadioButton";
      this.TestServerRadioButton.Size = new System.Drawing.Size(46, 17);
      this.TestServerRadioButton.TabIndex = 0;
      this.TestServerRadioButton.TabStop = true;
      this.TestServerRadioButton.Text = "Test";
      this.TestServerRadioButton.UseVisualStyleBackColor = true;
      this.TestServerRadioButton.CheckedChanged += new System.EventHandler(this.ServerRadioButton_CheckedChanged);
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(405, 333);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 21;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // BrowseXmlPathButton
      // 
      this.BrowseXmlPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BrowseXmlPathButton.Location = new System.Drawing.Point(405, 25);
      this.BrowseXmlPathButton.Name = "BrowseXmlPathButton";
      this.BrowseXmlPathButton.Size = new System.Drawing.Size(75, 23);
      this.BrowseXmlPathButton.TabIndex = 17;
      this.BrowseXmlPathButton.Text = "Browse...";
      this.BrowseXmlPathButton.UseVisualStyleBackColor = true;
      this.BrowseXmlPathButton.Click += new System.EventHandler(this.BrowseXmlPathButton_Click);
      // 
      // XmlPathTextBox
      // 
      this.XmlPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.XmlPathTextBox.Location = new System.Drawing.Point(15, 27);
      this.XmlPathTextBox.Name = "XmlPathTextBox";
      this.XmlPathTextBox.Size = new System.Drawing.Size(384, 20);
      this.XmlPathTextBox.TabIndex = 16;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 11);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(101, 13);
      this.label1.TabIndex = 15;
      this.label1.Text = "Path to export XML:";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(492, 366);
      this.Controls.Add(this.SuppressUpdateCheckBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.StatusTextBox);
      this.Controls.Add(this.ServerGroupBox);
      this.Controls.Add(this.StartButton);
      this.Controls.Add(this.BrowseXmlPathButton);
      this.Controls.Add(this.XmlPathTextBox);
      this.Controls.Add(this.label1);
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.Text = "Load Disqus Export";
      this.ServerGroupBox.ResumeLayout(false);
      this.ServerGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox SuppressUpdateCheckBox;
    private System.Windows.Forms.OpenFileDialog OpenXmlFileDialog;
    private System.Windows.Forms.Label label2;
    private System.ComponentModel.BackgroundWorker BackgroundWorker;
    private System.Windows.Forms.TextBox StatusTextBox;
    private System.Windows.Forms.GroupBox ServerGroupBox;
    private System.Windows.Forms.RadioButton LiveServerRadioButton;
    private System.Windows.Forms.RadioButton TestServerRadioButton;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.Button BrowseXmlPathButton;
    private System.Windows.Forms.TextBox XmlPathTextBox;
    private System.Windows.Forms.Label label1;
  }
}

