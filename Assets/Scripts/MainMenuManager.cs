using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Slider volumeSlider;

    // Start is called before the first frame update
    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android) Application.targetFrameRate = 60;
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 0.7f);
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.7f);
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
}