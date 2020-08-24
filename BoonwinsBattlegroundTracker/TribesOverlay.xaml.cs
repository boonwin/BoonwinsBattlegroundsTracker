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
    /// Interaktionslogik für TribesOverlay.xaml
    /// </summary>
    public partial class TribesOverlay : UserControl
    {   
        public TribesOverlay()
        {
            InitializeComponent();          
        }

        public void SetTribeImageSize(int index)
        {
            switch (index)
            {
                case 0:
                  
                    imgTribes.Width = 150;
                    imgTribes.Height = 150;
                    break;
                case 1:
                    
                   imgTribes.Width = 200;
                    imgTribes.Height = 200;
                    break;
                case 2:
                    
                    imgTribes.Width = 250;
                    imgTribes.Height = 250;
                    break;
                case 3:
                    
                    imgTribes.Width = 300;
                    imgTribes.Height = 300;
                    break;
            }
        }
    }
}
