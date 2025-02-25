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
    private List<Asteroid> children;

    void Awake()
    {
        children = new List<Asteroid>();
        rigid = GetComponent<Rigidbody>();
    }
    
    public void Initialize(int newSize, string namePrefix, Transform parent)
    {
        size = newSize;
        transform.localScale = Vector3.one * (size * AsteraX.S.AsteroidsSO.asteroidScale);
        name = namePrefix;

        if (parent != null)
        {
            transform.SetParent(parent);
        }

        InitVelocity();
    }

    void InitVelocity()
    {
        Vector2 vel2D = ScreenBounds.OOB(transform.position) ? Random.insideUnitCircle * 4 - (Vector2)transform.position : Random.insideUnitCircle;
        vel2D.Normalize();

        rigid.velocity = new Vector3(vel2D.x, vel2D.y, 0) * Random.Range(minVel, maxVel);
        rigid.angularVelocity = new Vector3(0, 0, Random.Range(-maxAngularVel, maxAngularVel));

        // Mantener la misma Z que el player
        transform.position = new Vector3(transform.position.x, transform.position.y, AsteraX.S.player.position.z);
    }

    public void GenerateChildren(int newSize, Transform parent)
    {
        if (newSize < 1) return;

        for (int i = 0; i < AsteraX.S.AsteroidsSO.numSmallerAsteroidsToSpawn; i++)
        {
            GameObject asteroidPrefab = AsteraX.S.AsteroidsSO.GetAsteroidPrefab();
            Vector3 spawnPosition = transform.position + Random.onUnitSphere * 0.5f;
            GameObject asteroidObject = Instantiate(asteroidPrefab, spawnPosition, Random.rotation);
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            Rigidbody asteroidRigidbody = asteroidObject.GetComponent<Rigidbody>();
            OffScreenWrapper asteroidOffScreenWrapper = asteroidObject.GetComponent<OffScreenWrapper>();
            MeshCollider asteroidCollider = asteroidObject.GetComponent<MeshCollider>();
            
            string childName = name + "-" + i;
            asteroidRigidbody.isKinematic = true;
            asteroidOffScreenWrapper.enabled = false;
            asteroidCollider.enabled = false;
            asteroid.Initialize(newSize, childName, parent);
            asteroid.GenerateChildren(newSize - 1, asteroidObject.transform);
            
            children.Add(asteroid);
        }
    }
    
    void OnCollisionEnter(Collision coll)
    {
        GameObject collObj = coll.gameObject;

        if (collObj.CompareTag("Bullet"))
        {
            Destroy(collObj);
            
            foreach (Asteroid child in children)
            {
                if (child != null)
                {
                    Rigidbody childRb = child.GetComponent<Rigidbody>();
                    OffScreenWrapper childOfw = child.GetComponent<OffScreenWrapper>();
                    MeshCollider childCollider = child.GetComponent<MeshCollider>();
                    
                    child.transform.SetParent(null);
                    childOfw.enabled = true;
                    childRb.isKinematic = false;
                    childCollider.enabled = true;
                    child.InitVelocity();
                }
            }
            
            Destroy(gameObject);
        }

        if (collObj.CompareTag("Player"))
        {
            foreach (Asteroid child in children)
            {
                if (child != null)
                {
                    Rigidbody childRb = child.GetComponent<Rigidbody>();
                    OffScreenWrapper childOfw = child.GetComponent<OffScreenWrapper>();
                    MeshCollider childCollider = child.GetComponent<MeshCollider>();
                    
                    child.transform.SetParent(null);
                    childOfw.enabled = true;
                    childRb.isKinematic = false;
                    childCollider.enabled = true;
                    child.InitVelocity();
                }
            }
            
            Destroy(gameObject);
        }
    }
}
