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
                            if (equipped == null) {
                                Debug.Log("AN ERROR 1 OCCURRED");
                            }
                            targetPebble = boardS.GetPebbleByPiece( equipped);
                            if (targetPebble == null) {
                                Debug.Log("AN ERROR 2 OCCURRED");
                            }
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
                            // if the pebble is an available move, do the following:
                            //      revert all pebble changed looks...
                            targetPebble = boardS.GetPebbleByGameObject( hit.collider.gameObject);
                            animationPlaying = true;
                            animType = 2;
                            Debug.Log("a");
                        }
                    }
                }
            }
        }    
        // UI change, not animation...
        if (!animationPlaying && targetPebble == null && animType == 0 && equipped != null) {
            // get available pebble locations since we know the equipped piece
            // for every location actually available, update the available moves
            // for every location, change the way the pebble looks...
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
                if (boardS.PathHop(equipped, targetPebble)) {
                    Pebble prev = boardS.GetPebbleByPiece( equipped);
                    prev.piece = null;
                    targetPebble.piece = equipped;
                    animationPlaying = false;
                    targetPebble = null;
                    equipped = null;
                            Debug.Log("c");
                }
            }
            else if ( animType == 3) { // we selected something to attack...
                            Debug.Log("e");

            }
        }
    }
}


