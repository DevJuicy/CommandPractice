## 1. 단일책임의원칙(single responsibility principle)

**SOLID**의 단일책임의원칙(single responsibility principle) 는 모든 클래스 또는 메소드는 단 하나의 분명한 역할 만을 책임져야 한다는것이다. 이 원리에 따라, BulletLauncher도 총알의 생성과 관리만 책임을 줘야한다.



간단한 키 입력장치를 만들어보자

```c#
public class BulletLauncher : MonoBehaviour
{
	void Update()
	{
    	if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) ||)
   	 	{
     	  	Debug.Log("Fired a bullet!"); 
    	}
	}
}
```
이와 같이 코딩하면 새로운 컨트롤러를 추가할 때 마다 일일이 코드를 수정해야함



그렇다면 인터페이스를 사용해야지

```c#
public interface IGameController
{
    bool FireButtonPressed();
}

public class MouseGameController : IGameController
{
    public bool FireButtonPressed()
    {
        return Input.GetMouseButtonDown(0);
    }
}

public class KeyGameController : IGameController
{
    public bool FireButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
```



인터페이스를 이용해서 컨트롤러별로 나누었다. 이제 다시 BulletLauncher.cs에서 이 인터페이스들을 받는 코드를 구현해보자면

```c#
// BulletLauncher.cs
IGameController controller;
void Start()
{
    controller = new MouseGameController();
    //controller = new KeyGameController();
}
```

역시 이런 방식이라면 게임컨트롤러를 변경할 때 마다 새로 타이핑해야하므로 PASS
이렇게 내부에서 게임 컨트롤러 클래스의 인스턴스를 만들기 보다는 외부에서 만들어진 인스턴스를
그냥 전달받는 식으로 하는것이 좋음



외부에서 전달받기 위한 SetGameController()

```c#
// BulletLauncher.cs
IGameController controller;
public void SetGamecontroller(IGameController controller)
{
    this.controller = controller;
}
```



그리고 GameManager.cs를 만듦

GameManager의 역할은

1. 프로젝트에 필요한 주요 클래스들의 인스턴스들을 생성하고
2.  이들이 서로 커뮤니케이션 할 수 있도록 연동

```c#
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
        launcher.SetGamecontroller(new KeyGameController());
        //launcher.SetGamecontroller(new MouseGameController());
    }
}

```

```C#
// BulletLauncher.cs
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
```



## 2. 디펜던시 인젝션

#### 디펜던시(Dependency)란?

class A,B가 있을 때, A가 어떤 일을 하기 위해 클래스 B의 인스턴스를 필요로 한다면, B를 A의 디펜던시라고 말함. 즉 A가 B에 의존하고 있다라는 뜻임. 유니티에서는 MonoBehaviour를 이용한 컴포넌트 구조라 GetComponent를 이용하면 손쉽게 의존할 수 있다. 그런데 하나의 오브젝트내에 여러 인스턴스가 있을 때는 아주 훌륭하게 작동하지만 서로 다른 오브젝트들이 연결되어 있은 경우는 그렇지 않음. (또 MonoBehaviour의 파생 클래스가 아닌 보통 C#클래스로 디펜던시가 만들어졌을 경우에도)

이런 경우 GameObject.Find() 나 GameObject.FindWithTag()를 사용하거나 싱글톤을 이용하는 방법이 있는데 프로젝트 규모가 커지고 개발자가 많아지면 점점 복잡한 문제가 생기게 됨.



#### 디펜던시 인젝션의 기본원리

> (injection 이라는 단어를 찾아 보면 ‘주입’이나 '주사')

```c#
// BulletLauncher.cs
IGameController controller;
public void SetGamecontroller(IGameController controller)
{
    this.controller = controller;
}
```

controller를 사용하기 위해 IGameController타입의 디펜던시를 가지고 있음.

그런데 디펜던시를 주입하는 행위가 강제로 진행되는 것이 아니기 때문에, 혹시나 프로그래머가 함수 호출을 잊어버린다면? 당연히 예외처리를 해주었기 때문에 수정할 수가 있다.

```c#
// BulletLauncher.cs
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
```

디펜던시 인젝션의 정석은 SetGameController() 처럼 별도의 함수에서 주입하는게 아닌 생성자에서 강제로 주입하는 방법이다.



그렇게 하려면 GamaManager.cs에서 인스턴스를 생성하면서 생성자를 호출하는게 맞지만

```C#
// Gamemanager.cs
void Start()
{
    launcher = new BulletLauncher(new KeyGameController()); 
    // 당연하겠지만 실행 X
}
```

유니티의 MonoBehaviour를 상속받는 클래스들은 new를 사용해서 만들 수 없다.



#### 디펜던시 인젝션이 꼭 필요해? 언제 사용해?

반드시 필요한건 아님. 유니티는 기본적으로 GetComponent를 이용해서 디펜던시를 사용하는게 일반적임.

사용할 만한 경우는

1. 하나의 인터페이스로부터 파생된 다수의 클래스 인스턴스들을 손쉽게 교체할 때
2. 느슨한 커플링과 유연하고 확장성 있는 프로그래밍 구현하고자 할 때