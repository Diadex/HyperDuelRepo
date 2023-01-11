using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BoardShadow : Board
{
    public Pebble[] boardPlacement;
    public Pebble[] waitPebbles;
    public Piece[] pieces;
    private float timeRemaining = 1;
    private float hopWaitTime = 0.2f;
    public float hopDurationTime = 0.7f;
    private float hopDurationCounter = 0f;
    public float hopUpwardsAmount = 0.1f;
    Vector3 smoothDampCurrentVelocity;
    [SerializeField]
    private AnimationCurve hopCurve1;
    [SerializeField]
    private AnimationCurve hopCurve2;


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
        pieces[0].self.transform.position = waitPebbles[0].self.transform.position + new Vector3(0, 0.5f, 0);
        previous = waitPebbles[0];
    }

    // Update is called once per frame
    void Update()
    {
        Test();
    }

    private void Test() {
        if ( Counter( ref timeRemaining)) { // Update every 1 seconds
            
            
            timeRemaining = 0.3f;
            //Debug.Log(tempName + " to " + tempPebble.self.name);
        }

        if ( Hop( pieces[0], (previous.transform.position + new Vector3(0, 0.5f, 0)) ,( tempPebble.self.transform.position + new Vector3(0, 0.5f, 0)))) {
            // hop complete
            string tempName = tempPebble.self.name;
            do {
                nextIndex = rnd.Next( 0, tempPebble.PebblesLinked.Length);  // creates a number between 1 and 12
            } while (tempPebble.PebblesLinked[nextIndex] == previous);
            previous = tempPebble;
            tempPebble = tempPebble.PebblesLinked[nextIndex];
        }
    }
    private bool Hop( Piece piece, Vector3 startPebbleLoc, Vector3 endPebbleLoc) {
        //Vector3 targetLocation = endPebbleLoc;
        Vector3 halfway = (startPebbleLoc + endPebbleLoc) / 2;
        hopDurationCounter += Time.deltaTime;
        float percentageHop = hopDurationCounter * piece.pieceMoveSpeed / hopDurationTime;
        //percentageHop = percentageHop * piece.pieceMoveSpeed;
        if ( percentageHop <= 0.5f) {
            piece.self.transform.position = Vector3.Lerp( startPebbleLoc, halfway + new Vector3(0, hopUpwardsAmount, 0), hopCurve1.Evaluate(percentageHop*2));
            return false;
        }
        else if( (piece.self.transform.position - endPebbleLoc).magnitude >= 0.01) {
            //piece.self.transform.position = Vector3.SmoothDamp( piece.self.transform.position, targetLocation, ref smoothDampCurrentVelocity, piece.pieceMoveSpeed * Time.deltaTime);
            piece.self.transform.position = Vector3.Lerp( halfway + new Vector3(0, hopUpwardsAmount, 0), endPebbleLoc, hopCurve2.Evaluate( (percentageHop - 0.5f) * 2));

            return false;
        }
        else if ( !Counter( ref hopWaitTime)) {
            piece.self.transform.position = endPebbleLoc;
            return false;
        }
        else {
            hopWaitTime = 0.2f;
            hopDurationCounter = 0f;
            return true;
        }
    }
    private bool UpwardsJump() {
        return false;
    }

    public void MovePieceTo(Piece piece, Pebble pebble) {
        piece.self.transform.position = pebble.self.transform.position + new Vector3(0, 0.5f, 0);
    }
}
