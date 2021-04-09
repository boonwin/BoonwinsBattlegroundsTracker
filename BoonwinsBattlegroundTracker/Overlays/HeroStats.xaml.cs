using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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
using BoonConector.Service;

namespace BoonwinsBattlegroundTracker
{
    /// <summary>
    /// Interaktionslogik für GameHistoryOverlay.xaml
    /// </summary>
    /// 
    public partial class HeroStats : UserControl
    {

       
        Config _config = new Config();
        List<GameRecord> _recordList = new List<GameRecord>();

        
        public HeroStats()
        {
            InitializeComponent();
         
            //eventuell wenn leer dann zeig nix an... oder dann funktioniert das feature nicht... oder er zieht sich den namen automatisch und speichert ihn in der config... 
            try
                {
                    var client = boonApiConnector.InitializeClient();
                    _recordList = GameRecord.LoadGameRecordFromApi(client, _config.player);
                    showAvgRanks();
                    imgBg.ImageSource = new BitmapImage(new Uri(Config._statsBackgroundPath));
                    mostTop3Hero();
                }
                catch
                {
                    MessageBox.Show("You cant show Stats yet, maybe you didnt play anygame so far.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                };

            }


            public void showAvgRanks()
            {
                int ranks = 0;
                int counter = 0;

                foreach (var game in _recordList)
                {
                    ranks = ranks + game.Position;
                    counter++;
                }
                lbTotalGames.Content = "Total Games: " + counter;
                if (counter > 0) lbAvgRanks.Content = "Average Rank: " + (ranks / counter).ToString();
                else lbAvgRanks.Content = "Average Rank: first Game";


            }


            public void mostTop3Hero()
            {

                var topThreePositionPerHero = _recordList
                    .Where(x => x.Position <= 3)
                    .GroupBy(t => new { Hero = t.Hero })
                    .Select(g => new
                    {
                        Hero = g.Key.Hero,
                        HeroId = _recordList.Where(x => x.Hero == g.Key.Hero && x.HeroID != null).Select(x => x.HeroID).FirstOrDefault(),
                        Count = g.Count()
                    })
                    .ToList();


                var bottomFivePositionPerHero = _recordList
                    .Where(x => x.Position > 3)
                    .GroupBy(t => new { Hero = t.Hero })
                    .Select(g => new
                    {
                        Hero = g.Key.Hero,
                        HeroId = _recordList.Where(x => x.Hero == g.Key.Hero && x.HeroID != null).Select(x => x.HeroID).FirstOrDefault(),
                        Count = g.Count()
                    })
                    .ToList();



                var _query = from heroTop in topThreePositionPerHero
                             join heroBottom in bottomFivePositionPerHero on heroTop.Hero equals heroBottom.Hero
                             select new { Hero = heroTop.Hero, Amount = heroTop.Count - heroBottom.Count, HeroId = heroTop.HeroId };


                lbBestHero.Content = "Best Hero: " + _query.Where(x => x.Amount == _query.Max(y => y.Amount)).Select(g => g.Hero).FirstOrDefault();
                var bestHeroID = _query.Where(x => x.Amount == _query.Max(y => y.Amount)).Select(g => g.HeroId).FirstOrDefault() + ".png";

                if (!String.IsNullOrEmpty(bestHeroID) && bestHeroID.Contains("TB_BaconShop_HERO"))
                {
                    var webImgPathBestHero = @"https://art.hearthstonejson.com/v1/heroes/latest/256x/" + bestHeroID;
                    imgStatsBG.Source = new BitmapImage(new Uri(webImgPathBestHero));
                }
                else
                {
                    imgStatsBG.Source = new BitmapImage(new Uri(Config._statsBestHeroBackgroundPath));
                }

                var viewList = _recordList.GroupBy(t => new { Hero = t.Hero })
                 .Select(g => new
                 {
                 //HeroImage = g.Select(l => l.HeroID),
                 Hero = g.Key.Hero,
                     Count = g.Count(),
                     Average = Math.Round(g.Average(p => p.Position), 1)
                 })
                 .OrderByDescending(o => o.Count)
                 ;



                dgGameHistory.ItemsSource = from q in _query
                                            join r in viewList on q.Hero equals r.Hero

                                            select new
                                            {
                                                Hero = r.Hero,
                                                Count = r.Count,
                                                Average = r.Average,
                                                Score = q.Amount
                                            };


            }


        }

    }