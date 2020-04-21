using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JUICY
{
    public class Missile : RecycleObject
    {
        [SerializeField]
        float moveSpeed = 5f;

        void Update()
        {
            if (!isActivate)
                return;

            transform.position += transform.up * moveSpeed * Time.deltaTime;

            if(IsArrivedToTarget())
            {
                Destroyed?.Invoke(this);
            }
        }


        bool IsArrivedToTarget()
        {
            float distance = Vector3.Distance(transform.position, targetPosition);
            return distance < 0.1f;
        }
    }
}