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
    public class ListViewAdapter : BaseAdapter<ListViewItem>
    {
        List<ListViewItem> items;
        Activity context;
        public ListViewAdapter(Activity context, List<ListViewItem> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override ListViewItem this[int position]
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
            if (item.Header)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ListItemGroupHeaderRow, null);
                view.FindViewById<TextView>(Resource.Id.Data).Text = string.Format("{0:d}", item.Data);
            }
            else
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ListItemRow, null);
                view.FindViewById<TextView>(Resource.Id.Valor).Text = string.Format("{0:c}", item.Valor);
                view.FindViewById<TextView>(Resource.Id.NomeEstabelecimento).Text = item.NomeEstabelecimento;
            }

            return view;
        }
    }
}