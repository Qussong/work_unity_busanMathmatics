using System;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class VideoView : BaseView
    {
        // ── 영상 파일 ─────────────────────────────────
        [Header("=== 영상 파일 ===")]
        [SerializeField] public string _fileNameEgypt;     // 이집트 교육 영상 파일명 (StreamingAssets 기준)
        [SerializeField] public string _fileNameChina;     // 중국 교육 영상 파일명
        [SerializeField] public string _fileNameRoma;      // 로마 교육 영상 파일명

        // ── 영상 표시 ─────────────────────────────────
        [Header("=== 영상 표시 ===")]
        [SerializeField] public RawImage _displayImage;    // 영상 출력 대상 이미지

        // ── 상단 UI ───────────────────────────────────
        [Header("=== 상단 UI ===")]
        [SerializeField] public Button _homeButton;        // 홈 이동 버튼
        [SerializeField] public Image _skipImage;          // 스킵 버튼 이미지
        [SerializeField] public Button _skipButton;        // 영상 스킵 버튼

        // ── 진행바 ────────────────────────────────────
        [Header("=== 진행바 ===")]
        [SerializeField] public Slider _progressbar;       // 영상 재생 진행바 (드래그로 탐색 가능)

        // ── 이벤트 ────────────────────────────────────
        public event Action _OnHomeButtonClicked;          // 홈 버튼 클릭 시 발생
        public event Action _OnSkipButtonClicked;          // 스킵 버튼 클릭 시 발생

        protected override void Initialize()
        {
        }

        protected override void BindUIEvent()
        {
            _homeButton?.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
            _skipButton?.onClick.AddListener(() => _OnSkipButtonClicked?.Invoke());
        }
    }
}
