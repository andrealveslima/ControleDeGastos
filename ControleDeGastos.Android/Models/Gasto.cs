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
    [Table("Gasto")]
    public class Gasto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public DateTime Data { get; set; }
        [Ignore]
        public Estabelecimento Estabelecimento { get; set; }
        [NotNull]
        public int EstabelecimentoId { get; set; }
        [NotNull]
        public decimal Valor { get; set; }
    }
}