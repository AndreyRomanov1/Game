
using Unity.VisualScripting;
using UnityEngine;

public class CornShooterScript: PistolScript
{
    protected override float damage { get; set; } = 50;
    
    protected override void SelfStart()
    {
        bullet = Resources.Load("Weapons/Bullets/пуля_початкострела").GameObject();
    }

}
