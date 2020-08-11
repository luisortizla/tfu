using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Preferences;
using Android.Util;
using Android.Views;
using Android.Widget;
using TapFood.Activities;

namespace TapFood.Adapters
{
    public class foodlistadapter : BaseAdapter<Producto>
    {
        public List<Producto> productos;
        Listfoodpage context;

        public foodlistadapter(Listfoodpage context, List<Producto> productos)
        {
            this.context = context;
            this.productos = productos;
        }

        public override Producto this[int position]
        {
            get { return productos[position]; }
        }

        public override int Count
        {
            get { return productos.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.plazafoodlistadapterlayout, parent, false);

            var prod = productos[position];

            ImageView prodimage = view.FindViewById<ImageView>(Resource.Id.fotoproductoiv);
            //var imagebit = GetBitmap(prod.FotoProducto);
            //prodimage.SetImageBitmap(imagebit);
            byte[] veremos = prod.FotoProducto;
            Bitmap mdg = BitmapFactory.DecodeByteArray(veremos, 0, veremos.Length);
            prodimage.SetImageBitmap(mdg);

            TextView prodnombre = view.FindViewById<TextView>(Resource.Id.productonametv);
            prodnombre.Text = prod.NombreProducto;

            TextView proddescripcion = view.FindViewById<TextView>(Resource.Id.productdescriptiontv);
            proddescripcion.Text = prod.Descripcion;

            TextView proddeadline = view.FindViewById<TextView>(Resource.Id.deadlinetv);
            proddeadline.Text = prod.TiempoEntrega.ToString() + " Min.";

            TextView proddescuento = view.FindViewById<TextView>(Resource.Id.descuentotv);
            proddescuento.Text = "Descuento " + prod.Descuento.ToString() + "%";

            int descuento = (Int32)prod.Descuento;
            int precio = (Int32)prod.PrecioProducto;
            float coner = precio / 100;
            float atrio = coner * descuento;
            float preciodescuent = precio - atrio;

            if (descuento == 0)
            {

                TextView prodprecio = view.FindViewById<TextView>(Resource.Id.pricetv);
                prodprecio.Text = "$" + prod.PrecioProducto.ToString();
            }
            else
            {
                TextView prodprecio = view.FindViewById<TextView>(Resource.Id.pricetv);
               // int precio = (Int32)prod.PrecioProducto;
                //int final = precio - (precio / 100 * descuento);
                prodprecio.SetTextColor(Color.Red);
                prodprecio.Text = preciodescuent.ToString();

            }

         
            Button button = view.FindViewById<Button>(Resource.Id.agregarbtn);
            button.SetTag(Resource.Id.agregarbtn, position);
            button.Click += delegate
            {

                var election = productos[position];
                Dialog popup = new Dialog(context);
                popup.SetContentView(Resource.Layout.popupfoodselectlayout);
                popup.Window.SetSoftInputMode(SoftInput.AdjustResize);
                popup.Show();
                popup.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
                var image = popup.FindViewById<ImageView>(Resource.Id.fotoproductopopup);
                var productoname = popup.FindViewById<TextView>(Resource.Id.nameproducto);
                var descr = popup.FindViewById<TextView>(Resource.Id.descripcionpopup);
                var agregar = popup.FindViewById<Button>(Resource.Id.agregarproducto);
                var quitar = popup.FindViewById<Button>(Resource.Id.quitarproducto);
                var cantidad = popup.FindViewById<TextView>(Resource.Id.cantidadpedido);
                var agregarcarrito = popup.FindViewById<Button>(Resource.Id.agregaracarrito);

                byte[] imgpopu = election.FotoProducto;
                Bitmap bitm = BitmapFactory.DecodeByteArray(imgpopu, 0, imgpopu.Length);
                image.SetImageBitmap(bitm);

                productoname.Text = election.NombreProducto;
                descr.Text = election.Descripcion;

                int x = 0;
                cantidad.Text = x.ToString();

                agregar.Click += delegate
                {
                    if (cantidad.Text == "0")
                    {
                        cantidad.Text = "0";
                    }
                    else
                    {
                        int y = x--;
                        cantidad.Text = y.ToString();
                    }

                };
                quitar.Click += delegate
                {

                    int y = x++;
                    cantidad.Text = y.ToString();

                };
                agregarcarrito.Click += delegate
                {
                    string idproducto = election.IdProducto;
                    string nombreproducto = election.NombreProducto;
                    float precioproducto = election.PrecioProducto;
                    string idrestaurante = election.IdRestaurante;
                    int cantidadproducto = Int32.Parse(cantidad.Text.ToString());
                    byte [] fotoproudcto = election.FotoProducto as byte[];
                    string please = Base64.EncodeToString(fotoproudcto, Base64Flags.Default);

                    ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(context);
                    ISharedPreferencesEditor edit = preff.Edit();
                    edit.PutString("IDPRODUCTO", idproducto);
                    edit.PutString("IDRESTAURANTE", idrestaurante);
                    edit.PutString("NOMBREPRODUCTO", nombreproducto);
                    edit.PutFloat("PRECIOPRODUCTO", precioproducto);
                    edit.PutInt("CANTIDADPRODUCTO", cantidadproducto);
                    edit.PutString("FOTOPRODUCTO", encodebase64(fotoproudcto));
                    edit.Apply();
                    popup.Dismiss();
                    context.addData();
                };

            };

            return view;

        }

        private string encodebase64(byte[] btp)
        {
            byte[] jam = btp;
            string please = Base64.EncodeToString(jam, Base64Flags.Default);
            return please;
        }
    }
}
