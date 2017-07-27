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

            SetContentView(Resource.Layout.Main);

            var listViewGastos = FindViewById<ListView>(Resource.Id.listViewGastos);
            listViewGastos.Adapter = new ListViewAdapter(this, gastos);
        }

        private List<Models.Gasto> CarregarGastos()
        {
            var estabelecimentos = new List<Models.Estabelecimento>();
            for (int c = 1; c <= 10; c++)
            {
                estabelecimentos.Add(new Models.Estabelecimento() { Id = c, Nome = string.Format("Estabelecimento {0}", c) });
            }

            var random = new Random();
            var gastos = new List<Models.Gasto>();
            for (int c = 1; c <= 15; c++)
            {
                var data = DateTime.Now.AddDays(random.Next(0, 3));
                var estabelecimento = estabelecimentos[random.Next(0, estabelecimentos.Count - 1)];
                var valor = random.Next(1, 50);
                gastos.Add(new Models.Gasto() { Id = c, Data = data, Estabelecimento = estabelecimento, Valor = valor });
            }

            return gastos;
        }
    }
}

