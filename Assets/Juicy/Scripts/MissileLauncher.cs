using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JUICY
{
    public class MissileLauncher : MonoBehaviour
    {
        IController gameController;

        public void SetGameController(IController gameController)
        {
            this.gameController = gameController;
        }

        void Update()
        {
            if (gameController != null)
            {
                if (gameController.PressAttackKey())
                {
                    Debug.Log("Fired a bullet!");
                }
            }
            else
            {
                Debug.LogError("controller is null");
            }
        }
    }
}