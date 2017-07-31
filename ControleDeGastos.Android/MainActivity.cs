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

            var listViewGastos = FindViewById<ExpandableListView>(Resource.Id.listViewGastos);
            var listViewGroups = PrepararListViewGroups(gastos);
            var adapter = new ListViewAdapter(this, listViewGroups);
            listViewGastos.SetAdapter(adapter);

            for (int group = 0; group <= adapter.GroupCount - 1; group++)
            {
                listViewGastos.ExpandGroup(group);
            }
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

        private List<ListViewGroup> PrepararListViewGroups(List<Models.Gasto> gastos)
        {
            var listViewGroups = new List<ListViewGroup>();

            if (gastos.Any())
            {
                var gastosOrdenados = gastos.OrderBy(g => g.Data);

                Models.Gasto gastoAnterior = null;
                ListViewGroup grupoAtual = null;
                foreach (var gasto in gastosOrdenados)
                {
                    if (gastoAnterior == null || gasto.Data.Date != gastoAnterior.Data.Date)
                    {
                        grupoAtual = new ListViewGroup() { Data = gasto.Data };
                        listViewGroups.Add(grupoAtual);
                    }

                    grupoAtual.ListViewItems.Add(new ListViewItem() { IdGasto = gasto.Id, NomeEstabelecimento = gasto.Estabelecimento.Nome, Valor = gasto.Valor });
                    gastoAnterior = gasto;
                }
            }

            return listViewGroups;
        }
    }
}

