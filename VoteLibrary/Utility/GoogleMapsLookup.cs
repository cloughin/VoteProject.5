using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using static System.String;

namespace Vote
{
  public static class GoogleMapsLookup
  {
    private const string ServerKey = "AIzaSyBi_U-gCQzCHhvUOIUpjLRVXslvpgVtcAU";
    //private const string ServerKey = "AIzaSyBSJeY9MAMhf8AdUjzFl1apWzWshauqQFs";

    public static GoogleMapsData Lookup(string streetAddress, string city, string state, string zip5)
    {
      var url = "https://maps.googleapis.com/maps/api/geocode/json?address=" +
        $"{HttpUtility.UrlEncode($"{streetAddress}, {city}, {state} {zip5}")}" +
        $"&region=us&key={ServerKey}";
      var result = new GoogleMapsData();
      try
      {
        using (var client = new WebClient())
        {
          var json = client.DownloadString(url);
          var serializer = new JavaScriptSerializer();
          var google = serializer.Deserialize<GoogleMapsResult>(json);
          result.Status = google.status;
          if (result.Status == "OK" && google.results.Length != 1)
            result.Status = "ambiguous";
          if (result.Status == "OK")
          {
            result.StreetNumber = google.GetAddressComponent("street_number", false);
            result.Street = google.GetAddressComponent("route", true);
            result.City = google.GetAddressComponent("locality", true);
            if (IsNullOrWhiteSpace(result.City))
              result.City = google.GetAddressComponent("sublocality", true);
            if (IsNullOrWhiteSpace(result.City))
              result.City = google.GetAddressComponent("sublocality_level_1", true);
            result.State = google.GetAddressComponent("administrative_area_level_1", false);
            result.Zip5 = google.GetAddressComponent("postal_code", false);
            result.Zip4 = google.GetAddressComponent("postal_code_suffix", false);
            result.Latitude = Math.Round(google.results[0].geometry.location.lat ?? 0.0, 6);
            result.Longitude = Math.Round(google.results[0].geometry.location.lng ?? 0.0, 6);
          }
        }
      }
      catch (Exception ex)
      {
        result.Status = ex.Message;
      }
      return result;
    }
  }

  // ReSharper disable ClassNeverInstantiated.Global
  // ReSharper disable InconsistentNaming
  // ReSharper disable UnusedMember.Global
  // ReSharper disable UnassignedField.Global

  public class GoogleAddressComponent
  {
    public string[] types;
    public string long_name;
    public string short_name;
  }

  public class GoogleAddressInfo
  {
    public string formatted_address;
    public GoogleAddressComponent[] address_components;
    public GoogleGeometry geometry;
  }

  public class GoogleLocation
  {
    public double? lat;
    public double? lng;
  }

  public class GoogleGeometry
  {
    public GoogleLocation location;
  }

  public class GoogleMapsResult
  {
    public string status;
    public GoogleAddressInfo[] results;

    public string GetAddressComponent(string type, bool useLongName)
    {
      Debug.Assert(results.Length == 1);
      var component =
        results[0].address_components.FirstOrDefault(
          c => c.types.Contains(type));
      return component == null
        ? null
        : (useLongName ? component.long_name : component.short_name);
    }
  }

  // ReSharper restore UnassignedField.Global
  // ReSharper restore UnusedMember.Global
  // ReSharper restore InconsistentNaming
  // ReSharper restore ClassNeverInstantiated.Global

  public class GoogleMapsData
  {
    public string Status;
    public string StreetNumber;
    public string Street;
    public string City;
    public string State;
    public string Zip5;
    public string Zip4;
    public double Latitude;
    public double Longitude;
  }
}
