using System;
using System.Diagnostics;
using UIKit;
namespace MovieSearch.iOS.Controllers
{
    public class TabBarController : UITabBarController
    {
        public TabBarController()
        {
            ViewControllerSelected += (sender, args) =>
            {
                UINavigationController currentSelectedController = (UINavigationController)args.ViewController;
                var topController = currentSelectedController.TopViewController;

                if (topController is TopTenController)
                {
                    var topTenController = (TopTenController)topController;
                    topTenController.download = true;
                }
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.TabBar.BarTintColor = UIColor.FromRGB(57, 57, 57);
            this.TabBar.TintColor = UIColor.White;

            this.SelectedIndex = 0;
        }
    }
}
