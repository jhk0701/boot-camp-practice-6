### Q1. 19강 UI 만들기

**확인 문제 : 강의를 듣고, 강의 내용을 다시 점검하는 문제를 풀어봅시다.**

<aside>
🧠 UI의 앵커와 피벗에 대해서 세팅하는 19강의 강의 자료를 다시 확인해보시고, 
아래 퀴즈를 풀어보세요!

</aside>

**[🅾️❎퀴즈]**

- 앵커와 피벗은 같은 기능을 한다. (O/X) X
- 피벗을 왼쪽 상단으로 설정하면, UI 요소는 화면의 왼쪽 상단을 기준으로 위치가 고정된다. (O/X) X -> 앵커 설명임
- 피벗을 UI 요소의 중심에 설정하면, 회전 시 UI 요소가 중심을 기준으로 회전한다. (O/X)
O

**[🤔생각해보기]**

- 게임의 상단바와 같이 화면에 특정 영역에 꽉 차게 구성되는 UI와 
화면의 특정 영역에 특정한 크기로 등장하는 UI의 앵커 구성이 어떻게 다른 지 설명해보세요.
    * 상단바와 같이 화면에 특정 영역에 꽉 차게 구성되는 UI

        앵커는 캔버스의 모서리로 이동합니다.(지정한 Stretch의 유형에 따라 특정한 모서리에 몰려있을 수 있습니다.)

    * 특정 영역에 특정한 크기로 등장하는 UI

        이 UI에 별다른 앵커를 지정하지 않았다면 화면 정중앙에 앵커가 위치해있습니다.
    

- 돌아다니는 몬스터의 HP 바와 늘 고정되어있는 플레이어의 HP바는 Canvas 컴포넌트의 어떤 설정이 달라질 지 생각해보세요.
    * 화면에 늘 고정된 플레이어의 Canvas는 렌더모드가 ScreenSpace 계열로 설정하고
    움직이는 몬스터를 따라다니는 HP 바의 Canvas는 렌더모드를 WorldSpace로 설정하여 구현합니다.


### Q2. 20강 게임로직 수정

**확인 문제 : 강의를 듣고, 강의 내용을 다시 점검하는 문제를 풀어봅시다.**

**[🅾️❎퀴즈]**

- 코루틴은 비동기 작업을 처리하기 위해 사용된다. (O/X) : O
- yield return new WaitForSeconds(1);는 코루틴을 1초 동안 대기시킨다. (O/X) : O
- 코루틴은 void를 반환하는 메소드의 형태로 구현된다. (O/X) : x -> IEnumerator 인터페이스를 반환하는 메서드입니다.
코루틴을 실행하는 StartCoroutine은 이 IEnumerator를 받아 코루틴을 실행하고, 실행한 Coroutine을 반환합니다.

**[🤔생각해보기]**

- 코루틴을 이미 실행중이라면 추가로 실행하지 않으려면 어떻게 처리해주면 될까요?

    * 코루틴 실행 시, 반환하는 Coroutine을 받아 캐싱해두고, 새로운 코루틴이 실행되려할 때,
    캐싱한 Coroutine 변수가 null인지 확인하여 추가로 실행하는지 알 수 있습니다.
    null 이라면 코루틴을 실행하고, null이 아니라면 실행하지 않습니다.

    이때, 실행한 코루틴의 끝에 캐싱할 때 쓴 Coroutine 변수에 접근하여 null로 바꿔줘야합니다.

- 코루틴 실행 중 게임오브젝트가 파괴되더라도 코루틴의 실행이 정상적으로 지속될까요?
    * 코루틴을 실행하는 오브젝트가 파괴되면 코루틴 또한 사라집니다.
    
### Q3. 21-22강 스텟 강화 - 플레이어 강화 아이템 구현

**확인 문제 : 강의를 듣고, 내용을 다시 점검하는 문제를 풀어봅시다.**

**[🅾️❎퀴즈]**

1. 추상 클래스는 new를 통해 인스턴스화(instantiation)할 수 없다. (O/X) : O
2. 추상 클래스는 다른 클래스처럼 일반 메서드와 속성을 포함할 수 있다. (O/X) : O
3. 추상 클래스를 상속받은 클래스는 추상 클래스의 모든 추상 메서드를 구현해야 한다. (O/X) : O
4. C#에서 한 클래스는 여러 개의 추상 클래스를 상속받을 수 있다. (O/X) : X -> C#은 다중 상속을 지원하지 않기에 interface와 같은 방법으로 다중 상속의 흉내를 낼 수 있습니다.

**[🤔생각해보기]**

- 추상 클래스를 사용하지 않고 동일한 기능(강의에 적용된 기능)을 구현하려면 어떤 문제가 발생할 수 있는지 설명해보세요.

    * 문제 1. 상속 시, 호출 메서드
        * 만약 추상 클래스를 사용하지 않고 PickUpItem을 만든다면, OnTriggerEnter2D 호출 시 호출되는 메서드가 자식에서 오버라이드된 메서드가 아닌 자기자신의 메서드를 사용하게 됩니다.

        * 이렇게되면 본래 의도한 자식에서 구체적인 내용을 구현하겠다는 목적을 달성할 수 없게 되고, 본래 의도대로 자식에서 구체적인 내용을 사용하려면 캐릭터는
        개별적으로 구현되어있는 아이템들에 각각 검사하고 접근하여 OnPickedUp 메서드를 호출해야합니다.
