using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace TotalWar
{
    //Codes for the chance of the fire attachment being added to the target upon a successful shot
    class Projectile_IncendiaryBullet : Bullet
    {
        public ModExtension_IncendiaryBullet Props => base.def.GetModExtension<ModExtension_IncendiaryBullet>();

        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
            //Make sure the hit thing is not null and that props isn't null
            if (Props != null && hitThing != null)
            {
                //Chance for the fire attachment to be applied to the object
                float rand = Rand.Value;
                //Do not really need to check here if fire can be attached to the hit thing because
                //this is already handled by source code
                if (rand <= Props.addBurnChance)
                {
                    Messages.Message("TotalWar_IncendiaryBullet_SuccessMessage".Translate(this.launcher.Label, hitThing.Label), 
                        MessageTypeDefOf.NeutralEvent);
                    //Generates a float for the size of the fire
                    float fireSize = Rand.Range(0.1f, 0.3f);
                    FireUtility.TryAttachFire(hitThing, fireSize);
                }
                else
                {
                    MoteMaker.ThrowText(hitThing.PositionHeld.ToVector3(), hitThing.MapHeld, "TotalWar_IncendiaryBullet_FailureMote".Translate(Props.addBurnChance), 12f);
                }
            }
        }
    }
}
