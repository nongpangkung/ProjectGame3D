using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FN
{
    public class Tutorail : MonoBehaviour
    {
        public TextMeshProUGUI tutorialText;
        public Image tutorialImage;
        public Button nextButton;
        public Button closeButton;

        public string[] steps;
        public Sprite[] images;

        private int currentStep = 0;

        void Start()
        {
            UpdateTutorial();
            nextButton.onClick.AddListener(NextStep);
            closeButton.onClick.AddListener(CloseTutorial);
        }

        void UpdateTutorial()
        {
            tutorialText.text = steps[currentStep];
            tutorialImage.sprite = images[currentStep];
        }

        void NextStep()
        {
            currentStep++;
            if (currentStep >= steps.Length)
            {
                // The tutorial is over, so you can perform any actions you need here
                // such as showing a message or unlocking content
                Debug.Log("Tutorial is over!");
                CloseTutorial();
                return;
            }
            UpdateTutorial();
        }

        void CloseTutorial()
        {
            gameObject.SetActive(false);
            // You can perform any actions you need here when the tutorial is closed
        }
    }
}
