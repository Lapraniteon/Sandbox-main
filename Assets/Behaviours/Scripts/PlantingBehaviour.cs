using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PlantingBehaviour : AttributeBehaviour
{
    [SerializeField] private GameObject plantPrefab;

    private CollisionAttributeHandler handler;

    private Vector3 spawnPosition;

    public override void Initialize(GameObject parentObj)
    {
        handler = parentObj.GetComponent<CollisionAttributeHandler>();
        base.Initialize(parentObj);
    }
    
    private void ParentOnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("PlantSurface"))
            return;
        
        if (handler != null && !handler.attachedBehaviours.Any(item => item is WetBehaviour))
            return;

        spawnPosition = other.GetContact(0).point;
        Invoke(nameof(SpawnPlant), 1f);
    }

    private void SpawnPlant()
    {
        GameObject newPlant = Instantiate(plantPrefab, spawnPosition, Quaternion.identity);
        newPlant.transform.localScale = Vector3.zero;
        newPlant.transform.DOScale(1f, .5f).SetEase(Ease.OutBounce);
    }
}
