using System;
using static System.String;

namespace Vote
{
  public class VideoInfo
  {
    public const int MaxVideoDescriptionLength = 300;

    // ReSharper disable once NotAccessedField.Global
    public readonly string Id;
    public readonly bool IsValid;
    public readonly string Title;
    public readonly string Description;
    public readonly DateTime PublishedAt;
    public readonly TimeSpan Duration;
    public readonly bool IsPublic;

    protected VideoInfo()
    {
    }

    protected VideoInfo(string id)
    {
      Id = id;
    }

    protected VideoInfo(string id, string title, string description, DateTime publishedAt,
      TimeSpan duration, bool isPublic)
    {
      Id = id;
      Title = title;
      Description = description;
      PublishedAt = publishedAt;
      Duration = duration;
      IsPublic = isPublic;
      IsValid = true;
    }

    public string FullDescription
    {
      get
      {
        var description = Description.SafeString().Trim();
        if (IsNullOrWhiteSpace(description)) description = Title.SafeString().Trim();
        if (IsNullOrWhiteSpace(description)) description = "Description not available";
        return description;
      }
    }

    public string ShortDescription
    {
      get
      {
        var description = FullDescription;
        if (description.Length > MaxVideoDescriptionLength)
          description = description.Substring(0, MaxVideoDescriptionLength) + "...";
        return description;
      }
    }
  }
}