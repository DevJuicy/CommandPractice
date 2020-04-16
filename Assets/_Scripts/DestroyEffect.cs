using UnityEngine;
using System.Collections;

public class DestroyEffect : RecycleObject
{
    [SerializeField]
    float timeToRemove = 1f;
    float elapsedTime = 0f;

    void Update()
    {
        if (isActivated)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeToRemove)
            {
                elapsedTime = 0f;
                DestroySelf();
            }
        }
    }

    void DestroySelf()
    {
        isActivated = false;
        Destroyed?.Invoke(this);
    }
}
