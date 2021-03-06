using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using UnityEngine;

namespace TotalWar
{
    //Subclasses Verb_LaunchProjectile, overriding only TryCastShot()
    //Most code is the same, but it is easier to subclass in this case
    //than transpile using harmony. Additionally, it allows for only
    //some weapons to run this code, leaving vanilla weapons unchanged.
    class Verb_LaunchProjectileTW : VerbTW
    {
        protected override bool TryCastShot()
        {
            if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
            {
                return false;
            }
            //This line is moved up here so that the ThingComp
            //that tells whether or not the weapon can be fired
            //can be called before the projectile is checked
            Thing equipment = base.EquipmentSource;
            ThingComp_Training training = equipment.TryGetComp<ThingComp_Training>();
            if (training != null)
            {
                ThingComp_TrainingProperties trainingProps =
                    (ThingComp_TrainingProperties)equipment.TryGetComp<ThingComp_Training>().props;
                if (trainingProps != null)
                {
                    int currentTick = Find.TickManager.TicksGame;
                    if (currentTick - training.lastShotTick < training.ticksToShoot)
                    {
                        return false;
                    }
                    training.pawnName = caster.Label;
                    training.lastShotTick = currentTick;
                }
            }
            ThingComp_Reliability reliability = equipment.TryGetComp<ThingComp_Reliability>();
            //Checks that the equipment actually has the reliability comp
            if (reliability != null)
            {
                //Gets the properties for the comp
                ThingComp_ReliabilityProperties reliabilityProps = 
                    (ThingComp_ReliabilityProperties) equipment.TryGetComp<ThingComp_Reliability>().props;
                if (reliabilityProps != null)
                {
                    reliability.checkStatus();
                    if (!reliability.canShoot)
                    {
                        MoteMaker.ThrowText(this.caster.PositionHeld.ToVector3(), this.caster.MapHeld, "Jammed!", 0.5f);
                        return false;
                    }
                    //Sets values for success chance when it has not yet been initialized
                    if (reliability.weaponSuccess == 0f)
                    {
                        reliability.setReliability();
                    }
                    float successChance = reliability.weaponSuccess;
                    if (Rand.Value > successChance)
                    {
                        MoteMaker.ThrowText(this.caster.PositionHeld.ToVector3(), this.caster.MapHeld, "Jammed!", 0.5f);
                        reliability.resetTickSinceLastShot();
                        return false;
                    }
                } 
            }
            ThingDef projectile = this.Projectile;
            if (projectile == null)
            {
                return false;
            }
            ShootLine shootLine;
            bool flag = base.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out shootLine);
            if (this.verbProps.stopBurstWithoutLos && !flag)
            {
                return false;
            }
            if (base.EquipmentSource != null)
            {
                CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
                if (comp != null)
                {
                    comp.Notify_ProjectileLaunched();
                }
                CompReloadable comp2 = base.EquipmentSource.GetComp<CompReloadable>();
                if (comp2 != null)
                {
                    comp2.UsedOnce();
                }
            }
            this.lastShotTick = Find.TickManager.TicksGame;
            Thing thing = this.caster;
            CompMannable compMannable = this.caster.TryGetComp<CompMannable>();
            if (compMannable != null && compMannable.ManningPawn != null)
            {
                thing = compMannable.ManningPawn;
                equipment = this.caster;
            }
            Vector3 drawPos = this.caster.DrawPos;
            Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, shootLine.Source, this.caster.Map, WipeMode.Vanish);
            if (this.verbProps.ForcedMissRadius > 0.5f)
            {
                float num = this.verbProps.ForcedMissRadius;
                Pawn caster;
                if (thing != null && (caster = (thing as Pawn)) != null)
                {
                    num *= this.verbProps.GetForceMissFactorFor(equipment, caster);
                }
                float num2 = VerbUtility.CalculateAdjustedForcedMiss(num, this.currentTarget.Cell - this.caster.Position);
                if (num2 > 0.5f)
                {
                    int max = GenRadial.NumCellsInRadius(num2);
                    int num3 = Rand.Range(0, max);
                    if (num3 > 0)
                    {
                        IntVec3 c = this.currentTarget.Cell + GenRadial.RadialPattern[num3];
                        this.ThrowDebugText("ToRadius");
                        this.ThrowDebugText("Rad\nDest", c);
                        ProjectileHitFlags projectileHitFlags = ProjectileHitFlags.NonTargetWorld;
                        if (Rand.Chance(0.5f))
                        {
                            projectileHitFlags = ProjectileHitFlags.All;
                        }
                        if (!this.canHitNonTargetPawnsNow)
                        {
                            projectileHitFlags &= ~ProjectileHitFlags.NonTargetPawns;
                        }
                        projectile2.Launch(thing, drawPos, c, this.currentTarget, projectileHitFlags, this.preventFriendlyFire, equipment, null);
                        return true;
                    }
                }
            }
            //This appears to be where miss chance is calculated
            ShotReport shotReport = ShotReport.HitReportFor(this.caster, this, this.currentTarget);
            //Values for chance to miss or hit cover that will not change the weapon
            //does not have the select fire feature
            float newAimOnTarget = shotReport.AimOnTargetChance_IgnoringPosture;
            float newPassCoverChance = shotReport.PassCoverChance;
            ThingComp_SelectFire selectFire = equipment.TryGetComp<ThingComp_SelectFire>();
            if (selectFire != null)
            {
                ThingComp_SelectFireProperties selectFireProps =
                    (ThingComp_SelectFireProperties)equipment.TryGetComp<ThingComp_SelectFire>().props;
                if (selectFireProps != null)
                {
                    if (selectFire.currentMode == 2)
                    {
                        newAimOnTarget -= newAimOnTarget * selectFireProps.autoPenalty;
                        newPassCoverChance -= newPassCoverChance * selectFireProps.autoPenalty;
                    } else if (selectFire.currentMode == 1)
                    {
                        newAimOnTarget -= newAimOnTarget * selectFireProps.burstPenalty;
                        newPassCoverChance -= newPassCoverChance * selectFireProps.burstPenalty;
                    }
                }
            }
            Thing randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
            ThingDef targetCoverDef = (randomCoverToMissInto != null) ? randomCoverToMissInto.def : null;
            if (!Rand.Chance(newAimOnTarget))
            {
                shootLine.ChangeDestToMissWild(shotReport.AimOnTargetChance_StandardTarget);
                this.ThrowDebugText("ToWild" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
                this.ThrowDebugText("Wild\nDest", shootLine.Dest);
                ProjectileHitFlags projectileHitFlags2 = ProjectileHitFlags.NonTargetWorld;
                if (Rand.Chance(0.5f) && this.canHitNonTargetPawnsNow)
                {
                    projectileHitFlags2 |= ProjectileHitFlags.NonTargetPawns;
                }
                projectile2.Launch(thing, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags2, this.preventFriendlyFire, equipment, targetCoverDef);
                return true;
            }
            //Chance to hit cover
            if (this.currentTarget.Thing != null && this.currentTarget.Thing.def.category == ThingCategory.Pawn && !Rand.Chance(newPassCoverChance))
            {
                this.ThrowDebugText("ToCover" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
                this.ThrowDebugText("Cover\nDest", randomCoverToMissInto.Position);
                ProjectileHitFlags projectileHitFlags3 = ProjectileHitFlags.NonTargetWorld;
                if (this.canHitNonTargetPawnsNow)
                {
                    projectileHitFlags3 |= ProjectileHitFlags.NonTargetPawns;
                }
                projectile2.Launch(thing, drawPos, randomCoverToMissInto, this.currentTarget, projectileHitFlags3, this.preventFriendlyFire, equipment, targetCoverDef);
                return true;
            }
            ProjectileHitFlags projectileHitFlags4 = ProjectileHitFlags.IntendedTarget;
            if (this.canHitNonTargetPawnsNow)
            {
                projectileHitFlags4 |= ProjectileHitFlags.NonTargetPawns;
            }
            if (!this.currentTarget.HasThing || this.currentTarget.Thing.def.Fillage == FillCategory.Full)
            {
                projectileHitFlags4 |= ProjectileHitFlags.NonTargetWorld;
            }
            this.ThrowDebugText("ToHit" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
            //Hits target
            if (this.currentTarget.Thing != null)
            {
                projectile2.Launch(thing, drawPos, this.currentTarget, this.currentTarget, projectileHitFlags4, this.preventFriendlyFire, equipment, targetCoverDef);
                this.ThrowDebugText("Hit\nDest", this.currentTarget.Cell);
            }
            //Exceptional case
            else
            {
                projectile2.Launch(thing, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags4, this.preventFriendlyFire, equipment, targetCoverDef);
                this.ThrowDebugText("Hit\nDest", shootLine.Dest);
            }
            return true;
        }

        //Two private methods that are called in the overridden method
        private void ThrowDebugText(string text, IntVec3 c)
        {
            if (DebugViewSettings.drawShooting)
            {
                MoteMaker.ThrowText(c.ToVector3Shifted(), this.caster.Map, text, -1f);
            }
        }
        private void ThrowDebugText(string text)
        {
            if (DebugViewSettings.drawShooting)
            {
                MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, text, -1f);
            }
        }

        //Other methods in the vanilla version of the file that now must be
        //copied over in order to subclass the modified version of
        //Verb
        public override void WarmupComplete()
        {
            base.WarmupComplete();
            Find.BattleLog.Add(new BattleLogEntry_RangedFire(this.caster, this.currentTarget.HasThing ? this.currentTarget.Thing : null, (base.EquipmentSource != null) ? base.EquipmentSource.def : null, this.Projectile, this.ShotsPerBurst > 1));
        }

        public virtual ThingDef Projectile
        {
            get
            {
                if (base.EquipmentSource != null)
                {
                    CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
                    if (comp != null && comp.Loaded)
                    {
                        return comp.Projectile;
                    }
                }
                return this.verbProps.defaultProjectile;
            }
        }

        public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
        {
            needLOSToCenter = true;
            ThingDef projectile = this.Projectile;
            if (projectile == null)
            {
                return 0f;
            }
            return projectile.projectile.explosionRadius;
        }

        public override bool Available()
        {
            if (!base.Available())
            {
                return false;
            }
            if (this.CasterIsPawn)
            {
                Pawn casterPawn = this.CasterPawn;
                if (casterPawn.Faction != Faction.OfPlayer && casterPawn.mindState.MeleeThreatStillThreat && casterPawn.mindState.meleeThreat.Position.AdjacentTo8WayOrInside(casterPawn.Position))
                {
                    return false;
                }
            }
            return this.Projectile != null;
        }
    }
}
