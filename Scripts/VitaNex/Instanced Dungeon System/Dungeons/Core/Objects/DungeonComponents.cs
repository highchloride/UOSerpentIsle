#region Header
//   Vorspire    _,-'/-'/  DungeonComponents.cs
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
using System.IO;

using Server;
using Server.Items;
#endregion

namespace VitaNex.Dungeons
{
	public abstract class DungeonComponents<T>
		where T : Dungeon
	{
		public void Generate(T dungeon)
		{
			OnGenerate(dungeon);
		}

		protected abstract void OnGenerate(T dungeon);

		protected virtual Item CreateObject(
			T dungeon,
			Type type,
			int itemID,
			Point3D loc,
			int amount,
			int hue,
			int light,
			string name)
		{
			if (dungeon == null || type == null || itemID < 0 || loc == Point3D.Zero)
			{
				return null;
			}

			Item obj;

			if (type.IsEqual<Static>() || type.IsEqual<StaticTile>())
			{
				obj = dungeon.CreateStatic(itemID, loc, false);
			}
			else
			{
				obj = dungeon.CreateItem(type, loc, false, new object[] { itemID }) ?? dungeon.CreateItem(type, loc, false);
			}

			if (obj == null)
			{
				return null;
			}

			if (name != null)
			{
				obj.Name = name;
			}

			if (hue > 0)
			{
				obj.Hue = hue;
			}

			if (amount > 1)
			{
				obj.Stackable = true;
				obj.Amount = amount;
			}

			if (light > -1)
			{
				obj.Light = (LightType)light;
			}

			return obj;
		}

		protected void LoadFile(T dungeon, string path)
		{
			using (var r = File.OpenText(path))
			{
				string line;
				string[] args;

				while (!r.EndOfStream)
				{
					line = r.ReadLine();

					if (String.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
					{
						continue;
					}

					args = line.Split('\t');

					if (args.Length < 3)
					{
						continue;
					}

					Type type;
					int itemID;
					Point3D loc;

					try
					{
						type = Type.GetType(args[0], false) ?? ScriptCompiler.FindTypeByFullName(args[0]);
					}
					catch
					{
						type = null;
					}

					try
					{
						itemID = Int32.Parse(args[1]);
					}
					catch
					{
						itemID = -1;
					}

					try
					{
						loc = Point3D.Parse(args[2]);
					}
					catch
					{
						loc = Point3D.Zero;
					}

					var amount = 1;
					var hue = 0;
					var light = -1;
					string name = null;

					if (args.Length > 3)
					{
						try
						{
							amount = Int32.Parse(args[3]);
						}
						catch
						{
							amount = 1;
						}
					}

					if (args.Length > 4)
					{
						try
						{
							hue = Int32.Parse(args[4]);
						}
						catch
						{
							hue = 0;
						}
					}

					if (args.Length > 5)
					{
						try
						{
							light = Int32.Parse(args[5]);
						}
						catch
						{
							light = 1;
						}
					}

					if (args.Length > 6)
					{
						name = args[6];
					}

					CreateObject(dungeon, type, itemID, loc, amount, hue, light, name);
				}
			}
		}
	}
}