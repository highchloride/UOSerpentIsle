// Play Music On Login v1.2.0
// Author: Felladrin
// Started: 2013-07-21
// Updated: 2016-01-18

using Server;
using Server.Network;

namespace Felladrin.Automations
{
    public static class PlayMusicOnLogin
    {
        public static class Config
        {
            public static bool Enabled = false;                          // Is this system enabled?
            public static bool PlayRandomMusic = false;                  // Should we play a random music from the list?
            public static MusicName SingleMusic = MusicName.OldUlt03;    // Music to be played if PlayRandomMusic = false.
        }

        public static void Initialize()
        {
            EventSink.Logout += OnLogout;

            if (Config.Enabled)
                EventSink.Login += OnLogin;
        }

        static void OnLogin(LoginEventArgs args)
        {
            MusicName toPlay = Config.SingleMusic;

            if (Config.PlayRandomMusic)
                toPlay = MusicList[Utility.Random(MusicList.Length)];

            args.Mobile.Send(PlayMusic.GetInstance(toPlay));
        }

        //SIOP - On logout, go back to the loginloop theme.
        static void OnLogout(LogoutEventArgs args)
        {
            args.Mobile.Send(PlayMusic.GetInstance(MusicName.LoginLoop));
        }
        
        public static MusicName[] MusicList = {
            MusicName.Stones2,
            MusicName.Magincia,
            MusicName.Minoc,
            MusicName.Ocllo,
            MusicName.Skarabra,
            MusicName.Trinsic,
            MusicName.Yew,
            MusicName.InTown01,
            MusicName.Moonglow,
            MusicName.MinocNegative,
            MusicName.ValoriaShips
        };
    }
}
