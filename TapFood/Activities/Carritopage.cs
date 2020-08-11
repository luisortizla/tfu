
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;


using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GoogleGson;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using TapFood.Adapters;
using TapFood.Entidades;
using AlertDialog = Android.App.AlertDialog;
using Newtonsoft.Json.Linq;
using Android.Text;

namespace TapFood.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Carritopage : AppCompatActivity    
    {
        public List<Pedidoadap> productos = new List<Pedidoadap>();
        private MySqlConnection conn;
        TextView lugarentrega, metodopago, propina, promocion, costoproductos, propinaagregada, costodeservicio, promocionagregada, totalapagar;
        Button finalizarpedido, changelugarentrega, metododepagobtn, propinabtn, promocionbtn;
        List<float> precios = new List<float>();

        public Carritopage()
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

        #region
        protected override  void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.carritolayout);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            changelugarentrega = FindViewById<Button>(Resource.Id.changelugarentrega);
            lugarentrega = FindViewById<TextView>(Resource.Id.lugarentrega);
            metododepagobtn = FindViewById<Button>(Resource.Id.metodopagobtn);
            promocionbtn = FindViewById<Button>(Resource.Id.promocionbtn);
            var productoscarrito = FindViewById<ListView>(Resource.Id.productoscarrito);
            propinabtn = FindViewById<Button>(Resource.Id.propinabtn);
            metodopago = FindViewById<TextView>(Resource.Id.metodopago);
            propina = FindViewById<TextView>(Resource.Id.propina);
            promocion = FindViewById<TextView>(Resource.Id.promocion);
            costoproductos = FindViewById<TextView>(Resource.Id.costoproductos);
            propinaagregada = FindViewById<TextView>(Resource.Id.propinagregada);
            costodeservicio = FindViewById<TextView>(Resource.Id.costoservicio);
            promocionagregada = FindViewById<TextView>(Resource.Id.promocionagregada);
            totalapagar = FindViewById<TextView>(Resource.Id.totalpagar);
            finalizarpedido = FindViewById<Button>(Resource.Id.finalizarpedidobtn);
            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            String json = preff.GetString("MyObject", "");
            Gson gson = new Gson();
            productos = JsonConvert.DeserializeObject<List<Pedidoadap>>(json);
            productoscarrito.Adapter = new Carritolistproductosadapter(this, productos);
            GetLocationAsync();
            changelugarentrega.Click += Changelugarentrega_Click;
            
            float suma = productos.Sum(productos=>productos.PrecioProducto);
            costoproductos.Text = "$" + suma.ToString()+ ".00";
            AddData(suma);
           finalizarpedido.Click += Finalizarpedido_Click1;
        }

        
        #endregion

        #region
        protected override async void OnRestart()
        {
            base.OnRestart();
            metododepagobtn.Click += Metododepagobtn_Click;
            propinabtn.Click += Propinabtn_Click;
            promocionbtn.Click += Promocionbtn_Click;
            var nomplaza = productos[0].NombrePlaza;
            var cityplaza = productos[0].Ciudad;
            string gol = string.Format("Select LatitudPlaza, LongitudPlaza from TapFood.Plaza where(NombrePlaza='{0}' and Ciudad='{1}')", nomplaza, cityplaza);
            MySqlCommand ltln = new MySqlCommand(gol, conn);
            MySqlDataReader san;
            san = ltln.ExecuteReader();
            san.Read();
             double la = (double)san["LatitudPlaza"];
             double lo = (double)san["LongitudPlaza"];
            
            ISharedPreferences prof = PreferenceManager.GetDefaultSharedPreferences(this);
            var idproduct = prof.GetString("LATITUD2", "");
            var nombreproduct = prof.GetString("LONGITUD2", "");
            double x = double.Parse(idproduct);
            double y = double.Parse(nombreproduct);
            var placemarks = await Geocoding.GetPlacemarksAsync(x, y);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                var geocodeAddress = lugarentrega.Text = placemark.Thoroughfare + ", " + placemark.SubThoroughfare + ", " + placemark.SubLocality + ", " + placemark.Locality;

                Location plaza = new Location(la, lo);
                Location user = new Location(x, y);
                double miles = Location.CalculateDistance(plaza, user, DistanceUnits.Kilometers);
                int costo = 30;

                if (miles <= 2)
                {
                    costodeservicio.Text = "$30.00";
                    float chalala = 30;
                    AddData(chalala);
                    san.Close();
                }
                else
                {
                    san.Close();
                    double resta = miles - 2;
                    double extra = Math.Truncate((resta * 100) / 100) * 2.5;
                    costodeservicio.Text = "$" + (costo + extra).ToString();
                    float pirilin = Convert.ToSingle(extra + 30);
                    AddData(pirilin);
                }
                double resta2 = miles - 2;
                double extra2 = Math.Truncate((resta2 * 100) / 100) * 2.5;
                
            }
        }
        #endregion

        #region
        private void Metododepagobtn_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("¿Que metodo de pago deaseas?");
            alert.SetButton("Efectivo", (c, ev) =>
            {
                metodopago.Text = "Efectivo";
            });
            alert.SetButton2("Tarjeta", (c, ev) =>
            {
                Dialog popup = new Dialog(this);
                popup.SetContentView(Resource.Layout.popuCardlayout);
                popup.Window.SetSoftInputMode(SoftInput.AdjustResize);
                popup.Show();
                popup.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
                var nombreimput = popup.FindViewById<EditText>(Resource.Id.nombreinput);
                var numtarjetainput = popup.FindViewById<EditText>(Resource.Id.numtarjetainput);
                var mesinput = popup.FindViewById<EditText>(Resource.Id.mesinput);
                var añoinput = popup.FindViewById<EditText>(Resource.Id.añoinput);
                var cvvinput = popup.FindViewById<EditText>(Resource.Id.cvvinput);
                var ingresartarjeta = popup.FindViewById<Button>(Resource.Id.ingresartarjeta);
                var numtarejtaview = popup.FindViewById<TextView>(Resource.Id.numtarjetaview);
                var nombreview = popup.FindViewById<TextView>(Resource.Id.nombreview);
                var mesview = popup.FindViewById<TextView>(Resource.Id.meview);
                var añoview = popup.FindViewById<TextView>(Resource.Id.añoview);
                nombreimput.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                    nombreview.Text = e.Text.ToString();
                };
                numtarjetainput.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                    numtarejtaview.Text = e.Text.ToString();
                };
                mesinput.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                    mesview.Text = e.Text.ToString();
                };
                añoinput.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                    añoview.Text = e.Text.ToString();
                };


                ingresartarjeta.Click += delegate {

                    popup.Dismiss();
                };

            });
            alert.Show();
            totalapagar.Text = precios.Sum().ToString();
        }
        #endregion


        #region
        private void Propinabtn_Click(object sender, EventArgs e)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View view = layoutInflater.Inflate(Resource.Layout.propinapopup, null);
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetView(view);
            builder.SetTitle("¿Caunto deseas agregar de propina?");
            var propinaa = view.FindViewById<EditText>(Resource.Id.lineapropina);
            builder.SetCancelable(false)
                .SetPositiveButton("Donar", (c, ev) =>
            {
                string lo = propinaa.Text;
                propina.Text = "$" + lo + ".00";
                propinaagregada.Text = "$" + lo + ".00";
                float gamesa = float.Parse(lo);
                AddData(gamesa);
            })
            .SetNegativeButton("Cancelar", (c, ev) =>
            {
                propina.Text = "$0.00";
                propinaagregada.Text = "$0.00";
            });
            AlertDialog lala = builder.Create();
            lala.Show();
            
        }
        #endregion

        #region
        private void Promocionbtn_Click(object sender, EventArgs e)
        {
            LayoutInflater inflater = LayoutInflater.From(this);
            View vv = inflater.Inflate(Resource.Layout.propinapopup, null);
            AlertDialog.Builder kola = new AlertDialog.Builder(this);
            kola.SetView(vv);
            kola.SetTitle("Por favor agrega tu promocion");
            var promo = vv.FindViewById<EditText>(Resource.Id.lineapropina);
            kola.SetCancelable(false)
                .SetPositiveButton("Agregar", (c, ev) =>
                {
                    string sql = string.Format("Select * from TapFood.Promociones where(IdPromociones = '{0}')", promo.ToString());
                    MySqlCommand loginverid = new MySqlCommand(sql, conn);
                    MySqlDataReader usr;
                    usr = loginverid.ExecuteReader();
                    if (usr.HasRows)
                    {
                        promocion.Text = (string)usr["IdPromocion"];
                        string x = (string)usr["Descuento"];
                        float cosumel = float.Parse(x);
                        AddData(cosumel);
                        Toast.MakeText(this, "Hemos agregado la promocion correctamente", ToastLength.Long).Show();
                        usr.Close();
                    }
                    else
                    {
                        AddData(0);
                        Toast.MakeText(this, "No hemos encontrado el codigo que introduciste, intenta de nuevo por favor", ToastLength.Long).Show();
                        usr.Close();
                        promocionagregada.Text = "$0.00";
                    }
                })
                .SetNegativeButton("Cancelar", (c, ev) =>
                {
                    AddData(0);
                    promocionagregada.Text = "0.00";
                    Finish();
                });
            AlertDialog cma = kola.Create();
            cma.Show();
            
        }
        #endregion

        #region
        private void Changelugarentrega_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Mappage));
        }
        #endregion

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #region
        async void GetLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);
                Console.WriteLine(location.Latitude.ToString(), location.Longitude);

                if (location != null)
                {

                    ISharedPreferences loca = PreferenceManager.GetDefaultSharedPreferences(this);
                    ISharedPreferencesEditor editable = loca.Edit();
                    editable.PutString("LATITUD", location.Latitude.ToString());
                    editable.PutString("LONGITUD", location.Longitude.ToString());
                    editable.Apply();
                }
            }

            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }
        }
        #endregion

        private void AddData(float x)
        {
            precios.Add(x);
            totalapagar.Text= totalapagar.Text ="$" + precios.Sum().ToString();
        }

        private void Finalizarpedido_Click1(object sender, EventArgs e)
        {
            ISharedPreferences prof = PreferenceManager.GetDefaultSharedPreferences(this);
            var idproduct = prof.GetString("LATITUD2", "");
            var nombreproduct = prof.GetString("LONGITUD2", "");
            Pedidoadap pedidoadap = productos.ElementAt(0);
            string no = "NO";
            for (int i = 0; i < productos.Count; i++)
            {
                pedidoadap = productos.ElementAt(i);
                string sql = string.Format("INSERT INTO `TapFood`.`Pedido` (`IdPedido`, `NombrePlaza`, `Ciudad`, `IdUsuario`, `NombreUsuario`, `IdRestaurante`, `IdProducto`, `NombreProducto`, `PrecioProducto`, `Cantidad`, `TipoDePago`, `LongitudPlaza`, `LatitudPlaza`, `LongitudUsuario`, `LatitudUsuario`, `Creada`, `Confirmada`, `Recolectada`, `Entregada`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}')", pedidoadap.IdPedido, pedidoadap.NombrePlaza, pedidoadap.Ciudad, pedidoadap.IdUsuario, pedidoadap.NombreUsuario, pedidoadap.IdRestaurante, pedidoadap.IdProducto, pedidoadap.NombreProducto, pedidoadap.PrecioProducto, pedidoadap.Cantidad, metodopago.Text, pedidoadap.LongitudPlaza, pedidoadap.LatitudPlaza, nombreproduct, idproduct, DateTime.Now.ToString(),no, no,no);
                MySqlCommand logincmdregister = new MySqlCommand(sql, conn);
                logincmdregister.ExecuteNonQuery();
                Toast.MakeText(this, "Tu pedido ha sido realizado con exito!", ToastLength.Long).Show();
                //StartActivity(typeof(MainActivity));
            }
            string data1 = pedidoadap.IdPedido;
            float data2= productos.Sum(productos => productos.PrecioProducto);
            float data3 = precios.ElementAt(1);
            float data4 = precios.ElementAt(2);
            float data5 = precios.ElementAt(3);
            float data6 = precios.Sum();
            string intocuenta = string.Format("INSERT INTO TapFood.Cuenta(IdPedido, VentaTotal, CostoServicio, Propina, Descuento, TotalAPagar) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')",data1,data2,data3,data4,data5,data6);
            MySqlCommand cmd = new MySqlCommand(intocuenta, conn);
            cmd.ExecuteNonQuery();
            Intent intent = new Intent(this, typeof(SeguimientoPage));
            intent.PutExtra(SeguimientoPage.IDPEDI, productos.ElementAt(0).IdPedido);
            StartActivity(intent);
        }
    }
}
