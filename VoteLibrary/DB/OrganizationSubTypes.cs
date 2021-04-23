using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  // ReSharper disable NotAccessedField.Global

  public sealed class OrganizationSubTypesData
  {
    public int OrgTypeId;
    public string OrgType;
    public OneSubType[] SubTypes;
  }

  public sealed class OneSubType
  {
    public int OrgSubTypeId;
    public string OrgSubType;
    public int Count;
  }

  public partial class OrganizationSubTypes
  {
    public static OrganizationSubTypesData[] GetOrganizationSubTypesData()
    {
      const string cmdText =
        "SELECT ot.OrgTypeId,ost.OrgSubTypeId,ost.OrgSubType,ot.OrgType,COUNT(*) AS Count,(o.OrgTypeId IS NULL) AS IsNull" +
        " FROM OrganizationTypes ot" +
        " LEFT OUTER JOIN OrganizationSubTypes ost ON ost.OrgTypeId = ot.OrgTypeId" +
        " LEFT OUTER JOIN Organizations o ON o.OrgTypeId=ost.OrgTypeId AND o.OrgSubTypeId=ost.OrgSubTypeId" +
        " GROUP BY ot.OrgTypeId,ost.OrgSubTypeId" +
        " ORDER BY ot.OrgTypeOrder,ost.OrgSubTypeOrder";

      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>().GroupBy(r => r.OrgTypeId()).Select(g =>
          new OrganizationSubTypesData
          {
            OrgTypeId = g.First().OrgTypeId(),
            OrgType = g.First().OrgType(),
            SubTypes = g.Where(r => r.OrgSubTypeId() != 0).Select(r => new OneSubType
            {
              OrgSubTypeId = r.OrgSubTypeId(),
              OrgSubType = r.OrgSubType(),
              Count = r.IsNull() ? 0 : r.Count()
            }).ToArray()
          }).ToArray();
      }
    }

    public static void SaveOrganizationSubTypesData(OrganizationSubTypesData[] data)
    {
      var organizationSubTypesTable = GetAllData();

      // delete any missing OrganizationTypes rows  
      foreach (var ostRow in organizationSubTypesTable)
        if (data.All(ot => ot.SubTypes.All(ost => ostRow.OrgSubTypeId != ost.OrgSubTypeId)))
          ostRow.Delete();

      // update or add remaining entries
      foreach (var ot in data)
      {
        var ostOrder = 0;
        foreach (var ost in ot.SubTypes)
        {
          ostOrder += 10;
          var ostRow = organizationSubTypesTable
            .Where(r => r.RowState != DataRowState.Deleted)
            .FirstOrDefault(r => r.OrgSubTypeId == ost.OrgSubTypeId);
          if (ostRow == null)
          {
            Insert(ost.OrgSubTypeId, ot.OrgTypeId, ost.OrgSubType, ostOrder);
          }
          else
          {
            if (ostRow.OrgSubTypeOrder != ostOrder)
              ostRow.OrgSubTypeOrder = ostOrder;
            if (ostRow.OrgSubType != ost.OrgSubType)
              ostRow.OrgSubType = ost.OrgSubType;
          }
        }
      }

      UpdateTable(organizationSubTypesTable);
    }
  }
}
