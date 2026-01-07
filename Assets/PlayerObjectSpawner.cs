using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerObjectSpawner : MonoBehaviour
{

    [SerializeField] private float maxRayDistance;

    [SerializeField] private SpawnableObject[] spawnableObjects;
    [SerializeField] private int selectedSpawnableObjectIndex;
    [SerializeField] private TextMeshProUGUI currentSpawnableObjectText;

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

        if (Input.GetKeyDown(KeyCode.J))
        {
            selectedSpawnableObjectIndex++;
            if (selectedSpawnableObjectIndex >= spawnableObjects.Length) 
                selectedSpawnableObjectIndex = 0;
            
            currentSpawnableObjectText.text = spawnableObjects[selectedSpawnableObjectIndex].name;
        }
        
        if (Input.GetMouseButtonDown(1) && GameManager.Instance.SpawnMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(InputSystem.GetPointerPosition());
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxRayDistance, excludePlayerMask))
            {
                CollisionAttributeHandler obj = Instantiate(spawnableObjects[selectedSpawnableObjectIndex].prefab, hitInfo.point, Quaternion.identity);
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

[System.Serializable]
public class SpawnableObject
{
    public string name;
    public CollisionAttributeHandler prefab;
}
