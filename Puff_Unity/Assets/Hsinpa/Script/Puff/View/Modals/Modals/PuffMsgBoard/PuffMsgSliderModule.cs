using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class PuffMsgSliderModule : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private Text sliderField;

        public float sliderValue => slider.value;

        public void SetSlider(int defaultValue, int minValue, int maxValue, bool isWholeNumber, System.Action<float> sliderCallback) {
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = isWholeNumber;
            slider.value = defaultValue;

            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener((float x) => sliderCallback(x));
        }

        public void SetSliderField(string p_string) {
            sliderField.text = p_string;
        }

    }
}