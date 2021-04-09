using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace BoonwinsBattlegroundTracker
{
    /// <summary>
    /// Interaction logic for InGameDisconectorOverlay.xaml
    /// </summary>
    public partial class InGameDisconectorOverlay : UserControl
    {
        public InGameDisconectorOverlay()
        {
            InitializeComponent();
            this.Loaded += InGameDisconectorOverlay_Loaded;
        }
        internal static Config _config = new Config();
        internal static Window _window;
        void InGameDisconectorOverlay_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Closing += window_Closing;
        }
        void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {
            _config.DisconectWindowOpen = false;
            _config.save();
        }
       
        private async void btnDisconectToggle_Click(object sender, RoutedEventArgs e)
        {
            
            borStatus.Background = Brushes.Red;
            btnDisconectToggle.Content = "Reconecting...";

            var conStatus = BgMatchData.ToggleDisconect();
            if (conStatus == 1)
            {
                await Task.Delay(3000);
                BgMatchData.ToggleDisconect();
                await Task.Delay(3000);
                btnDisconectToggle.Content = "Skip Fight";
                borStatus.Background = Brushes.Green;
            }      

        }

        //private void cbSkipAll_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (MessageBox.Show("This will Skip every fight automatically, do you really want that?", "Skip all?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes){
        //        MessageBox.Show("You filthy low life!", "Noob", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        _config.SkipAll = true;
        //        _config.save();
        //    };

        //}

        //private void cbSkipAll_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    _config.SkipAll = false;
        //    _config.save();
        //}
        public static void GetWindowName(Window window)
        {
            _window = window;
        }
        internal static void Close()
        {
            
            _window.Close();
        }
    }
}
