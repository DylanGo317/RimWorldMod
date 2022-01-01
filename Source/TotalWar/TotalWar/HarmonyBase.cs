using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using Verse;
using HarmonyLib;

namespace TotalWar
{
    [StaticConstructorOnStartup]
    public static class HarmonyBase
    {
        //Creates a harmony instance and runs all patches
        static HarmonyBase()
        {
            var harmony = new Harmony("Rimworld.TotalWar");
            harmony.PatchAll();
            //Uncomment to make sure harmony is instantiated by the game
            //Log.Message("Harmony for Total War");
        }
    }
}
