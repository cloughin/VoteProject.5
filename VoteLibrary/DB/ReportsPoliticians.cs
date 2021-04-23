using Vote;

namespace DB.Vote
{
  //public partial class ReportsPoliticiansRow {}

  //public partial class ReportsPoliticians
  //{
  //  public static void SetNotCurrent(
  //    int officeLevel, string stateCode, string countyCode, string localCode)
  //  {
  //    // We do this for everybody
  //    UpdateIsReportCurrentByStateCodeCountyCodeLocalCodeOfficeLevel(
  //      false, stateCode, string.Empty, string.Empty, OfficeClass.All.ToInt());
  //    UpdateIsReportCurrentByStateCodeCountyCodeLocalCodeOfficeLevel(
  //      false, stateCode, string.Empty, string.Empty, officeLevel);

  //    // If there is a countyCode...
  //    if (string.IsNullOrWhiteSpace(countyCode)) return;
  //    UpdateIsReportCurrentByStateCodeCountyCodeLocalCodeOfficeLevel(
  //      false, stateCode, countyCode, string.Empty, OfficeClass.All.ToInt());
  //    UpdateIsReportCurrentByStateCodeCountyCodeLocalCodeOfficeLevel(
  //      false, stateCode, countyCode, string.Empty, officeLevel);

  //    // If there is a localCode...
  //    if (string.IsNullOrWhiteSpace(localCode)) return;
  //    UpdateIsReportCurrentByStateCodeCountyCodeLocalCodeOfficeLevel(
  //      false, stateCode, countyCode, localCode, OfficeClass.All.ToInt());
  //    UpdateIsReportCurrentByStateCodeCountyCodeLocalCodeOfficeLevel(
  //      false, stateCode, countyCode, localCode, officeLevel);
  //  }

  //  public static void SetNotCurrent(
  //    OfficeClass officeClass, string stateCode, string countyCode, string localCode)
  //  {
  //    SetNotCurrent(officeClass.ToInt(), stateCode, countyCode, localCode);
  //  }
  //}
}