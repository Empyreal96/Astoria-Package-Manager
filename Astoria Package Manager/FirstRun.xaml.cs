using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Astoria_Package_Manager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstRun : Page
    {
        public FirstRun()
        {
            this.InitializeComponent();
            FRHeader.Text = "Astoria Package Manager\nFirst-run Setup";
        }
        public static string tokenforapp { get; set; }
        public string token2 { get; set; }
        public string RememberOBBFolder(StorageFolder Folder)
        {
            
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(tokenforapp, Folder);
            return tokenforapp;
        }
        public string RememberDLFolder(StorageFolder Folder)
        {
            token2 = "DLData";
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(token2, Folder);
            return token2;
        }
        //To retrieve the file the next time, you can use this:      

        public static string RememberFolder(StorageFolder file)
        {
            tokenforapp = "OBBData";
            string token = Guid.NewGuid().ToString();
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, file);
            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue("AndroidStorage", token);
            return token;
        }

        private async void ChooseDlDir_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker DLPicker = new FolderPicker();
            DLPicker.SuggestedStartLocation = PickerLocationId.Downloads;
            StorageFolder DLFolder = await DLPicker.PickSingleFolderAsync();
            if (DLFolder == null)
            {
                DownloadLocationInfo.Text = "No Folder Selected";
            }
            DownloadLocationInfo.Text = $"\"{DLFolder.Path}\" has been selected";
            RememberDLFolder(DLFolder);
        }

        private async void ChooseASDir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderPicker OBBPicker = new FolderPicker();
                OBBPicker.SuggestedStartLocation = PickerLocationId.Downloads;
                StorageFolder OBBFolder = await OBBPicker.PickSingleFolderAsync();
                AndroidLocation.Text += $"\n\n\"{OBBFolder.Path}\" has been selected";
                // RememberOBBFolder(OBBFolder);
                RememberFolder(OBBFolder);
            } catch (Exception ex)
            {
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }
        }

        private async void GithubLink_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("https://github.com/empyreal96/Astoria-Package-Manager");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void FinishSetup_Click(object sender, RoutedEventArgs e)
        {
            if(tokenforapp != "OBBData")
            {
                
                ExceptionHelper.Exceptions.AstoriaSetupError();
                return;
            }
            else
            {
                await ApplicationData.Current.LocalFolder.CreateFileAsync("FRComplete.txt");
                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
