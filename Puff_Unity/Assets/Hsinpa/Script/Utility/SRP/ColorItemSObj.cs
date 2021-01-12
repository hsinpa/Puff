using UnityEngine;

namespace Hsinpa.Utility {
    [CreateAssetMenu(fileName = "ColorItemSObj", menuName = "ScriptableObjects/ColorSetting", order = 1)]
    public class ColorItemSObj : ScriptableObject
    {
        public Color TextNormalColor;

        public Color TextHighlightColor;

        public Color TextUnSelectedColor;
    }
}
