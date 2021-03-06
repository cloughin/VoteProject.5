using System;
using System.Drawing;
using System.Globalization;
using DB.Vote;
using Jayrock.Json.Conversion;

namespace Vote.Politician
{
  public partial class AjaxImageUploaderPage : SecurePoliticianPage
  {
    #region Private

    private void WriteJsonResultToResponse(AjaxResponse result)
    {
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
      public bool Duplicate;
      public string Message;
      public string GroupName;
      public string UploadId;
      public long Length;
      public int Width;
      public int Height;
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
      var result = new AjaxResponse {Message = "Security Exception"};
      WriteJsonResultToResponse(result);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var result = new AjaxResponse();

      try
      {
        // assume success unless an exception is thrown
        result.Success = true;

        // copy back the groupname and uploadid
        result.GroupName = Request.Form["groupname"];
        result.UploadId = Request.Form["uploadid"];

        // there should always be exactly one file
        if (Request.Files.Count == 0)
          throw new VoteException("The upload file is missing");
        if (Request.Files.Count > 1)
          throw new VoteException("Unexpected files in the upload package");

        // Test error handling
        //throw new VoteException("Some weird-ass error");

        // get the file
        var postedFile = Request.Files[0];

        // save the timestamp so the time we post to all the various tables matches
        // exactly...
        var uploadTime = DateTime.UtcNow;

        // We only update headshot images if there is no Headshot -- 
        //   we check Headshot100 determine if this is so
        // We request duplicate testing...
        byte[] blob;
        Size originalSize;
        byte[] originalBlob;
        if (PoliticiansImagesBlobs.GetHeadshot100(PoliticianKey) == null)
          blob = ImageManager.UpdateAllPoliticianImages(PoliticianKey,
            postedFile.InputStream, uploadTime, true, out originalSize,
            out originalBlob);
        else
          blob = ImageManager.UpdatePoliticianProfileImages(PoliticianKey,
            postedFile.InputStream, uploadTime, true, out originalSize,
            out originalBlob);

        if (blob == null) // means it was a duplicate
        {
          result.Duplicate = true;
          result.Message =
            "The uploaded picture is the same as we already have on our servers.";
        }
        else
        {
          result.Message =
            "The picture uploaded successfully. Original file length = " +
            postedFile.InputStream.Length.ToString(CultureInfo.InvariantCulture);
          result.Length = postedFile.InputStream.Length;
          result.Width = originalSize.Width;
          result.Height = originalSize.Height;
          // invalidate cached image on load-balanced servers
          CommonCacheInvalidation.ScheduleInvalidation("politicianimage",
            PoliticianKey);
          LogImageChange(originalBlob, blob, uploadTime);
          Politicians.UpdateDatePictureUploaded(uploadTime, PoliticianKey);
        }
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