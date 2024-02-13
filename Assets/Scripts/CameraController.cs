using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    Camera mainCamera;

    void LateUpdate()
    {
        if (player.transform.position.y > transform.position.y) {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
    }

    public Bounds OrthographicBounds()
    {
        var screenAspect = Screen.width / (float)Screen.height;
        if (mainCamera == null) {
            // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
            mainCamera = Camera.main;
        }

        var cameraHeight = mainCamera.orthographicSize * 2;
        var bounds = new Bounds(
            transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}