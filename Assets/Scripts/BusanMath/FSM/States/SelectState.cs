using DG.Tweening;
using UnityEngine;
using BusanMath.FSM;
using BusanMath.Views;
using BusanMath.Controllers;
using BusanMath.Managers;
using BusanMath.Models;

namespace BusanMath.FSM.States
{
    public class SelectState : BaseState<SelectState, SelectView>
    {
        private float fadeDuration = 0.5f;  // 버튼 페이드 애니메이션 시간
        private bool bFade = false;          // 버튼이 현재 표시 중인지 여부
        private bool _skipToButtons = false; // 영상 스킵하여 바로 버튼 표시 여부

        /// <summary>
        /// true로 설정 시 영상을 95%로 스킵하여 바로 국가 선택 버튼을 표시한다.
        /// </summary>
        public bool SkipToButtons
        {
            set { _skipToButtons = value; }
        }

        public SelectState(SelectView view) : base(view) { }

        public override void Init()
        {
            base.Init();

            // 이벤트 구독 (최초 1회)
            _view._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
            _view._OnSkipButtonClicked += () => {
                VideoManager.Instance.Skip();
                _view._progressbar.SetValueWithoutNotify(VideoManager.Instance.Progress());
            };
            _view._OnEgyptButtonClicked += () => { NavigationController.Instance.GoToVideo(ECountry.Egypt); };
            _view._OnChinaButtonClicked += () => { NavigationController.Instance.GoToVideo(ECountry.China); };
            _view._OnRomaButtonClicked += () => { NavigationController.Instance.GoToVideo(ECountry.Roma); };
        }

        public override void Enter()
        {
            base.Enter();
            _view.Show();

            // VideoManager 설정
            VideoManager.Instance.SetDisplay(_view._displayImage);
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, _view._fileName);

            // SliderManager 설정
            SliderManager.Instance.Slider = _view._progressbar;
            SliderManager.Instance.Player = VideoManager.Instance.Player;

            // Video 재생
            VideoManager.Instance.Play(filePath);

            // 국가 선택 버튼 초기화 (숨김)
            InitButtons();
        }

        public override void Update()
        {
            // 영상 준비 완료 후 스킵 플래그가 설정되어 있으면 95%로 점프
            if (_skipToButtons && VideoManager.Instance.IsPlaying() && VideoManager.Instance.VideoLength() > 0)
            {
                VideoManager.Instance.Skip();
                _view._progressbar.SetValueWithoutNotify(VideoManager.Instance.Progress());
                _skipToButtons = false;
            }

            // 드래그 중이 아닐 때만 진행바 자동 갱신
            if(VideoManager.Instance.IsPlaying()
                && VideoManager.Instance.VideoLength() > 0
                && false == SliderManager.Instance.IsDragging)
            {
                _view._progressbar.SetValueWithoutNotify(VideoManager.Instance.Progress());
            }

            // 영상 90% 이상 → 국가 선택 버튼 표시, 되감기 시 다시 숨김
            float progress = VideoManager.Instance.Progress();
            if (!bFade && progress > 0.9f)
            {
                FadInButtons();
            }
            else if (bFade && progress <= 0.9f)
            {
                FadeOutButtons();
            }
        }

        public override void Exit()
        {
            base.Exit();

            VideoManager.Instance.Stop();
            InitButtons();
            _skipToButtons = false;

            _view.Hide();
        }

        /// <summary>
        /// 국가 선택 버튼을 즉시 숨김 (알파 0, 비활성화)
        /// </summary>
        private void InitButtons()
        {
            bFade = false;

            CanvasGroup canvasGroup = _view._buttonContainer.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;

            _view._buttonContainer.SetActive(false);
        }

        /// <summary>
        /// 국가 선택 버튼 페이드아웃 (되감기 시 호출)
        /// </summary>
        private void FadeOutButtons()
        {
            CanvasGroup canvasGroup = _view._buttonContainer.GetComponent<CanvasGroup>();
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                _view._buttonContainer.SetActive(false);
                bFade = false;
            });
        }

        /// <summary>
        /// 국가 선택 버튼 페이드인 (영상 90% 도달 시 호출)
        /// </summary>
        private void FadInButtons()
        {
            bFade = true;

            _view._buttonContainer.SetActive(true);

            CanvasGroup canvasGroup = _view._buttonContainer.GetComponent<CanvasGroup>();
            canvasGroup.DOFade(1f, fadeDuration);
        }
    }
}
