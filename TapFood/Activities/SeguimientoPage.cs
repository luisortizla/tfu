
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using Xamarin.Essentials;

namespace TapFood.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SeguimientoPage : AppCompatActivity
    {
        internal static readonly string IDPEDI = "";
        ImageView confirmadoiv, elaboracioniv, recolectadoiv, entregadoiv;
        Button seguirbtn;
        TextView direccionentrega, horaestimada, nombrerepartidor, telefonorepartidor;
        private MySqlConnection conn;

        public SeguimientoPage()
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
            SetContentView(Resource.Layout.pedidoseguimientolayout);
            confirmadoiv = FindViewById<ImageView>(Resource.Id.confirmadoiv);
            elaboracioniv = FindViewById<ImageView>(Resource.Id.elaboracioniv);
            recolectadoiv = FindViewById<ImageView>(Resource.Id.recolectadoiv);
            entregadoiv = FindViewById<ImageView>(Resource.Id.recolectadoiv);
            seguirbtn = FindViewById<Button>(Resource.Id.seguirbtn);
            direccionentrega = FindViewById<TextView>(Resource.Id.direccionentregatv);
            horaestimada = FindViewById<TextView>(Resource.Id.horaestimada);
            nombrerepartidor = FindViewById<TextView>(Resource.Id.nombrerepartidor);
            telefonorepartidor = FindViewById<TextView>(Resource.Id.telefonorepartidor);
            var idp = Intent.GetStringExtra(IDPEDI);
            seguirbtn.Click += Seguirbtn_Click;
            
            GetStatus(idp);
            
        }

        private string PassIdRepartidor(string idp)
        {
            
            string sql = string.Format("Select * From TapFood.Pedido Where(IdPedido='{0}') Limit 1", idp);
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            reader.Read();
            string idrepartidor = (string)reader["IdRepartidor"].ToString();
            return idrepartidor;
        }

        private void Seguirbtn_Click(object sender, EventArgs e)
        {
            var idp = Intent.GetStringExtra(IDPEDI);
            string idpedido = idp.ToString();
            //PassIdRepartidor(idp);
            Intent intent = new Intent(this, typeof(mapaseguimientopage));
            intent.PutExtra(mapaseguimientopage.IDREPA, PassIdRepartidor(idp));
            intent.PutExtra(mapaseguimientopage.IDPED, idpedido);
            StartActivity(intent);
        }

        private async void GetStatus(string id)
        {
            string sql = string.Format("Select * From TapFood.Pedido Where(IdPedido='{0}') Limit 1", id);
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            reader.Read();

            if (reader["NombreRepartidor"] != DBNull.Value)
            {
                string nombrerep = (string)reader["NombreRepartidor"];
                nombrerepartidor.Text = nombrerep;
            }
            if (reader["TelefonoRepartidor"] != DBNull.Value)
            {
                double telefonorep = (double)reader["TelefonoRepartidor"];
                telefonorepartidor.Text = telefonorep.ToString();
            }
            string creada = (string)reader["Creada"].ToString();
            string confirmada = (string)reader["Confirmada"].ToString();
            string recolectada = (string)reader["Recolectada"].ToString();
            string entregada = (string)reader["Entregada"].ToString();
            double longitud = (double)reader["LongitudUsuario"];
            double latitud = (double)reader["LatitudUsuario"];
            string date = (string)reader["Creada"];
            DateTime hora = Convert.ToDateTime(date);
            int x = 45;
            var placemarks = await Geocoding.GetPlacemarksAsync(latitud, longitud);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                var geocodeAddress  = direccionentrega.Text = placemark.Thoroughfare + ", " + placemark.SubThoroughfare + ", " + placemark.SubLocality + ", " + placemark.Locality;
            }
            DateTime  hestimada = hora.AddMinutes(x);
            var se= hestimada.ToString("hh: mm");
            horaestimada.Text = "Hora estimada de entrega\n" + se;
            

            if(confirmada.Length>2)
            {
                confirmadoiv.SetImageResource(Resource.Drawable.okicon2);
            }
            if (recolectada.Length > 2)
            {
                elaboracioniv.SetImageResource(Resource.Drawable.okicon2);
                recolectadoiv.SetImageResource(Resource.Drawable.okicon2);
            }
            if (entregada.Length > 2)
            {
                recolectadoiv.SetImageResource(Resource.Drawable.okicon2);
            }
            }
    }
}
