using System;
using System.IO;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Plugins;
using Microsoft.Win32;
using Newtonsoft.Json;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;
using System.Windows;
using Hearthstone_Deck_Tracker.HsReplay;
using System.Threading;
using AutoUpdaterDotNET;
using System.Threading.Tasks;
using Hearthstone_Deck_Tracker.API;
using BoonwinsBattlegroundTracker.Overlays;

namespace BoonwinsBattlegroundTracker
{
    public class BgMatchDataPlugin : IPlugin
    {
        private Config _config;
        private BgMatchOverlay _overlay;
        private OverlayManager _overlayManager;
        private TribesOverlay _tribes;
      
        private TriverOverlayManager _tribeOverlayManager;
        private InGameDisconectorOverlay _inGameDisconectorOverlay;

        private InBattleMmrScore _mmrView;
        private Flyout _settingsFlyout;
        private SettingsControl _settingsControl;
        private ConsoleOverlay _console;

        private SimpleOverlay _simpleOverlay;

        public void OnLoad()
        {
            // create overlay and insert into HDT overlay
            AutoUpdate();
            CreateDateFileEnviroment();

            _overlay = new BgMatchOverlay();
            _mmrView = new InBattleMmrScore();
            _tribes = new TribesOverlay();
            _inGameDisconectorOverlay = new InGameDisconectorOverlay();

            _console = new ConsoleOverlay();
            _simpleOverlay = new SimpleOverlay();

            BgMatchData._overlay = _overlay;
            BgMatchData._mmrView = _mmrView;
            BgMatchData._tribes = _tribes;
            BgMatchData._cheatButtonForNoobs = _inGameDisconectorOverlay;

            //BgMatchData._console = _console;
            BgMatchData._simpleOverlay = _simpleOverlay;

            // Triggered upon startup and when the user ticks the plugin on            
            GameEvents.OnGameStart.Add(BgMatchData.GameStart);
            GameEvents.OnInMenu.Add(BgMatchData.InMenu);
            GameEvents.OnTurnStart.Add(BgMatchData.TurnStart);
            GameEvents.OnGameEnd.Add(BgMatchData.GameEnd);


           

            BgMatchData.OnLoad(_config);

           


            if (_config.showStatsOverlay)
            {
                MountOverlay();
            }
            _overlayManager = new OverlayManager(_overlay, _config);           
            _tribeOverlayManager = new TriverOverlayManager(_tribes, _config);

            BgMatchData._input = _overlayManager;
            BgMatchData._tribeInput = _tribeOverlayManager;
            

            Canvas.SetTop(_overlay, _config.posTop);
            Canvas.SetLeft(_overlay, _config.posLeft);

            Canvas.SetTop(_inGameDisconectorOverlay, 50);
            Canvas.SetLeft(_inGameDisconectorOverlay, 300);

            Canvas.SetTop(_tribes, _config.tribePosTop);
            Canvas.SetLeft(_tribes, _config.tribePosLeft);



            _settingsFlyout = new Flyout();
            _settingsFlyout.Name = "BgSettingsFlyout";
            _settingsFlyout.Position = Position.Left;
            Panel.SetZIndex(_settingsFlyout, 100);
            _settingsFlyout.Header = "BoonwinsBattlegroundTracker Settings";
            _settingsControl = new SettingsControl(_config, MountOverlay, UnmountOverlay);
            _settingsFlyout.Content = _settingsControl;
            //_settingsFlyout.ClosingFinished += (sender, args) =>
            //{        
            //    config.save();
            //};
            Core.MainWindow.Flyouts.Items.Add(_settingsFlyout);

        }


        private void AutoUpdate()
        {
            AutoUpdater.InstalledVersion = Version;
            AutoUpdater.AppTitle = "Boonwins Battlegrounds Tracker";
            AutoUpdater.Start("https://boonwin.de/Downloads/version.xml");
            AutoUpdater.DownloadPath = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\";
            var currentDirectory = new DirectoryInfo(Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BoonwinsBattlegroundTracker\");
            if (currentDirectory.Parent != null)
            {
                AutoUpdater.InstallationPath = currentDirectory.Parent.FullName;
            }
            
        }
        private void CreateDateFileEnviroment()
        {
            var pluginDateFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BoonwinsBattlegroundTracker\data\";
            if (!Directory.Exists(pluginDateFolderPath)) { 
            Directory.CreateDirectory(pluginDateFolderPath);
            }
            if (File.Exists(Config._configLocation))
            {
                // load config from file, if available
                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Config._configLocation));
            }
            else
            { // create config file
                _config = new Config();
                _config.save();
            } 
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BoonwinsBattlegroundTracker\data\baseTheme.png") && _config._themeLocation != Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BoonwinsBattlegroundTracker\data\") 
            {
                _config._themeLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BoonwinsBattlegroundTracker\data\";
                _config.save();
            }


            if (!File.Exists(_config._gameRecordPath))
            {
                using (File.Create(_config._gameRecordPath));
                
            }
        }

        public void MountOverlay()
        {
            StackPanel BgsTopBar = (StackPanel)Core.OverlayWindow.FindName("BgsTopBar");
            BgsTopBar.Children.Insert(1, _mmrView);
        }


        public void UnmountOverlay()
        {
            StackPanel BgsTopBar = (StackPanel)Core.OverlayWindow.FindName("BgsTopBar");
            BgsTopBar.Children.Remove(_mmrView);
        }

        public void OnUnload()
        {
            Core.OverlayCanvas.Children.Remove(_overlay);
            if (_config.showStatsOverlay) UnmountOverlay();
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

        public string Description => "Shows your reached Ranks, Banned Tribes and best Heroes. Soon there will be even more, still in Progress :)";

        public string ButtonText => "Settings";

        public string Author => "Boonwin";

        public Version Version => new Version(0, 0, 1, 12);

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
