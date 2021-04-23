using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using DB.Vote;

namespace TestRedirect
{
  public partial class TestRedirectForm : Form
  {
    Uri UriFrom;
    Uri UriTo;
    int Count;

    public TestRedirectForm()
    {
      InitializeComponent();
    }

    private void ReportError(string message)
    {
      StatusTextBox.AppendText("Row: " + Count.ToString());
      StatusTextBox.AppendText(Environment.NewLine);
      StatusTextBox.AppendText("From: " + UriFrom.ToString());
      StatusTextBox.AppendText(Environment.NewLine);
      StatusTextBox.AppendText("To: " + UriTo.ToString());
      StatusTextBox.AppendText(Environment.NewLine);
      StatusTextBox.AppendText("New: " + message);
      StatusTextBox.AppendText(Environment.NewLine);
      StatusTextBox.AppendText(Environment.NewLine);
    }

    private void ReportError(Uri uriNew)
    {
      ReportError(uriNew.ToString());
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      var table = DB.VoteLog.Log301Redirect.GetAllData();
      var urlNormalizer = new UrlNormalizer();
      foreach (var row in table)
      {
        //if (Count >= 1000) return;
        Count++;
        try
        {
          UriFrom = new Uri(row.PageFrom);
          UriTo = new Uri(row.PageTo);
          if (urlNormalizer.Normalize(UriFrom))
          {
            if (urlNormalizer.NormalizedUri == null) //ok
            {
              bool ok = false;

              bool hostFromIsCanonical = 
                UrlManager.IsCanonicalHostName(UriFrom.Host);
              string hostStateFrom = 
                UrlManager.GetStateCodeFromHostName(UriFrom.Host);
              string pathFromLower = 
                UriFrom.AbsolutePath.ToLowerInvariant();
              QueryStringCollection qscFrom = 
                QueryStringCollection.Parse(UriFrom.Query);

              // ignore if UriFrom == UriTo
              if (Uri.Compare(UriFrom, UriTo,
                UriComponents.AbsoluteUri, UriFormat.SafeUnescaped,
                StringComparison.OrdinalIgnoreCase) == 0)
                ok = true;

              // ignore http://<canonical>/default.aspx (no query)
              if (hostFromIsCanonical &&
                pathFromLower == "/default.aspx" &&
                qscFrom.Count == 0)
                ok = true;

              //// ignore http://<canonical for state>/default.aspx?State=<state> 
              //if (hostFromIsCanonical &&
              //  pathFromLower == "/default.aspx" &&
              //  qscFrom.Count == 1 &&
              //  qscFrom["State"] == hostStateFrom)
              //  ok = true;

              if (!ok)
                ReportError("<ok>");
            }
            else
            {
              if (Uri.Compare(urlNormalizer.NormalizedUri, UriTo,
                UriComponents.AbsoluteUri, UriFormat.SafeUnescaped,
                StringComparison.OrdinalIgnoreCase) != 0)
              {
                bool ok = false;

                bool hostFromIsCanonical =
                  UrlManager.IsCanonicalHostName(UriFrom.Host);
                string canonicalHostFromLower = 
                  UrlManager.GetCanonicalHostName(UriFrom.Host).ToLowerInvariant();
                string pathFromLower =
                  UriFrom.AbsolutePath.ToLowerInvariant();
                string hostNewLower = 
                  urlNormalizer.NormalizedUri.Host.ToLowerInvariant();
                string pathNewLower =
                  urlNormalizer.NormalizedUri.AbsolutePath.ToLowerInvariant();
                QueryStringCollection qscFrom =
                  QueryStringCollection.Parse(UriFrom.Query);
                QueryStringCollection qscNew =
                  QueryStringCollection.Parse(urlNormalizer.NormalizedUri.Query);
                string queryStateFrom = qscFrom["State"];
                if (queryStateFrom == null || !db.Is_StateCode_State(queryStateFrom)) 
                  queryStateFrom = string.Empty;
                string stateHostFromLower = 
                  UrlManager.GetStateHostName(queryStateFrom).ToLower();

                // ignore http://<anyhost>/default.aspx (no query) ->
                // http://<canonical for anyhost>/
                if (canonicalHostFromLower == hostNewLower &&
                  pathFromLower == "/default.aspx" &&
                  pathNewLower == "/" &&
                  qscFrom.Count == 0 &&
                  qscNew.Count == 0)
                  ok = true;

                // ignore http://<anyhost>/default.aspx?State=<state> ->
                // http://<canonical for state>/
                if (qscFrom.Count == 1 &&
                  queryStateFrom != null &&
                  stateHostFromLower == hostNewLower &&
                  pathFromLower == "/default.aspx" &&
                  pathNewLower == "/")
                  ok = true;

                // ignore http://<canonical>/default.aspx?gclid=<anything>
                if (hostFromIsCanonical &&
                  pathFromLower == "/default.aspx" &&
                  qscFrom.Count == 1 &&
                  qscFrom["gclid"] != string.Empty)
                  ok = true;

                if (!ok)
                  ReportError(urlNormalizer.NormalizedUri);

                if (!urlNormalizer.Normalize(urlNormalizer.NormalizedUri) ||
                  urlNormalizer.NormalizedUri != null) // symmetry error
                  ReportError("symmetry");
              }
            }
          }
          else // error
          {
            ReportError(urlNormalizer.ErrorMessage);
          }
        }
        catch (Exception ex)
        {
          ReportError(ex.ToString());
        }
      }
    }
  }
}
