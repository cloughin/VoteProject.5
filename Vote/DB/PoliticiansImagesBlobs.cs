namespace DB.Vote
{
  public partial class PoliticiansImagesBlobs
  {
    public static void GuaranteePoliticianKeyExists(string politicianKey)
    {
      if (PoliticianKeyExists(politicianKey)) return;
      Insert(politicianKey, null, null, null, null, null, null, null, null, null, null);
    }
  }
}