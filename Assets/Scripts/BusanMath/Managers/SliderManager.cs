using BusanMath.Core;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

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

            // 슬라이더 드래그 이벤트 등록
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

    /// <summary>
    /// 싱글톤 초기화 시 호출되는 가상 메서드
    /// 서브클래스에서 오버라이드하여 초기화 로직 구현 
    /// </summary>
    protected override void OnSingletonAwake()
    {
        //
    }

    private void OnSliderValueChanged(float value)
    {
        if(true == _isDragging)
        {
            if (null == _player) return;

            // 슬라이더 값에 해당하는 시간으로 이동
            _player.time = value * _player.length;
        }
    }

    public void OnBeginDrag() => _isDragging = true;
    public void OnEndDrag() => _isDragging = false;


}
