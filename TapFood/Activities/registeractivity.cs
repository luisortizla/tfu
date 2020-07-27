
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

namespace TapFood.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class registeractivity : AppCompatActivity
    {
        EditText idcreatetxt, namecreatetxt, mailcreatetxt, phonecreatetxt, passcreatetxt;
        Button registertbn;

        bool resultado;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.registerpage);

            idcreatetxt = FindViewById<EditText>(Resource.Id.idcreatetxt);
            namecreatetxt = FindViewById<EditText>(Resource.Id.namecreatetxt);
            mailcreatetxt = FindViewById<EditText>(Resource.Id.mailcreatetxt);
            phonecreatetxt = FindViewById<EditText>(Resource.Id.phonecreatetxt);
            passcreatetxt = FindViewById<EditText>(Resource.Id.passcreatetxt);
            registertbn = FindViewById<Button>(Resource.Id.registerbtn);

            registertbn.Click += Registerbtn_Click;

        }

        private void Registerbtn_Click(object sender, EventArgs e)
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
      
                
                string verid = string.Format("Select * From `TapFood`.`Usuario` Where (`IdUsuario`) = ('{0}')", idcreatetxt.Text);
                MySqlCommand veriduser = new MySqlCommand(verid, con);
                MySqlDataReader veridread;
                veridread = veriduser.ExecuteReader();
            if (veridread.HasRows)
            {
                Toast.MakeText(this, "El Usuario ya esta registrado, intenta con otro", ToastLength.Long).Show();
            }
            else
            {
                veridread.Close();
                string vermail = string.Format("Select * From `TapFood`.`Usuario` Where (`EmailUsuario`) = ('{0}')", mailcreatetxt.Text);
                MySqlCommand vermailuser = new MySqlCommand(vermail, con);
                MySqlDataReader vermailread;
                vermailread = vermailuser.ExecuteReader();
                if (vermailread.HasRows)
                {
                    Toast.MakeText(this, "El correo electronico ya esta registrado, intenta con otro", ToastLength.Long).Show();
                }
                else
                {
                    vermailread.Close();
                    string verphone = string.Format("Select * From `TapFood`.`Usuario` Where (`TelefonoUsuario`) = ('{0}')", phonecreatetxt.Text);
                    MySqlCommand verphoneuser = new MySqlCommand(verphone, con);
                    MySqlDataReader verphoneread;
                    verphoneread = verphoneuser.ExecuteReader();
                    if (verphoneread.HasRows)
                    {
                        Toast.MakeText(this, "El numero telefonico ya esta registrado, intenta con otro", ToastLength.Long).Show();
                    }
                    else
                    {
                        verphoneread.Close();
                        try
                        {
                            string sql2 = string.Format("INSERT INTO `TapFood`.`Usuario` (`IdUsuario`, `NombreUsuario`, `EmailUsuario`, `TelefonoUsuario`, `ContraseñaUsuario` ) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}') ", idcreatetxt.Text, namecreatetxt.Text, mailcreatetxt.Text, phonecreatetxt.Text, passcreatetxt.Text);

                            MySqlCommand logincmdregister = new MySqlCommand(sql2, con);
                            logincmdregister.ExecuteNonQuery();


                        }
                        catch
                        {

                        }
                    }

                }

            }
        
         }
    }
}

            

                /*INSERT INTO `TapFood`.`Usuario` (`IdUsuario`, `NombreUsuario`, `EmailUsuario`, `TelefonoUsuario`, `ContraseñaUsuario`) VALUES ('abigailort', 'Abigail Ortiz', 'tengotutempo@hotmail.com', '9621512095', 'abiortiz123');
                string sql2 = string.Format("INSERT INTO `TapFood`.`Usuario` (`IdUsuario`, `NombreUsuario`, `EmailUsuario`, `TelefonoUsuario`, `ContraseñaUsuario` ) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}') ", idcreatetxt.Text, namecreatetxt.Text, mailcreatetxt.Text, phonecreatetxt.Text, passcreatetxt.Text);
                
                MySqlCommand logincmdregister = new MySqlCommand(sql2, con);
                logincmdregister.ExecuteNonQuery();
                
                Toast.MakeText(this, "Ya estas registrado!!", ToastLength.Long).Show();
                 }
            catch 
            {
                  Toast.MakeText(this, "Por favor verifica tus datos y vuelve a intentarlo.", ToastLength.Long).Show();
       
            }*/

            
    
    
