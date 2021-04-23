using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB.Vote;
using static System.String;

namespace Vote
{
  public partial class SampleBallotEnrolledPage : PublicPage
  {
    private const string TitleTag = "{0} | Automatic Ballot Choices Enrollment";

    protected SampleBallotEnrolledPage()
    {
      NoUrlEdit = true;
      NoIndex = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      //Page.IncludeJs("~/js/jq/jquery.cookie.js");
      Title = Format(TitleTag, PublicMasterPage.SiteName);
      var email = GetQueryString("email");
      EmailFixedAddress.InnerText = email;

      string tigerCode;

      var address = Request.Cookies["Address"]?.Value;
      var stateCode = Request.Cookies["State"]?.Value;
      var county = Request.Cookies["County"]?.Value;
      var countySupervisors = Request.Cookies["CountySupervisors"]?.Value;
      var congressionalDistrict = Request.Cookies["Congress"]?.Value;
      var stateSenateDistrict = Request.Cookies["StateSenate"]?.Value;
      var stateHouseDistrict = Request.Cookies["StateHouse"]?.Value;
      var place = Request.Cookies["Place"]?.Value;
      var district = Request.Cookies["District"]?.Value;
      var cityCouncil = Request.Cookies["CityCouncil"]?.Value;
      var elementary = Request.Cookies["Elementary"]?.Value;
      var secondary = Request.Cookies["Secondary"]?.Value;
      var unified = Request.Cookies["Unified"]?.Value;
      var schoolDistrictDistrict = Request.Cookies["SchoolDistrictDistrict"]?.Value;
      var components = WebService.GetComponentsFromCookies();
      var (latitude, longitude) = WebService.GetGeoFromCookies();

      if (!StateCache.IsValidStateCode(stateCode) || IsNullOrWhiteSpace(email) ||
        IsNullOrWhiteSpace(congressionalDistrict) ||
        IsNullOrWhiteSpace(stateSenateDistrict) || 
        !Offices.IsValidStateHouseDistrict(stateHouseDistrict, stateCode) ||
        IsNullOrWhiteSpace(county) || components == null || latitude == null ||
        longitude == null)
      {
        SafeTransferToError404();
        //EmailFixedAddress.InnerText = Join("|", stateCode.SafeString(), email.SafeString(),
        //  congressionalDistrict.SafeString(), stateSenateDistrict.SafeString(),
        //  stateHouseDistrict.SafeString(), county.SafeString(), 
        //  components == null ? "null" : "components",
        //  (latitude?.ToString()).SafeString(), (longitude?.ToString()).SafeString());
      }

      WebService.UpdateAddresses(email, "SBRL", Empty, Empty, components,
        stateCode, congressionalDistrict, stateSenateDistrict, stateHouseDistrict, county,
        district, place, elementary, secondary, unified, cityCouncil, countySupervisors,
        schoolDistrictDistrict, latitude, longitude);

      Control tr;

      if (!IsNullOrWhiteSpace(address))
      {
        tr = new TableRow().AddTo(DistrictsTable);
        new TableCell { Text = "Address" }.AddTo(tr);
        new TableCell { Text = HttpUtility.UrlDecode(address) }.AddTo(tr);
      }

      //tr = new TableRow().AddTo(DistrictsTable);
      //new TableCell { Text = "State" }.AddTo(tr);
      //new TableCell { Text = StateCache.GetStateName(stateCode) }.AddTo(tr);

      if (!IsNullOrWhiteSpace(county) && stateCode != "DC")
      {
        tr = new TableRow().AddTo(DistrictsTable);
        new TableCell { Text = "County" }.AddTo(tr);
        new TableCell { Text = CountyCache.GetCountyName(stateCode, county) }.AddTo(tr);
      }

      if (!IsNullOrWhiteSpace(countySupervisors))
      {
        tr = new TableRow().AddTo(DistrictsTable);
        new TableCell {Text = stateCode == "DC" ? "Advisory Neighborhood Commission" : "County Supervisors"}.AddTo(tr);
        new TableCell
        {
          Text = CountySupervisors.GetNameByStateCodeCountySupervisorsCode(stateCode,
            countySupervisors)
        }.AddTo(tr);
      }

      if (!IsNullOrWhiteSpace(congressionalDistrict) && stateCode != "DC")
      {
        if ((tigerCode =
          TigerToVoteCodes.GetTigerCodeByTableTypeStateCodeVoteCode("CD", stateCode,
            congressionalDistrict)) != null)
          congressionalDistrict = tigerCode;
        if (congressionalDistrict != "00")
        {
          tr = new TableRow().AddTo(DistrictsTable);
          new TableCell {Text = "Congressional District"}.AddTo(tr);
          new TableCell {Text = congressionalDistrict.TrimStart('0')}.AddTo(tr);
        }
      }

      if (!IsNullOrWhiteSpace(stateSenateDistrict))
      {
        if ((tigerCode =
          TigerToVoteCodes.GetTigerCodeByTableTypeStateCodeVoteCode("SS", stateCode,
            stateSenateDistrict)) != null)
          stateSenateDistrict = tigerCode;
        tr = new TableRow().AddTo(DistrictsTable);
        new TableCell {Text = stateCode == "DC" ? "Ward" : "State Senate District"}.AddTo(tr);
        new TableCell {Text = stateSenateDistrict?.TrimStart('0')}.AddTo(tr);
      }

      if (!IsNullOrWhiteSpace(stateHouseDistrict))
      {
        if ((tigerCode =
          TigerToVoteCodes.GetTigerCodeByTableTypeStateCodeVoteCode("SH", stateCode,
            stateHouseDistrict)) != null)
          stateHouseDistrict = tigerCode;
        tr = new TableRow().AddTo(DistrictsTable);
        new TableCell {Text = "State House District"}.AddTo(tr);
        new TableCell {Text = stateHouseDistrict.TrimStart('0')}.AddTo(tr);
      }

      if (stateCode != "DC")
      { 
        var districts = new List<string>();
        if (!IsNullOrWhiteSpace(place))
          districts.Add(TigerPlaces.GetNameByStateCodeTigerCode(stateCode, place));
        if (!IsNullOrWhiteSpace(district))
          districts.Add(TigerPlaces.GetNameByStateCodeTigerCode(stateCode, district));
        if (districts.Count > 0)
        {
          tr = new TableRow().AddTo(DistrictsTable);
          new TableCell {Text = "Local District"}.AddTo(tr);
          new TableCell {Text = Join(", ", districts.Distinct())}.AddTo(tr);
        }
      }

      if (!IsNullOrWhiteSpace(cityCouncil) && stateCode != "DC")
      {
        tr = new TableRow().AddTo(DistrictsTable);
        new TableCell {Text = "City Council"}.AddTo(tr);
        new TableCell
        {
          Text = CityCouncil.GetNameByStateCodeCityCouncilCode(stateCode, cityCouncil)
        }.AddTo(tr);
      }

      if (!IsNullOrWhiteSpace(elementary))
      {
        tr = new TableRow().AddTo(DistrictsTable);
        new TableCell {Text = "Elementary School District"}.AddTo(tr);
        new TableCell
        {
          Text = TigerSchools.GetNameByStateCodeTigerCodeTigerType(stateCode, elementary,
            "E")
        }.AddTo(tr);
      }

      if (!IsNullOrWhiteSpace(secondary))
      {
        tr = new TableRow().AddTo(DistrictsTable);
        new TableCell {Text = "Secondary School District"}.AddTo(tr);
        new TableCell
        {
          Text = TigerSchools.GetNameByStateCodeTigerCodeTigerType(stateCode, secondary,
            "S")
        }.AddTo(tr);
      }

      if (!IsNullOrWhiteSpace(unified))
      {
        tr = new TableRow().AddTo(DistrictsTable);
        new TableCell {Text = "Unified School District"}.AddTo(tr);
        new TableCell
        {
          Text = TigerSchools.GetNameByStateCodeTigerCodeTigerType(stateCode, unified, "U")
        }.AddTo(tr);
      }

      if (!IsNullOrWhiteSpace(schoolDistrictDistrict))
      {
        tr = new TableRow().AddTo(DistrictsTable);
        new TableCell {Text = "School Voting District"}.AddTo(tr);
        new TableCell
        {
          Text = SchoolDistrictDistricts.GetNameByStateCodeSchoolDistrictDistrictCode(
            stateCode, schoolDistrictDistrict)
        }.AddTo(tr);
      }
    }
  }
}