using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ControleDeGastos.Android
{
    [Activity(Label = "Controle de Gastos", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var gastos = CarregarGastos();
            var gastosFormatados = gastos.Select(g => string.Format("{0:d} - {1}: {2:n2}", g.Data, g.Estabelecimento.Nome, g.Valor)).ToArray();

            SetContentView(Resource.Layout.Main);

            var listViewGastos = FindViewById<ListView>(Resource.Id.listViewGastos);
            listViewGastos.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleListItem1, gastosFormatados);
        }

        private List<Models.Gasto> CarregarGastos()
        {
            var gastos = new List<Models.Gasto>();

            var restaurante = new Models.Estabelecimento() { Id = 1, Nome = "Restaurante" };
            var padaria = new Models.Estabelecimento() { Id = 2, Nome = "Padaria" };
            gastos.Add(new Models.Gasto() { Id = 1, Data = DateTime.Now, Estabelecimento = restaurante, Valor = 15 });
            gastos.Add(new Models.Gasto() { Id = 2, Data = DateTime.Now.AddDays(1), Estabelecimento = padaria, Valor = 3 });

            return gastos;
        }
    }
}

