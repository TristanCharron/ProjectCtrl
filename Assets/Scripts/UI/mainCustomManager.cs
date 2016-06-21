using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainCustomManager : MonoBehaviour {

    public CharacterCustomUi mainCharacter;
    public CharacterCustomUi.playerSelectionUI playerSelection;
    public mainCharacterManager managerCharacter;
    public GameObject[] prefabCustom;

    void Awake()
    {
        mainCharacter = new CharacterCustomUi();
      //  managerCharacter = new mainCharacterManager();

        prefabCustom  = Resources.LoadAll<GameObject>("Prefab");

        robotManager robot = new robotManager();
        if (playerSelection.playerObject != null)
            Destroy(playerSelection.playerObject);
        // managerCharacter.selectPlayerList[0].setPlayerIndex(0);
        onAssignRobot(/*managerCharacter.selectPlayerList[0],*/ 1);
        Debug.Log(playerSelection.SelectionIndex);
    }



       void onAssignRobot(/*CharacterCustomUi.playerSelectionUI pUI*/ int previousIndex)
       {
           if (playerSelection.playerObject != null)
               Destroy(playerSelection.playerObject);

           Player robot = robotManager.Database[playerSelection.SelectionIndex];
           robotManager.onSetSelectRobot(playerSelection.SelectionIndex, playerSelection.PlayerIndex);

           playerSelection.playerObject = Instantiate(prefabCustom[playerSelection.SelectionIndex - 1], playerSelection.customPosition.position, Quaternion.identity) as GameObject;
           playerSelection.name.text = robot.Name;
          onSetStatsBar(previousIndex);


       }

    public void onNextCustom(Button btn)
    {
        Debug.Log(playerSelection.SelectionIndex);
     /*CharacterCustomUi.playerSelectionUI pUI = playerSelection.selectPlayerList[returnpUIbyName(btn)];*/
        int previousIndex = playerSelection.SelectionIndex;
        playerSelection.setSelectIndex(playerSelection.SelectionIndex + 1);

        if (playerSelection.SelectionIndex > prefabCustom.Length)
            playerSelection.setSelectIndex(1);

        onAssignRobot(/*playerSelection,*/ previousIndex);

    }

    public void onPrevCustome(Button btn)
    {
        
        //CharacterCustomUi.playerSelectionUI pUI = playerSelection.selectPlayerList[returnpUIbyName(btn)];
        int previousIndex = playerSelection.SelectionIndex;
        playerSelection.setSelectIndex(playerSelection.SelectionIndex - 1);
        if (playerSelection.SelectionIndex < 1)
            playerSelection.setSelectIndex(prefabCustom.Length);

        onAssignRobot(previousIndex);


    }


    void onSetStatsBar(int prevIndex)
        {
            iTween.ValueTo(playerSelection.speedShow, iTween.Hash("from", robotManager.Database[prevIndex].Speed / CharacterCustomUi.playerSelectionUI.speedMax, "to", playerSelection.speedRatio, "time", 0.3f, "onupdate", "onScale", "easetype", "easeOutCubic"));
            iTween.ValueTo(playerSelection.healthShow, iTween.Hash("from", robotManager.Database[prevIndex].Health / CharacterCustomUi.playerSelectionUI.healthMax, "to", playerSelection.healthRatio, "time", 0.3f, "onupdate", "onScale", "easetype", "easeOutCubic"));
          
        }

    public void onReturn()
    {
        SceneManager.LoadSceneAsync(1);
    }
        
}
