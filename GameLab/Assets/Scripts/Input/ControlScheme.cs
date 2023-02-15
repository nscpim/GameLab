using System.Collections;
using System.Collections.Generic;
using System;

public class ControlScheme
{
    public bool isActive;
    public Dictionary<InputAction, InputBinding> input;
    public InputBinding[] _input;


    public ControlScheme()
    {
        input = new Dictionary<InputAction, InputBinding>();
        _input = new InputBinding[Enum.GetNames(typeof(InputAction)).Length];
    }

    // Update is called once per frame
    public void Update()
    {
        if (!isActive || input == null || _input.Length == 0)
        {
            return;
        }

        foreach (InputBinding binding in _input)
        {
            binding.HandleBinding();
        }
    }

    public void AssignInput(Player player)
    {
        _input[(int)InputAction.Test].action = player.Test;
    }

}
