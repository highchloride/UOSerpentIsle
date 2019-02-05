//Inherits the SkillCodex from Vitanex, set to provide 35 points to 7 skills. It will disappear once used.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Mobiles;
using VitaNex.Items;
using VitaNex.SuperGumps;

namespace Server.Items
{
    public sealed class SkillStudyBook : SkillCodex
    {
        [Constructable]
        public SkillStudyBook() : base(7, 50.0, true, SkillCodexMode.Fixed)
        {
            base.Name = "Skill Study Book";
        }

        public SkillStudyBook(Serial serial) : base(serial)
        {
        }        

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
