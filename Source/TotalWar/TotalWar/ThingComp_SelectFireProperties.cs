using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace TotalWar
{
    public class ThingComp_SelectFireProperties : CompProperties
    {
        //0 is semi, 1 is burst, 2 is auto
        public int longToVeryLong = 0;
        // Default > 25 tiles
        public int mediumToLong = 0;
        // Default < 25 Tiles
        public int shortToMedium = 1;
        // Default < 12 tiles
        public int touchToShort = 2;
        // Defualt < 3 tiles
        public int zeroToTouch = 2;

        //Sets definition of range for current weapon (in tiles)
        public int longRange = 40;
        public int mediumRange = 25;
        public int shortRange = 12;
        public int touchRange = 3;

        //Time between shots whether semi or burst is the same
        public int burstShots = 3;
        public int ticksBetweenShots = 60;

        //Accuracy penalty for different select fire
        //Decreases chances for not missing and not hitting cover
        //by the given percentage
        public float burstPenalty = 0.1f;
        public float autoPenalty = 0.6f;

        public ThingComp_SelectFireProperties()
        {
            this.compClass = typeof(ThingComp_SelectFire);
        }

        //Adds entries into the description for the values of the new weapon settings
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
        {
            foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
            {
                yield return statDrawEntry;
            }
            IEnumerator<StatDrawEntry> enumerator = null;
            yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Ranged, "Number of Burst Shots", burstShots.ToString(), null, 10000, null, null, false);
            yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Ranged, "Aim Readjustment Time", ((float)ticksBetweenShots / 60f).ToString() + " s", null, 10001, null, null, false);
            yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Ranged, "Burst Penalty", "-" + (burstPenalty * 100).ToString() + "%", null, 10002, null, null, false);
            yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Ranged, "Auto Penalty", "-" + (autoPenalty * 100).ToString() + "%", null, 10003, null, null, false);
        }
    }
}
