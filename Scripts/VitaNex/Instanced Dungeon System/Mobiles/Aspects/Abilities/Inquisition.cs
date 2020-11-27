#region Header
//   Vorspire    _,-'/-'/  Inquisition.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using VitaNex;
using VitaNex.FX;
using VitaNex.Network;
using VitaNex.SuperGumps;
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityInquisition : ExplodeAspectAbility
	{
		public override string Name { get { return "Inquisition"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Despair | AspectFlags.Death | AspectFlags.Decay; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(60); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(10); } }

		protected override BaseExplodeEffect CreateEffect(BaseAspect aspect)
		{
			var range = (int)Math.Ceiling(aspect.RangePerception * (aspect.Hits / (double)aspect.HitsMax));

			return new SmokeExplodeEffect(aspect.Location, aspect.Map, Math.Max(5, range));
		}

		protected override void OnLocked(BaseAspect aspect)
		{
			base.OnLocked(aspect);

			aspect.Yell("JUSTIFY YOUR EXISTENCE!");
		}

		protected override void OnUnlocked(BaseAspect aspect)
		{
			base.OnUnlocked(aspect);

			aspect.Yell("YOUR INQUISITION IS NIGH!");
		}

		protected override void OnAdded(State state)
		{
			base.OnAdded(state);

			if (!state.IsValid)
			{
				return;
			}

			state.Target.TryParalyze(Duration);

			state.Aspect.Hits -= (int)Math.Floor(10.0 * Duration.TotalSeconds);
			state.Aspect.Mana -= (int)Math.Floor(30.0 * Duration.TotalSeconds);

			if (state.Target is PlayerMobile)
			{
				new InquisitionGump((PlayerMobile)state.Target, Duration).Send();
			}
		}

		protected override void OnRemoved(State state)
		{
			base.OnRemoved(state);

			if (state.Target == null)
			{
				return;
			}

			if (state.Target.Paralyzed)
			{
				state.Target.Paralyzed = false;
			}

			state.Target.SendMessage(state.Aspect.SpeechHue, "[{0}]: The inquisition is over.", state.Aspect.RawName);
		}

		protected sealed class InquisitionPath : IEnumerable<Point3D>
		{
			public Point3D[] Path { get; set; }
			public int Index { get; set; }

			public int Length { get { return Path.Length; } }

			public Point3D Current { get { return this[Index]; } }

			public Point3D this[int index] { get { return Path.InBounds(index) ? Path[index] : Point3D.Zero; } }

			public object Value { get; set; }

			public InquisitionPath(object value, params Point3D[] path)
			{
				Value = value;
				Path = path ?? new Point3D[0];
				Index = 0;
			}

			public IEnumerator<Point3D> GetEnumerator()
			{
				return Path.AsEnumerable().GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		protected sealed class InquisitionGump : SuperGumpList<InquisitionPath>
		{
			private static readonly Rectangle3D _Bounds = new Rectangle3D(0, 0, 0, 1920, 1080, 25);

			private static readonly Color[] _HtmlColors =
			{
				Color.LightGray, Color.Red, Color.DarkRed, Color.OrangeRed,
				Color.DarkOrange
			};

			private static readonly string[] _Phrases =
			{
				"hell is empty and all the devils are here",
				"we enjoy the night, the darkness, where we can do things that aren't " +
				"acceptable in the light.\nnight is when we slake our thirst",
				"its symptoms are madness, caused by the music in his head, sung by an endless choir, " +
				"called 'the voice of the dead'",
				"once the game is over, the king and the pawn go back in the same box",
				"he who dies with the most toys is, nonetheless, still dead",
				"it is possible to provide security against other ills, but as far as death is concerned, " +
				"we men live in a city without walls",
				"death most resembles a prophet who is without honor in his own land or a poet " +
				"who is a stranger among his people",
				"death is the only inescapable, unavoidable, sure thing.\nwe are sentenced to die the day we're born",
				"a man who won't die for something is not fit to live", "one death is a tragedy.\na million deaths is a statistic",
				"you can discover what your enemy fears most by observing the means he uses to frighten you",
				"courage is not the absence of fear, but rather the judgement that something else is more important than fear",
				"the only thing we have to fear is fear itself", "the basis of optimism is sheer terror"
			};

			private static readonly int[] _Images =
			{
				//
				69, 70, 101, 102, 2251, 2259, 2274, 2278, 2279, //
				2280, 2300, 7011, 7019, 7034, 7038, 7039, 7040, //
				7060, 9804, 30501, 30505 //
			};

			private static readonly int[] _ItemIDs =
			{
				//
				3786, 3787, 3789, 3790, 3791, 3792, 3793, 3794, //
				3799, 3800, 4650, 4651, 4652, 4653, 4654, 4655, //
				6657, 6658, 6659, 6660, 6661, 6662, 6663, 6664, //
				6665, 6666, 6667, 6668, 6669, 6670 //
			};

			private DateTime? _Sent;
			private Body _BodyMod;
			private int _PhraseCount;

			public TimeSpan Duration { get; set; }

			public InquisitionGump(PlayerMobile user, TimeSpan duration)
				: base(user, null, 0, 0, null)
			{
				Duration = duration;

				CanClose = false;
				CanDispose = false;
				CanMove = false;
				CanResize = false;

				Modal = true;
				ModalSafety = false;

				Sorted = true;
				ForceRecompile = true;

				AutoRefreshRate = TimeSpan.FromMilliseconds(100.0);
				AutoRefresh = true;
			}

			protected override void CompileList(List<InquisitionPath> list)
			{
				if (_Sent == null)
				{
					list.Clear();
					list.AddRange(GenerateSprites());
				}
				else
				{
					list.RemoveAll(e => e == null || e.Length == 0 || e.Index >= e.Length);
					list.ForEach(e => ++e.Index);
				}

				EntriesPerPage = list.Count;
			}

			protected override void CompileLayout(SuperGumpLayout layout)
			{
				base.CompileLayout(layout);

				var range = GetListRange();

				if (range != null && range.Count > 0)
				{
					range.Values.For((index, entry) => CompileEntryLayout(layout, index, entry));
				}
			}

			public void CompileEntryLayout(SuperGumpLayout layout, int index, InquisitionPath entry)
			{
				layout.Add(
					"sprite/" + index,
					xpath =>
					{
						var cur = entry.Current;

						if (entry.Value is string)
						{
							AddHtml(cur.X, cur.Y, 200, 200, (string)entry.Value, false, false);
						}
						else if (entry.Value is Pair<bool, int>)
						{
							var pair = (Pair<bool, int>)entry.Value;

							if (pair.Left)
							{
								AddItem(cur.X, cur.Y, pair.Right, 0);
							}
							else
							{
								AddImage(cur.X, cur.Y, pair.Right, 0);
							}
						}
					});
			}

			private IEnumerable<InquisitionPath> GenerateSprites()
			{
				var rate = AutoRefreshRate.TotalMilliseconds;
				var duration = Duration.TotalMilliseconds;

				var frames = (int)(Math.Max(rate, duration) / rate);

				//var center = new Point2D(_Bounds.Start.X + (_Bounds.Width / 2), _Bounds.Start.Y + (_Bounds.Height / 2));

				for (var index = 0; index < _Bounds.Depth; index++)
				{
					var sprite = GenerateSprite();

					var html = sprite is string;

					var o = html ? 200 : 100;

					var start = new Point2D(
						Utility.RandomMinMax(_Bounds.Start.X, _Bounds.Start.X + (_Bounds.Width - o)),
						Utility.RandomMinMax(_Bounds.Start.Y, _Bounds.Start.Y + (_Bounds.Height - o)));

					var end = new Point2D(
						Utility.RandomMinMax(_Bounds.Start.X, _Bounds.Start.X + (_Bounds.Width - o)),
						Utility.RandomMinMax(_Bounds.Start.Y, _Bounds.Start.Y + (_Bounds.Height - o)));

					var path = new Point3D[frames];

					path.SetAll(frame => start.Lerp2D(end, frame / (double)frames).ToPoint3D(index));

					yield return new InquisitionPath(sprite, path);
				}
			}

			public object GenerateSprite()
			{
				if (Utility.RandomBool() && _PhraseCount < 5)
				{
					++_PhraseCount;
					return RandomFormatting(_Phrases.GetRandom());
				}

				var item = Utility.RandomBool();

				return Pair.Create(item, (item ? _ItemIDs : _Images).GetRandom());
			}

			public string RandomFormatting(string html)
			{
				var chars = html.ToCharArray();

				chars.SetAll(
					(j, c) =>
						Char.IsLetter(c) && Utility.RandomDouble() <= 0.33 ? (Char.IsUpper(c) ? Char.ToLower(c) : Char.ToUpper(c)) : c);

				html = String.Join(String.Empty, chars);

				switch (Utility.Random(3))
				{
					case 0:
						html = html.WrapUOHtmlTag("small");
						break;
					case 1:
						html = html.WrapUOHtmlTag("big");
						break;
				}

				return html.WrapUOHtmlColor(Color.DarkGray.Interpolate(_HtmlColors.GetRandom(), Utility.RandomDouble()), false);
			}

			protected override void OnSend()
			{
				base.OnSend();

				if (IsDisposed || _Sent != null)
				{
					return;
				}

				_Sent = DateTime.UtcNow;

				_BodyMod = User.BodyMod;

				User.SendSound(1383); // icu

				//User.BodyMod = User.Race.GhostBody(User);
				ScreenFX.FadeOut.Send(User);

				User.TryParalyze(Duration, m => m.SendMessage("The inquisition is over."));
			}

			protected override void OnClosed(bool all)
			{
				if (_Sent != null)
				{
					_Sent = null;

					//User.BodyMod = _BodyMod;
					ScreenFX.FadeIn.Send(User);
				}

				base.OnClosed(all);
			}

			protected override bool CanAutoRefresh()
			{
				return _Sent != null && base.CanAutoRefresh();
			}

			protected override void OnAutoRefresh()
			{
				base.OnAutoRefresh();

				if (_Sent != null && DateTime.UtcNow > _Sent + Duration)
				{
					Close(true);
				}
			}

			public override int SortCompare(InquisitionPath a, InquisitionPath b)
			{
				var result = 0;

				if (a.CompareNull(b, ref result))
				{
					return result;
				}

				var zL = a.Current.Z;
				var zR = b.Current.Z;

				if (zL > zR)
				{
					return 1;
				}

				if (zL < zR)
				{
					return -1;
				}

				return 0;
			}
		}
	}
}