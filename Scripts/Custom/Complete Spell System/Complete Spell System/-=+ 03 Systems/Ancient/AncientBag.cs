using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class BagOfAncientScrolls : ScrollBag
    {
        [Constructable]
        public BagOfAncientScrolls()
        {
            Hue = 1355;
            PlaceItemIn(30, 35, new AncientFireworksScroll());
            PlaceItemIn(50, 35, new AncientGlimmerScroll());
            PlaceItemIn(70, 35, new AncientAwakenScroll());
            PlaceItemIn(90, 35, new AncientThunderScroll());
            PlaceItemIn(30, 55, new AncientWeatherScroll());
            PlaceItemIn(50, 55, new AncientIgniteScroll());
            PlaceItemIn(70, 55, new AncientDouseScroll());
            PlaceItemIn(90, 55, new AncientLocateScroll());
            PlaceItemIn(30, 75, new AncientAwakenAllScroll());
            PlaceItemIn(50, 75, new AncientDetectTrapScroll());
            PlaceItemIn(70, 75, new AncientGreatDouseScroll());
            PlaceItemIn(90, 75, new AncientGreatIgniteScroll());
            PlaceItemIn(30, 90, new AncientEnchantScroll());
            PlaceItemIn(50, 90, new AncientFalseCoinScroll());
            PlaceItemIn(70, 90, new AncientGreatLightScroll());
            PlaceItemIn(90, 90, new AncientDestroyTrapScroll());
            PlaceItemIn(30, 45, new AncientSleepScroll());
            PlaceItemIn(50, 45, new AncientSwarmScroll());
            PlaceItemIn(70, 45, new AncientPeerScroll());
            PlaceItemIn(90, 45, new AncientSeanceScroll());
            PlaceItemIn(30, 65, new AncientCharmScroll());
            PlaceItemIn(50, 65, new AncientDanceScroll());
            PlaceItemIn(70, 65, new AncientMassSleepScroll());
            PlaceItemIn(90, 65, new AncientCloneScroll());
            PlaceItemIn(30, 85, new AncientCauseFearScroll());
            PlaceItemIn(50, 85, new AncientFireRingScroll());
            PlaceItemIn(70, 85, new AncientTremorScroll());
            PlaceItemIn(90, 85, new AncientSleepFieldScroll());
            PlaceItemIn(40, 35, new AncientMassMightScroll());
            PlaceItemIn(60, 35, new AncientMassCharmScroll());
            PlaceItemIn(80, 35, new AncientInvisibilityAllScroll());
            PlaceItemIn(40, 55, new AncientDeathVortexScroll());
            PlaceItemIn(60, 55, new AncientMassDeathScroll());
            PlaceItemIn(80, 55, new AncientArmageddonScroll());
        }

        public BagOfAncientScrolls(Serial serial)
            : base(serial)
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
}
