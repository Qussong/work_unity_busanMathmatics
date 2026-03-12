using DG.Tweening;
using UnityEngine;

public class SelectState : BaseState<SelectState, SelectView>
{
    private float fadeDuration = 0.5f;
    private bool bFade = false;

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

        // 버튼 초기화
        InitButtons();
    }

    public override void Update()
    {
        if(VideoManager.Instance.IsPlaying()
            && VideoManager.Instance.VideoLength() > 0
            && false == SliderManager.Instance.IsDragging)
        {
            _view._progressbar.SetValueWithoutNotify(VideoManager.Instance.Progress());
        }

        if(false == bFade && VideoManager.Instance.Progress() > 0.9f)
        {
            FadInButtons();
        }
    }

    public override void Exit()
    {
        base.Exit();

        VideoManager.Instance.Stop();
        InitButtons();

        _view.Hide();
    }

    private void InitButtons()
    {
        bFade = false;

        CanvasGroup canvasGroup = _view._buttonContainer.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        _view._buttonContainer.SetActive(false);
    }

    private void FadInButtons()
    {
        bFade = true;

        _view._buttonContainer.SetActive(true);

        CanvasGroup canvasGroup = _view._buttonContainer.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(1f, fadeDuration);
    }
}
