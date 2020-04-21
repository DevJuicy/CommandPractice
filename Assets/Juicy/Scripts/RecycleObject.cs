using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JUICY
{
    public class RecycleObject : MonoBehaviour
    {
        public Action<RecycleObject> Destroyed;
        protected bool isActivate;
        protected Vector3 targetPosition;

        public virtual void Activate(Vector3 position)
        {
            isActivate = true;
            transform.position = position;
        }

        public virtual void Activate(Vector3 startPosition, Vector3 targetPosition)
        {
            transform.position = startPosition;
            this.targetPosition = targetPosition;
            Vector3 dir = (targetPosition - startPosition).normalized;
            transform.rotation = Quaternion.LookRotation(transform.forward, dir);
            isActivate = true;
        }
    }
}