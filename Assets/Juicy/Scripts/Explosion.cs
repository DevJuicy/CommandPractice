using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JUICY
{
    public class Explosion : RecycleObject
    {
        [SerializeField]
        float delayTime = 3f;
        float elapseTime;
        void Update()
        {
            if (isActivate)
            {
                if (elapseTime < delayTime)
                {
                    elapseTime += Time.deltaTime;
                }
                else
                {
                    elapseTime = 0;
                    isActivate = false;
                    Destroyed?.Invoke(this);
                }
            }
        }
    }
}