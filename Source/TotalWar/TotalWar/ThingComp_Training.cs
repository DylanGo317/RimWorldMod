using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace TotalWar
{
    class ThingComp_Training : ThingComp
    {
        public int lastShotTick;
        public int ticksToShoot; //Normal weapon reload time
        public String pawnName;
        public bool alerted = false;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
        }
    }
}
