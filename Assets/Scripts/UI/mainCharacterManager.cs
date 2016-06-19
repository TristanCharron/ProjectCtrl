using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class mainCharacterManager : MonoBehaviour {

    public GameObject bgAccess;
    public GameObject bgCharacterS, bgCharacterCustom;
    public GameObject startGBtn;
    public GameObject[] prefabs;
    public const int nbPlayers = 4;
    public List<playerSelectionUI> selectPlayerList;

    [System.Serializable]
    public class playerSelectionUI
    {
        public playerSelectionUI()
        {
            confirm = false;
            selectionIndex = 1;
        }
      
        public Text name;
        public const float speedMax = 100, healthMax = 150;
        public GameObject containerArrow, readyBtn, speedShow, healthShow;
        public Transform worldPosition;
        public GameObject playerLayOut, playerObject;

        private bool confirm;
        public bool isConfirm { get { return confirm; } }
        public void setConfirm(bool state) { confirm = state; }
       

        public float speedRatio { get { return robotManager.Database[selectionIndex].Speed / speedMax;  } }
        public float healthRatio { get { return robotManager.Database[selectionIndex].Health / healthMax; } }

        private int selectionIndex,playerIndex;
        public int SelectionIndex { get { return selectionIndex; } }
        public int PlayerIndex { get { return playerIndex; } }
        public void setSelectIndex(int i) { selectionIndex = i; }
        public void setPlayerIndex(int i) { playerIndex = i; }


    }


    // Use this for initialization
    void Awake() {
        prefabs = Resources.LoadAll<GameObject>("Prefab");
        robotManager robot = new robotManager();
    }

    // Update is called once per frame
    void Update() {

        foreach (playerSelectionUI pUI in selectPlayerList)
        {
            if (Input.GetMouseButtonDown(0) && !pUI.containerArrow.activeInHierarchy && pUI.playerLayOut.activeInHierarchy)
            {
                pUI.containerArrow.SetActive(true);
                pUI.readyBtn.SetActive(false);
            }
        }

        if(Input.GetMouseButton(1))
        {
            onStartGame();
        }

    }

    void onStartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void onStartCharacterSelection()
    {
        if (bgAccess.activeInHierarchy)
        {
            bgCharacterS.SetActive(true);
            bgAccess.SetActive(false);

            for(int i = 0; i < selectPlayerList.Count;i++)
            {
                selectPlayerList[i].readyBtn.SetActive(false);
                if (!selectPlayerList[i].containerArrow.activeInHierarchy)
                    selectPlayerList[i].containerArrow.SetActive(true);

                selectPlayerList[i].setPlayerIndex(i);
                onAssignRobot(selectPlayerList[i],1);
            }
            startGBtn.SetActive(false);



        }

        else if (bgCharacterS.activeInHierarchy)
        {
            bgCharacterS.SetActive(false);
            bgAccess.SetActive(true);

            foreach (playerSelectionUI pUI in selectPlayerList)
            {
                Destroy(pUI.playerObject);
            }
        }


    }

    public void onStartCharacterCustomisation()
    {
        if (bgAccess.activeInHierarchy)
        {
            bgAccess.SetActive(false);
            bgCharacterCustom.SetActive(true);
        }
    }


    public int returnpUIbyName(Button btn)
    {
        int id = int.Parse(btn.name.Substring(btn.name.Length - 1, 1));
        return id - 1;
    }

    public void onNext(Button btn)
    {
        
        playerSelectionUI pUI = selectPlayerList[returnpUIbyName(btn)];
        int previousIndex = pUI.SelectionIndex;
        pUI.setSelectIndex(pUI.SelectionIndex + 1);

        if (pUI.SelectionIndex > prefabs.Length)
            pUI.setSelectIndex(1);

        onAssignRobot(pUI, previousIndex);

    }

    void onAssignRobot( playerSelectionUI pUI, int previousIndex)
    {
        if (pUI.playerObject != null)
            Destroy(pUI.playerObject);

        Player robot = robotManager.Database[pUI.SelectionIndex];
        robotManager.onSetSelectRobot(pUI.SelectionIndex, pUI.PlayerIndex);

       pUI.playerObject = Instantiate(prefabs[pUI.SelectionIndex - 1], pUI.worldPosition.position, Quaternion.identity) as GameObject;
       pUI.name.text = robot.Name;
       onSetStatsBar(pUI, previousIndex);
    }


   void  onSetStatsBar(playerSelectionUI pUI, int prevIndex)
    {
        iTween.ValueTo(pUI.speedShow, iTween.Hash("from", robotManager.Database[prevIndex].Speed / playerSelectionUI.speedMax, "to", pUI.speedRatio, "time", 0.3f, "onupdate", "onScale", "easetype", "easeOutCubic"));
        iTween.ValueTo(pUI.healthShow, iTween.Hash("from", robotManager.Database[prevIndex].Health / playerSelectionUI.healthMax, "to", pUI.healthRatio, "time", 0.3f, "onupdate", "onScale", "easetype", "easeOutCubic"));
    }





    public void onPrev(Button btn)
    {

        playerSelectionUI pUI = selectPlayerList[returnpUIbyName(btn)];
        int previousIndex = pUI.SelectionIndex;
        pUI.setSelectIndex(pUI.SelectionIndex - 1);
        if (pUI.SelectionIndex < 1)
            pUI.setSelectIndex(prefabs.Length);

        onAssignRobot(pUI, previousIndex);


    }

    public void onConfirm(Button btn)
    {
        if(returnpUIbyName(btn) < selectPlayerList.Count)
        {
            onSelect(returnpUIbyName(btn));
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
        selectPlayerList[id].setConfirm(true);
        selectPlayerList[id].readyBtn.SetActive(true);
        selectPlayerList[id].containerArrow.SetActive(false);
    }

    public void onSave()
    {

    }

   



  }


