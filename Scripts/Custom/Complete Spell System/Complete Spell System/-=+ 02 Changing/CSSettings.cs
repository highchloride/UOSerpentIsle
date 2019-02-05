using System;
using Server;

namespace Server.ACC.CSS
{
    public static class CSSettings
    {
        /*
         * Initial setting.  Will read from saves after first save.
         * Set to true if you want custom Spellbooks to be created full, unless you specify in the creation.
         */
        public static bool FullSpellbooks { get { return false; } }
    }
}