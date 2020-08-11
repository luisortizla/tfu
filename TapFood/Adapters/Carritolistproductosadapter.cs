using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using TapFood.Entidades;

namespace TapFood.Adapters
{
    public class Carritolistproductosadapter:BaseAdapter<Pedidoadap>
    {
        public List<Pedidoadap> productos;
        Activity context;

        public Carritolistproductosadapter(Activity context, List<Pedidoadap> productos):base()
        {
            this.context = context;
            this.productos = productos;
        }

        public override Pedidoadap this[int position]
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

            view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.carritolistproductosadapter, parent, false);

            var prods = productos[position];

            ImageView productoimagen = view.FindViewById<ImageView>(Resource.Id.imageproductcarrito);
            byte[] veremos = prods.Foto;
            Bitmap mdg = BitmapFactory.DecodeByteArray(veremos, 0, veremos.Length);
            productoimagen.SetImageBitmap(mdg);

            TextView nameproducto = view.FindViewById<TextView>(Resource.Id.nameproductocarrito);
            TextView cantidadproducto = view.FindViewById<TextView>(Resource.Id.cantidadproducto);
            TextView preciototalproducto = view.FindViewById<TextView>(Resource.Id.preciototalproducto);

            nameproducto.Text = prods.NombreProducto.ToString();
            cantidadproducto.Text = prods.Cantidad.ToString();
            preciototalproducto.Text = "$" + prods.PrecioProducto.ToString();

            return view;
        }
    }
}
