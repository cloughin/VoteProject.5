using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Vote
{
  public static class VoteSmart
  {
    public static ArrayList AsArrayList(object o)
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

    public static string GetJson(string methodName, string parameters)
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

    public static Dictionary<string, object> GetJsonAsDictionary(string methodName,
      string parameters)
    {
      return new JavaScriptSerializer()
        .Deserialize<Dictionary<string, object>>(GetJson(methodName, parameters));
    }
  }
}