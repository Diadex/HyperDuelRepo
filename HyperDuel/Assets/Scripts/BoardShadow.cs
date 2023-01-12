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
    public float hopWaitTime = 0.1f;
    private float hopWaitCounter = 0.1f;
    public float hopDurationTime = 0.7f;
    private float hopDurationCounter = 0f;
    public float hopUpwardsAmount = 0.1f;
    public float inPlaceUpwardsAmount = 1.2f;
    [SerializeField]
    private AnimationCurve hopCurve1;
    [SerializeField]
    private AnimationCurve hopCurve2;
    [SerializeField]
    private AnimationCurve hopCurve3;
    [SerializeField]
    private AnimationCurve hopCurve4;


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
        //UpwardsJump( pieces[0], (previous.transform.position));
        if ( Hop( pieces[0], (previous.transform.position) ,( tempPebble.self.transform.position))) {
             // hop complete
             do {
                 nextIndex = rnd.Next( 0, tempPebble.PebblesLinked.Length);  // creates a number between 1 and 12
             } while (tempPebble.PebblesLinked[nextIndex] == previous);
             previous = tempPebble;
             tempPebble = tempPebble.PebblesLinked[nextIndex];
        }
    }
    private bool Hop( Piece piece, Vector3 startPebble, Vector3 endPebble) {
        Vector3 startPebbleLoc = startPebble + new Vector3(0, 0.5f, 0);
        Vector3 endPebbleLoc = endPebble + new Vector3(0, 0.5f, 0);
        //Vector3 targetLocation = endPebbleLoc;
        Vector3 halfway = (startPebbleLoc + endPebbleLoc) / 2;
        hopDurationCounter += Time.deltaTime;
        float percentageHop = hopDurationCounter * piece.pieceMoveSpeed / hopDurationTime;
        //percentageHop = percentageHop * piece.pieceMoveSpeed;
        if ( percentageHop <= 0.5f) {
            Vector3 vec1 = Vector3.Lerp( startPebbleLoc, halfway, hopCurve1.Evaluate(percentageHop*2));
            Vector3 vec2 = Vector3.Lerp( startPebbleLoc, startPebbleLoc + new Vector3(0, hopUpwardsAmount, 0), hopCurve3.Evaluate(percentageHop*2));
            piece.self.transform.position = vec1 + vec2 - startPebbleLoc;
            return false;
        }
        else if( ( piece.self.transform.position - endPebbleLoc).magnitude >= 0.01) {
            //piece.self.transform.position = Vector3.SmoothDamp( piece.self.transform.position, targetLocation, ref smoothDampCurrentVelocity, piece.pieceMoveSpeed * Time.deltaTime);
            Vector3 vec3 = Vector3.Lerp( halfway, endPebbleLoc, hopCurve2.Evaluate((percentageHop - 0.5f) * 2));
            Vector3 vec4 = Vector3.Lerp( halfway + new Vector3(0, hopUpwardsAmount, 0), halfway, hopCurve4.Evaluate( (percentageHop - 0.5f) * 2));
            piece.self.transform.position = vec3 + vec4 - halfway;

            return false;
        }
        else if ( !Counter( ref hopWaitCounter)) {
            piece.self.transform.position = endPebbleLoc;
            return false;
        }
        else {
            hopWaitCounter = hopWaitTime;
            hopDurationCounter = 0f;
            return true;
        }
    }
    public bool UpwardsJump(Piece piece, Vector3 startPebble) {
        Vector3 startPebbleLoc = startPebble + new Vector3(0, 0.5f, 0);
        Vector3 endPebbleLoc = startPebbleLoc + new Vector3(0, inPlaceUpwardsAmount, 0);
        hopDurationCounter += Time.deltaTime;
        float percentageHop = hopDurationCounter * piece.pieceMoveSpeed / hopDurationTime;
        //percentageHop = percentageHop * piece.pieceMoveSpeed;
        if ( percentageHop <= 0.5f) {
            piece.self.transform.position  = Vector3.Lerp( startPebbleLoc, endPebbleLoc, hopCurve3.Evaluate(percentageHop*2));
            return false;
        }
        else if ( ( piece.self.transform.position - startPebbleLoc).magnitude >= 0.01) {
            piece.self.transform.position = Vector3.Lerp( endPebbleLoc, startPebbleLoc, hopCurve4.Evaluate( (percentageHop - 0.5f) * 2));
            return false;
        }
        else if ( !Counter( ref hopWaitCounter)) {
            piece.self.transform.position = startPebbleLoc;
            return false;
        }
        else {
            hopWaitCounter = hopWaitTime;
            hopDurationCounter = 0f;
            return true;
        }
    }

    public void MovePieceTo(Piece piece, Pebble pebble) {
        piece.self.transform.position = pebble.self.transform.position + new Vector3(0, 0.5f, 0);
    }

    public Piece GetPieceByGameObject(GameObject gameObject) {
        foreach (Piece piece in pieces)
        {
            if (piece.self == gameObject) {
                return piece;
            }
        }
        return null; // this shouldn't happen! Means the piece was not in the pieces array
    }
    public Pebble GetPebbleByGameObject(GameObject gameObject) {
        foreach (Pebble pebble in boardPlacement)
        {
            if (pebble.self == gameObject) {
                return pebble;
            }
        }
        return null; // this shouldn't happen! Means the pebble was not in the boardPlacement array
    }
    public Pebble GetPebbleByPiece(Piece piece) {
        foreach (Pebble pebble in boardPlacement)
        {
            if (pebble.piece == piece) {
                return pebble;
            }
        }
        return null; // this shouldn't happen! Means the pebble was not in the boardPlacement array
    }
}
