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

            _idGasto = Intent.Extras.GetInt("Id");

            var data = new DateTime(Intent.Extras.GetLong("Data"));
            _editTextData = FindViewById<EditText>(Resource.Id.editTextData);
            _editTextData.Text = data.ToShortDateString();
            _editTextData.Click += editTextData_Click;

            var nomeEstabelecimento = Intent.Extras.GetString("Estabelecimento");
            _spinnerEstabelecimento = FindViewById<Spinner>(Resource.Id.spinnerEstabelecimento);
            _spinnerEstabelecimento.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleSpinnerItem, MainActivity.Dados.Estabelecimentos.Select(e => e.Nome).ToArray());
            var estabelecimento = MainActivity.Dados.Estabelecimentos.FirstOrDefault(e => nomeEstabelecimento == e.Nome);
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

            var cancelar = FindViewById<Button>(Resource.Id.buttonCancelar);
            cancelar.Click += (s, e) => Finish();

            var salvar = FindViewById<Button>(Resource.Id.buttonSalvar);
            salvar.Click += Salvar_Click;
        }

        private void editTextData_Click(object sender, EventArgs e)
        {
            var frag = new DatePickerFragment(
                Convert.ToDateTime(_editTextData.Text),
                (data) =>
                {
                    _editTextData.Text = data.ToShortDateString();
                });
            frag.Show(FragmentManager, string.Empty);
        }

        private void Salvar_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.PutExtra("Id", _idGasto);
            var data = Convert.ToDateTime(_editTextData.Text);
            intent.PutExtra("Data", data.Ticks);
            var estabelecimento = MainActivity.Dados.Estabelecimentos[_spinnerEstabelecimento.SelectedItemPosition];
            intent.PutExtra("Estabelecimento", estabelecimento.Nome);
            intent.PutExtra("Valor", Convert.ToDouble(_editTextValor.Text, System.Globalization.CultureInfo.InvariantCulture));

            SetResult(Result.Ok, intent);

            var sharedPreferences = global::Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            sharedPreferences.Edit();
            var preferencesEditor = sharedPreferences.Edit();
            preferencesEditor.PutInt("UltimoEstabelecimento", estabelecimento.Id);
            preferencesEditor.Commit();

            Finish();
        }
    }
}