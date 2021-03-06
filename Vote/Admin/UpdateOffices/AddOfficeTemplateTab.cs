using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using DB.Vote;

namespace Vote.Admin
{
  public partial class UpdateOfficesPage
  {
    private void InitializeTabOfficeTemplate()
    {
      // attach the county and local office classes
      var countyClasses = Offices.GetOfficeClasses(GetOfficeClassesOptions.IncludeCounty);
      var localClasses = Offices.GetOfficeClasses(GetOfficeClassesOptions.IncludeLocal);
      var classes = new List<List<SimpleListItem>>
      {
        countyClasses.Select(c => new SimpleListItem
        {
          Text = Offices.GetOfficeClassDescription(c, StateCode),
          Value = c.ToString()
        }).ToList(),
        localClasses.Select(c => new SimpleListItem
        {
          Text = Offices.GetOfficeClassDescription(c, StateCode),
          Value = c.ToString()
        }).ToList()
      };
      var json = new JavaScriptSerializer().Serialize(classes);
      AddOfficeTemplateHidden.Attributes.Add("data-office-classes", json);
    }
  }
}