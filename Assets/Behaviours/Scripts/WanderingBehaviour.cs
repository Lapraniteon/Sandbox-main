using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WanderingBehaviour : AttributeBehaviour
{
    private GameObject parent;
    private NavMeshAgent agent;

    private Coroutine wanderCoroutine;
    [SerializeField] private float targetInterval;
    [SerializeField] private float targetRadius;
    
    
    public override void Initialize(GameObject parentObj)
    {
        base.Initialize(parentObj);
        parent = parentObj;
        agent = parent.AddComponent<NavMeshAgent>();
        wanderCoroutine = StartCoroutine(Wander());
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            Vector3 currentPos = parent.transform.position;
            Vector3 offset = Random.insideUnitSphere * targetRadius;
            Vector3 newPos = new Vector3(currentPos.x + offset.x, currentPos.y, currentPos.z + offset.z);
        
            agent.SetDestination(newPos);
        
            yield return new WaitForSeconds(targetInterval);
        }
    }

    public override void Kill()
    {
        StopCoroutine(wanderCoroutine);
        Destroy(agent);
        base.Kill();
    }
}
