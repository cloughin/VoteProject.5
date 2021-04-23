using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  // ReSharper disable NotAccessedField.Global

  public partial class OrganizationContacts
  {
    public static DataTable GetCacheData(int contactId)
    {
      const string cmdText =
        "SELECT oc.ContactId,oc.OrgId,oc.Contact,oc.Email,oc.Phone,oc.Title,o.Name,o.OrgAbbreviation," +
        "o.Address1,o.Address2,o.City,o.StateCode,o.Zip,ot.OrgType," +
        "IF(ost.OrgSubType IS NULL, 'None', ost.OrgSubType) AS OrgSubType," +
        "IF(oi.Ideology IS NULL, 'None', oi.Ideology) AS Ideology FROM OrganizationContacts oc" +
        " INNER JOIN Organizations o ON o.OrgId = oc.OrgId" +
        " INNER JOIN OrganizationTypes ot ON ot.OrgTypeId = o.OrgTypeId" +
        " LEFT OUTER JOIN OrganizationSubTypes ost ON ost.OrgTypeId = o.OrgTypeId" +
        "  AND ost.OrgSubTypeId = o.OrgSubTypeId" +
        " LEFT OUTER JOIN OrganizationIdeologies oi ON oi.IdeologyId = o.IdeologyId" +
        " WHERE oc.ContactId = @ContactId";

      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "ContactId", contactId);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }
  }
}
