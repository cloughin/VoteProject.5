using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using MySql.Data.MySqlClient;

namespace VoteSmart
{
  //public class OfficeType
  //{
  //  // ReSharper disable NotAccessedField.Global
  //  public string OfficeTypeId;
  //  public string OfficeLevelId;
  //  public string OfficeBranchId;
  //  public string Name;
  //  // ReSharper restore NotAccessedField.Global
  //}

  public partial class MainForm
  {
    //public Dictionary<string, OfficeType> OfficeTypeDictionary =
    //  new Dictionary<string, OfficeType>
    //  {
    //    {
    //      "P",
    //      new OfficeType
    //      {
    //        OfficeTypeId = "P",
    //        OfficeLevelId = "F",
    //        OfficeBranchId = "E",
    //        Name = "Presidential and Cabinet"
    //      }
    //    },
    //    {
    //      "C",
    //      new OfficeType
    //      {
    //        OfficeTypeId = "C",
    //        OfficeLevelId = "F",
    //        OfficeBranchId = "L",
    //        Name = "Congressional"
    //      }
    //    },
    //    {
    //      "G",
    //      new OfficeType
    //      {
    //        OfficeTypeId = "G",
    //        OfficeLevelId = "S",
    //        OfficeBranchId = "E",
    //        Name = "Governor and Cabinet"
    //      }
    //    },
    //    {
    //      "S",
    //      new OfficeType
    //      {
    //        OfficeTypeId = "S",
    //        OfficeLevelId = "S",
    //        OfficeBranchId = "E",
    //        Name = "Statewide"
    //      }
    //    },
    //    {
    //      "K",
    //      new OfficeType
    //      {
    //        OfficeTypeId = "K",
    //        OfficeLevelId = "S",
    //        OfficeBranchId = "J",
    //        Name = "State Judicial"
    //      }
    //    },
    //    {
    //      "L",
    //      new OfficeType
    //      {
    //        OfficeTypeId = "L",
    //        OfficeLevelId = "S",
    //        OfficeBranchId = "L",
    //        Name = "State Legislature"
    //      }
    //    },
    //    {
    //      "J",
    //      new OfficeType
    //      {
    //        OfficeTypeId = "J",
    //        OfficeLevelId = "F",
    //        OfficeBranchId = "J",
    //        Name = "Supreme Court"
    //      }
    //    },
    //    {
    //      "M",
    //      new OfficeType
    //      {
    //        OfficeTypeId = "M",
    //        OfficeLevelId = "L",
    //        OfficeBranchId = "E",
    //        Name = "Local Executive"
    //      }
    //    },
    //    {
    //      "N",
    //      new OfficeType
    //      {
    //        OfficeTypeId = "N",
    //        OfficeLevelId = "L",
    //        OfficeBranchId = "L",
    //        Name = "Local Legislative"
    //      }
    //    },
    //    {
    //      "H",
    //      new OfficeType
    //      {
    //        OfficeTypeId = "H",
    //        OfficeLevelId = "L",
    //        OfficeBranchId = "J",
    //        Name = "Local Judicial"
    //      }
    //    }
    //  };

    private static ArrayList AsArrayList(object o)
    {
      var result = o as ArrayList;
      if (result == null)
      {
        var dictionary = o as Dictionary<string, object>;
        if (dictionary != null)
          result = new ArrayList(new[] { dictionary });
      }
      return result;
    }

    private static string GetApiUrl(string methodName, string parameters)
    {
      var url =
        $"http://api.votesmart.org/{methodName}?key=2fdca40538f9ea5d0a9f7663bfb2248f&o=JSON&{parameters}";
      return url;
    }

    private static Dictionary<string, object> GetDataAsJson(DataRow row)
    {
      var serializer = new JavaScriptSerializer();
      // ReSharper disable once AssignNullToNotNullAttribute
      var jsonObj =
        serializer.Deserialize<Dictionary<string, object>>(row["fetch_data"] as string);
      return jsonObj;
    }

    private string GetNormalizedParameters(bool forQuery = false)
    {
      var parameters = string.Join("&", ParametersTextBox.Text.Trim()
        .Split('&')
        .OrderBy(p => p, StringComparer.OrdinalIgnoreCase));
      if (forQuery)
      {
        // add % at beginning and end, and replace & with % for LIKE
        parameters = "%" + parameters.Replace('&', '%') + "%";
      }
      return parameters;
    }

    private static DataTable GetRawFetches(string methodName, string parameters)
    {
      const string selectCmdText =
        "SELECT id,fetch_time,fetch_method,fetch_parameters," +
          "fetch_type,fetch_data FROM fetches_raw " +
          " WHERE fetch_method=@method AND fetch_parameters LIKE @parameters" +
          "  AND fetch_type='JSON'";
      var selectCmd = new MySqlCommand(selectCmdText);
      selectCmd.Parameters.AddWithValue("@method", methodName);
      selectCmd.Parameters.AddWithValue("@parameters", parameters);
      var table = new DataTable();
      using (var cn = GetOpenConnection())
      {
        selectCmd.Connection = cn;
        var adapter = new MySqlDataAdapter(selectCmd);
        adapter.Fill(table);
      }
      return table;
    }

    private static string GetVoteSmartJson(string methodName, string parameters)
    {
      string json;
      var url = GetApiUrl(methodName, parameters);
      var webRequest = WebRequest.Create(url);
      using (var webResponse = webRequest.GetResponse() as HttpWebResponse)
      {
        Debug.Assert(webResponse != null, "resp != null");
        var encoding = Encoding.UTF8;
        if (webResponse.ContentEncoding != string.Empty) encoding = Encoding.GetEncoding(webResponse.ContentEncoding);

        var stream = webResponse.GetResponseStream();
        Debug.Assert(stream != null, "stream != null");
        var reader = new StreamReader(stream, encoding);
        json = reader.ReadToEnd();
      }
      return json;
    }

    private static string ParseIdFromParameters(object parameters, string idName)
    {
      var id =
        Regex.Match(parameters.ToString(),
          @"(?:^|&)" + idName + @"=(?<id>[a-z0-9]+)", RegexOptions.IgnoreCase)
          .Groups["id"].Value;
      return id;
    }
  }
}