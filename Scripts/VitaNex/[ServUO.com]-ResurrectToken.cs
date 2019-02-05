#region Header
//   Vorspire    _,-'/-'/  ResurrectToken.cs
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

using Server;
using Server.Gumps;

using VitaNex.SuperGumps.UI;
#endregion

namespace VitaNex.Items
{
	public class ResurrectToken : Item
	{
		public override bool DisplayLootType { get { return false; } }

		[Constructable]
		public ResurrectToken()
			: base(0x2AAA)
		{
			Name = "Resurrection Token";
			Weight = 1.0;
			Hue = 85;
			Stackable = true;
			LootType = LootType.Blessed;
		}

		public ResurrectToken(Serial serial)
			: base(serial)
		{ }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add("Use: Resurrects you when you are dead".WrapUOHtmlColor(Color.LawnGreen));
		}

		public override void OnDoubleClick(Mobile m)
		{
			if (this.CheckDoubleClick(m, true, true, 2, true, false, false))
			{
				Resurrect(m);
			}
		}

		public override DeathMoveResult OnInventoryDeath(Mobile parent)
		{
			Timer.DelayCall(TimeSpan.FromSeconds(1.0), HandleDeath, parent);

			return base.OnInventoryDeath(parent);
		}

		protected virtual void HandleDeath(Mobile m)
		{
			if (!m.Alive)
			{
				new ConfirmResurrectGump(m, this).Send();
			}
		}

		public bool Resurrect(Mobile m)
		{
			if (m.Alive)
			{
				m.SendMessage("You may look and feel dead, but you are not!");
				return false;
			}

			m.Resurrect();

			if (!m.Alive)
			{
				return false;
			}

			Consume();
			return true;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();
		}

		public sealed class ConfirmResurrectGump : ConfirmDialogGump
		{
			private ResurrectToken _Token;

			public override bool InitPolling { get { return true; } }

			public ConfirmResurrectGump(Mobile m, ResurrectToken t)
				: base(m)
			{
				_Token = t;

				Modal = false;
				CanMove = true;

				Icon = _Token.ItemID;
				IconHue = _Token.Hue;
				IconItem = true;

				Title = "You Are Dead";
				Html = "You can resurrect using a Resurrection Token!\n\nClick OK to resurrect now.";
			}

			protected override void OnAccept(GumpButton button)
			{
				_Token.Resurrect(User);

				base.OnAccept(button);
			}

			protected override bool CanAutoRefresh()
			{
				return User.Alive || base.CanAutoRefresh();
			}

			protected override bool OnBeforeSend()
			{
				return !User.Alive && base.OnBeforeSend();
			}

			protected override void OnDisposed()
			{
				_Token = null;

				base.OnDisposed();
			}
		}
	}
}