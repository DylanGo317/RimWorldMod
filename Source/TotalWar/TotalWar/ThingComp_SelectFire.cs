using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TotalWar
{
    class ThingComp_SelectFire : ThingComp
    {
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
        }

        public ThingComp_SelectFireProperties Props => (ThingComp_SelectFireProperties)this.props;
    }
}
