using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

namespace Puff.View
{
    public class PuffMsgReivewModule : MonoBehaviour
    {
        [SerializeField]
        private Button[] starIcons;

        [SerializeField]
        private Sprite lightStar;
        
        [SerializeField]
        private Sprite darkStar;

        private int maxStarCount;
        public int score {
            get {
               return starIcons.Count(x => x.image.sprite == lightStar);
            }
        }

        public void Start()
        {
            maxStarCount = starIcons.Length;
            RegisterStarEvent();
        }

        public void SetScore(int score) {
            for (int i = 0; i < maxStarCount; i++) {
                Sprite chooseSprite = (score > i) ? lightStar : darkStar;
                starIcons[i].image.sprite = chooseSprite;
            }
        }

        private void RegisterStarEvent() {
            foreach (Button starBtn in starIcons) {

                starBtn.onClick.RemoveAllListeners();

                starBtn.onClick.AddListener(() =>
                {
                    this.OnStarBtnClick(starBtn);
                });
            }
        }

        private void OnStarBtnClick(Button starBtn) {

            int index = starBtn.transform.GetSiblingIndex();
            int newScore = index + 1;

            if (starBtn.image.sprite == lightStar && index+1 == score)
                newScore = index;

            SetScore(newScore);
        }

    }
}