namespace Vote
{
  // We make this an immutable object because it goes in a Dictionary
  // as a key. We also override the equality operators and GetHashCode 
  // for the same reason.
  public sealed class StreetInfo
  {
    public StreetInfo(string zipCode, string directionPrefix, string streetName,
      string streetSuffix, string directionSuffix)
    {
      ZipCode = zipCode;
      DirectionPrefix = directionPrefix;
      StreetName = streetName;
      StreetSuffix = streetSuffix;
      DirectionSuffix = directionSuffix;
    }

    public string DirectionPrefix { get; }

    public string DirectionSuffix { get; }

    public string StreetName { get; }

    public string StreetSuffix { get; }

    public string ZipCode { get; }

    #region Override GetHashCode and equality methods

    public override int GetHashCode() => 
      ZipCode.GetHashCode() ^
      DirectionPrefix.GetHashCode() ^
      StreetName.GetHashCode() ^
      StreetSuffix.GetHashCode() ^
      DirectionSuffix.GetHashCode();

    public static bool Equals(StreetInfo x, StreetInfo y) => 
      (x.ZipCode == y.ZipCode) &&
      (x.DirectionPrefix == y.DirectionPrefix) &&
      (x.StreetName == y.StreetName) &&
      (x.StreetSuffix == y.StreetSuffix) &&
      (x.DirectionSuffix == y.DirectionSuffix);

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to StreetInfo return false.
      var other = obj as StreetInfo;
      if ((object) other == null) return false;

      // Return true if the fields match:
      return Equals(this, other);
    }

    //public bool Equals(StreetInfo other)
    //{
    //  // If parameter is null return false:
    //  if ((object) other == null) return false;

    //  // Return true if the fields match:
    //  return Equals(this, other);
    //}

    public static bool operator ==(StreetInfo a, StreetInfo b)
    {
      // If both are null, or both are same instance, return true.
      if (ReferenceEquals(a, b)) return true;

      // If one is null, but not both, return false.
      if (((object) a == null) || ((object) b == null))
        return false;

      // Return true if the fields match:
      return Equals(a, b);
    }

    public static bool operator !=(StreetInfo a, StreetInfo b) => !(a == b);

    #endregion Override GetHashCode and equality methods
  }
}