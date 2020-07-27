using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

using System;
using MySql.Data.MySqlClient;
using TapFood.Activities;

namespace TapFood
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        EditText usertxt, passtxt;
        Button loginbtn, loginregisterbtn, passforgotbtn;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            usertxt = FindViewById<EditText>(Resource.Id.usertxt);
            passtxt = FindViewById<EditText>(Resource.Id.passtxt);
            loginbtn = FindViewById<Button>(Resource.Id.loginbtn);
            loginregisterbtn = FindViewById<Button>(Resource.Id.loginregisterbtn);
            passforgotbtn = FindViewById<Button>(Resource.Id.passforgotbtn);

            loginregisterbtn.Click += Loginregisterbtn_Click;
            loginbtn.Click += Loginbtn_Click;
            passforgotbtn.Click += Passforgotbtn_Click;
        }

        private void Passforgotbtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Loginregisterbtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(registeractivity));
        }

        private void Loginbtn_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnectionStringBuilder b = new MySqlConnectionStringBuilder();
                b.Server = "mysql-10068-0.cloudclusters.net";
                b.Port = 10112;
                b.Database = "TapFood";
                b.UserID = "tfadmin";
                b.Password = "tfadmin123";
                
                var con1 = b.ToString();
                MySqlConnection con = new MySqlConnection(con1);
                con.Open();
                string sql1 = string.Format("Select * From TapFood.Usuario Where (IdUsuario, ContraseñaUsuario) =('{0}','{1}')", usertxt.Text, passtxt.Text);
                MySqlCommand logincmd = new MySqlCommand(sql1, con);
                MySqlDataReader logininsert;
                logininsert = logincmd.ExecuteReader();
                if (logininsert.HasRows)
                {
                    Toast.MakeText(this, "Has ingresado!", ToastLength.Long).Show();
                    StartActivity(typeof(registeractivity));
                }
                else
                {
                    Toast.MakeText(this, "Tus datos son errones, revisalor por favor.", ToastLength.Long).Show();
                }
             
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show(); ;
            }

        }
    }
}
