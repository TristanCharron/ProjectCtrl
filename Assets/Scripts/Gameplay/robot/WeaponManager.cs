using UnityEngine;
using System.Collections;
using System;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public static void onFireBullet(Character character, GameObject origin)
    {
        GameObject bullet = character.Weapon.ObjPool.PooledObj;

        if (bullet != null)
        {
            bullet.GetComponent<bullet>().setInfo(character);
            bullet.GetComponent<bullet>().onStart(origin.transform.position);
        }
        Instance.StartCoroutine(character.onShake());
        Instance.StartCoroutine(Instance.onEmitWeaponParticle(character));

    }

    public IEnumerator onEmitWeaponParticle(Character character)
    {
        //character.setParticleObject(Resources.Load("Particles/" + character.Weapon.ParticleName) as GameObject);
        ParticleSystem pSystem = character.ParticleObject.GetComponent<ParticleSystem>();
        pSystem.Play();
        yield return new WaitForSeconds(0.1f);
        pSystem.Stop();
        yield break;
    }




}






public abstract class Weapon
{
    protected string name, particleName;
    protected float damage;
    protected int nbBullets;
    protected ObjPooler objPool;
    protected Tools.easeType easeType;
    protected Character shooter;
    protected GameObject gObject;

    // Getters variables for weapons
    protected fireType firetype;
    public fireType FireType { get { return firetype; } }
    public Tools.easeType EaseType { get { return easeType; } }

    //Weapon stats and info
    public string Name { get { return name; } }
    public string ParticleName { get { return particleName; } }
    public float Damage { get { return damage; } }
    

    public ObjPooler ObjPool { get { return objPool; } }
    public GameObject gameObject { get { return gObject; } }
    public Character Shooter { get { return shooter; } }

    public void onDestroy() { objPool.onDestroy(); }


    public void onFire()
    {
        if (firetype.onFireTrigger() && !shooter.isFiring)
            WeaponManager.onFireBullet(shooter, gameObject);
    }

    public abstract void onEnableUltimate();

}


[SerializeField]
public class Pistol : Weapon
{
    public Pistol(Character character, GameObject _gObject)
    {
        name = "Pistol";
        particleName = "Default";
        damage = 10f;
        easeType = Tools.easeType.easeOutExpo;
        firetype = new semiAutomatic();
        nbBullets = 100;
        shooter = character;
        gObject = _gObject;
        objPool = new ObjPooler(character, nbBullets, "Bullet/" + name);
    }

    public override void onEnableUltimate()
    {
        for(int i = 0; i < 10; i++)
        {
            WeaponManager.onFireBullet(shooter, gameObject);
        }
    }
}

public interface fireType {
    bool onFireTrigger();
    float ShakeRate {
        get; }
    float FireRate { get; }
    float Range { get; }
}

public class semiAutomatic : fireType
{
    public float FireRate
    {
        get
        {
            return 200;
        }
    }

    public float Range
    {
        get
        {
            return 10;
        }
    }

    public float ShakeRate
    {
        get
        {
            return 0.1f;
        }
    }

    public bool onFireTrigger()
    {
        return Input.GetButtonDown("Fire");
    }

  
}

public class Automatic : fireType
{
    public float FireRate
    {
        get
        {
            return 200;
        }
    }

    public float Range
    {
        get
        {
            return 10;
        }
    }

    public float ShakeRate
    {
        get
        {
            return 0.05f;
        }
    }

    public bool onFireTrigger()
    {
        return Input.GetButton("Fire");
    }

  

}

