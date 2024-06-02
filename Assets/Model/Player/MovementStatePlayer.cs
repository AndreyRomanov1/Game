﻿using System.Collections;
using UnityEngine;

public class MovementStatePlayer
{
    private readonly PlayerScript player;

    public MovementStatePlayer(PlayerScript player)
    {
        this.player = player;
    }

    public IEnumerator MovementStateCoroutine()
    {
        
        yield return new WaitForFixedUpdate();
        while (true)
        {
            for (var k0 = 0; player.PlayerState == PlayerStates.Nothing && k0 < 10; k0++)
            {
                if (!player.IsGrounded)
                    if (player.IsTouchedLeftWall)
                        player.PlayerState = PlayerStates.HangingOnLeftWall;
                    else if (player.IsTouchedRightWall)
                        player.PlayerState = PlayerStates.HangingOnRightWall;
                yield return null;
            }

            for (var k0 = 0;
                 player.PlayerState is PlayerStates.HangingOnLeftWall or PlayerStates.HangingOnRightWall && k0 < 10;
                 k0++)
            {
                if (player.IsGrounded)
                    player.PlayerState = PlayerStates.Nothing;
                yield return null;
            }

            if (player.PlayerState == PlayerStates.Rift)
            {
                player.Print();
                yield return new WaitForSeconds(player.RiftDurationTime);
                if (!player.IsGrounded)
                    player.PlayerState = PlayerStates.Jump;
                else
                {
                    player.Physic.velocity = new Vector2(0, player.Physic.velocity.y);
                    player.PlayerState = PlayerStates.Nothing;
                }

                player.Print();
            }

            if (player.PlayerState is PlayerStates.HangingOnLeftWall or PlayerStates.HangingOnRightWall)
            {
                if (player.IsGrounded)
                    player.PlayerState = PlayerStates.Nothing;
                else if (!player.IsTouchedLeftWall && !player.IsTouchedRightWall)
                    player.PlayerState = PlayerStates.Jump;
            }

            if (player.PlayerState == PlayerStates.Jump)
            {
                player.Print();

                // Debug.Log("1 Начало прыжка");
                var k = 0;
                for (; k < 15 && (player.IsGrounded || player.IsTouchedLeftWall || player.IsTouchedRightWall); k++)
                    yield return null; // Ждём, пока достаточно далеко отлетим от стены
                // Debug.Log($"2 Оторвал ноги от земли или стены. Был возле стены {k} тиков");
                while (!player.IsGrounded && !player.IsTouchedLeftWall && !player.IsTouchedRightWall)
                    yield return null; // Когда коснёмся поверхности, обнуляем X у скорости
                // Debug.Log("3 Коснулся земли или стены");
                player.Physic.velocity = new Vector2(0, player.Physic.velocity.y);
                if (player.IsGrounded)
                    player.PlayerState = PlayerStates.Nothing;
                else if (player.IsTouchedLeftWall)
                    player.PlayerState = PlayerStates.HangingOnLeftWall;
                else if (player.IsTouchedRightWall)
                    player.PlayerState = PlayerStates.HangingOnRightWall;
                // Debug.Log("4 Конец прыжка");
                player.Print();
            }

            yield return null;
        }
    }
}