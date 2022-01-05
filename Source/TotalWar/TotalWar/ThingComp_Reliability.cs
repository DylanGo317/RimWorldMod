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
        //Set initial values on initialization
        public override void Initialize(CompProperties props)
        {
            //Set these two equal so that the ticks are not incremented
            //Converts seconds to ticks
            base.Initialize(props);
            ticksToShoot = (int)weaponFailureFixTime;
            ticksSinceFailure = ticksToShoot;
        }

        //Increments ticksSinceFailure every tick and sets
        //canShoot to true when ticksSinceFailure is equal
        //to or greater than ticksToShoot
        public override void CompTick()
        {
            base.CompTick();
            if (ticksSinceFailure < ticksToShoot)
            {
                ticksSinceFailure++;
            }
            else
            {
                canShoot = true;
            }
        }

        //Used to set ticksSinceFailure back to zero so that
        //and canShoot to false so that the timer will start
        //and the weapon is temporarily disabled
        public void resetTickSinceLastShot()
        {
            ticksSinceFailure = 0;
            canShoot = false;
        }

        public bool canShoot = true;
        public ThingComp_ReliabilityProperties Props => (ThingComp_ReliabilityProperties)this.props;
        public float weaponFailureFixTime => Props.weaponFailureFixTime;
        private int ticksToShoot; //Stored tick value reached before canShoot is set back to true
        private int ticksSinceFailure; //Stored tick value that counts number of ticks since failure
    }
}
