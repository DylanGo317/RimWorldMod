using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;

namespace TotalWar
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "PawnGotoAction")]
    class Harmony_MoveImmediatelyPatch
    {
        static void Postfix(Pawn pawn)
        {
            if (pawn.equipment != null)
            {
                ThingComp_Training training = pawn.equipment.Primary.TryGetComp<ThingComp_Training>();
                if (training != null)
                {
                    if (training.ticksToShoot == 0)
                    {
                        List<Verb> verbs = pawn.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
                        foreach (Verb c in verbs)
                        {
                            //Stores reload time into the ThingComp
                            if (c.GetType().ToString().Equals("TotalWar.Verb_ShootTW"))
                            {
                                training.ticksToShoot = (int)(c.verbProps.AdjustedCooldownTicks(c, pawn));
                            }
                        }
                    }
                    training.alerted = false;
                    pawn.stances.SetStance(new Stance_Mobile());
                }
            }
        }
    }
}
