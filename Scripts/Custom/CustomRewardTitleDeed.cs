using System;
using Server.Mobiles;

namespace Server.Items
{
    public class CustomRewardTitleDeed : BaseRewardTitleDeed
    {
        private TextDefinition _Title;

        public override TextDefinition Title { get { return _Title; } }

        [Constructable]
        public CustomRewardTitleDeed(string title)
            : this(new TextDefinition(title))
        {
        }

        [Constructable]
        public CustomRewardTitleDeed(int title)
            : this(new TextDefinition(title))
        {
        }

        public CustomRewardTitleDeed(TextDefinition title)
        {
            _Title = title;
        }

        public CustomRewardTitleDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            TextDefinition.Serialize(writer, _Title);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int v = reader.ReadInt();

            _Title = TextDefinition.Deserialize(reader);
        }
    }
}