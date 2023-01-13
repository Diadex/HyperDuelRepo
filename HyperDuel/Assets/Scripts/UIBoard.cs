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
    public bool animationPlaying;
    public int animType;
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
        animationPlaying = false;
        animType = 0;
    }

    private void Update()
    {
        
        // if clicked
        if (Input.GetMouseButtonDown(0)) {
            //     raycast stuff
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f)) {
                // if (raycast was for something that can always work, even when the board is moving)
                // if (!animationPlaying && player's turn) // animation is not playing, pieces are clickable.
                if (!animationPlaying && gameStats.gameTurn % (gameStats.totalTurns * 2) < gameStats.totalTurns) {
            //     if (a piece is not active)
                    if (equipped == null ) {
            //         if (raycast was a piece)
            //             set current piece as that piece
            //             animationPlaying = true;
            //             animtype = 0
                        if (  hit.collider.CompareTag("PieceHit")) {
                            equipped = boardS.GetPieceByGameObject(hit.collider.gameObject); // dont forget to set to null when done
                            targetPebble = boardS.GetPebbleByPiece( equipped);
                            animationPlaying = true;
                            animType = 0;
                        }
            //         // else if a card
            //     else if(target pebble is not active)
                    } else if(targetPebble == null) {
            //             if (raycast was a piece)
            //                 set the current piece as that piece
            //                 animationPlaying = true;
            //                 animtype = 0
            //             // else if a card
                        if (  hit.collider.CompareTag("PieceHit")) {
                            equipped = boardS.GetPieceByGameObject(hit.collider.gameObject); // dont forget to set to null when done
                            targetPebble = boardS.GetPebbleByPiece( equipped);
                            animationPlaying = true;
                            animType = 0;
                        }
            //             else if (raycast was a pebble)
            //                 set the target pebble as that pebble
            //                 animationPlaying = true;
            //                 animType = 2
                        else if ( hit.collider.CompareTag("PebbleHit")) {
                            targetPebble = boardS.GetPebbleByGameObject( hit.collider.gameObject);
                            Pebble prev = boardS.GetPebbleByPiece( equipped);
                            prev.piece = null;
                            animationPlaying = true;
                            animType = 2;
                            Debug.Log("a");
                        }
                    }
                }
            }
        }    

        // if (animationPlaying && player's turn)
        if (animationPlaying && gameStats.gameTurn % (gameStats.totalTurns * 2) < gameStats.totalTurns) {
            Debug.Log("k");
            if (animType == 0) { // we selected a piece, UpwardsJump the piece in place.
                if (boardS.UpwardsJump(equipped, targetPebble.self.transform.position)) {
                    targetPebble.piece = equipped;
                    animationPlaying = false;
                    targetPebble = null;
                            Debug.Log("b");
                }
            }
            else if (animType == 1) { // we selected a card...
                
                            Debug.Log("d");
            }
            else if ( animType == 2) { // we selected a pebble and moving towards it...
                boardS.MovePieceTo(equipped, targetPebble);
                targetPebble.piece = equipped;
                animationPlaying = false;
                targetPebble = null;
                equipped = null;
                            Debug.Log("c");
            }
            else if ( animType == 3) { // we selected something to attack...
                            Debug.Log("e");

            }
        }
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