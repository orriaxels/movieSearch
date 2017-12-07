using System;
using Android.App;
using Android.Content;

namespace MovieSearch.Droid.Controllers.Spinner
{
    public class SpinnerLoader
    {
        private ProgressDialog _progressBar;
        public SpinnerLoader(Context context)
        {
            this._progressBar = new ProgressDialog(context);
            this._progressBar.SetCancelable(true);
            this._progressBar.SetProgressStyle(ProgressDialogStyle.Spinner);
        }

        public void setSpinnerMessage(String message)
        {
            this._progressBar.SetMessage(message);
        }

        public void show()
        {
            this._progressBar.Show();
        }

        public void hide()
        {
            this._progressBar.Hide();
        }
    }
}
