using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    BulletLauncher launcherPrefab;
    BulletLauncher launcher;

    void Start()
    {
        launcher = Instantiate(launcherPrefab);

        MouseGameController mouseGameController = gameObject.AddComponent<MouseGameController>();
        // MouseGameController는 MonoBehaviour 를 상속 받기 때문에 더이상 new 는 불가능
        mouseGameController.FireButtonPressed += launcher.OnFireButtonPressed;
    }
}
