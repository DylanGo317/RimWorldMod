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
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.Tick))]
    class Harmony_TrainingAlert
    {
        static void Postfix(ref Pawn_EquipmentTracker ___equipment, Pawn __instance)
        {
            if (___equipment != null)
            {
                if (___equipment.Primary != null)
                {
                    ThingComp_Training training = ___equipment.Primary.TryGetComp<ThingComp_Training>();
                    if (training != null)
                    {
                        if (Find.TickManager.TicksGame >= training.lastShotTick + training.ticksToShoot && !training.alerted && training.ticksToShoot >= 120)
                        {
                            training.alerted = true;
                            MoteMaker.ThrowText(__instance.PositionHeld.ToVector3(), __instance.MapHeld, "Ready to Fire!", 1f);
                        }
                    }
                }
            }
        }
    }
}
