using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverSceneController : MonoBehaviour
{
    public Text score;
    public GameObject best;

    // Start is called before the first frame update
    void Start()
    {
        score.text = "Your Score:\n" + PlayerPrefs.GetInt("CurrentScore");
        if(PlayerPrefs.GetInt("CurrentScore") > PlayerPrefs.GetInt("BestScore"))
        {
            Image img = best.GetComponent<Image>();
            img.enabled = true;

            PlayerPrefs.SetInt("BestScore", PlayerPrefs.GetInt("CurrentScore"));
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void MoveToMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
