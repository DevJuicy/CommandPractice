using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    [SerializeField]
    Bullet bulletPrefab;

    [SerializeField]
    Explosion explosionPrefab;

    [SerializeField]
    Transform firePosition;

    [SerializeField]
    float fireDelay = 0.5f;
    float elapsedFireTime;
    bool canShoot = true;

    Factory bulletFactory;
    Factory explosionFactory;

    void Start()
    {
        bulletFactory = new Factory(bulletPrefab);
        explosionFactory = new Factory(explosionPrefab);
    }

    void Update()
    {
        if(!canShoot)
        {
            elapsedFireTime += Time.deltaTime;
            if(elapsedFireTime >= fireDelay)
            {
                canShoot = true;
                elapsedFireTime = 0f;
            }
        }
    }

    public void OnFireButtonPressed(Vector3 position)
    {
        if (!canShoot)
            return;
        ;
        Bullet bullet = bulletFactory.Get() as Bullet;
        bullet.Activate(firePosition.position, position);
        bullet.Destroyed += OnBulletDestroyed;

        canShoot = false;
    }

    void OnBulletDestroyed(Bullet usedBullet)
    {
        Vector3 lastBulletPosition = usedBullet.transform.position;
        usedBullet.Destroyed -= OnBulletDestroyed;
        bulletFactory.Restore(usedBullet);

        Explosion explosion = explosionFactory.Get() as Explosion;
        explosion.Activate(lastBulletPosition);
        explosion.Destroyed += OnExplosionDestroyed;
    }

    void OnExplosionDestroyed(Explosion usedExplosion)
    {
        usedExplosion.Destroyed -= OnExplosionDestroyed;
        explosionFactory.Restore(usedExplosion);
    }
}
