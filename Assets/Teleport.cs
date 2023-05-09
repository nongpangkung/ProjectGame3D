using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FN
{
    public class Teleport : MonoBehaviour
    {
        public string LevelName;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Character")
            {
                SceneManager.LoadScene(LevelName);
            }
        }
    }
}
