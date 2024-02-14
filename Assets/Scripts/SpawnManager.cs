using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Default,
    Moving,
    Fragile,
    Slowing
}

public class SpawnManager : MonoBehaviour
{
    CameraController cameraController;
    ObjectPooler objectPooler;
    Dictionary<PlatformType, int> typeWeights;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        objectPooler = GameObject.Find("GameManager").GetComponent<ObjectPooler>();
        typeWeights = new Dictionary<PlatformType, int>
        {
            { PlatformType.Default, 65 },
            { PlatformType.Moving, 15 },
            { PlatformType.Fragile, 15 },
            { PlatformType.Slowing, 10 }
        };
    }

    // Update is called once per frame
    void Update()
    {
        var highestPlatform = HighestActivePlatform();
        var cameraBounds = cameraController.OrthographicBounds();
        if (highestPlatform == null || highestPlatform.transform.position.y < cameraBounds.max.y) {
            SpawnNewPlatform(cameraBounds, highestPlatform);
        }
    }

    void SpawnNewPlatform(Bounds cameraBounds, GameObject highestPlatform)
    {
        var platformType = RandomPlatformType();
        var newPlatform = objectPooler.GetPooledObject(platformType);
        newPlatform.SetActive(true);
        newPlatform.GetComponent<Platform>().platformType = platformType;

        float xCenter = 0;
        var jumpVelocity = PlayerController.Instance.jumpVelocity;
        var yMin = cameraBounds.min.y;
        var yMax = cameraBounds.min.y + 3;

        var xMin = xCenter;
        var xMax = xCenter;
        if (highestPlatform != null) {
            var highestPlatformPos = highestPlatform.transform.position;
            yMin = highestPlatformPos.y + 1f;
            if (highestPlatform.CompareTag(PlatformType.Slowing.ToString())) {
                jumpVelocity /= Platform.SlowingFactor;
            }

            yMax = highestPlatformPos.y + PlayerController.Instance.JumpHeight(jumpVelocity) * 0.8f;

            xCenter = highestPlatformPos.x;
            var jumpDistance = PlayerController.Instance.JumpDistance(jumpVelocity) * 0.8f;
            xMin = Mathf.Max(xCenter - Platform.PlatformWidth / 2 - jumpDistance,
                cameraBounds.min.x + Platform.PlatformWidth / 2);
            xMax = Mathf.Min(xCenter + Platform.PlatformWidth / 2 + jumpDistance,
                cameraBounds.max.x - Platform.PlatformWidth / 2);
        }

        newPlatform.transform.position = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0);
    }

    GameObject HighestActivePlatform()
    {
        GameObject highestSoFar = null;
        var pooledObjects = objectPooler.pooledObjects;
        foreach (var platform in pooledObjects)
            if (highestSoFar == null) {
                if (platform.activeInHierarchy) {
                    highestSoFar = platform;
                }
            }
            else if (platform.activeInHierarchy && platform.transform.position.y > highestSoFar.transform.position.y) {
                highestSoFar = platform;
            }

        return highestSoFar;
    }

    PlatformType RandomPlatformType()
    {
        var randomNumber = Random.Range(0, 100);
        foreach (var weight in typeWeights)
            if (randomNumber < weight.Value) {
                return weight.Key;
            }
            else {
                randomNumber -= weight.Value;
            }

        return PlatformType.Default;
    }
}