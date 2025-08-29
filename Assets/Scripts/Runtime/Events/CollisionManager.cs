using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

	[Header("References")]
	public GameObject prefab;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
				pos.y += 0.2f;
				if (prefab == null)
					Debug.Log("PREFAB IS NULL POSITION");
				Instantiate(prefab, new Vector3(1f,1f,1f), Quaternion.identity);
				Debug.Log("POSITION: " + pos);
            }
            i++;
        }
    }
}
