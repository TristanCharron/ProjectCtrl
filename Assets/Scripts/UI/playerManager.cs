using UnityEngine;
using System.Collections;


public class playerManager : MonoBehaviour
{
    public Player player = null;
    protected PlayerUIManager pUIManager;
    public PlayerUIManager PlayerUIManager { get { return pUIManager; } }
    bool active = true;
    bool alive = true;
    public bool isActive { get { return active; } }
    public bool isAlive { get { return alive; } }
    public GameObject Weapon;
    public GameObject ParticleObject;

    public void setAlive(bool state)
    {
        alive = state;
    }

    public void setActive(bool state)
    {
        active = state;
    }

    public void onActivate(int index)
    {
        player = robotManager.SelectedInGame[index];
        name = player.Name;
        player.onReset(gameObject, transform.GetChild(0).gameObject, transform.GetChild(1).gameObject, index);
        ParticleObject.GetComponent<ParticleSystem>().Stop();
    }


    void Update()
    {
        if (isActive && player != null && player.gameObject != null)
            player.onUpdate();
    }

    /*
    void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("Attack");
        GameObject collider = col.gameObject;
        if (collider.tag == "Enemy" && player.isAttacking)
            collider.GetComponent<playerManager>().player.onMeleeHit(player.Weapon.Damage);
        if (collider.tag == "Enemy" && player.isAttacking)
            collider.GetComponent<playerManager>().player.onMeleeHit(player.Weapon.Damage);

    }
    */
   

}







public abstract class Character
{
    protected bool firing, pausing, attacking, shaking;
    protected int index;
    protected float speed, health, maxHealth, regenRate;


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
    public int Index { get { return index; } }

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
        combatManager.onPlayerDeath(index);
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

    protected IMovable movable;
    public IMovable Movable { get { return movable; } }

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

        movable.onMove(gameObject);
        onAttack();
        onRotate();
        onRegenerateHealth();
        ultimate.onUpdate();
    }

   



    protected override void onAttack()
    {
        attacking = Input.GetButtonDown("Attack");
        gameObject.GetComponent<MeshRenderer>().materials[0].color = attacking ? Color.black : Color.red;
    }


}





