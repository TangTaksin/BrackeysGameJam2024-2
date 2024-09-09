using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    
    public virtual void Interact()
    {
        print("You interact with" + name);
    }

    
}
