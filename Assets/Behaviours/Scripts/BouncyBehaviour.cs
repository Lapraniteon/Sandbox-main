using UnityEngine;

public class BouncyBehaviour : AttributeBehaviour
{
    [SerializeField]
    private PhysicsMaterial bounceMaterial;

    private Collider _parentCollider;

    [SerializeField]
    private Vector3 startImpulse;

    public override void Initialize(GameObject parentObj)
    {
        base.Initialize(parentObj);
        
        _parentCollider = parentObj.GetComponent<Collider>();

        if (_parentCollider == null)
            return;
        
        _parentCollider.material = bounceMaterial;
        
        Rigidbody parentRigidbody = parentObj.GetComponent<Rigidbody>();

        if (parentRigidbody == null)
            return;
        
        parentRigidbody.AddForce(startImpulse, ForceMode.Impulse);
    }

    public override void Kill()
    {
        if (_parentCollider != null)
            _parentCollider.material = null;
        
        base.Kill();
    }
}
