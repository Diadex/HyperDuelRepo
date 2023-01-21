using UnityEngine;

public class Pebble : MonoBehaviour {
    public Piece piece;
    public GameObject self;
    public Pebble[] PebblesLinked;
    public bool isWaitPebble;
    public Pebble( Pebble[] pebbles) {
        piece = null;
    }

    public bool putPiece( Piece toBePut) {
        piece = toBePut;
        return true;
        
    }
    public void removePiece() {
        piece = null;
    }
}