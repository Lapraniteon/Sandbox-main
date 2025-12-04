using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectSpawner : MonoBehaviour
{

    [SerializeField] private float maxRayDistance;

    [SerializeField] private CollisionAttributeHandler prefabToSpawn;

    [SerializeField] private List<Attributes.ObjAttribute> selectedAttributes = new();

    private int excludePlayerMask;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        excludePlayerMask = ~(1 << playerLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            GameManager.Instance.ToggleSpawnMode();
        
        if (Input.GetMouseButtonDown(1) && GameManager.Instance.SpawnMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(InputSystem.GetPointerPosition());
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxRayDistance, excludePlayerMask))
            {
                CollisionAttributeHandler obj = Instantiate(prefabToSpawn, hitInfo.point, Quaternion.identity);
                obj.startAttributes = selectedAttributes;
                obj.InitializeStartAttributes();
            }
            
        }
    }

    public void AddAttribute(Attributes.ObjAttribute attribute)
    {
        if (!selectedAttributes.Contains(attribute))
            selectedAttributes.Add(attribute);
    }

    public void RemoveAttribute(Attributes.ObjAttribute attribute)
    {
        if (selectedAttributes.Contains(attribute))
            selectedAttributes.Remove(attribute);
    }
}
