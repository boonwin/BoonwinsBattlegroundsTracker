using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace BoonwinsBattlegroundTracker.Overlays
{
    /// <summary>
    /// Interaction logic for OverlayDesigner.xaml
    /// </summary>
    public partial class OverlayDesigner : UserControl
    {
        private BgMatchOverlay _overlay = new BgMatchOverlay();
        public OverlayDesigner(Config c)
        {
            InitializeComponent();
            _config = c;
        }

        internal static Window _window;
        private Config _config;
        
        TribesOverlay _tribes = new TribesOverlay();

        void OverlayDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
        }
 
        private void cpPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            
           _overlay.tbAvgRankText.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);
           _overlay.tbMmrText.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);
           _overlay.tbTotalGames.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);
           _overlay.tbMmrValueText.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);
            
            _config.TrackerFontColor = cpPickerTextColor.SelectedColor.Value.ToString();
            _config.save();
            _overlay.UpdateLayout();
            this.UpdateLayout();
            CenterFrame.Refresh();

        }

        private void cpPickerPlusMMR_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

            _overlay.tbMmrValueCangeText.Foreground = new SolidColorBrush(cpPickerPlusMMR.SelectedColor.Value);
            
            _config.MmrPlus = cpPickerPlusMMR.SelectedColor.Value.ToString();
            _config.save();
            _overlay.UpdateLayout();
            
            this.UpdateLayout();
            CenterFrame.Refresh();
        }

        private void cpPickerMinusMMR_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

            _overlay.tbMmrValueNegativeCange.Foreground = new SolidColorBrush(cpPickerMinusMMR.SelectedColor.Value);
            
            _config.MmrMinus = cpPickerMinusMMR.SelectedColor.Value.ToString();
            _config.save();
            _overlay.UpdateLayout();
            this.UpdateLayout();
            CenterFrame.Refresh();
        }

        //private void cbBannedTribeImagesSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //    var comboBox = sender as ComboBox;


        //    _tribes.SetTribeImageSize(comboBox.SelectedIndex);
        //    switch (comboBox.SelectedIndex)
        //    {
        //        case 0:
        //            _config.tribeSize = 0;
        //            _config.save();
        //            break;
        //        case 1:
        //            _config.tribeSize = 1;
        //            _config.save();
        //            break;
        //        case 2:
        //            _config.tribeSize = 2;
        //            _config.save();
        //            break;
        //        case 3:
        //            _config.tribeSize = 3;
        //            _config.save();
        //            break;
        //    }


        //}

      
        private void mmrPlus_Checked(object sender, RoutedEventArgs e)
        {

            _overlay.tbMmrValueCangeText.Visibility = Visibility.Visible;
            _overlay.UpdateLayout();

            this.UpdateLayout();
            CenterFrame.Refresh();
        }

        private void mmrPlus_Unchecked(object sender, RoutedEventArgs e)
        {
            _overlay.tbMmrValueCangeText.Visibility = Visibility.Hidden;
            _overlay.UpdateLayout();

            this.UpdateLayout();
            CenterFrame.Refresh();
        }

        private void mmrMinus_Checked(object sender, RoutedEventArgs e)
        {
            _overlay.tbMmrValueNegativeCange.Visibility = Visibility.Visible;
            _overlay.UpdateLayout();

            this.UpdateLayout();
            CenterFrame.Refresh();
        }

        private void mmrMinus_Unchecked(object sender, RoutedEventArgs e)
        {

            _overlay.tbMmrValueNegativeCange.Visibility = Visibility.Hidden;
            _overlay.UpdateLayout();

            this.UpdateLayout();
            CenterFrame.Refresh();
        }

        public static void GetWindowName(Window window)
        {
            _window = window;
        }
        internal static void Close()
        {

            _window.Close();
        }

        private void btnBackgroundImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string pathAndFilename = dlg.FileName;
                string fileName = dlg.SafeFileName;
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BoonwinsBattlegroundTracker\data\";
                if (!File.Exists(path+fileName)) {
                    File.Copy(pathAndFilename, path + fileName, true);
                    _config.backgroundImage = fileName;
                    _config.save();
                    _overlay.UpdateLayout();
                    this.UpdateLayout();
                    CenterFrame.Refresh();
                } else MessageBox.Show("File name already exists, please use different Filename.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _overlay.UpdateLayout();
            this.UpdateLayout();
            CenterFrame.Refresh();
        }
    }
}
