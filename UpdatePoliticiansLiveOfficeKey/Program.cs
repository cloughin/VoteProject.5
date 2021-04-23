using System;
using DB.Vote;

namespace UpdatePoliticiansLiveOfficeKey
{
  internal static class Program
  {
// ReSharper disable UnusedParameter.Local
    private static void Main(string[] args)
// ReSharper restore UnusedParameter.Local
    {
      Console.WriteLine("Begin UpdatePoliticiansLiveOfficeKey");
      Vote.UpdatePoliticiansLiveOfficeKey.Update();
      Console.WriteLine("End UpdatePoliticiansLiveOfficeKey");

      Console.WriteLine("Begin UpdateOfficesAlternateOfficeLevel");
      Vote.UpdateOfficesAlternateOfficeLevel.Update();
      Console.WriteLine("End UpdateOfficesAlternateOfficeLevel");

      Console.WriteLine("Begin UpdatePoliticianSearchKeys");
      Vote.UpdatePoliticianSearchKeys.Update();
      Console.WriteLine("End UpdatePoliticianSearchKeys");

      Console.WriteLine("Begin UpdateSingleCandidateContestWinners");
      Vote.UpdateSingleCandidateContestWinners.Update();
      Console.WriteLine("End UpdateSingleCandidateContestWinners");

      Console.WriteLine("Begin RefreshYouTubeAnswers");
      Vote.YouTubeVideoUtility.RefreshYouTubeAnswers();
      Console.WriteLine("End RefreshYouTubeAnswers");

      Console.WriteLine("Begin RefreshYouTubePoliticians");
      Vote.YouTubeVideoUtility.RefreshYouTubePoliticians();
      Console.WriteLine("End RefreshYouTubePoliticians");

      Console.WriteLine("Begin CreateOffYearGovernorsPseudoElections");
      Elections.CreateOffYearGovernorsPseudoElections();
      Console.WriteLine("End CreateOffYearGovernorsPseudoElections");
    }
  }
}