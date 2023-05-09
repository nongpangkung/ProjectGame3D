using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class FogWall : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void ActivateFogWall()
        {
            gameObject.SetActive(true);
            AudioManager.instance.Play("FogWall");
        }

        public void DeactivateFogWall()
        {
            gameObject.SetActive(false);
        }
    }
}
