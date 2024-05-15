using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsPlayer
{
    private readonly PlayerScript player;
    private readonly Dictionary<(PlayerStates oldState, PlayerStates newState), Action> stateTransitionAnimations;


    public AnimationsPlayer(PlayerScript player)
    {
        this.player = player;
        var animator = player.GetComponent<Animator>();

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
        var direction = player.transform.localEulerAngles.y == 0 ? Directions.Left : Directions.Right;
        player.FlipPlayerToDirection(direction);
    }
}