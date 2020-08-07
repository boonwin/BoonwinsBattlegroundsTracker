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
using System.Windows.Shapes;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Newtonsoft.Json;

namespace BoonwinsBattlegroundTracker
{
    /// <summary>
    /// Interaction logic for BgMatchOverlay.xaml
    /// </summary>
    public partial class BgMatchOverlay : UserControl
    {
        SolidColorBrush mmrplus;
        SolidColorBrush mmrminus;

        public BgMatchOverlay()
        {
            InitializeComponent();
            SetImgPathes();
            LoadConfig();
        }

        public void LoadConfig()
        {
           Config config = new Config();

            if (File.Exists(Config._configLocation))
            {
                // load config from file, if available
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Config._configLocation));
                if (String.IsNullOrEmpty(config.TrackerFontColor) == false)
                {

                 SolidColorBrush brush = (SolidColorBrush)new BrushConverter().ConvertFromString(config.TrackerFontColor);

                tbAvgRankText.Foreground = brush;
                tbMmrText.Foreground = brush;
                tbTotalGames.Foreground = brush;
                tbMmrValueText.Foreground = brush;
               
                }

                if (String.IsNullOrEmpty(config.MmrPlus) == false)
                {
                    mmrplus = (SolidColorBrush)new BrushConverter().ConvertFromString(config.MmrPlus);
                    tbMmrValueCangeText.Foreground = mmrplus;
                }
                if (String.IsNullOrEmpty(config.MmrMinus) == false)
                {
                    mmrminus = (SolidColorBrush)new BrushConverter().ConvertFromString(config.MmrMinus);
                    tbMmrValueNegativeCange.Foreground = mmrminus;
                }



            }

        }

        public void SetImgPathes()
        {
            Config config = new Config();
            Uri uriTheme;

            if (File.Exists(Config._configLocation))
            {
                // load config from file, if available
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Config._configLocation));

                if (String.IsNullOrEmpty(config.backgroundImage) != true && config.backgroundImage != null)
                {
                    uriTheme = new Uri(config._themeLocation + config.backgroundImage);
                    SetBackground(uriTheme);
                }
            }

            uriTheme = new Uri(config._themeLocation + config.backgroundImage);
            SetBackground(uriTheme);
        }


        public void SetBackground(Uri fileName)
        {
           
            imgTheme.Source = new BitmapImage(fileName);

        }

        public void UpdateMMR(int mmr)
        {
            tbMmrValueText.Content = mmr;
        }

        public void UpdateTestRank(int lastRank)
        {
            tbTotalGames.Content = "Games: " + lastRank;
        }

        public void UpdateMmrChangeValue(int mmr)
        {
            //todo we need to add loading colors from file here
        
            switch (mmr)
            {
                case int n when (n > 0) :
                    tbMmrValueCangeText.Content = mmr;
                    if(mmrplus == null) { 
                    tbMmrValueCangeText.Foreground = Brushes.Lime;
                    }
                    else { tbMmrValueCangeText.Foreground = mmrplus; }
                    tbMmrValueCangeText.Visibility = Visibility.Visible;
                    tbMmrValueNegativeCange.Visibility = Visibility.Hidden;
                    
                    break;
                case int n when (n < 0):
                    tbMmrValueNegativeCange.Content = mmr;
                    if (mmrplus == null)
                    {
                        tbMmrValueNegativeCange.Foreground = Brushes.Orange;
                    }
                    else { tbMmrValueNegativeCange.Foreground = mmrminus; }
                  
                    tbMmrValueNegativeCange.Visibility = Visibility.Visible;
                    tbMmrValueCangeText.Visibility = Visibility.Hidden;

                    break;              
            }
        }

        public string SetAvgRankValue(Ranks rank)
        {

            double totalAmount = rank.rank1Amount + rank.rank2Amount + rank.rank3Amount + rank.rank4Amount + rank.rank5Amount + rank.rank6Amount + rank.rank7Amount + rank.rank8Amount;
            double weightedAmount = (1 * rank.rank1Amount) + (2 * rank.rank2Amount) + (3 * rank.rank3Amount) + (4 * rank.rank4Amount) + (5 * rank.rank5Amount) + (6 * rank.rank6Amount) + (7 * rank.rank7Amount) + (8 * rank.rank8Amount);

            if (tbTotalGames.Visibility == Visibility.Visible)
            {
                tbTotalGames.Content = "Games: " + totalAmount.ToString();
            }
            if (tbTotalGamesSmallText.Visibility == Visibility.Visible)
            {
                tbTotalGamesSmallText.Content = totalAmount.ToString();
            }
            if (totalAmount != 0)
            {
                return Math.Round((weightedAmount / totalAmount), MidpointRounding.AwayFromZero).ToString();
            }
            else return "";
        }

        public void SetTextBoxValue(Ranks rank, string avgRank)
        {
            lbRank1.Content = rank.rank1Amount.ToString() + "x";
            lbRank2.Content = rank.rank2Amount.ToString() + "x";
            lbRank3.Content = rank.rank3Amount.ToString() + "x";
            lbRank4.Content = rank.rank4Amount.ToString() + "x";
            lbRank5.Content = rank.rank5Amount.ToString() + "x";
            lbRank6.Content = rank.rank6Amount.ToString() + "x";
            lbRank7.Content = rank.rank7Amount.ToString() + "x";
            lbRank8.Content = rank.rank8Amount.ToString() + "x";

            int[] numbers = new int[] { rank.rank1Amount, rank.rank2Amount, rank.rank3Amount, rank.rank4Amount, rank.rank5Amount, rank.rank6Amount, rank.rank7Amount, rank.rank8Amount };
            int maximumNumber = numbers.Max();

            if (maximumNumber != 0)
            {
                pbRank1.Maximum = maximumNumber;
                pbRank2.Maximum = maximumNumber;
                pbRank3.Maximum = maximumNumber;
                pbRank4.Maximum = maximumNumber;
                pbRank5.Maximum = maximumNumber;
                pbRank6.Maximum = maximumNumber;
                pbRank7.Maximum = maximumNumber;
                pbRank8.Maximum = maximumNumber;
            }

            pbRank1.Value = rank.rank1Amount;
            pbRank2.Value = rank.rank2Amount;
            pbRank3.Value = rank.rank3Amount;
            pbRank4.Value = rank.rank4Amount;
            pbRank5.Value = rank.rank5Amount;
            pbRank6.Value = rank.rank6Amount;
            pbRank7.Value = rank.rank7Amount;
            pbRank8.Value = rank.rank8Amount;

            if (tbAvgRankText.Visibility == Visibility.Visible)
            {
                tbAvgRankText.Content = "Ø-Rank: " + avgRank;
            }
            if (tbAvgRankSmallText.Visibility == Visibility.Visible)
            {
                tbAvgRankSmallText.Content = avgRank;
            }
        }
    }
}
