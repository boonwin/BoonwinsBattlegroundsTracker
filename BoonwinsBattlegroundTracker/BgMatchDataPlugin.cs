using System;
using System.IO;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.API;
using Microsoft.Win32;
using Newtonsoft.Json;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;
using System.Windows;
using Hearthstone_Deck_Tracker.HsReplay;
using System.Threading;

namespace BoonwinsBattlegroundTracker
{
    public class BgMatchDataPlugin : IPlugin
    {
        private Config config;
        private BgMatchOverlay _overlay;
        private TribesOverlay _tribes;
        private OverlayManager _overlayManager;
        private TriverOverlayManager _tribeOverlayManager;

        private View _view;
        private Flyout _settingsFlyout;
        private SettingsControl _settingsControl;
        private ConsoleOverlay _console;
        private GameHistoryOverlay _history;
        private SimpleOverlay _simpleOverlay;

        public void OnLoad()
        {
            // create overlay and insert into HDT overlay
            _overlay = new BgMatchOverlay();
            _view = new View();
            _tribes = new TribesOverlay();
            _history = new GameHistoryOverlay();
            _console = new ConsoleOverlay();
            _simpleOverlay = new SimpleOverlay();

            BgMatchData._overlay = _overlay;
            BgMatchData._view = _view;
            BgMatchData._tribes = _tribes;
            BgMatchData._history = _history;
            BgMatchData._console = _console;
            BgMatchData._simpleOverlay = _simpleOverlay;

            // Triggered upon startup and when the user ticks the plugin on            
            GameEvents.OnGameStart.Add(BgMatchData.GameStart);
            GameEvents.OnTurnStart.Add(BgMatchData.TurnStart);
            GameEvents.OnGameEnd.Add(BgMatchData.GameEnd);


            CreateDateFileEnviroment();
            BgMatchData.OnLoad(config);

           


            if (config.showStatsOverlay)
            {
                MountOverlay();
            }
            _overlayManager = new OverlayManager(_overlay, config);
            _tribeOverlayManager = new TriverOverlayManager(_tribes, config);

            BgMatchData._input = _overlayManager;
            BgMatchData._tribeInput = _tribeOverlayManager;


            Canvas.SetTop(_overlay, config.posTop);
            Canvas.SetLeft(_overlay, config.posLeft);

            Canvas.SetTop(_tribes, config.tribePosTop);
            Canvas.SetLeft(_tribes, config.tribePosLeft);



            _settingsFlyout = new Flyout();
            _settingsFlyout.Name = "BgSettingsFlyout";
            _settingsFlyout.Position = Position.Left;
            Panel.SetZIndex(_settingsFlyout, 100);
            _settingsFlyout.Header = "BoonwinsBattlegroundTracker Settings";
            _settingsControl = new SettingsControl(config, MountOverlay, UnmountOverlay);
            _settingsFlyout.Content = _settingsControl;
            //_settingsFlyout.ClosingFinished += (sender, args) =>
            //{        
            //    config.save();
            //};
            Core.MainWindow.Flyouts.Items.Add(_settingsFlyout);

        }

        private void CreateDateFileEnviroment()
        {
            var pluginDateFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BoonwinsBattlegroundTracker\data\";
            if (!Directory.Exists(pluginDateFolderPath)) { 
            Directory.CreateDirectory(pluginDateFolderPath);
                Thread.Sleep(300);
            }
            if (File.Exists(Config._configLocation))
            {
                // load config from file, if available
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Config._configLocation));
            }
            else
            { // create config file
                config = new Config();
                config.save();
            }

            if (!File.Exists(config._gameRecordPath))
            {
                using (File.Create(config._gameRecordPath));
                
            }
        }

        public void MountOverlay()
        {
            StackPanel BgsTopBar = (StackPanel)Core.OverlayWindow.FindName("BgsTopBar");
            BgsTopBar.Children.Insert(1, _view);
        }


        public void UnmountOverlay()
        {
            StackPanel BgsTopBar = (StackPanel)Core.OverlayWindow.FindName("BgsTopBar");
            BgsTopBar.Children.Remove(_view);
        }

        public void OnUnload()
        {
            Core.OverlayCanvas.Children.Remove(_overlay);
            if (config.showStatsOverlay) UnmountOverlay();
        }

        public void OnButtonPress()
        {
            // Triggered when the user clicks your button in the plugin list
            _settingsFlyout.IsOpen = true;

        }

        public void OnUpdate()
        {
            BgMatchData.Update();

            if (_settingsFlyout.IsOpen == false)
            {
                _settingsControl.mmrPlus.IsChecked = false;
                _settingsControl.mmrMinus.IsChecked = false;
            }
 
        }

        public string Name => "Boonwins Battlegrounds Tracker";

        public string Description => "Save your match statistics in a local CSV file or in a Google Sheet online. Tracks the hero, ending position, minions, and the turns to reach tavern tiers for each match.";

        public string ButtonText => "Settings";

        public string Author => "Boonwin";

        public Version Version => new Version(0, 0, 13);

        public MenuItem MenuItem => CreateMenu();


        private MenuItem CreateMenu()
        {
            MenuItem m = new MenuItem { Header = "Boonwins Battlegrounds Tracker Settings" };

            m.Click += (sender, args) =>
            {
                _settingsFlyout.IsOpen = true;
            };

            return m;
        }

    }

}
