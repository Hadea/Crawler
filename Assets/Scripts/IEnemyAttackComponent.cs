using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttackComponent
{
    void Attack(Ray ray, float distance);
}
