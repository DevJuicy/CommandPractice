using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // 프로젝트에 필요한 주요 클래스들의 인스턴스들을 생성하고
    // 이들이 서로 커뮤니케이션 할 수 있도록 연동해주는 역할

    [SerializeField]
    BulletLauncher launcherPrefab;
    BulletLauncher launcher;

    void Start()
    {
        launcher = Instantiate(launcherPrefab);
        launcher.SetGamecontroller(new KeyGameController());
        //launcher.SetGamecontroller(new MouseGameController());
    }
}
