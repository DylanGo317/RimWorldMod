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
        public float weaponFailureFixTime;
        private int ticksToShoot; //Stored tick value reached before canShoot is set back to true
        private int ticksSinceLastShot;

        //Set initial values on initialization
        public override void Initialize(CompProperties props)
        {
            weaponFailureFixTime = Props.weaponFailureFixTime;
            //Set these two equal so that the ticks are not incremented
            ticksToShoot = (int)Math.Round((decimal)weaponFailureFixTime * 60, 0);
            ticksSinceLastShot = ticksToShoot;
        }

        public override void CompTick()
        {
            if (ticksSinceLastShot <= ticksToShoot)
            {

            }
            else
            {
                canShoot = true;
            }
        }
        
        //Used to set ticksSinceLastShot back to zero so that
        //and canShoot to false so that the timer will start
        //and the weapon is temporarily disabled
        public void setTickSinceLastShot(int tick)
        {
            ticksSinceLastShot = 0;
            canShoot = false;
        }

    }

    public class ThingComp_ReliabilityProperties : CompProperties
    {
        public float weaponFailureFixTime;
        public float weaponSuccessChance;
    }
}
