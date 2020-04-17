using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{
    [SerializeField]
    Text scoreUI;

    void Start()
    {
        scoreUI.gameObject.SetActive(false);
    }
    public void OnGameStarted()
    {
        scoreUI.gameObject.SetActive(true);
        scoreUI.text = string.Format("Score: {0}", 0);
    }

    public void OnScoreChanged(int score)
    {
        scoreUI.text = string.Format("Score: {0}", score);
    }
}
