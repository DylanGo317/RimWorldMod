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
            ticksToShoot = (int) (weaponFailureFixTime * 60);
        }

        //Used to set ticksSinceFailure back to zero so that
        //and canShoot to false so that the timer will start
        //and the weapon is temporarily disabled
        public void resetTickSinceLastShot()
        {
            failureTick = Find.TickManager.TicksGame;
            canShoot = false;
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

        public bool canShoot = true;
        public ThingComp_ReliabilityProperties Props => (ThingComp_ReliabilityProperties)this.props;
        public float weaponFailureFixTime => Props.weaponFailureFixTime;
        private int ticksToShoot; //Stored tick value reached before canShoot is set back to true
        private int failureTick; //Tick that the weapon failed
    }
}
