using UnityEngine;

public class FireBehaviour : AttributeBehaviour
{
    private GameObject parent;

    [SerializeField] private float burnTime;
    
    public override void Initialize(GameObject parentObj)
    {
        parent = parentObj;
        Invoke(nameof(DestroyParent), burnTime);
    }

    public override void Kill()
    {
        CancelInvoke(nameof(DestroyParent));
        base.Kill();
    }
    
    private void DestroyParent() => Destroy(parent);
}
