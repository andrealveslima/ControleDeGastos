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
    public class Dados
    {
        public List<Models.Estabelecimento> Estabelecimentos { get; private set; }
        public List<Models.Gasto> Gastos { get; private set; }

        public Dados()
        {
            Estabelecimentos = new List<Models.Estabelecimento>();
            Gastos = new List<Models.Gasto>();
        }

        public void Carregar()
        {
            Estabelecimentos.Clear();
            Gastos.Clear();

            for (int c = 1; c <= 10; c++)
            {
                Estabelecimentos.Add(new Models.Estabelecimento() { Id = c, Nome = string.Format("Estabelecimento {0}", c) });
            }

            var random = new Random();
            for (int c = 1; c <= 15; c++)
            {
                var data = DateTime.Now.AddDays(random.Next(0, 3));
                var estabelecimento = Estabelecimentos[random.Next(0, Estabelecimentos.Count - 1)];
                var valor = random.Next(1, 50);
                Gastos.Add(new Models.Gasto() { Id = c, Data = data, Estabelecimento = estabelecimento, Valor = valor });
            }
        }
    }
}