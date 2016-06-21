using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bullet : MonoBehaviour {
    private Character shooter;
    private Vector3 direction;
    private Weapon weapon;
    private bool[] movementPattern;
    private MeshRenderer render;
    private Rigidbody rBody;
    bool active = false;
    
    // Use this for initialization
    public void setInfo(Character character)
    {
        shooter = character;
        weapon = shooter.Weapon;
    }

    void Awake () {
        render = GetComponent<MeshRenderer>();
        rBody = GetComponent<Rigidbody>();
    }  

    public void onStart(Vector3 position)
    {
        gameObject.SetActive(true);
        render.materials[0].color = new Color(render.materials[0].color.r, render.materials[0].color.g, render.materials[0].color.b, 1);
        onSetDirection(weapon.gameObject.transform.position);
        Invoke("onDestroy", 2);
       
    }

    void onSetDirection(Vector3 pos)
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = new Vector3(direction.x, direction.y, 90);

        transform.position = pos;
        transform.LookAt(direction);
        active = true;
    }

    void Update()
    {
        if(active)
        {
            onChangeAlpha();
            onMove();
        }
        isActive();

    }

 
    void isActive()
    {
        if (shooter == null)
        {
            active = false;
            onDestroy();
        }
    }


    void onMove()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, shooter.gameObject.transform.position.z);
        rBody.AddForce(transform.forward * weapon.FireType.FireRate);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && active)
        {
          
            playerManager playerManager = col.gameObject.GetComponent<playerManager>();
            if (playerManager.player.Index != shooter.Index)
            {
                playerManager.player.onWeaponHit(weapon.Damage);
                StartCoroutine(playerManager.player.onPause());
            }

           
        }

        onDestroy();
       

    }

    void onDestroy()
    {
        //iTween.Stop(gameObject);
        active = false;
        gameObject.SetActive(false);
        rBody.velocity = new Vector3(0f, 0f, 0f);
        rBody.angularVelocity = new Vector3(0f, 0f, 0f);


    }

    void onChangeAlpha()
    {
        Color c = render.materials[0].color;
        c.a -= Time.deltaTime;
        render.materials[0].color = c;
    }
}
