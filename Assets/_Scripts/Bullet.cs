using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : RecycleObject
{
    [SerializeField]
    float moveSpeed = 5f;

    Vector3 targetPosition;
    bool isActivated;

    public Action<Bullet> Destroyed;

    void Update()
    {
        if (!isActivated)
            return;

        transform.position += transform.up * moveSpeed * Time.deltaTime;

        if (IsArrivedToTarget())
        {
            isActivated = false;
            Destroyed?.Invoke(this);
        }
    }

    public void Activate(Vector3 startPosition, Vector3 targetPosition)
    {
        transform.position = startPosition;
        this.targetPosition = targetPosition;
        Vector3 dir = (targetPosition - startPosition).normalized;
        transform.rotation = Quaternion.LookRotation(transform.forward, dir);
        isActivated = true;
    }

    bool IsArrivedToTarget()
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        return distance < 0.1f;
    }
}
