using System;
using System.Collections;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientAwakenAllSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Awaken All", "Vas An Zu",
                                                        218,
                                                        9031,
                                                        Reagent.Garlic,
                                                        Reagent.Ginseng
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public AncientAwakenAllSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if(CheckSequence())
                Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);


                ArrayList targets = new ArrayList();
                if (this.Scroll != null)
                    Scroll.Consume();
                Map map = Caster.Map;

                if (map != null)
                {

                    IPooledEnumerable eable = map.GetItemsInRange(new Point3D(p), 3);

                    foreach (Item m in eable)
                    {


                        if (Caster.CanSee(m) && m is SleepingBody)
                            targets.Add(m);
                    }

                    eable.Free();
                }

                for (int i = 0; i < targets.Count; ++i)
                {

                    SleepingBody m = (SleepingBody)targets[i];


                    if (m != null)
                    {
                        m.Owner.RevealingAction();
                        m.Owner.Frozen = false;
                        m.Owner.Squelched = false;
                        m.Owner.Map = m.Map;
                        m.Owner.Location = m.Location;
                        m.Owner.Animate(21, 6, 1, false, false, 0);

                        m.Owner.SendMessage("You wake up!");

                        m.Delete();
                    }
                    Caster.SendMessage("You awaken them!");

                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AncientAwakenAllSpell m_Owner;

            public InternalTarget(AncientAwakenAllSpell owner)
                : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is SleepingBody)
                {
                    IPoint3D p = o as IPoint3D;

                    if (p != null)
                        m_Owner.Target(p);
                }
                else
                    from.SendMessage("That is not a slumbering being");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
