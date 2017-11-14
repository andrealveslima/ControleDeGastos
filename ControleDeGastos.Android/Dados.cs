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
        private string _caminhoBanco;

        public Dados()
        {
            Estabelecimentos = new List<Models.Estabelecimento>();
            Gastos = new List<Models.Gasto>();
            _caminhoBanco = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "gastos.db");
        }

        public void Carregar()
        {
            Estabelecimentos.Clear();
            Gastos.Clear();

            if (!System.IO.File.Exists(_caminhoBanco))
            {
                CriarBancoVazio();
            }

            using (var conn = new Mono.Data.Sqlite.SqliteConnection("Data Source=" + _caminhoBanco))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (var comm = new Mono.Data.Sqlite.SqliteCommand(conn))
                    {
                        comm.CommandText = "SELECT * FROM Estabelecimento";
                        using (var reader = comm.ExecuteReader())
                        {
                            using (var table = new System.Data.DataTable())
                            {
                                table.Load(reader);
                                foreach (System.Data.DataRow linha in table.Rows)
                                {
                                    Estabelecimentos.Add(new Models.Estabelecimento()
                                    {
                                        Id = Convert.ToInt32(linha["Id"]),
                                        Nome = linha["Nome"].ToString()
                                    });
                                }
                            }
                        }

                        comm.CommandText = "SELECT * FROM Gasto";
                        using (var reader = comm.ExecuteReader())
                        {
                            using (var table = new System.Data.DataTable())
                            {
                                table.Load(reader);
                                foreach (System.Data.DataRow linha in table.Rows)
                                {
                                    var estabelecimentoID = Convert.ToInt32(linha["EstabelecimentoId"]);
                                    var estabelecimento = Estabelecimentos.FirstOrDefault(e => e.Id == estabelecimentoID);
                                    Gastos.Add(new Models.Gasto()
                                    {
                                        Id = Convert.ToInt32(linha["Id"]),
                                        Data = Convert.ToDateTime(linha["Data"]),
                                        Estabelecimento = estabelecimento,
                                        Valor = Convert.ToDecimal(linha["Valor"])
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CriarBancoVazio()
        {
            Mono.Data.Sqlite.SqliteConnection.CreateFile(_caminhoBanco);
            using (var conn = new Mono.Data.Sqlite.SqliteConnection("Data Source=" + _caminhoBanco))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (var comm = new Mono.Data.Sqlite.SqliteCommand(conn))
                    {
                        comm.CommandText = "CREATE TABLE Estabelecimento (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Nome TEXT NOT NULL)";
                        comm.ExecuteNonQuery();

                        for (int c = 1; c <= 10; c++)
                        {
                            comm.CommandText = "INSERT INTO Estabelecimento (Nome) VALUES (@Nome)";
                            comm.Parameters.Clear();
                            comm.Parameters.AddWithValue("@Nome", string.Format("Estabelecimento {0}", c));
                            comm.ExecuteNonQuery();
                        }

                        comm.CommandText = "CREATE TABLE Gasto (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Data TEXT NOT NULL, EstabelecimentoId INTEGER NOT NULL, Valor REAL NOT NULL)";
                        comm.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Salvar()
        {
            using (var conn = new Mono.Data.Sqlite.SqliteConnection("Data Source=" + _caminhoBanco))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (var comm = new Mono.Data.Sqlite.SqliteCommand(conn))
                    {
                        comm.CommandText = "DELETE FROM Gasto";
                        comm.ExecuteNonQuery();

                        comm.CommandText = "INSERT INTO Gasto (Data, EstabelecimentoId, Valor) VALUES (@Data, @EstabelecimentoID, @Valor)";
                        foreach (var gasto in Gastos)
                        {
                            comm.Parameters.Clear();
                            comm.Parameters.AddWithValue("@Data", gasto.Data);
                            comm.Parameters.AddWithValue("@EstabelecimentoId", gasto.Estabelecimento.Id);
                            comm.Parameters.AddWithValue("@Valor", gasto.Valor);
                            comm.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}