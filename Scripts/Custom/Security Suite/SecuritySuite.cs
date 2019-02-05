using System;
using System.IO;
using System.Linq;
using Server;
using Server.Commands;
using Server.Network;

namespace Tresdni.Security
{
    class SecuritySuite
    {
        public static void Initialize()
        {
            if (!SecurityConfig.Enabled)
            {
                return;
            }

            CommandSystem.Register("SecuritySweep", AccessLevel.Administrator, SecuritySweep_OnCommand);
            EventSink.OnPropertyChanged += EventSink_OnPropertyChanged;
            EventSink.Speech += EventSink_OnPlayerSpeech;
        }

        [Usage("SecuritySweep")]
        [Description("Performs a full security sweep on your shard.")]
        private static void SecuritySweep_OnCommand(CommandEventArgs e)
        {
            foreach (Item item in from type in SecurityConfig.TypesToSecure from item in World.Items.Values where item != null && !item.Deleted && item.GetType() == type.ItemType && item.Amount >= type.Threshold select item)
            {
                LogSecurityIssue(String.Format("[Out of Ordinary Item Amount]: {0} {1} found at {2} - {3}.", item.Amount, item.Name, item.RootParentEntity == null ? item.Location : item.RootParentEntity.Location, item.Map));
            }
        }

        public static void SecuritySweep()
        {
            foreach (Item item in from type in SecurityConfig.TypesToSecure from item in World.Items.Values where item != null && !item.Deleted && item.GetType() == type.ItemType && item.Amount >= type.Threshold select item)
            {
                LogSecurityIssue(String.Format("[Out of Ordinary Item Amount]: {0} {1} found at {2} - {3}.", item.Amount, item.Name, item.RootParentEntity == null ? item.Location : item.RootParentEntity.Location, item.Map));
            }
        }

        private static void EventSink_OnPropertyChanged(OnPropertyChangedEventArgs args)
        {
            if (args.Mobile == null || args.NewValue == null || args.Property == null)
                return;

            if (SecurityConfig.PlayersExempt.Contains(args.Mobile.Name))
                return;
            
            bool found = false;

            foreach (SecureProp prop in SecurityConfig.PropsToSecure.Where(prop => prop.Prop == args.Property.Name && Convert.ToInt64(args.NewValue) > prop.Threshold).Where(prop => !SecurityConfig.PlayersExempt.Contains(args.Mobile.Name)))
            {
                found = true;
            }

            if (!found)
                return;

            LogSecurityIssue(String.Format("[Out of Ordinary Property Change]: {0} changed property {1} to {2} at {3} - {4}.", args.Mobile.Name, args.Property.Name, args.NewValue, args.Mobile.Location, args.Mobile.Map));
        }

        private static void EventSink_OnPlayerSpeech(SpeechEventArgs args)
        {
            if (args.Mobile == null || args.Speech == null)
                return;

            if (SecurityConfig.PlayersExempt.Contains(args.Mobile.Name))
                return;

            bool found = false;

            foreach (string speech in SecurityConfig.SpeechToSecure.Where(speech => args.Speech.ToLower().Contains(speech)))
            {
                found = true;
            }

            if(found)
                LogSecurityIssue(String.Format("[Possible Advertising or Speech Violation]: {0} said \"{1}\"", args.Mobile, args.Speech));
        }

        private static void LogSecurityIssue(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("SecuritySuite.log", true))
                {
                    sw.WriteLine("{0}", DateTime.UtcNow);
                    sw.WriteLine("--------------------------------");
                    sw.WriteLine(message);
                    sw.WriteLine("");
                    sw.WriteLine("");
                }

                Console.WriteLine(message);

                if (SecurityConfig.BroadcastToAdmins)
                {
                    foreach (
                        Mobile mob in
                            NetState.Instances.Select(state => state.Mobile)
                                .Where(mob => mob != null)
                                .Where(mob => mob.AccessLevel >= AccessLevel.Administrator))
                    {
                        mob.SendMessage(message);
                    }
                }
            }
            catch { }
        }
    }

    public class SecuritySuiteTimer : Timer
    {
        public static void Initialize()
        {
            if (!SecurityConfig.Enabled)
            {
                return;
            }

            new SecuritySuiteTimer();
            Console.WriteLine("Tresdni's Security Suite Initialized!");
        }

        public SecuritySuiteTimer()
            : base(TimeSpan.FromSeconds(5), TimeSpan.FromHours(1))
        {
            Start();
        }

        protected override void OnTick()
        {
            SecuritySuite.SecuritySweep();
        }
    }
}
