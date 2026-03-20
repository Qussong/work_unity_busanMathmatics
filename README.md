# 부산 수학문화관 인터랙티브 교육 키오스크

> 고대 문명의 숫자 체계를 게임, 영상, 드로잉으로 체험하는 박물관 전시 콘텐츠

`[📷 이미지 배치 예정 - 앱 메인 화면 스크린샷]`

| 항목 | 내용 |
|------|------|
| 클라이언트 | 부산 수학문화관 |
| 플랫폼 | Windows (터치 디스플레이 키오스크) |
| 엔진/언어 | Unity 6 (URP) / C# |
| 설계 패턴 | FSM + MVC 변형 (State-View) + Singleton + Event-Driven |
| 배포 환경 | 부산 수학문화관 현장 키오스크 |

**문서**: [포트폴리오 명세서](PORTFOLIO_SPEC.md) · [기술 명세서](TECH_SPEC.md)

---

## 화면 구성

| Home | Select | Video | NumGame | CardGame |
|:----:|:------:|:-----:|:-------:|:--------:|
| `[📷 이미지 배치 예정]` | `[📷 이미지 배치 예정]` | `[📷 이미지 배치 예정]` | `[📷 이미지 배치 예정]` | `[📷 이미지 배치 예정]` |

| Vote | Write | Drawing | VoteResult |
|:----:|:-----:|:-------:|:----------:|
| `[📷 이미지 배치 예정]` | `[📷 이미지 배치 예정]` | `[📷 이미지 배치 예정]` | `[📷 이미지 배치 예정]` |

```
Home ──► Select (인트로 영상 + 국가 선택)
              │
              ├─ Egypt/China/Roma ──► Video (교육 영상)
              │                           │
              ▼                           ▼
         NumGameDesc ──► NumGame (숫자 퀴즈)
                              │ [다른나라] ──► Select (영상 스킵)
                              ▼
                        CardGameDesc ──► CardGame (60초 매칭)
                                              │
                                              ▼
                                           Vote (투표)
                                              │
                                              ▼
                                         Write (날짜 선택)
                                              │
                                              ▼
                                        Drawing (드로잉) ──[다시선택]──► Write
                                              │
                                              ▼
                                        VoteResult ──► Home
```

| 화면 | 설명 |
|------|------|
| Home | 시작 화면, 좌측(Select)/우측(NumGameDesc) 분기 |
| Select | 인트로 영상 재생 + 90% 이상 시청 시 국가 선택 버튼 등장 |
| Video | 이집트/중국/로마 교육 영상 (진행바 드래그/스킵 지원) |
| NumGameDesc | 숫자 변환 퀴즈 설명 화면 |
| NumGame | 아라비아 숫자 → 고대 숫자 변환 퀴즈 |
| CardGameDesc | 카드 매칭 게임 설명 화면 |
| CardGame | 6쌍(12장) 60초 제한 카드 매칭 게임 |
| Vote | 가장 마음에 드는 숫자 체계 투표 |
| Write | 생일 날짜 선택 (년/월/일 동적 버튼) |
| Drawing | 선택한 날짜를 고대 숫자 이미지로 변환, 반투명 오버레이 위에 드로잉 |
| VoteResult | 누적 투표 결과 순위/바 차트 표시 |

- 어떤 화면에서든 홈 버튼으로 즉시 복귀 가능
- 60초 무입력 시 IdleManager가 자동으로 Home 복귀 (무인 운영)
- 다른 나라 선택 시 인트로 영상 95% 스킵 → 바로 국가 버튼 표시
- 영상 진행바 되감기 시 버튼 페이드아웃, 재도달 시 페이드인

---

## 핵심 기능

| 기능 | 설명 |
|------|------|
| 문명별 교육 영상 | VideoPlayer + RenderTexture 기반, 진행바 드래그/스킵 지원 |
| 숫자 변환 퀴즈 | 2~3자리 랜덤 출제, 이집트 상형문자/중국 한자/로마 숫자 변환 |
| 카드 매칭 게임 | 6쌍 셔플 + DOTween Y축 플립 애니메이션, 60초 제한 |
| 투표 시스템 | JSON 파일 영속화, 누적 결과 순위/바 차트 표시 |
| 생일 드로잉 | 날짜 → 고대 숫자 이미지 변환, 반투명 오버레이 + 터치 드로잉 |
| 자동 홈 복귀 | 60초 무입력 감지 시 홈 강제 복귀, 무인 키오스크 연속 운영 |

---

## 아키텍처

FSM(유한 상태 머신) + MVC 변형(State-View 1:1 매핑) + Singleton + Event-Driven 조합

| 레이어 | 역할 | 주요 클래스 |
|--------|------|-------------|
| Controllers | FSM 오케스트레이터, 화면 전환 중앙 관리 | `NavigationController`, `CardFlip` |
| FSM | 상태 인터페이스/추상 클래스, Dictionary 기반 상태 관리 | `IState`, `BaseState<TState,TView>`, `StateMachine` |
| States | 11개 화면별 비즈니스 로직 (이벤트 구독, Manager 호출) | `HomeState`, `SelectState`, `VideoState` 외 8개 |
| Views | 11개 화면별 UI 요소 참조 + Action 이벤트 발생 | `BaseView`, `HomeView`, `SelectView` 외 9개 |
| Managers | 도메인 로직 싱글톤 (게임/영상/투표/입력 감지) | `NumGameManager`, `CardGameManager`, `VoteManager`, `VideoManager`, `IdleManager`, `SoundManager`, `SliderManager`, `NumPad` |
| Models | 데이터 정의 (열거형, ScriptableObject) | `ECountry`, `CardDatabaseSO`, `StringSpritePairContainerSO` |
| Core | 싱글톤 베이스 | `MonoSingleton<T>` |

> 클래스별 상세 명세는 [기술 명세서](TECH_SPEC.md)를 참고하세요.

---

## 디렉토리 구조

```
Assets/Scripts/
├── BusanMath/
│   ├── Core/
│   │   └── MonoSingleton.cs
│   ├── FSM/
│   │   ├── IState.cs
│   │   ├── BaseState.cs
│   │   ├── StateMachine.cs
│   │   └── States/                  # 11개 State
│   │       ├── HomeState.cs
│   │       ├── SelectState.cs
│   │       ├── VideoState.cs
│   │       ├── NumGameDescriptionState.cs
│   │       ├── NumGameState.cs
│   │       ├── CardGameDescriptionState.cs
│   │       ├── CardGameState.cs
│   │       ├── VoteState.cs
│   │       ├── DrawingState.cs
│   │       ├── WriteState.cs
│   │       └── VoteResultState.cs
│   ├── Controllers/
│   │   ├── NavigationController.cs  # FSM 오케스트레이터 (싱글톤)
│   │   └── CardFlip.cs              # DOTween 카드 플립
│   ├── Managers/                    # 8개 Manager
│   │   ├── NumGameManager.cs
│   │   ├── CardGameManager.cs
│   │   ├── VoteManager.cs
│   │   ├── VideoManager.cs
│   │   ├── SliderManager.cs
│   │   ├── SoundManager.cs
│   │   ├── IdleManager.cs
│   │   └── NumPad.cs
│   ├── Models/
│   │   ├── ECountry.cs
│   │   ├── CardDatabaseSO.cs
│   │   └── StringSpritePairContainerSO.cs
│   └── Views/                       # 11개 View
│       ├── BaseView.cs
│       ├── HomeView.cs
│       ├── ...                      # 화면별 View (10개)
│       └── VoteResultView.cs
└── SwipeUI/
    ├── SwipeUI.cs
    └── HoverDetector.cs
```

---

## 데이터 흐름

```
앱 시작
 └─► NavigationController.OnSingletonAwake()
      ├─► EnsureAllViewsInitialized()  (비활성 View 포함 11개 전부 초기화)
      └─► StateMachine 초기화 (11개 State 등록 → HomeState)

터치 입력
 └─► View.Action 이벤트 발생
      └─► State 핸들러 실행
           └─► Manager 로직 호출
                └─► 결과 이벤트 → State → View UI 업데이트
```

투표 데이터 영속화:

```json
// Application.persistentDataPath/vote_data.json
{ "egypt": 42, "china": 35, "roma": 28 }
```

---

## 변경 이력

| 날짜 | 내용 |
|------|------|
| 2026-03-13 | TECH_SPEC.md 기술 명세서 작성 |
| 2026-03-13 | 전체 View/State 주석 정비 - 한글 섹션 구분 주석, XML summary 주석 추가 |
| 2026-03-13 | NumGame 다른나라 버튼 → Select 영상 스킵 전환 |
| 2026-03-13 | SelectState 국가 선택 버튼 되감기 대응 |
| 2026-03-13 | DrawingView 국가별 날짜 이미지 미리보기 구현 |
| 2026-03-13 | Write↔Drawing 양방향 전환, 날짜 검증, ECountry 전달 체인 구성 |
| 2026-03-13 | 전체 네임스페이스 추가 (BusanMath.Core/FSM/States/Views/Controllers/Managers/Models) |
| 2026-03-13 | WriteView/WriteState ↔ DrawingView/DrawingState 리네임, ECountry enum 분리 |
| 2026-03-12 | Write2State/Write2View 추가 - 날짜 선택 UI |
| 2026-03-11 | BaseView에 EnsureInitialized() 추가 |
| 2026-03-11 | PagingTemplate 아키텍처 적용 - IState 5단계 생명주기, BaseState 제네릭화, StateMachine Init 1회 보장 |
| 2026-03-11 | FSM State 초기화/정리 로직 보완 - 이벤트 구독 해제 버그 수정, 리소스 정리 |
| 2026-03-11 | 전체 스크립트 주석 정비 - 한글 XML 주석 재작성, 불필요한 using 제거 |
