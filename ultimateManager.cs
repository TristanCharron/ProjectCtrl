using UnityEngine;
using System;

public class ultimateManager : MonoBehaviour
{
    void Awake()
    {

    }

}

public interface IUltimate
{
    void onRegenerate();
    void onEnable();
    void onDisable();
    void onUpdate();
    void onBonus();
    void onCharge();

    float Amount
    {
        get;
    }

    float BonusAmount
    {
        get;
    }

    float MaxAmount
    {
        get;
    }
    float RegenRate
    {
        get;
    }

    bool isAvailable
    {
        get;
    }

    bool isActive
    {
        get;
    }

    float Length
    {
        get;
    }

    string Name
    {
        get;
    }

}



public class ultimate1 : IUltimate
{
    protected float amount = 0;
    protected bool available = true, active = false;
    protected float countdown = 0;
    protected string name = "";
    public string Name
    {
        get { return name; }
    }
    public float Amount
    {
        get { return Mathf.Clamp(amount,0, MaxAmount); }
    }

    public float MaxAmount
    {
        get { return 100; }
    }

    public float RegenRate
    {
        get { return 2f; }
    }

    public bool isAvailable
    {
        get
        {
            return available;
        }
    }

    public bool isActive
    {
        get
        {
            return active;
        }
    }

    public float BonusAmount
    {
        get
        {
            return 10;
        }
    }

    public float Length
    {
        get
        {
            return 3f;
        }
    }

    

    public ultimate1()
    {
        amount = 0;
        active = false;
        available = false;
    }

    public void onRegenerate()
    {
        if (!active)
            amount += RegenRate;
        
        Mathf.Clamp(amount, 0, MaxAmount);

    }
    public void onUpdate()
    {
        onRegenerate();
        available = amount >= MaxAmount;
        if (active)
            onCharge();
    }
    public void onEnable()
    {
        amount = 0;
        active = true;
        available = false;



    }
    public void onDisable()
    {
        active = false;
    }

    public void onBonus()
    {
        amount += BonusAmount;
        Mathf.Clamp(Amount, 0, MaxAmount);
    }

    public void onCharge()
    {
        countdown += Time.deltaTime;
        if (countdown >= Length)
        {
            countdown = 0;
            onDisable();
        }
    }
}


