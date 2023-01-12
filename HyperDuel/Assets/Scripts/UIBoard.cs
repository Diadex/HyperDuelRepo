using System.Collections;
using System.Collections.Generic;
using UnityEngine;



enum PlayerStatus 
{
PieceOrCardSelect,
PebbleSelect,
AttackSelect
}

public class UIBoard : MonoBehaviour
{
    private Camera mainCamera;

    //private Renderer _renderer;
    public GameStats gameStats;
    private Ray ray;
    private RaycastHit hit;
    //private int myLayerMask = 1 << 6;
    // or: private LayerMask _myLayerMask = 6;
    public bool isWaiting;
    public bool ableToMakeMoves;
    public BoardShadow boardS; 
    public Piece equipped;
    public Pebble targetPebble;

    private void Start()
    {
        gameStats = new GameStats();
        equipped = null;
        mainCamera = Camera.main;
        targetPebble = null;
        isWaiting = false;
        ableToMakeMoves = true;
    }

    private void Update()
    {
        /*
        if clicked
            raycast stuff
            if (raycast was for something that can always work, even when the board is moving)
            if (!animationPlaying && player's turn) // animation is not playing, pieces are clickable.
                if (a piece is not active)
                    if (raycast was a piece)
                        set current piece as that piece
                        animationPlaying = true;
                        animtype = 0
                    // else if a card
                else if(target pebble is not active)
                        if (raycast was a piece)
                            set the current piece as that piece
                            animationPlaying = true;
                            animtype = 0
                        // else if a card
                        else if (raycast was a pebble)
                            set the target pebble as that pebble
                            animationPlaying = true;
                            animType = 2

                    

        if (animationPlaying && player's turn)
            if (animType == 0) // we selected a piece, UpwardsJump the piece in place.
                if (boardS.UpwardsJump(equipped, targetPebble.self.transform.position)) {
                    animationPlaying = false;
                    targetPebble = null;
                }
            else if (animType == 1) { // we selected a card...
                
            }
            else if ( animType == 2) { // we selected a pebble and moving towards it...

            }
            else if ( animtype == 3) { // we selected something to attack...

            }
        */
    }
}



/*
if (gameStats.gameTurn % (gameStats.totalTurns * 2) < gameStats.totalTurns) { // it is your turn
            if ( ableToMakeMoves) {
                if (Input.GetMouseButtonDown(0))
                {
                    //ray = new Ray( mainCamera.ScreenToWorldPoint(Input.mousePosition),
                    //    mainCamera.transform.forward);
                    ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (gameStats.gameTurn % gameStats.totalTurns == 0) {
                        if (equipped == null || targetPebble == null) { // if either are null, keep on checking
                                    Debug.Log("c");
                            if (Physics.Raycast(ray, out hit, 1000f)) {
                                if (  hit.collider.CompareTag("PieceHit")) {
                                    Debug.Log("a");
                                    equipped = boardS.GetPieceByGameObject(hit.collider.gameObject); // dont forget to set to null when done
                                    targetPebble = boardS.GetPebbleByPiece( equipped);
                                    ableToMakeMoves = false;
                                }
                                // other stuff like picking cards etc.
                            }
                        }
                        else {
                        }
                        
                        
                    }



                    if (Physics.Raycast(ray, out hit, 1000f, myLayerMask))
                    { // hit is the stuff that got hit
                        //foreach (RaycastHit hit in hits) {
                        if ( hit.collider.CompareTag("PebbleHit")) {
                            //hit.collider.gameObject.name;
                            // create BoardShadow.getPebbleBy( GameObject gameObject); // returns the pebble by reference. We then can call the appropriate functions...
                            //  multiple hops to that pebble
                            //  check for surrounding enemies
                            //      if no enemy around
                            //          turnto++;
                            //      else
                            //          
                        }
                        else if (  hit.collider.CompareTag("Piece")) {
                            //turnto++;
                        }
                        // if (_hit.transform == transform)
                        // {
                        //     Debug.Log("Click");
                        //     _renderer.material.color =
                        //         _renderer.material.color == Color.red ? Color.blue : Color.red;
                        // } 
                    }
                    //ableToMakeMoves = false;
                }
            
            }
            else if( gameStats.gameTurn % gameStats.totalTurns == 0) { 
                if (boardS.UpwardsJump(equipped, targetPebble.self.transform.position)) {
                    targetPebble = null;
                    gameStats.gameTurn++;
                }
            }
            else {
                gameStats.gameTurn = 0;
                ableToMakeMoves = true;
            }

        }
        else {

        }
        
    }
    public void JumpPieceWhenClicked(){
        

    }*/