using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CircleCollider2D),typeof(Rigidbody2D))]
public class Explosion : RecycleObject
{
    CircleCollider2D circle;
    Rigidbody2D body;

    [SerializeField]
    float timeToRemove = 1f;
    float elapsedTime = 0f;
    bool isActivated;

    public Action<Explosion> Destroyed;

    void Awake()
    {
        circle = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();

        circle.isTrigger = true;
        body.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if(isActivated)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= timeToRemove)
            {
                elapsedTime = 0f;
                DestroySelf();
            }
        }
    }

    public void Activate(Vector3 position)
    {
        isActivated = true;
        transform.position = position;
    }

    void DestroySelf()
    {
        isActivated = false;
        Destroyed?.Invoke(this);
    }
}
