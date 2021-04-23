using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.Vote
{
  // ReSharper disable NotAccessedField.Global

  public sealed class OrganizationNotesData
  {
    public int OrgTypeId;
    public string OrgType;
    public OneOrganizationNotes[] Organizations;
  }

  public sealed class OneOrganizationNotes
  {
    public int OrgId;
    public string Name;
    public OneNote[] Notes;
  }

  public sealed class OneNote
  {
    public int Id;
    public DateTime? DateStamp;
    public string Notes;
  }

  public partial class OrganizationNotes
  {
    public static OrganizationNotesData[] GetOrganizationNotesData()
    {
      const string cmdText =
        "SELECT ot.OrgTypeId,ot.OrgType,(o.OrgId IS NULL) AS IsNull,o.OrgId,o.OrgTypeId," +
        "o.Name FROM OrganizationTypes ot" +
        " LEFT OUTER JOIN Organizations o ON o.OrgTypeId = ot.OrgTypeId" +
        " ORDER BY ot.OrgTypeOrder,o.Name";

      OrganizationNotesData[] result;
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        result = table.Rows.OfType<DataRow>().GroupBy(r => r.OrgTypeId()).Select(g =>
          new OrganizationNotesData
          {
            OrgTypeId = g.First().OrgTypeId(),
            OrgType = g.First().OrgType(),
            Organizations = g.Where(r => r.OrgId() != 0).Select(r => new OneOrganizationNotes
            {
              OrgId = r.OrgId(),
              Name = r.Name()
            }).ToArray()
          }).ToArray();
      }

      // attach notes
      var notesDictionary = GetAllData().GroupBy(r => r.OrgId)
        .ToDictionary(g => g.Key, g => g);
      foreach (var orgType in result)
        foreach (var org in orgType.Organizations)
          if (notesDictionary.ContainsKey(org.OrgId))
            org.Notes = notesDictionary[org.OrgId].OrderByDescending(n => n.DateStamp)
              .Select(n => new OneNote
              {
                Id = n.Id,
                DateStamp = n.DateStamp.AsUtc(),
                Notes = HttpUtility.HtmlEncode(n.Notes).ReplaceNewLinesWithBreakTags()
              })
              .ToArray();
          else
            org.Notes = new OneNote[0];

      return result;
    }
  }
}
