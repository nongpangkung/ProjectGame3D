using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FN
{
    public class CurrentPotions : MonoBehaviour
    {
        PlayerManager player;
        public TextMeshProUGUI currentHealthPotions;

        public List<FlaskItem> flask;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        private void Update()
        {
            if (flask.Count > 0 && currentHealthPotions != null)
            {
                int totalHealthPotions = 0;
                foreach (FlaskItem item in flask)
                {
                    totalHealthPotions += item.currentItemAmount;
                }
                currentHealthPotions.text = totalHealthPotions.ToString();
            }
        }

        public void SetPotionCountText(int currentHealth)
        {
            currentHealthPotions.text = currentHealth.ToString();
        }

        public void SetCurrent(List<FlaskItem> health)
        {
            for (int i = 0; i < health.Count; i++)
            {
                FlaskItem flaskItem = health[i];

                int currentHealth = int.Parse(currentHealthPotions.text);
                flaskItem.currentItemAmount = currentHealth;
            }

            SetPotionCountText(health[0].currentItemAmount);
        }
    }
}
