
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using TapFood.Adapters;
using TapFood.Entidades;

namespace TapFood.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Foodzonepage : AppCompatActivity
    {
        public List<Plaza> plazas = new List<Plaza>();
        ListView listfoodzone;
        private MySqlConnection conn;

        public Foodzonepage()
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
            SetContentView(Resource.Layout.foodzonelayout);
            listfoodzone = FindViewById<ListView>(Resource.Id.listplazas);
            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = preff.GetString("CiudadUsuario", "");

            string sql = string.Format("Select * from TapFood.Plaza where (Ciudad='{0}')", jee.ToString());
            MySqlCommand search = new MySqlCommand(sql, conn);
            MySqlDataReader reader;
            reader = search.ExecuteReader();
            while (reader.Read())
            {
                
                Plaza plaza = new Plaza();
                plaza.IdPlaza = Convert.ToInt32(reader["IdPlaza"].ToString());
                plaza.NombrePlaza = reader["NombrePlaza"].ToString();
                plaza.Ciudad = reader["Ciudad"].ToString();
                plaza.LogoPlaza = reader["LogoPlaza"] as byte[];
                plaza.LatitudPlaza = Convert.ToDouble(reader["LatitudPlaza"]);
                plaza.LongitudPlaza = Convert.ToDouble(reader["LongitudPlaza"]);
                plazas.Add(plaza);
            
            }

            reader.Close();
            listfoodzone.Adapter = new Listfoodzoneadapter(this, plazas);
            listfoodzone.ItemClick += Listfoodzone_ItemClick;
        }

        private void Listfoodzone_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string idplaza = plazas[e.Position].IdPlaza.ToString();
            string nombreplaza = plazas[e.Position].NombrePlaza.ToString();
            string ciudadplaza = plazas[e.Position].Ciudad.ToString();
            string latitudplaza = plazas[e.Position].LatitudPlaza.ToString();
            string longitudplaza = plazas[e.Position].LongitudPlaza.ToString();
            Intent intent = new Intent(this, typeof(Listfoodpage));
            intent.PutExtra(Listfoodpage.IDPLAZA, idplaza);
            intent.PutExtra(Listfoodpage.NOMBREPLAZA, nombreplaza);
            intent.PutExtra(Listfoodpage.CIUDADPLAZA, ciudadplaza);
            intent.PutExtra(Listfoodpage.LATITUDPLAZA, latitudplaza);
            intent.PutExtra(Listfoodpage.LONGITUDPLAZA, longitudplaza);
            StartActivity(intent);
        }
    }
}
