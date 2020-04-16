using UnityEngine;
using System;

public class RecycleObject : MonoBehaviour
{
    protected bool isActivated;
    protected Vector3 targetPosition;

    public Action<RecycleObject> Destroyed;

    public virtual void Activate(Vector3 position)
    {
        isActivated = true;
        transform.position = position;
    }

    public virtual void Activate(Vector3 startPosition, Vector3 targetPosition)
    {
        transform.position = startPosition;
        this.targetPosition = targetPosition;
        Vector3 dir = (targetPosition - startPosition).normalized;
        transform.rotation = Quaternion.LookRotation(transform.forward, dir);
        isActivated = true;
    }
}
