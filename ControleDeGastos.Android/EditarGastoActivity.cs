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
    [Activity(Label = "Editar gasto")]
    public class EditarGastoActivity : Activity
    {
        private int _idGasto;
        private EditText _editTextData;
        private Spinner _spinnerEstabelecimento;
        private EditText _editTextValor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EditarGasto);

            // Pegando os valores passados pelo intent da janela principal.
            _idGasto = Intent.Extras.GetInt("Id");

            var data = new DateTime(Intent.Extras.GetLong("Data"));
            _editTextData = FindViewById<EditText>(Resource.Id.editTextData);
            _editTextData.Text = data.ToShortDateString();
            _editTextData.Click += editTextData_Click;

            var nomeEstabelecimento = Intent.Extras.GetString("Estabelecimento");
            _spinnerEstabelecimento = FindViewById<Spinner>(Resource.Id.spinnerEstabelecimento);
            _spinnerEstabelecimento.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleSpinnerItem, MainActivity.Dados.Estabelecimentos.Select(e => e.Nome).ToArray());
            var estabelecimento = MainActivity.Dados.Estabelecimentos.FirstOrDefault(e => nomeEstabelecimento == e.Nome);

            // Se um estabelecimento não foi passado no intent, nós tentamos preencher com o último estabelecimento selecionado anteriormente.
            if (estabelecimento == null)
            {
                var sharedPreferences = global::Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                var estabelecimentoID = sharedPreferences.GetInt("UltimoEstabelecimento", -1);
                if (estabelecimentoID != -1)
                {
                    estabelecimento = MainActivity.Dados.Estabelecimentos.FirstOrDefault(e => e.Id == estabelecimentoID);
                }
            }
            if (estabelecimento != null)
            {
                _spinnerEstabelecimento.SetSelection(MainActivity.Dados.Estabelecimentos.IndexOf(estabelecimento));
            }

            var valor = Intent.Extras.GetDouble("Valor");
            _editTextValor = FindViewById<EditText>(Resource.Id.editTextValor);
            _editTextValor.Text = valor.ToString(System.Globalization.CultureInfo.InvariantCulture);

            // Botões e eventos.
            var cancelar = FindViewById<Button>(Resource.Id.buttonCancelar);
            cancelar.Click += (s, e) => Finish();

            var salvar = FindViewById<Button>(Resource.Id.buttonSalvar);
            salvar.Click += Salvar_Click;

            var excluir = FindViewById<Button>(Resource.Id.buttonExcluir);
            excluir.Click += Excluir_Click;

            var novoEstabelecimento = FindViewById<Button>(Resource.Id.buttonNovoEstabelecimento);
            novoEstabelecimento.Click += NovoEstabelecimento_Click;

            var excluirEstabelecimento = FindViewById<Button>(Resource.Id.buttonExcluirEstabelecimento);
            excluirEstabelecimento.Click += ExcluirEstabelecimento_Click;
        }

        private void ExcluirEstabelecimento_Click(object sender, EventArgs e)
        {
            if (_spinnerEstabelecimento.SelectedItemPosition >= 0)
            {
                var estabelecimento = MainActivity.Dados.Estabelecimentos[_spinnerEstabelecimento.SelectedItemPosition];
                if (MainActivity.Dados.Gastos.Any(g => g.EstabelecimentoId == estabelecimento.Id))
                {
                    Toast.MakeText(ApplicationContext, "Este estabelecimento está sendo utilizado", ToastLength.Long).Show();
                }
                else
                {
                    MainActivity.Dados.Estabelecimentos.Remove(estabelecimento);
                    MainActivity.Dados.Salvar();
                    _spinnerEstabelecimento.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleSpinnerItem, MainActivity.Dados.Estabelecimentos.Select(est => est.Nome).ToArray());
                }
            }
        }

        private void NovoEstabelecimento_Click(object sender, EventArgs e)
        {
            EditText editTextEstabelecimento = new EditText(this);
            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            dialog.SetTitle("Nome do Estabelecimento");
            dialog.SetView(editTextEstabelecimento);
            dialog.SetPositiveButton("OK", (senderAlert, args) =>
            {
                var estabelecimento = new Models.Estabelecimento();
                estabelecimento.Id = MainActivity.Dados.Estabelecimentos.Max(est => est.Id) + 1;
                estabelecimento.Nome = editTextEstabelecimento.Text;
                MainActivity.Dados.Estabelecimentos.Add(estabelecimento);
                _spinnerEstabelecimento.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleSpinnerItem, MainActivity.Dados.Estabelecimentos.Select(est => est.Nome).ToArray());
                _spinnerEstabelecimento.SetSelection(MainActivity.Dados.Estabelecimentos.IndexOf(estabelecimento));
            });
            dialog.SetNegativeButton("Cancelar", (IDialogInterfaceOnClickListener)null);

            dialog.Show();
        }

        private void Excluir_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirmação");
            alert.SetMessage("Confirma a exclusão do gasto?");
            alert.SetPositiveButton("OK", (senderAlert, args) => 
            {
                var gasto = MainActivity.Dados.Gastos.FirstOrDefault(g => g.Id == _idGasto);
                if (gasto != null)
                {
                    MainActivity.Dados.Gastos.Remove(gasto);
                }

                var intent = new Intent();
                intent.PutExtra("Id", _idGasto);
                intent.PutExtra("Excluido", true);
                SetResult(Result.Ok, intent);
                Finish();
            });
            alert.SetNegativeButton("Cancelar", (IDialogInterfaceOnClickListener)null);

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void editTextData_Click(object sender, EventArgs e)
        {
            var dataBase = DateTime.Now.Date;
            if (!string.IsNullOrWhiteSpace(_editTextData.Text))
            {
                DateTime.TryParse(_editTextData.Text, out dataBase);
            }

            var frag = new DatePickerFragment(
                dataBase,
                (data) =>
                {
                    _editTextData.Text = data.ToShortDateString();
                });
            frag.Show(FragmentManager, string.Empty);
        }

        private void Salvar_Click(object sender, EventArgs e)
        {
            DateTime data;
            double valor;

            if (ValidarPreenchimento(out data, out valor))
            {
                var intent = new Intent();
                intent.PutExtra("Id", _idGasto);
                intent.PutExtra("Data", data.Ticks);
                intent.PutExtra("Valor", valor);

                var estabelecimento = MainActivity.Dados.Estabelecimentos[_spinnerEstabelecimento.SelectedItemPosition];
                intent.PutExtra("Estabelecimento", estabelecimento.Nome);
                var sharedPreferences = global::Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                var preferencesEditor = sharedPreferences.Edit();
                preferencesEditor.PutInt("UltimoEstabelecimento", estabelecimento.Id);
                preferencesEditor.Commit();

                SetResult(Result.Ok, intent);
                Finish();
            }
            else
            {
                Toast.MakeText(ApplicationContext, "Data e/ou valor inválido", ToastLength.Long).Show();
            }
        }

        private bool ValidarPreenchimento(out DateTime data, out double valor)
        {
            data = DateTime.MinValue;
            valor = double.MinValue;
            return !string.IsNullOrWhiteSpace(_editTextData.Text) && !string.IsNullOrWhiteSpace(_editTextValor.Text) &&
                   DateTime.TryParse(_editTextData.Text, out data) && 
                   double.TryParse(_editTextValor.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor) &&
                   valor > 0;
        }
    }
}