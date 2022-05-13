using System;
using System.Collections.Generic;
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

namespace BoonwinsBattlegroundTracker
{
    /// <summary>
    /// Interaction logic for AddRankPrompt.xaml
    /// </summary>
    public partial class AddRankPrompt : UserControl
    {
        public AddRankPrompt()
        {
            InitializeComponent();
        }
        internal static Window _window;
        //internal static 
        public static void GetWindowName(Window window)
        {
            _window = window;
        }

        //private void btnAddRank_Click(object sender, RoutedEventArgs e)
        //{
        //    BgMatchData.AddRankManualy(Int32.Parse(ResponseTextBox.Text));
        //    //_window.Close();
        //}

        private void plus1_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.AddRankManualy(1);
            minus1.IsEnabled = true;

            minus2.IsEnabled = false;
            minus3.IsEnabled = false;
            minus4.IsEnabled = false;
            minus5.IsEnabled = false;
            minus6.IsEnabled = false;
            minus7.IsEnabled = false;
            minus8.IsEnabled = false;
        }

        private void minus1_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.RemoveRankManualy(1);
            minus1.IsEnabled = false;
        }

        private void plus2_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.AddRankManualy(2);
            minus2.IsEnabled = true;

            minus1.IsEnabled = false;
            minus3.IsEnabled = false;
            minus4.IsEnabled = false;
            minus5.IsEnabled = false;
            minus6.IsEnabled = false;
            minus7.IsEnabled = false;
            minus8.IsEnabled = false;
        }

        private void minus2_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.RemoveRankManualy(2);
            minus2.IsEnabled = false;
        }

        private void plus3_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.AddRankManualy(3);
            minus3.IsEnabled = true;

            minus2.IsEnabled = false;
            minus1.IsEnabled = false;
            minus4.IsEnabled = false;
            minus5.IsEnabled = false;
            minus6.IsEnabled = false;
            minus7.IsEnabled = false;
            minus8.IsEnabled = false;
        }

        private void minus3_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.RemoveRankManualy(3);
            minus3.IsEnabled = false;
        }

        private void plus4_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.AddRankManualy(4);
            minus4.IsEnabled = true;

            minus2.IsEnabled = false;
            minus3.IsEnabled = false;
            minus1.IsEnabled = false;
            minus5.IsEnabled = false;
            minus6.IsEnabled = false;
            minus7.IsEnabled = false;
            minus8.IsEnabled = false;
        }

        private void minus4_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.RemoveRankManualy(4);
            minus4.IsEnabled = false;
        }

        private void plus5_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.AddRankManualy(5);
            minus5.IsEnabled = true;

            minus2.IsEnabled = false;
            minus3.IsEnabled = false;
            minus4.IsEnabled = false;
            minus1.IsEnabled = false;
            minus6.IsEnabled = false;
            minus7.IsEnabled = false;
            minus8.IsEnabled = false;
        }

        private void minus5_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.RemoveRankManualy(5);

            minus5.IsEnabled = false;
        }

        private void plus6_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.AddRankManualy(6);
            minus6.IsEnabled = true;

            minus2.IsEnabled = false;
            minus3.IsEnabled = false;
            minus4.IsEnabled = false;
            minus5.IsEnabled = false;
            minus1.IsEnabled = false;
            minus7.IsEnabled = false;
            minus8.IsEnabled = false;
        }

        private void minus6_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.RemoveRankManualy(6);
            minus6.IsEnabled = false;
        }

        private void plus7_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.AddRankManualy(7);
            minus7.IsEnabled = true;

            minus2.IsEnabled = false;
            minus3.IsEnabled = false;
            minus4.IsEnabled = false;
            minus5.IsEnabled = false;
            minus6.IsEnabled = false;
            minus1.IsEnabled = false;
            minus8.IsEnabled = false;
        }

        private void minus7_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.RemoveRankManualy(7);
            minus7.IsEnabled = false;
        }

        private void plus8_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.AddRankManualy(8);
            minus8.IsEnabled = true;

            minus2.IsEnabled = false;
            minus3.IsEnabled = false;
            minus4.IsEnabled = false;
            minus5.IsEnabled = false;
            minus6.IsEnabled = false;
            minus7.IsEnabled = false;
            minus1.IsEnabled = false;
        }

        private void minus8_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.RemoveRankManualy(8);
            minus8.IsEnabled = false;
        }
    }
}
