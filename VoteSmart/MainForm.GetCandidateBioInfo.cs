using System;
using MySql.Data.MySqlClient;

namespace VoteSmart
{
  public partial class MainForm
  {
    private static void GetCandidateBioInfo(string candidateId)
    {
      const string bioMethod = "CandidateBio.getBio";
      const string addlBioMethod = "CandidateBio.getAddlBio";
      const string campaignAddressMethod = "Address.getCampaign";
      const string campaignWebAddressMethod = "Address.getCampaignWebAddress";
      const string officeAddressMethod = "Address.getOffice";
      const string officeWebAddressMethod = "Address.getOfficeWebAddress";
      var parameters = "candidateId=" + candidateId;

      // only get bio if not already there
      const string countCmdText = "SELECT COUNT(*) FROM fetches_raw " +
        "WHERE fetch_method=@method AND fetch_parameters=@parameters";
      var bioCountCmd = new MySqlCommand(countCmdText);
      bioCountCmd.Parameters.AddWithValue("@method", bioMethod);
      bioCountCmd.Parameters.AddWithValue("@parameters", parameters);
      int bioCount;
      using (var cn = GetOpenConnection())
      {
        bioCountCmd.Connection = cn;
        bioCount = Convert.ToInt32(bioCountCmd.ExecuteScalar());
      }
      if (bioCount == 0)
        SaveRawData(bioMethod, parameters);

      // only get addlBio if not already there
      var addlBioCountCmd = new MySqlCommand(countCmdText);
      addlBioCountCmd.Parameters.AddWithValue("@method", addlBioMethod);
      addlBioCountCmd.Parameters.AddWithValue("@parameters", parameters);
      int addlBioCount;
      using (var cn = GetOpenConnection())
      {
        addlBioCountCmd.Connection = cn;
        addlBioCount = Convert.ToInt32(addlBioCountCmd.ExecuteScalar());
      }
      if (addlBioCount == 0)
        SaveRawData(addlBioMethod, parameters);

      // only get campaignAddress if not already there
      var campaignAddressCountCmd = new MySqlCommand(countCmdText);
      campaignAddressCountCmd.Parameters.AddWithValue("@method", campaignAddressMethod);
      campaignAddressCountCmd.Parameters.AddWithValue("@parameters", parameters);
      int campaignAddressCount;
      using (var cn = GetOpenConnection())
      {
        campaignAddressCountCmd.Connection = cn;
        campaignAddressCount = Convert.ToInt32(campaignAddressCountCmd.ExecuteScalar());
      }
      if (campaignAddressCount == 0)
        SaveRawData(campaignAddressMethod, parameters);

      // only get campaignWebAddress if not already there
      var campaignWebAddressCountCmd = new MySqlCommand(countCmdText);
      campaignWebAddressCountCmd.Parameters.AddWithValue("@method", campaignWebAddressMethod);
      campaignWebAddressCountCmd.Parameters.AddWithValue("@parameters", parameters);
      int campaignWebAddressCount;
      using (var cn = GetOpenConnection())
      {
        campaignWebAddressCountCmd.Connection = cn;
        campaignWebAddressCount = Convert.ToInt32(campaignWebAddressCountCmd.ExecuteScalar());
      }
      if (campaignWebAddressCount == 0)
        SaveRawData(campaignWebAddressMethod, parameters);

      // only get officeAddress if not already there
      var officeAddressCountCmd = new MySqlCommand(countCmdText);
      officeAddressCountCmd.Parameters.AddWithValue("@method", officeAddressMethod);
      officeAddressCountCmd.Parameters.AddWithValue("@parameters", parameters);
      int officeAddressCount;
      using (var cn = GetOpenConnection())
      {
        officeAddressCountCmd.Connection = cn;
        officeAddressCount = Convert.ToInt32(officeAddressCountCmd.ExecuteScalar());
      }
      if (officeAddressCount == 0)
        SaveRawData(officeAddressMethod, parameters);

      // only get officeWebAddress if not already there
      var officeWebAddressCountCmd = new MySqlCommand(countCmdText);
      officeWebAddressCountCmd.Parameters.AddWithValue("@method", officeWebAddressMethod);
      officeWebAddressCountCmd.Parameters.AddWithValue("@parameters", parameters);
      int officeWebAddressCount;
      using (var cn = GetOpenConnection())
      {
        officeWebAddressCountCmd.Connection = cn;
        officeWebAddressCount = Convert.ToInt32(officeWebAddressCountCmd.ExecuteScalar());
      }
      if (officeWebAddressCount == 0)
        SaveRawData(officeWebAddressMethod, parameters);
    }
  }
}
