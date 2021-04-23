using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  // ReSharper disable NotAccessedField.Global

  public sealed class OrganizationsData
  {
    public OneOrgType[] OrgTypes;
    public OneIdeology[] Ideologies;
  }

  public sealed class OneOrgType
  {
    public int OrgTypeId;
    public string OrgType;
    public OneOrganization[] Organizations;
    public OneOrgTypeSubType[] SubTypes;
    public OneOrgTypeEmailTag[] EmailTags;
  }

  public sealed class OneOrganization
  {
    public int OrgId;
    public int OrgTypeId;
    public int OrgSubTypeId;
    public int IdeologyId;
    public string Name;
    public string OrgAbbreviation;
    public string Address1;
    public string Address2;
    public string City;
    public string StateCode;
    public string Zip;
    public string Url;
    public string LongMission;
    public string ShortMission;
    public string EmailMission;
    public int[] EmailTagIds;
    public OneOrganizationContact[] Contacts;
    public OneOrganizationMissionUrl[] MissionUrls;
    public DateTime? DateStamp;
  }

  public sealed class OneIdeology
  {
    public int IdeologyId;
    public string Ideology;
  }

  public sealed class OneOrgTypeSubType
  {
    public int OrgSubTypeId;
    public string OrgSubType;
  }

  public sealed class OneOrgTypeEmailTag
  {
    public int EmailTagId;
    public string EmailTag;
  }

  public sealed class OneOrganizationContact
  {
    public int ContactId;
    public string Contact;
    public string Email;
    public string Phone;
    public string Title;
  }

  public sealed class OneOrganizationMissionUrl
  {
    public int OrgMissionUrlId;
    public string Url;
  }

  public sealed class OrganizationImageData
  {
    public int OrgTypeId;
    public string OrgType;
    public OneLogoOrganization[] Organizations;
  }

  public sealed class OneLogoOrganization
  {
    public int OrgId;
    public string Name;
  }

  public sealed class OrganizationsSelectReportData
  {
    public OneReportOrgType[] OrgTypes;
    public OneIdeology[] Ideologies;
  }

  public sealed class OneReportOrgType
  {
    public int OrgTypeId;
    public string OrgType;
    public OneOrgTypeSubType[] SubTypes;
    public OneOrgTypeEmailTag[] EmailTags;
  }

  public sealed class OrganizationReportItem
  {
    public int OrgId;
    public string Name;
    public string OrgAbbreviation;
    public string StateCode;
    public string Url;
    public DateTime DateStamp;
  }

  public sealed class OrganizationAdData
  {
    public string AdImageName;
    public string AdUrl;
    public string OrgUrl;
    public string Sample;
  }

  public partial class Organizations
  {
    public static OrganizationsData GetOrganizationsData()
    {
      const string cmdText =
        "SELECT ot.OrgTypeId,ot.OrgType,(o.OrgId IS NULL) AS IsNull,o.OrgId,o.OrgTypeId," +
        "o.OrgSubTypeId,o.IdeologyId,o.DateStamp,o.Name,o.OrgAbbreviation,o.Address1,o.Address2,o.City," +
        "o.StateCode,o.Zip,o.Url,o.LongMission,o.ShortMission,o.EmailMission" +
        " FROM OrganizationTypes ot" +
        " LEFT OUTER JOIN Organizations o ON o.OrgTypeId = ot.OrgTypeId" +
        " ORDER BY ot.OrgTypeOrder,o.Name";

      var result = new OrganizationsData();
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        result.OrgTypes = table.Rows.OfType<DataRow>().GroupBy(r => r.OrgTypeId()).Select(
          g => new OneOrgType
          {
            OrgTypeId = g.First().OrgTypeId(),
            OrgType = g.First().OrgType(),
            Organizations = g.Where(r => r.OrgId() != 0).Select(r => new OneOrganization
            {
              OrgId = r.OrgId(),
              OrgTypeId = g.First().OrgTypeId(),
              OrgSubTypeId = r.OrgSubTypeId(),
              IdeologyId = r.IdeologyId(),
              DateStamp = r.DateStamp().AsUtc(),
              Name = r.Name(),
              OrgAbbreviation = r.OrgAbbreviation(),
              Address1 = r.Address1(),
              Address2 = r.Address2(),
              City = r.City(),
              StateCode = r.StateCode(),
              Zip = r.Zip(),
              Url = r.Url(),
              LongMission = r.LongMission(),
              ShortMission = r.ShortMission(),
              EmailMission = r.EmailMission()
            }).ToArray()
          }).ToArray();
      }

      // attach sub types
      var subTypeDictionary = OrganizationSubTypes.GetAllData().GroupBy(r => r.OrgTypeId)
        .ToDictionary(g => g.Key, g => g);
      foreach (var orgType in result.OrgTypes)
        if (subTypeDictionary.ContainsKey(orgType.OrgTypeId))
          orgType.SubTypes = subTypeDictionary[orgType.OrgTypeId]
            .OrderBy(t => t.OrgSubTypeOrder).Select(t =>
              new OneOrgTypeSubType
              {
                OrgSubTypeId = t.OrgSubTypeId, OrgSubType = t.OrgSubType
              }).ToArray();
        else
          orgType.SubTypes = new OneOrgTypeSubType[0];

      // attach ideologies
      result.Ideologies = OrganizationIdeologies.GetAllData().OrderBy(r => r.IdeologyOrder)
        .Select(r => new OneIdeology {IdeologyId = r.IdeologyId, Ideology = r.Ideology})
        .ToArray();

      // attach email tags
      var tagDictionary = OrganizationEmailTags.GetAllData().GroupBy(r => r.OrgTypeId)
        .ToDictionary(g => g.Key, g => g);
      foreach (var orgType in result.OrgTypes)
        if (tagDictionary.ContainsKey(orgType.OrgTypeId))
          orgType.EmailTags = tagDictionary[orgType.OrgTypeId].OrderBy(t => t.EmailTagOrder)
            .Select(t =>
              new OneOrgTypeEmailTag {EmailTagId = t.EmailTagId, EmailTag = t.EmailTag})
            .ToArray();
        else
          orgType.EmailTags = new OneOrgTypeEmailTag[0];

      // attach assigned email tags
      var assignedTagDictionary = OrganizationAssignedEmailTags.GetAllData()
        .GroupBy(r => r.OrgId).ToDictionary(g => g.Key, g => g);
      foreach (var orgType in result.OrgTypes)
      foreach (var org in orgType.Organizations)
        if (assignedTagDictionary.ContainsKey(org.OrgId))
          org.EmailTagIds = assignedTagDictionary[org.OrgId].Select(t => t.EmailTagId)
            .OrderBy(id => id).ToArray();
        else
          org.EmailTagIds = new int[0];

      // attach contacts
      var contactsDictionary = OrganizationContacts.GetAllData().GroupBy(r => r.OrgId)
        .ToDictionary(g => g.Key, g => g);
      foreach (var orgType in result.OrgTypes)
      foreach (var org in orgType.Organizations)
        if (contactsDictionary.ContainsKey(org.OrgId))
          org.Contacts = contactsDictionary[org.OrgId].OrderBy(c => c.ContactOrder).Select(
            c => new OneOrganizationContact
            {
              ContactId = c.ContactId,
              Contact = c.Contact,
              Email = c.Email,
              Phone = c.Phone,
              Title = c.Title
            }).ToArray();
        else
          org.Contacts = new OneOrganizationContact[0];

      // attach mission urls
      var missionUrlsDictionary = OrganizationMissionUrls.GetAllData().GroupBy(r => r.OrgId)
        .ToDictionary(g => g.Key, g => g);
      foreach (var orgType in result.OrgTypes)
      foreach (var org in orgType.Organizations)
        if (missionUrlsDictionary.ContainsKey(org.OrgId))
          org.MissionUrls = missionUrlsDictionary[org.OrgId].OrderBy(c => c.UrlOrder)
            .Select(c => new OneOrganizationMissionUrl
            {
              OrgMissionUrlId = c.OrgMissionUrlId, Url = c.Url
            }).ToArray();
        else
          org.MissionUrls = new OneOrganizationMissionUrl[0];

      return result;
    }

    public static OrganizationsData GetOrganizationsStructuralData()
    {
      // like above but does not return data for specific organizations
      const string cmdText =
        "SELECT ot.OrgTypeId,ot.OrgType FROM OrganizationTypes ot" +
        " ORDER BY ot.OrgTypeOrder";

      var result = new OrganizationsData();
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        result.OrgTypes = table.Rows.OfType<DataRow>().GroupBy(r => r.OrgTypeId()).Select(
          g => new OneOrgType
          {
            OrgTypeId = g.First().OrgTypeId(),
            OrgType = g.First().OrgType()
          }).ToArray();
      }

      // attach sub types
      var subTypeDictionary = OrganizationSubTypes.GetAllData().GroupBy(r => r.OrgTypeId)
        .ToDictionary(g => g.Key, g => g);
      foreach (var orgType in result.OrgTypes)
        if (subTypeDictionary.ContainsKey(orgType.OrgTypeId))
          orgType.SubTypes = subTypeDictionary[orgType.OrgTypeId]
            .OrderBy(t => t.OrgSubTypeOrder).Select(t =>
              new OneOrgTypeSubType
              {
                OrgSubTypeId = t.OrgSubTypeId,
                OrgSubType = t.OrgSubType
              }).ToArray();
        else
          orgType.SubTypes = new OneOrgTypeSubType[0];

      // attach ideologies
      result.Ideologies = OrganizationIdeologies.GetAllData().OrderBy(r => r.IdeologyOrder)
        .Select(r => new OneIdeology { IdeologyId = r.IdeologyId, Ideology = r.Ideology })
        .ToArray();

      // attach email tags
      var tagDictionary = OrganizationEmailTags.GetAllData().GroupBy(r => r.OrgTypeId)
        .ToDictionary(g => g.Key, g => g);
      foreach (var orgType in result.OrgTypes)
        if (tagDictionary.ContainsKey(orgType.OrgTypeId))
          orgType.EmailTags = tagDictionary[orgType.OrgTypeId].OrderBy(t => t.EmailTagOrder)
            .Select(t =>
              new OneOrgTypeEmailTag { EmailTagId = t.EmailTagId, EmailTag = t.EmailTag })
            .ToArray();
        else
          orgType.EmailTags = new OneOrgTypeEmailTag[0];

      return result;
    }

    public static void SaveOrganizationsData(OneOrgType[] data)
    {
      var organizationsTable = GetAllData();
      var contactsTable = OrganizationContacts.GetAllData();
      var missionUrlsTable = OrganizationMissionUrls.GetAllData();
      var emailTagsTable = OrganizationAssignedEmailTags.GetAllData();

      bool UpdateContacts(int orgId, OneOrganizationContact[] items,
        OrganizationContactsTable table)
      {
        var changed = false;

        // delete any missing contacts
        foreach (var row in table.Where(r => r.RowState != DataRowState.Deleted && r.OrgId == orgId))
          if (items.All(i => row.ContactId != i.ContactId))
          {
            row.Delete();
            changed = true;
          }

        // update or add remaining entries
        var order = 0;
        foreach (var i in items)
        {
          order += 10;
          var row = table.Where(r => r.RowState != DataRowState.Deleted)
            .FirstOrDefault(r => r.OrgId == orgId && r.ContactId == i.ContactId);
          if (row == null)
          {
            OrganizationContacts.Insert(i.ContactId, orgId, i.Contact, i.Email, i.Phone,
              i.Title, order);
            changed = true;
          }
          else
          {
            if (row.Contact != i.Contact)
            {
              row.Contact = i.Contact;
              changed = true;
            }

            if (row.Email != i.Email)
            {
              row.Email = i.Email;
              changed = true;
            }

            if (row.Phone != i.Phone)
            {
              row.Phone = i.Phone;
              changed = true;
            }

            if (row.Title != i.Title)
            {
              row.Title = i.Title;
              changed = true;
            }

            if (row.ContactOrder != order)
            {
              row.ContactOrder = order;
              changed = true;
            }
          }
        }

        return changed;
      }

      bool UpdateMissionUrls(int orgId, OneOrganizationMissionUrl[] items,
        OrganizationMissionUrlsTable table)
      {
        var changed = false;

        // delete any missing urls
        foreach (var row in table.Where(r => r.RowState != DataRowState.Deleted && r.OrgId == orgId))
          if (items.All(i => row.OrgMissionUrlId != i.OrgMissionUrlId))
          {
            row.Delete();
            changed = true;
          }

        // update or add remaining entries
        var order = 0;
        foreach (var i in items)
        {
          order += 10;
          var row = table.Where(r => r.RowState != DataRowState.Deleted).FirstOrDefault(r =>
            r.OrgId == orgId && r.OrgMissionUrlId == i.OrgMissionUrlId);
          if (row == null)
          {
            OrganizationMissionUrls.Insert(i.OrgMissionUrlId, orgId, i.Url, order);
            changed = true;
          }
          else
          {
            if (row.Url != i.Url)
            {
              row.Url = i.Url;
              changed = true;
            }

            if (row.UrlOrder != order)
            {
              row.UrlOrder = order;
              changed = true;
            }
          }
        }

        return changed;
      }

      bool UpdateEmailTags(int orgId, int[] items, OrganizationAssignedEmailTagsTable table)
      {
        var changed = false;

        // delete any missing tags
        foreach (var row in table.Where(r => r.RowState != DataRowState.Deleted && r.OrgId == orgId))
          if (items.All(i => row.EmailTagId != i))
          {
            row.Delete();
            changed = true;
          }

        // add remaining entries
        foreach (var i in items)
        {
          var row = table.Where(r => r.RowState != DataRowState.Deleted)
            .FirstOrDefault(r => r.OrgId == orgId && r.EmailTagId == i);
          if (row == null)
          {
            OrganizationAssignedEmailTags.Insert(orgId, i);
            changed = true;
          }
        }

        return changed;
      }

      // delete any missing Organizations rows and related contacts, mission urls
      // and assigned email tags
      foreach (var orgRow in organizationsTable)
        if (data.All(ot => ot.Organizations.All(o => orgRow.OrgId != o.OrgId)))
        {
          foreach (var row in contactsTable)
            if (row.RowState != DataRowState.Deleted && row.OrgId == orgRow.OrgId)
              row.Delete();
          foreach (var row in missionUrlsTable)
            if (row.RowState != DataRowState.Deleted && row.OrgId == orgRow.OrgId)
              row.Delete();
          foreach (var row in emailTagsTable)
            if (row.RowState != DataRowState.Deleted && row.OrgId == orgRow.OrgId)
              row.Delete();
          orgRow.Delete();
        }

      // update or add remaining entries
      foreach (var ot in data)
      {
        foreach (var o in ot.Organizations)
        {
          var orgRow = organizationsTable.Where(r => r.RowState != DataRowState.Deleted)
            .FirstOrDefault(r => r.OrgId == o.OrgId);
          if (orgRow == null)
          {
            Insert(o.OrgId, ot.OrgTypeId, o.OrgSubTypeId, o.IdeologyId, DateTime.UtcNow,
              o.Name, o.OrgAbbreviation, o.Address1, o.Address2, o.City, o.StateCode, o.Zip,
              o.Url, o.LongMission, o.ShortMission, o.EmailMission, null, Empty,
              Empty, null, Empty);
          }
          else
          {
            var changed = false;
            if (orgRow.OrgSubTypeId != o.OrgSubTypeId)
            {
              orgRow.OrgSubTypeId = o.OrgSubTypeId;
              changed = true;
            }

            if (orgRow.IdeologyId != o.IdeologyId)
            {
              orgRow.IdeologyId = o.IdeologyId;
              changed = true;
            }

            if (orgRow.Name != o.Name)
            {
              orgRow.Name = o.Name;
              changed = true;
            }

            if (orgRow.OrgAbbreviation != o.OrgAbbreviation)
            {
              orgRow.OrgAbbreviation = o.OrgAbbreviation;
              changed = true;
            }

            if (orgRow.Address1 != o.Address1)
            {
              orgRow.Address1 = o.Address1;
              changed = true;
            }

            if (orgRow.Address2 != o.Address2)
            {
              orgRow.Address2 = o.Address2;
              changed = true;
            }

            if (orgRow.City != o.City)
            {
              orgRow.City = o.City;
              changed = true;
            }

            if (orgRow.StateCode != o.StateCode)
            {
              orgRow.StateCode = o.StateCode;
              changed = true;
            }

            if (orgRow.Zip != o.Zip)
            {
              orgRow.Zip = o.Zip;
              changed = true;
            }

            if (orgRow.Url != o.Url)
            {
              orgRow.Url = o.Url;
              changed = true;
            }

            if (orgRow.LongMission != o.LongMission)
            {
              orgRow.LongMission = o.LongMission;
              changed = true;
            }

            if (orgRow.ShortMission != o.ShortMission)
            {
              orgRow.ShortMission = o.ShortMission;
              changed = true;
            }

            if (orgRow.EmailMission != o.EmailMission)
            {
              orgRow.EmailMission = o.EmailMission;
              changed = true;
            }

            changed |= UpdateContacts(o.OrgId, o.Contacts, contactsTable);
            changed |= UpdateMissionUrls(o.OrgId, o.MissionUrls, missionUrlsTable);
            changed |= UpdateEmailTags(o.OrgId, o.EmailTagIds, emailTagsTable);

            if (changed)
              orgRow.DateStamp = DateTime.UtcNow;
          }
        }
      }

      UpdateTable(organizationsTable);
      OrganizationContacts.UpdateTable(contactsTable);
      OrganizationMissionUrls.UpdateTable(missionUrlsTable);
      OrganizationAssignedEmailTags.UpdateTable(emailTagsTable);
    }

    public static DataTable GetOrganizationsForCsv()
    {
      const string cmdText = "SELECT o.OrgId,o.Name,o.OrgAbbreviation,t.OrgType," +
        "COALESCE(s.OrgSubType,'') AS OrgSubType,COALESCE(i.Ideology,'') AS Ideology," +
        "o.StateCode,o.Url,o.Address1,o.Address2,o.City,o.Zip,c.Contact,c.Email,c.Phone,c.Title" +
        " FROM Organizations o" +
        " INNER JOIN OrganizationTypes t ON t.OrgTypeId = o.OrgTypeId" +
        " LEFT OUTER JOIN OrganizationSubTypes s on s.OrgSubTypeId = o.OrgSubTypeId" +
        " LEFT OUTER JOIN OrganizationIdeologies i on i.IdeologyId = o.IdeologyId" +
        " LEFT OUTER JOIN OrganizationContacts c on c.OrgId = o.OrgId" +
        " ORDER BY t.OrgType,o.Name,o.OrgId,c.ContactOrder";
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static OrganizationImageData[] GetOrganizationImageData()
    {
      const string cmdText =
        "SELECT ot.OrgTypeId,ot.OrgType,(o.OrgId IS NULL) AS IsNull,o.OrgId,o.OrgTypeId," +
        "o.Name FROM OrganizationTypes ot" +
        " LEFT OUTER JOIN Organizations o ON o.OrgTypeId = ot.OrgTypeId" +
        " ORDER BY ot.OrgTypeOrder,o.Name";

      OrganizationImageData[] result;
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        result = table.Rows.OfType<DataRow>().GroupBy(r => r.OrgTypeId()).Select(g =>
          new OrganizationImageData
          {
            OrgTypeId = g.First().OrgTypeId(),
            OrgType = g.First().OrgType(),
            Organizations = g.Where(r => r.OrgId() != 0).Select(r =>
              new OneLogoOrganization {OrgId = r.OrgId(), Name = r.Name()}).ToArray()
          }).ToArray();
      }

      return result;
    }

    public static OrganizationsSelectReportData GetOrganizationsSelectReportData()
    {
      const string cmdText = "SELECT ot.OrgTypeId,ot.OrgType FROM OrganizationTypes ot" +
        " ORDER BY ot.OrgTypeOrder";

      var result = new OrganizationsSelectReportData();
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        result.OrgTypes = table.Rows.OfType<DataRow>().GroupBy(r => r.OrgTypeId())
          .Select(g => new OneReportOrgType
          {
            OrgTypeId = g.First().OrgTypeId(), OrgType = g.First().OrgType()
          }).ToArray();
      }

      // attach sub types
      var subTypeDictionary = OrganizationSubTypes.GetAllData().GroupBy(r => r.OrgTypeId)
        .ToDictionary(g => g.Key, g => g);
      foreach (var orgType in result.OrgTypes)
        if (subTypeDictionary.ContainsKey(orgType.OrgTypeId))
          orgType.SubTypes = subTypeDictionary[orgType.OrgTypeId]
            .OrderBy(t => t.OrgSubTypeOrder).Select(t =>
              new OneOrgTypeSubType
              {
                OrgSubTypeId = t.OrgSubTypeId, OrgSubType = t.OrgSubType
              }).ToArray();
        else
          orgType.SubTypes = new OneOrgTypeSubType[0];

      // attach ideologies
      result.Ideologies = OrganizationIdeologies.GetAllData().OrderBy(r => r.IdeologyOrder)
        .Select(r => new OneIdeology {IdeologyId = r.IdeologyId, Ideology = r.Ideology})
        .ToArray();

      // attach email tags
      var tagDictionary = OrganizationEmailTags.GetAllData().GroupBy(r => r.OrgTypeId)
        .ToDictionary(g => g.Key, g => g);
      foreach (var orgType in result.OrgTypes)
        if (tagDictionary.ContainsKey(orgType.OrgTypeId))
          orgType.EmailTags = tagDictionary[orgType.OrgTypeId].OrderBy(t => t.EmailTagOrder)
            .Select(t =>
              new OneOrgTypeEmailTag {EmailTagId = t.EmailTagId, EmailTag = t.EmailTag})
            .ToArray();
        else
          orgType.EmailTags = new OneOrgTypeEmailTag[0];

      return result;
    }

    public static OrganizationReportItem[] GetOrganizationsReportData(int orgTypeId,
      int subTypeId, int ideologyId, string stateCode, int[] tagIds, string sortItem, string sortDir)
    {
      var wheres = new List<string>();
      var tagJoin = Empty;
      wheres.Add($"o.OrgTypeId={orgTypeId}");
      if (subTypeId != 0)
        wheres.Add($"o.OrgSubTypeId={subTypeId}");
      if (ideologyId != 0)
        wheres.Add($"o.IdeologyId={ideologyId}");
      if (stateCode != Empty)
        wheres.Add($"o.stateCode='{stateCode}'");
      var whereClause = Join(" AND ", wheres);
      if (tagIds.Length != 0 )
        tagJoin = $" INNER JOIN OrganizationAssignedEmailTags t ON t.OrgId=o.OrgId AND {tagIds.SqlIn("EmailTagId")}";

      var orderBy = Empty;
      switch (sortItem)
      {
        case "Name":
          orderBy = $"ORDER BY o.Name {sortDir}";
          break;

        case "StateCode":
          orderBy = $"ORDER BY o.StateCode {sortDir}, o.Name {sortDir}";
          break;

        case "DateStamp":
          orderBy = $"ORDER BY o.DateStamp {sortDir}, o.Name {sortDir}";
          break;
      }

      var cmdText =
        "SELECT o.OrgId,o.Name,o.OrgAbbreviation,o.StateCode,o.Url,o.DateStamp FROM Organizations o" +
        $" {tagJoin} WHERE {whereClause} GROUP BY o.OrgId {orderBy}";
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>().Select(r => new OrganizationReportItem
          {
            OrgId = r.OrgId(),
            Name = r.Name(),
            OrgAbbreviation = r.OrgAbbreviation(),
            StateCode = r.StateCode(),
            Url = VotePage.NormalizeUrl(r.Url()),
            DateStamp = r.DateStamp().Date
          })
          .ToArray();
      }

    }

    public static void UpdateAdInfo(int orgId, string adUrl, bool imageFileChanged,
      string adImageName, byte[] adImage)
    {
      if (imageFileChanged)
      {
        const string cmdText = "UPDATE Organizations SET AdUrl=@AdUrl,AdImageName=@AdImageName," +
          "AdImage=@AdImage WHERE OrgId=@OrgId";
        var cmd = VoteDb.GetCommand(cmdText, -1);
        VoteDb.AddCommandParameter(cmd, "OrgId", orgId);
        VoteDb.AddCommandParameter(cmd, "AdUrl", adUrl);
        VoteDb.AddCommandParameter(cmd, "AdImageName", adImageName);
        VoteDb.AddCommandParameter(cmd, "AdImage", adImage);
        VoteDb.ExecuteNonQuery(cmd);
      }
      else
      {
        const string cmdText = "UPDATE Organizations SET AdUrl=@AdUrl WHERE OrgId=@OrgId";
        var cmd = VoteDb.GetCommand(cmdText, -1);
        VoteDb.AddCommandParameter(cmd, "OrgId", orgId);
        VoteDb.AddCommandParameter(cmd, "AdUrl", adUrl);
        VoteDb.ExecuteNonQuery(cmd);
      }
    }
  }
}
