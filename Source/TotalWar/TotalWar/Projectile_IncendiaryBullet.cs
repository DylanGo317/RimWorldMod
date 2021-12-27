using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace TotalWar
{
    //Codes for the chance of the hediff being added to the target upon a successful shot
    class Projectile_IncendiaryBullet : Bullet
    {
        public ModExtension_IncendiaryBullet Props => base.def.GetModExtension<ModExtension_IncendiaryBullet>();

        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
            if (Props != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                //Chance for the hediff to be applied to the pawn
                float rand = Rand.Value;
                if (rand <= Props.addHediffChance)
                {
                    Messages.Message("TotalWar_IncendiaryBullet_SuccessMessage".Translate(this.launcher.Label, hitPawn.Label), 
                        MessageTypeDefOf.NeutralEvent);
                    Hediff burnOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Props.hediffToAdd);
                    //Generates an int for the severity of the burn
                    Random rnd = new Random();
                    int randomSeverity = rnd.Next(1, 4);
                    if (burnOnPawn != null)
                    {
                        burnOnPawn.Severity += randomSeverity;
                    }
                    else
                    {
                        Hediff hediff = HediffMaker.MakeHediff(Props.hediffToAdd, hitPawn);
                        hediff.Severity = randomSeverity;
                        hitPawn.health.AddHediff(hediff);
                    }
                }
                else
                {
                    MoteMaker.ThrowText(hitThing.PositionHeld.ToVector3(), hitThing.MapHeld, "TotalWar_IncendiaryBullet_FailureMote".Translate(Props.addHediffChance), 12f);
                }
            }
        }
    }
}
