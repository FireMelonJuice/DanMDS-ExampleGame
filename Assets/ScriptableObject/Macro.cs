using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MacroAction
{
    Wait,
    MoveLeft,
    MoveRight,
    Jump,
    JumpDown,
    Attack,
}

[System.Serializable]
public struct MacroCommand
{
    public MacroAction action;
    public float duration;
}

[CreateAssetMenu(fileName = "new Macro", menuName = "Scriptable Object/Macro", order = int.MaxValue)]
public class Macro : ScriptableObject
{
    [SerializeField]
    public List<MacroCommand> commands;
}
