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

namespace ControleDeGastos.Android.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public Estabelecimento Estabelecimento { get; set; }
        public decimal Valor { get; set; }
    }
}