using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public playerManager PlayerManager;
    public Image[] healthBars;
    public Image ultimateBar;
    private Text Name;
    private int nbHealthBars;
    private int maxNbHealthBars;
    int currentHealthValue { get { return (nbHealthBars-1) * 33; } }
    int maxHealthValue { get { return (maxNbHealthBars - 1) * 33; } }
    bool active = false;
    public bool isActive{
        get {
            return active;
        }
    }


    public void Awake()
    {
        Name = GetComponent<Text>();
    }

    // Use this for initialization
    public void onActivate(playerManager pManager)
    {
        active = true;
        gameObject.SetActive(true);
        PlayerManager = pManager;
        Name.text = PlayerManager.player.Name;

        nbHealthBars = Mathf.CeilToInt(PlayerManager.player.Health / 33);
        maxNbHealthBars = nbHealthBars;
        onGenerateHealthBar();
    }

    public void onDesactivate()
    {
        active = false;
        gameObject.SetActive(false);
    }


    // Update is called once per frame
    public void  Update()
    {
        if(active)
        {
            onUpdateHealthBar();
            onUpdateUltimateBar();
            onUltimateChange();
        }
        gameObject.SetActive(isActive);
     
    }

   
    

    void onGenerateHealthBar()
    {
       

        for (int i = 0; i < healthBars.Length; i++)
            healthBars[i].gameObject.SetActive(i < nbHealthBars ? true : false);

        if (nbHealthBars == 0)
            healthBars[0].gameObject.SetActive(false);

        onHealthChange();
    }


    public void onHealthChange(Image healthBar)
    {
        float health = PlayerManager.player.Health - currentHealthValue;
        healthBar.fillAmount = Mathf.Clamp01(health / 33);

        if (healthBar.fillAmount == 0)
        {
            nbHealthBars--;
            onGenerateHealthBar();
            return;
        }


       
    }

    public void onUltimateChange()
    {
       ultimateBar.fillAmount = Mathf.Clamp01(PlayerManager.player.Ultimate.Amount/ PlayerManager.player.Ultimate.MaxAmount);
    }

    public void onHealthChange()
    {
        if(nbHealthBars -1 >= 0)
        onHealthChange(healthBars[nbHealthBars-1]);
    }

    void onUpdateHealthBar()
    {
        foreach (Image img in healthBars)
        {
            if (img.isActiveAndEnabled)
            {
                img.color = returnNewAlpha(img, Mathf.Sin(Time.time * 10) / 3 + 0.8f);
            }
        }
    }

    void onUpdateUltimateBar()
    {
        ultimateBar.color = returnNewAlpha(ultimateBar, Mathf.Sin(Time.time * 10) / 3 + 0.8f);
    }


    public static Color returnNewAlpha(Graphic graphic, float alpha)
    {
        return new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }


}
