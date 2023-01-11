using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BoardShadow : Board
{
    public Pebble[] boardPlacement;
    public Piece[] pieces;
    private float timeRemaining = 1;

    public bool Counter( ref float f) {
        if (f > 0)
        {
            f -= Time.deltaTime;
            return false;
        }
        return true;
    }


    // test variables
    public Pebble tempPebble, previous;
    private readonly Random rnd  = new Random();
    int nextIndex = 0;
    void Start() {
        tempPebble = boardPlacement[0];

        previous = null;
    }

    // Update is called once per frame
    void Update()
    {
        Test();
    }

    private void Test() {
        if ( Counter( ref timeRemaining)) { // update at 1 sec intervals
            Hop( pieces[0], tempPebble);
            
            string tempName = tempPebble.self.name;
            do {
                nextIndex = rnd.Next( 0, tempPebble.PebblesLinked.Length);  // creates a number between 1 and 12
            } while (tempPebble.PebblesLinked[nextIndex] == previous);
            previous = tempPebble;
            tempPebble = tempPebble.PebblesLinked[nextIndex];


            timeRemaining = 0.3f;
            //Debug.Log(tempName + " to " + tempPebble.self.name);
        }
    }

    private void Hop( Piece piece, Pebble pebble) {
        Vector3 targetLocation = pebble.self.transform.position+ new Vector3(0, 0.5f, 0);
        // float countTime = 0.3f;
        // while( !Counter(countTime)) {
        //while( (piece.self.transform.position - targetLocation).magnitude >= 0.1) {
            //piece.self.transform.Translate( targetLocation * piece.pieceMoveSpeed * Time.deltaTime);
            //Debug.Log("1");
        //}
        Debug.Log("2");
        piece.self.transform.position = targetLocation;
    }

    public void MovePieceTo(Piece piece, Pebble pebble) {
        piece.self.transform.position = pebble.self.transform.position + new Vector3(0, 0.5f, 0);
    }
}
