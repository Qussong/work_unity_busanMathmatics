using BusanMath.Core;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// 비디오 재생 슬라이더(프로그레스바) 드래그 관리
/// </summary>
public class SliderManager : MonoSingleton<SliderManager>
{
    private VideoPlayer _player;
    private Slider _slider;
    private bool _isDragging = false;

    public VideoPlayer Player
    {
        set { _player = value; }
    }

    public Slider Slider
    {
        set
        {
            _slider = value;

            // 슬라이더 값 변경 이벤트 등록
            _slider.onValueChanged.RemoveAllListeners();
            _slider.onValueChanged.AddListener(OnSliderValueChanged);

            // EventTrigger 생성 또는 참조
            EventTrigger trigger = _slider.GetComponent<EventTrigger>();
            if(null == trigger)
                trigger = _slider.gameObject.AddComponent<EventTrigger>();

            // BeginDrag 이벤트 등록
            bool hasBeginDrag = trigger.triggers.Any(e => e.eventID == EventTriggerType.BeginDrag);
            if(false == hasBeginDrag)
            {
                EventTrigger.Entry beginDrag = new EventTrigger.Entry();
                beginDrag.eventID = EventTriggerType.BeginDrag;
                beginDrag.callback.AddListener((data) => OnBeginDrag());
                trigger.triggers.Add(beginDrag);
            }

            // EndDrag 이벤트 등록
            bool hasEndDrag = trigger.triggers.Any(e => e.eventID == EventTriggerType.EndDrag);
            if(false == hasEndDrag)
            {
                EventTrigger.Entry endDrag = new EventTrigger.Entry();
                endDrag.eventID = EventTriggerType.EndDrag;
                endDrag.callback.AddListener((data) => OnEndDrag());
                trigger.triggers.Add(endDrag);
            }
        }
    }

    public bool IsDragging
    {
        get { return _isDragging; }
    }

    protected override void OnSingletonAwake()
    {
    }

    /// <summary>
    /// 슬라이더 드래그 중일 때 비디오 시간 이동
    /// </summary>
    private void OnSliderValueChanged(float value)
    {
        if(true == _isDragging)
        {
            if (null == _player) return;
            _player.time = value * _player.length;
        }
    }

    public void OnBeginDrag() => _isDragging = true;
    public void OnEndDrag() => _isDragging = false;
}
