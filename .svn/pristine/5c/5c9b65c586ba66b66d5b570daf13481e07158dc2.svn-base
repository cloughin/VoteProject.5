using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Utility
{
  public static class WebUtility
  {
    public static string GetWebPage(
      string uri, // http:// etc including query string
      string postInfo, // formatted like a query string (null if none)
      ICredentials credentials, int timeout // milliseconds
      )
    {
      #region Example of use

      //string Url = db.Http() + db.SERVER_NAME()
      //  + "/IssuePage.aspx"
      //  + "?Issue=" + IssueKey
      //  + "&Office=" + OfficeKey
      //  + "&Election=" + ElectionKey;

      //string thePage = WebUtility.GetWebPage(Url, null, null, 10000); //10 seconds

      //if (thePage != null)
      //{
      //  LabelThePage.Text = thePage;
      //}
      //else
      //{
      //  LabelThePage.Text = "Sorry! Our servers are currently overloaded. Please try again later.";
      //}

      #endregion

      string result = null;
      try
      {
        var req = WebRequest.Create(uri) as HttpWebRequest;
        if (req != null)
        {
          req.ReadWriteTimeout = timeout;
          req.Timeout = timeout;
          if (credentials != null)
          {
            req.Credentials = credentials;
            req.UnsafeAuthenticatedConnectionSharing = true; // safe here
          }
          if (postInfo != null)
            CopyPostInfo(req, postInfo);

          using (var resp = req.GetResponse() as HttpWebResponse)
          {
            Debug.Assert(resp != null, "resp != null");
            var encoding = Encoding.UTF8;
            if (resp.ContentEncoding != string.Empty)
              encoding = Encoding.GetEncoding(resp.ContentEncoding);

            var stream = resp.GetResponseStream();
            //Need to wrap in :using (Impersonate imp = new Impersonate(Environment.MachineName, "Administrator", "v0+3u$@"))
            Debug.Assert(stream != null, "stream != null");
            var reader = new StreamReader(stream, encoding);
            result = reader.ReadToEnd();
          }
        }
      }
      catch /*(Exception ex)*/
      {
        //Need to do
        //db.LogError408RequestTimeout(ex, uri);
      }
      return result;
    }

    private static void CopyPostInfo(WebRequest request, string postInfo)
    {
      request.ContentType = "application/x-www-form-urlencoded";
      request.Method = "POST";
      using (
        var writer = new StreamWriter(request.GetRequestStream(), Encoding.Default))
        writer.Write(postInfo);
    }
  }
}