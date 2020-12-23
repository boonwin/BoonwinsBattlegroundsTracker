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
    /// Interaktionslogik für SimpleOverlay.xaml
    /// </summary>
    public partial class SimpleOverlay : UserControl
    {
        public SimpleOverlay()
        {
            InitializeComponent();
        }

        public void SetLastRank(int rank)
        {
               
          tbRanks.Text = tbRanks.Text + rank.ToString() + ", ";
                
        }
    }
}
