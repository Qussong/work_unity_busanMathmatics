using LS.DrawTexture.Runtime;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class DrawingView : BaseView
    {
        [Header("=== Drawing Board ===")]
        public GameObject _writeBoardContainer;
        public DrawTextureUI _drawTextureUI;

        [Header("=== Buttons ===")]
        public Button _homeButton;
        public Button _moveNextButton;

        public event Action _OnHomeButtonClicked;
        public event Action _OnMoveNextButtonClicked;

        protected override void Initialize()
        {
            _writeBoardContainer.SetActive(false);
        }

        protected override void BindUIEvent()
        {
            _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
            _moveNextButton.onClick.AddListener(() => _OnMoveNextButtonClicked?.Invoke());
        }
    }
}
