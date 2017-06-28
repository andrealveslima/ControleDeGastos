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
    public class Estabelecimento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}