using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Services
{
	public class WeatherService
	{
        public string GetWeather()
		{
            //URL to the API:
            string url = "https://api.openweathermap.org/data/2.5/forecast?id=2610613&appid=1694568cbd10bedf6e3af69e989ef2b6";

            //Create a web client:
            WebClient client = new();

            //Get the data:
            string json = client.DownloadString(url);

            //Deserialize the data, i.e. convert the JSON to C# object:
            Root rootData = JsonConvert.DeserializeObject<Root>(json);

            //Get the specific temperature:
            double temperature = rootData.list[0].main.temp;

            //Get the specific weather description:
            string desc = rootData.list[0].weather[0].description;

            //Get the icon:
            string icon = rootData.list[0].weather[0].icon;

            //Return the weather:
            return $"{desc}, {temperature - 273.15:f1} °C";
		}
	}
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public Coord coord { get; set; }
        public string country { get; set; }
        public int population { get; set; }
        public int timezone { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Coord
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class List
    {
        public int dt { get; set; }
        public Main main { get; set; }
        public List<Weather> weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public int visibility { get; set; }
        public double pop { get; set; }
        public Sys sys { get; set; }
        public string dt_txt { get; set; }
        public Rain rain { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }
        public int humidity { get; set; }
        public double temp_kf { get; set; }
    }

    public class Rain
    {
        public double _3h { get; set; }
    }

    public class Root
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public List<List> list { get; set; }
        public City city { get; set; }
    }

    public class Sys
    {
        public string pod { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
        public double gust { get; set; }
    }


}
