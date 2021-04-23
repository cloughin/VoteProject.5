using System;
using DB.VoteZipNewLocal;
using LastUpdate = DB.VoteZipNew.LastUpdate;
using ZipStreets = DB.VoteZipNew.ZipStreets;

namespace Vote
{
  public static class LocalZipUpdate
  {
    //private static void CopyZipCitiesDownloaded()
    //{
    //  var count = 0;
    //  ZipCitiesDownloadedTemp.Drop();
    //  ZipCitiesDownloadedTemp.Create();
    //  var done = false;
    //  while (!done)
    //    using (var reader = ZipCitiesDownloaded.GetAllDataReaderAt(count, 0))
    //      try
    //      {
    //        while (reader.Read())
    //        {
    //          ZipCitiesDownloadedTemp.Insert(
    //            zipCode: reader.ZipCode, city: reader.City, state: reader.State,
    //            county: reader.County, areaCode: reader.AreaCode,
    //            cityType: reader.CityType,
    //            cityAliasAbbreviation: reader.CityAliasAbbreviation,
    //            cityAliasName: reader.CityAliasName, latitude: reader.Latitude,
    //            longitude: reader.Longitude, timeZone: reader.TimeZone,
    //            elevation: reader.Elevation, countyFips: reader.CountyFips,
    //            daylightSaving: reader.DaylightSaving,
    //            preferredLastLineKey: reader.PreferredLastLineKey,
    //            classificationCode: reader.ClassificationCode,
    //            multiCounty: reader.MultiCounty, stateFips: reader.StateFips,
    //            cityStateKey: reader.CityStateKey,
    //            cityAliasCode: reader.CityAliasCode,
    //            primaryRecord: reader.PrimaryRecord,
    //            cityMixedCase: reader.CityMixedCase,
    //            cityAliasMixedCase: reader.CityAliasMixedCase,
    //            stateAnsi: reader.StateAnsi, countyAnsi: reader.CountyAnsi,
    //            facilityCode: reader.FacilityCode,
    //            cityDeliveryIndicator: reader.CityDeliveryIndicator,
    //            carrierRouteRateSortation: reader.CarrierRouteRateSortation,
    //            financeNumber: reader.FinanceNumber,
    //            uniqueZipName: reader.UniqueZipName,
    //            metaphoneAliasName: reader.MetaphoneAliasName,
    //            metaphoneAliasAbbreviation: reader.MetaphoneAliasAbbreviation);
    //          count++;
    //          if ((count%10000) == 0)
    //            Console.WriteLine("ZipCitiesDownloaded: {0}", count);
    //        }
    //        done = true;
    //        Console.WriteLine("ZipCitiesDownloaded: {0}", count);
    //      }
    //      catch {}
    //}

    //private static void CopyZipSingleUszd()
    //{
    //  var count = 0;
    //  ZipSingleUszdTemp.Drop();
    //  ZipSingleUszdTemp.Create();
    //  var done = false;
    //  while (!done)
    //    using (var reader = ZipSingleUszd.GetAllDataReaderAt(count, 0))
    //      try
    //      {
    //        while (reader.Read())
    //        {
    //          ZipSingleUszdTemp.Insert(
    //            zipCode: reader.ZipCode, congress: reader.Congress,
    //            stateSenate: reader.StateSenate, stateHouse: reader.StateHouse,
    //            ldsStateCode: reader.LdsStateCode, county: reader.County,
    //            stateCode: reader.StateCode);
    //          count++;
    //          if ((count%10000) == 0)
    //            Console.WriteLine("ZipSingleUszd: {0}", count);
    //        }
    //        done = true;
    //        Console.WriteLine("ZipSingleUszd: {0}", count);
    //      }
    //      catch {}
    //}

    private static void CopyZipStreets()
    {
      var count = 0;
      ZipStreetsTemp.Drop();
      ZipStreetsTemp.Create();
      var done = false;
      while (!done)
        using (var reader = ZipStreets.GetAllDataReaderAt(count, 0))
          try
          {
            //DB.VoteZipNewLocal.VoteZipNewLocalDb.StartTransaction();
            while (reader.Read())
            {
              ZipStreetsTemp.Insert(
                zipCode: reader.ZipCode, updateKey: reader.UpdateKey,
                directionPrefix: reader.DirectionPrefix,
                streetName: reader.StreetName, streetSuffix: reader.StreetSuffix,
                directionSuffix: reader.DirectionSuffix,
                primaryLowNumber: reader.PrimaryLowNumber,
                primaryHighNumber: reader.PrimaryHighNumber,
                primaryOddEven: reader.PrimaryOddEven,
                buildingName: reader.BuildingName,
                secondaryType: reader.SecondaryType,
                secondaryLowNumber: reader.SecondaryLowNumber,
                secondaryHighNumber: reader.SecondaryHighNumber,
                secondaryOddEven: reader.SecondaryOddEven,
                metaphone: reader.Metaphone, stateCode: reader.StateCode,
                congress: reader.Congress, stateSenate: reader.StateSenate,
                stateHouse: reader.StateHouse, county: reader.County);
              count++;
              if (count % 10000 == 0)
                //DB.VoteZipNewLocal.VoteZipNewLocalDb.Commit();
                //DB.VoteZipNewLocal.VoteZipNewLocalDb.StartTransaction();
                Console.WriteLine($"ZipStreets: {count}");
            }
            done = true;
            //DB.VoteZipNewLocal.VoteZipNewLocalDb.Commit();
            Console.WriteLine($"ZipStreets: {count}");
          }
          catch
          {
            //DB.VoteZipNewLocal.VoteZipNewLocalDb.Commit();
          }
    }

    public static void Process()
    {
      var commonZipUpdateTime = LastUpdate.GetLastUpdateTime(DateTime.MinValue);
      var localZipUpdateTime =
        DB.VoteZipNewLocal.LastUpdate.GetLastUpdateTime(DateTime.MinValue);
      if (commonZipUpdateTime > localZipUpdateTime)
      {
        UpdateLocalZip();
        UpdateLocalTimestamp(commonZipUpdateTime);
      }
      else
        Console.WriteLine($"No update needed. Common update time: {commonZipUpdateTime}." +
          $" Local update time: {localZipUpdateTime}");
    }

    private static void UpdateLocalZip()
    {
      //CopyZipCitiesDownloaded();
      //CopyZipSingleUszd();
      CopyZipStreets();
    }

    private static void UpdateLocalTimestamp(DateTime commonZipUpdateTime)
    {
      if (DB.VoteZipNewLocal.LastUpdate.Exists())
        DB.VoteZipNewLocal.LastUpdate.UpdateLastUpdateTime(commonZipUpdateTime);
      else
        DB.VoteZipNewLocal.LastUpdate.Insert(lastUpdateTime: commonZipUpdateTime);
    }
  }
}