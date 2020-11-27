using Server.Commands;
using Server.Gumps;
using System;

namespace Server.Customs.Gumps
{
    public class AutoRefreshGump : Gump
    {
        public AutoRefreshGump(Mobile from, int x, int y, object[] args = null) : this(TimeSpan.FromSeconds(1), from, x, y, args)
        {
        }

        public AutoRefreshGump(TimeSpan delay, Mobile from, int x, int y, object[] args = null) : base(x, y)
        {
            Timer.DelayCall(delay, () =>
            {
                from.CloseGump(GetType());
                from.SendGump(Activator.CreateInstance(GetType(), delay, from, x, y, args) as AutoRefreshGump); ;
            });
        }
    }

    public class AutoClockGump : AutoRefreshGump
    {
        public static void Initialize()
        {
            CommandSystem.Register("ServerTimeGump", AccessLevel.GameMaster, (handler) => handler.Mobile.SendGump(new AutoClockGump(handler.Mobile)));
        }

        public AutoClockGump(TimeSpan delay, Mobile from, int x, int y, object[] args) : this(from)
        {
        }

        public AutoClockGump(Mobile from) : base(from, 100, 100)
        {
            Closable = false;
            Disposable = false;
            Dragable = false;

            AddPage(0);

            AddBackground(0, 0, 320, 32, 5054);
            AddImageTiled(10, 10, 300, 22, 2624);
            AddAlphaRegion(10, 10, 300, 32);

            AddLabel(10, 10, 34, $"Current Servertime: {DateTime.Now}");
        }
    }
}
