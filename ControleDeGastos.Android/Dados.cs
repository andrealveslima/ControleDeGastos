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

using SQLite;

namespace ControleDeGastos.Android
{
    public class Dados
    {
        public List<Models.Estabelecimento> Estabelecimentos { get; private set; }
        public List<Models.Gasto> Gastos { get; private set; }
        private readonly string _caminhoBanco;

        public Dados()
        {
            Estabelecimentos = new List<Models.Estabelecimento>();
            Gastos = new List<Models.Gasto>();
            _caminhoBanco = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "gastos.db");
        }

        public void Carregar()
        {
            if (!System.IO.File.Exists(_caminhoBanco))
            {
                CriarBancoVazio();
            }

            using (var db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), _caminhoBanco, storeDateTimeAsTicks: false))
            {
                Estabelecimentos = db.Table<Models.Estabelecimento>().ToList();
                Gastos = db.Table<Models.Gasto>().ToList();
                foreach (var gasto in Gastos)
                {
                    gasto.Estabelecimento = Estabelecimentos.FirstOrDefault(e => e.Id == gasto.EstabelecimentoId);
                }
            }
        }

        private void CriarBancoVazio()
        {
            using (var db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), _caminhoBanco, storeDateTimeAsTicks: false))
            {
                db.CreateTable<Models.Estabelecimento>();

                for (int c = 1; c <= 10; c++)
                {
                    db.Insert(new Models.Estabelecimento() { Nome = string.Format("Estabelecimento {0}", c) });
                }

                db.CreateTable<Models.Gasto>();
            }
        }

        public void Salvar()
        {
            using (var db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), _caminhoBanco, storeDateTimeAsTicks: false))
            {
                db.Execute("DELETE FROM Estabelecimento");
                db.InsertAll(Estabelecimentos);

                db.Execute("DELETE FROM Gasto");
                foreach (var gasto in Gastos)
                {
                    gasto.EstabelecimentoId = gasto.Estabelecimento.Id;
                }
                db.InsertAll(Gastos);
            }
        }
    }
}