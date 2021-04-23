namespace DB.VoteZipNewLocal
{
  public partial class ZipStreetsRow
  {
  }

  public partial class ZipStreets
  {
    public static ZipStreetsTable GetDataByZipCodes(
      string[] zipCodes, string houseNumber, string directionalPrefix,
      string streetNamePattern, string streetSuffix, string directionalSuffix,
      bool useMetaphone, int commandTimeout)
    {
      // We violate the "only-parameters" rule because they can't be used with IN
      string zipCodeCondition;
      if (zipCodes.Length == 1)
        zipCodeCondition = " AND ZipCode='" + zipCodes[0] + "' ";
      else
        zipCodeCondition = " AND ZipCode IN ('" + string.Join("','", zipCodes) +
          "') ";

      var houseNumberCondition = string.Empty;
      var evenOddCondition = string.Empty;
      if (!string.IsNullOrEmpty(houseNumber))
      {
        // We include wildcards (both numbers empty)
        // We also match the length to prevent false matches. Only need to
        // check one length since they should be the same.
        houseNumberCondition = " AND (@HouseNumber >= AddressPrimaryLowNumber " +
          "  AND @HouseNumber <= AddressPrimaryHighNumber " +
          "  AND Length(@HouseNumber) = Length(AddressPrimaryHighNumber) " +
          "  OR AddressPrimaryLowNumber = '' AND AddressPrimaryHighNumber = '') ";

        // the evenOddCondition is based on the last numeric digit, and excludes
        // the non-matching condition (ie "even" becomes <> 'O') to allow 'B' to match
        for (var n = houseNumber.Length - 1; n >= 0; n--)
        {
          var lastDigit = houseNumber[n];
          if (!char.IsDigit(lastDigit)) continue;
          evenOddCondition = lastDigit % 2 == 0
            ? " AND AddressPrimaryEvenOdd <> 'O' "
            : " AND AddressPrimaryEvenOdd <> 'E' ";
          break;
        }
      }

      string streetCondition;
      if (useMetaphone)
      {
        streetNamePattern = streetNamePattern.Replace('%', ' ')
          .Trim();
        streetCondition = " Metaphone = @StreetNamePattern ";
      }
      else
        streetCondition = " StName LIKE @StreetNamePattern ";

      var directionalPrefixCondition = string.Empty;
      if (!string.IsNullOrEmpty(directionalPrefix))
        directionalPrefixCondition = " AND @DirectionalPrefix = StPreDirAbbr ";

      var streetSuffixCondition = string.Empty;
      if (!string.IsNullOrEmpty(streetSuffix))
        streetSuffixCondition = " AND @StreetSuffix = StSuffixAbbr ";

      var directionalSuffixCondition = string.Empty;
      if (!string.IsNullOrEmpty(directionalSuffix))
        directionalSuffixCondition = " AND @DirectionalSuffix = StPostDirAbbr ";

      var cmdText =
        string.Format(
          SelectAllCommandText + " WHERE  {0} {1} {2} {3} {4} {5} {6}",
          streetCondition, zipCodeCondition, houseNumberCondition,
          directionalPrefixCondition, streetSuffixCondition,
          directionalSuffixCondition, evenOddCondition);
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);

      VoteZipNewLocalDb.AddCommandParameter(
        cmd, "StreetNamePattern", streetNamePattern);
      if (houseNumber != null)
        VoteZipNewLocalDb.AddCommandParameter(cmd, "HouseNumber", houseNumber);
      if (directionalPrefix != null)
        VoteZipNewLocalDb.AddCommandParameter(
          cmd, "DirectionalPrefix", directionalPrefix);
      if (streetSuffix != null)
        VoteZipNewLocalDb.AddCommandParameter(cmd, "StreetSuffix", streetSuffix);
      if (directionalSuffix != null)
        VoteZipNewLocalDb.AddCommandParameter(
          cmd, "DirectionalSuffix", directionalSuffix);

      return FillTable(cmd);
    }
  }
}