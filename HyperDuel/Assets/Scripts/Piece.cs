
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject self;
    public int movementStat = 3;
    public int startMovementStat = 2;
    public int waitStatus = 0;
    public int attackStat;
    public float pieceMoveSpeed = 0.5f;
    public bool belongsToPlayerA = true;
}
