using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;                     // WebUtility
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;         // DeviceInfo
using Microsoft.Maui.Devices.Sensors; // Geocoding, Location
using Microsoft.Maui.Maps;            // MapSpan, Distance
using Microsoft.Maui.Storage;         // FilePicker, FileSystem

// Alias para evitar ambigüedad con Microsoft.Maui.ApplicationModel.Map
using MapControl = Microsoft.Maui.Controls.Maps.Map;
using MapPin = Microsoft.Maui.Controls.Maps.Pin;

using ObligatorioTecnologia.Data;     // <-- tu namespace
using ObligatorioTecnologia.Modelo;   // <-- tu namespace

namespace ObligatorioTecnologia  // <-- tu namespace
{
    public partial class SponsorFormPage : ContentPage
    {
        private readonly SponsorDb _db;
        private Sponsor _editing;
        private string _currentLogoPath;
        private double? _lat, _lng;

        private readonly bool _isWindows;
        private MapControl _map;                 // mapa nativo (Android/iOS)

        // Referencias encontradas por nombre (evita depender de campos generados)
        private ContentView _nativeMapContainer;
        private ContentView _webMapContainer;
        private WebView _webMap;

        public SponsorFormPage(SponsorDb db, int? sponsorId = null)
        {
            InitializeComponent();

            // Buscar los elementos por nombre (si no existen, quedan null pero compila)
            _nativeMapContainer = this.FindByName<ContentView>("NativeMapContainer");
            _webMapContainer = this.FindByName<ContentView>("WebMapContainer");
            _webMap = this.FindByName<WebView>("WebMap");

            _db = db;
            _isWindows = DeviceInfo.Platform == DevicePlatform.WinUI;

            if (_nativeMapContainer != null) _nativeMapContainer.IsVisible = !_isWindows;
            if (_webMapContainer != null) _webMapContainer.IsVisible = _isWindows;

            if (!_isWindows && _nativeMapContainer != null)
            {
                _map = new MapControl();
                _nativeMapContainer.Content = _map;
            }

            _ = LoadAsync(sponsorId);
        }

        private async Task LoadAsync(int? id)
        {
            if (!id.HasValue) return;

            _editing = await _db.GetAsync(id.Value);
            if (_editing == null) return;

            NameEntry.Text = _editing.Name;
            AddressEntry.Text = _editing.Address;
            _currentLogoPath = _editing.LogoPath;

            if (!string.IsNullOrWhiteSpace(_currentLogoPath) && File.Exists(_currentLogoPath))
                Logo.Source = ImageSource.FromFile(_currentLogoPath);

            _lat = _editing.Latitude;
            _lng = _editing.Longitude;

            if (_lat.HasValue && _lng.HasValue)
                ShowLocation(_lat.Value, _lng.Value, _editing.Name, _editing.Address);

            DeleteButton.IsVisible = true;
        }

        private async void OnPickLogo(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions { FileTypes = FilePickerFileType.Images });
            if (result == null) return;

            var ext = Path.GetExtension(result.FileName);
            var newFile = Path.Combine(FileSystem.AppDataDirectory, $"logo_{Guid.NewGuid()}{ext}");

            using var src = await result.OpenReadAsync();
            using var dst = File.Create(newFile);
            await src.CopyToAsync(dst);

            _currentLogoPath = newFile;
            Logo.Source = ImageSource.FromFile(newFile);
        }

        private async void OnGeocode(object sender, EventArgs e)
        {
            var address = AddressEntry.Text?.Trim();
            if (string.IsNullOrWhiteSpace(address))
            {
                await DisplayAlert("Dirección", "Ingresá una dirección.", "OK");
                return;
            }

            try
            {
                var loc = (await Geocoding.GetLocationsAsync(address))?.FirstOrDefault();
                if (loc == null)
                {
                    await DisplayAlert("Mapa", "No se encontró la dirección.", "OK");
                    return;
                }

                _lat = loc.Latitude;
                _lng = loc.Longitude;

                ShowLocation(_lat.Value, _lng.Value, NameEntry.Text, address);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Geocoding", ex.ToString(), "OK");
            }
        }

        private void ShowLocation(double lat, double lng, string name, string address)
        {
            var title = string.IsNullOrWhiteSpace(name) ? "Patrocinador" : name;

            if (_isWindows)
            {
                LoadLeaflet(lat, lng, title, address ?? "");
            }
            else if (_map != null)
            {
                var location = new Location(lat, lng);
                _map.Pins.Clear();
                _map.Pins.Add(new MapPin { Label = title, Address = address, Location = location });
                _map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));
            }
        }

        private void LoadLeaflet(double lat, double lng, string name, string address)
        {
            if (_webMap == null) return;

            var safeName = WebUtility.HtmlEncode(name ?? "Patrocinador");
            var safeAddr = WebUtility.HtmlEncode(address ?? "");

            string sLat = lat.ToString(CultureInfo.InvariantCulture);
            string sLng = lng.ToString(CultureInfo.InvariantCulture);

            var html = $@"<!DOCTYPE html>
<html><head>
<meta charset='utf-8' />
<meta name='viewport' content='width=device-width, initial-scale=1.0' />
<link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css'/>
<script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
<style>html,body,#map{{height:100%; margin:0;}}</style>
</head><body>
<div id='map'></div>
<script>
var map = L.map('map').setView([{sLat},{sLng}], 16);
L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{ maxZoom: 19 }}).addTo(map);
L.marker([{sLat},{sLng}]).addTo(map).bindPopup('{safeName} - {safeAddr}');
</script>
</body></html>";

            _webMap.Source = new HtmlWebViewSource { Html = html };
        }

        private async void OnSave(object sender, EventArgs e)
        {
            var name = NameEntry.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Validación", "El nombre es obligatorio.", "OK");
                return;
            }

            var sponsor = _editing ?? new Sponsor
            {
                Name = name
            };

            sponsor.Name = name;
            sponsor.Address = AddressEntry.Text?.Trim();
            sponsor.LogoPath = _currentLogoPath;
            sponsor.Latitude = _lat;
            sponsor.Longitude = _lng;

            await _db.SaveAsync(sponsor);
            await Navigation.PopAsync();
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            if (_editing == null) return;

            var confirm = await DisplayAlert("Eliminar", "¿Eliminar este patrocinador?", "Sí", "No");
            if (!confirm) return;

            await _db.DeleteAsync(_editing);
            await Navigation.PopAsync();
        }
    }
}
