#region Header
//   Vorspire    _,-'/-'/  DungeonLootUI.cs
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Server;
using Server.Items;
using Server.Mobiles;

using Ultima;

using VitaNex.SuperGumps;
using VitaNex.SuperGumps.UI;
#endregion

namespace VitaNex.Dungeons
{
	public class DungeonLootUI : SuperGumpList<DungeonLootEntry>
	{
		public static void DisplayTo(Mobile user, bool refreshOnly)
		{
			DisplayTo(user, refreshOnly, InstanceMaps.Instances.GetDungeon(user));
		}

		public static void DisplayTo(Mobile user, bool refreshOnly, Dungeon dungeon)
		{
			var info = EnumerateInstances<DungeonLootUI>(user).FirstOrDefault(g => g != null && !g.IsDisposed && g.IsOpen);

			if (info == null)
			{
				if (refreshOnly)
				{
					return;
				}

				info = new DungeonLootUI(user, dungeon);
			}
            else if (info.Dungeon == dungeon && info.EntryCount >= info.EntriesPerPage)
            {
                return;
            }

            if (dungeon != null && !dungeon.Deleted)
			{
				info.Dungeon = dungeon;
			}

			info.Refresh(true);
		}

		public Dungeon Dungeon { get; set; }

		public DungeonLootUI(Mobile user, Dungeon dungeon)
			: base(user, null, 100, 50)
		{
			Dungeon = dungeon;

			CanClose = false;
			CanDispose = false;
			CanMove = false;
			CanResize = false;

			EntriesPerPage = 9;

			ForceRecompile = true;

			AutoRefreshRate = TimeSpan.FromSeconds(30.0);
			AutoRefresh = true;
		}

		protected override void CompileList(List<DungeonLootEntry> list)
		{
			if (User is PlayerMobile)
			{
				var user = (PlayerMobile)User;

				list.Clear();

                var loot = Dungeon.Loot.Where(e => e.Valid && !e.HasWinner)
                                  .Where(e => e.Rolls.GetValue(user) == null)
                                  .Take(EntriesPerPage);

                list.AddRange(loot);
			}

			base.CompileList(list);
		}

		protected override void CompileLayout(SuperGumpLayout layout)
		{
			base.CompileLayout(layout);

			/*
			 *    1095px
			 * [_1_][_4_][_7_]
			 * [_2_][_5_][_8_] 525px
			 * [_3_][_6_][_9_]
			 *
			 * ((355 + 10) * (165 + 10)) * (3 * 3)
			 */

			var range = GetListRange();

			if (range.Count == 0)
			{
				return;
			}

			var d = (int)Math.Ceiling(Math.Sqrt(range.Count));

			var index = 0;

			for (var x = 0; x < d; x++)
			{
				for (var y = 0; y < d; y++)
				{
					var e = range.GetValueAt(index);

					if (e != null && e.Item != null)
					{
						CompileEntryLayout(layout, x, y, index, e);
					}

					if (!range.InBounds(++index))
					{
						return;
					}
				}
			}
		}

		protected virtual void CompileEntryLayout(SuperGumpLayout layout, int x, int y, int index, DungeonLootEntry entry)
		{
			var title = String.Empty;

			if (entry.Item == null)
			{
				return;
			}

#if NEWPARENT
			var parent = entry.Item.RootParent;
#else
			var parent = entry.Item.RootParentEntity;
#endif

			if (parent != null && !parent.Deleted)
			{
				if (parent is Corpse)
				{
					var c = (Corpse)parent;

					if (c.Owner != null)
					{
						title = c.Owner.RawName;
					}
				}
				else if (parent is Item)
				{
					title = ((Item)parent).Name;
				}
			}

			x *= 365;
			y *= 175;

			layout.Add(
				"entry/bg/" + index,
				() =>
				{
					AddItem(x + 85, y + 0, 14020); //Fire1
					AddItem(x + 105, y + 0, 14020); //Fire2
					AddItem(x + 141, y + 15, 14007); //Fire3

					AddImage(x + 6, y + 4, 10400, 1258); //DragonHighlight

					AddBackground(x + 50, y + 60, 300, 100, 5120); //MainBG

					AddImage(x + 3, y + 5, 10400); //Dragon
					AddImageTiled(x + 50, y + 105, 10, 50, 5123); // Dragon3DFix

					AddBackground(x + 65, y + 70, 55, 55, 5120); //ItemBG
					AddLabelCropped(x + 65, y + 70, 55, 55, 0, " ");
					AddProperties(entry.Item);

					AddBackground(x + 125, y + 70, 150, 55, 5120); //ItemNameBG
					AddLabelCropped(x + 125, y + 70, 150, 55, 0, " ");
					AddProperties(entry.Item);

					AddBackground(x + 65, y + 130, 275, 20, 5120); //TimeBarBG

					if (!String.IsNullOrWhiteSpace(title))
					{
						AddBackground(x + 95, y + 25, 255, 30, 5120); //TitleBG
					}

					AddItem(x + 85, y + 30, 4655); //Blood1
					AddItem(x + 124, y + 20, 4655); //Blood2

					var id = entry.Item.Parent is Corpse ? 9804 : 9803;

					AddImage(x + 98, y + 13, id, 1258); //SkullHighlight
					AddImage(x + 100, y + 15, id); //Skull

					AddItem(x + 65, y + 0, 14017); //Fire4
				});

			layout.Add(
				"entry/content/" + index,
				() =>
				{
					if (!String.IsNullOrWhiteSpace(title))
					{
						AddHtml(
							x + 170,
							y + 30,
							175,
							40,
							title.WrapUOHtmlTag("CENTER").WrapUOHtmlColor(Color.OrangeRed, false),
							false,
							false);
						// Title
					}

					var o = ArtExtUtility.GetImageOffset(entry.Item.ItemID);

					//Item (Loot)
					if (entry.Item.Hue > 0 && entry.Item.Hue <= 3000)
					{
						AddItem(x + 70 + o.X, y + 75 + o.Y, entry.Item.ItemID, entry.Item.Hue - 1);
					}
					else
					{
						AddItem(x + 70 + o.X, y + 75 + o.Y, entry.Item.ItemID);
					}

					AddProperties(entry.Item);

					var name = entry.Item.ResolveName(User);
					name = name.ToUpperWords().WrapUOHtmlColor(Color.Yellow, false);

					//ItemName
					AddHtml(x + 135, y + 75, 135, 45, name, false, false);

					AddButton(x + 287, y + 73, 22153, 22154, b => OnHelp(entry)); //Help
					AddTooltip(1061037); //Help

					AddButton(x + 317, y + 73, 22150, 22151, b => OnPass(entry)); //Pass
					AddTooltip(1014123); //Please, keep it. I don't want it.

					//AddButton(x + 285, y + 101, 11281, 11280, b => OnNeed(entry)); //Need
					AddItem(x + 277, y + 98, Utility.RandomMinMax(19384, 19397)); //Dice
					AddTooltip(1014113); //I've been needing one of these.

					//AddButton(x + 315, y + 101, 11280, 11280, b => OnGreed(entry)); //Greed
					AddItem(x + 307, y + 98, 19400); //Chips
					AddTooltip(1062441); //I've given enough to charity, lately!

					var elapsed = DateTime.UtcNow - entry.Created;
					var duration = entry.Expire - entry.Created;

					string time;

					if (elapsed <= duration && duration > TimeSpan.Zero)
					{
						var length = 270 * (1.0 - (elapsed.TotalSeconds / duration.TotalSeconds));

						AddBackground(x + 66, y + 132, (int)Math.Ceiling(length), 16, 5100); //TimeBar

						time = (duration - elapsed).ToSimpleString("h:m:s");
					}
					else
					{
						time = "EXPIRED";
					}

					if (String.IsNullOrWhiteSpace(time))
					{
						return;
					}

					AddHtml(x + 65, y + 130, 275, 40, time.WrapUOHtmlTag("CENTER").WrapUOHtmlColor(Color.Yellow, false), false, false);
					AddTooltip(1115782);
				});

			layout.AddBefore(
				"entry/bg/" + index,
				"entry/content/deferred/" + index,
				() =>
				{
					AddButton(x + 285, y + 101, 11281, 11280, b => OnNeed(entry)); //Need
					AddButton(x + 315, y + 101, 11280, 11280, b => OnGreed(entry)); //Greed
				});
		}

		protected virtual void OnHelp(DungeonLootEntry e)
		{
			Refresh(true);

			var html = new StringBuilder();

			html.AppendLine("Need Before Greed");
			html.AppendLine("If everyone declares Greed, then everyone rolls for the item;".WrapUOHtmlTag("SMALL"));
			html.AppendLine("If one or more people declare Need, only they roll on the item.".WrapUOHtmlTag("SMALL"));
			html.AppendLine("");
			html.AppendLine("GREED".WrapUOHtmlTag("B").WrapUOHtmlColor(Color.LawnGreen, Color.White));
			html.AppendLine(
				"Looting an item because you intend to sell it or pass it on to a friend or alt.".WrapUOHtmlTag("SMALL"));
			html.AppendLine("");
			html.AppendLine("NEED".WrapUOHtmlTag("B").WrapUOHtmlColor(Color.LawnGreen, Color.White));
			html.AppendLine(
				"Looting an item because you intend to use that item yourself, whether as equipment or for crafting purposes."
					.WrapUOHtmlTag("SMALL"));
			html.AppendLine("");

			new NoticeDialogGump(User)
			{
				HtmlColor = Color.White,
				Title = "Loot Help",
				Html = html.ToString()
			}.Send();
		}

		protected virtual void OnPass(DungeonLootEntry e)
		{
			e.Pass(User as PlayerMobile);

			Refresh(true);
		}

		protected virtual void OnNeed(DungeonLootEntry e)
		{
			e.Need(User as PlayerMobile);

			Refresh(true);
		}

		protected virtual void OnGreed(DungeonLootEntry e)
		{
			e.Greed(User as PlayerMobile);

			Refresh(true);
		}

		protected override bool OnBeforeSend()
		{
			return Dungeon != null && List.Count > 0 && base.OnBeforeSend();
		}
	}
}