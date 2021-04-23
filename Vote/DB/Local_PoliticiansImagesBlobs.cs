namespace DB.VoteImagesLocal
{
  public partial class PoliticiansImagesBlobs
  {
    public static void Upsert(string politicianKey, byte[] profileOriginal, byte[] profile300,
      byte[] profile200, byte[] headshot100, byte[] headshot75, byte[] headshot50,
      byte[] headshot35, byte[] headshot25, byte[] headshot20, byte[] headshot15,
      int commandTimeout = -1)
    {
      const string cmdText =
        "INSERT INTO PoliticiansImagesBlobs" +
        " (PoliticianKey,ProfileOriginal,Profile300,Profile200,Headshot100,Headshot75," +
        "Headshot50,Headshot35,Headshot25,Headshot20,Headshot15)" +
        " VALUES (@PoliticianKey,@ProfileOriginal,@Profile300,@Profile200,@Headshot100," +
        "@Headshot75,@Headshot50,@Headshot35,@Headshot25,@Headshot20,@Headshot15)" +
        " ON DUPLICATE KEY UPDATE ProfileOriginal=VALUES(ProfileOriginal),Profile300=VALUES(Profile300)," +
        "Profile200=VALUES(Profile200),Headshot100=VALUES(Headshot100),Headshot75=VALUES(Headshot75)," +
        "Headshot50=VALUES(Headshot50),Headshot35=VALUES(Headshot35),Headshot25=VALUES(Headshot25)," +
        "Headshot20=VALUES(Headshot20),Headshot15=VALUES(Headshot15)";
      var cmd = VoteImagesLocalDb.GetCommand(cmdText, commandTimeout);
      VoteImagesLocalDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      VoteImagesLocalDb.AddCommandParameter(cmd, "ProfileOriginal", profileOriginal);
      VoteImagesLocalDb.AddCommandParameter(cmd, "Profile300", profile300);
      VoteImagesLocalDb.AddCommandParameter(cmd, "Profile200", profile200);
      VoteImagesLocalDb.AddCommandParameter(cmd, "Headshot100", headshot100);
      VoteImagesLocalDb.AddCommandParameter(cmd, "Headshot75", headshot75);
      VoteImagesLocalDb.AddCommandParameter(cmd, "Headshot50", headshot50);
      VoteImagesLocalDb.AddCommandParameter(cmd, "Headshot35", headshot35);
      VoteImagesLocalDb.AddCommandParameter(cmd, "Headshot25", headshot25);
      VoteImagesLocalDb.AddCommandParameter(cmd, "Headshot20", headshot20);
      VoteImagesLocalDb.AddCommandParameter(cmd, "Headshot15", headshot15);
      VoteImagesLocalDb.ExecuteNonQuery(cmd);
    }
  }
}