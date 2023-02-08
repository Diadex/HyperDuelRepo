
using System;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject self;
    public int movementStat = 3;
    public int startMovementStat = 2;
    public int waitStatus = 0;
    public readonly int healingStationAfterWait = 3;
    public int attackStat;
    public float pieceMoveSpeed = 0.5f;
    public bool belongsToPlayerA = true;
    public bool surroundDeath = false;
    public PieceStatus statusUI; 
    public PieceAttack attackStatus;
    private void Start() {
        statusUI = GetComponent<PieceStatus>();
    }

    public void HealingStationAfter() {
        if ( waitStatus < healingStationAfterWait)  {
            waitStatus = healingStationAfterWait;
        }
    }

    public void EnableWaitUIStatus() {
        if (waitStatus > 0) {
            statusUI.SetWaitText( waitStatus);
        }
    }
    public void DisableWaitUIStatus() {
        if (waitStatus <= 0) {
            statusUI.SetWaitText( waitStatus);
        }
    }

    public bool CanMove()
    {
        if ( waitStatus > 0) {
            return false;
        }
        return true;
    }

    // public Tuple<int, PieceAttack.AttackType> GetPieceMoveAtPercentage( int percentage) {
        
    // }
}
