using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private FixedJoystick joystick;
    public FixedJoystick Joystick => joystick;
}
