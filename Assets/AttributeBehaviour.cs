using UnityEngine;

public abstract class AttributeBehaviour : MonoBehaviour
{
    
    [SerializeField] private Attributes.ObjAttribute _attribute;
    public Attributes.ObjAttribute Attribute => _attribute;
    
    public virtual void Initialize(GameObject parentObj)
    {
        
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }
    
    
}
