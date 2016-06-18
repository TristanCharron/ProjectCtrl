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

    float amount
    {
        get;
    }

    float bonusAmount
    {
        get;
    }

    float maxAmount
    {
        get;
    }
    float regenRate
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

    float length
    {
        get;
    }

    string name
    {
        get;
    }

}



public class ultimate1 : IUltimate
{
    protected float Amount;
    protected bool available = true, active = false;
    protected float countdown = 0;
    protected string Name = "";
    public string name
    {
        get { return Name; }
    }
    public float amount
    {
        get { return Amount; }
    }

    public float maxAmount
    {
        get { return 100; }
    }

    public float regenRate
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

    public float bonusAmount
    {
        get
        {
            return 10;
        }
    }

    public float length
    {
        get
        {
            return 3f;
        }
    }

    public ultimate1()
    {
        Amount = 0;
        active = false;
        available = false;
    }

    public void onRegenerate()
    {
        if (!active)
        {
            Amount += regenRate;
            Mathf.Clamp(Amount, 0, maxAmount);

        }


    }
    public void onUpdate()
    {
        onRegenerate();
        available = Amount >= maxAmount;
        if (active)
            onCharge();
    }
    public void onEnable()
    {
        Amount = 0;
        active = true;
        available = false;



    }
    public void onDisable()
    {
        active = false;
    }

    public void onBonus()
    {
        Amount += bonusAmount;
        Mathf.Clamp(Amount, 0, maxAmount);
    }

    public void onCharge()
    {
        countdown += Time.deltaTime;
        if (countdown >= length)
        {
            countdown = 0;
            onDisable();
        }
    }
}


