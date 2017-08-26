using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Client
{
    public class CLocation
    {
        private GeoCoordinateWatcher watcher;

        private static string baseUri = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";

        public static string location = string.Empty;

        private static string lat = string.Empty;
        private static string lng = string.Empty;

        public void GetLocationDataEvent()
        {
            this.watcher = new GeoCoordinateWatcher();

            this.watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);

            this.watcher.Start();
            Thread.Sleep(8000);
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            PrintPosition(e.Position.Location.Latitude, e.Position.Location.Longitude);
            // Stop receiving updates after the first one.
            this.watcher.Stop();
        }

        private void PrintPosition(double Latitude, double Longitude)
        {
            //Console.WriteLine("Latitude: {0}, Longitude {1}", Latitude, Longitude);
            RetrieveFormatedAddress(Latitude.ToString(), Longitude.ToString());
        }

        private void RetrieveFormatedAddress(string lat, string lng)
        {
            string requestUri = string.Format(baseUri, lat, lng);

            using (WebClient wc = new WebClient())
            {
                //string externalip = wc.DownloadString("http://icanhazip.com");

                string result = wc.DownloadString(requestUri);

                var xmlElm = XElement.Parse(result);
                var status = (from elm in xmlElm.Descendants() where elm.Name == "status" select elm).FirstOrDefault();

                if (status.Value.ToLower() == "ok")
                {
                    var res = (from elm in xmlElm.Descendants() where elm.Name == "formatted_address" select elm).FirstOrDefault();
                    requestUri = res.Value;
                    //Console.WriteLine("Formatted address: {0}", res.ToString());
                    location = requestUri.Split(',')[1].Trim();
                    location = Regex.Replace(location, @"[\d-]", string.Empty).Trim();
                }
            }
        }
    }
}