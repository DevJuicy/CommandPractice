using UnityEngine;
using System.Collections;

public class KeyGameController : IGameController
{
    public bool FireButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
