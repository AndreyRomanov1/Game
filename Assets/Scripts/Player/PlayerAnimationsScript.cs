using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsScript : MonoBehaviour
{
    private Dictionary<(PlayerStates oldState, PlayerStates newState), Action> stateTransitionAnimations;
    private GameObject Tools { get; set; }

    public void Start()
    {
        var animator = GetComponent<Animator>();

        stateTransitionAnimations = new Dictionary<(PlayerStates oldState, PlayerStates newState), Action>
        {
            [(PlayerStates.Nothing, PlayerStates.CrouchedToJump)] = () => animator.Play("preparing for jump"),
            [(PlayerStates.CrouchedToJump, PlayerStates.Rift)] = () => animator.Play("rift"),
            [(PlayerStates.CrouchedToJump, PlayerStates.Jump)] = () => animator.Play("jump"),
            [(PlayerStates.CrouchedToJumpFromLeftWall, PlayerStates.Jump)] = () => animator.Play("jump from left wall"),
            [(PlayerStates.CrouchedToJumpFromRightWall, PlayerStates.Jump)] = () =>
            {
                FlipPlayer();
                animator.Play("jump from right wall");
            },
            [(PlayerStates.Jump, PlayerStates.HangingOnLeftWall)] = () => animator.Play("lending on left wall"),
            [(PlayerStates.Jump, PlayerStates.HangingOnRightWall)] = () => animator.Play("landing on right wall"),
            [(PlayerStates.Jump, PlayerStates.Nothing)] = () => animator.Play("lending on ground"),
            [(PlayerStates.HangingOnLeftWall, PlayerStates.Nothing)] = () => animator.Play("lending on ground"),
            [(PlayerStates.HangingOnRightWall, PlayerStates.Nothing)] = () => animator.Play("lending on ground"),
        };
        Tools = transform.Find("Tools").gameObject;
    }

    public void PlayAnimation(PlayerStates oldStates, PlayerStates newStates)
    {
        if (stateTransitionAnimations.TryGetValue((oldStates, newStates), out var action))
            action();
        else
            Debug.Log($"Анимация не найдена: {oldStates} => {newStates}");
    }

    private void FlipPlayer()
    {
        var direction = transform.localEulerAngles.y == 0 ? Directions.Left : Directions.Right;
        FlipPlayerToDirection(direction);
    }

    private void FlipPlayerToDirection(Directions flipDirection)
    {
        var angle = 180 * (int)flipDirection;
        transform.localEulerAngles = new Vector3(0, angle, 0);
        Tools.transform.localEulerAngles = new Vector3(0, angle, 0);
    }
}