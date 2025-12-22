using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

namespace SwipeUI
{
    public class SwipeUI : MonoBehaviour
    {
        /// <summary>
        /// Swipe UI
        /// </summary>
        [Header("=== Swipe UI ===")]
        [SerializeField] private Scrollbar _scrollbar;          // scrollbar의 위치를 바탕으로 현제 페이지 검사
        [SerializeField] private float _swipeTime = 0.2f;       // 페이지가 swipe 되는 시간
        [SerializeField] private float _swipeDistance = 50.0f;  // 페이지가 swipe 되기 위해 움직여야 하는 최소 거리

        private float[] _scrollPageValues;                      // 각 페이지의 위치 값 (범위 : 0.0 ~ 1.0)
        private float _valueDistance;                           // 각 페이지 사이의 거리
        private int _currentPage = 0;                           // 현재 페이지
        private int _maxPage = 0;                               // 최대 페이지
        private float _startTouchX;                             // 터치 시작 위치 
        private float _endTouchX;                               // 터치 종료 위치
        private bool _isSwipeMode = false;                      // 현재 swipe가 되고 있는지 체크

        [SerializeField] private GameObject[] _hoverContents;   // 현재 페이지에 해당하는 객체들
        private bool _isSwipeActive = false;    // 스크롤 측정 조건 만족여부 확인, 화면상에 SwipeUI가 복수개 존재하는경우 해당 객체 판별용으로 사용됨

        public int CurrentPage
        {
            get { return _currentPage; }
        }

        /// <summary>
        /// Circle Content
        /// </summary>
        [Header("=== Circle Content ===")]
        [SerializeField] private bool _bUseCircleContent = true;
        [SerializeField] private float _circleContentScale = 1.6f;  // 현재 페이지의 Circle 크기 (배율)
        [SerializeField] private Transform[] _circleContents;       // 현재 페이지를 나타내는 Circle Image UI들의 Transform

        public event Action _OnSwipeCompleted;                      // swipe 이후 후속처리를 위한 이벤트

        void Awake()
        {
            // 스크롤 되는 페이지의 각 value 값을 저장하는 배열 메모리 할당
            _scrollPageValues = new float[transform.childCount];

            // 스크롤 되는 페이지 사이의 거리
            _valueDistance = 1f / (_scrollPageValues.Length - 1f);
            Debug.Log("스크롤 되는 페이지 사이의 거리 : " + _valueDistance);

            // 스크롤 되는 패이지의 각 value 위치 설정 (0 <= value <= 1)
            for (int i = 0; i < _scrollPageValues.Length; ++i)
            {
                _scrollPageValues[i] = _valueDistance * i;
            }

            // 최대 페이지 수
            _maxPage = transform.childCount;

            // HoverDetector 추가
            if(0 < _hoverContents.Length)
            {
                foreach (GameObject obj in _hoverContents)
                    obj.AddComponent<HoverDetector>();
            }

        }

        private void Start()
        {
            // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정
            SetScrollBarValue(0);
        }

        private void SetScrollBarValue(int index)
        {
            // 매개변수로 받은 값을 현재 페이지 인덱스로 설정
            _currentPage = index;

            // 현재 페이지 인덱스에 해당하는 Scrollbar의 value값 설정
            _scrollbar.value = _scrollPageValues[index];
        }

        private void Update()
        {
            // 화면 제어
            UpdateInput();

            // 아래에 배치된 페이지 버튼 제어
            if (false == _bUseCircleContent) return;
            UpdateCircleContent();
        }

        private void UpdateInput()
        {
            if (true == _isSwipeMode)
                return;

            /// <summary>
            /// 전처리기 #if - #endif
            /// 프로그램 실행 전에 #if 조건에 만족하는 코드는 활성화하고,
            /// 조건에 만족하지 않는 코드는 비활성호해서 아예 실행되지 않는다.
            /// - #if UNITY_EDITOR : 현재 플레이 환경이 에디터인 경우
            /// - #if UNITY_ANDROID : 현재 플레이 환경이 안드로이드일 떄
            /// </summary>
#if UNITY_EDITOR
            // 마우스 왼쪽 버튼을 눌렀을 때 한번
            if (Input.GetMouseButtonDown(0)
                && true == _hoverContents[_currentPage].GetComponent<HoverDetector>().IsHover())
            {
                // 터치 시작 지점
                _startTouchX = Input.mousePosition.x;

                // 해당 객체에서 swipe 측정이 시작됨을 확인
                _isSwipeActive = true;
            }
            // 마우스 왼쪽 버튼을 땠을 때
            else if (Input.GetMouseButtonUp(0)
                && true == _isSwipeActive)
            {
                // 터치 종료 지점
                _endTouchX = Input.mousePosition.x;

                // Swipe 수행 여부 판별 및 실행
                UpdateSwipe();

                // swipe 여부 확인 후 해제
                _isSwipeActive = false;
            }
#endif

#if UNITY_ANDROID
            // 안드로이드 환경인 경우...
#endif
            
        }

        private void UpdateSwipe()
        {
            // 너무 작은 거리를 움직였을 경우 Swipe 동작 안함
            if (Mathf.Abs(_startTouchX - _endTouchX) < _swipeDistance)
            {
                // 원래 페이지로 swipe 이동
                StartCoroutine(OnSwipeOneStep(_currentPage));
                return;
            }

            // Swipe 방향 판별
            // 1. true = 시작 지점의 x 값이 더 큰 경우 -> 왼쪽으로 Swipe 진행
            // 2. false = 시작 지점의 x 값이 더 작은 경우 -> 오른쪽으로 Swipe 진행
            bool isLeft = _startTouchX < _endTouchX ? true : false;

            // 이동 방향이 왼쪽인 경우
            if (true == isLeft)
            {
                // 현재 페이지가 왼쪽 끝인 경우 종료
                if (0 == _currentPage)
                    return;

                // 왼쪽으로 이동을 위해 현제 페이지를 1 감소
                --_currentPage;
            }
            else
            {
                // 현재 페이지가 오른쪽 끝인 경우 종료
                if (_maxPage - 1 == _currentPage) 
                    return;

                // 오른쪽으로 이동을 위해 현제 페이지를 1 증가
                ++_currentPage;
            }

            // 새롭게 설정된 현재 페이지로 swipe 이동
            StartCoroutine(OnSwipeOneStep(_currentPage));

            // 스와프 이후 후속처리
            _OnSwipeCompleted?.Invoke();
        }

        public void AutoSwipe(bool isLeft)
        {
            // 왼쪽 Swipe
            if(true == isLeft)
            {
                // 현재 페이지가 왼쪽 끝인 경우 종료
                if (0 == _currentPage)
                    return;

                // 왼쪽으로 이동을 위해 현제 페이지를 1 감소
                --_currentPage;
            }
            // 오른쪽 Swipe
            else
            {
                // 현재 페이지가 오른쪽 끝인 경우 종료
                if (_maxPage - 1 == _currentPage)
                    return;

                // 오른쪽으로 이동을 위해 현제 페이지를 1 증가
                ++_currentPage;
            }

            // 새롭게 설정된 현재 페이지로 swipe 이동
            StartCoroutine(OnSwipeOneStep(_currentPage));

            // 스와프 이후 후속처리
            _OnSwipeCompleted?.Invoke();
        }

        /// <summary>
        /// 페이지를 한 장 옆으로 넘기는 Swipe 효과 재생
        /// </summary>
        private IEnumerator OnSwipeOneStep(int index)
        {
            float start = _scrollbar.value; // 현재 스크롤바의 위치 (범위 : 0 ~ 1)
            float current = 0;              // 누적시간
            float percent = 0;              // swipe 진행도
            float end = _scrollPageValues[index];

            // swipe 모드 ON (추가 입력 방지)
            _isSwipeMode = true;

            // 스크롤 진행
            while (1 > percent)
            {
                current += Time.deltaTime;
                percent = current / _swipeTime;

                // 현재 스크롤바의 위치(start) 부터 목적지(end) 까지 _swipeTime 시간동안 화면 스크롤해서 이동
                _scrollbar.value = Mathf.Lerp(start, end, percent);

                yield return null;
            }

            // swipe 모드 OFF (추가 입력 받을 준비)
            _isSwipeMode = false;
        }

        private void UpdateCircleContent()
        {
            for(int i = 0; i < _scrollPageValues.Length; ++i)
            {
                _circleContents[i].localScale = Vector2.one;
                //_circleContents[i].GetComponent<Image>().color = Color.white;

                // 페이지의 절반을 넘어가면 현재 페이지 원을 바꾸도록
                if(_scrollbar.value < _scrollPageValues[i] + (_valueDistance / 2)
                    && _scrollbar.value > _scrollPageValues[i] - (_valueDistance / 2))
                {
                    _circleContents[i].localScale = Vector2.one * _circleContentScale;
                    //_circleContents[i].GetComponent<Image>().color = Color.black;
                }
            }
        }
    }
}


