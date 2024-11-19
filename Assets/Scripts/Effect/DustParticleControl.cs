using UnityEngine;

public class DustParticleControl : MonoBehaviour
{
    [SerializeField] bool createDustOnWalk = true;
    [SerializeField] ParticleSystem dustParticleSystem;

    public void CreateDustParticle()
    {
        if(createDustOnWalk)
        {
            dustParticleSystem.Stop();
            dustParticleSystem.Play();
        }
    }
}