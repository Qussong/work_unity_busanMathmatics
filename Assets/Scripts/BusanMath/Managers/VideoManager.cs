using BusanMath.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BusanMath.Managers
{
    /// <summary>
    /// 비디오 재생을 담당하는 싱글톤 매니저
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

        public void Play(string filePath)
        {
            _player.source = VideoSource.Url;
            _player.url = filePath;
            _player.Prepare();
        }

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

        public double VideoLength()
        {
            if (null == _player) return 0;
            return _player.length;
        }

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

        public void Skip()
        {
            SetPlayerTime((float)(VideoManager.Instance.VideoLength() * 0.95f));
        }

        protected override void OnDestroy()
        {
            if (null != _renderTexture) _renderTexture.Release();
        }
    }
}
