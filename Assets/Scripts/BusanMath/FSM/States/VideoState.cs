using UnityEngine;
using UnityEngine.Video;

public enum ECountry
{
    Egypt,
    China,
    Roma,
    MAX_CNT,
    None,
}

public class VideoState : BaseState<VideoState, VideoView>
{
    private ECountry _country;

    public ECountry Country
    {
        set { _country = value; }
    }

    public VideoState(VideoView view) : base(view) { }

    public override void Init()
    {
        base.Init();

        // 이벤트 구독 (최초 1회)
        _view._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _view._OnSkipButtonClicked += () => {
            VideoManager.Instance.Skip();
            _view._progressbar.SetValueWithoutNotify(VideoManager.Instance.Progress());
        };
    }

    public override void Enter()
    {
        base.Enter();
        _view.Show();

        // VideoManager 설정
        VideoManager.Instance.SetDisplay(_view._displayImage);
        string targetFileName = "";
        switch(_country)
        {
            case ECountry.Egypt:
                targetFileName = _view._fileNameEgypt;
                break;
            case ECountry.China:
                targetFileName = _view._fileNameChina;
                break;
            case ECountry.Roma:
                targetFileName = _view._fileNameRoma;
                break;
            default:
                break;
        }
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, targetFileName);

        // SliderManager 설정
        SliderManager.Instance.Slider = _view._progressbar;
        SliderManager.Instance.Player = VideoManager.Instance.Player;

        // Video 재생
        VideoManager.Instance.Play(filePath);

        // Video 재생 완료 콜백 등록
        VideoManager.Instance.Player.loopPointReached += OnVideoFinished;
    }

    public override void Update()
    {
        if (VideoManager.Instance.IsPlaying()
            && VideoManager.Instance.VideoLength() > 0
            && false == SliderManager.Instance.IsDragging)
        {
            _view._progressbar.SetValueWithoutNotify(VideoManager.Instance.Progress());
        }
    }

    public override void Exit()
    {
        base.Exit();

        VideoManager.Instance.Stop();
        VideoManager.Instance.Player.loopPointReached -= OnVideoFinished;

        _view.Hide();
    }

    public void OnVideoFinished(VideoPlayer vp)
    {
        NavigationController.Instance.GoToNumGameDescription(_country);
        _country = ECountry.None;
    }
}
