using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OffScreenWrapper))]
public class Asteroid : MonoBehaviour
{
    public float minVel = 5;
    public float maxVel = 10;
    public float maxAngularVel = 10;
    private Rigidbody rigid;
    private int size;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        //InitVelocity();
    }
    
    public void Initialize(int newSize, string namePrefix, Transform parent)
    {
        size = newSize;
        transform.localScale = Vector3.one * (size * AsteroidsScriptableObject.S.asteroidScale);
        name = namePrefix;

        if (parent != null)
        {
            transform.SetParent(parent);
        }

        InitVelocity();
    }

    void InitVelocity()
    {
        Vector3 vel = ScreenBounds.OOB(transform.position) ? ((Vector3)Random.insideUnitCircle * 4) - transform.position : Random.insideUnitCircle;
        vel.Normalize();
        rigid.velocity = vel * Random.Range(minVel, maxVel);
        rigid.angularVelocity = Random.insideUnitSphere * maxAngularVel;
    }

    public void GenerateChildren(int newSize, Transform parent)
    {
        if (newSize < 1) return;  // Detener si el tamaño es menor a 1

        for (int i = 0; i < AsteroidsScriptableObject.S.numSmallerAsteroidsToSpawn; i++)
        {
            GameObject asteroidPrefab = AsteroidsScriptableObject.S.GetAsteroidPrefab();
            Vector3 spawnPosition = transform.position + Random.onUnitSphere * 0.5f;
            GameObject asteroidObject = Instantiate(asteroidPrefab, spawnPosition, Random.rotation);
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            

            string childName = name + "-" + i;
            asteroid.Initialize(newSize, childName, parent);
            asteroid.GenerateChildren(newSize - 1, asteroidObject.transform);
        }
    }

    // Gestionar la colisión
    void OnCollisionEnter(Collision coll)
    {
        GameObject collObj = coll.gameObject;

        if (collObj.CompareTag("Bullet") || collObj.CompareTag("Player"))
        {
            Destroy(collObj);
            Destroy(gameObject);
        }
    }
}
