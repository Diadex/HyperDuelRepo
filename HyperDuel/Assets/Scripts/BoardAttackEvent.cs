using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAttackEvent : MonoBehaviour
{
    Piece AttackingPiece;
    Piece DefendingPiece;
    public void SetAttacksAs( Piece Attacker, Piece Defender) {
        AttackingPiece = Attacker;
        DefendingPiece = Defender;
    }/*
    public string GetPieceAttackFromEachPiece( ) {
        AttackingPiece.
    }*/
    
}
