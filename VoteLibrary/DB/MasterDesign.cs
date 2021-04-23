using Vote;

namespace DB.Vote
{
  public partial class MasterDesign
  {
    private static string GetDesignStringWithSubstitutions(string dataColumnName,
      Substitutions substitutions = null)
    {
      return GetDesignStringWithSubstitutions(GetColumn(dataColumnName),
        substitutions);
    }

    private static string GetDesignStringWithSubstitutions(Column dataColumn,
      Substitutions substitutions = null)
    {
      var dataColumnName = GetColumnName(dataColumn);
      var masterColumn = GetColumn(dataColumnName);
      var text = GetColumn(masterColumn) as string;
      if (substitutions == null)
        substitutions = new Substitutions();
      return substitutions.Substitute(text);
    }

    public static string GetSubstitutionText(string textKey)
    {
      return GetDesignStringWithSubstitutions(textKey);
    }
  }
}