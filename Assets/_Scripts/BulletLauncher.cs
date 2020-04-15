using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    [SerializeField]
    Bullet bulletPrefab;

    [SerializeField]
    Transform firePosition;

    Factory bulletFactory;

    void Start()
    {
        bulletFactory = new Factory(bulletPrefab);
    }

    public void OnFireButtonPressed(Vector3 position)
    {
        Debug.Log("Fired a bullet" + position);
        Bullet bullet = bulletFactory.Get() as Bullet;
        bullet.Activate(firePosition.position, position);
        bullet.Destroyed += OnBulletDestroyed;
    }

    void OnBulletDestroyed(Bullet usedBullet)
    {
        usedBullet.Destroyed -= OnBulletDestroyed;
        bulletFactory.Restore(usedBullet);
    }
}
