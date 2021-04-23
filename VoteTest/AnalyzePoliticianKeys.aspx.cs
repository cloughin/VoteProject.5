using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vote;
using DB.Vote;

namespace VoteTest
{
  public partial class AnalyzePoliticianKeys : System.Web.UI.Page
  {
    class BadKey
    {
      public string PoliticianKey;
      public string Errors;
    }

    List<BadKey> BadKeyList;

    private void CheckForNonAcsiiLetters()
    {
      BadKeyList = new List<BadKey>();
      int nonAscii = 0;
      int nonAlpha = 0;
      int potentialDuplicates = 0;
      int empty = 0;
      int badStateCode = 0;
      Regex nonAlphaRegex = new Regex(@"\P{L}");

      var table = Politicians.GetAllPoliticianKeysData();

      foreach (var row in table)
      {
        List<string> errors = new List<string>();
        string politicianKey = row.PoliticianKey;

        // check for non-ascii characters
        string allAscii = politicianKey.ToAscii();
        if (allAscii != politicianKey)
        {
          nonAscii++;
          errors.Add("non-ascii");
          // get the all-ascii key from the db -- if not
          // equal to the original we have a duplicate
          string dbKey = Politicians.GetPoliticianKeyByPoliticianKey(allAscii);
          if (dbKey != null && dbKey != politicianKey)
          {
            potentialDuplicates++;
            errors.Add(string.Format("duplicate ascii key ({0})", allAscii));
          }
        }

        // check for non-alphabetic characters
        if (nonAlphaRegex.Match(allAscii).Success)
        {
          nonAlpha++;
          errors.Add("non-alpha");
          string allAlpha = nonAlphaRegex.Replace(allAscii, match => String.Empty);
          if (allAlpha.Length == 0)
          {
            empty++;
            errors.Add("empty alpha key");
          }
          else if (Politicians.PoliticianKeyExists(allAlpha))
          {
            potentialDuplicates++;
            errors.Add(string.Format("duplicate alpha key ({0})", allAlpha));
          }
        }

        // check for missing state code
        string stateCode = politicianKey.Substring(0, 
          Math.Min(2, politicianKey.Length));
        if (stateCode != stateCode.ToUpper() ||
          !db.Is_StateCode_State(stateCode))
        {
          badStateCode++;
          errors.Add("invalid state code");
        }

        // if errors, add to list
        if (errors.Count > 0)
        {
          BadKeyList.Add(new BadKey()
          {
            PoliticianKey = politicianKey,
            Errors = string.Join(", ", errors)
          });
        }
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      CheckForNonAcsiiLetters();

      Table table = new Table();
      TableRow header = new TableRow();
      table.Rows.Add(header);

      TableHeaderCell keyHeader = new TableHeaderCell();
      header.Cells.Add(keyHeader);
      Literal keyHeaderLiteral = new Literal();
      keyHeader.Controls.Add(keyHeaderLiteral);
      keyHeaderLiteral.Text = "Politician Key";

      TableHeaderCell errorHeader = new TableHeaderCell();
      header.Cells.Add(errorHeader);
      Literal errorHeaderLiteral = new Literal();
      errorHeader.Controls.Add(errorHeaderLiteral);
      errorHeaderLiteral.Text = "Errors";

      PlaceHolderForTable.Controls.Add(table);
      foreach (BadKey badkey in BadKeyList)
      {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);

        TableCell keyCell = new TableCell();
        tr.Cells.Add(keyCell);
        Literal keyLiteral = new Literal();
        keyCell.Controls.Add(keyLiteral);
        keyLiteral.Text = badkey.PoliticianKey;

        TableCell errorCell = new TableCell();
        tr.Cells.Add(errorCell);
        Literal errorLiteral = new Literal();
        errorCell.Controls.Add(errorLiteral);
        errorLiteral.Text = badkey.Errors;
      }
    }
  }
}