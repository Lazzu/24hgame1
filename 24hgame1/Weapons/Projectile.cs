using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hgame1.Physics;

namespace hgame1.Weapons
{
    class Projectile : ISimplePhysics
    {
        private float physicstravelspeed;
        private float physicsdirection;

        public Projectile()
        {

        }
        #region ISimplephysics implementation
        public float travelspeed
        {
            get
            {
                return this.physicstravelspeed;
            }
            set
            {
                this.physicstravelspeed = travelspeed;
            }
        }

        public float traveldirection
        {
            get
            {
                return this.physicsdirection;
            }
            set
            {
                this.physicsdirection = traveldirection;
            }
        }
        #endregion
    }
}
