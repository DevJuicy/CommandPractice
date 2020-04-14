using UnityEngine;
using System.Collections;

public class MouseGameController : IGameController
{
    public bool FireButtonPressed()
    {
        return Input.GetMouseButtonDown(0);
    }
}
