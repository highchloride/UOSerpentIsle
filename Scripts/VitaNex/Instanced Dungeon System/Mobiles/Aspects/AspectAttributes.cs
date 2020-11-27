#region Header
//   Vorspire    _,-'/-'/  AspectAttributes.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using VitaNex;
#endregion

namespace Server.Mobiles
{
	public sealed class AspectAttributes : SettingsObject<AspectFlags>
	{
		public bool IsEmpty { get { return Flags == AspectFlags.None; } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Time { get { return GetFlag(AspectFlags.Time); } set { SetFlag(AspectFlags.Time, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Light { get { return GetFlag(AspectFlags.Light); } set { SetFlag(AspectFlags.Light, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Darkness { get { return GetFlag(AspectFlags.Darkness); } set { SetFlag(AspectFlags.Darkness, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Faith { get { return GetFlag(AspectFlags.Faith); } set { SetFlag(AspectFlags.Faith, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Despair { get { return GetFlag(AspectFlags.Despair); } set { SetFlag(AspectFlags.Despair, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Illusion { get { return GetFlag(AspectFlags.Illusion); } set { SetFlag(AspectFlags.Illusion, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Life { get { return GetFlag(AspectFlags.Life); } set { SetFlag(AspectFlags.Life, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Death { get { return GetFlag(AspectFlags.Death); } set { SetFlag(AspectFlags.Death, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Elements { get { return GetFlag(AspectFlags.Elements); } set { SetFlag(AspectFlags.Elements, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Greed { get { return GetFlag(AspectFlags.Greed); } set { SetFlag(AspectFlags.Greed, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Famine { get { return GetFlag(AspectFlags.Famine); } set { SetFlag(AspectFlags.Famine, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Tech { get { return GetFlag(AspectFlags.Tech); } set { SetFlag(AspectFlags.Tech, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Decay { get { return GetFlag(AspectFlags.Decay); } set { SetFlag(AspectFlags.Decay, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Earth { get { return GetFlag(AspectFlags.Earth); } set { SetFlag(AspectFlags.Earth, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Fire { get { return GetFlag(AspectFlags.Fire); } set { SetFlag(AspectFlags.Fire, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Frost { get { return GetFlag(AspectFlags.Frost); } set { SetFlag(AspectFlags.Frost, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Poison { get { return GetFlag(AspectFlags.Poison); } set { SetFlag(AspectFlags.Poison, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Energy { get { return GetFlag(AspectFlags.Energy); } set { SetFlag(AspectFlags.Energy, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Chaos { get { return GetFlag(AspectFlags.Chaos); } set { SetFlag(AspectFlags.Chaos, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool All { get { return GetFlag(AspectFlags.All); } set { SetFlag(AspectFlags.All, value); } }

		public AspectAttributes(BaseAspect owner)
			: this(owner.DefaultAspects)
		{
			OnChanged = owner.AspectChanged;
		}

		public AspectAttributes()
			: this(AspectFlags.None)
		{ }

		public AspectAttributes(AspectFlags flags)
			: base(flags)
		{ }

		public AspectAttributes(GenericReader reader)
			: base(reader)
		{ }

		public override string ToString()
		{
			return "...";
		}

		public override void Clear()
		{
			Flags = AspectFlags.None;
		}

		public override void Reset()
		{
			Flags = AspectFlags.None;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();
		}
	}
}