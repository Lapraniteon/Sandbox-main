using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using static Attributes;

public class CollisionAttributeHandler : MonoBehaviour
{

    public List<ObjAttribute> startAttributes = new (); // Attributes to initialize the object with
    public List<AttributeBehaviour> attachedBehaviours;

    private Dictionary<ObjAttribute, AttributeBehaviour> attDict;

    private void Start()
    {
        InitializeStartAttributes();
    }

    public void InitializeStartAttributes()
    {
        attDict = GameManager.Instance.attributeBehaviourDictionary.GetDictionary();
        
        foreach (ObjAttribute attrib in startAttributes)
        {
            AddAttribute(attDict[attrib]);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        BroadcastMessage("ParentOnCollisionEnter", other, SendMessageOptions.DontRequireReceiver);
        
        if (other.gameObject.CompareTag("DontApply"))
            return;
        
        // Should all be things that it applies to ITSELF

        List<AttributeBehaviour> otherAttributes = other.gameObject.GetComponent<CollisionAttributeHandler>()?.attachedBehaviours;

        if (otherAttributes != null)
            HandleAttributes(otherAttributes);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DontApply"))
            return;
        
        // Should all be things that it applies to ITSELF

        List<AttributeBehaviour> otherAttributes = other.gameObject.GetComponent<CollisionAttributeHandler>()?.attachedBehaviours;
        
        if (otherAttributes != null)
            HandleAttributes(otherAttributes);
    }

    private void HandleAttributes(List<AttributeBehaviour> otherAttributes)
    {
        Debug.Log("Handling attributes");
        
        if (otherAttributes.Any(item => item is FireBehaviour) && attachedBehaviours.Any(item => item is FlammableBehaviour))
        {
            AddAttribute(attDict[ObjAttribute.OnFire]);
        }

        if (otherAttributes.Any(item => item is BouncyBehaviour))
        {
            AddAttribute(attDict[ObjAttribute.Bouncy]);
        }
        
        if (otherAttributes.Any(item => item is ScreamBehaviour))
        {
            AddAttribute(attDict[ObjAttribute.Screaming]);
        }
    }

    public void MakeSticky()
    {
        AddAttribute(attDict[ObjAttribute.Sticky], false);
    }

    public bool AddAttribute(AttributeBehaviour attribute, bool propagate = true)
    {
        if (attachedBehaviours.All(item => item.GetType() != attribute.GetType())) // If object doesnt already have this attribute
        {
            AttributeBehaviour newAttribute = Instantiate(attribute, transform.position, transform.rotation, transform);
            newAttribute.Initialize(gameObject);
            
            // Apply behaviour to all welded components.
            if (propagate)
            {
                HashSet<Weldable> connectedWeldables = GetComponent<Weldable>()?.GetAllConnectedRecursive();
                if (connectedWeldables != null)
                {
                    foreach (Weldable connectedWeldable in connectedWeldables)
                    {
                        CollisionAttributeHandler handler = connectedWeldable.GetComponent<CollisionAttributeHandler>();
                        handler.AddAttribute(attribute, false);
                    }
                }
            }
                
            attachedBehaviours.Add(newAttribute);
            
            return true; // Attribute succesfully added
        }

        return false; // Attribute already existed
    }

    private bool RemoveAttribute(AttributeBehaviour attribute)
    {
        if (attachedBehaviours.Count == 0)
            return false;
        
        var existingAttribute = attachedBehaviours
            .FirstOrDefault(item => item.GetType() == attribute.GetType());

        if (existingAttribute != null)
        {
            existingAttribute.Kill();
            attachedBehaviours.Remove(existingAttribute);
            return true; // Attribute succesfully removed
        }

        return false; // Attribute didn't exist
    }

    public string GetAttributesAsString()
    {
        string attributes = "";

        foreach (AttributeBehaviour attrib in attachedBehaviours)
        {
            attributes += attrib.Attribute.ToString() + Environment.NewLine;
        }

        return attributes;
    }
}
