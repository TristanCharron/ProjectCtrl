using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyPlayer : MonoBehaviour
{

    public static int nbPlayersConnected = 0;
    public static bool isCurrentlyInLobby = false;
    public static bool arePlayersReady { get { return nbPlayersConnected > 0; } }
    bool isFading;
    bool hasJoined;
    public int index;
    Image UI;

    void Reset()
    {
        hasJoined = false;
        isFading = false;
        nbPlayersConnected = 0;
        UI = GetComponent<Image>();
        LobbyController.OnReadyToBeginGame(false);
    }

    public static void ChangeLobbyState(bool state)
    {
        isCurrentlyInLobby = state;
    }

    void Update()
    {
        if (isCurrentlyInLobby && !isFading)
        {
            if (Input.GetButtonDown(InputController.PRESS_A + (index + 1)))
                JoinLobby();
            if (Input.GetButtonDown(InputController.PRESS_START + (index + 1)))
                EndLobby();

        }

    }

    void JoinLobby()
    {
        hasJoined = !hasJoined;
        nbPlayersConnected = hasJoined ? nbPlayersConnected + 1 : nbPlayersConnected - 1;
        UI.CrossFadeColor(hasJoined ? Color.red : Color.white, 1f, true, true);
        isFading = true;
        Invoke("EndFade", 1);


    }

    void EndLobby()
    {
        if (arePlayersReady)
            LobbyController.onStartGame();
    }



    void EndFade()
    {
        isFading = false;
        LobbyController.OnReadyToBeginGame(arePlayersReady);
    }

    void Awake()
    {
        Reset();
    }

}
