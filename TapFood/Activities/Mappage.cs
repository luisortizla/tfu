using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

using Android.App;
using Android.Content;

using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Mapbox.Mapboxsdk;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using Android.Preferences;
using Com.Mapbox.Mapboxsdk.Annotations;

namespace TapFood.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Mappage : AppCompatActivity, IOnMapReadyCallback
    {
        
        MapView mapView = null;
        MapboxMap mapbox = null;
        Button selectlocationbtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            string x = "sk.eyJ1IjoibHVpc29ydGl6cyIsImEiOiJja2RkbWJ5cWExNnhoMnlwY3hwZXV6ZTE1In0.zdLAf030Z_wllV9R0WSwbw";
            Mapbox.GetInstance(this, x);
            SetContentView(Resource.Layout.mapslayout);
            selectlocationbtn = FindViewById<Button>(Resource.Id.selectlocationbtn);
            mapView = FindViewById<MapView>(Resource.Id.mapView);
            //mapView = new MapView(this);
            mapView.OnCreate(savedInstanceState);
            mapView.GetMapAsync(this);

            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var latitud = preff.GetString("LATITUD", "");
            var lognitud = preff.GetString("LONGITUD", "");

            selectlocationbtn.Click += Selectlocationbtn_Click;
        }

        public void OnMapReady(MapboxMap mapbox)
        {
            this.mapbox = mapbox;
            mapbox.SetStyle("mapbox://styles/luisortizs/ckdeotksa59vs1imw35jiqemz");

            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var latitud = preff.GetString("LATITUD", "");
            var lognitud = preff.GetString("LONGITUD", "");
            Console.WriteLine(latitud,lognitud);

            double ltd = double.Parse(latitud);
            double lng = double.Parse(lognitud);

            var position = new CameraPosition.Builder()
                           .Target(new LatLng(ltd, lng))
                           .Zoom(13)
                           .Build();

            mapbox.AnimateCamera(CameraUpdateFactory.NewCameraPosition(position));
            ISharedPreferencesEditor edita = preff.Edit();
            edita.Clear();
            edita.Apply();
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
        }

        private void Selectlocationbtn_Click(object sender, EventArgs e)
        {
            var positioncam = mapbox.CameraPosition;
            LatLng maplocation = positioncam.Target;
            double ltdd = mapbox.CameraPosition.Target.Latitude;
            double lgnn = mapbox.CameraPosition.Target.Longitude;
            
            ISharedPreferences loca = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editable = loca.Edit();
            editable.PutString("LATITUD2", ltdd.ToString());
            editable.PutString("LONGITUD2", lgnn.ToString());
            editable.Apply();
            Finish();
        }
    }
}
