using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;

    void LateUpdate()
    {
        var position = transform.position;
        position.y = Mathf.Max(PlayerController.Instance.transform.position.y, position.y);
        transform.position = position;
    }

    public Bounds OrthographicBounds()
    {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }

        var screenAspect = Screen.width / (float)Screen.height;
        var cameraHeight = mainCamera.orthographicSize * 2;
        var bounds = new Bounds(
            transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}