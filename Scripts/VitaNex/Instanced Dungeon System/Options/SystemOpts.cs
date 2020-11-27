#region Header
//   Vorspire    _,-'/-'/  SystemOpts.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2015  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using Server;
#endregion

namespace VitaNex.InstanceMaps
{
	[PropertyObject]
	public class InstanceMapsOptions : CoreServiceOptions
	{
		private Map _BounceMap;

		[CommandProperty(Instances.Access)]
		public Map BounceMap
		{
			get { return _BounceMap; }
			set
			{
				while (value is InstanceMap)
				{
					value = ((InstanceMap)value).Parent;
				}

				_BounceMap = value;
			}
		}

		[CommandProperty(Instances.Access)]
		public Point3D BounceLocation { get; set; }

		public InstanceMapsOptions()
			: base(typeof(Instances))
		{
			// Britain Peninsula (West of the Bank)
			BounceMap = Map.Felucca;
			BounceLocation = new Point3D(1383, 1713, 20);
		}

		public InstanceMapsOptions(GenericReader reader)
			: base(reader)
		{ }

		public override void Clear()
		{
			BounceMap = null;
			BounceLocation = Point3D.Zero;
		}

		public override void Reset()
		{
			// Britain Peninsula (West of the Bank)
			BounceMap = Map.Felucca;
			BounceLocation = new Point3D(1383, 1713, 20);
		}

		public override string ToString()
		{
			return "Instance Maps Options";
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			var version = writer.SetVersion(0);

			switch (version)
			{
				case 1:
				{
					writer.Write(BounceMap);
					writer.Write(BounceLocation);
				}
					goto case 0;
				case 0:
					break;
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.GetVersion();

			switch (version)
			{
				case 1:
				{
					BounceMap = reader.ReadMap();
					BounceLocation = reader.ReadPoint3D();
				}
					goto case 0;
				case 0:
					break;
			}
		}
	}
}