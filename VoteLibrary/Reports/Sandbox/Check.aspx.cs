using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB.Vote;

namespace Vote.Sandbox
{
  public partial class Check : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Label1.Text = "Offices.GetLocalizedOfficeName(\"USPresident\") [Level 1:USPresident]";
      Value1.Text = Offices.GetLocalizedOfficeName("USPresident");
      Label2.Text = "Offices.GetLocalizedOfficeName(\"CAUSSenate\") [Level 2:USSenate]";
      Value2.Text = Offices.GetLocalizedOfficeName("CAUSSenate");
      Label3.Text = "Offices.GetLocalizedOfficeName(\"CAUSHouse33\") [Level 3:USHouse]";
      Value3.Text = Offices.GetLocalizedOfficeName("CAUSHouse33");
      Label4.Text = "Offices.GetLocalizedOfficeName(\"CAGovernor\") [Level 4:StateWide]";
      Value4.Text = Offices.GetLocalizedOfficeName("CAGovernor");
      Label5.Text = "Offices.GetLocalizedOfficeName(\"DCMayor\") [Level 4:StateWide]";
      Value5.Text = Offices.GetLocalizedOfficeName("DCMayor");
      Label6.Text = "Offices.GetLocalizedOfficeName(\"CAStateSenate34\") [Level 5:StateSenate]";
      Value6.Text = Offices.GetLocalizedOfficeName("CAStateSenate34");
      Label7.Text = "Offices.GetLocalizedOfficeName(\"DCBoardOfEducation3\") [Level 5:StateSenate]";
      Value7.Text = Offices.GetLocalizedOfficeName("DCBoardOfEducation3");
      Label8.Text = "Offices.GetLocalizedOfficeName(\"CAStateHouse61\") [Level 6:StateHouse]";
      Value8.Text = Offices.GetLocalizedOfficeName("CAStateHouse61");
      Label9.Text = "Offices.GetLocalizedOfficeName(\"DCStateHouse4\") [Level 6:StateHouse]";
      Value9.Text = Offices.GetLocalizedOfficeName("DCStateHouse4");
      Label10.Text = "Offices.GetLocalizedOfficeName(\"VA059Treasurer\") [Level 8:CountyExecutive]";
      Value10.Text = Offices.GetLocalizedOfficeName("VA059Treasurer");
      Label11.Text = "Offices.GetLocalizedOfficeName(\"VA059BoardOfSupervisors\") [Level 9:CountyLegislative]";
      Value11.Text = Offices.GetLocalizedOfficeName("VA059BoardOfSupervisors");
      Label12.Text = "Offices.GetLocalizedOfficeName(\"VA059SchoolBoard\") [Level 10:CountySchoolBoard]";
      Value12.Text = Offices.GetLocalizedOfficeName("VA059SchoolBoard");
      Label13.Text = "Offices.GetLocalizedOfficeName(\"VA05931Mayor\") [Level 12:LocalExecutive]";
      Value13.Text = Offices.GetLocalizedOfficeName("VA05931Mayor");
      Label14.Text = "Offices.GetLocalizedOfficeName(\"VA05916BoardOfSupervisors\") [Level 13:LocalLegislative]";
      Value14.Text = Offices.GetLocalizedOfficeName("VA05916BoardOfSupervisors");
      Label15.Text = "Offices.GetLocalizedOfficeName(\"VA05916SchoolBoard\") [Level 14:LocalSchoolBoard]";
      Value15.Text = Offices.GetLocalizedOfficeName("VA05916SchoolBoard");
      Label16.Text = "Offices.GetLocalizedOfficeName(\"VA05918SoilWaterDirector\") [Level 15:LocalCommission]";
      Value16.Text = Offices.GetLocalizedOfficeName("VA05918SoilWaterDirector");

      //Label1.Text = "db.Name_Office_State(\"USPresident\") [Level 1:USPresident]";
      //Value1.Text = db.Name_Office_State("USPresident");
      //Label2.Text = "db.Name_Office_State(\"CAUSSenate\") [Level 2:USSenate]";
      //Value2.Text = db.Name_Office_State("CAUSSenate");
      //Label3.Text = "db.Name_Office_State(\"CAUSHouse33\") [Level 3:USHouse]";
      //Value3.Text = db.Name_Office_State("CAUSHouse33");
      //Label4.Text = "db.Name_Office_State(\"CAGovernor\") [Level 4:StateWide]";
      //Value4.Text = db.Name_Office_State("CAGovernor");
      //Label5.Text = "db.Name_Office_State(\"DCMayor\") [Level 4:StateWide]";
      //Value5.Text = db.Name_Office_State("DCMayor");
      //Label6.Text = "db.Name_Office_State(\"CAStateSenate34\") [Level 5:StateSenate]";
      //Value6.Text = db.Name_Office_State("CAStateSenate34");
      //Label7.Text = "db.Name_Office_State(\"DCBoardOfEducation3\") [Level 5:StateSenate]";
      //Value7.Text = db.Name_Office_State("DCBoardOfEducation3");
      //Label8.Text = "db.Name_Office_State(\"CAStateHouse61\") [Level 6:StateHouse]";
      //Value8.Text = db.Name_Office_State("CAStateHouse61");
      //Label9.Text = "db.Name_Office_State(\"DCStateHouse4\") [Level 6:StateHouse]";
      //Value9.Text = db.Name_Office_State("DCStateHouse4");
      //Label10.Text = "db.Name_Office_State(\"VA059Treasurer\") [Level 8:CountyExecutive]";
      //Value10.Text = db.Name_Office_State("VA059Treasurer");
      //Label11.Text = "db.Name_Office_State(\"VA059BoardOfSupervisors\") [Level 9:CountyLegislative]";
      //Value11.Text = db.Name_Office_State("VA059BoardOfSupervisors");
      //Label12.Text = "db.Name_Office_State(\"VA059SchoolBoard\") [Level 10:CountySchoolBoard]";
      //Value12.Text = db.Name_Office_State("VA059SchoolBoard");
      //Label13.Text = "db.Name_Office_State(\"VA05931Mayor\") [Level 12:LocalExecutive]";
      //Value13.Text = db.Name_Office_State("VA05931Mayor");
      //Label14.Text = "db.Name_Office_State(\"VA05916BoardOfSupervisors\") [Level 13:LocalLegislative]";
      //Value14.Text = db.Name_Office_State("VA05916BoardOfSupervisors");
      //Label15.Text = "db.Name_Office_State(\"VA05916SchoolBoard\") [Level 14:LocalSchoolBoard]";
      //Value15.Text = db.Name_Office_State("VA05916SchoolBoard");
      //Label16.Text = "db.Name_Office_State(\"VA05918SoilWaterDirector\") [Level 15:LocalCommission]";
      //Value16.Text = db.Name_Office_State("VA05918SoilWaterDirector");
      //Label17.Text = "db.Name_Office_State(\"USPresident\") [Level 9:]";
      //Value17.Text = db.Name_Office_State("USPresident");
      //Label18.Text = "db.Name_Office_State(\"USPresident\") [Level 9:]";
      //Value18.Text = db.Name_Office_State("USPresident");
      //Label19.Text = "db.Name_Office_State(\"USPresident\") [Level 9:]";
      //Value19.Text = db.Name_Office_State("USPresident");
      //Label20.Text = "db.Name_Office_State(\"USPresident\") [Level 9:]";
      //Value20.Text = db.Name_Office_State("USPresident");
    }
  }
}