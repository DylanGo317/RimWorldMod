using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TotalWar
{
    public class ThingComp_SelectFireProperties : CompProperties
    {
        //0 is semi, 1 is burst, 2 is auto
        // Default > 25 tiles
        public int MediumToLong = 0;
        // Default < 25 Tiles
        public int ShortToMedium = 0;
        // Default < 12 tiles
        public int TouchToShort = 1;
        // Defualt < 3 tiles
        public int ZeroToTouch = 2;

        //Sets definition of range for current weapon (in tiles)
        public int longRange = 40;
        public int mediumRange = 25;
        public int shortRange = 12;
        public int touchRange = 3;

        //Sets weapon accuracy multiplier
        public float auto = 0.5f;
        public float burst = 0.8f;
        public float semi = 1f;

        //Time between shots whether semi or burst is the same
        public int burstShots = 3;
        public int ticksBetweenShots = 60;
    }
}
