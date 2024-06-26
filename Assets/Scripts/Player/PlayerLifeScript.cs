using System;
using UnityEngine;

public class PlayerLifeScript : MonoBehaviour, IDamageable
{
    private const float MaxHealthPoints = 200f;
    private float healthPoints;
    private UnityEngine.UI.Image healthPointsBar;

    public void Start()
    {        
        healthPoints = MaxHealthPoints;
        healthPointsBar = transform.Find("Tools").Find("Main Camera").Find("StatesInspector").Find("HP bar")
            .GetComponent<UnityEngine.UI.Image>();
    }

    public void Heal(float count)
    {
        healthPoints = Math.Min(healthPoints + count, MaxHealthPoints);
        healthPointsBar.fillAmount = healthPoints / MaxHealthPoints;
    }

    public void TakeDamage(float damage)
    {
        player.sound.Damage();
        if (Model.IsEducation)
            damage *= 0.1f;
        else
            damage *= 0.8f;
        healthPoints -= damage;
        if (healthPoints <= 0)
            Die();

        healthPointsBar.fillAmount = healthPoints / MaxHealthPoints;
    }

    private static void Die()
    {
        GameSounds.EndGame();
        CurrentGame.KillCurrentGame();

        UI.Show(UI.LostGameGameObject);
    }
}