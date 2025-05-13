using UnityEngine;

public class PitchReactor : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float pitch;

    public PitchReactor(ParticleSystem particleSystem, float pitch)
    {
        this.particleSystem = particleSystem;
        this.pitch = pitch;
    }
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
