using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteraX : MonoBehaviour
{
    public static AsteraX S { get; private set; }

    public AsteroidsScriptableObject AsteroidsSO;
    public Transform player;

    void Awake()
    {
        if (S != null)
        {
            Debug.LogWarning("Second attempt to set AsteraX singleton.");
            return;
        }
        S = this;
    }

    void Start()
    {
        GenerateAsteroids(3);
    }

    void GenerateAsteroids(int size)
    {
        for (int i = 0; i < size; i++)
        {
            Vector3 spawnPosition = player.position + Random.onUnitSphere * 5;
            spawnPosition.z = player.position.z;

            GameObject asteroidPrefab = AsteroidsSO.GetAsteroidPrefab();
            GameObject asteroidObject = Instantiate(asteroidPrefab, spawnPosition, Random.rotation);
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            Rigidbody asteroidRigidbody = asteroidObject.GetComponent<Rigidbody>();
            OffScreenWrapper asteroidOffScreenWrapper = asteroidObject.GetComponent<OffScreenWrapper>();

            asteroidRigidbody.isKinematic = false;
            asteroidOffScreenWrapper.enabled = true;
            asteroid.Initialize(size, i.ToString(), null);
            asteroid.GenerateChildren(size - 1, asteroidObject.transform);
        }
    }
}