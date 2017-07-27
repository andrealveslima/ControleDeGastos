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
    public class ListViewItem
    {
        public int IdGasto { get; set; }
        public DateTime Data { get; set; }
        public string NomeEstabelecimento { get; set; }
        public decimal Valor { get; set; }
        public bool Header { get; set; }
    }
}