using System;

namespace Vote
{
  // We make this an immutable object because it goes in a Dictionary.
  // We also override the equality operators and GetHashCode for the
  // same reason.
  public class ZipPlus4
  {
    public ZipPlus4(string zip5, string zip4)
    {
      if ((zip5 == null) || (zip4 == null))
        throw new ArgumentNullException();

      if ((zip5.Length != 5) || (zip4.Length != 4))
        throw new ArgumentException();

      Zip5 = zip5;
      Zip4 = zip4;
    }

    public string Zip4 { get; }

    public string Zip5 { get; }

    #region Override GetHashCode and equality methods

    public override int GetHashCode() => 
      Zip5.GetHashCode() ^ Zip4.GetHashCode();

    public static bool Equals(ZipPlus4 x, ZipPlus4 y) => 
      (x.Zip5 == y.Zip5) && (x.Zip4 == y.Zip4);

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ZipPlus4Info return false.
      var other = obj as ZipPlus4;
      return ((object) other != null) && Equals(this, other);

      // Return true if the fields match:
    }

    //public bool Equals(ZipPlus4 other)
    //{
    //  // If parameter is null return false:
    //  if ((object) other == null) return false;

    //  // Return true if the fields match:
    //  return Equals(this, other);
    //}

    public static bool operator ==(ZipPlus4 a, ZipPlus4 b)
    {
      // If both are null, or both are same instance, return true.
      if (ReferenceEquals(a, b)) return true;

      // If one is null, but not both, return false.
      if (((object) a == null) || ((object) b == null))
        return false;

      // Return true if the fields match:
      return Equals(a, b);
    }

    public static bool operator !=(ZipPlus4 a, ZipPlus4 b) => !(a == b);

    #endregion Override GetHashCode and equality methods
  }
}