
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using GoogleGson;
using MySql.Data.MySqlClient;
using TapFood.Adapters;
using TapFood.Entidades;
using Newtonsoft.Json;

namespace TapFood.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Listfoodpage : AppCompatActivity
    {
        internal static readonly string IDPLAZA = "IdPlaza";
        internal static readonly string NOMBREPLAZA = "NombrePlaza";
        internal static readonly string CIUDADPLAZA = "CiudadPlaza";
        internal static readonly string LATITUDPLAZA = "LatitudPlaza";
        internal static readonly string LONGITUDPLAZA = "LongitudPlaza";
        public List<Producto> productos = new List<Producto>();
        public List<Pedidoadap> pedidos = new List<Pedidoadap>();

        private MySqlConnection conn;
        TextView plazaname;
        ListView listproductos;
        TextView carrito;



        public Listfoodpage()
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
            SetContentView(Resource.Layout.plazafoodlistlayout);
            listproductos = FindViewById<ListView>(Resource.Id.listproductosplaza);
            plazaname = FindViewById<TextView>(Resource.Id.plazanamepage);
            carrito = FindViewById<TextView>(Resource.Id.carrito);
            var idplaza = Intent.GetStringExtra(IDPLAZA);
            var plazanombre = Intent.GetStringExtra(NOMBREPLAZA);
            ISharedPreferences prof = PreferenceManager.GetDefaultSharedPreferences(this);
            var idproduct = prof.GetString("IDPRODUCTO", "");
            var nombreproduct = prof.GetString("NOMBREPRODUCTO", "");
            var idresta = prof.GetString("IDRESTAURANTE", "");
            var precioproduct = prof.GetFloat("PRECIOPRODUCTO", 0);
            var cantidadproduct = prof.GetInt("CANTIDADPRODUCTO", 0);
            ISharedPreferences clos = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = clos.GetString("Usuario", "");

            plazaname.Text = plazanombre.ToString();

            
            
            string sql = string.Format("Select * From TapFood.Producto where (IdPlaza = '{0}')", idplaza);
            MySqlCommand exe = new MySqlCommand(sql, conn);
            MySqlDataReader reader;
            reader = exe.ExecuteReader();
            while (reader.Read())
            {
                Producto producto = new Producto();
                producto.IdProducto = reader["IdProducto"].ToString();
                producto.NombreProducto = reader["NombreProducto"].ToString();
                producto.IdRestaurante = reader["IdRestaurante"].ToString();
                producto.NombreRestaurante = reader["NombreRestaurante"].ToString();
                producto.IdPlaza = Convert.ToInt32(reader["IdPlaza"].ToString());
                producto.TipoDeComida = reader["TipoDeComida"].ToString();
                producto.Descripcion = reader["Descripcion"].ToString();
                producto.PrecioProducto = Convert.ToInt32(reader["PrecioProducto"].ToString());
                producto.TiempoEntrega = Convert.ToInt32(reader["TiempoEntrega"].ToString());
                producto.Descuento = Convert.ToInt32(reader["Descuento"].ToString());
                producto.FotoProducto = reader["FotoProducto"] as byte[];
                productos.Add(producto);
            }
            reader.Close();
            listproductos.Adapter = new foodlistadapter(this, productos);
            listproductos.ChoiceMode = ChoiceMode.None;

            string holo = "Tu carrito";
            carrito.SetText(holo, TextView.BufferType.Editable);
            carrito.TextAlignment = TextAlignment.Center;



        }



        public void addData()
        {

            ISharedPreferences clos = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = clos.GetString("Usuario", "");
            var nomu = clos.GetString("NombreUsuario", "");

            ISharedPreferences prof = PreferenceManager.GetDefaultSharedPreferences(this);
            var idproduct = prof.GetString("IDPRODUCTO", "");
            var nombreproduct = prof.GetString("NOMBREPRODUCTO", "");
            var idresta = prof.GetString("IDRESTAURANTE", "");
            var precioproduct = prof.GetFloat("PRECIOPRODUCTO", 0);
            var cantidadproduct = prof.GetInt("CANTIDADPRODUCTO", 0);
            var foto = prof.GetString("FOTOPRODUCTO", "");
            var idped = prof.GetString("IDPEDIDO", "");


            
            float precioitemtotal = (precioproduct * cantidadproduct);
            Console.WriteLine(idproduct);
            Console.WriteLine(nombreproduct);
            Console.WriteLine(idresta);
            Console.WriteLine(precioproduct);
            Console.WriteLine(cantidadproduct);
            Console.WriteLine(precioitemtotal);

            var latplaza = Intent.GetStringExtra(LATITUDPLAZA);
            var lgnplaza = Intent.GetStringExtra(LONGITUDPLAZA);
            Pedidoadap pedido = new Pedidoadap();
            pedido.IdPedido = idped;
            pedido.NombrePlaza = Intent.GetStringExtra(NOMBREPLAZA);
            pedido.Ciudad = Intent.GetStringExtra(CIUDADPLAZA);
            pedido.IdUsuario = jee;
            pedido.NombreUsuario = nomu;
            pedido.IdRestaurante = idresta;
            pedido.IdProducto = idproduct;
            pedido.NombreProducto = nombreproduct;
            pedido.PrecioProducto = precioitemtotal;
            pedido.Cantidad = cantidadproduct;
            pedido.Foto = decodeBase64(foto);
            pedido.LatitudPlaza = latplaza;
            pedido.LongitudPlaza = lgnplaza;
            pedidos.Add(pedido);
            

            ISharedPreferencesEditor edita = prof.Edit();
            edita.Remove("IDPRODUCTO");
            edita.Remove("NOMBREPRODUCTO");
            edita.Remove("IDRESTAURANTE");
            edita.Remove("PRECIOPRODUCTO");
            edita.Remove("CANTIDADPRODUCTO");
            edita.Remove("FOTOPRODUCTO");
            edita.Apply();

            float suma = pedidos.Sum(pedido => pedido.PrecioProducto);
            string holo2 = "Tu carrito         $" + suma.ToString();
            if (pedidos.Count > 0)
            {
                carrito.SetText(holo2, TextView.BufferType.Editable);
            }
            carrito.Click += delegate
            {
                var jsona = JsonConvert.SerializeObject(pedidos);

                ISharedPreferences mPrefs = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor prefsEditor = mPrefs.Edit();
                
                prefsEditor.PutString("MyObject", jsona);
                prefsEditor.Commit();
                StartActivity(typeof(Carritopage));

            };

        }

        private byte[] decodeBase64(String input)
        {
            byte[] decodedByte = Base64.Decode(input, 0);
            return decodedByte;
        }
    }
}
