
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Mapbox.Geojson;
using Com.Mapbox.Mapboxsdk;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using Com.Mapbox.Mapboxsdk.Style.Layers;
using Com.Mapbox.Mapboxsdk.Style.Sources;
using Com.Mapbox.Services.Android.Navigation.V5.Navigation;
using MySql.Data.MySqlClient;
using Point = Com.Mapbox.Geojson.Point;


namespace TapFood.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class mapaseguimientopage : AppCompatActivity
    {
        internal static readonly string IDREPA = "";
        internal static readonly string IDPED = "";
        private MySqlConnection conn;
        TextView nombrerepartidormapa, telefonorepartidormapa, descripcionvehiculomapa;
        ImageView fotorepartidormapa;
        MapView mapView = null;
        MapboxMap mapbox = null;
        const string MAPBOX_KEY = "sk.eyJ1IjoibHVpc29ydGl6cyIsImEiOiJja2RkbWJ5cWExNnhoMnlwY3hwZXV6ZTE1In0.zdLAf030Z_wllV9R0WSwbw";



        public mapaseguimientopage()
        {
            MySqlConnectionStringBuilder con = new MySqlConnectionStringBuilder();
            con.Server = "mysql-10951-0.cloudclusters.net";
            con.Port = 10951;
            con.Database = "TapFood";
            con.UserID = "curecu";
            con.Password = "curecu123";

            conn = new MySqlConnection(con.ToString());

            conn.Open();
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            string x = "sk.eyJ1IjoibHVpc29ydGl6cyIsImEiOiJja2RkbWJ5cWExNnhoMnlwY3hwZXV6ZTE1In0.zdLAf030Z_wllV9R0WSwbw";
            Mapbox.GetInstance(this, x);
            SetContentView(Resource.Layout.mapaseguimientolayout);
            nombrerepartidormapa = FindViewById<TextView>(Resource.Id.nombrerepartidormapa);
            telefonorepartidormapa = FindViewById<TextView>(Resource.Id.telefonorepartidormapa);
            descripcionvehiculomapa = FindViewById<TextView>(Resource.Id.descripcionvehiculomapa);
            fotorepartidormapa = FindViewById<ImageView>(Resource.Id.fotorepartidormapa);
            //mapView = FindViewById<MapView>(Resource.Id.mapViewsg);
            //mapView.OnCreate(savedInstanceState);
            //mapView.GetMapAsync(this);
            var jee = Intent.GetStringExtra(IDREPA);
            //Mapbox.GetInstance(this,MAPBOX_KEY);
            GetRepartidorData(jee);
        }

        private void GetRepartidorData(string idp)
        {
            string sql = string.Format("Select * from TapFood.Repartidor where(IdRepartidor='{0}')", idp);
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            reader.Read();
            string nombre = (string)reader["NombreRepartidor"];
            double telefono = (double)reader["TelefonoRepartidor"];
            string descripcionvehiculo = (string)reader["DescripcionVehiculo"];
            byte[] foto = reader["FotoRepartidor"] as byte[];

            nombrerepartidormapa.Text = nombre;
            telefonorepartidormapa.Text = telefono.ToString();
            descripcionvehiculomapa.Text = descripcionvehiculo;
            Bitmap mdg = BitmapFactory.DecodeByteArray(foto, 0, foto.Length);
            fotorepartidormapa.SetImageBitmap(mdg);
            BuildingRouteAsync();
        }

        private async void BuildingRouteAsync()
        {
            var id = Intent.GetStringExtra(IDPED);
            string sql = string.Format("Select * From TapFood.Pedido Where(IdPedido='{0}') Limit 1", id);
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            reader.Read();
            double latusr = (double)reader["LatitudUsuario"];
            double lngusr = (double)reader["LongitudUsuario"];
            double latrep = (double)reader["LatitudRepartidor"];
            double lngrep = (double)reader["LongitudRepartidor"];

            MapboxNavigation navigation = new MapboxNavigation(this, MAPBOX_KEY);

            Point origin = Point.FromLngLat(lngusr, latusr);
            Point destination = Point.FromLngLat(lngrep, latrep);

            var response = await NavigationRoute
                .GetBuilder()
                .AccessToken(Mapbox.AccessToken)
                .Origin(origin)
                .Destination(destination)
                .Build()
                .GetRouteAsync();

            System.Diagnostics.Debug.WriteLine(response);
        }


        /*public void OnMapReady(MapboxMap mapbox)
        {
            this.mapbox = mapbox;
            mapbox.SetStyle("mapbox://styles/luisortizs/ckdeotksa59vs1imw35jiqemz");
            var id = Intent.GetStringExtra(IDPED);
            string sql = string.Format("Select * From TapFood.Pedido Where(IdPedido='{0}') Limit 1", id);
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            reader.Read();
            double latusr = (double)reader["LatitudUsuario"];
            double lngusr = (double)reader["LongitudUsuario"];
            double latrep = (double)reader["LatitudRepartidor"];
            double lngrep = (double)reader["LongitudRepartidor"];

            MarkerOptions marker1 = new MarkerOptions();
            marker1.SetPosition(new LatLng(latusr, lngusr));
            mapbox.AddMarker(marker1);

            MarkerOptions marker2 = new MarkerOptions();
            marker2.SetPosition(new LatLng(latrep, lngrep));
            mapbox.AddMarker(marker2);

            Point point1 = Point.FromLngLat(latusr, lngusr);
            Point point2 = Point.FromLngLat(latrep, lngrep);
                  
            
        }

        

        

        protected override void OnStart()
        {
            base.OnStart();
            mapView?.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
            mapView?.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
            mapView?.OnPause();
        }

        protected override void OnStop()
        {
            base.OnStop();
            mapView?.OnStop();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mapView?.OnSaveInstanceState(outState);
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mapView?.OnLowMemory();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            mapView?.OnDestroy();
        }*/
    }
}
