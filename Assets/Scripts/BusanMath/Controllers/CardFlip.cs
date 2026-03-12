using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BusanMath.Controllers
{
    /// <summary>
    /// 카드 뒤집기 애니메이션 및 클릭 처리
    /// DOTween을 사용한 Y축 회전 애니메이션
    /// </summary>
    public class CardFlip : MonoBehaviour, IPointerClickHandler
    {
        public int _cardIdx = -1;
        private Image _cardImage;
        public Sprite _frontSprite;
        public Sprite _backSprite;
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
            if (true == _isFront) return;
            if (true == _isFlipping) return;

            _OnClickCard.Invoke(_cardIdx);
            Flip();
        }

        public void Flip()
        {
            transform.DORotate(new Vector3(0, 90, 0), _duration / 2)
                .OnComplete(() =>
                {
                    _isFront = !_isFront;
                    _cardImage.sprite = _isFront ? _frontSprite : _backSprite;
                    transform
                        .DORotate(new Vector3(0, 0, 0), _duration / 2)
                        .OnComplete(() =>
                        {
                            _isFlipping = false;
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
            transform.DOKill();
            StopAllCoroutines();

            _cardIdx = -1;
            _frontSprite = null;
            _isFlipping = false;
            _isFront = false;

            transform.rotation = Quaternion.identity;

            _cardImage.raycastTarget = true;
            _cardImage.color = new Color(1f, 1f, 1f, 1f);
            _cardImage.sprite = _backSprite;
        }
    }
}
