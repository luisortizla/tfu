using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using TapFood.Entidades;

namespace TapFood.Adapters
{
    public class Listfoodzoneadapter:BaseAdapter<Plaza>
    {
        public List<Plaza> plazas;
        Activity context;

        public Listfoodzoneadapter(Activity context, List<Plaza> plazas) : base()
        {
            this.context = context;
            this.plazas = plazas;
        }

        public override Plaza this[int position]
        {
            get { return plazas[position]; }
        }

        public override int Count
        {
            get { return plazas.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.foodzonelistadaptlayout, parent, false);

            var plas = plazas[position];

            ImageView plazaimage = view.FindViewById<ImageView>(Resource.Id.logoplazaiv);
            TextView plazaname = view.FindViewById<TextView>(Resource.Id.plazaname);

            byte[] logo = plas.LogoPlaza;
            Bitmap mdg = BitmapFactory.DecodeByteArray(logo, 0, logo.Length);
            plazaimage.SetImageBitmap(mdg);

            plazaname.Text = plas.NombrePlaza.ToString();

            return view;
        }
    }
}
