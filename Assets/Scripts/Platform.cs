using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Platform : MonoBehaviour
{
    public const float PlatformWidth = 1.5f;
    public const float SlowingFactor = 1.4f;
    public PlatformType platformType;
    public float platformSpeed = 1f;
    CameraController cameraController;
    float moveDistance;
    float movingOffset;


    void Start()
    {
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        moveDistance = cameraController.OrthographicBounds().max.x - PlatformWidth / 2;
        if (platformType == PlatformType.Moving) {
            movingOffset = Random.Range(0f, 2f * (float)Math.PI);
        }
    }

    void Update()
    {
        if (platformType == PlatformType.Moving) {
            var position = transform.position;
            position.x = moveDistance * Mathf.Sin(Time.time * platformSpeed + movingOffset);
            transform.position = position;
        }

        if (transform.position.y < cameraController.OrthographicBounds().min.y) {
            gameObject.SetActive(false);
        }
    }
}