using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AttributeUIEntry : MonoBehaviour
{
    public bool isEnabled;
    public bool isSelected;
    public Attributes.ObjAttribute representingAttribute;
    
    [SerializeField] private Image checkmark;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Image selectedBackground;

    public void SetAttribute(Attributes.ObjAttribute attribute)
    {
        representingAttribute = attribute;
        label.text = representingAttribute.ToString();
    }

    public bool ToggleEnabled()
    {
        isEnabled = !isEnabled;
        checkmark.enabled = isEnabled;
        
        return isEnabled;
    }

    public void SetSelected(bool pSelected)
    {
        isSelected = pSelected;
        selectedBackground.enabled = isSelected;
    }
}
