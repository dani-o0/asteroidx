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
        GenerateAsteroids(AsteroidsSO.initialSize);
    }

    void GenerateAsteroids(int size)
    {
        for (int i = 0; i < size; i++)
        {
            Vector3 spawnPosition = player.position + Random.onUnitSphere * 5;

            if (AsteroidsSO == null || AsteroidsSO.asteroidPrefabs.Length == 0)
                return;

            GameObject asteroidPrefab = AsteroidsSO.GetAsteroidPrefab();
            GameObject asteroidObject = Instantiate(asteroidPrefab, spawnPosition, Random.rotation);
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();

            asteroid.Initialize(size, i.ToString(), null);
            asteroid.GenerateChildren(size - 1, asteroidObject.transform);
        }
    }
}