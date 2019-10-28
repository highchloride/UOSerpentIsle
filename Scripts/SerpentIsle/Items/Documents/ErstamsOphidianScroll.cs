using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{ 
    public class ErstamsOphidianScroll : Item
    {
        [Constructable]
        public ErstamsOphidianScroll() : base(0x1F35)
        {
            Name = "Erstam's Translation";
            Movable = true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in thy pack to use it.");
                return;
            }
                

            if (!from.HasGump(typeof(ErstamsOphidianScrollGump)))
            {
                from.SendGump(new ErstamsOphidianScrollGump(from));
            }
        }

        public ErstamsOphidianScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ErstamsOphidianScrollGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register("ErstamsOphidianScrollGump", AccessLevel.GameMaster, new CommandEventHandler(ErstamsOphidianScrollGump_OnCommand));
        }

        private static void ErstamsOphidianScrollGump_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new ErstamsOphidianScrollGump(e.Mobile));
        }

        public ErstamsOphidianScrollGump(Mobile owner) : base(50, 50)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;

            AddPage(0);
            AddBackground(6, 11, 390, 324, 9380);
            AddHtml(38, 55, 329, 237, Color("#080808", @"<p>Dearest Drogeni,<br />What follows is an excerpt from my translation of an ancient manuscript. The translation is crude since I do not as yet fully understand the Serpent Runes, but I think thou wilt find this very exciting. Until we meet again,<br />-- Erstam, thy devoted servant</p><p>I write this in great haste, for I can already hear the forces of Order breaching the keep walls. I know not how this missive will survive to reach the outside lands, or for that matter, future generations. Mine only hope is that this speedily-drafted work will offer record of our hallowed philosophy. For our culture to have any chance of enduring the ages, someone, somewhere must find this. Please, reader, I beseech thee, spread the word of our peoples.</p><p>Balance -- The harmony between the Principles of Order and Chaos -- is the one pure axiom we hold true. All three Principles are symbolized in our hieroglyphs: The Great Earth Serpent, keeper of Balance, lies on a vertical plane, around which the two opposing serpents of Chaos and Order wrap themselves. Chaos and Order each embrace three Forces. These six Forces, when combined, form the three Principles of Balance. The Forces of Chaos are Tolerance, Enthusiasm, and Emotion; the Forces of Order are Ethicality, Discipline, and Logic.</p><p>CHAOS -- Tolerance is that which encourages the acceptance of all things. Enthusiasm is the energy that allows one to perform great tasks. Emotion is the ability to perceive those feelings that come from the heart, as opposed to coming from the mind.<br />ORDER -- Ethicality is the belief that there is great value in abiding by rules of conduct. Discipline is the drive to complete a task and avoid the distractions that will prevent its completion. Logic permits clear, reasoned thought, free from any instinctual biases.<br />BALANCE -- From the marriage between two Forces, one each from Chaos and Order, come the Principles. Tolerance and Ethicality combine to form Harmony, the ability to be at peace with the self, the individual, and the world. From the union of Enthusiasm and Discipline springs Dedication, that which permits one to surmount obstacles and lead others. Finally, Emotion tempered by Logic results in Rationality, the ability to comprehend life and understand the world around us.</p><p>The Forces of Chaos and Order must ever remain in Balance, for imbalance leads to disaster. Witness the war-torn state of our world today! As thou canst surely see, my world hath been torn asunder by disregard for Balance -- our dearest axiom! If thou dost thrive in a time less violent, I can do no more than plead with thee to help restore Balance to the Serpent Isle! I must end this brief explication here, for I can hear mine attackers pounding upon the oaken door downstairs. I wish thee and thy world better fortune than mine own.<br />-- Ssithnos, the Great Hierophant</p>"), false, true);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        //Cancel                        
                        break;
                    }
            }
        }

        private string Color(string color, string str)
        {
            return String.Format("<basefont color={0}>{1}", color, str);
        }
    }
}
