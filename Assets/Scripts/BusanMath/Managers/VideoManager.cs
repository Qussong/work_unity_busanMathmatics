using BusanMath.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// 비디오 재생을 담당하는 싱글톤 매니저
/// VideoPlayer, RenderTexture 생성 및 관리
/// </summary>
public class VideoManager : MonoSingleton<VideoManager>
{
    private VideoPlayer _player;
    private RawImage _display;
    private RenderTexture _renderTexture;

    public VideoPlayer Player
    {
        get { return _player; }
    }

    protected override void OnSingletonAwake()
    {
        if (null == _player)
        {
            _player = gameObject.AddComponent<VideoPlayer>();
            _player.playOnAwake = false;
            _player.prepareCompleted += OnPrepared;
        }
    }

    public void SetDisplay(RawImage display)
    {
        _display = display;
    }

    /// <summary>
    /// 비디오 파일 경로를 받아 재생 준비
    /// </summary>
    /// <param name="filePath">StreamingAssets 기준 전체 경로</param>
    public void Play(string filePath)
    {
        _player.source = VideoSource.Url;
        _player.url = filePath;
        _player.Prepare();
    }

    /// <summary>
    /// 비디오 준비 완료 콜백
    /// RenderTexture 생성 후 재생 시작
    /// </summary>
    private void OnPrepared(VideoPlayer vp)
    {
        if (null != _renderTexture) _renderTexture.Release();

        _renderTexture = new RenderTexture((int)vp.width, (int)vp.height, 0);
        vp.targetTexture = _renderTexture;
        _display.texture = _renderTexture;

        vp.Play();
    }

    public void Stop() => _player?.Stop();
    public void Pause() => _player?.Pause();

    public bool IsPlaying()
    {
        if (null == _player) return false;
        return _player.isPlaying;
    }

    /// <summary>
    /// 전체 비디오 길이 (초)
    /// </summary>
    public double VideoLength()
    {
        if (null == _player) return 0;
        return _player.length;
    }

    /// <summary>
    /// 재생 진행률 (0.0 ~ 1.0)
    /// </summary>
    public float Progress()
    {
        if (null == _player) return 0;
        return (float)(_player.time / _player.length);
    }

    public void SetPlayerTime(float value)
    {
        if (null == _player) return;
        _player.time = value;
    }

    /// <summary>
    /// 비디오 95% 지점으로 스킵
    /// </summary>
    public void Skip()
    {
        SetPlayerTime((float)(VideoManager.Instance.VideoLength() * 0.95f));
    }

    protected override void OnDestroy()
    {
        if (null != _renderTexture) _renderTexture.Release();
    }
}
