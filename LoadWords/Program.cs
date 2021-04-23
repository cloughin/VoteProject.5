using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DB.Vote;

namespace LoadWords
{
  static class Program
  {
    private static readonly Regex WordRegex = new Regex(("^[a-z]+$"));
    static void Main(string[] args)
    {
      var wordCount = 0;
      var badCount = 0;
      var totalCount = 0;
      var table = WordsCommon.GetAllData();
      using (var file = File.OpenText(@"c:\Temp\2of12inf.txt"))
        while (!file.EndOfStream)
        {
          var word = file.ReadLine().Trim();
          if (word.EndsWith("%", System.StringComparison.Ordinal))
            word = word.Substring(0, word.Length - 1);
          if (WordRegex.IsMatch(word))
          {
            wordCount++;
            table.AddRow(word);
          }
          else
          {
            badCount++;
          }
          totalCount++;
        }
      //WordsCommon.UpdateTable(table);
    }
  }
}
