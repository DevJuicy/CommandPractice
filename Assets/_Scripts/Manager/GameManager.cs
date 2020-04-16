using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    BulletLauncher launcherPrefab;
    BulletLauncher launcher;

    [SerializeField]
    Transform launcherLocator;

    [SerializeField]
    Building buildingPrefab;

    [SerializeField]
    Transform[] buildingLocators;

    MouseGameController mouseGameController;
    BuildingManager buildingManager;
    TimeManager timeManager;

    void Start()
    {
        launcher = Instantiate(launcherPrefab);
        launcher.transform.position = launcherLocator.position;

         mouseGameController = gameObject.AddComponent<MouseGameController>();

        buildingManager = new BuildingManager(buildingPrefab, buildingLocators);
        timeManager = gameObject.AddComponent<TimeManager>();
        BindEvents();
        timeManager.StartGame(1f);
    }

    void OnDestroy()
    {
        UnBindEvents();
    }

    void BindEvents()
    {
        mouseGameController.FireButtonPressed += launcher.OnFireButtonPressed;
        timeManager.GameStarted += buildingManager.OnGameStarted;
        timeManager.GameStarted += launcher.OnGameStarted;
    }

    void UnBindEvents()
    {
        mouseGameController.FireButtonPressed -= launcher.OnFireButtonPressed;
        timeManager.GameStarted -= buildingManager.OnGameStarted;
        timeManager.GameStarted -= launcher.OnGameStarted;
    }
}
