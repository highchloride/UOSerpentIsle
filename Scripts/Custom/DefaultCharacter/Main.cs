//CURRENTLY DISABLED
//In the original incarnation, this script disabled the client CC system by creating a default "Create New Character" character when Characters = 0.
//I've reinstated the client CC system and disabled this.

//using System;
//using System.Collections.Generic;
//using System.Text;
//using Server;
//using Server.Accounting;
//using Server.Mobiles;
//using Server.Misc;
//using Server.Network;
//using Server.Items;
//using Server.Gumps;

//namespace Bittiez.DefaultCharacter
//{
//    public class Main
//    {
//        [CallPriority(Int32.MaxValue)]
//        public static void Initialize()
//        {
//            EventSink.AccountLogin += new AccountLoginEventHandler(EventSink_AccountLogin);
//            Console.ForegroundColor = System.ConsoleColor.DarkMagenta;
//            Console.WriteLine("Default Character Loaded.");
//            Console.ForegroundColor = System.ConsoleColor.White;
//            EventSink.DeleteRequest += new DeleteRequestEventHandler(EventSink_DeleteRequest);
//            EventSink.Login += new LoginEventHandler(EventSink_Login);
//        }

//        private static Mobile CreateMobile(Account a)
//        {
//            if (a.Count >= a.Limit)
//                return null;

//            for (int i = 0; i < a.Length; ++i)
//            {
//                if (a[i] == null)
//                    return (a[i] = new PlayerMobile());
//            }

//            return null;
//        }

//        private static void EventSink_CharacterCreated(CharacterCreatedEventArgs args)
//        {
//            //Console.WriteLine("TEST");
//            args.Profession = 0;
//            NetState state = null;
//            if (args.State != null) state = args.State;

//            //if (state == null)                return;

//            Mobile newChar = CreateMobile(args.Account as Account);

//            if (newChar == null)
//            {
//                Utility.PushColor(ConsoleColor.Red);
//                Console.WriteLine("Login: {0}: Character creation failed, account full", state == null ? "" : state.Address.ToString());
//                Utility.PopColor();
//                return;
//            }

//            args.Mobile = newChar;
//            newChar.Player = true;
//            newChar.AccessLevel = args.Account.AccessLevel;
//            newChar.Female = args.Female;

//            if (Core.Expansion >= args.Race.RequiredExpansion)
//                newChar.Race = args.Race;	//Sets body
//            else
//                newChar.Race = Race.DefaultRace;

//            newChar.Hue = newChar.Race.ClipSkinHue(args.Hue & 0x3FFF) | 0x8000;

//            newChar.Hunger = 20;
//            newChar.Thirst = 20;

//            bool young = false;

//            if (newChar is PlayerMobile)
//            {
//                PlayerMobile pm = (PlayerMobile)newChar;

//                pm.Profession = args.Profession;

//                if (((Account)pm.Account).Young)
//                    young = pm.Young = true;
//            }

//            newChar.Name = args.Name;
//            newChar.Str = args.Str;
//            newChar.Dex = args.Dex;
//            newChar.Int = args.Int;
//            AddBackpack(newChar);

//            AddStartItems(newChar);

//            Race race = newChar.Race;

//            if (race.ValidateHair(newChar, args.HairID))
//            {
//                newChar.HairItemID = args.HairID;
//                newChar.HairHue = race.ClipHairHue(args.HairHue & 0x3FFF);
//            }

//            if (race.ValidateFacialHair(newChar, args.BeardID))
//            {
//                newChar.FacialHairItemID = args.BeardID;
//                newChar.FacialHairHue = race.ClipHairHue(args.BeardHue & 0x3FFF);
//            }

//            //Spawn in at full stats
//            newChar.Hits = newChar.HitsMax;
//            newChar.Mana = newChar.ManaMax;
//            newChar.Stam = newChar.StamMax;

//            //WHY DID I SPECIFY THIS BELOW IF WE'RE THROWING IT AWAY?
//            //CityInfo city = new CityInfo("", "", 1, 1, 10);
//            //newChar.MoveToWorld(city.Location, Map.Felucca);
//            //WHYYYYY

//            newChar.SetAllSkills(5.0);

//            //Move to new location
//            newChar.MoveToWorld(args.City.Location, args.City.Map);

//            //Welcome timer
//            new WelcomeTimer(newChar).Start();

//            //XmlAttach.AttachTo(newChar, new XmlPoints());
//            //XmlAttach.AttachTo(newChar, new XmlMobFactions());
//        }

//        //Adds the starting items
//        private static void AddStartItems(Mobile m)
//        {
//            //StatCodex statCodex = new StatCodex();
//            m.AddToBackpack(new StatCodex());

//            //SkillStudyBook skillCodex = new SkillStudyBook();
//            m.AddToBackpack(new SkillStudyBook());

//            //Gold gold = new Gold(5000);
//            m.AddToBackpack(new Gold(200));

//            m.AddToBackpack(new Waterskin());

//            m.AddToBackpack(new WoodenBowlOfStew());

//            m.AddToBackpack(new Dagger());

//            m.AddToBackpack(new ScrollCharCreate());
//        }


//        //Creates the backpack
//        private static void AddBackpack(Mobile m)
//        {
//            Container pack = m.Backpack;

//            if (pack == null)
//            {
//                pack = new Backpack();
//                pack.Movable = false;

//                m.AddItem(pack);
//            }

//        }

//        //SIOP ADDITION
//        //If your character's name is Create New Character, you'll get the CC gump.
//        public static void EventSink_Login(LoginEventArgs e)
//        {
//            if(e.Mobile.Name == "Create New Character")
//            {
//                e.Mobile.SendGump(new GumpCharCreate(e.Mobile));
//            }

//            if(e.Mobile.Region.Name == "the Serpent Pillar Expedition Launch")
//            {
//                e.Mobile.Send(PlayMusic.GetInstance(MusicName.OldUlt01));
//            }
//        }

//        //SIOP ADDITION
//        //If you have ZERO characters, a new character is automagically created!
//        public static void EventSink_DeleteRequest(DeleteRequestEventArgs e)
//        {
//            IAccount acc = Accounts.GetAccount(e.State.Account.Username);
//            if (acc.Count > 0)
//                return;

//            CharacterCreatedEventArgs arg = new CharacterCreatedEventArgs(
//                    null,
//                    acc,
//                    "Create New Character",
//                    false,
//                    33770,
//                    20,
//                    20,
//                    20,
//                    GetStartLoc(), //SIChange
//                    null,
//                    1,
//                    1,
//                    1,
//                    1,
//                    1,
//                    1,
//                    1,
//                    Race.Human
//                    );
//            EventSink_CharacterCreated(arg);
//            Console.ForegroundColor = System.ConsoleColor.DarkMagenta;
//            Console.WriteLine("[DC]New character created for account " + acc.Username);
//            Console.ForegroundColor = System.ConsoleColor.White;

            
//        }

//        public static CityInfo GetStartLoc()
//        {
//            int i = 0;

//            i = new Random().Next(0, 11);

//            switch (i)
//            {
//                case 0: return new CityInfo("", "", 236, 265, 0, Map.Tokuno);
//                case 1: return new CityInfo("", "", 235, 265, 0, Map.Tokuno);
//                case 2: return new CityInfo("", "", 234, 265, 0, Map.Tokuno);
//                case 3: return new CityInfo("", "", 233, 265, 0, Map.Tokuno);

//                case 4: return new CityInfo("", "", 232, 266, 0, Map.Tokuno);
//                case 5: return new CityInfo("", "", 232, 267, 0, Map.Tokuno);
//                case 6: return new CityInfo("", "", 232, 268, 0, Map.Tokuno);
//                case 7: return new CityInfo("", "", 232, 269, 0, Map.Tokuno);

//                case 8: return new CityInfo("", "", 233, 270, 0, Map.Tokuno);
//                case 9: return new CityInfo("", "", 234, 270, 0, Map.Tokuno);
//                case 10: return new CityInfo("", "", 235, 270, 0, Map.Tokuno);
//                case 11: return new CityInfo("", "", 236, 270, 0, Map.Tokuno);
//            }

//            return new CityInfo("", "", 236, 265, 0, Map.Tokuno); //SIOP SPEL Starting Loc
//        }

//        public static void EventSink_AccountLogin(AccountLoginEventArgs e)
//        {
//            if (e.Accepted)
//            {
//                IAccount acc = Accounts.GetAccount(e.Username);
//                if (acc == null || acc.Count > 0)
//                    return;

//                CharacterCreatedEventArgs arg = new CharacterCreatedEventArgs(
//                    null,
//                    acc,
//                    "Create New Character",
//                    false,
//                    33770,
//                    20,
//                    20,
//                    20,
//                    GetStartLoc(), //SIOP - Randomly sets your start location around the firepit
//                    null,
//                    1,
//                    1,
//                    1,
//                    1,
//                    1,
//                    1,
//                    1,
//                    Race.Human
//                    );
//                EventSink_CharacterCreated(arg);
//                Console.ForegroundColor = System.ConsoleColor.DarkMagenta;
//                Console.WriteLine("[DC]New character created for account " + acc.Username);
//                Console.ForegroundColor = System.ConsoleColor.White;
//            }
//        }
//    }
//}
