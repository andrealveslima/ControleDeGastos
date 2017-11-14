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

using SQLite.Net.Attributes;

namespace ControleDeGastos.Android.Models
{
    [Table("Estabelecimento")]
    public class Estabelecimento
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Nome { get; set; }
    }
}