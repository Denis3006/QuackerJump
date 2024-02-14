using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip newHighscoreSound;
    [SerializeField] AudioClip jumpSound;
    AudioSource audioSource;

    void Awake()
    {
        GameManager.OnGameOver.AddListener(PlayGameOverSound);
        GameManager.OnHighscoreAchieved.AddListener(PlayNewHighscoreSound);
        PlayerController.OnJump.AddListener(PlayJumpSound);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("volume", 1f);
    }

    void Update()
    {
        transform.position = PlayerController.Instance.transform.position;
    }

    void PlayNewHighscoreSound()
    {
        audioSource.PlayOneShot(newHighscoreSound, 1f);
    }

    void PlayGameOverSound()
    {
        audioSource.PlayOneShot(gameOverSound, 1f);
    }

    void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound, 0.75f);
    }
}