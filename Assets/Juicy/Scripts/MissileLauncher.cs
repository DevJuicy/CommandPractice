using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JUICY
{
    public class MissileLauncher : MonoBehaviour
    {
        [SerializeField]
        float fireDelay = 1.5f;
        float elapsedFireTime;
        bool canFire;

        Factory missileFactory;
        Factory explosionFactory;

        [SerializeField]
        RecycleObject missilePrefab;
        [SerializeField]
        RecycleObject explosionPrefab;

        void Start()
        {
            missileFactory = new Factory(missilePrefab);
            explosionFactory = new Factory(explosionPrefab);
        }

        void Update()
        {
            if (!canFire)
            {
                elapsedFireTime += Time.deltaTime;
                if (elapsedFireTime >= fireDelay)
                {
                    canFire = true;
                    elapsedFireTime = 0;
                }
            }
        }

        public void Shoot(Vector3 position)
        {
            if (!canFire)
            {
                return;
            }

            canFire = false;
            var obj = missileFactory.Get();
            obj.Activate(transform.position, position);
            obj.Destroyed += OnMissileDestroyed;
        }

        void OnMissileDestroyed(RecycleObject missile)
        {
            var obj = explosionFactory.Get();
            obj.Activate(missile.transform.position);
            obj.Destroyed += OnExplosionDestryed;

            missileFactory.Restore(missile);
            missile.Destroyed -= OnMissileDestroyed;
        }

        void OnExplosionDestryed(RecycleObject explosion)
        {
            explosionFactory.Restore(explosion);
            explosion.Destroyed -= OnExplosionDestryed;
        }
    }
}