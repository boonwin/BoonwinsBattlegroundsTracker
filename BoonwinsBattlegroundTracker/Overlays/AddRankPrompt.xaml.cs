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

        private void btnAddRank_Click(object sender, RoutedEventArgs e)
        {
            BgMatchData.AddRankManualy(Int32.Parse(ResponseTextBox.Text));
            _window.Close();
        }
    }
}
