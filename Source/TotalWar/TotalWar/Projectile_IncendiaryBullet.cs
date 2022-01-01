using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace TotalWar
{
    //Projectile that performs the function of the base bullet in accordance with
    //the data provided in the def as well as adding a fire attachment to the hit thing
    //Change thingClass to this class in xml to make the projectile incendiary
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
                    //Generates a float for the size of the fire
                    float fireSize = Rand.Range(0.1f, 0.3f);
                    FireUtility.TryAttachFire(hitThing, fireSize);
                }
            }
        }
    }
}
