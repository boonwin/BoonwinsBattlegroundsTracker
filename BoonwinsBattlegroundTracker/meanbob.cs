using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;

namespace BoonwinsBattlegroundTracker
{
   public class meanbob
    {

        

        public static void meanBobLines(int rank, Config _config)
        {
            
            if (_config.IsMeanBobChecked)
            {
                switch (rank)
                {
                    case 1:
                        if (File.Exists(_config._soundLocation + "1.wav"))
                        {
                            SoundPlayer player = new SoundPlayer(_config._soundLocation + "1.wav");
                            player.Play();
                        }
                        break;
                    case 2:
                        if (File.Exists(_config._soundLocation + "2.wav"))
                        {
                            SoundPlayer player = new SoundPlayer(_config._soundLocation + "2.wav");
                            player.Play();
                        }
                        break;
                    case 3:
                        if (File.Exists(_config._soundLocation + "3.wav"))
                        {
                            SoundPlayer player = new SoundPlayer(_config._soundLocation + "3.wav");
                            player.Play();
                        }
                        break;
                    case 4:
                        if (File.Exists(_config._soundLocation + "4.wav"))
                        {
                            SoundPlayer player = new SoundPlayer(_config._soundLocation + "4.wav");
                            player.Play();
                        }
                        break;
                    case 5:
                        if (File.Exists(_config._soundLocation + "5.wav"))
                        {
                            SoundPlayer player = new SoundPlayer(_config._soundLocation + "5.wav");
                            player.Play();
                        }
                        break;
                    case 6:
                        if (File.Exists(_config._soundLocation + "6.wav"))
                        {
                            SoundPlayer player = new SoundPlayer(_config._soundLocation + "6.wav");
                            player.Play();
                        }
                        break;
                    case 7:
                        if (File.Exists(_config._soundLocation + "7.wav"))
                        {
                            SoundPlayer player = new SoundPlayer(_config._soundLocation + "7.wav");
                            player.Play();
                        }
                        break;
                    case 8:
                        if (File.Exists(_config._soundLocation + "8.wav"))
                        {
                            SoundPlayer player = new SoundPlayer(_config._soundLocation + "8.wav");
                            player.Play();
                        }

                        break;
                    default: break;
                }


               
            }
        }

    }
}
