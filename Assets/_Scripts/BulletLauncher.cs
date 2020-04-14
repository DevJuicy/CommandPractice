using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    public void OnFireButtonPressed(Vector3 position)
    {
        Debug.Log("Fired a bullet" + position);
    }
}
