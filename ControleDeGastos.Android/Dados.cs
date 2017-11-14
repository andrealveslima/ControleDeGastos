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

            var diretorio = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            var caminhoEstabelecimentos = System.IO.Path.Combine(diretorio, "estabelecimentos.json");
            if (System.IO.File.Exists(caminhoEstabelecimentos))
            {
                using (var stream = new System.IO.FileStream(caminhoEstabelecimentos, System.IO.FileMode.Open))
                {
                    var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<Models.Estabelecimento>));
                    Estabelecimentos = (List<Models.Estabelecimento>)serializer.ReadObject(stream);
                }
            }
            else
            {
                for (int c = 1; c <= 10; c++)
                {
                    Estabelecimentos.Add(new Models.Estabelecimento() { Id = c, Nome = string.Format("Estabelecimento {0}", c) });
                }
            }

            var caminhoGastos = System.IO.Path.Combine(diretorio, "gastos.json");
            if (System.IO.File.Exists(caminhoGastos))
            {
                using (var stream = new System.IO.FileStream(caminhoGastos, System.IO.FileMode.Open))
                {
                    var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<Models.Gasto>));
                    Gastos = (List<Models.Gasto>)serializer.ReadObject(stream);
                }
            }
        }

        public void Salvar()
        {
            var diretorio = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            using (var stream = new System.IO.FileStream(System.IO.Path.Combine(diretorio, "estabelecimentos.json"), System.IO.FileMode.Create))
            {
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<Models.Estabelecimento>));
                serializer.WriteObject(stream, Estabelecimentos);
            }
            using (var stream = new System.IO.FileStream(System.IO.Path.Combine(diretorio, "gastos.json"), System.IO.FileMode.Create))
            {
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<Models.Gasto>));
                serializer.WriteObject(stream, Gastos);
            }
        }
    }
}