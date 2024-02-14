using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static readonly UnityEvent OnGameOver = new();
    public static readonly UnityEvent OnHighscoreAchieved = new();
    public static readonly UnityEvent OnScoreUpdated = new();
    [SerializeField] GameObject highscoreLine;
    [SerializeField] Camera cam;
    [SerializeField] GameObject player;
    public float score;
    public float highscore;
    public bool isGameOver;
    CameraController cameraController;
    bool highscoreAchieved;
    public static GameManager Instance { get; private set; }


    void Awake()
    {
        cameraController = cam.GetComponent<CameraController>();
        Instance = this;
        OnGameOver.AddListener(GameOver);
        highscore = PlayerPrefs.GetFloat("highscore", 0);

        var position = highscoreLine.transform.position;
        position = new Vector3(position.x, cam.transform.position.y + highscore, position.z);
        highscoreLine.transform.position = position;
        highscoreLine.SetActive(true);
    }


    void Update()
    {
        if (player.transform.position.y > cam.transform.position.y) {
            score += player.transform.position.y - cam.transform.position.y;
            OnScoreUpdated.Invoke();
        }

        if (score > highscore && !highscoreAchieved) {
            highscoreAchieved = true;
            Handheld.Vibrate();
            OnHighscoreAchieved.Invoke();
        }

        if (highscoreAchieved) {
            PlayerPrefs.SetFloat("highscore", score);
        }

        if (player.transform.position.y < cameraController.OrthographicBounds().min.y && !isGameOver) {
            OnGameOver.Invoke();
        }


        if (isGameOver && Input.GetKey(KeyCode.Space)) {
            RestartGame();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        player.SetActive(false);
    }

    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}