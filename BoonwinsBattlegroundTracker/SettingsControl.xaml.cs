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
        TribesOverlay _tribes = new TribesOverlay();
        ConsoleOverlay _console = new ConsoleOverlay();
        SimpleOverlay _simpleOverlay = new SimpleOverlay();

        private Action _mount;
        private Action _unmount;
        private Config _config;
        public string bgFileName;
        public SettingsControl(Config c, Action mount, Action unmount)
        {
            InitializeComponent();
            cbLeaderboardName.TextChanged += cbLeaderboardName_TextChanged;
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
            if (_config.showTribeColors == true)
            {
                cbEnableBannedTribeColors.IsChecked = true;
            }
            else { cbEnableBannedTribeColors.IsChecked = false; }
            if (_config.showTribeImages == true)
            {
                cbEnableBannedTribeImages.IsChecked = true;
            }
            else { cbEnableBannedTribeImages.IsChecked = false; }
            if (_config.ingameOverlayEnabled == true)
            {
                cbIsInGameEnabled.IsChecked = true;
                cbBannedTribeImagesSizes.IsEnabled = true;
            }
            else { cbIsInGameEnabled.IsChecked = false;
                cbBannedTribeImagesSizes.IsEnabled = false;
            }

            if (_config.isSoundChecked == true)
            {
                cbSounds.IsChecked = true;
            }
            else
            {
                cbSounds.IsChecked = false;
            }

            if (_config.showSimpleOverlay == true) cbSimpleOverlay.IsChecked = true;
            else cbSimpleOverlay.IsChecked = false;

            if (_config.isLeaderboardActivated)
            {
                cbLeaderboard.IsChecked = true;
                LeaderboardNamePanel.IsEnabled = true;
                
            }
            else { cbLeaderboard.IsChecked = false;
                LeaderboardNamePanel.IsEnabled = false;

            }
            if (!String.IsNullOrEmpty(_config.leaderboardName))
            {
                cbLeaderboardName.Text = _config.leaderboardName;
            }
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

            BgMatchData._overlay.tbAvgRankText.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);
            BgMatchData._overlay.tbMmrText.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);
            BgMatchData._overlay.tbTotalGames.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);
            BgMatchData._overlay.tbMmrValueText.Foreground = new SolidColorBrush(cpPickerTextColor.SelectedColor.Value);

            _config.TrackerFontColor = cpPickerTextColor.SelectedColor.Value.ToString();
            _config.save();

        }

        private void cpPickerPlusMMR_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            BgMatchData._overlay.tbMmrValueCangeText.Foreground = new SolidColorBrush(cpPickerPlusMMR.SelectedColor.Value);

            _config.MmrPlus = cpPickerPlusMMR.SelectedColor.Value.ToString();
            _config.save();

        }

        private void cpPickerMinusMMR_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            BgMatchData._overlay.tbMmrValueNegativeCange.Foreground = new SolidColorBrush(cpPickerMinusMMR.SelectedColor.Value);

            _config.MmrMinus = cpPickerMinusMMR.SelectedColor.Value.ToString();
            _config.save();

        }

        private void mmrPlus_Checked(object sender, RoutedEventArgs e)
        {
            BgMatchData._overlay.tbMmrValueCangeText.Visibility = Visibility.Visible;
        }

        private void mmrPlus_Unchecked(object sender, RoutedEventArgs e)
        {
            BgMatchData._overlay.tbMmrValueCangeText.Visibility = Visibility.Hidden;
        }

        private void mmrMinus_Checked(object sender, RoutedEventArgs e)
        {
            BgMatchData._overlay.tbMmrValueNegativeCange.Visibility = Visibility.Visible;
        }

        private void mmrMinus_Unchecked(object sender, RoutedEventArgs e)
        {
            BgMatchData._overlay.tbMmrValueNegativeCange.Visibility = Visibility.Hidden;
        }


        private void cbIsMenuOverlay_Checked(object sender, RoutedEventArgs e)
        {

            _config.menuOverlayEnabled = true;
            _config.save();

            //if (Core.Game.CurrentMode == Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.BACON)
            //{
            //    if (_config.menuOverlayEnabled == true)
            //    {

            //        if (!Core.OverlayCanvas.Children.Contains(_overlay))
            //        {
            //            Core.OverlayCanvas.Children.Add(_overlay);
            //        }

            //    }
            //    else
            //    {
            //        if (Core.OverlayCanvas.Children.Contains(_overlay)) { 
            //        Core.OverlayCanvas.Children.Remove(_overlay);
            //        }
            //    }

            //}


        }

        private void cbIsMenuOverlay_Unchecked(object sender, RoutedEventArgs e)
        {
            _config.menuOverlayEnabled = false;
            _config.save();
            //if (Core.Game.CurrentMode == Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.BACON)
            //{
            //    if (_config.menuOverlayEnabled == false)
            //    {


            //        if (Core.OverlayCanvas.Children.Contains(_overlay))
            //        {
            //            Core.OverlayCanvas.Children.Remove(_overlay);
            //        }

            //    }

            //}
        
        }

        private void cbIsInGameOverlay_Checked(object sender, RoutedEventArgs e)
        {
            _config.ingameOverlayEnabled = true;
            _config.save();
        }

        private void cbIsInGameOverlay_Unchecked(object sender, RoutedEventArgs e)
        {
            _config.ingameOverlayEnabled = false;
            _config.save();
        }

        private void BtnUnlockOverlay_Click(object sender, RoutedEventArgs e)
        {
            btnUnlock.Content = BgMatchData._input.Toggle() ? "Lock Ranks" : "Unlock Ranks";
        }

        private void cbEnableBannedTribeColors_Checked(object sender, RoutedEventArgs e)
        {
            _config.showTribeColors = true;
            _config.save();
        }

        private void cbEnableBannedTribeColors_Unchecked(object sender, RoutedEventArgs e)
        {
            _config.showTribeColors = false;
            _config.save();
        }

        private void cbEnableBannedTribeImages_Checked(object sender, RoutedEventArgs e)
        {
            _config.showTribeImages = true;
            cbBannedTribeImagesSizes.IsEnabled = true;
            _config.save();
        }

        private void cbEnableBannedTribeImages_Unchecked(object sender, RoutedEventArgs e)
        {
            _config.showTribeImages = false;
            cbBannedTribeImagesSizes.IsEnabled = false;
            _config.save();
        }

        private void btntribesUnlock_Click(object sender, RoutedEventArgs e)
        {

            btntribesUnlock.Content = BgMatchData._tribeInput.Toggle() ? "Lock Tribes" : "Unlock Tribes";

        }

        private void cbBannedTribeImagesSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var comboBox = sender as ComboBox;


            _tribes.SetTribeImageSize(comboBox.SelectedIndex);
            switch(comboBox.SelectedIndex)
            {
                case 0:
                    _config.tribeSize = 0;
                    _config.save();
                    break;
                case 1:
                    _config.tribeSize = 1;
                    _config.save();
                    break;
                case 2:
                    _config.tribeSize = 2;
                    _config.save();
                    break;
                case 3:
                    _config.tribeSize = 3;
                    _config.save();
                    break;
            }
            

        }

        private void cbBannedTribeImagesSizes_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("150x150");
            data.Add("200x200");
            data.Add("250x250");
            data.Add("300x300");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;


            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            // ... Make the first item selected.
            if(_config.tribeSize != 0)
            {
                comboBox.SelectedIndex = _config.tribeSize;
            }else comboBox.SelectedIndex = 0;
        }

        private void btnHideConsole_Click(object sender, RoutedEventArgs e)
        {
            _config.showConsole = false;
            _config.save();
            ShowHideMenu("sbHideTopMenu", btnTopMenuHide, btnTopMenuShow);
        }

        private void btnShowConsole_Click(object sender, RoutedEventArgs e)
        {
            _config.showConsole = true;
            _config.save();
            ShowHideMenu("sbShowTopMenu", btnTopMenuHide, btnTopMenuShow);
        }

        public void ShowHideMenu(string Storyboard, Button btnHide, Button btnShow)
        {
            

            if (Storyboard.Contains("Show"))
            {
                btnHide.Visibility = System.Windows.Visibility.Visible;
                btnShow.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (Storyboard.Contains("Hide"))
            {
                btnHide.Visibility = System.Windows.Visibility.Hidden;
                btnShow.Visibility = System.Windows.Visibility.Visible;
            }
        }

     
        private void cbLeaderboard_Checked(object sender, RoutedEventArgs e)
        {
           _config.isLeaderboardActivated = true;
           LeaderboardNamePanel.IsEnabled = true;
           _config.save();
        }

        private void cbLeaderboard_Unchecked(object sender, RoutedEventArgs e)
        {
            _config.isLeaderboardActivated = false;
            LeaderboardNamePanel.IsEnabled = false;
            _config.save();
        }       
        private void cbLeaderboardName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = sender as TextBox;
            _config.leaderboardName = text.Text;
            _config.save();
        }

        private void cbSounds_Checked(object sender, RoutedEventArgs e)
        {
            _config.isSoundChecked = true;
            _config.save();
        }

        private void cbSounds_Unchecked(object sender, RoutedEventArgs e)
        {
            _config.isSoundChecked = false;
            _config.save();
        }

        private void cbSimpleOverlay_Checked(object sender, RoutedEventArgs e)
        {
            _config.showSimpleOverlay = true;
            _config.save();
        }

        private void cbSimpleOverlay_Unchecked(object sender, RoutedEventArgs e)
        {
            _config.showSimpleOverlay = false;
            _config.save();
        }

   

        private void btnStatsShow_Click(object sender, RoutedEventArgs e)
        {
            Window gameHistoryOverlay = new Window()
            {
                Title = "Boonwin",
                Content = new GameHistoryOverlay(),
               
                ResizeMode = ResizeMode.CanResizeWithGrip
            };

            gameHistoryOverlay.Show();
        
        }
    }
}
