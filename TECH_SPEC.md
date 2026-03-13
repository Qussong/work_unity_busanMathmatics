# 📐 부산 수학문화관 인터랙티브 교육 애플리케이션 — 기술 명세서

> **프로젝트명:** work_unity_busanMathmatics
> **용도:** 부산 수학문화관 터치 키오스크 전시 콘텐츠
> **작성일:** 2026-03-13
> **버전:** 1.0

---

## 📚 목차

- [1. 프로젝트 개요](#1-프로젝트-개요)
- [2. 기술 스택](#2-기술-스택)
- [3. 시스템 아키텍처](#3-시스템-아키텍처)
- [4. 핵심 설계 패턴](#4-핵심-설계-패턴)
- [5. 모듈별 클래스 명세](#5-모듈별-클래스-명세)
- [6. 데이터 흐름](#6-데이터-흐름)
- [7. 화면/기능 전환 흐름](#7-화면기능-전환-흐름)
- [8. 라이프사이클](#8-라이프사이클)
- [9. 확장 가이드](#9-확장-가이드)
- [10. 설계 결정 및 트레이드오프](#10-설계-결정-및-트레이드오프)

---

## 📌 1. 프로젝트 개요

### 🎯 배경 및 목적

부산 수학문화관에 설치되는 터치 키오스크 전시물로, 이집트·중국·로마 3개 문명의 역사적 숫자 체계를 영상, 게임, 드로잉 활동을 통해 체험 학습하도록 설계된 인터랙티브 교육 애플리케이션이다.

### 🧩 핵심 기능

| 기능 | 설명 |
|------|------|
| 인트로 영상 | 국가 선택 전 소개 영상 재생 (진행바 드래그/스킵 지원) |
| 교육 영상 | 이집트/중국/로마 각 국가별 숫자 체계 교육 영상 |
| 숫자 변환 퀴즈 | 랜덤 숫자를 해당 문명의 숫자 체계로 변환하는 퀴즈 (넘패드 입력) |
| 카드 매칭 게임 | 6쌍(12장) 카드를 60초 내 매칭하는 기억력 게임 |
| 투표 | 가장 마음에 드는 숫자 체계에 투표 (JSON 영속화) |
| 생일 적기 | 년/월/일을 선택하여 해당 문명의 숫자로 변환된 미리보기 확인 |
| 드로잉 | 선택된 날짜의 문명 숫자 이미지를 반투명 오버레이로 보며 터치 드로잉 |
| 투표 결과 | 누적 투표 결과를 순위/비율/바 차트로 표시 |
| 자동 홈 복귀 | 60초 무입력 시 홈 화면으로 자동 복귀 |

### 🖥 타겟 환경

| 항목 | 내용 |
|------|------|
| 플랫폼 | Windows (키오스크 전용) |
| 입력 방식 | 터치 디스플레이 (마우스/키보드 호환) |
| 해상도 | 키오스크 고정 해상도 (Full Screen) |
| 데이터 저장 | `Application.persistentDataPath/vote_data.json` (투표 데이터) |
| 영상 파일 | `StreamingAssets/` 폴더 내 비디오 파일 |

---

## 🧰 2. 기술 스택

| 분류 | 항목 |
|------|------|
| 게임 엔진 | Unity 6 (URP) |
| 언어 | C# |
| UI 시스템 | Unity UI (uGUI) — Button, Image, Slider, TMP_Text, RawImage, ScrollRect |
| 영상 재생 | UnityEngine.Video.VideoPlayer + RenderTexture |
| 애니메이션 | DOTween (DG.Tweening) — 카드 플립(Y축 회전), 버튼 페이드(CanvasGroup) |
| 드로잉 | LS.DrawTexture.Runtime.DrawTextureUI (외부 에셋) |
| 데이터 포맷 | JSON (JsonUtility), ScriptableObject (카드DB, 숫자↔스프라이트 매핑) |
| 이벤트 시스템 | System.Action 델리게이트, UnityEvent (Button.onClick) |
| 텍스트 렌더링 | TextMeshPro (TMPro) |

---

## 🏗 3. 시스템 아키텍처

### 🧱 레이어 구조

```
┌─────────────────────────────────────────────────────┐
│                    Unity Engine                       │
│  (MonoBehaviour Lifecycle, Input, VideoPlayer, UI)    │
└──────────────────────┬──────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────┐
│              Controllers Layer                        │
│  NavigationController (싱글톤, FSM 오케스트레이터)      │
│  CardFlip (DOTween 카드 애니메이션)                    │
└──────┬───────────────────────────────┬──────────────┘
       │                               │
       ▼                               ▼
┌──────────────┐              ┌────────────────┐
│   FSM Layer  │              │  Managers Layer │
│  StateMachine│◄────────────►│  (싱글톤들)      │
│  IState      │   호출/이벤트  │  NumGameManager │
│  BaseState   │              │  CardGameManager│
│  11개 State  │              │  VideoManager   │
└──────┬───────┘              │  VoteManager    │
       │                      │  SoundManager   │
       ▼                      │  SliderManager  │
┌──────────────┐              │  IdleManager    │
│  Views Layer │              └────────────────┘
│  BaseView    │
│  11개 View   │◄─── Action 이벤트 ───── State
└──────┬───────┘
       │
       ▼
┌──────────────┐
│ Models Layer │
│  ECountry    │
│  CardData    │
│  ScriptableObjects │
└──────────────┘
```

### 📁 디렉토리 구조

```
Assets/Scripts/BusanMath/
├── Core/
│   └── MonoSingleton.cs            # 싱글톤 베이스 클래스
├── FSM/
│   ├── IState.cs                   # 상태 인터페이스 (5단계 생명주기)
│   ├── BaseState.cs                # 상태 추상 클래스 (제네릭)
│   ├── StateMachine.cs             # 상태 머신 (Dictionary + HashSet)
│   └── States/
│       ├── HomeState.cs            # 홈 화면
│       ├── SelectState.cs          # 국가 선택 + 인트로 영상
│       ├── VideoState.cs           # 교육 영상 재생
│       ├── NumGameDescriptionState.cs  # 숫자게임 설명
│       ├── NumGameState.cs         # 숫자 변환 퀴즈
│       ├── CardGameDescriptionState.cs # 카드게임 설명
│       ├── CardGameState.cs        # 60초 카드 매칭 게임
│       ├── VoteState.cs            # 투표
│       ├── WriteState.cs           # 생일 적기 (날짜 선택 UI)
│       ├── DrawingState.cs         # 드로잉 (국가별 숫자 이미지 미리보기)
│       └── VoteResultState.cs      # 투표 결과
├── Controllers/
│   ├── NavigationController.cs     # FSM 관리, 화면 전환 라우팅 (싱글톤)
│   └── CardFlip.cs                 # DOTween 카드 플립 애니메이션
├── Managers/
│   ├── NumGameManager.cs           # 숫자게임 로직
│   ├── CardGameManager.cs          # 카드게임 로직
│   ├── SoundManager.cs             # 효과음 재생
│   ├── VideoManager.cs             # 비디오 재생/정지
│   ├── SliderManager.cs            # 비디오 진행바 동기화
│   ├── VoteManager.cs              # 투표 데이터 영속화 (JSON)
│   ├── IdleManager.cs              # 60초 무입력 감지 → 홈 복귀
│   └── NumPad.cs                   # 숫자 버튼 핸들러
├── Models/
│   ├── ECountry.cs                 # 국가 열거형
│   ├── CardDatabaseSO.cs           # 카드 데이터 ScriptableObject
│   └── StringSpritePairContainerSO.cs  # 숫자↔스프라이트 매핑
└── Views/
    ├── BaseView.cs                 # View 추상 클래스
    ├── HomeView.cs                 # ~ VoteResultView.cs (11개)
    └── ...

Assets/Scripts/SwipeUI/
├── SwipeUI.cs                     # 스와이프 UI 컴포넌트
└── HoverDetector.cs               # 마우스/터치 호버 감지
```

### 네임스페이스 체계

```
BusanMath
├── Core              # MonoSingleton<T>
├── FSM               # IState, BaseState, StateMachine
│   └── States        # 11개 구체 State 클래스
├── Controllers       # NavigationController, CardFlip
├── Managers          # 7개 Manager 싱글톤 + NumPad
├── Models            # ECountry, CardData, ScriptableObject들
└── Views             # BaseView + 11개 구체 View 클래스

SwipeUI               # SwipeUI, HoverDetector
```

---

## 🧩 4. 핵심 설계 패턴

### 4.1 FSM (유한 상태 머신)

**적용 이유:** 11개 화면이 순차적/분기적으로 전환되며, 각 화면은 독립적인 진입/이탈 로직을 가진다. FSM으로 상태별 생명주기를 강제하여 리소스 누수를 방지한다.

**구현:**

```csharp
// IState — 5단계 생명주기 인터페이스
public interface IState
{
    void Init();    // 최초 1회 (이벤트 구독)
    void Enter();   // 매 진입 시 (UI 초기화, Show)
    void Update();  // 매 프레임
    void Exit();    // 매 이탈 시 (정리, Hide)
    void Dispose(); // 앱 종료 시
}

// StateMachine — Init 1회 호출 보장 (HashSet 추적)
public void ChangeState<T>() where T : IState
{
    _currentState?.Exit();
    _currentState = newState;
    if (_initializedStates.Add(type))  // HashSet으로 중복 방지
        _currentState.Init();
    _currentState.Enter();
    OnStateChanged?.Invoke(oldState, _currentState);
}
```

| 역할 | 클래스 | 책임 |
|------|--------|------|
| 인터페이스 | `IState` | 5단계 생명주기 계약 정의 |
| 추상 구현 | `BaseState<TState, TView>` | 제네릭 View 참조, 로그 자동 출력 |
| 상태 관리자 | `StateMachine` | Dictionary 기반 상태 저장, HashSet으로 Init 추적, Update 위임 |
| 오케스트레이터 | `NavigationController` | GoToXxx() 메서드로 상태 전환 라우팅, 파라미터 전달 |

### 4.2 MVC (Model-View-Controller 변형)

**적용 이유:** UI 표시(View)와 비즈니스 로직(State)을 분리하여 유지보수성을 높인다.

| 역할 | 클래스 | 책임 |
|------|--------|------|
| View | `BaseView` 상속 11개 | UI 요소 참조(public 필드), 버튼 이벤트 노출(Action 델리게이트) |
| Controller/State | `BaseState` 상속 11개 | 이벤트 핸들링, Manager 호출, UI 업데이트 지시 |
| Model | `ECountry`, `CardData`, `VoteData` | 데이터 구조 정의 |

**이벤트 바인딩 흐름:**

```csharp
// View: 버튼 클릭 → Action 이벤트 발생
protected override void BindUIEvent()
{
    _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
}

// State: Init()에서 이벤트 구독 (최초 1회)
public override void Init()
{
    _view._OnHomeButtonClicked += () => NavigationController.Instance.GoToHome();
}
```

### 4.3 Singleton (MonoSingleton)

**적용 이유:** Manager 클래스들이 전역에서 접근 가능해야 하며, 씬 전환 시에도 유지되어야 한다.

```csharp
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            lock (_lock)  // 스레드 안전
            {
                if (null == _instance)
                {
                    _instance = FindAnyObjectByType<T>();
                    if (null == _instance)
                    {
                        GameObject obj = new GameObject($"[Singleton] {typeof(T)}");
                        _instance = obj.AddComponent<T>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return _instance;
            }
        }
    }
}
```

| 역할 | 클래스 | 책임 |
|------|--------|------|
| 베이스 | `MonoSingleton<T>` | 스레드 안전 인스턴스 관리, DontDestroyOnLoad, 종료 시 재생성 방지 |
| 구현 | `NavigationController` | FSM 오케스트레이션 |
| 구현 | `NumGameManager` | 숫자 퀴즈 로직 |
| 구현 | `CardGameManager` | 카드 매칭 로직 |
| 구현 | `VideoManager` | 영상 재생 |
| 구현 | `VoteManager` | 투표 영속화 |
| 구현 | `SoundManager` | 효과음 |
| 구현 | `SliderManager` | 슬라이더 드래그 |
| 구현 | `IdleManager` | 무입력 감지 |

### 4.4 EnsureInitialized 패턴

**적용 이유:** Unity에서 비활성화(SetActive(false))된 GameObject는 Awake/Start가 호출되지 않아, 초기 비활성 View의 Initialize/BindUIEvent가 누락될 수 있다.

```csharp
// BaseView
public void EnsureInitialized()
{
    if (_isInitialized) return;
    _isInitialized = true;
    Initialize();
    BindUIEvent();
}

// NavigationController — 앱 시작 시 모든 View 강제 초기화
private void EnsureAllViewsInitialized()
{
    _homeView.EnsureInitialized();
    _selectView.EnsureInitialized();
    // ... 11개 View 모두 호출
}
```

---

## 🧱 5. 모듈별 클래스 명세

### 5.1 Core

#### MonoSingleton\<T\>

> 네임스페이스: `BusanMath.Core`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `Instance` | `static T` | 싱글톤 인스턴스 (lazy, 스레드 안전) |
| `HasInstance` | `static bool` | 인스턴스 존재 여부 (생성 없이) |

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `OnSingletonAwake()` | void | 초기화 훅 (서브클래스 오버라이드) |
| `OnSingletonDestroy()` | void | 정리 훅 (서브클래스 오버라이드) |

### 5.2 FSM

#### IState

> 네임스페이스: `BusanMath.FSM`

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `Init()` | void | 최초 1회 초기화 |
| `Enter()` | void | 상태 진입 (매번) |
| `Update()` | void | 매 프레임 |
| `Exit()` | void | 상태 이탈 (매번) |
| `Dispose()` | void | 앱 종료 시 정리 |

#### BaseState\<TState, TView\>

> 네임스페이스: `BusanMath.FSM`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `_view` | `TView` | 대응하는 View 참조 (생성자 주입) |

#### StateMachine

> 네임스페이스: `BusanMath.FSM`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `CurrentState` | `IState` | 현재 활성 상태 |
| `OnStateChanged` | `Action<IState, IState>` | 상태 변경 이벤트 (old, new) |

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `AddState<T>(T)` | void | 상태 등록 |
| `InitializeAllStates()` | void | 등록된 모든 상태의 Init() 호출 |
| `ChangeState<T>()` | void | 상태 전환 (Exit→Init→Enter→이벤트) |
| `GetState<T>()` | `T` | 등록된 상태 조회 (파라미터 전달용) |
| `IsCurrentState<T>()` | bool | 현재 상태 타입 확인 |

### 5.3 Controllers

#### NavigationController

> 네임스페이스: `BusanMath.Controllers` — `MonoSingleton<NavigationController>`

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `GoToHome()` | void | 홈 화면으로 전환 |
| `GoToSelect(bool skipToButtons = false)` | void | 국가 선택 화면 (skipToButtons=true 시 영상 95% 스킵) |
| `GoToVideo(ECountry)` | void | 교육 영상 (국가 전달) |
| `GoToNumGameDescription(ECountry)` | void | 숫자게임 설명 (국가 전달) |
| `GoToNumGame(ECountry)` | void | 숫자게임 (국가 전달) |
| `GoToCardGameDescription()` | void | 카드게임 설명 |
| `GoToCardGame()` | void | 카드게임 |
| `GoToVote()` | void | 투표 |
| `GoToWrite(ECountry)` | void | 생일 적기 (국가 전달) |
| `GoToDrawing(ECountry, int, int, int)` | void | 드로잉 (국가, 년, 월, 일 전달) |
| `GoToVoteResult()` | void | 투표 결과 |
| `IsHome()` | bool | 현재 홈 상태 여부 |

#### CardFlip

> 네임스페이스: `BusanMath.Controllers` — `MonoBehaviour, IPointerClickHandler`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `_cardIdx` | int | 카드 인덱스 |
| `_frontSprite` | Sprite | 앞면 스프라이트 |
| `_backSprite` | Sprite | 뒷면 스프라이트 |
| `_OnClickCard` | `Action<int>` | 카드 클릭 이벤트 |

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `Flip()` | void | Y축 90° 회전 후 스프라이트 교체 (DOTween) |
| `LateFlip(float)` | void | 지연 후 플립 (코루틴) |
| `Restore()` | void | 카드 상태 완전 초기화 (트윈 킬, 코루틴 정지) |

### 5.4 Managers

#### VideoManager

> 네임스페이스: `BusanMath.Managers` — `MonoSingleton<VideoManager>`

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `SetDisplay(RawImage)` | void | 영상 출력 대상 설정 |
| `Play(string filePath)` | void | 영상 준비 및 재생 (Prepare→OnPrepared→Play) |
| `Stop()` / `Pause()` | void | 정지 / 일시정지 |
| `IsPlaying()` | bool | 재생 중 여부 |
| `VideoLength()` | double | 영상 길이 (초) |
| `Progress()` | float | 재생 진행률 (0.0~1.0) |
| `SetPlayerTime(float)` | void | 재생 위치 직접 설정 |
| `Skip()` | void | 영상 95% 지점으로 스킵 |

#### NumGameManager

> 네임스페이스: `BusanMath.Managers` — `MonoSingleton<NumGameManager>`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `RndNum` | string | 출제된 랜덤 숫자 (아라비아 숫자 문자열) |
| `Answer` | string | 사용자 입력 정답 |

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `StartGame(ECountry)` | void | 국가 설정 + 랜덤 숫자 생성 |
| `GetRndNumSprite()` | Sprite | 출제 숫자의 이집트/로마 스프라이트 |
| `GetRndNumToHanJa()` | string | 출제 숫자의 중국 한자 변환 |
| `SelectNumTile(int)` | bool | 넘패드 입력 처리 (자릿수 초과 시 false) |
| `CompareAnswerAndRndNum()` | bool | 정답 비교 |
| `InitAnswer()` | void | 입력 초기화 |
| `InitGame()` | void | 전체 게임 초기화 |

#### CardGameManager

> 네임스페이스: `BusanMath.Managers` — `MonoSingleton<CardGameManager>`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `_OnMatchSuccess` | `Action<int, int>` | 매칭 성공 이벤트 (인덱스 2개) |
| `_OnMatchFail` | `Action<int, int>` | 매칭 실패 이벤트 |
| `_OnGameClear` | `Action` | 게임 클리어 이벤트 |
| `_isSuccess` | bool | 시간 내 클리어 여부 |

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `StartGame()` | void | 6쌍 카드 셔플 + 게임 시작 |
| `SelectCard(int)` | void | 카드 선택 → 2장 선택 시 매칭 판정 |
| `GetCard(int)` | CardData | 인덱스로 카드 데이터 조회 |
| `GetCurrentDeck()` | `List<CardData>` | 현재 덱 반환 |
| `ResetGame()` | void | 게임 완전 초기화 |

#### VoteManager

> 네임스페이스: `BusanMath.Managers` — `MonoSingleton<VoteManager>`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `_OnVoteUpdated` | `Action<VoteData>` | 투표 갱신 이벤트 |

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `Vote(ECountry)` | void | 투표 + JSON 저장 |
| `GetTotal()` | int | 전체 투표 수 |
| `GetRate(ECountry)` | float | 특정 국가 투표 비율 (0.0~1.0) |
| `GetRanking()` | `List<ECountry>` | 투표 순위 (내림차순) |
| `GetData()` | VoteData | 원시 투표 데이터 |

#### SoundManager

> 네임스페이스: `BusanMath.Managers` — `MonoSingleton<SoundManager>`

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `PlayCorrectSound()` | void | 정답 효과음 |
| `PlayDisCorrectSound()` | void | 오답 효과음 |
| `PlayButtonSound()` | void | 버튼 클릭 효과음 |

#### SliderManager

> 네임스페이스: `BusanMath.Managers` — `MonoSingleton<SliderManager>`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `Player` | VideoPlayer (set) | 비디오 플레이어 참조 설정 |
| `Slider` | Slider (set) | 슬라이더 참조 설정 + EventTrigger 자동 등록 |
| `IsDragging` | bool (get) | 드래그 중 여부 |

#### IdleManager

> 네임스페이스: `BusanMath.Managers` — `MonoSingleton<IdleManager>`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `OnIdleTimeout` | `Action` | 타임아웃 이벤트 |
| `OnIdleReset` | `Action` | 타이머 리셋 이벤트 |
| `IdleTime` | float | 현재 유휴 시간 |
| `RemainingTime` | float | 남은 시간 |

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `ResetTimer()` | void | 타이머 0으로 리셋 |
| `SetTimeout(float)` | void | 타임아웃 시간 변경 |
| `SetEnabled(bool)` | void | 활성/비활성 전환 |
| `Pause()` / `Resume()` | void | 일시정지 / 재개 |

### 5.5 Models

#### ECountry

> 네임스페이스: `BusanMath.Models`

```csharp
public enum ECountry
{
    Egypt,      // 0
    China,      // 1
    Roma,       // 2
    MAX_CNT,    // 3 (반복용)
    None,       // 4
}
```

#### CardData / CardDatabaseSO

> 네임스페이스: `BusanMath.Models`

```csharp
public class CardData
{
    public string _country;     // 국가 식별자
    public int _value;          // 숫자 값 (매칭 비교용)
    public Sprite _cardSprite;  // 카드 앞면 이미지
}

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Game/Card Database")]
public class CardDatabaseSO : ScriptableObject
{
    public List<CardData> cards;  // 전체 카드 풀 (40+장)
}
```

#### StringSpritePairContainerSO

> 네임스페이스: `BusanMath.Models`

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `ToDictionary()` | `Dictionary<string, Sprite>` | 전체 매핑을 딕셔너리로 변환 |
| `GetSprite(string)` | Sprite | 키로 스프라이트 조회 |
| `GetRandom()` | StringSpritePair | 랜덤 페어 반환 (출제용) |

### 5.6 Views

#### BaseView

> 네임스페이스: `BusanMath.Views`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `_rootPanel` | GameObject | Show/Hide 대상 |
| `IsVisible` | bool | 현재 표시 상태 |
| `OnShow` / `OnHide` | Action | 표시/숨김 이벤트 |

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `Show()` | void | rootPanel 활성화 |
| `Hide()` | void | rootPanel 비활성화 |
| `Toggle()` | void | 표시 상태 전환 |
| `EnsureInitialized()` | void | 비활성 View도 안전 초기화 |
| `Initialize()` | void | 서브클래스 초기화 훅 (protected virtual) |
| `BindUIEvent()` | void | 서브클래스 이벤트 바인딩 훅 (protected virtual) |

### 5.7 SwipeUI

#### SwipeUI

> 네임스페이스: `SwipeUI`

| 멤버 | 타입 | 설명 |
|------|------|------|
| `CurrentPage` | int | 현재 페이지 인덱스 |
| `_OnSwipeCompleted` | Action | 스와이프 완료 이벤트 |

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `AutoSwipe(bool isLeft)` | void | 프로그램 제어 스와이프 (true=왼쪽/이전) |

#### HoverDetector

> 네임스페이스: `SwipeUI` — `IPointerEnterHandler, IPointerExitHandler`

| 메서드 | 반환 | 설명 |
|--------|------|------|
| `IsHover()` | bool | 현재 포인터가 위에 있는지 여부 |

---

## 🔄 6. 데이터 흐름

### 앱 초기화

```
Unity Awake
  └─► NavigationController.OnSingletonAwake()
      ├─► EnsureAllViewsInitialized()     // 11개 View 강제 초기화
      │   └─► 각 View.EnsureInitialized()
      │       ├─► Initialize()             // 팝업 비활성화 등
      │       └─► BindUIEvent()            // Button.onClick → Action 연결
      │
      └─► InitializeStateMachine()
          ├─► StateMachine 생성
          ├─► 11개 State 등록 (AddState)
          ├─► InitializeAllStates()        // 모든 State.Init() 호출
          │   └─► 각 State.Init()          // View 이벤트 구독 (Action += 핸들러)
          ├─► OnStateChanged += IdleManager.ResetTimer
          └─► ChangeState<HomeState>()     // 최초 화면 진입
```

### 상태 전환

```
NavigationController.GoToXxx(params)
  └─► StateMachine.GetState<T>().Property = value   // 파라미터 전달
      └─► StateMachine.ChangeState<T>()
          ├─► oldState.Exit()
          │   ├─► Manager 정리 (Stop, Reset 등)
          │   ├─► View.Hide()      // rootPanel 비활성화
          │   └─► 이벤트 해제 (매 진입 등록분)
          │
          ├─► newState.Init()      // (최초 1회만, HashSet 체크)
          │
          ├─► newState.Enter()
          │   ├─► View.Show()      // rootPanel 활성화
          │   └─► Manager 초기화 (StartGame, Play 등)
          │
          └─► OnStateChanged 이벤트
              └─► IdleManager.ResetTimer()
```

### 사용자 상호작용

```
터치/클릭 입력
  └─► Button.onClick
      └─► View의 Action 이벤트 발생
          └─► State의 이벤트 핸들러 실행
              ├─► Manager 로직 호출
              │   └─► Manager 결과 이벤트 (성공/실패/클리어)
              │       └─► State가 View UI 업데이트
              └─► 또는 NavigationController.GoToXxx()
                  └─► 상태 전환 (위 흐름)
```

### 투표 영속화

```
VoteState → VoteManager.Vote(ECountry)
  ├─► VoteData 카운터 증가
  ├─► JsonUtility.ToJson() → File.WriteAllText()
  │   경로: Application.persistentDataPath/vote_data.json
  └─► _OnVoteUpdated 이벤트 발생

VoteResultState.Enter()
  └─► VoteManager.GetRanking() / GetRate() / GetData()
      └─► VoteResultView UI 업데이트 (순위, 비율, 바 차트)
```

### 무입력 감지

```
IdleManager.Update() (매 프레임)
  ├─► HasAnyInput() 체크 (마우스/터치/키보드)
  │   ├─► true  → ResetTimer() (0으로 초기화)
  │   └─► false → idleTimer += deltaTime
  │
  └─► CheckTimeout()
      └─► idleTimer >= 60초
          ├─► OnIdleTimeout 이벤트 발생
          │   └─► NavigationController.GoToHome() (홈이 아닌 경우)
          └─► ResetTimer()
```

---

## 🧭 7. 화면/기능 전환 흐름

### 정상 흐름

```
┌──────────┐
│   Home   │
└────┬─────┘
     │
     ├─[왼쪽 버튼]──► Select (국가 선택 + 인트로 영상)
     │                  │ [영상 90% 도달 시 버튼 페이드인]
     │                  ├─ Egypt ──┐
     │                  ├─ China ──┼──► Video (교육 영상)
     │                  └─ Roma  ──┘       │ [영상 완료 or 스킵]
     │                                     ▼
     ├─[오른쪽 버튼]──► NumGameDescription (숫자게임 설명)
     │                         │ [스와이프 3페이지 → 시작]
     │                         ▼
     │                    NumGame (숫자 변환 퀴즈)
     │                         │ [다른나라]──► Select (영상 95% 스킵)
     │                         │ [다음게임]
     │                         ▼
     │                  CardGameDescription (카드게임 설명)
     │                         │ [스와이프 → 시작]
     │                         ▼
     │                    CardGame (60초 카드 매칭)
     │                         │ [완료/시간초과 → 다음]
     │                         ▼
     │                      Vote (투표)
     │                         │ [국가 선택 → 투표 저장]
     │                         ▼
     │                      Write (생일 적기 날짜 선택)
     │                         │ [년/월/일 모두 선택 시]
     │                         ▼
     │                     Drawing (드로잉) ──[다시선택]──► Write
     │                         │ [다음]
     │                         ▼
     │                    VoteResult (투표 결과)
     │                         │ [홈 버튼]
     └─────────────────────────┘
```

### 예외 흐름

```
[어떤 화면에서든]
  ├─► 홈 버튼 클릭 → GoToHome() → Home
  └─► 60초 무입력 → IdleManager → GoToHome() → Home

[Select 화면]
  └─► 진행바 되감기(90% 이하) → 버튼 페이드아웃
      → 다시 90% 도달 시 → 버튼 페이드인

[NumGame 결과 팝업]
  ├─► 다시하기 → RetryNumGame() (같은 State 내 재시작)
  ├─► 다음게임 → GoToCardGameDescription()
  └─► 다른나라 → GoToSelect(skipToButtons: true)

[CardGame]
  ├─► 60초 경과 → 자동 게임 클리어 (실패)
  ├─► Space 키 → 강제 종료 (디버그용)
  └─► 다시하기 → HandleRetryButton() (같은 State 내 재시작)

[Write → Drawing 전환]
  └─► 년/월/일 중 하나라도 미선택(==0) → 전환 차단 (return)
```

### 내부 호출 체인 — 화면 전환 예시 (Vote → Write)

```
1. VoteView._romaButton.onClick
2. → VoteView._OnRomaButtonClicked?.Invoke()
3. → VoteState.VoteCountry(ECountry.Roma)
4.   → VoteManager.Instance.Vote(ECountry.Roma)   // JSON 저장
5.   → NavigationController.Instance.GoToWrite(ECountry.Roma)
6.     → StateMachine.GetState<WriteState>().Country = Roma
7.     → StateMachine.ChangeState<WriteState>()
8.       → VoteState.Exit()
9.         → VoteView.Hide()
10.      → WriteState.Enter()
11.        → WriteView.Show()
12.        → 날짜 선택 UI 초기화 ("?" 표시)
13.      → OnStateChanged → IdleManager.ResetTimer()
```

---

## ♻️ 8. 라이프사이클

### 앱 시작

```
시간 ──────────────────────────────────────────────────────►

[Unity Awake Phase]
  MonoSingleton들 Awake
    ├─ VideoManager: VideoPlayer 생성
    ├─ VoteManager: JSON 로드
    ├─ IdleManager: 타이머 리셋
    └─ NavigationController:
        ├─ EnsureAllViewsInitialized()
        ├─ StateMachine 생성 + 11 State 등록
        ├─ InitializeAllStates()  ← 모든 State.Init() 호출
        └─ ChangeState<HomeState>()
            ├─ HomeState.Enter()
            └─ HomeView.Show()

[Unity Start Phase]
  BaseView.Start() → EnsureInitialized() (이미 초기화됨, 스킵)
  IdleManager.Start() → OnIdleTimeout += GoToHome

[런타임 루프]
  StateMachine.Update() → currentState.Update()
  IdleManager.Update() → 무입력 감지
```

### 상태 전환 타임라인

```
시간 ──────────────────────────────────────────────────────►

State A                    State B
  │                          │
  │ ◄── Update() 반복 ──►   │
  │                          │
  ├── Exit() ─────────────── ┤
  │   ├─ Manager 정리        │
  │   ├─ 이벤트 해제         │
  │   └─ View.Hide()        │
  │                          ├── Init() (최초 1회만)
  │                          │   └─ 이벤트 구독
  │                          ├── Enter()
  │                          │   ├─ View.Show()
  │                          │   └─ Manager 초기화
  │                          │
  │                          │ ◄── Update() 반복 ──►
```

### 앱 종료

```
[OnDestroy]
  StateMachine.OnDestroy()
    └─ foreach state: state.Dispose()  // 이벤트 해제

  NavigationController.OnDestroy()
    └─ OnStateChanged -= HandleStateChanged

  MonoSingleton.OnDestroy()
    └─ _instance = null

[OnApplicationQuit]
  MonoSingleton._isApplicationQuitting = true  // 재생성 방지
```

---

## 🧑‍💻 9. 확장 가이드

### 새 화면 추가 가이드

#### Step 1: View 클래스 생성

`Assets/Scripts/BusanMath/Views/` 에 생성:

```csharp
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class NewFeatureView : BaseView
    {
        // ── UI 요소 ───────────────────────────────────
        [Header("=== UI 요소 ===")]
        public Button _homeButton;        // 홈 이동 버튼
        // 필요한 UI 요소 추가...

        // ── 이벤트 ────────────────────────────────────
        public event Action _OnHomeButtonClicked;
        // 필요한 이벤트 추가...

        protected override void Initialize()
        {
            // 초기 상태 설정 (팝업 숨기기 등)
        }

        protected override void BindUIEvent()
        {
            _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        }
    }
}
```

#### Step 2: State 클래스 생성

`Assets/Scripts/BusanMath/FSM/States/` 에 생성:

```csharp
using BusanMath.FSM;
using BusanMath.Views;
using BusanMath.Controllers;

namespace BusanMath.FSM.States
{
    public class NewFeatureState : BaseState<NewFeatureState, NewFeatureView>
    {
        public NewFeatureState(NewFeatureView view) : base(view) { }

        public override void Init()
        {
            base.Init();
            // 이벤트 구독 (최초 1회)
            _view._OnHomeButtonClicked += () => NavigationController.Instance.GoToHome();
        }

        public override void Enter()
        {
            base.Enter();
            _view.Show();
            // 진입 시 초기화 로직
        }

        public override void Exit()
        {
            base.Exit();
            // 정리 로직
            _view.Hide();
        }
    }
}
```

#### Step 3: NavigationController에 등록

```csharp
// 1. View 필드 추가
[SerializeField] private NewFeatureView _newFeatureView;

// 2. EnsureAllViewsInitialized()에 추가
_newFeatureView.EnsureInitialized();

// 3. InitializeStateMachine()에 State 등록
StateMachine.AddState(new NewFeatureState(_newFeatureView));

// 4. 네비게이션 메서드 추가
public void GoToNewFeature()
{
    StateMachine.ChangeState<NewFeatureState>();
}
```

#### Step 4: Unity 에디터 설정

1. 새 화면용 UI Canvas/Panel 생성
2. View 컴포넌트 부착 + UI 요소 할당
3. NavigationController의 Inspector에서 View 참조 연결

### 핵심 규칙

| 규칙 | 이유 |
|------|------|
| View에는 로직을 넣지 않는다 | UI 참조와 이벤트 발생만 담당, 로직은 State에서 처리 |
| State.Init()에서 이벤트 구독 | StateMachine이 HashSet으로 1회만 호출 보장 |
| State.Enter()에서 View.Show() | 매 진입 시 UI 초기화 보장 |
| State.Exit()에서 View.Hide() | 화면 이탈 시 UI 정리 보장 |
| Manager 이벤트는 Enter/Exit에서 등록/해제 | Init에서 하면 해제가 누락될 수 있음 (CardGameState 참고) |
| 파라미터는 GoToXxx()에서 GetState\<T\>()로 전달 | State 생성자는 View만 받음, 런타임 파라미터는 set 프로퍼티 사용 |
| NavigationController만 ChangeState 호출 | 전환 로직을 한 곳에서 관리하여 흐름 추적 용이 |

---

## ⚖️ 10. 설계 결정 및 트레이드오프

### 10.1 State가 MonoBehaviour가 아닌 일반 클래스

| 항목 | 내용 |
|------|------|
| **결정** | State는 MonoBehaviour를 상속하지 않고 순수 C# 클래스로 구현 |
| **장점** | new로 생성 가능, Unity 생명주기에 의존하지 않아 초기화 순서 제어 용이 |
| **단점** | Update()를 StateMachine이 수동 위임해야 함, 코루틴 직접 사용 불가 |
| **결론** | StateMachine이 MonoBehaviour로서 Update()를 위임하고, 코루틴은 Manager에서 처리 |

### 10.2 View의 public 필드 + Action 이벤트

| 항목 | 내용 |
|------|------|
| **결정** | View의 UI 요소를 public 필드로 노출하고, 버튼 클릭은 Action 이벤트로 전파 |
| **장점** | State에서 직접 UI 조작 가능(유연), Inspector 할당 용이, 느슨한 결합 |
| **단점** | 캡슐화 약함, State가 View 내부 구조에 의존 |
| **결론** | 프로젝트 규모(11화면)에서 실용적, 과도한 추상화 대비 개발 속도 우선 |

### 10.3 Init 1회 호출 보장 (HashSet)

| 항목 | 내용 |
|------|------|
| **결정** | StateMachine이 HashSet으로 Init() 호출 여부를 추적하여 중복 방지 |
| **장점** | 이벤트 중복 구독 방지, 리소스 한 번만 할당 |
| **단점** | 동적으로 이벤트를 변경해야 하는 경우 별도 처리 필요 |
| **결론** | 대부분의 이벤트가 고정적이므로 현재 구조에 적합 |

### 10.4 EnsureInitialized 패턴

| 항목 | 내용 |
|------|------|
| **결정** | 앱 시작 시 NavigationController가 모든 View의 EnsureInitialized()를 명시적 호출 |
| **장점** | 비활성 GameObject도 확실하게 초기화, 초기화 순서 보장 |
| **단점** | 새 View 추가 시 NavigationController에 수동 등록 필요 |
| **결론** | Unity의 비활성 GameObject 초기화 문제를 안정적으로 해결 |

### 10.5 국가별 스프라이트를 List\<Sprite\>로 관리

| 항목 | 내용 |
|------|------|
| **결정** | DrawingView에서 년/월/일 스프라이트를 List로 관리 (index = value - offset) |
| **장점** | Inspector에서 드래그&드롭으로 쉽게 할당, 순차적 접근에 효율적 |
| **단점** | 키가 연속적이지 않으면 빈 슬롯 낭비 (현재는 연속적이므로 문제 없음) |
| **결론** | 년(1980~2030), 월(1~12), 일(1~31) 모두 연속 범위이므로 List가 최적 |

### 10.6 GoToSelect(skipToButtons) 영상 스킵

| 항목 | 내용 |
|------|------|
| **결정** | 다른 나라 선택 시 Select 화면에서 영상을 95%로 스킵하여 바로 국가 버튼 표시 |
| **장점** | 이미 인트로 영상을 본 사용자가 다시 기다릴 필요 없음, UX 개선 |
| **단점** | 영상 Prepare 완료 후에만 스킵 가능하여 Update()에서 대기 로직 필요 |
| **결론** | Update()에서 IsPlaying && VideoLength > 0 체크 후 1회 스킵으로 해결 |
