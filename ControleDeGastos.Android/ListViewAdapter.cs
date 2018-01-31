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
using Java.Lang;

namespace ControleDeGastos.Android
{
    public class ListViewAdapter : BaseExpandableListAdapter
    {
        private readonly List<ListViewGroup> grupos;
        private readonly Activity context;

        public ListViewAdapter(Activity context, List<ListViewGroup> grupos) : base()
        {
            this.context = context;
            this.grupos = grupos;
        }

        public override int GroupCount
        {
            get
            {
                return grupos.Count;
            }
        }

        public override bool HasStableIds
        {
            get
            {
                return true;
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return grupos[groupPosition].Gastos.Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View view = convertView;
            var gasto = grupos[groupPosition].Gastos[childPosition];

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ListItemRow, null);
            }
            view.FindViewById<TextView>(Resource.Id.Valor).Text = string.Format("{0:c}", gasto.Valor);
            view.FindViewById<TextView>(Resource.Id.NomeEstabelecimento).Text = gasto.Estabelecimento.Nome;

            return view;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return null;
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View view = convertView;
            var item = grupos[groupPosition];

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ListItemGroupHeaderRow, null);
            }
            view.FindViewById<TextView>(Resource.Id.Data).Text = string.Format("{0:d}", item.Data);

            return view;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}