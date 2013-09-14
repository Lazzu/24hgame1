using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hgame1.Weapons
{
    class Weapon
    {
        public Projectile Weaponprojectile { get; private set; }
        public int MaxAmmo { get; private set; }
        public int Ammo { get; private set; }

        public Weapon(int _maxammo, int _ammo, Projectile _projetile)
        {
            this.MaxAmmo = _maxammo;
            this.Ammo = _ammo;
            this.Weaponprojectile = _projetile;
        }
    }
}
