namespace AnalyzeTigerPlaces
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
      this.StartButton = new System.Windows.Forms.Button();
      this.StateDropDown = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.OptionsFileTextBox = new System.Windows.Forms.TextBox();
      this.OptionsFileBrowseButton = new System.Windows.Forms.Button();
      this.OptionsOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.label3 = new System.Windows.Forms.Label();
      this.TigerFolderTextBox = new System.Windows.Forms.TextBox();
      this.TigerFolderBrowseButton = new System.Windows.Forms.Button();
      this.TigerFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.OutputSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.OutputFileTextBox = new System.Windows.Forms.TextBox();
      this.OutputFileBrowseButton = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.OutputCousubsTextBox = new System.Windows.Forms.TextBox();
      this.OutputCousubsBrowseButton = new System.Windows.Forms.Button();
      this.OutputPlacesTextBox = new System.Windows.Forms.TextBox();
      this.OutputPlacesBrowseButton = new System.Windows.Forms.Button();
      this.StartShapefilesButton = new System.Windows.Forms.Button();
      this.CousubSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.PlacesSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.SuspendLayout();
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(597, 112);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(75, 23);
      this.StartButton.TabIndex = 0;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // StateDropDown
      // 
      this.StateDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.StateDropDown.FormattingEnabled = true;
      this.StateDropDown.Location = new System.Drawing.Point(50, 6);
      this.StateDropDown.Name = "StateDropDown";
      this.StateDropDown.Size = new System.Drawing.Size(61, 21);
      this.StateDropDown.TabIndex = 2;
      this.StateDropDown.SelectedIndexChanged += new System.EventHandler(this.StateDropDown_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(32, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "State";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 36);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(90, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "JSON Options file";
      // 
      // OptionsFileTextBox
      // 
      this.OptionsFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.OptionsFileTextBox.Location = new System.Drawing.Point(108, 33);
      this.OptionsFileTextBox.Name = "OptionsFileTextBox";
      this.OptionsFileTextBox.Size = new System.Drawing.Size(483, 20);
      this.OptionsFileTextBox.TabIndex = 5;
      this.OptionsFileTextBox.Text = "D:\\Users\\CurtNew\\Dropbox\\Documents\\Vote\\Tiger\\Reconcile Districts\\AnalyzeTigerPla" +
    "ces.txt";
      // 
      // OptionsFileBrowseButton
      // 
      this.OptionsFileBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.OptionsFileBrowseButton.Location = new System.Drawing.Point(597, 31);
      this.OptionsFileBrowseButton.Name = "OptionsFileBrowseButton";
      this.OptionsFileBrowseButton.Size = new System.Drawing.Size(75, 23);
      this.OptionsFileBrowseButton.TabIndex = 6;
      this.OptionsFileBrowseButton.Text = "Browse...";
      this.OptionsFileBrowseButton.UseVisualStyleBackColor = true;
      this.OptionsFileBrowseButton.Click += new System.EventHandler(this.OptionsFileBrowseButton_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 62);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(89, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Tiger Data Folder";
      // 
      // TigerFolderTextBox
      // 
      this.TigerFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TigerFolderTextBox.Location = new System.Drawing.Point(108, 59);
      this.TigerFolderTextBox.Name = "TigerFolderTextBox";
      this.TigerFolderTextBox.Size = new System.Drawing.Size(483, 20);
      this.TigerFolderTextBox.TabIndex = 8;
      this.TigerFolderTextBox.Text = "D:\\Users\\CurtNew\\Dropbox\\Documents\\Vote\\Tiger\\2016";
      // 
      // TigerFolderBrowseButton
      // 
      this.TigerFolderBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.TigerFolderBrowseButton.Location = new System.Drawing.Point(597, 57);
      this.TigerFolderBrowseButton.Name = "TigerFolderBrowseButton";
      this.TigerFolderBrowseButton.Size = new System.Drawing.Size(75, 23);
      this.TigerFolderBrowseButton.TabIndex = 9;
      this.TigerFolderBrowseButton.Text = "Browse...";
      this.TigerFolderBrowseButton.UseVisualStyleBackColor = true;
      this.TigerFolderBrowseButton.Click += new System.EventHandler(this.TigerFolderBrowseButton_Click);
      // 
      // TigerFolderBrowserDialog
      // 
      this.TigerFolderBrowserDialog.HelpRequest += new System.EventHandler(this.TigerFolderBrowserDialog_HelpRequest);
      // 
      // OutputSaveFileDialog
      // 
      this.OutputSaveFileDialog.DefaultExt = "csv";
      this.OutputSaveFileDialog.Filter = "csv files|*.csv|All files|*.*";
      // 
      // OutputFileTextBox
      // 
      this.OutputFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.OutputFileTextBox.Location = new System.Drawing.Point(108, 85);
      this.OutputFileTextBox.Name = "OutputFileTextBox";
      this.OutputFileTextBox.Size = new System.Drawing.Size(483, 20);
      this.OutputFileTextBox.TabIndex = 10;
      // 
      // OutputFileBrowseButton
      // 
      this.OutputFileBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.OutputFileBrowseButton.Location = new System.Drawing.Point(597, 83);
      this.OutputFileBrowseButton.Name = "OutputFileBrowseButton";
      this.OutputFileBrowseButton.Size = new System.Drawing.Size(75, 23);
      this.OutputFileBrowseButton.TabIndex = 11;
      this.OutputFileBrowseButton.Text = "Browse...";
      this.OutputFileBrowseButton.UseVisualStyleBackColor = true;
      this.OutputFileBrowseButton.Click += new System.EventHandler(this.OutputFileBrowseButton_Click);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 88);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(63, 13);
      this.label4.TabIndex = 12;
      this.label4.Text = "Output CSV";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label5.Location = new System.Drawing.Point(12, 145);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(135, 13);
      this.label5.TabIndex = 13;
      this.label5.Text = "Create Shapefile CSVs";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(12, 164);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(72, 13);
      this.label6.TabIndex = 14;
      this.label6.Text = "Cousubs CSV";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(12, 190);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(63, 13);
      this.label7.TabIndex = 15;
      this.label7.Text = "Places CSV";
      // 
      // OutputCousubsTextBox
      // 
      this.OutputCousubsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.OutputCousubsTextBox.Location = new System.Drawing.Point(108, 161);
      this.OutputCousubsTextBox.Name = "OutputCousubsTextBox";
      this.OutputCousubsTextBox.Size = new System.Drawing.Size(483, 20);
      this.OutputCousubsTextBox.TabIndex = 16;
      this.OutputCousubsTextBox.Text = "D:\\Users\\CurtNew\\Dropbox\\Documents\\Vote\\Tiger\\Reconcile Districts\\Analysis\\cousub" +
    "s.csv";
      // 
      // OutputCousubsBrowseButton
      // 
      this.OutputCousubsBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.OutputCousubsBrowseButton.Location = new System.Drawing.Point(597, 159);
      this.OutputCousubsBrowseButton.Name = "OutputCousubsBrowseButton";
      this.OutputCousubsBrowseButton.Size = new System.Drawing.Size(75, 23);
      this.OutputCousubsBrowseButton.TabIndex = 17;
      this.OutputCousubsBrowseButton.Text = "Browse...";
      this.OutputCousubsBrowseButton.UseVisualStyleBackColor = true;
      this.OutputCousubsBrowseButton.Click += new System.EventHandler(this.OutputCousubsBrowseButton_Click);
      // 
      // OutputPlacesTextBox
      // 
      this.OutputPlacesTextBox.Location = new System.Drawing.Point(108, 187);
      this.OutputPlacesTextBox.Name = "OutputPlacesTextBox";
      this.OutputPlacesTextBox.Size = new System.Drawing.Size(483, 20);
      this.OutputPlacesTextBox.TabIndex = 18;
      this.OutputPlacesTextBox.Text = "D:\\Users\\CurtNew\\Dropbox\\Documents\\Vote\\Tiger\\Reconcile Districts\\Analysis\\places" +
    ".csv";
      // 
      // OutputPlacesBrowseButton
      // 
      this.OutputPlacesBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.OutputPlacesBrowseButton.Location = new System.Drawing.Point(597, 185);
      this.OutputPlacesBrowseButton.Name = "OutputPlacesBrowseButton";
      this.OutputPlacesBrowseButton.Size = new System.Drawing.Size(75, 23);
      this.OutputPlacesBrowseButton.TabIndex = 19;
      this.OutputPlacesBrowseButton.Text = "Browse...";
      this.OutputPlacesBrowseButton.UseVisualStyleBackColor = true;
      this.OutputPlacesBrowseButton.Click += new System.EventHandler(this.OutputPlacesBrowseButton_Click);
      // 
      // StartShapefilesButton
      // 
      this.StartShapefilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.StartShapefilesButton.Location = new System.Drawing.Point(597, 214);
      this.StartShapefilesButton.Name = "StartShapefilesButton";
      this.StartShapefilesButton.Size = new System.Drawing.Size(75, 23);
      this.StartShapefilesButton.TabIndex = 20;
      this.StartShapefilesButton.Text = "Start";
      this.StartShapefilesButton.UseVisualStyleBackColor = true;
      this.StartShapefilesButton.Click += new System.EventHandler(this.StartShapefilesButton_Click);
      // 
      // CousubSaveFileDialog
      // 
      this.CousubSaveFileDialog.DefaultExt = "csv";
      this.CousubSaveFileDialog.Filter = "csv files|*.csv|All files|*.*";
      // 
      // PlacesSaveFileDialog
      // 
      this.PlacesSaveFileDialog.DefaultExt = "csv";
      this.PlacesSaveFileDialog.Filter = "csv files|*.csv|All files|*.*";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(684, 248);
      this.Controls.Add(this.StartShapefilesButton);
      this.Controls.Add(this.OutputPlacesBrowseButton);
      this.Controls.Add(this.OutputPlacesTextBox);
      this.Controls.Add(this.OutputCousubsBrowseButton);
      this.Controls.Add(this.OutputCousubsTextBox);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.OutputFileBrowseButton);
      this.Controls.Add(this.OutputFileTextBox);
      this.Controls.Add(this.TigerFolderBrowseButton);
      this.Controls.Add(this.TigerFolderTextBox);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.OptionsFileBrowseButton);
      this.Controls.Add(this.OptionsFileTextBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.StateDropDown);
      this.Controls.Add(this.StartButton);
      this.MaximizeBox = false;
      this.Name = "Form1";
      this.ShowIcon = false;
      this.Text = "Analyze Tiger Locals";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.ComboBox StateDropDown;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox OptionsFileTextBox;
    private System.Windows.Forms.Button OptionsFileBrowseButton;
    private System.Windows.Forms.OpenFileDialog OptionsOpenFileDialog;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox TigerFolderTextBox;
    private System.Windows.Forms.Button TigerFolderBrowseButton;
    private System.Windows.Forms.FolderBrowserDialog TigerFolderBrowserDialog;
    private System.Windows.Forms.SaveFileDialog OutputSaveFileDialog;
    private System.Windows.Forms.TextBox OutputFileTextBox;
    private System.Windows.Forms.Button OutputFileBrowseButton;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox OutputCousubsTextBox;
    private System.Windows.Forms.Button OutputCousubsBrowseButton;
    private System.Windows.Forms.TextBox OutputPlacesTextBox;
    private System.Windows.Forms.Button OutputPlacesBrowseButton;
    private System.Windows.Forms.Button StartShapefilesButton;
    private System.Windows.Forms.SaveFileDialog CousubSaveFileDialog;
    private System.Windows.Forms.SaveFileDialog PlacesSaveFileDialog;
  }
}

