using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DB.VoteSecurity;

namespace VoteSecurity.AppCode
{
  public class VoteRoleProvider : System.Web.Security.SqlRoleProvider
  {
    private Guid? _ApplicationId = null;

    private Guid ApplicationId
    {
      get
      {
        if (_ApplicationId == null)
          _ApplicationId = Applications.
            GetApplicationIdByApplicationName(ApplicationName).Value;
        return _ApplicationId.Value;
      }
    }
  
    public override string[] GetRolesForUser(string username)
    {
      // Get the primary list of roles from the base class
      string[] originalRoles = base.GetRolesForUser(username);

      // for each of these roles, look up any implied roles
      var allRolesList =
        originalRoles.SelectMany(
          roleName =>
            {
              Guid roleIdFrom = 
                Roles.GetRoleIdByApplicationIdLoweredRoleName(
                ApplicationId, roleName).Value;
              var table = RolesInRolesView.GetDataByRoleIdFrom(roleIdFrom);
              return table.Select(row => row.RoleNameTo);
            }).ToList();

      // add the originals to the new list
      allRolesList.AddRange(originalRoles); 
      
      string[] allRoles = 
        allRolesList
        .Distinct()
        .OrderBy(role => role)
        .ToArray(); // no duplicates, sorted

      return allRoles;
    }
  }
}