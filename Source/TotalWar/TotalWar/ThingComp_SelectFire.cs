using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TotalWar
{
    class ThingComp_SelectFire : ThingComp
    {
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
        }

        public ThingComp_SelectFireProperties Props => (ThingComp_SelectFireProperties)this.props;
        public int longToVeryLong => Props.longToVeryLong;
        public int mediumToLong => Props.mediumToLong;
        public int shortToMedium => Props.shortToMedium;
        public int touchToShort => Props.touchToShort;
        public int zeroToTouch => Props.zeroToTouch;
        public int longRange => Props.longRange;
        public int mediumRange => Props.mediumRange;
        public int shortRange => Props.shortRange;
        public int touchRange => Props.touchRange;
        public int burstShots => Props.burstShots;
        public int ticksBetweenShots => Props.ticksBetweenShots;
        public float burstPenalty => Props.burstPenalty;
        public float autoPenalty => Props.autoPenalty;
        public int currentMode = 2;
        //Stores default warmup time for the weapon so it can
        //revert back after changing fire modes.
        public float warmupTime = 0f;
    }
}
