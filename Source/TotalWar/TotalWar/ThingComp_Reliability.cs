using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TotalWar
{
    class ThingComp_Reliability : ThingComp
    {
        public bool canShoot = true;
        public ThingComp_ReliabilityProperties Props => (ThingComp_ReliabilityProperties)this.props;
        public float weaponFailureFixTime => Props.weaponFailureFixTime;
        public float weaponSuccessChance => Props.weaponSuccessChance;
        public float weaponSuccess = 0f;
        private int ticksToShoot; //Stored tick value reached before canShoot is set back to true
        private int failureTick; //Tick that the weapon failed

        public override void Initialize(CompProperties props)
        {
            //Set these two equal so that the ticks are not incremented
            //Converts seconds to ticks
            base.Initialize(props);
            ticksToShoot = (int) (weaponFailureFixTime * 60);
        }

        //Disables shooting
        public void resetTickSinceLastShot()
        {
            failureTick = Find.TickManager.TicksGame;
            canShoot = false;
        }

        //Sets weapon reliability based on weapon quality
        public void setReliability()
        {
            CompQuality compQual = parent.TryGetComp<CompQuality>();
            if (compQual != null)
            {
                float temp = Props.weaponSuccessChance;
                if (compQual.Quality == QualityCategory.Awful)
                {
                    weaponSuccess = temp - ((1f - temp) * 2f);
                }
                else if (compQual.Quality == QualityCategory.Poor)
                {
                    weaponSuccess = temp - ((1f - temp));
                }
                else if (compQual.Quality == QualityCategory.Normal)
                {
                    weaponSuccess = temp;
                }
                else if (compQual.Quality == QualityCategory.Good)
                {
                    weaponSuccess = temp + ((1f - temp) * 0.2f);
                }
                else if (compQual.Quality == QualityCategory.Excellent)
                {
                    weaponSuccess = temp + ((1f - temp) * 0.4f);
                }
                else if (compQual.Quality == QualityCategory.Masterwork)
                {
                    weaponSuccess = temp + ((1f - temp) * 0.6f);
                }
                else if (compQual.Quality == QualityCategory.Legendary)
                {
                    weaponSuccess = temp + ((1f - temp) * 0.8f);
                }
            }
        }

        //Checks whether enough ticks have passed and updates
        //canShoot accordingly
        public void checkStatus()
        {
            if (Find.TickManager.TicksGame - failureTick >= ticksToShoot)
            {
                canShoot = true;
            }
        }

        //Adds entries into the description for the values of the new weapon settings
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            if (weaponSuccess == 0f)
            {
                CompQuality compQual = parent.TryGetComp<CompQuality>();
                if (compQual != null)
                {
                    float temp = Props.weaponSuccessChance;
                    if (compQual.Quality == QualityCategory.Awful)
                    {
                        weaponSuccess = temp - ((1f - temp) * 2f);
                    }
                    else if (compQual.Quality == QualityCategory.Poor)
                    {
                        weaponSuccess = temp - ((1f - temp));
                    }
                    else if (compQual.Quality == QualityCategory.Normal)
                    {
                        weaponSuccess = temp;
                    }
                    else if (compQual.Quality == QualityCategory.Good)
                    {
                        weaponSuccess = temp + ((1f - temp) * 0.2f);
                    }
                    else if (compQual.Quality == QualityCategory.Excellent)
                    {
                        weaponSuccess = temp + ((1f - temp) * 0.4f);
                    }
                    else if (compQual.Quality == QualityCategory.Masterwork)
                    {
                        weaponSuccess = temp + ((1f - temp) * 0.6f);
                    }
                    else if (compQual.Quality == QualityCategory.Legendary)
                    {
                        weaponSuccess = temp + ((1f - temp) * 0.8f);
                    }
                }
            }
            IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
            if (enumerable != null) { 
                foreach (StatDrawEntry statDrawEntry in enumerable)
                {
                    yield return statDrawEntry;
                }
            }
            yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Ranged, "Time to fix weapon failure", weaponFailureFixTime.ToString() + " s", null, 9999, null, null, false);
            yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Ranged, "Base Weapon reliability", (weaponSuccess * 100).ToString() + "%", null, 9998, null, null, false);
        }
    }
}
