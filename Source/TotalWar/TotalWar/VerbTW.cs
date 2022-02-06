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
            Thing equipment = base.EquipmentSource;
            ThingComp_SelectFire selectFire = equipment.TryGetComp<ThingComp_SelectFire>();
            if (selectFire != null) {
                ThingComp_SelectFireProperties selectFireProps =
                    (ThingComp_SelectFireProperties)equipment.TryGetComp<ThingComp_SelectFire>().props;
                if (selectFireProps != null)
                { 
                    if (selectFire.warmupTime == 0f)
                    {
                        selectFire.warmupTime = verbProps.warmupTime;
                    }
                    //The float value of distance to the target squared
                    float shotDistanceSquared = ((float)this.CasterPawn.Position.DistanceToSquared(CurrentTarget.Cell));
                    int fireMode = selectFire.currentMode;
                    if (!selectFire.forceMode)
                    {
                        if (shotDistanceSquared > selectFireProps.longRange * selectFireProps.longRange)
                        {
                            fireMode = selectFireProps.longToVeryLong;
                        }
                        else if (shotDistanceSquared > selectFireProps.mediumRange * selectFireProps.mediumRange)
                        {
                            fireMode = selectFireProps.mediumToLong;
                        }
                        else if (shotDistanceSquared > selectFireProps.shortRange * selectFireProps.shortRange)
                        {
                            fireMode = selectFireProps.shortToMedium;
                        }
                        else if (shotDistanceSquared > selectFireProps.touchRange * selectFireProps.touchRange)
                        {
                            fireMode = selectFireProps.touchToShort;
                        }
                        else
                        {
                            fireMode = selectFireProps.zeroToTouch;
                        }
                    }
                    if (fireMode == 0)
                    {
                        this.burstShotsLeft = 1;
                        verbProps.warmupTime = selectFireProps.ticksBetweenShots.TicksToSeconds();
                    }
                    else if (fireMode == 1)
                    {
                        this.burstShotsLeft = 3;
                        verbProps.warmupTime = selectFireProps.ticksBetweenShots.TicksToSeconds();
                    }
                    else
                    {
                        this.burstShotsLeft = 1;
                        verbProps.warmupTime = selectFire.warmupTime;
                    }
                    selectFire.currentMode = fireMode;
                    this.state = VerbState.Bursting;
                    this.TryCastNextBurstShot();
                } else
                {
                    base.WarmupComplete();
                }
            } else
            {
                base.WarmupComplete();
            }
        }
    }
}
