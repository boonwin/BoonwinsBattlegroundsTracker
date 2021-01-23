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
        }

        private async void btnDisconectToggle_Click(object sender, RoutedEventArgs e)
        {
            
            borStatus.Background = Brushes.Red;
            btnDisconectToggle.Content = "Reconecting...";

            var conStatus = BgMatchData.ToggleDisconect();
            if (conStatus == 1)
            {
                await Task.Delay(1000);
                BgMatchData.ToggleDisconect();
                await Task.Delay(1000);
                btnDisconectToggle.Content = "Skip Fight";
                borStatus.Background = Brushes.Green;
            }      

        }



    }
}
