using UnityEngine;
using System.Collections;


public class playerManager : MonoBehaviour
{
    public Player player = null;
    protected PlayerUIManager pUIManager;
    public PlayerUIManager PlayerUIManager { get { return pUIManager; } }
    public bool isActive = false;
    public GameObject Weapon;
    public GameObject ParticleObject;


    public void onActivate(int index)
    {
        player = robotManager.SelectedInGame[index];
        player.onReset(gameObject, transform.GetChild(0).gameObject, transform.GetChild(1).gameObject, index);
        ParticleObject.GetComponent<ParticleSystem>().Stop();
    }


    void Update()
    {
        if (isActive && player != null && player.gameObject != null)
            player.onUpdate();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("Attack");
        GameObject collider = col.gameObject;
        if (collider.tag == "Enemy" && player.isAttacking)
            collider.GetComponent<playerManager>().player.onMeleeHit(player.Weapon.Damage);

    }



}





public abstract class Character
{
    protected bool firing, pausing, attacking, shaking;
    protected int index;
    protected float speed, health, maxHealth, regenRate, accuracy, shield, ammo;


    public bool isAttacking { get { return attacking; } }
    public bool isPausing { get { return pausing; } }
    public bool isFiring { get { return firing; } }


    //Weapon info
    protected Weapon weapon;
    public Weapon Weapon { get { return weapon; } }


    //Getters character info
    public float Speed { get { return speed; } }
    public float Health { get { return health; } }
    public float RegenRate { get { return regenRate; } }
    public float MaxHealth { get { return maxHealth; } }
    public float Accuracy { get { return accuracy; } } 
    public float Shield { get { return shield; } }
    public float Ammo { get { return ammo; } }


    //gameObject info
    protected GameObject gObject, particleObject;
    public GameObject gameObject { get { return gObject; } }
    public GameObject ParticleObject { get { return particleObject; } }
    public void setGameObject(GameObject g) { gObject = g; }
    public void setParticleObject(GameObject g) { particleObject = g; }
    public Vector3 Position { get { return gObject.transform.position; } }

    //Basic Functions
    public abstract void onReset(GameObject playerObject, GameObject weaponObject, GameObject particleObject, int index);

    protected void onRotate()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y,
            gameObject.transform.position.z - Camera.main.transform.position.z));
        Vector3 eA = gameObject.transform.rotation.eulerAngles;
        gameObject.transform.localEulerAngles = new Vector3(eA.x, eA.y, Mathf.Atan2((screenPos.y - gameObject.transform.position.y), (screenPos.x - gameObject.transform.position.x)) * Mathf.Rad2Deg);
    }

    public Character()
    {
        maxHealth = health;
    }

    protected virtual void onMove()
    {

    }



    public IEnumerator onShake()
    {
        if (!shaking)
        {
            shaking = true;
            firing = true;
            UIEffects.onEnableShake(0.1f);
            Vector2 shake = Random.insideUnitCircle;
            gameObject.transform.position += (new Vector3(shake.x, shake.y, 0) * weapon.FireType.ShakeRate);
            firing = false;
            shaking = false;
        }

        yield break;
    }

    protected virtual void onAttack() { }

    protected void onDeath()
    {
        MonoBehaviour.Destroy(gameObject);
    }
    public void onWeaponHit(float damage)
    {

        onHit(damage);

    }
    public void onMeleeHit(float damage)
    {
        onHit(damage);
    }

    public void onHit(float damage)
    {
        pausing = true;
        health -= damage;
        UIManager.onHealthChange(index);
        if (health < 0)
        {
            onDeath();
            return;
        }

    }

    public IEnumerator onPause()
    {
        if (!pausing)
        {
            pausing = true;
            yield return new WaitForEndOfFrame();
            pausing = false;
            yield break;
        }

    }

    protected void onRegenerateHealth()
    {
        if (health < maxHealth && health % 33 > 0)
        {
            health += regenRate;
            UIManager.onHealthChange(index);
        }

    }

    public abstract void onUpdate();




}



[System.Serializable]
public class Player : Character
{
    protected IUltimate ultimate;
    public IUltimate Ultimate { get { return ultimate; } }

    protected string name;
    public string Name { get { return name; } }

    protected virtual void onCheckUltimate() { }


    public Player()
    {

    }

    public override void onReset(GameObject playerObject, GameObject weaponObject, GameObject ParticleObject, int _index)
    {
        gObject = playerObject;
        particleObject = ParticleObject;
        weapon = new Pistol(this, weaponObject);
        index = _index;
    }

    public override void onUpdate()
    {
        if (weapon != null)
            weapon.onFire();

        onMove();
        onAttack();
        onRotate();
        onRegenerateHealth();
        ultimate.onUpdate();
    }

    protected override void onMove()
    {
        base.onMove();
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        gameObject.transform.position += new Vector3(move.x, move.y, move.z) * speed * Time.deltaTime;
    }




    protected override void onAttack()
    {
        attacking = Input.GetButtonDown("Attack");
        gameObject.GetComponent<MeshRenderer>().materials[0].color = attacking ? Color.black : Color.red;
    }


}

public class robot1 : Player
{
    public robot1()
    {
        maxHealth = health;
        speed = 10;
        regenRate = 0.15f;
        shield = 50;
        ammo = 25;
        accuracy = 70;
        health = 71;
        pausing = false; firing = false;
        ultimate = new ultimate1();
        name = "Donkey Konga 3";

    }

    protected override void onCheckUltimate()
    {
        if (Input.GetMouseButtonDown(1) && ultimate.isAvailable)
        {
            weapon.onEnableUltimate();
            ultimate.onEnable();
        }
    }

    public override void onUpdate()
    {
        base.onUpdate();
        onCheckUltimate();
    }



}

public class robot2 : Player
{
    public robot2()
    {
        maxHealth = health;
        speed = 50;
        regenRate = 0.68f;
        health = 45;
        pausing = false; firing = false;
        ultimate = new ultimate1();
        name = "Vaporwave";
        shield = 80;
        ammo = 15;
        accuracy = 85;
    }

    protected override void onCheckUltimate()
    {
        if (Input.GetMouseButtonDown(1) && ultimate.isAvailable)
        {
            weapon.onEnableUltimate();
            ultimate.onEnable();
        }
    }

    public override void onUpdate()
    {
        base.onUpdate();
        onCheckUltimate();
    }
}

public class robot3 : Player
{
    public robot3()
    {
        maxHealth = health;
        speed = 75;
        regenRate = 0.02f;
        health = 89;
        pausing = false; firing = false;
        ultimate = new ultimate1();
        name = "Jean-Daniel";
        shield = 99;
        ammo = 10;
        accuracy = 90;
    }

    protected override void onCheckUltimate()
    {
        if (Input.GetMouseButtonDown(1) && ultimate.isAvailable)
        {
            weapon.onEnableUltimate();
            ultimate.onEnable();
        }
    }

    public override void onUpdate()
    {
        base.onUpdate();
        onCheckUltimate();
    }
}








