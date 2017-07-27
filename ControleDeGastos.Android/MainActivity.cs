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
            var culture = new System.Globalization.CultureInfo("de-DE");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            base.OnCreate(bundle);

            var gastos = CarregarGastos();

            SetContentView(Resource.Layout.Main);

            var listViewGastos = FindViewById<ListView>(Resource.Id.listViewGastos);
            var listViewItems = PrepararListViewItems(gastos);
            listViewGastos.Adapter = new ListViewAdapter(this, listViewItems);
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

        private List<ListViewItem> PrepararListViewItems(List<Models.Gasto> gastos)
        {
            var listViewItems = new List<ListViewItem>();

            if (gastos.Any())
            {
                var gastosOrdenados = gastos.OrderBy(g => g.Data).ToList();
                var primeiroGasto = gastosOrdenados.First();

                listViewItems.Add(new ListViewItem() { Header = true, Data = primeiroGasto.Data });
                listViewItems.Add(new ListViewItem() { IdGasto = primeiroGasto.Id, Data = primeiroGasto.Data, NomeEstabelecimento = primeiroGasto.Estabelecimento.Nome, Valor = primeiroGasto.Valor });

                var gastoAnterior = primeiroGasto;
                for (int c = 1; c <= gastosOrdenados.Count - 1; c++)
                {
                    var gastoAtual = gastosOrdenados[c];

                    if (gastoAtual.Data.Date != gastoAnterior.Data.Date)
                    {
                        listViewItems.Add(new ListViewItem() { Header = true, Data = gastoAtual.Data });
                    }

                    listViewItems.Add(new ListViewItem() { IdGasto = gastoAtual.Id, Data = gastoAtual.Data, NomeEstabelecimento = gastoAtual.Estabelecimento.Nome, Valor = gastoAtual.Valor });

                    gastoAnterior = gastoAtual;
                }
            }

            return listViewItems;
        }
    }
}

