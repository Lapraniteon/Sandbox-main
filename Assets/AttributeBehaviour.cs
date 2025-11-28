using UnityEngine;

public abstract class AttributeBehaviour : MonoBehaviour
{
    
    public virtual void Initialize(GameObject parentObj)
    {
        
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }
}
