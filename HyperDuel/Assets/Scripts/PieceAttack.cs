using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAttack : MonoBehaviour
{
    public int attackNumberCount;
    public int[] attackpercentages;
    public int[] attackAmounts;
    public AttackType[] attackTypes;
    public enum AttackType
    {
        miss,
        white,
        blue,
        purple,
        yellow
    }
    public void CreateWheelDefault() {
        attackNumberCount = 5;
        attackpercentages = new int[]{20,20,20,20,20};
        attackAmounts = new int[]{0,101,0,3,21};
        attackTypes = new AttackType[]{AttackType.miss, 
                        AttackType.white, AttackType.blue, AttackType.purple, AttackType.yellow};
    }
    public void CreateWheel(int attackNo, int[] attackPercentages, int[] attackAmounts, AttackType[] attackTypes) {
        
    }

    // public Tuple<int, AttackType> GetMoveAtPercentage( int percentage) {
    //     int 
    //     for() {

    //     }
    // }

}
