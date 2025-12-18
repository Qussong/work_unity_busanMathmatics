using BusanMath.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI;

public class VideoManager : MonoSingleton<VideoManager>
{
    private VideoPlayer _player;
    private RawImage _display;              // 비디오를 표시할 UI RawImage
    private RenderTexture _renderTexture;   // 비디오 프레임을 렌더링할 텍스처

    public VideoPlayer Player
    {
        get { return _player; }
    }

    /// <summary>
    /// 싱글톤 초기화 시 호출되는 가상 메서드
    /// 서브클래스에서 오버라이드하여 초기화 로직 구현 
    /// </summary>
    protected override void OnSingletonAwake() 
    {
        // VideoPlayer 컴포넌트가 존재하지 않으면 최초 1회 생성
        if (null == _player)
        {
            _player = gameObject.AddComponent<VideoPlayer>();

            // 자동 재생 비활성화 (Prepare 완료 후 수동으로 재생)
            _player.playOnAwake = false;

            // 비디오 준비 완료 시 호출될 콜백 등록
            _player.prepareCompleted += OnPrepared;
        }
    }

    public void SetDisplay(RawImage display)
    {
        _display = display;
    }

    /// <summary>
    /// 지정된 경로의 비디오를 재생
    /// </summary>
    /// <param name="filePath">
    /// StreamingAssets 폴더
    /// System.IO.Path.Combine(Application.streamingAssetsPath, "sample.mp4")
    /// </param>
    public void Play(string filePath)
    {
        // 비디오 소스를 URL 모드로 설정
        _player.source = VideoSource.Url;

        // 재생할 비디오 경로 설정
        _player.url = filePath;

        // 비디오 로딩 및 디코딩 준비 시작
        _player.Prepare();
    }

    /// <summary>
    /// 비디오 준비 완료 시 호출되는 콜백
    /// RenderTexture 를 생성하고 실제 재생을 시작
    /// </summary>
    private void OnPrepared(VideoPlayer vp)
    {
        // 기존 RenderTexture 가 있으면 메모리 해제
        if (null != _renderTexture) _renderTexture.Release();

        // 비디오 해상도에 맞는 새로운 RenderTexture 생성
        _renderTexture = new RenderTexture((int)vp.width, (int)vp.height, 0);

        // VideoPlayer 의 출력 대상을 RenderTexture 로 설정
        vp.targetTexture = _renderTexture;

        // RawImage에 RenderTexture 연결하여 화면에 표시
        _display.texture = _renderTexture;

        // 비디오 재생 시작
        vp.Play();
    }

    public void Stop() => _player?.Stop();
    public void Pause() => _player?.Pause();

    /// <summary>
    /// 비디오 재생 여부 확인
    /// </summary>
    public bool IsPlaying()
    {
        if (null == _player) return false;
        return _player.isPlaying;
    }

    /// <summary>
    /// 전체 영상 길이 (초)
    /// </summary>
    /// <returns></returns>
    public double VideoLength()
    {
        if (null == _player) return 0;
        return _player.length;
    }

    /// <summary>
    /// 진행률 (0.0 ~ 1.0)
    /// </summary>
    /// <returns></returns>
    public float Progress()
    {
        if (null == _player) return 0;
        return (float)(_player.time / _player.length);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetPlayerTime(float value)
    {
        if (null == _player) return;
        _player.time = value;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void Skip()
    {
        // 자연스러운 배경을 위해 95% 까지만 스킵
        SetPlayerTime((float)(VideoManager.Instance.VideoLength() * 0.95f));
    }

    protected override void OnDestroy()
    {
        if (null != _renderTexture) _renderTexture.Release();
    }

}
