using Android.OS;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;

namespace MovieSearch.Droid.Activities
{
    public class FavoriteFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            // Create your fragment here

            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}
