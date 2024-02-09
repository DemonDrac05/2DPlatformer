using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerAttack
{
    public float attackTime {  get; set; }
    public float attackDuration { get; set; }
    public float attackCD { get; set; }
    public float attackCDtime { get; set; }
}
