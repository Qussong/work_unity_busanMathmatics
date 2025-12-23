using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour, IPointerClickHandler
{
    public int _cardIdx = -1;
    private Image _cardImage;
    public Sprite _frontSprite;  // 앞면
    public Sprite _backSprite;   // 뒷면
    private float _duration = 0.3f;

    private bool _isFront = false;

    private bool _isFlipping = false;

    public event Action<int> _OnClickCard;

    void Awake()
    {
        _cardImage = GetComponent<Image>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 이미 앞면인 상태면 선택 안됨
        if (true == _isFront) return;

        // 카드가 뒤집히는 중엔 선택 안됨
        if (true == _isFlipping) return;

        _OnClickCard.Invoke(_cardIdx);
        Flip();
    }

    public void Flip()
    {
        // Y축으로 90도 회전 → 이미지 교체 → 다시 90도 회전
        transform.DORotate(new Vector3(0, 90, 0), _duration / 2)
            .OnComplete(() =>
            {
                _isFront = !_isFront;
                _cardImage.sprite = _isFront ? _frontSprite : _backSprite;
                transform
                    .DORotate(new Vector3(0, 0, 0), _duration / 2)
                    .OnComplete(() =>
                    {
                        _isFlipping = false; // 모든 회전 완료
                    });
            });
    }

    public void LateFlip(float time)
    {
        StartCoroutine(LateFlipCoroutine(time));
    }

    private IEnumerator LateFlipCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        Flip();
    }

    public void Restore()
    {
        _cardIdx = -1;
        _frontSprite = null;

        if (false == _isFront) return;
        _isFront = false;
        _cardImage.raycastTarget = true;
        _cardImage.color = new Color(1f, 1f, 1f, 1f);
        _cardImage.sprite = _backSprite;
    }

}
