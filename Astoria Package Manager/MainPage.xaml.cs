﻿using ApkInfoReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Xml.Linq;
using System.Xml;
using Windows.Management.Deployment;
using Windows.UI.Popups;

using System.Collections.ObjectModel;

using CG.Web.MegaApiClient;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Phone_Helper;
using System.Runtime.InteropServices;

using Windows.Storage.AccessCache;
using System.Diagnostics;
using Windows.Storage.Search;

//using Windows.Data.Xml.Dom;


namespace Astoria_Package_Manager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public bool IsFirstRunComplete { get; set; }
        public string OBBStorageToken = "OBBData";
        public string DownloadsToken = "DLData";
        public StorageFolder PackageFolder { get; set; }
        public static StorageFolder obbFiles { get; set; }
        public static StorageFolder AndroidFolder{ get; set; }
        public StorageFolder SelectedObbFolder { get; set; }
        public bool IsAppDataRequired { get; set; }
        public static string content { get; set; }
        public static StorageFile SelectedPackage { get; set; }
        public static StorageFile file2 { get; set; }
        public static StorageFolder storage { get; set; }
        public static StorageFolder storage2 { get; set; }
        public StorageFileQueryResult result { get; set; }
        public string PermName { get; set; }
        public string newmanifest { get; set; }
        public string PackageInfo { get; set; }
        public string PackageName { get; set; }
        public string FoundPermissions { get; set; }
        public string InstaledPkgList { get; set; }
        public string AoWMount = @"C:\Data\Users\DefApps\AppData\Local\Aow\data\";
        /// <summary>
        /// 
        /// </summary>
        public MainPage()
        {
            try
            {
                this.InitializeComponent();
                Loaded += Page_Loaded;
                if (Debugger.IsAttached == true)
                {
                    ClearFirstRunData.Visibility = Visibility.Visible;
                } else
                {
                    ClearFirstRunData.Visibility = Visibility.Collapsed;
                }
                RepoGrid.Visibility = Visibility.Collapsed;
                Appsbutton.Visibility = Visibility.Collapsed;
                GamesButton.Visibility = Visibility.Collapsed;
                GamesStack.Visibility = Visibility.Collapsed;
                progressBar.Visibility = Visibility.Collapsed;
                ViewManifestBtn.Visibility = Visibility.Collapsed;
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                RepoHome.Visibility = Visibility.Collapsed;
                AudioVideo.Visibility = Visibility.Collapsed;
                Browsers.Visibility = Visibility.Collapsed;
                Emulation.Visibility = Visibility.Collapsed;
                GApps.Visibility = Visibility.Collapsed;
                Imaging.Visibility = Visibility.Collapsed;
                Messaging.Visibility = Visibility.Collapsed;
                Misc.Visibility = Visibility.Collapsed;
                Readers.Visibility = Visibility.Collapsed;
                SocialMedia.Visibility = Visibility.Collapsed;
                TextEdit.Visibility = Visibility.Collapsed;
                Tools.Visibility = Visibility.Collapsed;
                progressRepo.Visibility = Visibility.Collapsed;
                AddOBBData.Visibility = Visibility.Collapsed;
                
                OutputBox.Text = "\n[Alpha Build]\n\n" +
                    "Open a compatible package to install, bigger packages (60MB+) will take longer to process.\n\n" +
                    "Please be aware functionality is limited for now, over time this will become perfected!";
                AboutText.Text = "This software uses several Open Source libraries:\n• Iteedee.ApkReader by hylander0\n" +
                    "• MegaApiClient fork from MediaExplorer\n\n\n" +
                    "Want to submit an app to the repo?" +
                    "\nRepack/Convert the app to Appx, and upload to the ProjectA Telegram, I will update the repo as needed";
            } catch (Exception ex)
            {
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }
        }



        /// <summary>
        /// Open a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OpenArchive_Click(object sender, RoutedEventArgs e)
        {
            ClearStorage();
            
            AddOBBData.Visibility = Visibility.Collapsed;
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".appx");
            StorageFile file = await filePicker.PickSingleFileAsync();
            if (file == null)
            {
                OpenArchiveHeader.Text = "No file selected";
            }
            else
            {
                OpenArchiveHeader.Text = $"{file.Name}";
                await StartDecompression(file);

            }
        }



        /// <summary>
        /// Read Android Manifest XML Elements and Attributes to TextBlock
        /// </summary>
        /// <param name="textblock"></param>
        /// <param name="manifest"></param>
        /// <returns></returns>

        public async void PrintAndroidManifest(TextBlock textblock, string manifest, Stream stream)
        {
            try
            {


                XDocument doc = XDocument.Load(stream);

                string friendlymanifest = manifest.Replace(">", ">\n");
                textblock.Text = friendlymanifest;

                var packageName = doc.Root.Attribute("package");
                PackageName = packageName.ToString().Replace("package=", "").Replace("\"", "");
                var versionCode = doc.Root.Attribute("versionCode");
                var versionName = doc.Root.Attribute("versionName");
                var sdktargets = doc.Root.Element("uses-sdk");
                var minsdk = sdktargets.Attribute("minSdkVersion");
                var targetsdk = sdktargets.Attribute("targetSdkVersion");

                AndroidAPIs.CheckminAPILevel(minsdk.ToString().Replace("minSdkVersion=", "").Replace("\"", ""));
                AndroidAPIs.CheckmaxAPILevel(targetsdk.ToString().Replace("targetSdkVersion=", "").Replace("\"", ""));
                var permissions = from d in doc.Root.Elements("uses-permission")
                                  select new
                                  {
                                      PermName = (string)d.Attribute("name")
                                  };
                foreach (var permission in permissions)
                {
                    FoundPermissions += $"{permission.PermName}\n";
                }
                AndroidAPIs.CheckPermissions(FoundPermissions);
                PackageInfo = $"\nPackage Name: {packageName.ToString().Replace("package=", "").Replace("\"", "")}\nVersion: {versionName.ToString().Replace("versionName=", "").Replace("\"", "")}\nMinimum Build: {AndroidAPIs.minSDKLevel}\nTarget Build: {AndroidAPIs.targetSDKLevel}\n\n\nPermissions:\n{AndroidAPIs.PermissionsRequested}";


                //
                CheckAPICompat(AndroidAPIs.minSDKLevel);
                await GetAndroidFolder();
                textblock.Text = PackageInfo;

                InstallPackageBtn.Visibility = Visibility.Visible;
                AddOBBData.Visibility = Visibility.Visible;
                progressBar.IsEnabled = false;
                progressBar.IsEnabled = false;
                progressBar.Visibility = Visibility.Collapsed;
                ViewManifestBtn.Visibility = Visibility.Visible;

            } catch (Exception ex)
            {
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewManifestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OutputBox.Text != newmanifest)
            {
                OutputBox.Text = newmanifest;
                ViewManifestBtn.Content = "View Permissions";
            }
            else
            {
                OutputBox.Text = PackageInfo;
                ViewManifestBtn.Content = "View Manifest";
            }
        }

        /// <summary>
        /// Check to see if Android Version matches Astoria's base (Android 4.4), if not then not supported.
        /// </summary>
        /// <param name="apiLevel"></param>
        public void CheckAPICompat(string apiLevel)
        {
            if (apiLevel == "Android 5.0")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 5.1")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 6")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 7.0")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 7.1")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 8.0")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 8.1")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 9")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 10")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 11")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 12")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
            if (apiLevel == "Android 13")
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
                OpenArchiveHeader.Text = "App not supported.";
            }
        }



        /// <summary>
        /// Start the Decompress processes to extract APK and AndroidManifest.
        /// </summary>
        /// <param name="file"></param>
        public async Task StartDecompression(StorageFile file)
        {
            progressBar.IsEnabled = true;
            progressBar.IsEnabled = true;
            progressBar.IsIndeterminate = true;
            progressBar.Visibility = Visibility.Visible;
            try
            {

                storage = ApplicationData.Current.LocalCacheFolder;

                OutputBox.Text = "Processing Package Information";
                await Decompress(file, storage);


                await DecompressManifest(file, storage);
                OutputBox.Text = "Reading Manifest";
                DecodeManifest(storage);




            }
            catch (Exception ex)
            {
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
                ClearStorage();
            }
        }





        /// <summary>
        /// Decompresses the specified file to the specified folder.
        /// </summary>
        /// <param name="source">The compressed zip file.</param>
        /// <param name="destination">The folder where the file will be decompressed.</param>

        public static async Task Decompress(StorageFile source, StorageFolder destination)
        {
            SelectedPackage = source;
            // GlobalStrings.Logger = "";
            //log = new List<ArchiverError>();
            try
            {
                using (Stream stream = await source.OpenStreamForReadAsync())
                {
                    using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name == "payload.apk")
                            {

                                string fileName = entry.FullName.Replace("/", "\\");

                                using (Stream entryStream = entry.Open())
                                {
                                    byte[] buffer = new byte[entry.Length];
                                    entryStream.Read(buffer, 0, buffer.Length);

                                    try
                                    {
                                        StorageFile file = await destination.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                                        using (IRandomAccessStream uncompressedFileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                                        {
                                            using (Stream data = uncompressedFileStream.AsStreamForWrite())
                                            {
                                                data.Write(buffer, 0, buffer.Length);
                                                data.Flush();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ExceptionHelper.Exceptions.ThrownExceptionError(ex);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }
        }


        /// <summary>
        /// Decompresses the specified file to the specified folder.
        /// </summary>
        /// <param name="source">The compressed zip file.</param>
        /// <param name="destination">The folder where the file will be decompressed.</param>

        public static async Task DecompressManifest(StorageFile source, StorageFolder destination)
        {
            // GlobalStrings.Logger = "";
            //log = new List<ArchiverError>();
            StorageFile file = await destination.GetFileAsync("payload.apk");
            try
            {
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name == "AndroidManifest.xml")
                            //if (!string.IsNullOrEmpty(entry.FullName))
                            {

                                string fileName = entry.FullName.Replace("/", "\\");

                                using (Stream entryStream = entry.Open())
                                {
                                    byte[] buffer = new byte[entry.Length];
                                    entryStream.Read(buffer, 0, buffer.Length);

                                    try
                                    {
                                        file2 = await destination.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                                        using (IRandomAccessStream uncompressedFileStream = await file2.OpenAsync(FileAccessMode.ReadWrite))
                                        {
                                            using (Stream data = uncompressedFileStream.AsStreamForWrite())
                                            {
                                                data.Write(buffer, 0, buffer.Length);
                                                data.Flush();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                        // GlobalStrings.Logger += $"Error with Archive {ex.Message}, {fileName}";
                                        //    log.Add(new ArchiverError(ex.Message, fileName));
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }
        }





        /// <summary>
        /// Decode an Android XML file (e.g AndroidManifest.xml)
        /// </summary>
        /// <param name="folder"></param>
        public async void DecodeManifest(StorageFolder folder)
        {
            var readBuffer = await FileIO.ReadBufferAsync(file2);
            // byte[] bytes = new byte[100 * 2048];
            byte[] bytes = readBuffer.ToArray();
            content = "";
            Stream strm = await file2.OpenStreamForReadAsync();
            int size = strm.Read(bytes, 0, bytes.Length);

            using (BinaryReader s = new BinaryReader(strm))
            {
                byte[] bytes2 = new byte[size];
                Array.Copy(bytes, bytes2, size);
                AndroidDecompress decompress = new AndroidDecompress();
                content = decompress.decompressXML(bytes);


                string manifest = content;
                newmanifest = manifest.Replace(">", ">\n");
                await FileIO.WriteTextAsync(await folder.CreateFileAsync("AndroidManifest_decoded.xml"), newmanifest);
                /// FOR DEBUG
                //await SaveManifest();
                ///
                PrintAndroidManifest(OutputBox, newmanifest, await folder.OpenStreamForReadAsync("AndroidManifest_decoded.xml"));




            }

        }

        public async
        Task
SaveManifest()
        {
            FolderPicker folderPicker = new FolderPicker();
            storage2 = await folderPicker.PickSingleFolderAsync();
            var saveman = await storage.GetFileAsync("AndroidManifest_decoded.xml");
            await saveman.CopyAsync(storage2, "AndroidManifest_decoded.xml", NameCollisionOption.GenerateUniqueName);
        }


        public PackageManager pacman = new PackageManager();
        /// <summary>
        /// 
        /// </summary>
        public async void CheckIfInstalled()
        {

        }



        public async void InstallPackage(StorageFile file)
        {
            try
            {
                InstallPackageBtn.Visibility = Visibility.Collapsed;
            progressBar.IsEnabled = true;
            progressBar.Visibility = Visibility.Visible;
            progressBar.IsIndeterminate = true;
            OpenArchiveHeader.Text = "Installing, Please Wait";
            //pacman.FindPackageForUser

            

                
                await pacman.AddPackageAsync(new Uri(file.Path), null, DeploymentOptions.ForceTargetApplicationShutdown);
                OpenArchiveHeader.Text = "Select another file to install more";
                progressBar.IsEnabled = false;
                progressBar.Visibility = Visibility.Collapsed;
                progressBar.IsIndeterminate = true;
                var CompletedInstall = new MessageDialog($"{PackageName} has installed successfully");
                CompletedInstall.Commands.Add(new UICommand("Close"));
                await CompletedInstall.ShowAsync();



            } catch (Exception ex)
            {
                OutputBox.Text = $"An error occured while trying to install: \n{ex.Message}";
            }
        }
       
        /// <summary>
        /// Clears all files in the Apps LocalCacheFolder
        /// </summary>
        public async void ClearStorage()
        {
            var tempFolder = ApplicationData.Current.LocalCacheFolder;
            var tempFiles = await tempFolder.GetFilesAsync();
            if (tempFiles != null)
            {
                foreach (var tempItem in tempFiles)
                {
                    try
                    {
                        await tempItem.DeleteAsync();
                    }
                    catch (Exception ee)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void InstallPackageBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (IsAppDataRequired == true)
            {
                try
                {
                   // StorageNotifier.Notifier.ShowAStorageMessage();
                    //FolderPicker picker = new FolderPicker();
                    //picker.FileTypeFilter.Add(".obb");
                    //StorageFolder folder = await picker.PickSingleFolderAsync();
                    OpenArchiveHeader.Text = "Placeholder: Copying Files";
                    var obbFile = await SelectedObbFolder.GetFilesAsync();
                   var output = await AndroidFolder.CreateFolderAsync(PackageName, CreationCollisionOption.OpenIfExists);
                    foreach (var obb in obbFile)
                    {
                        try
                        {

                            await obb.CopyAsync(output);
                        }
                        catch (Exception ex)
                        {
                            ExceptionHelper.Exceptions.ThrownExceptionError(ex);
                        }
                    }
                    //OpenArchiveHeader.Text = savedOutput.Path;
                    //var obbFilesFound = await savedOutput.GetFilesAsync();
                    
                } catch (Exception ex)
                {
                    ExceptionHelper.Exceptions.ThrownExceptionError(ex);
                }
                
            }
            InstallPackage(SelectedPackage);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<StorageFolder> GetFileForToken(string token)
        {
            if (!StorageApplicationPermissions.FutureAccessList.ContainsItem(token)) return null;
            return await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<StorageFolder> GetAndroidFolder()
        {
            AndroidFolder = null;

            var fileToken = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("AndroidStorage", "");
            if (fileToken.Length > 0)
            {
                AndroidFolder = await GetFileForToken(fileToken);
            }
            
           
           

            return AndroidFolder;
        }














        /// 
        ///
        ///
        ///
        ///
        Button newButton { get; set; }
        MegaApiClient client { get; set; }
        public INode Selnode { get; set; }
        IEnumerable<INode> nodes { get; set; }
        public string ProjectAGames = "https://mega.nz/folder/D1xkjaCS#fOiJnamGmb7uh6u4nlS9wA";
        public string ProjectAApps = "https://mega.nz/folder/yx4EhSZJ#KENf3nCTSTRY-auFQC18Pw";

        private void Appsbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                GamesStack.Children.Clear();
                GamesStack.Visibility = Visibility.Visible;
                //client = new MegaApiClient();
                Appsbutton.Visibility = Visibility.Collapsed;
                GamesButton.Visibility = Visibility.Collapsed;
                RepoHome.Visibility = Visibility.Visible;

                AudioVideo.Visibility = Visibility.Visible;
                Browsers.Visibility = Visibility.Visible;
                Emulation.Visibility = Visibility.Visible;
                GApps.Visibility = Visibility.Visible;
                Imaging.Visibility = Visibility.Visible;
                Messaging.Visibility = Visibility.Visible;
                Misc.Visibility = Visibility.Visible;
                Readers.Visibility = Visibility.Visible;
                SocialMedia.Visibility = Visibility.Visible;
                TextEdit.Visibility = Visibility.Visible;
                Tools.Visibility = Visibility.Visible;
            } catch (Exception ex)
            {
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }

        }




        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            GamesStack.Children.Clear();
            GamesStack.Visibility = Visibility.Visible;
            progressRepo.IsEnabled = true;
            progressRepo.IsIndeterminate = true;
            progressRepo.Visibility = Visibility.Visible;
            try
            {
                Appsbutton.Visibility = Visibility.Collapsed;
                GamesButton.Visibility = Visibility.Collapsed;
                RepoHome.Visibility = Visibility.Visible;
                //ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;



                Uri folderLink = new Uri(ProjectAGames);
                nodes = client.GetNodesFromLink(folderLink);
                foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
                {
                    //Console.WriteLine($"Downloading {node.Name}");
                    newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                    newButton.Click += AppList_Click;
                    newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                    newButton.Background = null;
                    newButton.BorderBrush = null;
                    newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                    Selnode = node;

                    newButton.Tag = $"{node.Name.Replace(" ", "")}";
                    // RelativePanel.SetBelow(newButton, GamesButton);
                    GamesStack.Children.Add(newButton);

                    //client.DownloadFile(node, node.Name);
                }


                //client.Logout();
                progressRepo.IsIndeterminate = false;
                progressRepo.IsEnabled = false;
                progressRepo.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                // client.Logout();
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
                progressRepo.IsIndeterminate = false;
                progressRepo.IsEnabled = false;
                progressRepo.Visibility = Visibility.Collapsed;
            }
        }




        private async void AppList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                progressRepo.Visibility = Visibility.Visible;
                progressRepo.IsEnabled = true;
                progressRepo.IsIndeterminate = true;
                //client.LoginAnonymous();
                //client.DownloadFile(node, node.Name);
                FolderPicker picker = new FolderPicker();
                StorageFolder storageFolder = await picker.PickSingleFolderAsync();

                foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
                {
                    if (node.Name.Replace(" ", "") == button.Tag.ToString())
                    {
                        Selnode = node;
                    }
                }

                StorageFile storageFile = await storageFolder.CreateFileAsync($"{button.Tag.ToString()}");
                using (var stream = client.Download(Selnode))
                {
                    var outFile = await storageFile.OpenAsync(FileAccessMode.ReadWrite);
                    var outStream = outFile;
                    OutputText.Text = $"Downloading {Selnode.Name.Replace(" ", "")}";

                    await stream.CopyToAsync(outStream.AsStreamForWrite());

                    //await outStream.CopyToAsync((Stream)inputStream);
                    await outStream.FlushAsync();
                }

                //client.Logout();
                OutputText.Text = "Downloading Completed";
                progressRepo.IsIndeterminate = false;
                progressRepo.IsEnabled = false;
                progressRepo.Visibility = Visibility.Collapsed;
                ExceptionHelper.Exceptions.DownloadCompleted($"\"{Selnode.Name.Replace(" ", "")}\"");
            } catch (Exception ex)
            {
                // client.Logout();
                OutputText.Text = "Downloading Failed";
                progressRepo.IsIndeterminate = false;
                progressRepo.IsEnabled = false;
                progressRepo.Visibility = Visibility.Collapsed;
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }
        }

        public void HideRepoAppButtons()
        {
            AudioVideo.Visibility = Visibility.Collapsed;
            Browsers.Visibility = Visibility.Collapsed;
            Emulation.Visibility = Visibility.Collapsed;
            GApps.Visibility = Visibility.Collapsed;
            Imaging.Visibility = Visibility.Collapsed;
            Messaging.Visibility = Visibility.Collapsed;
            Misc.Visibility = Visibility.Collapsed;
            Readers.Visibility = Visibility.Collapsed;
            SocialMedia.Visibility = Visibility.Collapsed;
            TextEdit.Visibility = Visibility.Collapsed;
            Tools.Visibility = Visibility.Collapsed;

        }
        public void ShowRepoAppButtons()
        {

            AudioVideo.Visibility = Visibility.Visible;
            Browsers.Visibility = Visibility.Visible;
            Emulation.Visibility = Visibility.Visible;
            GApps.Visibility = Visibility.Visible;
            Imaging.Visibility = Visibility.Visible;
            Messaging.Visibility = Visibility.Visible;
            Misc.Visibility = Visibility.Visible;
            Readers.Visibility = Visibility.Visible;
            SocialMedia.Visibility = Visibility.Visible;
            TextEdit.Visibility = Visibility.Visible;
            Tools.Visibility = Visibility.Visible;

        }


        private void RepoHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // client.Logout();
            } catch (Exception ex)
            {

            }
            OutputText.Text = "Browse the ProjectA Repo";
            GamesStack.Children.Clear();
            GamesStack.Visibility = Visibility.Collapsed;
            AudioVideo.Visibility = Visibility.Collapsed;
            Browsers.Visibility = Visibility.Collapsed;
            Emulation.Visibility = Visibility.Collapsed;
            GApps.Visibility = Visibility.Collapsed;
            Imaging.Visibility = Visibility.Collapsed;
            Messaging.Visibility = Visibility.Collapsed;
            Misc.Visibility = Visibility.Collapsed;
            Readers.Visibility = Visibility.Collapsed;
            SocialMedia.Visibility = Visibility.Collapsed;
            TextEdit.Visibility = Visibility.Collapsed;
            Tools.Visibility = Visibility.Collapsed;
            Appsbutton.Visibility = Visibility.Visible;
            GamesButton.Visibility = Visibility.Visible;
            RepoHome.Visibility = Visibility.Collapsed;

        }

        private void AudioVideo_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/Psw1ja5B#1sfW-giAMipxKF58r_aQ9A");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        private void Browsers_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/KhpVgYYA#DgYmTy6oLLydJGRURQxybA");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        private void Emulation_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/D84XgCpL#5Owij6v-K09B66jxxptoSw");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        private void GApps_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/2ox1wSLa#S9aUrw6enLuJZu8lnrBzXw");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        private void Imaging_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/XhpDlYIC#FUZ11hSMeZ0lZHeczwG0HQ");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        private void Messaging_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/C0onWK5b#0D0e5LXnlqZop7nvb1fbMw");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        private void Misc_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/fhxHEALb#GFkNbczsNQIr6D41n_IKjw");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        private void Readers_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/OlhhWKyY#YOAX2x8xIvO1RLJXh90p3Q");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        private void SocialMedia_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/SoxBGCSS#YydrrwJrTE95gBmhY45VyQ");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        private void TextEdit_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/WxhHWC5Y#lqEZAC1cJVAsInmy4XIpAQ");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        private void Tools_Click(object sender, RoutedEventArgs e)
        {
            HideRepoAppButtons();


            Uri folderLink = new Uri("https://mega.nz/folder/bhx1zaKD#sRr_uzlTCy2-khnrbsuoqA");
            nodes = client.GetNodesFromLink(folderLink);
            foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
            {
                //Console.WriteLine($"Downloading {node.Name}");
                newButton = new Button { Content = $"{node.Name.Replace(" ", "")}   {node.Size.ToFileSize()}" };
                newButton.Click += AppList_Click;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.Background = null;
                newButton.BorderBrush = null;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                Selnode = node;

                newButton.Tag = $"{node.Name.Replace(" ", "")}";
                // RelativePanel.SetBelow(newButton, GamesButton);
                GamesStack.Children.Add(newButton);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void APIPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InstallPackageBtn.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TelegramLink_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("https://t.me/joinchat/TPTmYQq0ZOgzhcPP");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadRepo_Click(object sender, RoutedEventArgs e)
        {
            RepoGrid.Visibility = Visibility.Visible;
            LoadRepo.Visibility = Visibility.Collapsed;
            GamesStack.Visibility = Visibility.Visible;
            Appsbutton.Visibility = Visibility.Visible;
            GamesButton.Visibility = Visibility.Visible;
            client = new MegaApiClient();
            client.LoginAnonymous();
        }

        /// <summary>
        /// 
        /// </summary>
        public async void DEBUGRemoveAppData()
        {

            try
            {

                var OldData = await ApplicationData.Current.LocalFolder.GetFileAsync("FRComplete.txt");
                await OldData.DeleteAsync();
            }
            catch (Exception ex)
            {
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public async void HasFirstRunComplete()
        {

            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync("FRComplete.txt");
                IsFirstRunComplete = true;
                return;
            }
            catch (Exception ex)
            {
                IsFirstRunComplete = false;
                LoadFirstRun();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public async void LoadFirstRun()
        {
            //this.Frame.Navigate(typeof(FirstRun));
            try
            {

                var SetupPage = $"Astoria_Package_Manager.FirstRun";
                var HomePageType = Type.GetType(SetupPage);
                Frame.Navigate(HomePageType);
            } catch (Exception ex)
            {
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }
        }

       
        ///public string obbDestination = @"C:\Data\Users\DefApps\AppData\Local\Aow\mnt\shell\emulated\0\Android\obb";
        
        ///
        private async void AddOBBData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderPicker picker = new FolderPicker();
                picker.FileTypeFilter.Add(".obb");
                picker.SuggestedStartLocation = PickerLocationId.Downloads;
                SelectedObbFolder = await picker.PickSingleFolderAsync();
                if (SelectedObbFolder == null)
                {
                    return;
                }

                var files = await SelectedObbFolder.GetFilesAsync();


                //FileOpenPicker obbFilePicker = new FileOpenPicker();
                //obbFilePicker.FileTypeFilter.Add(".obb");
                


                OutputBox.Text += "\n\nData Files Loaded: ";
                foreach (var obb in files)
                {
                    OutputBox.Text += $"\n{obb.Name}";
                }
                

                //
                IsAppDataRequired = true;
                //

            } catch (Exception ex)
            {
                ExceptionHelper.Exceptions.ThrownExceptionError(ex);
            }
        }

        

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            HasFirstRunComplete();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFirstRunData_Click(object sender, RoutedEventArgs e)
        {
            DEBUGRemoveAppData();
        }

        private void GithubAStorageLnk_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.Current.LocalFolder.GetFileAsync("README.md") == null)
            {

            }
        }
    }
}
