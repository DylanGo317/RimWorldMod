using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

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

        //Adds entries into the description for the values of the new weapon settings
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
        {
            foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
            {
                yield return statDrawEntry;
            }
            yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Ranged, "Time to fix weapon failure", weaponFailureFixTime.ToString() + " s", null, 9999, null, null, false);
            yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Ranged, "Base Weapon reliability", (weaponSuccessChance * 100).ToString() + "%", null, 9998, null, null, false);
        }
    }
}
