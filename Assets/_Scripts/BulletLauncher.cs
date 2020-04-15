using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    [SerializeField]
    Bullet bulletPrefab;
    Bullet bullet;

    [SerializeField]
    Transform firePosition;
    public void OnFireButtonPressed(Vector3 position)
    {
        Debug.Log("Fired a bullet" + position);
        bullet = Instantiate(bulletPrefab);
        bullet.Activate(firePosition.position, position);
    }
}
