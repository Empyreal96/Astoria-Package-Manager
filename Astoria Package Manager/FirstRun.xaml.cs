using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
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
    /// APManager First-Run setup
    /// </summary>
    public sealed partial class FirstRun : Page
    {
        public FirstRun()
        {
            this.InitializeComponent();
            FRHeader.Text = "Astoria Package Manager\nFirst-run Setup";
            ReadmeBox.Visibility = Visibility.Collapsed;
            ReadmeFrame.Visibility = Visibility.Collapsed;
            CloseReadme.Visibility = Visibility.Collapsed;
        }
        DownloadOperation downloadOperation;
        CancellationTokenSource cancellationToken;

        BackgroundDownloader backgroundDownloader = new BackgroundDownloader();
        public static string tokenforapp { get; set; }
        public string token2 { get; set; }
        public string RememberOBBFolder(StorageFolder Folder)
        {
            
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(tokenforapp, Folder);
            return tokenforapp;
        }

        /// <summary>
        /// Remember the selected Download StorageFolder 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string RememberDLFolder(StorageFolder file)
        {
            token2 = "DLData";
            
            string token = Guid.NewGuid().ToString();
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, file);
            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue("DownloadStorage", token);
            return token;
        }
        //To retrieve the file the next time, you can use this:      

        /// <summary>
        /// Remember Android StorageFolder
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string RememberAFolder(StorageFolder file)
        {
            tokenforapp = "OBBData";
            string token = Guid.NewGuid().ToString();
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, file);
            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue("AndroidStorage", token);
            return token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChooseDlDir_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker DLPicker = new FolderPicker();
            DLPicker.SuggestedStartLocation = PickerLocationId.Downloads;
            StorageFolder DLFolder = await DLPicker.PickSingleFolderAsync();
            if (DLFolder == null)
            {
                DownloadLocationInfo.Text = "No Folder Selected";
                return;
            }
            DownloadLocationInfo.Text = $"\"{DLFolder.Path}\" has been selected";
            RememberDLFolder(DLFolder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChooseASDir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderPicker OBBPicker = new FolderPicker();
                OBBPicker.SuggestedStartLocation = PickerLocationId.Downloads;
                StorageFolder OBBFolder = await OBBPicker.PickSingleFolderAsync();
                if (OBBFolder == null)
                {
                    AndroidLocation.Text = "No Folder Selected";
                    return;
                }
                AndroidLocation.Text += $"\n\n\"{OBBFolder.Path}\" has been selected";
                // RememberOBBFolder(OBBFolder);
                RememberAFolder(OBBFolder);
            } catch (Exception ex)
            {
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }
        }

        private async void GithubLink_Click(object sender, RoutedEventArgs e)
        {
            string readmeURL = "https://github.com/Empyreal96/Astoria-Package-Manager/raw/main/README.txt";
            
            var readmecreate = await ApplicationData.Current.LocalFolder.CreateFileAsync("README.txt");
            downloadOperation = backgroundDownloader.CreateDownload(new Uri(readmeURL), readmecreate);
            cancellationToken = new CancellationTokenSource();
            await downloadOperation.StartAsync().AsTask(cancellationToken.Token);
            var readmeFile = await ApplicationData.Current.LocalFolder.GetFileAsync("README.txt");
            if (readmeFile == null)
            {
                ExceptionHelper.Exceptions.CustomException("Error fetching file \"README.txt\"");
            }
            var text = await FileIO.ReadLinesAsync(readmeFile);
            ReadmeBox.Text = text.ToString();
            CloseReadme.Visibility = Visibility.Visible;
            ReadmeBox.Visibility = Visibility.Visible;
            ReadmeFrame.Visibility = Visibility.Visible;
            //ReadmeBox.Con
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
