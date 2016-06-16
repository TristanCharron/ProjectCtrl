using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[ExecuteInEditMode]
public class mainCharacterManager : MonoBehaviour {

    public GameObject bgAccess;
    public GameObject bgCharacterS;
    public GameObject startGBtn;
    public GameObject[] prefabs;
    public List<robot> robots;
    public const int nbPlayers = 4;
    public List<playerSelectionUI> selectPlayerList;

    [System.Serializable]
    public class playerSelectionUI
    {
        public playerSelectionUI()
        {

        }
        public bool isConfirm;
        public Text health, speed, name;
        public GameObject containerArrow, readyBtn;
        public Transform worldPosition;
        public GameObject playerLayOut, playerObject;
        public int selectionIndex;

    }
    

    // Use this for initialization
    void Awake() {
       robots = new List<robot>();
        for(int i = 0; i <= nbPlayers; i++)
        {
            robots.Add(onGenerateRobot(i));
        }

        prefabs = Resources.LoadAll<GameObject>("Prefab");

    }

    // Update is called once per frame
    void Update() {

        foreach(playerSelectionUI pUI in selectPlayerList)
        {
            if(Input.GetMouseButtonDown(0) && !pUI.containerArrow.activeInHierarchy && pUI.playerLayOut.activeInHierarchy)
            {
                pUI.containerArrow.SetActive(true);
                pUI.readyBtn.SetActive(false);
            }
        }

    }

    public void onStartCharacterSelection()
    {
        if(bgAccess.activeInHierarchy)
        {
            bgCharacterS.SetActive(true);
            bgAccess.SetActive(false);

            foreach (playerSelectionUI pUI in selectPlayerList)
            {
                pUI.readyBtn.SetActive(false);
                if (!pUI.containerArrow.activeInHierarchy)
                    pUI.containerArrow.SetActive(true);

                assignRobot(pUI);
            }
            startGBtn.SetActive(false);
           


        } 

        else if(bgCharacterS.activeInHierarchy)
        {
            bgCharacterS.SetActive(false);
            bgAccess.SetActive(true);

            foreach(playerSelectionUI pUI in selectPlayerList)
            {
                Destroy(pUI.playerObject);
            }
        }


    }


    public playerSelectionUI returnpUIbyName(Button btn)
    {
        Debug.Log(btn.name.Substring(btn.name.Length - 1, 1));
        int id = int.Parse(btn.name.Substring(btn.name.Length - 1, 1));
        return selectPlayerList[id-1];
    }

    public void onNext(Button btn)
    {
        playerSelectionUI pUI = returnpUIbyName(btn);
        if (pUI.selectionIndex > prefabs.Length )
        {
            pUI.selectionIndex = 1;
           
        }
        else if(pUI.selectionIndex < prefabs.Length )
        {
            pUI.selectionIndex++;
        }
        assignRobot(pUI);

    }

    void assignRobot( playerSelectionUI pUI)
    {
        if (pUI.playerObject != null)
            Destroy(pUI.playerObject);

        pUI.playerObject = Instantiate(prefabs[pUI.selectionIndex - 1], pUI.worldPosition.position, Quaternion.identity) as GameObject;
        pUI.health.text = robots[pUI.selectionIndex].health.ToString();
        pUI.speed.text = robots[pUI.selectionIndex].speed.ToString();
        pUI.name.text = robots[pUI.selectionIndex].name;

    }

    public void onPrev(Button btn)
    {

        playerSelectionUI pUI = returnpUIbyName(btn);
        if (pUI.selectionIndex < 1)
        {
            pUI.selectionIndex = 1;

        }
        else if (pUI.selectionIndex >= 2)
        {
            pUI.selectionIndex--;
        }
        assignRobot(pUI);


    }

    public void onConfirm(Button btn)
    {
        Debug.Log(btn.name.Substring(btn.name.Length - 1, 1));
        int id = int.Parse(btn.name.Substring(btn.name.Length - 1, 1));
        if(id < selectPlayerList.Count)
        {
            onSelect(id);
            startGBtn.SetActive(arePlayersReady());
        }
           
    }

    bool arePlayersReady()
    {
        bool isReady = true;
        foreach (playerSelectionUI pUI in selectPlayerList)
        {
            if (pUI.isConfirm == false)
            {
                isReady = false;
                break;
            }
        }
        return isReady;
    }

    void onSelect(int id)
    {
        selectPlayerList[id].isConfirm = true;
        selectPlayerList[id].readyBtn.SetActive(true);
        selectPlayerList[id].containerArrow.SetActive(false);
    }

    public void onSave()
    {

    }

    robot onGenerateRobot(int id)
    {
        switch(id)
        {
            case 1:
                return new robotP1();
             

            case 2:
                return new robotP2();

            case 3:
                return new robotP3();
              
        }

        return null;
    }



  }


[System.Serializable]
public class robot
{

    public float health;
    public float speed;
    public string ult;
    public string name;

    public robot()
    {
        
    }

 
}

[System.Serializable]
public class robotP1: robot
{
    public robotP1()
    {
        health = 69;
        speed = 105;
        ult = "donkey pongo";
        name = "Diddy";
    }
}

public class robotP2 : robot
{
    public robotP2()
    {
        health = 54;
        speed = 155;
        ult = "Grosse Sucette";
        name = "Cool";
    }
}

public class robotP3 : robot
{
    public robotP3()
    {
        health = 12;
        speed = 205;
        ult = "Tetez un boutte";
        name = "Homme3D";
    }
}
