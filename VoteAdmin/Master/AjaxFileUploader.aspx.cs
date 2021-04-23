using System;
using System.Data;
using System.IO;
using System.Linq;
using DB.Vote;
using Jayrock.Json.Conversion;
using Vote;
using static System.String;
using static VoteAdmin.ElectionSpreadsheetsPage;

namespace VoteAdmin
{
  public partial class AjaxFileUploader : SecurePage
  {
    #region Private

    private void WriteJsonResultToResponse(AjaxResponse result)
    {
      Response.Clear();
      Response.CacheControl = "no-cache";
      Response.ContentType = "application/json; charset=utf-8";
      Response.Write(JsonConvert.ExportToString(result));
      Response.End();
    }

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    // This must be public so that the JsonConvert class can see it
    // ReSharper disable NotAccessedField.Global
    public class AjaxResponse
    {
      public bool Success;
      public string Message;
      public string Html;
    }

    // ReSharper restore NotAccessedField.Global

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public

    #region Event handlers and overrides

    protected override void SecurityExceptionHandler()
    {
      var result = new AjaxResponse { Message = "Security Exception" };
      WriteJsonResultToResponse(result);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var result = new AjaxResponse();

      try
      {
        // assume success unless an exception is thrown
        result.Success = true;

        // there should always be exactly one file
        if (Request.Files.Count == 0)
          throw new VoteException("The upload file is missing");
        if (Request.Files.Count > 1)
          throw new VoteException("Unexpected files in the upload package");

        // Test error handling
        //throw new VoteException("Some weird-ass error");

        // get the file
        var postedFile = Request.Files[0];
        var electionKey = Request.Form["electionKey"];
        Elections.ActualizeElection(electionKey);
        var jurisdictionScope = Request.Form["jurisdictionScope"];
        var electionScope = Request.Form["electionScope"];
        using (var memoryStream = new MemoryStream())
        {
          postedFile.InputStream.Position = 0;
          postedFile.InputStream.CopyTo(memoryStream);
          var spreadsheet =
            ParseSpreadsheet(memoryStream, IsExcel(postedFile.FileName));
          var columns = spreadsheet.Columns.OfType<DataColumn>().Select(c => c.ColumnName)
            .ToList();
          var id = ElectionSpreadsheets.Insert(postedFile.FileName, DateTime.UtcNow,
            memoryStream.ToArray(), electionKey, false, columns.Count, spreadsheet.Rows.Count,
            jurisdictionScope, electionScope);
          for (var x = 0; x < columns.Count; x++)
            ElectionSpreadsheetsColumns.Insert(id, x, columns[x], Empty);
          for (var x = 0; x < spreadsheet.Rows.Count; x++)
            ElectionSpreadsheetsRows.Insert(id, x, Empty, Empty, Empty, Empty);
          result.Html = GetSpreadsheetListHtml(false, id);
        }

        result.Message = "Ok";
      }
      catch (Exception ex)
      {
        result.Success = false;
        result.Message = ex.Message;
      }

      WriteJsonResultToResponse(result);
    }

    #endregion Event handlers and overrides
  }
}