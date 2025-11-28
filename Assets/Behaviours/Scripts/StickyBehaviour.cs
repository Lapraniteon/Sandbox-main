using UnityEngine;

public class StickyBehaviour : AttributeBehaviour
{
    private void ParentOnCollisionEnter(Collision collision)
    {
        Debug.Log("Welding");
        GameManager.Instance.welder.Weld(transform.parent.gameObject);
    }

    public override void Kill()
    {
        base.Kill();
    }
}