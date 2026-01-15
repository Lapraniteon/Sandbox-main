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

    private bool canAwardCollisionScore = true;

    [SerializeField] private bool canLoseAttributes = true;
    [SerializeField] private bool canAwardScore = true;

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

        if (canAwardCollisionScore)
        {
            canAwardCollisionScore = false;
            GameManager.Instance.RegisterInteraction(5, canAwardScore);
            Invoke(nameof(ResetCanAwardCollisionScore), 0.5f);
        }
        
        if (other.gameObject.CompareTag("DontApply"))
            return;
        
        // Should all be things that it applies to ITSELF

        CollisionAttributeHandler otherAttributeHandler = other.gameObject.GetComponent<CollisionAttributeHandler>();

        if (otherAttributeHandler != null)
            HandleAttributes(otherAttributeHandler);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DontApply"))
            return;
        
        // Should all be things that it applies to ITSELF

        CollisionAttributeHandler otherAttributeHandler = other.gameObject.GetComponent<CollisionAttributeHandler>();
        
        if (otherAttributeHandler != null)
            HandleAttributes(otherAttributeHandler);
    }

    private void HandleAttributes(CollisionAttributeHandler otherAttributeHandler)
    {
        List<AttributeBehaviour> otherAttributes = otherAttributeHandler.attachedBehaviours;
        
        if (otherAttributes.Any(item => item is FireBehaviour) && 
            attachedBehaviours.Any(item => item is FlammableBehaviour))
        {
            AddAttribute(attDict[ObjAttribute.OnFire]);
            GameManager.Instance.RegisterInteraction(500, canAwardScore);
        }

        if (otherAttributes.Any(item => item is BouncyBehaviour))
        {
            AddAttribute(attDict[ObjAttribute.Bouncy]);
            GameManager.Instance.RegisterInteraction(200, canAwardScore);
        }
        
        if (otherAttributes.Any(item => item is ScreamBehaviour))
        {
            AddAttribute(attDict[ObjAttribute.Screaming]);
            GameManager.Instance.RegisterInteraction(100, canAwardScore);
        }
        
        if (otherAttributes.Any(item => item is WetBehaviour))
        {
            if (RemoveAttribute(attDict[ObjAttribute.OnFire]))
                otherAttributeHandler.RemoveAttribute(attDict[ObjAttribute.Wet]);
            GameManager.Instance.RegisterInteraction(200, canAwardScore);
        }

        if (otherAttributes.Any(item => item is FireBehaviour) &&
            attachedBehaviours.Any(item => item is ExplosiveBehaviour))
        {
            RemoveAttribute(attDict[ObjAttribute.Explosive]);
            AddAttribute(attDict[ObjAttribute.OnFire]);
            GameManager.Instance.RegisterInteraction(500, canAwardScore);
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

        if (!canLoseAttributes)
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

    public void PropagateExplosion()
    {
        Invoke(nameof(PropagateExplosionDelay), 0.02f);
    }

    private void PropagateExplosionDelay()
    {
        GameManager.Instance.RegisterInteraction(500, canAwardScore);
        RemoveAttribute(attDict[ObjAttribute.Explosive]);
        AddAttribute(attDict[ObjAttribute.OnFire]);
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
    
    private void ResetCanAwardCollisionScore() => canAwardCollisionScore = true;
}
