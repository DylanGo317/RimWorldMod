using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace TotalWar
{
    //Potential field used to manipulate the success chance of a weapon
    //firing, which upon failure requires that the pawn fix the failure
    class ModExtension_Reliability : DefModExtension
    {
        public float weaponSuccessChance = 0.95f;
    }
}
