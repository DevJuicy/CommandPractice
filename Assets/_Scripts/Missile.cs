using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Missile : RecycleObject
{
    BoxCollider2D box;
    Rigidbody2D body;

    [SerializeField]
    float moveSpeed = 3f;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();

        body.bodyType = RigidbodyType2D.Kinematic;
        box.isTrigger = true;
    }

    void Update()
    {
        if (!isActivated)
        {
            return;
        }

        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Building>()!=null)
        {
            DestroySelf();
            return;
        }

        if(collision.GetComponent<Explosion>()!=null)
        {
            DestroySelf();
            return;
        }
    }

    void DestroySelf()
    {
        isActivated = false;
        Destroyed?.Invoke(this);
    }
}
