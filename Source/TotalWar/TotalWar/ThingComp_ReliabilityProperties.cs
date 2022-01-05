using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TotalWar
{
    public class ThingComp_ReliabilityProperties : CompProperties
    {
        public float weaponFailureFixTime = 5.0f; //Sets amount of time that a weapon is disabled when a failure occurs in XML
        public float weaponSuccessChance = 0.98f; //Sets chance of a weapon succeeding in XML

        public ThingComp_ReliabilityProperties()
        {
            this.compClass = typeof(ThingComp_Reliability);
        }
    }
}
