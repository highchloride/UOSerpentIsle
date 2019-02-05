//Maintenance Mode
//By Tresdni (aka DxMonkey)
//Last Update:  07/26/2015

namespace Server.Gumps
{
    public class MaintenanceModeGump : Gump
    {
        public MaintenanceModeGump()
            : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(160, 120, 537, 129, 9200);
            AddLabel(375, 129, 155, @"Maintenance Mode");
            AddItem(472, 131, 4138);
            AddItem(331, 131, 4138);
            AddHtml(179, 167, 496, 60, @"The shard is currently under maintenance mode.  Please try logging in again in a few moments.", (bool)true, (bool)true);
        }
    }
}