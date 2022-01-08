using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace StorageNotifier
{
    class Notifier
    {
        public static async void ShowAStorageMessage()
        {
            var ShowPopup = new MessageDialog("You *MUST* select the \"[Android Storage] > [Android] > [obb]\" folder now");
            ShowPopup.Commands.Add(new UICommand("Close"));
            await ShowPopup.ShowAsync();
        }
    }
    
}
