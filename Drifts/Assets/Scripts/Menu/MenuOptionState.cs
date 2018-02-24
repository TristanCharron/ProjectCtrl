using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MenuOptionStateID
{
    PLAYDEATHMATCH = 0,
    PLAYBELL = 1,
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

    public const float TransitionTime = 0.1f;

    private void Awake()
    {
        TextMeshProComponent = GetComponent<TextMeshPro>();
        TextMeshProComponent.text = Text;
        SetFocus(false);
    }

    public virtual IEnumerator EnterState()
    {
        SetFocus(true);
        yield return new WaitForSecondsRealtime(TransitionTime);
    }

    public virtual IEnumerator ExitState()
    {
        SetFocus(false);
        yield break;
    }

    protected virtual void SetFocus(bool isFocus)
    {
        TextMeshProComponent.color = isFocus ? EnabledColor : DisabledColor;
    }


    public Action Select;




}
