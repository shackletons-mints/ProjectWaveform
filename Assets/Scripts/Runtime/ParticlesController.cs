using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour {
    public Color paintColor;

    public float minRadius = 0.05f;
    public float maxRadius = 0.2f;
    public float strength = 1;
    public float hardness = 1;

    public float paintCooldown = 0.1f; // seconds between paints

    ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;

    private Dictionary<Paintable, float> lastPaintTime = new Dictionary<Paintable, float>();

    // Store paint requests to process later
    private class PaintRequest {
        public Paintable paintable;
        public Vector3 position;
        public float radius;
    }

    private List<PaintRequest> paintRequests = new List<PaintRequest>();

    void Start() {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other) {
        Paintable p = other.GetComponent<Paintable>();
        if (p == null) return;

        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        if (numCollisionEvents == 0) return;

        if (lastPaintTime.TryGetValue(p, out float lastTime)) {
            if (Time.time - lastTime < paintCooldown) {
                return; // skip painting to throttle
            }
        }

        // Average collision positions to paint once per Paintable per frame
        Vector3 avgPos = Vector3.zero;
        for (int i = 0; i < numCollisionEvents; i++) {
            avgPos += collisionEvents[i].intersection;
        }
        avgPos /= numCollisionEvents;

        float radius = Random.Range(minRadius, maxRadius);

        // Store paint request instead of painting now
        paintRequests.Add(new PaintRequest {
            paintable = p,
            position = avgPos,
            radius = radius
        });

        lastPaintTime[p] = Time.time;
    }

    void LateUpdate() {
        // Process all paint requests here, outside OnParticleCollision
        foreach (var req in paintRequests) {
            PaintManager.instance.paint(req.paintable, req.position, req.radius, hardness, strength, paintColor);
        }
        paintRequests.Clear();
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ParticlesController: MonoBehaviour{
//    public Color paintColor;
    
//    public float minRadius = 0.05f;
//    public float maxRadius = 0.2f;
//    public float strength = 1;
//    public float hardness = 1;
//    [Space]
//    ParticleSystem part;
//    List<ParticleCollisionEvent> collisionEvents;

//    void Start(){
//        part = GetComponent<ParticleSystem>();
//        collisionEvents = new List<ParticleCollisionEvent>();
//        //var pr = part.GetComponent<ParticleSystemRenderer>();
//        //Color c = new Color(pr.material.color.r, pr.material.color.g, pr.material.color.b, .8f);
//        //paintColor = c;
//    }

//    void OnParticleCollision(GameObject other) {
//        // other.name is visuals and we want it to be some sort of plane
//        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

//        Paintable p = other.GetComponent<Paintable>();
//        // we are not getting p
//        if (p != null)
//        {
//            for (int i = 0; i < numCollisionEvents; i++)
//            {
//                Vector3 pos = collisionEvents[i].intersection;
//                float radius = Random.Range(minRadius, maxRadius);
//                PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
//            }
//        }
//    }
//}
