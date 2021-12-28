using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;


//Codes for a potential field that can be added to a def to inflict burn damage
namespace TotalWar
{
    public class ModExtension_IncendiaryBullet : DefModExtension
    {
        public float addBurnChance = 0.9f;
    }
}
