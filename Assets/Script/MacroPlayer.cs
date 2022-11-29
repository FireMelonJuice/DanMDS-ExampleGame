using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacroPlayer : MonoBehaviour
{
    public List<Macro> macro;
    public Player player;

    private int idx;
    private int phase;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        idx = 0;
        phase = 0;
        StartCoroutine(PlayCommand());
    }

    IEnumerator PlayCommand()
    {
        if (phase == 0)
        {
            idx = Random.Range(0, macro.Count);
        }

        switch (macro[idx].commands[phase].action)
        {
            case MacroAction.Wait:
                break;

            case MacroAction.MoveLeft:
                player.SetLeft(true);
                break;

            case MacroAction.MoveRight:
                player.SetRight(true);
                break;

            case MacroAction.Jump:
                player.SetJump(true);
                break;

            case MacroAction.JumpDown:
                player.SetJump(true);
                player.SetDown(true);
                break;

            case MacroAction.Attack:
                player.SetAttack(true);
                break;
        }

        if (macro[idx].commands[phase].action == MacroAction.Wait)
        {
            yield return new WaitForSeconds(macro[idx].commands[phase].duration * Random.Range(0.8f, 1.2f));
        }
        else
        {
            yield return new WaitForSeconds(macro[idx].commands[phase].duration);
        }

        switch (macro[idx].commands[phase].action)
        {
            case MacroAction.Wait:
                break;

            case MacroAction.MoveLeft:
                player.SetLeft(false);
                break;

            case MacroAction.MoveRight:
                player.SetRight(false);
                break;

            case MacroAction.Jump:
                player.SetJump(false);
                break;

            case MacroAction.JumpDown:
                player.SetJump(false);
                player.SetDown(false);
                break;

            case MacroAction.Attack:
                player.SetAttack(false);
                break;
        }

        phase = (phase + 1) % macro[idx].commands.Count;
        StartCoroutine(PlayCommand());
    }
}
