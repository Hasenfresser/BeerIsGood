using System;
using System.Collections.Generic;
using Plugin.Geolocator;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BeerIsGood.Views
{
    public partial class Home : ContentPage
    {
        ScrollView ScrollView_Direct_Search = new ScrollView();
        StackLayout StackLayout_Direct_Search = new StackLayout();

        Label Label_Status = new Label();
        Entry Entry_Keyword = new Entry();
        Picker Picker_SearchType = new Picker();
        Button Button_Search = new Button();
        Label Label_SearchResults = new Label();

        ListView ListView_SearchResults = new ListView();

        Classes.Facilities SearchedData = new Classes.Facilities();

        public Home()
        {
            // InitializeComponent();

            StackLayout_Direct_Search.Spacing = 10;
            StackLayout_Direct_Search.Orientation = StackOrientation.Vertical;

            Label_Status.TextColor = Color.White;
            Label_Status.FontSize = 15;
            Label_Status.Text = "Suchbegriff eingeben:";

            Entry_Keyword.Text = string.Empty;
            Entry_Keyword.FontSize = 20;

            Picker_SearchType.Items.Add("Entfernung (benötigt GPS)");
            Picker_SearchType.Items.Add("Bewertung");
            Picker_SearchType.SelectedIndex = 0;

            Button_Search.FontSize = 20;
            Button_Search.BackgroundColor = Color.Gray;
            Button_Search.TextColor = Color.White;
            Button_Search.Text = "Suchen";
            Button_Search.Clicked += new EventHandler(this.Button_Search_Clicked);

            Label_SearchResults.TextColor = Color.White;
            Label_SearchResults.FontSize = 15;

            ListView_SearchResults.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(ListView_SearchResults_Clicked);
            ListView_SearchResults.HasUnevenRows = true;
            ListView_SearchResults.VerticalOptions = LayoutOptions.StartAndExpand;
            ListView_SearchResults.HorizontalOptions = LayoutOptions.Fill;

            StackLayout_Direct_Search.Children.Add(Label_Status);
            StackLayout_Direct_Search.Children.Add(Entry_Keyword);
            StackLayout_Direct_Search.Children.Add(Picker_SearchType);
            StackLayout_Direct_Search.Children.Add(Button_Search);
            StackLayout_Direct_Search.Children.Add(Label_SearchResults);
            StackLayout_Direct_Search.Children.Add(ListView_SearchResults);

            ScrollView_Direct_Search.Content = StackLayout_Direct_Search;
            Content = ScrollView_Direct_Search;
        }

        private async void Button_Search_Clicked(object Sender1, EventArgs EventArgs1)
        {
            Label_SearchResults.Text = string.Empty;

            string Keyword = Entry_Keyword.Text.Trim();

            if (Keyword == string.Empty)
            {
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Kein Suchbegriff eingegeben!";
            }
            else
            {
                Label_Status.TextColor = Color.Yellow;
                Label_Status.Text = "Es wird gesucht...";

                Entry_Keyword.IsEnabled = false;
                Button_Search.IsEnabled = false;
                Picker_SearchType.IsEnabled = false;

                if (Picker_SearchType.SelectedIndex == 0)
                    SearchedData = await sortedDistanceList(Entry_Keyword.Text);
                else
                    SearchedData = await sortedScoreList(Entry_Keyword.Text);

                if (SearchedData.FacilityList.Count > 0)
                {
                    Label_Status.TextColor = Color.Green;
                    Label_Status.Text = "Ergebnisse gefunden.";

                    Label_SearchResults.Text = "Ergebnisse:";

                    List<string> SearchedItems = new List<string>();

                    for (short Counter = 0; /*Counter < 5 && */Counter < SearchedData.FacilityList.Count; ++Counter)
                        SearchedItems.Add(SearchedData.FacilityList[Counter].name/* + " (" + SearchedData.FacilityList[Counter].Distance + ')'*/);

                    ListView_SearchResults.ItemsSource = SearchedItems;
                }
                else
                {
                    Label_SearchResults.Text = string.Empty;
                    ListView_SearchResults.ItemsSource = null;
                }

                Entry_Keyword.IsEnabled = true;
                Button_Search.IsEnabled = true;
                Picker_SearchType.IsEnabled = true;
            }
        }

        private async Task<Classes.Facilities> sortedScoreList(string Keyword)
        {
            Classes.Facilities Facilities = new Classes.Facilities();

            try
            {
                Facilities = await App.ConnectionService.parseXMLStringFacilities(Connection.ConnectionConstants.SearchCity, Keyword);

                if (Facilities.FacilityList[0].id == string.Empty)
                {
                    Label_Status.TextColor = Color.Red;
                    Label_Status.Text = "Keine Ergebnisse gefunden.";

                    return new Classes.Facilities();
                }

                Facilities.FacilityList.Sort(new Classes.FacilityScoreComparer());

                Classes.Facilities SortedFacilities = new Classes.Facilities();

                for (int Counter = 0; Counter < 5 && Counter < Facilities.FacilityList.Count; ++Counter)
                    if (Facilities.FacilityList[Counter].overall != "0")
                        SortedFacilities.FacilityList.Add(Facilities.FacilityList[Counter]);

                if (SortedFacilities.FacilityList.Count == 0)
                {
                    Label_Status.TextColor = Color.Red;
                    Label_Status.Text = "Keine Ergebnisse gefunden.";

                    return new Classes.Facilities();
                }

                return SortedFacilities;
            }
            catch (Exception)
            {
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Keine Internetverbindung.";

                return new Classes.Facilities();
            }
        }

        private async Task<Classes.Facilities> sortedDistanceList(string Keyword)
        {
            Classes.Facilities Facilities = new Classes.Facilities();

            try
            {
                Facilities = await App.ConnectionService.parseXMLStringFacilities(Connection.ConnectionConstants.SearchCity, Keyword);
            }
            catch (Exception)
            {
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Keine Internetverbindung.";

                return new Classes.Facilities();
            }

            if (Facilities.FacilityList[0].id == string.Empty)
            {
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Keine Ergebnisse gefunden.";

                return new Classes.Facilities();
            }

            try
            {
                getPosition();
            }
            catch (Exception)
            {
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Kein GPS-Signal!";

                return new Classes.Facilities();
            }

            Classes.Facilities SortedFacilities = new Classes.Facilities();

            for (int Counter = 0; Counter < Facilities.FacilityList.Count; ++Counter)
            {
                Facilities.FacilityList[Counter].Distance = await getDistance(Facilities.FacilityList[Counter].id);

                if (Facilities.FacilityList[Counter].Distance > 0)
                    SortedFacilities.FacilityList.Add(Facilities.FacilityList[Counter]);
            }

            SortedFacilities.FacilityList.Sort(new Classes.FacilityDistanceComparer());

            if (SortedFacilities.FacilityList.Count > 5)
                SortedFacilities.FacilityList.RemoveRange(5, SortedFacilities.FacilityList.Count - 5);

            if (SortedFacilities.FacilityList.Count == 0)
            {
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Keine Ergebnisse gefunden.";

                return new Classes.Facilities();
            }

            return SortedFacilities;
        }

        private async void getPosition()
        {
            Plugin.Geolocator.Abstractions.IGeolocator Geolocator = CrossGeolocator.Current;

            try
            {
                Geolocator.DesiredAccuracy = 10;

                App.Position = await Geolocator.GetPositionAsync(timeoutMilliseconds: 10000);

                // Status.Text = "Gefunden.";
            }
            catch (Exception)
            {
                // Status.Text = "Kein GPS";
            }
        }

        private async Task<double> getDistance(string ID)
        {
            Classes.Map Map = new Classes.Map();

            try
            {
                Map = await App.ConnectionService.parseXMLStringMap(ID);
            }
            catch (Exception)
            {
                return -1;
            }

            double MapLatitude;
            double MapLongitude;

            try
            {
                MapLatitude = Convert.ToDouble(Map.lat.Replace('.', ',')).toRadians();
            }
            catch (Exception)
            {
                return -1;
            }

            try
            {
                MapLongitude = Convert.ToDouble(Map.lng.Replace('.', ','));
            }
            catch (Exception)
            {
                return -1;
            }

            App.Position.Latitude = 50.6843234;
            App.Position.Longitude = 10.931586;
            MapLatitude = 52.543055;
            MapLongitude = 13.355910;

            double DifferenceLatitude = (MapLatitude - App.Position.Latitude).toRadians();
            double DifferenceLongitude = (MapLongitude - App.Position.Longitude).toRadians();

            double Cache = Math.Pow(Math.Sin(DifferenceLatitude / 2.0), 2) + Math.Pow(Math.Sin(DifferenceLongitude / 2.0), 2)
                         * Math.Cos(MapLatitude) * Math.Cos(App.Position.Latitude.toRadians());

            double Cache2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(Cache)));

            double Radius = 6371;

            return Radius * Cache2;
        }

        private async void ListView_SearchResults_Clicked(object Sender, SelectedItemChangedEventArgs EventArgs)
        {
            if (EventArgs.SelectedItem == null)
                return;

            for (short Counter = 0; Counter < 5 && Counter < SearchedData.FacilityList.Count; ++Counter)
            {
                if (SearchedData.FacilityList[Counter].name == EventArgs.SelectedItem.ToString())
                {
                    Classes.Map Map = new Classes.Map();
                    bool MapDataAvailable = true;

                    try
                    {
                        Map = await App.ConnectionService.parseXMLStringMap(SearchedData.FacilityList[Counter].id);

                        if (Map.lat != string.Empty && Map.lng != string.Empty)
                        {
                            double Latitide;
                            double Longitude;

                            try
                            {
                                Latitide = Convert.ToDouble(Map.lat);
                            }
                            catch (Exception)
                            {
                                MapDataAvailable = false;
                                Latitide = 0;
                            }


                            try
                            {
                                Longitude = Convert.ToDouble(Map.lng);
                            }
                            catch (Exception)
                            {
                                MapDataAvailable = false;
                                Longitude = 0;
                            }

                            if (Latitide == 0 || Longitude == 0)
                                MapDataAvailable = false;
                        }
                    }
                    catch (Exception)
                    {
                        MapDataAvailable = false;
                    }

                    string Text = string.Empty;

                    if (SearchedData.FacilityList[Counter].name != string.Empty)
                        Text += "Name: " + SearchedData.FacilityList[Counter].name + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].status != string.Empty)
                        Text += "Status: " + SearchedData.FacilityList[Counter].status + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].street != string.Empty)
                        Text += "Straße: " + SearchedData.FacilityList[Counter].street + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].city != string.Empty)
                        Text += "Ort: " + SearchedData.FacilityList[Counter].city + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].state != string.Empty)
                        Text += "Staat: " + SearchedData.FacilityList[Counter].state + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].zip != string.Empty)
                        Text += "Postleitzahl: " + SearchedData.FacilityList[Counter].zip + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].country != string.Empty)
                        Text += "Land: " + SearchedData.FacilityList[Counter].country + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].phone != string.Empty)
                        Text += "Telefon: " + SearchedData.FacilityList[Counter].phone + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].overall != "0")
                        Text += "Gesamtbewertung: " + SearchedData.FacilityList[Counter].overall + Environment.NewLine;

                    try
                    {
                        Classes.Score Score = await App.ConnectionService.parseXMLStringScore(SearchedData.FacilityList[Counter].id);

                        if (Score.selection != "0")
                            Text += "Auswahl: " + Score.selection + Environment.NewLine;

                        if (Score.service != "0")
                            Text += "Service: " + Score.service + Environment.NewLine;

                        if (Score.atmosphere != "0")
                            Text += "Atmosphäre: " + Score.atmosphere + Environment.NewLine;

                        if (Score.food != "0")
                            Text += "Essen: " + Score.food + Environment.NewLine;
                    }
                    catch (Exception)
                    {

                    }

                    if (MapDataAvailable)
                    {
                        bool Navigation = await DisplayAlert("Ergebnis-ID: " + SearchedData.FacilityList[Counter].id, Text, "Navigation", "Zurück");

                        if (Navigation)
                        {
                            Uri geoUri = new Uri("http://maps.google.com/?q=" + Map.lat + ',' + Map.lng);

                            Device.OpenUri(geoUri);
                        }
                    }
                    else
                        await DisplayAlert("Ergebnis-ID: " + SearchedData.FacilityList[Counter].id, Text, "Zurück");

                    break;
                }
            }
        }
    }
}
