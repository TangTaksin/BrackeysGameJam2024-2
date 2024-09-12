using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Interactable, IDamagable
{
    public PlantData plantdata;

    SpriteRenderer plantImg;

    public int hitPoints;
    int regenCounter;

    public bool isDead;
    public bool isDamagable
    {
        get { return !isDead; }
        set {}
    }

    public bool isProtected;

    public delegate void PlantEvent(int damage, bool _resisted, bool _proteted, bool _died);
    public static PlantEvent OnDamage;


    private void Start()
    {
        plantImg = GetComponent<SpriteRenderer>();

        //
        hitPoints = plantdata.baseHitPoint;

        SpriteUpdate();
    }

    private void OnEnable()
    {
        DaySystem.OnDayStart += OnNewDay;
    }

    private void OnDisable()
    {
        DaySystem.OnDayStart -= OnNewDay;
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
        OnDamage?.Invoke(finalDamage, isResist, isProtected, isDead);
    }

    public void RecoverHealth(int amount)
    {
        if (isDead)
            return;

        hitPoints += amount;

        if (hitPoints > plantdata.baseHitPoint)
            hitPoints = plantdata.baseHitPoint;

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

    void OnNewDay()
    {
        SetProtected(false);
        RegenCheck();
    }

    void RegenCheck()
    {
        regenCounter++;
        if (regenCounter >= plantdata.daysToHeal)
        {
            RecoverHealth(1);
            regenCounter = 0;
        }
    }
}
