#region Header
//   Vorspire    _,-'/-'/  DungeonUI.cs
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

using Server;
using Server.Gumps;
using Server.Mobiles;

using VitaNex.SuperGumps;
#endregion

namespace VitaNex.Dungeons
{
	public enum DungeonLootMode
	{
		Standard,
		Advanced
	}

	public class DungeonUI : SuperGumpList<PlayerMobile>
	{
		public static void DisplayTo(PlayerMobile user, bool refreshOnly)
		{
			DisplayTo(user, refreshOnly, InstanceMaps.Instances.GetDungeon(user));
		}

		public static void DisplayTo(PlayerMobile user, bool refreshOnly, Dungeon dungeon)
		{
			var info = EnumerateInstances<DungeonUI>(user).FirstOrDefault(g => g != null && !g.IsDisposed && g.IsOpen);

			if (info == null)
			{
				if (refreshOnly)
				{
					return;
				}

				info = new DungeonUI(user, dungeon);
			}

			if (dungeon != null && !dungeon.Deleted)
			{
				info.Dungeon = dungeon;
			}

			info.Refresh(true);
		}

		public Dungeon Dungeon { get; set; }

		private bool _CompactView;

		public bool CompactView
		{
			get { return _CompactView; }
			set
			{
				_CompactView = value;
				CanClose = CanDispose = !_CompactView;
			}
		}

		public bool AlphaBG { get; set; }

		public DungeonUI(PlayerMobile user, Dungeon dungeon)
			: base(user, null, 610, 0)
		{
			Dungeon = dungeon;

			CanResize = false;
			CanMove = true;

			CompactView = true;

			Sorted = true;

			EntriesPerPage = 5;

			ForceRecompile = true;

			AutoRefreshRate = TimeSpan.FromSeconds(10.0);
			AutoRefresh = true;
		}

		public override void Close(bool all)
		{
			if (!all && !CompactView)
			{
				CompactView = true;
				Refresh(true);
				return;
			}

			base.Close(all);
		}

		protected override void Compile()
		{
			AlphaBG = !CompactView && User.InCombat();

			base.Compile();
		}

		protected override bool OnBeforeSend()
		{
			return Dungeon != null && base.OnBeforeSend();
		}

		protected override void CompileList(List<PlayerMobile> list)
		{
			list.Clear();
			list.AddRange(Dungeon.Group);

			base.CompileList(list);
		}

		protected override void CompileLayout(SuperGumpLayout layout)
		{
			base.CompileLayout(layout);

			layout.Add("header/bg", () => AddBackground(0, 0, 400, 30, 5120));

			layout.Add(
				"header/button/view",
				() =>
				{
					AddButton(10, 5, CompactView ? 9906 : 9900, CompactView ? 9908 : 9902, ToggleView);
					AddTooltip(CompactView ? 3002086 : 3002085);
				});

			layout.Add(
				"header/title",
				() =>
				{
					AddBackground(35, 5, 150, 20, 5120);
					AddTooltip(1075159);
	
					var title = Dungeon.Name;
	
					title = title.WrapUOHtmlSmall().WrapUOHtmlCenter().WrapUOHtmlColor(Color.PaleGoldenrod, false);
	
					AddHtml(35, 5, 150, 40, title, false, false);
					AddTooltip(1075159);
				});

			layout.Add(
				"header/time",
				() =>
				{
					AddImage(195, 1, 30223);
					AddTooltip(1115782);
	
					AddBackground(225, 5, 65, 20, 5120);
					AddTooltip(1115782);
	
					var now = DateTime.UtcNow;
					var time = Dungeon.Deadline > now ? (Dungeon.Deadline - now) : TimeSpan.Zero;
					var dtText = time.ToSimpleString("d:h:m:s");
	
					dtText = dtText.WrapUOHtmlSmall().WrapUOHtmlCenter().WrapUOHtmlColor(Color.Yellow, false);
	
					AddHtml(225, 5, 65, 40, dtText, false, false);
					AddTooltip(1115782);
				});

			layout.Add(
				"header/group",
				() =>
				{
					AddImage(300, 1, 30098);
					AddTooltip(3000332);
	
					AddBackground(330, 5, 65, 20, 5120);
					AddTooltip(3000332);
	
					var dgText = String.Format(
						"{0:#,0} / {1:#,0}",
						List.Count(m => InstanceMaps.Instances.GetDungeon(m) == Dungeon),
						Dungeon.GroupMax);
	
					dgText = dgText.WrapUOHtmlSmall().WrapUOHtmlCenter().WrapUOHtmlColor(Color.LawnGreen, false);
	
					AddHtml(330, 5, 65, 40, dgText, false, false);
					AddTooltip(3000332);
				});

			if (CompactView)
			{
				return;
			}

			layout.AddBefore(
				"header/bg",
				"body/bg",
				() =>
				{
					var h = Math.Max(220, 45 + (35 * Math.Min(EntriesPerPage, List.Count)));
	
					AddBackground(0, 0, 400, h, 5120);
	
					if (AlphaBG)
					{
						AddAlphaRegion(0, 0, 400, h);
					}
				});

			layout.Add(
				"body/desc",
				() =>
				{
					AddBackground(10, 40, 175, 80, 5120);
	
					if (AlphaBG)
					{
						AddAlphaRegion(10, 40, 175, 80);
					}
	
					AddTooltip(1079449);
	
					var desc = Dungeon.Desc.WrapUOHtmlSmall().WrapUOHtmlColor(Color.PaleGoldenrod, false);
	
					AddHtml(20, 45, 160, 70, desc, false, true);
					AddTooltip(1079449);
				});

			//DungeonCP?
			layout.Add(
				"body/panel",
				() =>
				{
					AddBackground(10, 130, 175, 80, 5120);
	
					if (AlphaBG)
					{
						AddAlphaRegion(10, 130, 175, 80);
					}
	
					if (User != Dungeon.ActiveLeader)
					{
						//AddHtml(20, 135, 160, 70, "?", false, true);
						return;
					}
	
					AddButton(
						20,
						135,
						4005,
						4007,
						b =>
						{
							switch (Dungeon.LootMode)
							{
								case DungeonLootMode.Standard:
									Dungeon.LootMode = DungeonLootMode.Advanced;
									break;
								case DungeonLootMode.Advanced:
									Dungeon.LootMode = DungeonLootMode.Standard;
									break;
							}
		
							Refresh(true);
						});
	
					var lootMode = "Loot: ";
	
					switch (Dungeon.LootMode)
					{
						case DungeonLootMode.Advanced:
							lootMode += "Need or Greed";
							break;
						default:
							lootMode += Dungeon.LootMode.ToString().SpaceWords();
							break;
					}
	
					AddHtml(55, 137, 125, 40, lootMode.WrapUOHtmlColor(Color.Gold, false), false, false);
				});

			layout.Add(
				"body/list",
				() =>
				{
					const int x = 195;
					const int y = 40;
	
					var range = GetListRange();
	
					var index = 0;
	
					foreach (var m in range.Values)
					{
						var o = 35 * index;
	
						var online = m.IsOnline();
						var inDungeon = InstanceMaps.Instances.GetDungeon(m) == Dungeon;
	
						int icon, tooltip;
	
						if (inDungeon)
						{
							if (!online)
							{
								icon = 30224;
								tooltip = 1153036;
							}
							else if (!m.Alive)
							{
								icon = 30215;
								tooltip = 1078368;
							}
							else if (m.InCombat())
							{
								icon = 30233;
								tooltip = 1078592;
							}
							else
							{
								icon = 30210;
								tooltip = 1112231;
							}
						}
						else if (!online)
						{
							icon = 30224;
							tooltip = 1153036;
						}
						else
						{
							icon = 30039;
							tooltip = 1112231;
						}
	
						AddImage(x, y + o, icon);
						AddTooltip(tooltip);
	
						AddBackground(x + 30, y + o, 150, 20, 5120);
	
						if (AlphaBG)
						{
							AddAlphaRegion(x + 30, y + o, 150, 20);
						}
	
						AddProperties(m);
	
						var name = m.RawName.WrapUOHtmlCenter().WrapUOHtmlColor(Color.LawnGreen, false);
	
						AddHtml(x + 30, y + o, 150, 40, name, false, false);
						AddProperties(m);
	
						AddImageTiled(x, y + 25 + o, 180, 3, online ? 30074 : 30072);
						AddTooltip(1053042);
	
						var bLength = (int)Math.Floor(180 * (m.Hits / (double)m.HitsMax));
	
						if (bLength > 0)
						{
							AddImageTiled(x, y + 25 + o, bLength, 3, 30073);
							AddTooltip(1053042);
						}
	
						++index;
					}
	
					range.Clear();
				});

			layout.Add(
				"body/list/scroll",
				() =>
				{
					var h = Math.Max(170, 35 * Math.Min(EntriesPerPage, List.Count));
	
					AddBackground(378, 40, 16, h, 5120);
	
					if (AlphaBG)
					{
						AddAlphaRegion(378, 40, 16, h);
					}
	
					AddScrollbarV(
						379,
						40,
						PageCount,
						Page,
						PreviousPage,
						NextPage,
						new Rectangle(3, 15, 10, 140),
						new Rectangle(0, 0, 16, 16),
						new Rectangle(0, 155, 16, 16),
						Tuple.Create(5124, 5104),
						Tuple.Create(9760, 9761, 9760),
						Tuple.Create(9764, 9765, 9764));
				});
		}

		protected virtual void ToggleView(GumpButton b)
		{
			CompactView = !CompactView;

			Refresh(true);
		}

		public override int SortCompare(PlayerMobile a, PlayerMobile b)
		{
			var res = 0;

			if (a.CompareNull(b, ref res))
			{
				return res;
			}

			Dungeon ad = InstanceMaps.Instances.GetDungeon(a), bd = InstanceMaps.Instances.GetDungeon(b);

			if (Dungeon == ad && Dungeon == bd)
			{
				if (a == ad.ActiveLeader && b != bd.ActiveLeader)
				{
					return -1;
				}

				if (a != ad.ActiveLeader && b == bd.ActiveLeader)
				{
					return 1;
				}

				bool ao = a.IsOnline(), bo = b.IsOnline();

				if (ao && !bo)
				{
					return -1;
				}

				if (!ao && bo)
				{
					return 1;
				}

				return 0;
			}

			if (Dungeon == ad && Dungeon != bd)
			{
				return -1;
			}

			if (Dungeon != ad && Dungeon == bd)
			{
				return 1;
			}

			return 0;
		}
	}
}