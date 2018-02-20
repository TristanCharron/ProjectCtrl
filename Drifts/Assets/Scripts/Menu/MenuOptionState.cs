using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MenuOptionStateID
{
    PLAYDEATHMATCH = 0,
    PLAYSCORE = 1,
    QUIT = 2
}


[RequireComponent(typeof(TextMeshPro))]
public abstract class MenuOptionState : MonoBehaviour
{

    public string Text { protected set; get; }

    public TextMeshPro TextMeshProComponent { private set; get; }

    public MenuOptionStateID EnumID { protected set; get; }

    public Color EnabledColor { get { return Color.white; } }
    public Color DisabledColor { get { return EnabledColor * 0.8f; } }

    private void Awake()
    {
        TextMeshProComponent = GetComponent<TextMeshPro>();
        TextMeshProComponent.text = Text;
        SetFocus(false);
    }

    public virtual IEnumerator EnterState()
    {
        SetFocus(true);
        yield return new WaitForSecondsRealtime(0.3f);
    }

    public virtual IEnumerator ExitState()
    {
        SetFocus(false);
        yield break;
    }

    protected virtual void SetFocus(bool isFocus)
    {
        //TextMeshProComponent.CrossFadeColor(isFocus ? EnabledColor : DisabledColor , 0.3f, true, true);
        TextMeshProComponent.color = isFocus ? EnabledColor : DisabledColor;
    }


    public Action Select;




}
