using UnityEngine;

public class ObjectLifetime : MonoBehaviour
{
    [SerializeField] private float lifetime = 30f;

    [SerializeField] private bool startLifetimeOnCollisionWithAttributeHolder;

    private void Start()
    {
        if (!startLifetimeOnCollisionWithAttributeHolder)
            Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!startLifetimeOnCollisionWithAttributeHolder)
            return;
        
        if (collision.gameObject.TryGetComponent(out CollisionAttributeHandler handler))
        {
            Destroy(gameObject, lifetime);
        }
    }
}
