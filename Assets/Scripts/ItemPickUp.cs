using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    public ItemData ItemData;

    public override void Interact(Player _player)
    {
        _player.SetItem(ItemData);
    }

    public virtual void DropItem(Vector3 position)
    {

    }
}
