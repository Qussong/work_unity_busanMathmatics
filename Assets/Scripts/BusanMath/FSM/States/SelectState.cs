using DG.Tweening;
using UnityEngine;
using UnityEngine.Video;

public class SelectState : BaseState
{
    private SelectView _selectView;
    private float fadeDuration = 0.5f;
    private bool bFade = false;

    public SelectState(SelectView view)
    {
        _selectView = view;

        // 이벤트 등록
        _selectView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _selectView._OnSkipButtonClicked += () => {
            VideoManager.Instance.Skip();
            _selectView._progressbar.SetValueWithoutNotify(VideoManager.Instance.Progress()); 
        };
        _selectView._OnEgyptButtonClicked += () => { NavigationController.Instance.GoToVideo(ECountry.Egypt); };
        _selectView._OnChinaButtonClicked += () => { NavigationController.Instance.GoToVideo(ECountry.China); };
        _selectView._OnRomaButtonClicked += () => { NavigationController.Instance.GoToVideo(ECountry.Roma); };
        
    }

    public override void Enter()
    {
        Debug.Log("[SelectState] Enter");
        _selectView.Show();

        // VideoManager 세팅
        VideoManager.Instance.SetDisplay(_selectView._displayImage);    // 렌더 타겟 이미지 설정
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, _selectView._fileName);   // 영상 경로 

        // SliderManager 세팅
        SliderManager.Instance.Slider = _selectView._progressbar;
        SliderManager.Instance.Player = VideoManager.Instance.Player;

        // Video 재생
        VideoManager.Instance.Play(filePath);   // 영상 로드 및 재생

        // 버튼 숨기기
        InitButtons();
    }

    public override void Update()
    {
        // 진행률 표시
        if(VideoManager.Instance.IsPlaying() 
            && VideoManager.Instance.VideoLength() > 0
            && false == SliderManager.Instance.IsDragging)
        {
            // SetValueWithoutNotify() : 값을 설정하지만 콜백을 트리거하지 않는다.
            // 사용자의 입력처럼 이벤트 처리가 필요한 경우 slider 의 value 를 직접 변경하지만,
            // 코드에서 UI의 동기화만 필요한 경우 SetValueWithoutNotify() 를 사용한다.
            _selectView._progressbar.SetValueWithoutNotify(VideoManager.Instance.Progress());
        }

        if(false == bFade && VideoManager.Instance.Progress() > 0.9f)
        {
            FadInButtons();
        }
    }

    public override void Exit()
    {
        Debug.Log("[SelectState] Eixt");

        // 영상 재생 멈춤 및 초기화
        VideoManager.Instance.Stop();

        // 버튼들의 알파값 0f
        InitButtons();

        _selectView.Hide();
    }

    private void InitButtons()
    {
        bFade = false;

        CanvasGroup canvasGroup = _selectView._buttonContainer.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        _selectView._buttonContainer.SetActive(false);
    }

    private void FadInButtons()
    {
        bFade = true;

        _selectView._buttonContainer.SetActive(true);

        CanvasGroup canvasGroup = _selectView._buttonContainer.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(1f, fadeDuration);
    }
}
