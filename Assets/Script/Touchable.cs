﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class Touchable : Text {

    protected override void Awake()
    {
        base.Awake();
    }




}


[CustomEditor(typeof(Touchable))]
public class Touchable_Editor: Editor
{
    public override void OnInspectorGUI()
    {

    }
}