using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.String;

namespace VotePrebuild
{
  static class Program
  {
    private const string ConfigKey = "VotePrebuild";

    private static void Main()
    {
      try
      {
        // Get the configuration
        var settings = ConfigurationManager.AppSettings[ConfigKey];
        if (IsNullOrWhiteSpace(settings))
          throw new Exception($"Could not get AppSetting '{ConfigKey}'");

        // Parse the attributes
        string dataMainPath = null;
        string requireMainPath = null;
        string publicMasterPath = null;
        var match = Regex.Match(settings, @"(?:\s*(?<attribute>[_a-zA-Z][_.a-zA-Z0-9-]+)\s*=\s*""\s*(?<value>[^""]*)\s*""\s*(?:;|$))+");
        if (!match.Success)
          throw new Exception($"Could not parse the settings '{settings}'");
        var valueIndex = 0;
        foreach (var attributeCapture in match.Groups["attribute"].Captures.Cast<Capture>())
        {
          var attribute = attributeCapture.Value;
          var value = match.Groups["value"].Captures[valueIndex++].Value;
          switch (attribute)
          {
            case "data-main":
              dataMainPath = value;
              break;

            case "require-main":
              requireMainPath = value;
              break;

            case "public-master":
              publicMasterPath = value;
              break;

            default:
              throw new Exception($"Unidentified attribute in settings '{attribute}'");
          }
        }

        // open the dataMain file and locate the data-main attribute
        Debug.Assert(dataMainPath != null, "dataMainPath != null");
        var dataMainContent = File.ReadAllText(dataMainPath);
        var dataMainMatch = Regex.Match(dataMainContent,
         @"\s+data-main\s*=\s*""/js/require-main.js\?v=(?<version>[1-9][0-9]*)""",
         RegexOptions.IgnoreCase);
        if (!dataMainMatch.Success)
          throw new Exception("Could not find matching 'data-main' attribute in data-main file");
        var dataMainCaptures = dataMainMatch.Groups["version"].Captures;
        if (dataMainCaptures.Count != 1)
          throw new Exception("Expecting exactly 1 matching 'data-main' attribute in data-main file");
        var dataMainVersion = int.Parse(dataMainCaptures[0].Value);

        // open the requireMain file and locate the urlArgs attribute
        Debug.Assert(requireMainPath != null, "requireMainPath != null");
        var requireMainContent = File.ReadAllText(requireMainPath);
        var requireMainMatch = Regex.Match(requireMainContent,
         @"\s*urlArgs\s*:\s*""v=(?<version>[1-9][0-9]*)""",
         RegexOptions.IgnoreCase);
        if (!requireMainMatch.Success)
          throw new Exception("Could not find matching 'urlArgs' attribute in require-main file");
        var requireMainCaptures = requireMainMatch.Groups["version"].Captures;
        if (requireMainCaptures.Count != 1)
          throw new Exception("Expecting exactly 1 matching 'urlArgs' attribute in data-main file");
        var requireMainVersion = int.Parse(requireMainCaptures[0].Value);

        // open public.master and locate the .css and .js references
        Debug.Assert(publicMasterPath != null, "publicMasterPath != null");
        var publicMasterContent = File.ReadAllText(publicMasterPath);
        //var publicMasterCssMatch = Regex.Match(requireMainContent,
        // @"<link\s+rel\s*=\s*""stylesheet""\s+href\s*=\s*""/css/vote/public.css\?(?<version>[1-9][0-9]*)""\s*/>",
        // RegexOptions.IgnoreCase);
        var publicMasterCssMatch = Regex.Match(publicMasterContent,
         @"<link\s+rel\s*=\s*""stylesheet""\s+href\s*=\s*""/css/vote/public.min.css\?(?<version>[1-9][0-9]*)""\s*/>", 
         RegexOptions.IgnoreCase);
        if (!publicMasterCssMatch.Success)
          throw new Exception("Could not find matching stylesheet tag in public master file");
        var publicMasterCssCaptures = publicMasterCssMatch.Groups["version"].Captures;
        if (publicMasterCssCaptures.Count != 1)
          throw new Exception("Expecting exactly 1 matching stylesheet tag in public master file");
        var publicMasterCssVersion = int.Parse(publicMasterCssCaptures[0].Value);
        var publicMasterJsMatch = Regex.Match(publicMasterContent,
         @"<script\s+type\s*=\s*""text/javascript""\s+src\s*=\s*""/js/scripts.min.js\?(?<version>[1-9][0-9]*)""\s*>",
         RegexOptions.IgnoreCase);
        if (!publicMasterJsMatch.Success)
          throw new Exception("Could not find matching script tag in public master file");
        var publicMasterJsCaptures = publicMasterJsMatch.Groups["version"].Captures;
        if (publicMasterJsCaptures.Count != 1)
          throw new Exception("Expecting exactly 1 matching script tag in public master file");
        var publicMasterJsVersion = int.Parse(publicMasterJsCaptures[0].Value);

        // use higher version
        var newVersion = (Math.Max(Math.Max(Math.Max(dataMainVersion, requireMainVersion), 
          publicMasterCssVersion), publicMasterJsVersion) + 1)
          .ToString(CultureInfo.InvariantCulture);

        // replace version in each file with new version
        dataMainContent = dataMainContent.Substring(0, dataMainCaptures[0].Index) +
          newVersion +
          dataMainContent.Substring(dataMainCaptures[0].Index +
            dataMainCaptures[0].Length);
        requireMainContent =
          requireMainContent.Substring(0, requireMainCaptures[0].Index) +
            newVersion +
            requireMainContent.Substring(requireMainCaptures[0].Index +
              requireMainCaptures[0].Length);

        // these must be done last to first
        var first = publicMasterCssCaptures;
        var second = publicMasterJsCaptures;
        if (first[0].Index > second[0].Index)
        {
          var temp = first;
          first = second;
          second = temp;
        }
        publicMasterContent = publicMasterContent.Substring(0, second[0].Index) +
          newVersion +
          publicMasterContent.Substring(second[0].Index + second[0].Length);
        publicMasterContent = publicMasterContent.Substring(0, first[0].Index) +
          newVersion +
          publicMasterContent.Substring(first[0].Index + first[0].Length);

        // replace in public master

        // write back the new files
        File.WriteAllText(dataMainPath, dataMainContent);
        File.WriteAllText(requireMainPath, requireMainContent);
        File.WriteAllText(publicMasterPath, publicMasterContent);

        Console.WriteLine($"New version = {newVersion}");
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
      finally
      {
        Console.Write("Press any key to exit");
        Console.ReadKey();
      }
    }
  }
}
