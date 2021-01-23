using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsFirewallHelper;
using System.Windows.Media;

namespace BoonwinsBattlegroundTracker
{
    class Disconnector
    {


        public const string ruleName = @"Boontracker_Hearthstone";
        public const string hsPath = @"E:\Games\Hearthstone\Hearthstone.exe";


        public static void CheckAndCreateRule()
        {
           

            try
            {
                var rule = FirewallManager.Instance.Rules.Where(o =>
                    o.Direction == FirewallDirection.Outbound &&
                    o.Name.Equals(ruleName)
                ).FirstOrDefault();

                if (rule == null) rule = CreateRule();

            }
            catch (Exception exception)
            {
               
                MessageBox.Show(exception.Message, "Cant Use Firewall-Tool start Tracker as Admin");
               
            }    

        }

        public static int RuleSwitcher(Config _config)
        {

            var rule = FirewallManager.Instance.Rules.Where(o =>
                    o.Direction == FirewallDirection.Outbound &&
                    o.Name.Equals(ruleName)
                ).FirstOrDefault();
            if (!_config.DisconectedThisGame) _config.DisconectedThisGame = true;
            if (!rule.IsEnable)
            {
                //Wait Timer and auto reconect
                Disconnect();
                
                
                return 1;

            } else {
                Connect();
                return 0;
           }

            
            
        }

        private static IRule CreateRule()
        {

            IRule rule = FirewallManager.Instance.CreateApplicationRule(
            FirewallManager.Instance.GetProfile().Type,
            ruleName,
            FirewallAction.Block,
            hsPath
            );
            rule.Direction = FirewallDirection.Outbound;
            FirewallManager.Instance.Rules.Add(rule);
            return rule;
        }
          
        
        private static void RunCMD(string argument)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = @"C:\Windows\System32\cmd.exe";       
            startInfo.Arguments = argument;
            process.StartInfo = startInfo;
            process.Start();
        }

        public static void Disconnect()
        {

            RunCMD(@"/C netsh advfirewall firewall set rule name=""Boontracker_Hearthstone"" new enable=yes");

        }


        public static void Connect()
        {
            RunCMD(@"/C netsh advfirewall firewall set rule name=""Boontracker_Hearthstone"" new enable=no");
        }
    }
}
