using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    [SerializeField]
    Bullet bulletPrefab;

    [SerializeField]
    Transform firePosition;

    [SerializeField]
    float fireDelay = 0.5f;
    float elapsedFireTime;
    bool canShoot = true;

    Factory bulletFactory;

    void Start()
    {
        bulletFactory = new Factory(bulletPrefab);
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
        usedBullet.Destroyed -= OnBulletDestroyed;
        bulletFactory.Restore(usedBullet);
    }
}
