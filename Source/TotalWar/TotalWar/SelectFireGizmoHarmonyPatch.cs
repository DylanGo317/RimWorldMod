using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;

namespace TotalWar
{
    //Adds selectFire gizmo to any pawn that has a weapon with with ThingComp_SelectFire
    [HarmonyPatch(typeof(Pawn), "GetGizmos")]
    class SelectFireGizmoHarmonyPatch
    {
        static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> gizmos, Pawn __instance)
        {
            foreach (var gizmo in gizmos)
            {
                yield return gizmo;
            }
            if (__instance.equipment != null)
            {
                ThingComp_SelectFire selectFire = __instance.equipment.Primary.TryGetComp<ThingComp_SelectFire>();
                if (selectFire != null)
                { 
                    Command_Toggle selectFireToggle = new Command_Toggle();
                    selectFireToggle.icon = TexCommand.SquadAttack;
                    selectFireToggle.isActive = (() => selectFire.forceMode);
                    selectFireToggle.activateIfAmbiguous = false;
                    //Layered ternary operators appear to be necessary to get the text to change
                    selectFireToggle.defaultLabel = !selectFire.forceMode ? "Auto Select Fire" : (selectFire.currentMode == 0 ? "Semi-Automatic" : 
                        (selectFire.currentMode == 1 ? "Burst Fire" : "Automatic Fire"));
                    selectFireToggle.defaultDesc = "Selection for firing mode";
                    selectFireToggle.toggleAction = delegate ()
                    {
                        if (!selectFire.forceMode)
                        {
                            selectFire.forceMode = true;
                            selectFire.currentMode = 0;
                        }
                        else if (selectFire.forceMode && selectFire.currentMode == 0)
                        {
                            selectFire.currentMode = 1;
                        }
                        else if (selectFire.forceMode && selectFire.currentMode == 1)
                        {
                            selectFire.currentMode = 2;
                        }
                        else
                        {
                            selectFire.forceMode = false;
                        }
                    
                    };
                    yield return selectFireToggle;
                }
            }
        }
    }
}
