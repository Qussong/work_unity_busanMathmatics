using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BusanMath.FSM;
using BusanMath.Views;
using BusanMath.Controllers;
using BusanMath.Managers;
using BusanMath.Models;

namespace BusanMath.FSM.States
{
    public class DrawingState : BaseState<DrawingState, DrawingView>
    {
        private ECountry _country;
        private int _selectedYear;
        private int _selectedMonth;
        private int _selectedDay;

        public ECountry Country
        {
            set { _country = value; }
        }

        public int SelectedYear
        {
            set { _selectedYear = value; }
        }

        public int SelectedMonth
        {
            set { _selectedMonth = value; }
        }

        public int SelectedDay
        {
            set { _selectedDay = value; }
        }

        public DrawingState(DrawingView view) : base(view) { }

        public override void Init()
        {
            base.Init();

            _view._OnReselectButtonClicked += () => NavigationController.Instance.GoToWrite(_country);
            _view._OnMoveNextButtonClicked += () => NavigationController.Instance.GoToVoteResult();
        }

        public override void Enter()
        {
            base.Enter();
            _view.Show();
            _view._writeBoardContainer.SetActive(true);
            ApplyDatePreview();
        }

        private void ApplyDatePreview()
        {
            var (yearSprites, monthSprites, daySprites) = GetSpritesByCountry(_country);

            int yearIndex = _selectedYear - 1980;
            int monthIndex = _selectedMonth - 1;
            int dayIndex = _selectedDay - 1;

            if (yearSprites != null && yearIndex >= 0 && yearIndex < yearSprites.Count)
                ApplySprite(_view._yearPreviewImage, yearSprites[yearIndex]);

            if (monthSprites != null && monthIndex >= 0 && monthIndex < monthSprites.Count)
                ApplySprite(_view._monthPreviewImage, monthSprites[monthIndex]);

            if (daySprites != null && dayIndex >= 0 && dayIndex < daySprites.Count)
                ApplySprite(_view._dayPreviewImage, daySprites[dayIndex]);
        }

        private void ApplySprite(Image image, Sprite sprite)
        {
            image.sprite = sprite;
            image.SetNativeSize();

            switch (_country)
            {
                case ECountry.Egypt:
                    image.rectTransform.localScale = Vector3.one / 3f;
                    break;
                case ECountry.China:
                    image.rectTransform.localScale = Vector3.one / 4.5f;
                    break;
                case ECountry.Roma:
                    image.rectTransform.localScale = Vector3.one / 3f;
                    break;
                default:
                    image.rectTransform.localScale = Vector3.one;
                    break;
            }

            var color = image.color;
            color.a = 100f / 255f;
            image.color = color;
        }

        private (List<Sprite>, List<Sprite>, List<Sprite>) GetSpritesByCountry(ECountry country)
        {
            switch (country)
            {
                case ECountry.Egypt:
                    return (_view._egyptYearSprites, _view._egyptMonthSprites, _view._egyptDaySprites);
                case ECountry.China:
                    return (_view._chinaYearSprites, _view._chinaMonthSprites, _view._chinaDaySprites);
                case ECountry.Roma:
                    return (_view._romaYearSprites, _view._romaMonthSprites, _view._romaDaySprites);
                default:
                    return (null, null, null);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _view._drawTextureUI.Clear();
            _view._writeBoardContainer.SetActive(false);
            _view.Hide();
        }
    }
}
