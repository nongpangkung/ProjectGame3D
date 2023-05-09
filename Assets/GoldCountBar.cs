using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FN
{
    public class GoldCountBar : MonoBehaviour
    {
        public TextMeshProUGUI goldCountText;

        public void SetGoldCountText(int goldCount)
        {
            goldCountText.text = goldCount.ToString();
        }
    }
}
