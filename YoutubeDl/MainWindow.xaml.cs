using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YoutubeDl
{

    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            try
            {
                _vm = MainWindowVm.Deserialize();
            }
            catch (Exception e)
            {
                _vm = new MainWindowVm();
            }

            MainGrid.DataContext = _vm;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _vm.Serialize();
        }

        MainWindowVm _vm ;


        private void ButtonBase_OnDownload(object sender, RoutedEventArgs e)
        {
            var yd = new YoutubeDownloader(_vm);
            int format = _vm.FormatNum;
            yd.GetVideo(TbUrl.Text, format);
        
    }

        private async void ButtonBase_OnGetFormats(object sender, RoutedEventArgs e)
        {
            var yd = new YoutubeDownloader(_vm);
            await yd.GetVideoFormats(TbUrl.Text);
           // _vm.Serialize();
        }

        private void TbLog_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TbLog.ScrollToEnd();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.IsSelected)
            {
                if (item.Content is YoutubeInfo cont)
                {
                    //_vm.FormatNum = cont.FormatNum;
                }
            }
        }

        private void TbUrl_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var cb = Clipboard.GetText(TextDataFormat.Text);
            if (string.IsNullOrWhiteSpace(TbUrl.Text) && cb.StartsWith("http")) TbUrl.Text = Clipboard.GetText();
        }
    }
}
