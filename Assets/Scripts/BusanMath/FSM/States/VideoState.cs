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

public class VideoState : BaseState
{
    private VideoView _videoView;
    private ECountry _country;

    public ECountry Country
    {
        set
        {
            _country = value;
        }
    }

    public VideoState(VideoView view)
    {
        _videoView = view;

        // 이벤트 등록
        _videoView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _videoView._OnSkipButtonClicked += () => {
            VideoManager.Instance.Skip();
            _videoView._progressbar.SetValueWithoutNotify(VideoManager.Instance.Progress());
        };
        
    }

    public override void Enter()
    {
        Debug.Log("[VideoState] Enter");
        _videoView.Show();

        // VideoManager 세팅
        VideoManager.Instance.SetDisplay(_videoView._displayImage);    // 렌더 타겟 이미지 설정
        string targetFileName = "";
        switch(_country)
        {
            case ECountry.Egypt:
                targetFileName = _videoView._fileNameEgypt;
                break;
            case ECountry.China:
                targetFileName = _videoView._fileNameChina;
                break;
            case ECountry.Roma:
                targetFileName = _videoView._fileNameRoma;
                break;
            default:
                break;
        }
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, targetFileName);   // 영상 경로 

        // SliderManager 세팅
        SliderManager.Instance.Slider = _videoView._progressbar;
        SliderManager.Instance.Player = VideoManager.Instance.Player;

        // Video 재생
        VideoManager.Instance.Play(filePath);   // 영상 로드 및 재생

        // Video 재생 완료시 호출될 콜백 함수 등록
        VideoManager.Instance.Player.loopPointReached += OnVideoFinished;
    }

    public override void Update()
    {
        // 진행률 표시
        if (VideoManager.Instance.IsPlaying()
            && VideoManager.Instance.VideoLength() > 0
            && false == SliderManager.Instance.IsDragging)
        {
            _videoView._progressbar.SetValueWithoutNotify(VideoManager.Instance.Progress());
        }
    }

    public override void Exit()
    {
        Debug.Log("[VideoState] Eixt");

        // 영상 재생 멈춤 및 초기화
        VideoManager.Instance.Stop();

        // 콜백 함수 해제
        VideoManager.Instance.Player.loopPointReached -= OnVideoFinished;

        _videoView.Hide();
    }

    public void OnVideoFinished(VideoPlayer vp)
    {
        NavigationController.Instance.GoToNumGameDescription(_country);

        // 국가 선택 기록 초기화
        _country = ECountry.None;
    }
}
