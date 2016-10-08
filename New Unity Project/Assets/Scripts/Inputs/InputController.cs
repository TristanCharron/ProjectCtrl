using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{

    public static OrbController Instance { get { return instance; } }
    static OrbController instance;

    public const string CONTROLLER_PS3 = "PLAYSTATION(R)3";
    public const string CONTROLLER_XBOXONE = "Xbox One For Windows";
    public const string 
        PRESS_L = "L_Press_", 
        PRESS_R = "R_Press_", 
        PRESS_Y = "Y_Press_",
        PRESS_A = "A_Press_",
        PRESS_START = "Start_Press_",
        VERTICAL_MOVE = "Vertical_Move_",
        HORIZONTAL_MOVE = "Horizontal_Move_";

    public enum controllerType { NONE = 0, PLAYSTATION3 = 1, XBOXONE = 2, };

    public string[] ControllerList { get { return Input.GetJoystickNames(); } }

    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < ControllerList.Length; i++)
        {
            Debug.Log(ControllerList[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static controllerType OnGetControllerType(string controllerTxt)
    {
        if(controllerTxt.Contains(CONTROLLER_PS3))
        {
            return controllerType.PLAYSTATION3;
        }
        else if(controllerTxt.Contains(CONTROLLER_XBOXONE))
        {
            return controllerType.XBOXONE;
        }
        else
        {
            return controllerType.NONE;
        }
        
    }

}
