using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttributesUIPanel : MonoBehaviour
{
    private List<AttributeUIEntry> attributeUIEntries = new ();
    [SerializeField] private AttributeUIEntry attributeUIEntryPrefab;
    
    [SerializeField] private PlayerObjectSpawner playerObjectSpawner;

    private int selectedIndex = 0;

    private void Start() // Spawn all attribute UI entries
    {
        foreach (Attributes.ObjAttribute objAttribute in Enum.GetValues(typeof(Attributes.ObjAttribute)))
        {
            AttributeUIEntry newEntry = Instantiate(attributeUIEntryPrefab, transform);
            newEntry.SetAttribute(objAttribute);
            attributeUIEntries.Add(newEntry);
        }
        
        attributeUIEntries[selectedIndex].SetSelected(true);
    }

    private void Update()
    {
        if (!GameManager.Instance.SpawnMode)
            return;

        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll > 0f)
        {
            ScrollSelection(-1);
        }
        else if (scroll < 0f)
        {
            ScrollSelection(1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            bool entryNowEnabled = attributeUIEntries[selectedIndex].ToggleEnabled();
            Attributes.ObjAttribute attribute = attributeUIEntries[selectedIndex].representingAttribute;
            
            if (entryNowEnabled)
                playerObjectSpawner.AddAttribute(attribute);
            else
                playerObjectSpawner.RemoveAttribute(attribute);
        }
    }

    private void OnEnable()
    {
        
    }

    private void ScrollSelection(int direction)
    {
        attributeUIEntries[selectedIndex].SetSelected(false);
        
        selectedIndex += direction;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, attributeUIEntries.Count - 1);
        
        attributeUIEntries[selectedIndex].SetSelected(true);
    }
}
