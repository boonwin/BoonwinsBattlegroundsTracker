using HearthDb.Enums;

using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.Utility;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;
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


namespace BoonwinsBattlegroundTracker
{
    public partial class SettingsControl : UserControl
    {

        BgMatchOverlay _overlay = new BgMatchOverlay();

        private Action _mount;
        private Action _unmount;
        private Config _config;
        public string bgFileName;
        public SettingsControl(Config c, Action mount, Action unmount)
        {
            InitializeComponent();
            _config = c;
            UpdateConfig(c);
            _mount = mount;
            _unmount = unmount;

            IsBigMenuEnabled();
        }

        public void IsBigMenuEnabled()
        {
            if (_config.menuOverlayEnabled == true)
            {
                cbIsBigEmanled.IsChecked = true;
            }
            else { cbIsBigEmanled.IsChecked = false; }
        }

        public void UpdateConfig(Config c)
        {
           
                  
         }
        
        private void Mount(object sender, RoutedEventArgs e)
        {
            _mount();
            _config.showStatsOverlay = true;
            
        }

        private void Unmount(object sender, RoutedEventArgs e)
        {
           
            _unmount();
            _config.showStatsOverlay = false;
           
        }

        private void cpPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
     
            BgMatchData.Overlay.tbAvgRankText.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);
            BgMatchData.Overlay.tbMmrText.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);
            BgMatchData.Overlay.tbTotalGames.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);
            BgMatchData.Overlay.tbMmrValueText.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);

            _config.TrackerFontColor = cpPickerTextColor.SelectedColor.Value.ToString();
            _config.save();

        }

        private void cpPickerPlusMMR_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            BgMatchData.Overlay.tbMmrValueCangeText.Foreground = new SolidColorBrush(cpPickerPlusMMR.SelectedColor.Value);

            _config.MmrPlus = cpPickerPlusMMR.SelectedColor.Value.ToString();
            _config.save();
          
        }

        private void cpPickerMinusMMR_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            BgMatchData.Overlay.tbMmrValueNegativeCange.Foreground = new SolidColorBrush(cpPickerMinusMMR.SelectedColor.Value);

            _config.MmrMinus = cpPickerMinusMMR.SelectedColor.Value.ToString();
            _config.save();
    
        }

        private void mmrPlus_Checked(object sender, RoutedEventArgs e)
        {
            BgMatchData.Overlay.tbMmrValueCangeText.Visibility = Visibility.Visible;
        }

        private void mmrPlus_Unchecked(object sender, RoutedEventArgs e)
        {
            BgMatchData.Overlay.tbMmrValueCangeText.Visibility = Visibility.Hidden;
        }

        private void mmrMinus_Checked(object sender, RoutedEventArgs e)
        {
            BgMatchData.Overlay.tbMmrValueNegativeCange.Visibility = Visibility.Visible;
        }

        private void mmrMinus_Unchecked(object sender, RoutedEventArgs e)
        {
            BgMatchData.Overlay.tbMmrValueNegativeCange.Visibility = Visibility.Hidden;
        }

   
        private void cbIsRight_Checked(object sender, RoutedEventArgs e)
        {
            _config.screenIsRight = true;
            _config.save();
        }

        private void cbIsRight_Unchecked(object sender, RoutedEventArgs e)
        {
            _config.screenIsRight = false;
            _config.save();
        }

        private void cbScreenwidth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           ComboBoxItem item = (ComboBoxItem)cbScreenwidth.SelectedItem;
           var windowWidth = Int32.Parse(item.Content.ToString());
            cbIsRight.IsEnabled = true;
            _config.screenWidth = windowWidth;
            _config.save();
        }

        private void cbIsMenuOverlay_Checked(object sender, RoutedEventArgs e)
        {
            
            _config.menuOverlayEnabled = true;
            _config.save();

            if (Core.Game.CurrentMode == Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.BACON)
                {
                     if (_config.menuOverlayEnabled == true)
                     {

                    if (Core.OverlayCanvas.Children.Contains(_overlay) == false)
                    {
                        Core.OverlayCanvas.Children.Add(_overlay);
                    }

                }
                else
                {
                    
                        Core.OverlayCanvas.Children.Remove(_overlay);
                    
                }

            }
          

        }

        private void cbIsMenuOverlay_Unchecked(object sender, RoutedEventArgs e)
        {
            _config.menuOverlayEnabled = false;
            _config.save();
            if (Core.Game.CurrentMode == Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.BACON)
            {
                if (_config.menuOverlayEnabled == false)
                     {

                
                    if (Core.OverlayCanvas.Children.Contains(_overlay) == true)
                    {
                        Core.OverlayCanvas.Children.Remove(_overlay);
                    }

                }

            } else
            {
                
                    Core.OverlayCanvas.Children.Remove(_overlay);
                
            }
        }
    }
}
