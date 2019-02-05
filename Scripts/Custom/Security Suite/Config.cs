using System;
using System.Collections.Generic;
using Server.Items;

namespace Tresdni.Security
{
    class SecurityConfig
    {
        public const bool Enabled = false;
        public const bool BroadcastToAdmins = true;

        //Players to ignore in the security system (example:  shard owner's name)
        public static readonly List<string> PlayersExempt = new List<string>()
        {
            "higchloride"
        };

        //Which types to check for irregular amounts?  Type/Amount Threshold
        public static readonly List<SecureType> TypesToSecure = new List<SecureType>()
        {
            //new SecureType(typeof(BloodOathCoins), 1000)
        };       

        //Which props to check for irregular settings?  Prop Name/Amount Threshold
        public static readonly List<SecureProp> PropsToSecure = new List<SecureProp>()
        {
            new SecureProp("Luck", 200),
            new SecureProp("AttackChance", 50),
            new SecureProp("FasterCasting", 2),
            new SecureProp("FasterCastRecovery", 6)
        };
 
        //Which phrases to check for in player's speech?  (lowercase)
        public static readonly List<string> SpeechToSecure = new List<string>()
        {
            "tresdni",
            ".com",
            "servegame",
            "logon.",
            "play."
        }; 
    }

    class SecureType
    {
        public readonly Type ItemType;
        public readonly int Threshold;

        public SecureType(Type type, int amount)
        {
            ItemType = type;
            Threshold = amount;
        }
    }

    class SecureProp
    {
        public readonly string Prop;
        public readonly int Threshold;

        public SecureProp(string prop, int amount)
        {
            Prop = prop;
            Threshold = amount;
        }
    }
}
