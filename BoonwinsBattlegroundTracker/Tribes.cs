using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Hearthstone_Deck_Tracker.Hearthstone;
using HearthDb.Enums;
using System.Windows;

namespace BoonwinsBattlegroundTracker
{
    public class Tribes
    {

        public static HashSet<Race> GetTribes(Guid? gameID, View _view, Config _config, TribesOverlay _tribes)
        {
            var gameTribes = BattlegroundsUtils.GetAvailableRaces(gameID);
            return gameTribes;
        }

            public static void SetBannedTribes(Guid? gameID, View _view, Config _config,  TribesOverlay _tribes)
        {

            var gameTribes = BattlegroundsUtils.GetAvailableRaces(gameID);


            if (!gameTribes.Contains(Race.PIRATE) && !gameTribes.Contains(Race.MURLOC))
            {
                _view.SetisBanned("Pirates", "Murlocs");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(89, 0, 0), Color.FromRgb(4, 133, 25), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"piratemurloc.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"piratemurloc.png"));
                    }
                }

            }
            else if (!gameTribes.Contains(Race.PIRATE) && !gameTribes.Contains(Race.DEMON))
            {
                _view.SetisBanned("Pirates", "Demons");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(89, 0, 0), Color.FromRgb(82, 0, 113), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"piratedemon.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"piratedemon.png"));
                    }
                }
            }
            else if (!gameTribes.Contains(Race.PIRATE) && !gameTribes.Contains(Race.MECHANICAL))
            {
                _view.SetisBanned("Pirates", "Mechs");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(89, 0, 0), Color.FromRgb(0, 139, 137), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"piratemech.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"piratemech.png"));
                    }
                }
            }

            else if (!gameTribes.Contains(Race.PIRATE) && !gameTribes.Contains(Race.ELEMENTAL))
            {
                _view.SetisBanned("Pirates", "Elementals");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(89, 0, 0), Color.FromRgb(218, 205, 1), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"pirateelemental.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"pirateelemental.png"));
                    }
                }
            }


            else if (!gameTribes.Contains(Race.PIRATE) && !gameTribes.Contains(Race.BEAST))
            {
                _view.SetisBanned("Pirates", "Beasts");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(89, 0, 0), Color.FromRgb(113, 72, 0), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"piratebeast.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"piratebeast.png"));
                    }
                }
            }
            else if (!gameTribes.Contains(Race.PIRATE) && !gameTribes.Contains(Race.DRAGON))
            {
                _view.SetisBanned("Pirates", "Dragons");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(89, 0, 0), Color.FromRgb(0, 0, 101), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"piratedragon.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"piratedragon.png"));
                    }
                }
            }

            else if (!gameTribes.Contains(Race.BEAST) && !gameTribes.Contains(Race.MURLOC))
            {
                _view.SetisBanned("Beasts", "Murlocs");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(113, 72, 0), Color.FromRgb(4, 133, 25), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"beastmurloc.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"beastmurloc.png"));
                    }
                }
            }
            else if (!gameTribes.Contains(Race.BEAST) && !gameTribes.Contains(Race.DEMON))
            {
                _view.SetisBanned("Beasts", "Demons");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(113, 72, 0), Color.FromRgb(82, 0, 113), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"beastdemon.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"beastdemon.png"));
                    }
                }
            }
            else if (!gameTribes.Contains(Race.BEAST) && !gameTribes.Contains(Race.MECHANICAL))
            {
                _view.SetisBanned("Beasts", "Mechs");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(113, 72, 0), Color.FromRgb(0, 139, 137), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"beastmech.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"beastmech.png"));
                    }
                }
            }
            else if (!gameTribes.Contains(Race.BEAST) && !gameTribes.Contains(Race.ELEMENTAL))
            {
                _view.SetisBanned("Beasts", "Elementals");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(113, 72, 0), Color.FromRgb(218, 205, 1), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"beastelemental.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"beastelemental.png"));
                    }
                }
            }
            else if (!gameTribes.Contains(Race.BEAST) && !gameTribes.Contains(Race.DRAGON))
            {
                _view.SetisBanned("Beasts", "Dragons");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(113, 72, 0), Color.FromRgb(0, 0, 101), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"beastdragon.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"beastdragon.png"));
                    }
                }
            }

            else if (!gameTribes.Contains(Race.DEMON) && !gameTribes.Contains(Race.MURLOC))
            {
                _view.SetisBanned("Demons", "Murlocs");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(82, 0, 113), Color.FromRgb(4, 133, 25), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"demonmurloc.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"demonmurloc.png"));
                    }
                }
            }

            else if (!gameTribes.Contains(Race.DEMON) && !gameTribes.Contains(Race.MECHANICAL))
            {
                _view.SetisBanned("Demons", "Mechs");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(82, 0, 113), Color.FromRgb(0, 139, 137), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"demonmech.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"demonmech.png"));
                    }
                }
            }

            else if (!gameTribes.Contains(Race.DEMON) && !gameTribes.Contains(Race.ELEMENTAL))
            {
                _view.SetisBanned("Demons", "Elementals");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(82, 0, 113), Color.FromRgb(218, 205, 1), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"demonelemental.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"demonelemental.png"));
                    }
                }
            }

            else if (!gameTribes.Contains(Race.DEMON) && !gameTribes.Contains(Race.DRAGON))
            {
                _view.SetisBanned("Demons", "Dragons");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(82, 0, 113), Color.FromRgb(0, 0, 101), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"demondragon.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"demondragon.png"));
                    }
                }
            }

            else if (!gameTribes.Contains(Race.DRAGON) && !gameTribes.Contains(Race.MURLOC))
            {
                _view.SetisBanned("Dragons", "Murlocs");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(0, 0, 101), Color.FromRgb(4, 133, 25), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"dragonmurloc.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"dragonmurloc.png"));
                    }
                }
            }

            else if (!gameTribes.Contains(Race.DRAGON) && !gameTribes.Contains(Race.MECHANICAL))
            {
                _view.SetisBanned("Dragons", "Mechs");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(0, 0, 101), Color.FromRgb(0, 139, 137), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"dragonmech.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"dragonmech.png"));
                    }
                }
            }


            else if (!gameTribes.Contains(Race.DRAGON) && !gameTribes.Contains(Race.ELEMENTAL))
            {
                _view.SetisBanned("Dragons", "Elementals");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(0, 0, 101), Color.FromRgb(218, 205, 1), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"dragonelemental.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"dragonelemental.png"));
                    }
                }
            }


            else if (!gameTribes.Contains(Race.ELEMENTAL) && !gameTribes.Contains(Race.MURLOC))
            {
                _view.SetisBanned("Elementals", "Murlocs");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(218, 205, 1), Color.FromRgb(4, 133, 25), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"elementalmurloc.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"elementalmurloc.png"));
                    }
                }
            }

            else if (!gameTribes.Contains(Race.ELEMENTAL) && !gameTribes.Contains(Race.MECHANICAL))
            {
                _view.SetisBanned("Elementals", "Mechs");
                if (_config.showTribeColors == true)
                {
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(218, 205, 1), Color.FromRgb(0, 139, 137), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"elementalmech.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"elementalmech.png"));
                    }
                }
            }


            else if (!gameTribes.Contains(Race.MECHANICAL) && !gameTribes.Contains(Race.MURLOC))
            {
                _view.SetisBanned("Mechs", "Murlocs");
                if (_config.showTribeColors == true)
                {

                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(0, 139, 137), Color.FromRgb(4, 133, 25), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;

                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"mechmurloc.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"mechmurloc.png"));
                    }
                }
            }

            else if (gameTribes.Contains(Race.INVALID))
            {
                _view.SetisBanned("N/A", "");
                if (_config.showTribeColors == true)
                {

                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(0, 0, 0), Color.FromRgb(0, 0, 0), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"na.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"na.png"));
                    }
                }

            }
            else
            {
                _view.SetisBanned("N/A", "");
                if (_config.showTribeColors == true)
                {

                    LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(0, 0, 0), Color.FromRgb(0, 0, 0), new Point(0.5, 0), new Point(0.5, 1));
                    _view.spBanned.Background = gradientBrush;
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"na.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"na.png"));
                    }
                }
            }


        }

    }
}
