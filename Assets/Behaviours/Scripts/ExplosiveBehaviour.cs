using System.Linq;
using UnityEngine;

public class ExplosiveBehaviour : AttributeBehaviour
{

    private bool hasExploded;
    
    [Header("Explosion Settings")]
    public float radius = 5.0f;
    public float power = 10.0f;
    public float upwardsModifier = 3.0f;
    
    [Header("Optional Settings")]
    public LayerMask affectedLayers = ~0; // All layers by default

    [Header("VFX")] 
    public ParticleSystem explosionParticles;

    public override void Kill()
    {
        if (!hasExploded)
            ApplyExplosionForce();
        
        base.Kill();
    }
    
    public void ApplyExplosionForce()
    {
        Debug.Log("Apply explosion");

        hasExploded = true;
        
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius, affectedLayers);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upwardsModifier, ForceMode.Impulse);
                Instantiate(explosionParticles, explosionPosition, Quaternion.identity);
            }
            
            if (collider.TryGetComponent(out CollisionAttributeHandler collisionAttributeHandler))
            {
                collisionAttributeHandler.PropagateExplosion();
            }
        }
    }
}
