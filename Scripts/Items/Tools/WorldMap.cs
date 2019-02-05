using System;
using Server;

namespace Server.Items
{
	public class WorldMap : MapItem
	{
		[Constructable]
		public WorldMap()
		{
			SetDisplay( 0, 0, 5119, 4095, 400, 400 );
		}

		public override void CraftInit( Mobile from )
		{
			// Unlike the others, world map is not based on crafted location
            Map map = from.Map;

			double skillValue = from.Skills[SkillName.Cartography].Value;
            int x20 = 0; int size = 0;

            if (map == Map.Trammel || map == Map.Felucca)
            {
                x20 = (int)(skillValue * 20); size = 25 + (int)(skillValue * 6.6);
            }
            if (map == Map.Ilshenar)
            {
                x20 = (int)(skillValue * 8); size = 25 + (int)(skillValue * 6.6);
            }
            if (map == Map.Malas)
            {
                x20 = (int)(skillValue * 10); size = 25 + (int)(skillValue * 6.6);
            }
            if (map == Map.Tokuno)
            {
                x20 = (int)(skillValue * 7); size = 25 + (int)(skillValue * 6.6);
            }
            if (map == Map.TerMur)
            {
                x20 = (int)(skillValue * 6); size = 25 + (int)(skillValue * 6.6);
            }
            if (map == Map.SerpentIsle)
            {
                x20 = (int)(skillValue * 8); size = 25 + (int)(skillValue * 6.6);
            }

			if ( size < 200 )
				size = 200;
			else if ( size > 400 )
				size = 400;
           
            if ( map == Map.Trammel || map == Map.Felucca )
                SetDisplay(1900 - x20, 1600 - x20, 3000 + x20, 2080 + x20, size, size, from.Map);
            if (map == Map.Ilshenar)
                SetDisplay(940 - x20, 900 - x20, 1150 + x20, 700 + x20, size, size, from.Map);
            if (map == Map.Malas)
                SetDisplay(1515 - x20, 1000 - x20, 1550 + x20, 1000 + x20, size, size, from.Map);
            if (map == Map.Tokuno)
                SetDisplay(700 - x20, 700 - x20, 740 + x20, 710 + x20, size, size, from.Map);
            if (map == Map.TerMur)
                SetDisplay(840 - x20, 3410 - x20, 660 + x20, 3510 + x20, size, size, from.Map);
            if (map == Map.SerpentIsle)
                SetDisplay(940 - x20, 900 - x20, 1150 + x20, 700 + x20, size, size, from.Map);
        }

		public override int LabelNumber{ get{ return 1015233; } } // world map

		public WorldMap( Serial serial ) : base( serial )
		{
		}

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            Map map = this.Facet;
            string mDesc = "";

            if (map == Map.Trammel)
                mDesc = "for Trammel";
            if (map == Map.Felucca)
                mDesc = "for Felucca";
            if (map == Map.Ilshenar)
                mDesc = "for Serpent Isle";
            if (map == Map.Malas)
                mDesc = "for Malas";
            if (map == Map.Tokuno)
                mDesc = "for Tokuno Islands";
            if (map == Map.TerMur)
                mDesc = "for Ter Mur";
            if (map == Map.SerpentIsle)
                mDesc = "for Serpent Isle";

            list.Add(1053099, String.Format("<BASEFONT COLOR=#DDCC22>\t{0}<BASEFONT COLOR=#FFFFFF>", mDesc));
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
