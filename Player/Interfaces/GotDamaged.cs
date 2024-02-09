using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GotDamaged
{
    void Damage();
    void Die();
    void RestartLevel();
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
}
