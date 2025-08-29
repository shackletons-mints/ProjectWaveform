using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject prefabToInstantiate; // Drag your prefab here in the inspector
    
    void Start()
    {
        Debug.Log("START");
        particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }
    
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);
        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;
        Debug.Log("HERE - Collision Events: " + numCollisionEvents);
        
        while (i < numCollisionEvents)
        {
            Debug.Log("INSIDE WHILE - Event " + i);
            
            Vector3 pos = collisionEvents[i].intersection;
            Vector3 normal = collisionEvents[i].normal;
            Debug.Log("POSITION: " + pos);
            
            // Instantiate prefab at collision position
            if (prefabToInstantiate != null)
            {
                // Optional: Align the instantiated object with the collision normal
                Quaternion rotation = Quaternion.LookRotation(normal);
                GameObject instance = Instantiate(prefabToInstantiate, pos, rotation);
                
                // Optional: You can also instantiate without rotation
                // GameObject instance = Instantiate(prefabToInstantiate, pos, Quaternion.identity);
                
                Debug.Log("Instantiated prefab at: " + pos);
            }
            else
            {
                Debug.LogWarning("No prefab assigned to instantiate!");
            }
            
            // Optional: Apply force to the collided object if it has a rigidbody
            if (rb)
            {
                Vector3 force = collisionEvents[i].velocity * 10;
                rb.AddForce(force);
            }
            
            i++;
        }
    }
}
