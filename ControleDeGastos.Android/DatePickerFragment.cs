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
    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        private readonly Action<DateTime> _onDateSelected;
        private readonly DateTime _initialDate;

        public DatePickerFragment(DateTime initialDate, Action<DateTime> onDateSelected)
        {
            this._initialDate = initialDate;
            this._onDateSelected = onDateSelected;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return new DatePickerDialog(
                Activity,
                this,
                _initialDate.Year,
                _initialDate.Month - 1,
                _initialDate.Day);
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _onDateSelected(selectedDate);
        }
    }
}