using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PudderVarsel.Data;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PudderVarsel
{
    public sealed partial class MainPage : Page
    {
        Geolocator geo = null;
        //private Geolocator _geolocator = null;
        private CancellationTokenSource _cts = null;
        private Task<String> _locations;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Accuracy { get; set; }
        public const int MaxDistance = 100;

        public IEnumerable<Lokasjon> Lokasjoner { get; set; }
        public MainPage()
        {
            InitializeComponent();
            geo = new Geolocator();
            GetLocation();
            _locations = GetLocationsString();
            FinnPudderButton.IsEnabled = true;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //MapLocationButton.IsEnabled = true;
            //CancelGetLocationButton.IsEnabled = false;
        }

        private async Task<string> GetLocationsString()
        {
            var loc = Windows.ApplicationModel.Package.Current.InstalledLocation;

            var sampleFile = await loc.GetFileAsync("Locations.xml");

            var text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

            //if (Longitude != 0)
            //    FinnPudderButton.IsEnabled = true;
            
            return text;

        }



        private void PowderResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Use e.AddedItems to get the items that are selected in the ItemsControl.
            //selectedItems = (List<object>)e.AddedItems;
        }

        private async void GetLocation()
        {
            var pos = await geo.GetGeopositionAsync();
            Longitude = pos.Coordinate.Longitude;
            Latitude = pos.Coordinate.Latitude;
            Accuracy = pos.Coordinate.Accuracy;
            //if (!string.IsNullOrEmpty(_locations.Result))
            //    FinnPudderButton.IsEnabled = true;

        }

        private void FinnPudderButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_locations.Result) || Longitude == 0)
            {
                return;
            }

            var data = new WeatherData();
            Lokasjoner = data.GetLocationForecast(Latitude, Longitude, _locations.Result, MaxDistance, string.Empty);
            foreach (var lokasjon in Lokasjoner)
            {
                //var t = new MetClient();
                var grunndata = MetClient.GetForecast(lokasjon.Latitude.ToString(), lokasjon.Longitude.ToString());

                var weatherData = data.ProcessResponse(grunndata);

                var filteredPowderData = Utils.GetRelevantPowderData(weatherData);
                var dayByDayPowderData = Utils.GetDailyPowderData(filteredPowderData);

                var byDayPowderData = dayByDayPowderData as IList<DagligPuddervarsel> ?? dayByDayPowderData.ToList();
                lokasjon.DagligVarsel = byDayPowderData;

                var totalPrecipitation = byDayPowderData.Sum(p => p.Precipitation);
                var threeDays = byDayPowderData.Where(p => p.From < DateTime.Now.AddDays(2)).Sum(t => t.Precipitation);
                lokasjon.ThreeDaysPrecipitation = threeDays;
                lokasjon.TotalPrecipitation = totalPrecipitation;

                //location.LocationUrl = string.Format("http://maps.google.no/maps?q=N+{0}+E+{1}",
                //                                     location.Latitude.ToString(ciUs), location.Longitude.ToString(ciUs));
            }

            var sortedPowder = Lokasjoner.Where(p => p != null).OrderByDescending(p => p.TotalPrecipitation);

            ListViewLocations.ItemsSource = sortedPowder;
            
            SetButtons();

            ResetButtons();
        }


        private void SetButtons()
        {
            //MapLocationButton.IsEnabled = false;
            //CancelGetLocationButton.IsEnabled = true;

            //// Remove any previous location icon.
            //if (Map.Children.Count > 0)
            //{
            //    Map.Children.RemoveAt(0);
            //}
        }


        private void ResetButtons()
        {
            //MapLocationButton.IsEnabled = true;
            //CancelGetLocationButton.IsEnabled = false;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            base.OnNavigatingFrom(e);
        }

        
        private void ListViewLocations_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Lokasjon;
            this.Frame.Navigate(typeof(PudderDetaljer2), item.DagligVarsel);
        }
    }

}


