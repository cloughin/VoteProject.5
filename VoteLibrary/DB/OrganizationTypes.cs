using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  // ReSharper disable NotAccessedField.Global
  public sealed class OrganizationTypesData
  {
    public int OrgTypeId;
    public string OrgType;
    public int Count;
  }

  public partial class OrganizationTypes
  {
    public static OrganizationTypesData[] GetOrganizationTypesData()
    {
      const string cmdText =
        "SELECT ot.OrgTypeId,ot.OrgType,(o.OrgTypeId IS NULL) AS IsNull,COUNT(*) AS Count" +
        " FROM OrganizationTypes ot" +
        " LEFT OUTER JOIN Organizations o ON o.OrgTypeId=ot.OrgTypeId" +
        " GROUP BY OrgTypeId" +
        " ORDER BY ot.OrgTypeOrder";

      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>().Select(r =>
          new OrganizationTypesData
          {
            OrgTypeId = r.OrgTypeId(),
            OrgType = r.OrgType(),
            Count = r.IsNull() ? 0 : r.Count()
          }).ToArray();
      }
    }

    public static void SaveOrganizationTypesData(OrganizationTypesData[] data)
    {
      var organizationTypesTable = GetAllData();

      // delete any missing OrganizationTypes rows  
      foreach (var otRow in organizationTypesTable)
        if (data.All(ot => otRow.OrgTypeId != ot.OrgTypeId))
        {
          var typeId = otRow.OrgTypeId;
          otRow.Delete();
          // we also delete any organization sub types or email tags with this org types
          // note: the application prevents deletion of an org type with any organizations assigned
          OrganizationSubTypes.DeleteByOrgTypeId(typeId);
          OrganizationEmailTags.DeleteByOrgTypeId(typeId);
        }

      // update or add remaining entries
      var otOrder = 0;
      foreach (var ot in data)
      {
        otOrder += 10;
        var otRow = organizationTypesTable.Where(r => r.RowState != DataRowState.Deleted)
          .FirstOrDefault(r => r.OrgTypeId == ot.OrgTypeId);
        if (otRow == null)
        {
          Insert(ot.OrgTypeId, ot.OrgType, otOrder);
        }
        else
        {
          if (otRow.OrgTypeOrder != otOrder)
            otRow.OrgTypeOrder = otOrder;
          if (otRow.OrgType != ot.OrgType)
            otRow.OrgType = ot.OrgType;
        }

      }

      UpdateTable(organizationTypesTable);
    }
  }
}