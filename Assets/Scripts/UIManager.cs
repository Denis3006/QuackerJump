using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Button restartButton;
    [SerializeField] Button unpauseButton;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI gameOverText;
    TextMeshProUGUI unpauseButtonText;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android) {
            Application.targetFrameRate = 60;
        }

        if (SceneManager.GetActiveScene().name == "Game") {
            unpauseButtonText = unpauseButton.GetComponentInChildren<TextMeshProUGUI>();
            scoreText.text = "Score: " + Convert.ToInt32(GameManager.Instance.score);
            highscoreText.text = "Highscore: " + Convert.ToInt32(GameManager.Instance.highscore);
            GameManager.OnScoreUpdated.AddListener(() =>
                scoreText.text = "Score: " + Convert.ToInt32(GameManager.Instance.score));
            GameManager.OnGameOver.AddListener(() =>
            {
                gameOverText.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
            });
        }
        else {
            sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 0.7f);
            volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.7f);
        }
    }

    public void ChangeSensitivity()
    {
        PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
    }

    public void ChangeVolume()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        unpauseButtonText.text = "TAP TO CONTINUE";
        unpauseButton.gameObject.SetActive(true);
    }

    IEnumerator UnpauseCountdown()
    {
        for (var i = 3; i > 0; i--) {
            unpauseButtonText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        Time.timeScale = 1f;
        unpauseButton.gameObject.SetActive(false);
    }

    public void UnpauseGame()
    {
        StartCoroutine(UnpauseCountdown());
    }
}