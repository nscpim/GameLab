using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputBinding
{

    public Action action;

    public string axis;
    public KeyCode keyCode;
    public int mouseButton;
    public KeyStrokeType strokeType;

    // Start is called before the first frame update
    public InputBinding()
    {
        axis = "";
        keyCode = KeyCode.None;
        mouseButton = -1;
        strokeType = KeyStrokeType.any;
    }

    public void HandleBinding()
    {
        if (action == null)
        {
            return;
        }
        if (axis.Length != 0 && Input.GetAxis(axis) != 0)
        {
            action.Invoke();
        }
        else if (keyCode != KeyCode.None) 
        {
            bool invoke = false;

            switch (strokeType)
            {
                case KeyStrokeType.up:
                    if (Input.GetKeyUp(keyCode))
                    {
                        invoke = true;
                    }
                    break;
                case KeyStrokeType.down:
                    if (Input.GetKeyDown(keyCode))
                    {
                        invoke = true;
                    }
                    break;
                case KeyStrokeType.any:
                    if (Input.GetKey(keyCode))
                    {
                        invoke = true;
                    }
                    break;
                default:
                    break;
            }
            if (invoke)
            {
                action.Invoke();
            }
        }
        else if (mouseButton > -1)
        {
            bool invoke = false;

            switch (strokeType)
            {
                case KeyStrokeType.up:
                    if (Input.GetMouseButtonUp(mouseButton))
                    {
                        invoke = true;
                    }
                    break;
                case KeyStrokeType.down:
                    if (Input.GetMouseButtonDown(mouseButton))
                    {
                        invoke = true;
                    }
                    break;
                case KeyStrokeType.any:
                    if (Input.GetMouseButton(mouseButton))
                    {
                        invoke = true;
                    }
                    break;
                default:
                    break;
            }
            if (invoke)
            {
                action.Invoke();
            }
        }
    }
}
public enum KeyStrokeType 
{
 up,
 down,
 any
}
