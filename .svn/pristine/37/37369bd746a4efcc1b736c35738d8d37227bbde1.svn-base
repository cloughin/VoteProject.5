using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CloneControl;
using DB.Vote;
using Vote;
using System.Configuration;

namespace TestRedirect
{
  public partial class Instructions : Form
  {
    static Instructions Current;

    enum Table
    {
      MasterDesign,
      DomainDesigns
    }

    #region InstructionSets
    List<InstructionSet> InstructionSets = new List<InstructionSet>()
    {
      new InstructionSet()
      {
        Title = "Archives",
        FlagColumnName = "IsTextInstructionsArchivesPage",
        TextColumnName = "InstructionsArchivesPage"
      },
      new InstructionSet()
      {
        Title = "Ballot Upcoming",
        FlagColumnName = "IsTextInstructionsUpcomingElectionBallotPage",
        TextColumnName = "InstructionsUpcomingElectionBallotPage"
      },
      new InstructionSet()
      {
        Title = "Ballot Previous",
        FlagColumnName = "IsTextInstructionsUpcomingElectionBallotPage",
        TextColumnName = "InstructionsUpcomingElectionBallotPage"
      },
      new InstructionSet()
      {
        Title = "Elected",
        FlagColumnName = "IsTextInstructionsElectedPage",
        TextColumnName = "InstructionsElectedPage"
      },
      new InstructionSet()
      {
        Title = "Election Pres Pri",
        FlagColumnName = "IsTextInstructionsElectionPageUSPresPrimary",
        TextColumnName = "InstructionsElectionPageUSPresPrimary"
      },
      new InstructionSet()
      {
        Title = "Election Pres",
        FlagColumnName = "IsTextInstructionsElectionPageUSPres",
        TextColumnName = "InstructionsElectionPageUSPres"
      },
      new InstructionSet()
      {
        Title = "Election Senate",
        FlagColumnName = "IsTextInstructionsElectionPageUSSenate",
        TextColumnName = "InstructionsElectionPageUSSenate"
      },
      new InstructionSet()
      {
        Title = "Election House",
        FlagColumnName = "IsTextInstructionsElectionPageUSHouse",
        TextColumnName = "InstructionsElectionPageUSHouse"
      },
      new InstructionSet()
      {
        Title = "Election Governors",
        FlagColumnName = "IsTextInstructionsElectedOfficialsPageGovernors",
        TextColumnName = "InstructionsElectedOfficialsPageGovernors"
      },
      new InstructionSet()
      {
        Title = "Election State",
        FlagColumnName = "IsTextInstructionsElectionPageState",
        TextColumnName = "InstructionsElectionPageState"
      },
      new InstructionSet()
      {
        Title = "Election County",
        FlagColumnName = "IsTextInstructionsElectionPageCounty",
        TextColumnName = "InstructionsElectionPageCounty"
      },
      new InstructionSet()
      {
        Title = "Election Local",
        FlagColumnName = "IsTextInstructionsElectionPageLocal",
        TextColumnName = "InstructionsElectionPageLocal"
      }
    };
    #endregion InstructionSets

    MasterDesignTable MasterDesignTable;
    DomainDesignsTable DomainDesignsTable;
    string[] DomainDesignCodes;

    Control ModelItem;

    public Instructions()
    {
      InitializeComponent();
      Current = this;

      // Get the model item structure and detach it
      ModelItem = ItemGroupBox;
      ModelItem.Parent.Controls.Remove(ModelItem);

      // Clear any pre-existing tabs
      MainTabControl.TabPages.Clear();

      // Create tabs
      foreach (var instructionSet in InstructionSets)
      {
        TabPage tabPage = new TabPage(instructionSet.Title);
        instructionSet.TabPage = tabPage;
        tabPage.Tag = instructionSet;
        tabPage.AutoScroll = true;
        MainTabControl.TabPages.Add(tabPage);
      }

      SetConnectionString();
      LoadData();

      //InitAllPages();
    }

    private void LoadData()
    {
      MasterDesignTable = MasterDesign.GetAllData();
      DomainDesignsTable = DomainDesigns.GetAllData();
      DomainDesignCodes = DomainDesignsTable
        .Select(row => row.DomainDesignCode)
        .ToArray();

      foreach (TabPage tabPage in MainTabControl.TabPages)
        LoadData(tabPage);

      CreateControls(MainTabControl.SelectedTab);
    }

    private void CreateControls(TabPage tabPage)
    {
      ClearControls(tabPage);
      InstructionSet instructionSet = tabPage.Tag as InstructionSet;

      tabPage.SuspendLayout();
      foreach (var itemInfo in instructionSet.Items)
      {
        Control newItem = DeepCloneControl(ModelItem);

        int controlCount = tabPage.Controls.Count;
        if (controlCount > 0)
        {
          Control lastControl = tabPage.Controls[controlCount - 1];
          int bottom = lastControl.Location.Y + lastControl.Size.Height;
          newItem.Location = new Point(newItem.Location.X, bottom + 10);
        }

        tabPage.Controls.Add(newItem);
        itemInfo.AttachToControl(newItem);
      }
      tabPage.ResumeLayout();
    }

    private void ClearControls(TabPage tabPage)
    {
      // Save current values
      foreach (Control control in tabPage.Controls)
      {
        ItemInfo itemInfo = control.Tag as ItemInfo;
        itemInfo.Flag = itemInfo.CheckBox.Checked;
        itemInfo.Text = itemInfo.TextBox.Text;
      }
      tabPage.Controls.Clear();
    }

    private void LoadData(TabPage tabPage)
    {
      tabPage.Controls.Clear();
      InstructionSet instructionSet = tabPage.Tag as InstructionSet;
      instructionSet.Items = new List<ItemInfo>();
      LoadMasterData(tabPage);
      LoadDomainData(tabPage);
    }

    private void LoadMasterData(TabPage tabPage)
    {
      LoadData(tabPage, Table.MasterDesign, null);
    }

    private void LoadDomainData(TabPage tabPage)
    {
      foreach (string domainDesignCode in DomainDesignCodes)
        LoadData(tabPage, Table.DomainDesigns, domainDesignCode);
    }

    private static void LoadData(TabPage tabPage, Table table, string domainCode)
    {
      InstructionSet instructionSet = tabPage.Tag as InstructionSet;

      string keyIndex = string.Empty;
      if (!string.IsNullOrWhiteSpace(domainCode))
        keyIndex = "[" + domainCode + "]";
      string title = Table.MasterDesign + keyIndex + "." + instructionSet.TextColumnName;

      ItemInfo itemInfo = new ItemInfo()
      {
        Table = table,
        FlagColumnName = instructionSet.FlagColumnName,
        TextColumnName = instructionSet.TextColumnName,
        Domain = domainCode,
        Title = title
      };
      itemInfo.GetValues();
      instructionSet.Items.Add(itemInfo);
    }

    #region InitPages methods
    private void InitAllPages()
    {
      foreach (TabPage tabPage in MainTabControl.TabPages)
        tabPage.Controls.Clear();

      GetTables();
      InitArchivesPage();
      //InitBallotUpcomingPage();
      //InitBallotPreviousPage();
      InitElectedPage();
      //InitElectionUSPresidentPrimaryPage();
      InitElectionUSPresidentPage();
      //InitElectionUSSenatePage();
      //InitElectionUSHousePage();
      //InitElectionGovernorsPage();
      //InitElectionStatePage();
      //InitElectionCountyPage();
      //InitElectionLocalPage();

      HideEmptyCheckBox.Checked = false;
    }

    private void InitArchivesPage()
    {
      AddMasterDesignItem(ArchivesTabPage, "IsTextInstructionsArchivesPage", 
        "InstructionsArchivesPage");
      AddDomainDesignsItems(ArchivesTabPage, "IsTextInstructionsArchivesPage", 
        "InstructionsArchivesPage");
    }

    private void InitBallotPreviousPage()
    {
      AddMasterDesignItem(BallotUpcomingTabPage, "IsTextInstructionsUpcomingElectionBallotPage", 
        "InstructionsUpcomingElectionBallotPage");
      AddDomainDesignsItems(BallotUpcomingTabPage, "IsTextInstructionsUpcomingElectionBallotPage", 
        "InstructionsUpcomingElectionBallotPage");
    }

    private void InitBallotUpcomingPage()
    {
      AddMasterDesignItem(BallotPreviousTabPage, "IsTextInstructionsPreviousElectionBallotPage",
        "InstructionsPreviousElectionBallotPage");
      AddDomainDesignsItems(BallotPreviousTabPage, "IsTextInstructionsPreviousElectionBallotPage",
        "InstructionsPreviousElectionBallotPage");
    }

    private void InitElectedPage()
    {
      AddMasterDesignItem(ElectedTabPage, "IsTextInstructionsElectedPage",
        "InstructionsElectedPage");
      AddDomainDesignsItems(ElectedTabPage, "IsTextInstructionsElectedPage",
        "InstructionsElectedPage");
    }

    private void InitElectionUSPresidentPrimaryPage()
    {
      AddMasterDesignItem(ElectionUSPresidentPrimaryTabPage, "IsTextInstructionsElectionPageUSPresPrimary",
        "InstructionsElectionPageUSPresPrimary");
      AddDomainDesignsItems(ElectionUSPresidentPrimaryTabPage, "IsTextInstructionsElectionPageUSPresPrimary",
        "InstructionsElectionPageUSPresPrimary");
    }

    private void InitElectionUSPresidentPage()
    {
      AddMasterDesignItem(ElectionUSPresidentTabPage, "IsTextInstructionsElectionPageUSPres",
        "InstructionsElectionPageUSPres");
      AddDomainDesignsItems(ElectionUSPresidentTabPage, "IsTextInstructionsElectionPageUSPres",
        "InstructionsElectionPageUSPres");
    }

    private void InitElectionUSSenatePage()
    {
      AddMasterDesignItem(ElectionUSSenateTabPage, "IsTextInstructionsElectionPageUSSenate",
        "InstructionsElectionPageUSSenate");
      AddDomainDesignsItems(ElectionUSSenateTabPage, "IsTextInstructionsElectionPageUSSenate",
        "InstructionsElectionPageUSSenate");
    }

    private void InitElectionUSHousePage()
    {
      AddMasterDesignItem(ElectionUSHouseTabPage, "IsTextInstructionsElectionPageUSHouse",
        "InstructionsElectionPageUSHouse");
      AddDomainDesignsItems(ElectionUSHouseTabPage, "IsTextInstructionsElectionPageUSHouse",
        "InstructionsElectionPageUSHouse");
    }

    private void InitElectionGovernorsPage()
    {
      AddMasterDesignItem(ElectionGovernorsTabPage, "IsTextInstructionsElectedOfficialsPageGovernors",
        "InstructionsElectedOfficialsPageGovernors");
      AddDomainDesignsItems(ElectionGovernorsTabPage, "IsTextInstructionsElectedOfficialsPageGovernors",
        "InstructionsElectedOfficialsPageGovernors");
    }

    private void InitElectionStatePage()
    {
      AddMasterDesignItem(ElectionStateTabPage, "IsTextInstructionsElectionPageState",
        "InstructionsElectionPageState");
      AddDomainDesignsItems(ElectionStateTabPage, "IsTextInstructionsElectionPageState",
        "InstructionsElectionPageState");
    }

    private void InitElectionCountyPage()
    {
      AddMasterDesignItem(ElectionCountyTabPage, "IsTextInstructionsElectionPageCounty",
        "InstructionsElectionPageCounty");
      AddDomainDesignsItems(ElectionCountyTabPage, "IsTextInstructionsElectionPageCounty",
        "InstructionsElectionPageCounty");
    }

    private void InitElectionLocalPage()
    {
      AddMasterDesignItem(ElectionLocalTabPage, "IsTextInstructionsElectionPageLocal",
        "InstructionsElectionPageLocal");
      AddDomainDesignsItems(ElectionLocalTabPage, "IsTextInstructionsElectionPageLocal",
        "InstructionsElectionPageLocal");
    }

    private void AddDomainDesignsItems(TabPage tabPage, string flagColumn, string textColumn)
    {
      foreach (string domainDesignCode in DomainDesignCodes)
        AddItem(tabPage, Table.DomainDesigns, flagColumn, textColumn, domainDesignCode);
    }

    private void AddMasterDesignItem(TabPage tabPage, string flagColumn, string textColumn)
    {
      AddItem(tabPage, Table.MasterDesign, flagColumn, textColumn, null);
    }

    private void AddItem(TabPage tabPage, Table table, string flagColumn, string textColumn, string domain)
    {
      Control newItem = DeepCloneControl(ModelItem);

      int controlCount = tabPage.Controls.Count;
      if (controlCount > 0)
      {
        Control lastControl = tabPage.Controls[controlCount - 1];
        int bottom = lastControl.Location.Y + lastControl.Size.Height;
        newItem.Location = new Point(newItem.Location.X, bottom + 10);
      }

      tabPage.Controls.Add(newItem);
      AttachItemInfo(newItem, table, flagColumn, textColumn, domain);
    }

    private void AttachItemInfo(Control newItem, Table table, string flagColumn, string textColumn, string domain)
    {
      ItemInfo itemInfo = new ItemInfo()
      {
        Table = table,
        FlagColumnName = flagColumn,
        TextColumnName = textColumn,
        Domain = domain,
        GroupBox = Find(newItem, "ItemGroupBox") as GroupBox,
        CheckBox = Find(newItem, "ItemCheckBox") as CheckBox,
        TextBox = Find(newItem, "ItemTextBox") as TextBox,
        UpdateButton = Find(newItem, "ItemUpdateButton") as Button
      };

      itemInfo.GroupBox.Tag = itemInfo;
      itemInfo.CheckBox.Tag = itemInfo;
      itemInfo.TextBox.Tag = itemInfo;
      itemInfo.UpdateButton.Tag = itemInfo;

      string keyIndex = string.Empty;
      if (!string.IsNullOrWhiteSpace(domain))
        keyIndex = "[" + domain + "]";
      string name = table + keyIndex + "." + textColumn;
      itemInfo.GroupBox.Text = name;

      if (string.IsNullOrWhiteSpace(flagColumn))
      {
        itemInfo.CheckBox.Enabled = false;
        itemInfo.CheckBox.Text = "no flag";
      }
      else
        itemInfo.CheckBox.Text = flagColumn;

      itemInfo.CheckBox.CheckedChanged += ItemCheckBox_CheckedChanged;
      itemInfo.TextBox.TextChanged += ItemTextBox_TextChanged;
      itemInfo.UpdateButton.Click += ItemUpdateButton_Click;

      itemInfo.GetValues();
    }
    #endregion InitPages methods

    #region Utility methods

    private Control DeepCloneControl(Control control)
    {
      Control newControl = ControlFactory.CloneCtrl(control);

      foreach (Control child in control.Controls)
        newControl.Controls.Add(DeepCloneControl(child));

      return newControl;
    }

    private static Control Find(Control control, string name)
    {
      if (control.Name == name) return control;

      foreach (Control child in control.Controls)
      {
        Control result = Find(child, name);
        if (result != null) return result;
      }

      return null;
    }

    private void GetTables()
    {
      MasterDesignTable = MasterDesign.GetAllData();
      DomainDesignsTable = DomainDesigns.GetAllData();
      DomainDesignCodes = DomainDesignsTable
        .Select(row => row.DomainDesignCode)
        .ToArray();
    }

    private void SetConnectionString()
    {
      string appSettingskey;
      if (TestServerRadioButton.Checked)
        appSettingskey = "TestDb";
      else if (LiveServerRadioButton.Checked)
        appSettingskey = "LiveDb";
      else
        throw new VoteException("Invalid server");
      string connectionString = ConfigurationManager.AppSettings[appSettingskey];
      if (string.IsNullOrWhiteSpace(connectionString))
        throw new VoteException("Invalid connection string");
      VoteDb.ConnectionString = connectionString;
    }

    private void SetItemVisibility()
    {
      bool emptyVisibility = !HideEmptyCheckBox.Checked;
      foreach (TabPage tabPage in MainTabControl.TabPages)
        foreach (Control control in tabPage.Controls)
        {
          ItemInfo itemInfo = control.Tag as ItemInfo;
          if (itemInfo.OriginalText == string.Empty)
            control.Visible = emptyVisibility;
          else
            control.Visible = true;
        }
    }

    #endregion Utility methods

    #region Event Handlers
    private void HideEmptyCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      SetItemVisibility();
    }

    private void ItemUpdateButton_Click(object sender, EventArgs e)
    {
      Button button = sender as Button;
      ItemInfo itemInfo = button.Tag as ItemInfo;
      itemInfo.Update();
    }

    private void ItemCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      CheckBox checkBox = sender as CheckBox;
      ItemInfo itemInfo = checkBox.Tag as ItemInfo;
      itemInfo.EnableUpdateButton();
    }

    private void ItemTextBox_TextChanged(object sender, EventArgs e)
    {
      TextBox textBox = sender as TextBox;
      ItemInfo itemInfo = textBox.Tag as ItemInfo;
      itemInfo.EnableUpdateButton();
    }

    private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      foreach (TabPage tabPage in MainTabControl.TabPages)
        ClearControls(tabPage);
      CreateControls(MainTabControl.SelectedTab);
    }

    private void ServerRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      RadioButton radioButton = sender as RadioButton;
      if (radioButton != null && radioButton.Checked)
      {
        SetConnectionString();
        LoadData();
      }
    }
    #endregion Event Handlers

    #region InstructionSet
    class InstructionSet
    {
      public string Title;
      public string FlagColumnName;
      public string TextColumnName;
      public List<ItemInfo> Items;
      public TabPage TabPage;
    }
    #endregion InstructionSet

    #region ItemInfo class
    class ItemInfo
    {
      public Table Table;
      public string FlagColumnName;
      public string TextColumnName;
      public string Domain;
      public GroupBox GroupBox;
      public CheckBox CheckBox;
      public TextBox TextBox;
      public Button UpdateButton;
      public bool OriginalFlag;
      public string OriginalText = string.Empty;
      public bool Flag;
      public string Text;
      public string Title;

      internal void EnableUpdateButton()
      {
        UpdateButton.Enabled = CheckBox.Checked != OriginalFlag ||
          TextBox.Text != OriginalText;
      }

      internal void GetValues()
      {
        switch (Table)
        {
          case Table.MasterDesign:
            {
              var row = Current.MasterDesignTable[0];
              OriginalFlag = (bool) row[FlagColumnName];
              //CheckBox.Checked = OriginalFlag;
              OriginalText = row[TextColumnName] as string;
              //TextBox.Text = OriginalText;
              Flag = OriginalFlag;
              Text = OriginalText;
            }
            break;

          case Table.DomainDesigns:
            {
              var row = Current.DomainDesignsTable
                .Where(r => r.DomainDesignCode == Domain)
                .Single();
              OriginalFlag = (bool) row[FlagColumnName];
              //CheckBox.Checked = OriginalFlag;
              OriginalText = row[TextColumnName] as string;
              //TextBox.Text = OriginalText;
              Flag = OriginalFlag;
              Text = OriginalText;
            }
            break;
        }
      }

      internal void Update()
      {
        if (CheckBox.Checked != OriginalFlag)
          UpdateFlag();
        if (TextBox.Text != OriginalText)
          UpdateText();
        EnableUpdateButton();
      }

      private void UpdateFlag()
      {
        switch (Table)
        {
          case Table.MasterDesign:
            {
              MasterDesign.Column flagColumn = MasterDesign.GetColumn(FlagColumnName);
              MasterDesign.UpdateColumn(flagColumn, CheckBox.Checked);
              OriginalFlag = CheckBox.Checked;
            }
            break;

          case Table.DomainDesigns:
            {
              DomainDesigns.Column flagColumn = DomainDesigns.GetColumn(FlagColumnName);
              DomainDesigns.UpdateColumn(flagColumn, CheckBox.Checked, Domain);
              OriginalFlag = CheckBox.Checked;
            }
            break;
        }
      }

      private void UpdateText()
      {
        switch (Table)
        {
          case Table.MasterDesign:
            {
              MasterDesign.Column textColumn = MasterDesign.GetColumn(TextColumnName);
              MasterDesign.UpdateColumn(textColumn, TextBox.Text);
              OriginalText = TextBox.Text;
            }
            break;

          case Table.DomainDesigns:
            {
              DomainDesigns.Column textColumn = DomainDesigns.GetColumn(TextColumnName);
              DomainDesigns.UpdateColumn(textColumn, TextBox.Text, Domain);
              OriginalText = TextBox.Text;
            }
           break;
        }
      }

      internal void AttachToControl(Control newItem)
      {
        GroupBox = Find(newItem, "ItemGroupBox") as GroupBox;
        CheckBox = Find(newItem, "ItemCheckBox") as CheckBox;
        TextBox = Find(newItem, "ItemTextBox") as TextBox;
        UpdateButton = Find(newItem, "ItemUpdateButton") as Button;

        GroupBox.Tag = this;
        CheckBox.Tag = this;
        TextBox.Tag = this;
        UpdateButton.Tag = this;

        GroupBox.Text = Title;
        CheckBox.Text = FlagColumnName;
        CheckBox.Checked = Flag;
        TextBox.Text = Text;

        CheckBox.CheckedChanged += Current.ItemCheckBox_CheckedChanged;
        TextBox.TextChanged += Current.ItemTextBox_TextChanged;
        UpdateButton.Click += Current.ItemUpdateButton_Click;

        EnableUpdateButton();
      }
    }
    #endregion ItemInfo class
  }
}
