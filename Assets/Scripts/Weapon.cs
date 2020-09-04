using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Weapon
    {
        public string name;
        public float reloadTime;
        public float acuracy;
        public int currentAmmo;
        public int maxAmmo;
        public float damage;
        public float delayShoot;
        public float baseBulletSpeed;
        public string firingMode;
        public int index;

        public Weapon(string name, float reloadSpeed, float acuracy, int currentAmmo, int maxAmmo, float damage, float delayShoot, float baseBulletSpeed, string firingMode, int index)
        {
            this.name = name;
            this.reloadTime = reloadSpeed;
            this.acuracy = acuracy;
            this.currentAmmo = currentAmmo;
            this.maxAmmo = maxAmmo;
            this.damage = damage;
            this.delayShoot = delayShoot;
            this.baseBulletSpeed = baseBulletSpeed;
            this.firingMode = firingMode;
            this.index = index;
        }
    }
}
