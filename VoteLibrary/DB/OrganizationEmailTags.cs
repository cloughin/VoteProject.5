using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  // ReSharper disable NotAccessedField.Global

  public sealed class OrganizationEmailTagsData
  {
    public int OrgTypeId;
    public string OrgType;
    public OneEmailTag[] EmailTags;
  }

  public sealed class OneEmailTag
  {
    public int EmailTagId;
    public string EmailTag;
    public int Count;
  }

  public partial class OrganizationEmailTags
  {
    public static OrganizationEmailTagsData[] GetOrganizationEmailTagsData()
    {
      const string cmdText =
        "SELECT ot.OrgTypeId,et.EmailTagId,et.EmailTag,ot.OrgType,COUNT(*) AS Count,(aet.OrgId IS NULL) AS IsNull" +
        " FROM OrganizationTypes ot" +
        " LEFT OUTER JOIN OrganizationEmailTags et ON et.OrgTypeId = ot.OrgTypeId" +
        " LEFT OUTER JOIN OrganizationAssignedEmailTags aet ON aet.EmailTagId=et.EmailTagId" +
        " GROUP BY ot.OrgTypeId,et.EmailTagId" +
        " ORDER BY ot.OrgTypeOrder,et.EmailTagOrder";

      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>().GroupBy(r => r.OrgTypeId()).Select(g =>
          new OrganizationEmailTagsData
          {
            OrgTypeId = g.First().OrgTypeId(),
            OrgType = g.First().OrgType(),
            EmailTags = g.Where(r => r.EmailTagId() != 0).Select(r => new OneEmailTag
            {
              EmailTagId = r.EmailTagId(),
              EmailTag = r.EmailTag(),
              Count = r.IsNull() ? 0 : r.Count()
            }).ToArray()
          }).ToArray();
      }
    }

    public static void SaveOrganizationEmailTagsData(OrganizationEmailTagsData[] data)
    {
      var organizationEmailTagsTable = GetAllData();

      // delete any missing OrganizationTypes rows  
      foreach (var etRow in organizationEmailTagsTable)
        if (data.All(ot => ot.EmailTags.All(et => etRow.EmailTagId != et.EmailTagId)))
          etRow.Delete();

      // update or add remaining entries
      foreach (var ot in data)
      {
        var etOrder = 0;
        foreach (var et in ot.EmailTags)
        {
          etOrder += 10;
          var etRow = organizationEmailTagsTable.Where(r => r.RowState != DataRowState.Deleted)
            .FirstOrDefault(r => r.EmailTagId == et.EmailTagId);
          if (etRow == null)
          {
            Insert(et.EmailTagId, ot.OrgTypeId, et.EmailTag, etOrder);
          }
          else
          {
            if (etRow.EmailTagOrder != etOrder)
              etRow.EmailTagOrder = etOrder;
            if (etRow.EmailTag != et.EmailTag)
              etRow.EmailTag = et.EmailTag;
          }
        }

        }

      UpdateTable(organizationEmailTagsTable);
    }
  }
}
