using System;
using System.Xml;
using System.IO;
using static System.String;

namespace GenerateDbClasses
{
  public static class Generator
  {
    // ReSharper disable once UnusedMember.Global
    public static TextWriter GenerateAll(string configPath,
      string namespaceName)
    {
      var configDocument = new XmlDocument();
      if (IsNullOrWhiteSpace(configPath))
        configPath = @"..\..\config.xml";
      configDocument.Load(configPath);

      if (!(configDocument.DocumentElement?.SelectSingleNode("databases") is XmlElement databasesElement))
        throw new ApplicationException("'databases' element is missing");

      if (bool.TryParse(databasesElement.GetAttribute("supportMsSql"), out var value))
        SupportMsSql = value;
      if (bool.TryParse(databasesElement.GetAttribute("supportMySql"), out value))
        SupportMySql = value;
      if (bool.TryParse(databasesElement.GetAttribute("secondary"), out value))
        Secondary = value;

      var writer = new StringWriter();
      Writer.TextWriter = writer;

      var databasesGenerator =
        new DatabasesGenerator(databasesElement, namespaceName);
      databasesGenerator.Generate();

      return writer;
    }

    public static bool Secondary { get; set; }

    public static bool SupportMsSql { get; private set; } = true;

    public static bool SupportMySql { get; private set; } = true;
  }
}
