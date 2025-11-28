using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttributeEntry
{
    public Attributes.ObjAttribute key;
    public AttributeBehaviour value;
}

[CreateAssetMenu(fileName = "AttributeDictionary", menuName = "Scriptable Objects/AttributeDictionary")]
public class AttributeDictionary : ScriptableObject
{
    public List<AttributeEntry> attributeEntries;
    
    public Dictionary<Attributes.ObjAttribute, AttributeBehaviour> GetDictionary()
    {
        var dict = new Dictionary<Attributes.ObjAttribute, AttributeBehaviour>();
        foreach (var entry in attributeEntries)
        {
            dict[entry.key] = entry.value;
        }
        return dict;
    }
}

