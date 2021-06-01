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

namespace BoonwinsBattlegroundTracker.Overlays
{
    /// <summary>
    /// Interaction logic for InBattleMmrScore.xaml
    /// </summary>
    public partial class InBattleMmrScore : UserControl
    {
        public List<string> CurrentBannedTribes { get; set; }

        public InBattleMmrScore()
        {
            InitializeComponent();
            CurrentBannedTribes = new List<string>();
            DataContext = this;
        }
        public void ShowBannedTribes(string[] bannedTribes)
        {
          
            foreach (var tribe in bannedTribes)
            {
                CurrentBannedTribes.Add(tribe.Trim());
            }
            lvBannedTribes.ItemsSource = CurrentBannedTribes;

        }

        public void RemoveBannedTribes()
        {

            CurrentBannedTribes.Clear();
            lvBannedTribes.ItemsSource = null;

        }

        internal void SetPeak(string peak)
        {
            lbPeak.Content = "Peak: " + peak;
        }

        internal void SetMMR(int rating)
        {

            lbrMMR.Content = "MMR: " + rating;
        }

    }
}
