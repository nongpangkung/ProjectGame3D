using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class BonfireLitPopUPUI : MonoBehaviour
    {
        CanvasGroup canvas;

        private void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
        }

        public void DisplayBonfireLitPopUp()
        {
            StartCoroutine(FadeInPopUp());
        }

        IEnumerator FadeInPopUp()
        {
            gameObject.SetActive(true);

            for (float fade = 0.05f; fade < 1; fade = fade + 0.05f)
            {
                canvas.alpha = fade;

                if (fade > 0.9f)
                {
                    StartCoroutine(FadeOutPopUp());
                }

                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator FadeOutPopUp()
        {
            //We wait 2 seconds before we begin to fade the pop up out
            yield return new WaitForSeconds(2);

            for (float fade = 1f; fade > 0; fade = fade - 0.05f)
            {
                canvas.alpha = fade;

                if (fade <= 0.05f)
                {
                    gameObject.SetActive(false);
                }

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
