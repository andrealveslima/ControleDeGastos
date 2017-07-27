using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ControleDeGastos.Android
{
    public class ListViewAdapter : BaseAdapter<Models.Gasto>
    {
        List<Models.Gasto> items;
        Activity context;
        public ListViewAdapter(Activity context, List<Models.Gasto> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Models.Gasto this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.ListItemRow, null);
            view.FindViewById<TextView>(Resource.Id.Valor).Text = string.Format("{0:c}", item.Valor);
            view.FindViewById<TextView>(Resource.Id.Estabelecimento).Text = item.Estabelecimento.Nome;
            view.FindViewById<TextView>(Resource.Id.Data).Text = string.Format("{0:d}", item.Data);
            return view;
        }
    }
}