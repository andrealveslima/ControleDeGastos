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
    public class ListViewGroup
    {
        public DateTime Data { get; set; }
        public List<Models.Gasto> Gastos { get; set; }

        public ListViewGroup()
        {
            Gastos = new List<Models.Gasto>();
        }
    }
}