using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FN
{
    public class GameOverUI : MonoBehaviour
    {
        public void Retry()
        {
            string lastSceneName = PlayerPrefs.GetString("LastSceneName");
            SceneManager.LoadScene(lastSceneName);
        }
    }
}
