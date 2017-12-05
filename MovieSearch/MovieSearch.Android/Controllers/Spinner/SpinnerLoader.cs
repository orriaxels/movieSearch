using System;
using Android.App;

namespace MovieSearch.Droid.Controllers.Spinner
{
    public class SpinnerLoader
    {
        private ProgressDialog _progressBar;
        public SpinnerLoader(Activity activity)
        {
            this._progressBar = new ProgressDialog(activity);
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
