using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Missile : RecycleObject
{
    BoxCollider2D box;
    Rigidbody2D body;

    [SerializeField]
    float moveSpeed = 3f;

    float bottomY;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();

        body.bodyType = RigidbodyType2D.Kinematic;
        box.isTrigger = true;
    }

    void Start()
    {
        Vector3 bottomPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        bottomY = bottomPosition.y - box.size.y;
    }

    void Update()
    {
        if (!isActivated)
        {
            return;
        }
        transform.position += transform.up * moveSpeed * Time.deltaTime;
        CheckOutOfScreen();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Building>() != null)
        {
            DestroySelf();
            return;
        }

        if (collision.GetComponent<Explosion>() != null)
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

    void CheckOutOfScreen()
    {
        if (transform.position.y < bottomY)
        {
            isActivated = false;
            OutOfScreen?.Invoke(this);
        }
    }
}
