# work_unity_busanMathmatics

부산 수학문화관 인터랙티브 교육 애플리케이션

- **플랫폼:** Windows (터치 디스플레이 키오스크)
- **엔진:** Unity 6 (URP)
- **언어:** C#
- **핵심 기능:** 이집트/중국/로마 역사적 숫자 체계를 게임, 영상, 그리기 활동으로 학습

## 아키텍처

| 패턴 | 역할 |
|------|------|
| **FSM (유한 상태 머신)** | `StateMachine` + `IState`/`BaseState<TState,TView>` 기반 5단계 생명주기(Init/Enter/Update/Exit/Dispose), Init 1회 호출 보장 |
| **MVC** | `BaseView` 추상 클래스를 상속한 12개 View가 UI 표시, State가 비즈니스 로직 담당 |
| **Singleton** | `MonoSingleton<T>` (스레드 안전, DontDestroyOnLoad)로 Manager 클래스 관리 |
| **Event-Driven** | Action 델리게이트로 View↔State↔Manager 간 느슨한 결합 |
| **EnsureInitialized** | `BaseView.EnsureInitialized()`로 비활성 View도 프로그램 시작 시 안전하게 초기화 |

## 디렉토리 구조

```
Assets/Scripts/BusanMath/
├── Core/
│   └── MonoSingleton.cs            # 싱글톤 베이스 클래스
├── FSM/
│   ├── IState.cs                   # 상태 인터페이스
│   ├── BaseState.cs                # 상태 추상 클래스
│   ├── StateMachine.cs             # 상태 머신 (Dictionary 기반)
│   └── States/
│       ├── HomeState.cs            # 홈 화면
│       ├── SelectState.cs          # 국가 선택 + 인트로 영상
│       ├── VideoState.cs           # 교육 영상 재생
│       ├── NumGameDescriptionState.cs  # 숫자게임 설명
│       ├── NumGameState.cs         # 숫자 변환 퀴즈
│       ├── CardGameDescriptionState.cs # 카드게임 설명
│       ├── CardGameState.cs        # 60초 카드 매칭 게임
│       ├── VoteState.cs            # 투표
│       ├── WriteState.cs           # 생일 쓰기 + 드로잉
│       ├── Write2State.cs          # 생일 쓰기 2 (날짜 선택 UI)
│       └── VoteResultState.cs      # 투표 결과
├── Controllers/
│   ├── NavigationController.cs     # FSM 관리, 화면 전환 라우팅 (싱글톤)
│   └── CardFlip.cs                 # DOTween 카드 플립 애니메이션
├── Managers/
│   ├── NumGameManager.cs           # 숫자게임 로직 (랜덤 출제, 정답 비교)
│   ├── CardGameManager.cs          # 카드게임 로직 (셔플, 매칭, 클리어)
│   ├── SoundManager.cs             # 효과음 재생
│   ├── VideoManager.cs             # 비디오 재생/정지
│   ├── SliderManager.cs            # 비디오 진행바 동기화
│   ├── VoteManager.cs              # 투표 데이터 영속화 (JSON)
│   ├── IdleManager.cs              # 60초 무입력 감지 → 홈 복귀
│   └── NumPad.cs                   # 숫자 버튼 핸들러
├── Models/
│   ├── CardDatabaseSO.cs           # 카드 데이터 ScriptableObject (40+장)
│   └── StringSpritePairContainerSO.cs  # 숫자↔스프라이트 매핑
└── Views/
    ├── BaseView.cs                 # View 추상 클래스 (Show/Hide/Toggle)
    ├── HomeView.cs
    ├── SelectView.cs
    ├── VideoView.cs
    ├── NumGameDescriptionView.cs
    ├── NumGameView.cs
    ├── CardGameDescriptionView.cs
    ├── CardGameView.cs
    ├── VoteView.cs
    ├── WriteView.cs
    ├── Write2View.cs
    └── VoteResultView.cs
```

## 주요 클래스

### Core / FSM

| 클래스 | 역할 |
|--------|------|
| `MonoSingleton<T>` | 스레드 안전 싱글톤, DontDestroyOnLoad, OnSingletonAwake/Destroy 훅 |
| `StateMachine` | Dictionary<Type, IState> 기반 상태 관리, ChangeState<T>(), OnStateChanged 이벤트 |
| `BaseState<TState, TView>` | IState 제네릭 구현, 타입 안전한 View 참조(`_view`), 로그 자동 출력 |
| `NavigationController` | FSM 오케스트레이터, GoToXxx() 메서드로 화면 전환, EnsureAllViewsInitialized()로 비활성 View 초기화 보장 |

### Managers

| 클래스 | 역할 |
|--------|------|
| `NumGameManager` | 2~3자리 랜덤 숫자 출제, 이집트 상형문자/중국 한자/로마 숫자 변환, 정답 비교 |
| `CardGameManager` | 6쌍(12장) 카드 셔플, 선택/매칭 판정, 지연 콜백(코루틴), 게임 클리어 |
| `VoteManager` | vote_data.json 로드/세이브, 투표/비율/랭킹 계산 |
| `VideoManager` | VideoPlayer 래핑, 재생/정지/스킵/진행률 |
| `IdleManager` | 마우스/터치/키보드 입력 감지, 60초 타임아웃 시 홈 복귀 |
| `SoundManager` | 정답/오답/버튼 효과음 재생 |

### Views

| 클래스 | 역할 |
|--------|------|
| `BaseView` | rootPanel Show/Hide, IsVisible, OnShow/OnHide 이벤트, EnsureInitialized()로 비활성 GO도 안전 초기화, Initialize/BindUIEvent 훅 |
| 각 View | 해당 화면의 UI 요소 참조 및 버튼 이벤트 노출 (Action 델리게이트) |

## 화면/기능 흐름

```
┌──────────┐
│   Home   │
└────┬─────┘
     │
     ├─[왼쪽 버튼]──► Select (국가 선택 + 인트로 영상)
     │                  │
     │                  ├─ Egypt ──┐
     │                  ├─ China ──┼──► Video (교육 영상)
     │                  └─ Roma  ──┘       │
     │                                     ▼
     ├─[오른쪽 버튼]──► NumGameDescription (숫자게임 설명)
     │                         │
     │                         ▼
     │                    NumGame (숫자 변환 퀴즈)
     │                         │
     │                         ▼
     │                  CardGameDescription (카드게임 설명)
     │                         │
     │                         ▼
     │                    CardGame (60초 카드 매칭)
     │                         │
     │                         ▼
     │                      Vote (투표)
     │                         │
     │                         ▼
     │                     Write2 (생일 적기 날짜 선택)
     │                         │
     │                         ▼
     │                    VoteResult (투표 결과)
     │                         │
     └─────────────────────────┘

※ 60초 무입력 시 IdleManager가 자동으로 Home 복귀
※ 각 화면의 Home 버튼으로 언제든 Home 복귀 가능
```

## 데이터 흐름

```
1. NavigationController.OnSingletonAwake()
   └─► EnsureAllViewsInitialized()  // 비활성 View도 Initialize/BindUIEvent 실행
       └─► StateMachine.ChangeState<T>()
           ├─► oldState.Exit()    // 매 이탈 시: 이벤트 해제, UI 정리
           ├─► newState.Init()    // 최초 1회: View 이벤트 구독 (HashSet 추적)
           ├─► newState.Enter()   // 매 진입 시: UI 초기화, Show
           └─► OnStateChanged     // IdleManager 타이머 리셋

2. State.Enter()
   └─► View.Show()           // rootPanel 활성화
       └─► Manager 초기화     // StartGame(), Play() 등

3. View 버튼 클릭
   └─► Action 이벤트 발생
       └─► State 핸들러 실행
           └─► Manager 로직 호출
               └─► Manager 이벤트 발생 (성공/실패/클리어)
                   └─► State가 UI 업데이트

4. 투표 데이터 영속화
   VoteManager.Vote() → VoteData 갱신 → JSON 파일 저장
   경로: Application.persistentDataPath/vote_data.json
```

## 변경 이력

| 날짜 | 변경 내용 |
|------|-----------|
| 2026-03-11 | FSM State 초기화/정리 로직 보완 - 이벤트 구독 해제 버그 수정, 코루틴/애니메이션 정리, UI 초기화 누락 보완, 싱글톤 정리 개선 |
| 2026-03-11 | PagingTemplate 아키텍처 적용 - IState 5단계 생명주기(Init/Dispose 추가), BaseState 제네릭화, StateMachine Init 1회 호출 보장(HashSet 추적), 전 State 리팩토링 |
| 2026-03-11 | BaseView에 EnsureInitialized() 추가 - 비활성 View의 Initialize/BindUIEvent 누락 방지, NavigationController에서 명시적 호출 |
| 2026-03-11 | 전체 스크립트 주석 정비 - 깨진 인코딩 주석을 한글 XML 주석으로 재작성, 불필요한 using(NUnit.Framework 등) 제거 |
| 2026-03-12 | Write2State/Write2View 추가 - 생일 적기 2번째 콘텐츠, 날짜 선택 UI(년/월/일 동적 버튼 생성, 패널 토글) |
| 2026-03-13 | Write2State View Show/Hide 누락 수정 - Enter()에 Show(), Exit()에 Hide() 추가 |
