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
    [HarmonyPatch(typeof(WorkGiver_HunterHunt), "HasHuntingWeapon")]
    class Harmony_TWHunting
    {
        static void Postfix(Pawn p, ref bool __result)
        {
            if (!__result && p.equipment.Primary.def.Verbs != null)
            {
                foreach (VerbProperties v in p.equipment.Primary.def.Verbs)
                {
                    if (typeof(Verb_LaunchProjectileTW).IsAssignableFrom(v.verbClass))
                    {
                        __result = true;
                    }
                }
            }
        }
    }
}
