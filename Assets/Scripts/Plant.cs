using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Interactable, IDamagable
{
    public PlantData plantdata;

    SpriteRenderer plantImg;

    public int hitPoints;

    public bool isDead;
    public bool isDamagable
    {
        get { return !isDead; }
        set {}
    }

    public bool isProtected;

    private void Start()
    {
        plantImg = GetComponent<SpriteRenderer>();

        //
        hitPoints = plantdata.baseHitPoint;

        SpriteUpdate();
    }

    public override void Interact(Player _player)
    {
        
    }

    public void TakeDamage(elements element, int damage)
    {
        var isResist = (element == plantdata.resistance);
        
        var resiModifier = (!isResist).GetHashCode();
        var protModifier = (!isProtected).GetHashCode();

        var finalDamage = damage * resiModifier * protModifier;

        hitPoints -= finalDamage;

        if (hitPoints <= 0)
        {
            isDead = true;
            
        }

        SpriteUpdate();
    }

    public void RecoverHealth(int amount)
    {
        if (isDead)
            return;

        hitPoints += amount;

        SpriteUpdate();
    }

    void SpriteUpdate()
    {
        var evaluate = hitPoints / plantdata.baseHitPoint;

        if (evaluate > .5f)
            plantImg.sprite = plantdata.normal;
        else
            plantImg.sprite = plantdata.hurt;

        if (isProtected)
            plantImg.sprite = plantdata.reenforce;

        if (isDead)
            plantImg.sprite = plantdata.dead;
    }
    
    public void SetProtected(bool _value)
    {
        isProtected = _value;
        SpriteUpdate();
    }
}
