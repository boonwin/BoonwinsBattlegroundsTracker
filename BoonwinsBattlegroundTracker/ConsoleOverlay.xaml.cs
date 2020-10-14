using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace BoonwinsBattlegroundTracker
{
    /// <summary>
    /// Interaktionslogik für ConsoleOverlay.xaml
    /// </summary>
    public partial class ConsoleOverlay : UserControl
    {
        public ConsoleOverlay()
        {
            InitializeComponent();
        }

        public void SetConsoleText(string Text)
        {
            tbConsoleText.Text += "\n" + Text;
            tbConsoleText.ScrollToEnd();
        }

        //internal void btnTopMenuHide_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowHideMenu("sbHideTopMenu", btnTopMenuHide, btnTopMenuShow, pnlTopMenu);
        //}

        //internal void btnTopMenuShow_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowHideMenu("sbShowTopMenu", btnTopMenuHide, btnTopMenuShow, pnlTopMenu);
        //}

        //public void ShowHideMenu(string Storyboard, Button btnHide, Button btnShow, StackPanel pnl)
        //{
        //    Storyboard sb = Resources[Storyboard] as Storyboard;
        //    sb.Begin(pnl);

        //    if (Storyboard.Contains("Show"))
        //    {
        //        btnHide.Visibility = System.Windows.Visibility.Visible;
        //        btnShow.Visibility = System.Windows.Visibility.Hidden;
        //    }
        //    else if (Storyboard.Contains("Hide"))
        //    {
        //        btnHide.Visibility = System.Windows.Visibility.Hidden;
        //        btnShow.Visibility = System.Windows.Visibility.Visible;
        //    }
        //}

    }
}
