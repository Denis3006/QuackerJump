using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] GameObject highscoreLine;
    [SerializeField] Camera cam;
    [SerializeField] GameObject player;
    [SerializeField] Button restartButton;
    [SerializeField] Button unpauseButton;
    public float score;
    public float highscore;
    public bool isGameOver;
    AudioSource audioSource;
    CameraController cameraController;
    bool highscoreAchieved;
    PlayerController playerController;
    TextMeshProUGUI unpauseButtonText;

    void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        audioSource = player.GetComponent<AudioSource>();
        highscore = PlayerPrefs.GetFloat("highscore", 0);
        scoreText.text = "Score: " + Convert.ToInt32(score);
        highscoreText.text = "Highscore: " + Convert.ToInt32(highscore);
        var position = highscoreLine.transform.position;
        position = new Vector3(position.x, cam.transform.position.y + highscore, position.z);
        highscoreLine.transform.position = position;
        highscoreLine.SetActive(true);
    }

    void Start()
    {
        cameraController = cam.GetComponent<CameraController>();
        unpauseButtonText = unpauseButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > cam.transform.position.y) {
            score += player.transform.position.y - cam.transform.position.y;
        }

        if (score > highscore && !highscoreAchieved) {
            highscoreAchieved = true;
            audioSource.PlayOneShot(playerController.newHighscoreSound, 1f);
        }

        if (player.transform.position.y < cameraController.OrthographicBounds().min.y) {
            GameOver();
        }

        scoreText.text = "Score: " + Convert.ToInt32(score);
        if (highscoreAchieved) {
            PlayerPrefs.SetFloat("highscore", score);
        }

        if (isGameOver && Input.GetKey(KeyCode.Space)) {
            RestartGame();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        audioSource.PlayOneShot(playerController.gameOverSound, 1f);
        if (score > highscore) {
            PlayerPrefs.SetFloat("highscore", score);
            highscore = score;
        }

        player.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
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