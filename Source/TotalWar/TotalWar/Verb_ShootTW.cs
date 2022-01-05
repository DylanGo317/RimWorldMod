using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace TotalWar
{
    //Subclasses Verb_LaunchProjectileTW and adds the same functionality
    //that Verb_Shoot adds to Verb_LaunchProjectile
    class Verb_ShootTW : Verb_LaunchProjectileTW
    {
        //These three methods are the same as the vanilla
        //Verb_Shoot because their functionality does not have to be
        //changed, although this is still the class that
        //the weapon calls in xml.
        protected override int ShotsPerBurst
        {
            get
            {
                return this.verbProps.burstShotCount;
            }
        }

        public override void WarmupComplete()
        {
            base.WarmupComplete();
            Pawn pawn = this.currentTarget.Thing as Pawn;
            if (pawn != null && !pawn.Downed && this.CasterIsPawn && this.CasterPawn.skills != null)
            {
                float num = pawn.HostileTo(this.caster) ? 170f : 20f;
                float num2 = this.verbProps.AdjustedFullCycleTime(this, this.CasterPawn);
                this.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2, false);
            }
        }

        protected override bool TryCastShot()
        {
            bool flag = base.TryCastShot();
            if (flag && this.CasterIsPawn)
            {
                this.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
            }
            return flag;
        }
    }
}
