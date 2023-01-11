using UnityEngine;

public class Pebble : MonoBehaviour {
    public Piece piece;
    public GameObject self;
    public Pebble[] PebblesLinked;

    public Pebble( Pebble[] pebbles) {
        piece = null;
    }

    public bool putPiece( Piece toBePut) {
        if ( piece == null) {
            return false;
        } else {
            piece = toBePut;
            return true;
        }
    }
    public void removePiece() {
        piece = null;
    }
}