using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Interactable, IDamagable
{
    public PlantData plantdata;

    SpriteRenderer plantImg;

    public int hitPoints;
    int regenCounter = 0;

    public bool isDead;
    public bool isDamagable
    {
        get { return !isDead; }
        set {}
    }

    public bool isProtected;
    public bool Attacked;

    public delegate void PlantEvent(Plant _plant,int damage, bool _resisted, bool _proteted, bool _died, bool _attacked);
    public static PlantEvent OnDamage;

    private void Awake()
    {
        if (!plantImg)
        plantImg = GetComponent<SpriteRenderer>();
        hitPoints = plantdata.baseHitPoint;
    }

    private void Start()
    {
        SpriteUpdate();
    }

    private void OnEnable()
    {
        DaySystem.OnDayStart += OnNewDay;
        StageSystem.OnReset += ResetState;
    }

    private void OnDisable()
    {
        DaySystem.OnDayStart -= OnNewDay;
        StageSystem.OnReset -= ResetState;
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
            hitPoints = 0;
            isDead = true;
            isInteractable = false;
        }

        SpriteUpdate();
        //print(string.Format("{0} was hit for {1} {2} damage", name, finalDamage, element));
        OnDamage?.Invoke(this ,finalDamage, isResist, isProtected, isDead, Attacked);
        
        Attacked = finalDamage > 0;
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
        Attacked = false;
    }

    void RegenCheck()
    {
        if (hitPoints >= plantdata.baseHitPoint || isDead)
            return;

        regenCounter++;
        if (regenCounter >= plantdata.daysToHeal )
        {
            print("Regen!");
            RecoverHealth(1);
            regenCounter = 0;
        }
    }

    void ResetState(bool _bool)
    {
        isDead = false;
        isProtected = false;
        isInteractable = true;
        hitPoints = plantdata.baseHitPoint;
        regenCounter = 0;

        SpriteUpdate();
    }
}
