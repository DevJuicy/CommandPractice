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

        [SerializeField]
        RecycleObject missilePrefab;

        void Start()
        {
            missileFactory = new Factory(missilePrefab, 5);
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
            missileFactory.Restore(missile);
            missile.Destroyed -= OnMissileDestroyed;
        }
    }
}