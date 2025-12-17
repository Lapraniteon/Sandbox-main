using UnityEngine;

public class WetBehaviour : AttributeBehaviour
{

    public ParticleSystem steamParticles;
    
    public override void Kill()
    {
        Instantiate(steamParticles, transform.position, Quaternion.identity);
        
        base.Kill();
    }
}
