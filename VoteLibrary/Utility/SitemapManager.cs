using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using DB;
using DB.Vote;
using static System.String;

namespace Vote
{
  public class SitemapManager
  {
    #region Private

    private XmlTextWriter _XmlTextWriter;
    private int _Pages;

    private readonly string[] _UxCodes = { "U1", "U2", "U3", "U4" };

    private readonly Dictionary<string, object> _ElectionPages = 
      new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase); // ElectionKey
    private readonly Dictionary<StringPairIgnoreCase, object> _CompareCandidatesPages =
      new Dictionary<StringPairIgnoreCase, object>(); // ElectionKey, OfficeKey
    private readonly Dictionary<string, object> _IntroPages = 
      new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase); // PoliticianKey
    private readonly Dictionary<string, object> _OfficialsPages = 
      new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase); // Report

    private void AddCompareCandidates(string electionKey, string officeKey)
    {
      var pair = new StringPairIgnoreCase(electionKey, officeKey);
      if (!_CompareCandidatesPages.ContainsKey(pair))
        _CompareCandidatesPages.Add(pair, null);
    }

    private void AddElection(string electionKey)
    {
      if (!_ElectionPages.ContainsKey(electionKey))
        _ElectionPages.Add(electionKey, null);
    }

    private void AddIntroPages(string politicianKey)
    {
      if (!_IntroPages.ContainsKey(politicianKey))
        _IntroPages.Add(politicianKey, null);
    }

    private void AddOfficialsPages(string reportKey)
    {
      if (!_OfficialsPages.ContainsKey(reportKey))
        _OfficialsPages.Add(reportKey, null);
    }

    private void CreateDomainSitemap(string domainCode)
    {
      IncludeElectedOfficials(domainCode);
      IncludeViewableElectionsOfficesAndCandidates(domainCode);

      if (domainCode == "US")
        WriteUrl(UrlManager.GetSiteUri()); // Include home page
      WriteOfficialsPages();
      WriteElectionPages();
      WriteCompareCandidatesPages();
      WriteIntroPages();

      Sitemap.UpdateLastCreated(DateTime.UtcNow, domainCode);
    }

    private void CreateNewDomainSitemap(string domainCode)
    {
      IncludeElectedOfficials(domainCode);
      IncludeViewableElectionsOfficesAndCandidates(domainCode);
    }

    private void IncludeElectedOfficials(string domainCode)
    {
      // We currently omit county and local.
      if (domainCode == "US")
        foreach (var report in _UxCodes)
          AddOfficialsPages(report);
      else
        AddOfficialsPages(domainCode);

      // Add the officials. USPresident has blank state code, so is retrieved for all states, but
      // only included if it matched the state.
      foreach (var politicianKey in OfficesOfficials.GetIncumbentsByState(domainCode))
        AddIntroPages(politicianKey);
    }

    private void IncludeViewableElectionsOfficesAndCandidates(string domainCode)
    {
      // We fetch future US "elections" always, so candidates can be included in their appropriate
      // state, plus state.

      var table = Elections.GetElectionsForSitemap(domainCode);
      var minCompareCandidatesDate = DateTime.UtcNow.Subtract(new TimeSpan(60, 0, 0, 0));
      var minIntroDate = DateTime.UtcNow.Subtract(new TimeSpan(365, 0, 0, 0));

      // Only add elections that match domain. Don't add US election: The CompareCandidates entry
      // will be sufficient.
      if (domainCode != "US")
        foreach (var electionKey in table.Rows
          .Cast<DataRow>()
          .Select(row => row.ElectionKey())
          .Distinct()
          .Where(key => domainCode.IsEqIgnoreCase(Elections.GetStateCodeFromKey(key))))
          AddElection(electionKey);

      // Add CompareCandidates for all offices with multiple candidates that match domain
      // but only for elections less than 60 days old
      foreach (var office in table.Rows
        .Cast<DataRow>()
        .Where(row => domainCode.IsEqIgnoreCase(Elections.GetStateCodeFromKey(row.ElectionKey())) &&
          row.ElectionDate() > minCompareCandidatesDate)
        .GroupBy(row => new { ElectionKey = row.ElectionKey(), OfficeKey = row.OfficeKey() }))
        if (office.Count() > 1)
          AddCompareCandidates(office.Key.ElectionKey, office.Key.OfficeKey);

      // Add Intro for all candidates and running mates that match domain
      // but only for elections less than 365 days old
      foreach (var politicianKey in table.Rows
        .Cast<DataRow>()
        .Where(row => row.ElectionDate() > minIntroDate)
        .Select(row => row.PoliticianKey())
        .Where(key => domainCode.IsEqIgnoreCase(Politicians.GetStateCodeFromKey(key)))
        .Union(table.Rows
          .Cast<DataRow>()
          .Select(row => row.RunningMateKey()))
          .Where(key => domainCode.IsEqIgnoreCase(Politicians.GetStateCodeFromKey(key))))
          AddIntroPages(politicianKey);
    }

    private static void ServeSitemapContent(
      string domainDataCode, DateTime lastModDate, bool isModified)
    {
      var response = HttpContext.Current.Response;
      var maxAge = new TimeSpan(24, 0, 0); // 24 hours -- used in headers
      var expiration = DateTime.UtcNow + maxAge;

      response.Cache.SetCacheability(HttpCacheability.Public);
      response.Cache.SetETag(
        '"' + lastModDate.Ticks.ToString(CultureInfo.InvariantCulture) + '"');
      response.Cache.SetLastModified(lastModDate);
      response.Cache.SetMaxAge(maxAge);
      response.Cache.SetExpires(expiration);
      response.Cache.SetSlidingExpiration(false);
      if (isModified) // serve actual sitemap
      {
        var sitemap = Sitemap.GetSitemapXml(domainDataCode);
        if (sitemap == null || sitemap.Length == 0)
          throw new VoteException("No sitemap for {0}", domainDataCode);

        response.ContentType = "text/xml";
        response.ContentEncoding = Encoding.UTF8;
        response.BinaryWrite(sitemap);
      }
      else // tell client to use cached version
      {
        response.StatusCode = 304;

        // Explicitly set the Content-Length header so the client doesn't wait for
        //  content but keeps the connection open for other requests 
        response.AddHeader("Content-Length", "0");
      }
    }

    private int UpdateVirtualPage(string domainDataCode)
    {
      var stream = new MemoryStream();
      _XmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
      _XmlTextWriter.WriteStartDocument(true);
      const string urlSet = "http://www.sitemaps.org/schemas/sitemap/0.9";
      _XmlTextWriter.WriteStartElement("urlset", urlSet);
      CreateDomainSitemap(domainDataCode);
      _XmlTextWriter.WriteEndElement(); 
      _XmlTextWriter.WriteEndDocument();
      _XmlTextWriter.Close();
      Sitemap.UpdateSitemapXml(stream.ToArray(), domainDataCode);
      return _Pages;
    }

    // this is the one used after we eliminated the 51 domains
    private int UpdateVirtualPage()
    {
      var stream = new MemoryStream();
      _XmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
      _XmlTextWriter.WriteStartDocument(true);
      const string urlSet = "http://www.sitemaps.org/schemas/sitemap/0.9";
      _XmlTextWriter.WriteStartElement("urlset", urlSet);

      WriteUrl(UrlManager.GetSiteUri()); // Include home page

      var table = Sitemap.GetAllKeyData(0);
      foreach (var row in table)
        CreateNewDomainSitemap(row.DomainDataCode);

      WriteOfficialsPages();
      WriteElectionPages();
      WriteCompareCandidatesPages();
      WriteIntroPages();

      _XmlTextWriter.WriteEndElement();
      _XmlTextWriter.WriteEndDocument();
      _XmlTextWriter.Close();
      Sitemap.UpdateSitemapXml(stream.ToArray(), "US");
      Sitemap.UpdateLastCreated(DateTime.UtcNow, "US");
      return _Pages;
    }

    private void WriteCompareCandidatesPages()
    {
      foreach (var key in _CompareCandidatesPages.Keys.OrderBy(key => key))
        WriteUrl(UrlManager.GetCompareCandidatesPageUri(key.String1, key.String2));
    }

    private void WriteElectionPages()
    {
      foreach (var electionKey in _ElectionPages.Keys.OrderBy(key => key))
        WriteUrl(UrlManager.GetElectionPageUri(electionKey));
    }

    private void WriteIntroPages()
    {
      foreach (var politicialKey in _IntroPages.Keys.OrderBy(key => key))
        WriteUrl(UrlManager.GetIntroPageUri(politicialKey));
    }

    private void WriteOfficialsPages()
    {
      foreach (var report in _OfficialsPages.Keys.OrderBy(key => key))
        WriteUrl(UrlManager.GetOfficialsPageUri(report));
    }

    private void WriteUrl(Uri uri)
    {
      _XmlTextWriter.WriteStartElement("url");
      _XmlTextWriter.WriteStartElement("loc");
      _XmlTextWriter.WriteString(uri.ToString());
      _XmlTextWriter.WriteEndElement();
      _XmlTextWriter.WriteEndElement();
      _Pages++;
    }

    #endregion

    public static void ServeSitemap(string domainDataCode)
    {
      var request = HttpContext.Current.Request;
      var response = HttpContext.Current.Response;

      try
      {
        var lastModDate = Sitemap.GetLastCreated(domainDataCode).AsUtc();
        if (lastModDate == null) // base code, return 404
          throw new VoteException("No sitemap for {0}", domainDataCode);

        // First check if the sitemap has been modified
        var ifModifiedSinceHeader = request.Headers["If-Modified-Since"];
        var isModified = true; // assume modified unless we prove otherwise
        if (!IsNullOrWhiteSpace(ifModifiedSinceHeader) &&
          DateTime.TryParse(ifModifiedSinceHeader, out var ifModifiedSince))
        {
          isModified = false; // change our assumption
          // If mod date is greater, we need to check for insignificant (< 1 sec)
          // difference, because of lossy date conversions.
          if (lastModDate > ifModifiedSince)
            isModified = lastModDate - ifModifiedSince > TimeSpan.FromSeconds(1);
        }

        ServeSitemapContent(domainDataCode, lastModDate.Value, isModified);
      }
      catch (VoteException ex)
      {
        response.Write(ex.Message);
        response.StatusCode = 404;
      }
      catch (Exception)
      {
        response.Write("Sitemap not available");
        response.StatusCode = 404;
      }
      finally
      {
        response.End();
      }
    }

    public static int UpdateAllSitemapVirtualPages()
    {
      string message;
      var pages = 0;

      try
      {
        VotePage.LogInfo("UpdateAllSitemapVirtualPages", "Started");

        var table = Sitemap.GetAllKeyData(0);
        foreach (var row in table)
          pages += new SitemapManager().UpdateVirtualPage(row.DomainDataCode);

        message = "Completed";
      }
      catch (Exception ex)
      {
        VotePage.LogException("UpdateAllSitemapVirtualPages", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("UpdateAllSitemapVirtualPages", message);
      return pages;
    }

    public static int UpdateSitemapVirtualPage()
    {
      string message;
      var pages = 0;

      try
      {
        VotePage.LogInfo("UpdateSitemapVirtualPage", "Started");

        //var table = Sitemap.GetAllKeyData(0);
        //foreach (var row in table)
        //  new SitemapManager().UpdateVirtualPage(row.DomainDataCode);

        pages = new SitemapManager().UpdateVirtualPage();

        message = "Completed";
      }
      catch (Exception ex)
      {
        VotePage.LogException("UpdateSitemapVirtualPage", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("UpdateSitemapVirtualPage", message);
      return pages;
    }
  }
}