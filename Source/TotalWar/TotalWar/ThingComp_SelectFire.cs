﻿using System;
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
        public float auto => Props.auto;
        public float burst => Props.burst;
        public float semi => Props.semi;
        public int burstShots => Props.burstShots;
        public int ticksBetweenShots => Props.ticksBetweenShots;
    }
}
