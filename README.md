## 1. 단일책임의원칙(single responsibility principle)

**SOLID**의 단일책임의원칙(single responsibility principle) 는 모든 클래스 또는 메소드는 단 하나의 분명한 역할 만을 책임져야 한다는것이다. 이 원리에 따라, BulletLauncher도 총알의 생성과 관리만 책임을 줘야한다.



간단한 키 입력장치를 만들어보자

```C#
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

***





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



***



## 3. 이벤트를 이용한 IGameController

이벤트를 사용한다는 것은 마치 라디오 방송국에서 전파를 날리는 것과 마찬가지. 라디오 방송국에서 신호를 보낼 대, 누가 그 방송을 듣는지는 고려하지 않음. 그냥 신호를 보낼 뿐



#### 이벤트 송신

위 예제에서 이벤트는 "총알 발사 버튼이 눌러졌다"와 같은 신호일것이고 이러한 이벤트를 발생시키는 쪽은 MouseGameController나 KeyGameController일것이다.

```C#
using UnityEngine;
using System.Collections;
using System; // 델리게이트를 사용하려면 System이 필요함

public class MouseGameController : IGameController // IGameController의 내부는 잠시 비워놓음
{
    public Action FireButtonPressed;
}
```

Action은 C#에서 제공하는 델리게이트이다. 



> 본문에 앞서 델리게이트 강의 (알면 넘어가던가 흥)
>
> 1. **delegate**
>
>    델리게이트는 함수에 대한 참조 '무엇을 대신한다', '대리한다' 라는 의미를 갖음. 델리게이트에 마치 체인처럼 함수들을 할당해놓고, 특정한 상황에서 해당 델리게이트를 호출함으로써 체인에 연결된 함수들을 동시에 호출 할 수 있음. 다음과 같이 여러 형식의 델리게이트를 만들 수 있다.
>
> 	```c#
> delegate void typeA();
> delegate void typeB(int);
> delegate float typeC(float);
> delegate string typeD(int);
> 	```
>
> 	```C#
> delegate int Operate(int a, int b);
> void Start()
> {
>     Operate operate;
>     operate = new Operate((a, b) => a + b);
>     Debug.Log(operate(3, 2));
> }
> 	```
>
> 	델리게이트는 콜백메소드로 사용할 수 도있다.
>
> 	```c#
> void Calculate(Operate oper) { }
>
>    void Start()
>    {   
>        Operate operate;
>        operate = new Operate((a, b) => a + b);
>        Calculate(operate);
>    }
>    ```
>    
> 2. **Func**
>
>    반환값이 있는 메소드를 참조하는 델리게이트 변수
>
>    ```C#
>    Func<float> func = ()=> 0.1f;
>    Func<int,float> func = (a) =>a*0.1f;
>    ```
>
>    <>맨뒷 타입이 반환 타입이다. 반환타입이 있어야 하기 때문에 반드시 <> 사용해야함
>
> 3. **Action**
>
>    반환값이 없는 메소드를 참조하는 델리게이트 변수
>
>    ```C3
>    Action act = ()=> Debug.Log("hello");
>    Action<string> act = (name) => Debug.Log(name);
>    ```
>
>    반환값이 없고 <> 안의 타입들은 모두 인자들이다.
>



이렇게 코드를 수정하면 마우스왼쪽버튼을 누르면 FireButtonPressed 델리게이트에 엮인 모든 함수들을 **대신** 실행해주게 된다.

```C#
using UnityEngine;
using System.Collections;
using System;

public class MouseGameController : MonoBehaviour, IGameController //Update()를 사용하기위해
{
    public Action FireButtonPressed;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            FireButtonPressed();
        }
    }
}
```



null체크로 이렇게 할 수 도 있지만

```C#
        if (Input.GetMouseButtonDown(0))
        {
            if (FireButtonPressed != null)
            {
                FireButtonPressed();
            }
        }
```



이렇게도 가능하다.

```C#
        if (Input.GetMouseButtonDown(0))
        {
            FireButtonPressed?.Invoke();
        }
```



#### 이벤트 수신

이벤트를 송신하는 함수는 만들어졌으니, 이제 이벤트를 수신하는 함수를 만들어보자

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    public void OnFireButtonPressed()
    {
        Debug.Log("Fired a bullet");
    }
}
```



#### 송신&수신 함수 연결

이벤트를 연동하는 방법은 두가지이다.

1. 수신자(BulletLauncher)의 내부에서 연결하는것
2. 수신자의 외부에서 연결하는것

2번의 방법을 사용할것인데 이 방법의 핵심은 MouseGameController나 BulletLauncher가 서로의 존재를 모르지만 커뮤니케이션 할 수 있다는 점이다. 그러기 위해선 중재자 역할의 클래스가 필요함(GameManager.cs)

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

        MouseGameController mouseGameController = gameObject.AddComponent<MouseGameController>();
        // MouseGameController는 MonoBehaviour 를 상속 받기 때문에 더이상 new 는 불가능
        mouseGameController.FireButtonPressed += launcher.OnFireButtonPressed;
    }
}
```

잘 연결이 되었다.



다시 BulletLauncher로 돌아와서 보자면

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    public void OnFireButtonPressed()
    {
        Debug.Log("Fired a bullet");
    }
}
```

MouseGameController와 관련된 항목들은 존재하지 않음. 어떤 게임 컨트롤러를 사용하는것과 상관없이 BulletLauncher는 그대로이다. 그저 외부에서 OnFireButtonPressed()를 전달시켜주기만 하면 된다.😁

물론 MouseGameController 에도 BulletLauncher와 관계된 것은 하나도 없다.

```C#
using UnityEngine;
using System.Collections;
using System;

public class MouseGameController : MonoBehaviour, IGameController
{
    public Action FireButtonPressed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireButtonPressed?.Invoke();
        }
    }
}
```



***



## 4. Object Pooling

쓸데없는 메모리 낭비를 막기 위해 유니티에서는 총알 같은 오브젝트들에 오브젝트 풀링기법을 이용해 오브젝트를 재사용한다.



#### 오브젝트 풀링으로 사용할 Factory 만들기

먼저 재사용할 오브젝트들을 추상화한 RecycleObject.cs를 만든다

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleObject : MonoBehaviour
{
    
}
```

이제 오브젝트 풀링에 사용할 총알같은 스크립트에 RecycleObject를 상속을 받으면 된다

```c#
public class Bullet : RecycleObject
```



재사용오브젝트들을 관리할 Factory

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory
{
    List<RecycleObject> pool = new List<RecycleObject>();
    int defaultPoolSize;
    RecycleObject prefab;

    public Factory(RecycleObject prefab, int defaultPoolSize = 5)
    {
        this.prefab = prefab;
        this.defaultPoolSize = defaultPoolSize;

        Debug.Assert(this.prefab != null, "Prefba is null");
    }
}
```

Factory는 MonoBehaviour를 상속받지 않기 때문에 RecycleObject를 의존하기 위해선 생성자를 이용하여 디펜던시 인젝션을 해주어야한다.



```c#
// Factory.cs
void CreatePool()
{
    for(int i =0; i < defaultPoolSize;i++)
    {
        RecycleObject obj = GameObject.Instantiate(prefab) as RecycleObject;
        obj.gameObject.SetActive(false);
        pool.Add(obj);
    }
}
```

총알(재사용할 오브젝트)를 만들어주고 담는 그릇(List< RecycleObject >) 속에 넣어준다.



그릇속의 총알을 가져오기 위한 Get()

```c#
// Factory.cs
public RecycleObject Get()
{
    if(pool.Count ==0)
    {
        CreatePool();
    }

    int lastIndex = pool.Count - 1;
    RecycleObject obj = pool[lastIndex];
    pool.RemoveAt(lastIndex);
    obj.gameObject.SetActive(true);
    return obj;
}
```

lastIndex를 사용하는 이유는 첫Index를 사용해서 꺼내면 List의 모든 오브젝트들이 앞으로 하나 씩 당겨지기 때문에 마지막 인덱스를 사용하는것이 조금이라도 더 효율적



마지막으로 사용이 완료된 총알을 다시 그릇으로 추가시키는 Restore()

```c#
public void Restore(RecycleObject obj)
{
    Debug.Assert(obj != null, "Null object to be returned!");
    obj.gameObject.SetActive(false);
    pool.Add(obj);
}
```



#### 총알 발사로직에 Factory를 추가시켜주기

```c#
Factory bulletFactory;

void Start()
{
    bulletFactory = new Factory(bulletPrefab);
}

public void OnFireButtonPressed(Vector3 position)
{
    Debug.Log("Fired a bullet" + position);
    bullet = bulletFactory.Get() as Bullet;
    bullet.Activate(firePosition.position, position);
}
```



#### 총알을 다시 반환하기

1. 총알이 발사 될 때(Activate()) isActivated를 true로 변경시켜주고
2. 총알이 해당 위치까지 도달했는지 반환하는 IsArrivedToTarget() 함수를 만들었다.

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : RecycleObject
{
    [SerializeField]
    float moveSpeed = 5f;

    Vector3 targetPosition;
    bool isActivated;
	
    // 총알이 파괴시 실행되는 Destoryed
    // 다시 Factory에 저장하기 위해 Bullet이 Factory를 알거나 BulletLuancher를
    // 알필요 없이 Bullet은 그냥 Destryed만 제때 실행시켜주면 된다.
    public Action<Bullet> Destroyed;

    void Update()
    {
        if (!isActivated)
            return;

        transform.position += transform.up * moveSpeed * Time.deltaTime;

        if (IsArrivedToTarget())
        {
            isActivated = false;
            Destroyed?.Invoke(this);
        }
    }

    public void Activate(Vector3 startPosition, Vector3 targetPosition)
    {
        transform.position = startPosition;
        this.targetPosition = targetPosition;
        Vector3 dir = (targetPosition - startPosition).normalized;
        transform.rotation = Quaternion.LookRotation(transform.forward, dir);
        isActivated = true;
    }

    bool IsArrivedToTarget()
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        return distance < 0.1f;
    }
}
```



```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    [SerializeField]
    Bullet bulletPrefab;

    [SerializeField]
    Transform firePosition;

    Factory bulletFactory;

    void Start()
    {
        bulletFactory = new Factory(bulletPrefab);
    }

    public void OnFireButtonPressed(Vector3 position)
    {
        Debug.Log("Fired a bullet" + position);
        Bullet bullet = bulletFactory.Get() as Bullet;
        bullet.Activate(firePosition.position, position);
   		// 총알파괴 할 때 Factory에 다시 저장을 해줘야 하기 때문에 연결시켜준다
        bullet.Destroyed += OnBulletDestroyed;
    }

    void OnBulletDestroyed(Bullet usedBullet)
    {
        usedBullet.Destroyed -= OnBulletDestroyed;
        bulletFactory.Restore(usedBullet);
    }
}
```

***



## 5. MonoBehaviour? 

프로젝트의 TimeManager.cs와 BuildingManager.cs 를 볼 수 있는데 TimeManager은 MonoBehaviour의 상속을 받고 BuildingManager은 받지 않는 것을 볼 수 있다. 왜 일까?



#### MonoBehaviour의 기능

유니티 도큐먼트의 MonoBehaviour을 참조해보면 ( https://docs.unity3d.com/kr/530/ScriptReference/MonoBehaviour.html ) 평소에 굉장히 많이 쓰는 함수들이 보인다.

> StartCoroutine, Awake, Update, Start, OnTriggerEnter 등등

또 MonoBehaviour을 상속 받은 오브젝트만 Unity Inspector 컴포넌트로써 사용할 수 있고, 대신 new 를 이용해서 생성할 수 없다.

하지만 기능이 많다는건 당연히 그만큼 무게가 나간다는 의미이기도 하다.



#### 결론

당연히 코딩에 정답은 없지만🙆‍♂️

클래스에서 유니티 내장함수를 사용하고자 한다면  MonoBehaviour을 상속받고 GetComponent나 AddComponent를 이용하여 인스턴스를 이용하고, 좀 더 일반적인 C# 프로그래밍을 하는 경우에는 보통의 C# 클래스를 만들고 new 키워드를 이용해서 인스턴스를 만드는것이 좋은 방법일듯 싶다.



## 6. AudioManager 

Dictionary에 ID에 매칭되는 음향을 넣기 위해 enum타입으로 선언된 SoundID

```c#
public enum SoundID
{
    Shoot, BulletExplosion, BuildingExplosion, GameEnd
}
```



```c#
[CreateAssetMenu]
public class AudioStorage : ScriptableObject
{
    [SerializeField]
    SoundSrc[] soundSrcs;

    Dictionary<SoundID, AudioClip> dicSounds = new Dictionary<SoundID, AudioClip>();

    void GenerateDictionary()
    {
        for(int i = 0; i<soundSrcs.Length;i++)
        {
            dicSounds.Add(soundSrcs[i].ID, soundSrcs[i].SoundFile);
        }
    }

    public AudioClip Get(SoundID ID)
    {
        if(dicSounds.Count == 0)
        {
            GenerateDictionary();
        }
        return dicSounds[ID];
    }
}
```

AudioStorage에는 다양한 효과음들을 넣은 ScriptableObject이다.

이 AudioStorage는 SerailizeField로 SoundSrc를 담고있고 SoundSrc는 Dictionary에서 사용할 SoundID와 실제 음향인 AudioClip이 담겨있다



```c#
[Serializable]
public struct SoundSrc
{
    [SerializeField]
    AudioClip soundFile;
    public AudioClip SoundFile
    {
        get
        {
            return soundFile;
        }
    }

    [SerializeField]
    SoundID soundID;
    public SoundID ID
    {
        get
        {
            return soundID;
        }
    }
}
```

SoundSrc를 Serializable하게 만드는 이유는 ScriptableObject의 값은 Serializable 해야 Inspector에서 사용하고, 또 저장이 가능하기 때문이다.



다른곳에서 사용하려면 먼저 싱글톤으로 선언된 AudioManager가 필요하다.

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    AudioStorage soundStorage;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void PlaySound(SoundID ID)
    {
        AudioSource.PlayClipAtPoint(soundStorage.Get(ID), Vector3.zero);
    }
}
```

그리고 소리가 나야할 부분에

```C#
AudioManager.instance.PlaySound(SoundID.BuildingExplosion);
```

이 코드를 추가시켜주면 정상적으로 작동되는것을 볼 수 있다.



***

## 마치며

마지막 AudioManager를 제외하면 다른 Manager들은 Singleton패턴을 사용하지않았다는게 신기하다.

C# 델리게이트 액션을 기존에도 사용을 할 줄은 알았지만 제대로 사용하지 못하고 있었던걸 느꼈다.

단일책임의원칙은 어찌보면 객체지향적으로 코딩하는 방법중 기본중의 기본이라 할 수 있는데 지금까지 책임을 다른곳에 잘 못 전가하고 있었던것 같기도하다. 예를들면 Building이 폭파될 때 굳이 소리나 다른 이벤트들을 알고 있을 필요가 없다. 그냥 Destoyed 액션만 실행시켜주면 Manager가 거기에 연관된 이벤트들을 실행시켜줄 뿐이다.

특히 Factory 클래스를 이용한 ObjectPool 기법이 좋았던것 같다.