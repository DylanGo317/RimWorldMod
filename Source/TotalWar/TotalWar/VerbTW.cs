using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace TotalWar
{
    public abstract class VerbTW : Verse.Verb
    {
        //It makes sense to automatically decide select fire
        //before the first series of shots are fired
        public override void WarmupComplete()
        {
            //The float value of distance to the target squared
            float shotDistanceSquared = ((float)this.CasterPawn.Position.DistanceToSquared(CurrentTarget.Cell));
            //set this.verbProps.accuracyTouch/Short/Medium/Long
            //Retrieve thingComp that stores information on range, accuracy, and fire modes here
            Log.Message(shotDistanceSquared.ToString());
            base.WarmupComplete();
        }
    }
}
