using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Web.Http;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Windows.Networking.BackgroundTransfer;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ASimplePlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Text_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker MyPicker = new FileOpenPicker();
            MyPicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            MyPicker.FileTypeFilter.Add(".mp4");
            MyPicker.FileTypeFilter.Add(".mp3");
            StorageFile file = await MyPicker.PickSingleFileAsync();
            if (file != null)
            {
                MyPlayer.AreTransportControlsEnabled = true;
                MyText.Text = file.Name;
                var stream = await file.OpenAsync(FileAccessMode.Read);
                MyPlayer.SetSource(stream, file.ContentType);
            }
        }

        private void PlayOnline_Click(object sender, RoutedEventArgs e)
        {
            MyPlayer.AreTransportControlsEnabled = true;
            MyText.Text = "NEU Song";
            MyPlayer.Source=new Uri("http://www.neu.edu.cn/indexsource/neusong.mp3");
            MyPlayer.AutoPlay = true;
        }


        private async void Download_Click(object sender, RoutedEventArgs e)
        {

            StorageFile file=await GetFileAsync();
            if (file != null)
            {
                MyPlayer.AreTransportControlsEnabled = true;
                MyText.Text = file.Name;
                var stream = await file.OpenAsync(FileAccessMode.Read);
                MyPlayer.SetSource(stream, file.ContentType);
                MyPlayer.AutoPlay = true;
            }
            else
            {
                MyText.Text = "empty file";
            }
        }
        public async Task<StorageFile> GetFileAsync()
        {
            try
            {
                var httpClient = new Windows.Web.Http.HttpClient();
                var buffer = await httpClient.GetBufferAsync(new Uri("http://www.neu.edu.cn/indexsource/neusong.mp3"));
                if (buffer != null)
                {
                    StorageFile File = await KnownFolders.MusicLibrary.CreateFileAsync(
                               "neusong.mp3", CreationCollisionOption.ReplaceExisting);

                     using (var stream = await File.OpenAsync(FileAccessMode.ReadWrite))
                     {
                         await stream.WriteAsync(buffer);
                         await stream.FlushAsync();
                     }
                  //  var stream = await File.OpenAsync(FileAccessMode.ReadWrite);
                   // await stream.WriteAsync(buffer);
                    //await stream.FlushAsync();
                    MyText.Text = "neusong";
                    return File;
                }
            }
            catch { }
            return null;
        }

    }
}
