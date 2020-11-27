#region Header
//   Vorspire    _,-'/-'/  DungeonPortal.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Drawing;
using System.Linq;

using Server;
using Server.Gumps;
using Server.Mobiles;

using VitaNex.InstanceMaps;
using VitaNex.Network;
#endregion

namespace VitaNex.Dungeons
{
	public class DungeonPortal : Item
	{
		private DungeonInfo _Info;

		[CommandProperty(AccessLevel.GameMaster)]
		public DungeonInfo Info
		{
			get
			{
				if (_Info == null || _Info.ID != _ID)
				{
					_Info = Instances.DungeonInfo.FirstOrDefault(i => i.ID == _ID);
				}

				return _Info;
			}
			set
			{
				if (_Info == value)
				{
					return;
				}

				_Info = value;

				if (_Info != null && _Info.ID != _ID)
				{
					_ID = _Info.ID;
				}

				InvalidateProperties();
			}
		}

		private DungeonID _ID;

		[CommandProperty(AccessLevel.GameMaster)]
		public DungeonID ID
		{
			get
			{
				if (_Info != null && _Info.ID != _ID)
				{
					_ID = _Info.ID;
				}

				return _ID;
			}
			set
			{
				if (_ID == value)
				{
					return;
				}

				_ID = value;

				if (_Info == null || _Info.ID != _ID)
				{
					_Info = Instances.DungeonInfo.FirstOrDefault(i => i.ID == _ID);
				}

				InvalidateProperties();
			}
		}

		private bool _Active;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Active
		{
			get { return _Active; }
			set
			{
				_Active = value;
				InvalidateProperties();
			}
		}

		public override string DefaultName { get { return "Dungeon Portal"; } }

		public override bool ForceShowProperties { get { return Active; } }

		[Constructable]
		public DungeonPortal()
			: this(DungeonID.None)
		{ }

		[Constructable]
		public DungeonPortal(DungeonID dungeon)
			: base(0x4B8F)
		{
			ID = dungeon;
			Active = true;

			Movable = false;
		}

		public DungeonPortal(Serial serial)
			: base(serial)
		{ }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			var eopl = new ExtendedOPL(list);

			if (Info != null)
			{
				eopl.Add(Info.Name.WrapUOHtmlColor(Color.Gold));
				eopl.Add("Group Size: {0}", Info.GroupMax);

				eopl.Add("Deadline: {0}", Info.Duration.ToSimpleString(@"<d\d ><h\h ><m\m ><s\s >"));
				eopl.Add("Lockout: {0}", Info.Lockout.ToSimpleString(@"<d\d ><h\h ><m\m ><s\s >"));
			}

			eopl.Add("[{0}]", Active ? "Active".WrapUOHtmlColor(Color.LawnGreen) : "Inactive".WrapUOHtmlColor(Color.OrangeRed));
			eopl.Apply();
		}

		public override void OnDoubleClick(Mobile m)
		{
			if (!this.CheckDoubleClick(m) || !(m is PlayerMobile))
			{
				return;
			}

			if (m.AccessLevel >= AccessLevel.GameMaster)
			{
				m.SendGump(
					new PropertiesGump(m, this)
					{
						X = 100,
						Y = 100
					});
			}

			if (!Active)
			{
				return;
			}

			if (ID != DungeonID.None && Info != null)
			{
				Instances.EnterDungeon((PlayerMobile)m, Info, true);
			}
			else
			{
				Instances.ExitDungeon((PlayerMobile)m, true);
			}
		}

		public override bool OnMoveOver(Mobile m)
		{
			if (!(m is PlayerMobile) || !Active)
			{
				return base.OnMoveOver(m);
			}

			if (ID != DungeonID.None && Info != null)
			{
				Instances.EnterDungeon((PlayerMobile)m, Info, true);
			}
			else
			{
				Instances.ExitDungeon((PlayerMobile)m, true);
			}

			return base.OnMoveOver(m);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.WriteFlag(ID);
			writer.Write(Active);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			ID = reader.ReadFlag<DungeonID>();
			Active = reader.ReadBool();
		}
	}
}