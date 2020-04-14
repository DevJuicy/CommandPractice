using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    // #1
    // 이와 같이 코딩하면 새로운 컨트롤러를 추가할 때 마다 일일이 코드를 수정해야함
    // 그렇다면 인터페이스를 사용해야지

    //void Update()
    //{
    //    if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) ||)
    //    {
    //       Debug.Log("Fired a bullet!"); 
    //    }
    //}


    // #2
    // 역시 이런 방식이라면 게임컨트롤러를 변경할 때 마다 새로 타이핑해야하므로 PASS
    // 이렇게 내부에서 게임 컨트롤러 클래스의 인스턴스를 만들기 보다는 외부에서 만들어진 인스턴스를
    // 그냥 전달받는 식으로 하는것이 좋음
    //IGameController controller;

    //void Start()
    //{
    //    //controller = new MouseGameController();
    //    //controller = new KeyGameController();
    //}


    IGameController controller;

    public void SetGamecontroller(IGameController controller)
    {
        this.controller = controller;
    }

    void Update()
    {
        if (controller != null)
        {
            if (controller.FireButtonPressed())
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
