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

    List<int> availableMovement = null;
    bool showPossibleMovement = false;
    bool boardUpdate = false;
    bool isTurnEnd = false;
    private void Update()
    {
        
        // UI change, not animation...
        if (showPossibleMovement ) { // && availableMovement == null && !animationPlaying && targetPebble == null && animType == 0 && equipped != null
            if ( equipped.CanMove()) {
                // get available pebble locations since we know the equipped piece
                // for every location actually available, update the available moves
                // for every location, enable the particle system...
                availableMovement = boardS.getAllPebblesInRadius(equipped);
                boardS.enableDisableParticlesOf(availableMovement);
            }
            showPossibleMovement = false;
        }

        // if clicked
        if (Input.GetMouseButtonDown(0)) {
            //     raycast stuff
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f)) {
                // if (raycast was for something that can always work, even when the board is moving)
                // if (!animationPlaying && player's turn) // animation is not playing, pieces are clickable.
                if (!animationPlaying && gameStats.gameTurn % (gameStats.totalTurns * 2) < gameStats.totalTurns) {
            //     if (a piece is not active)
                    if (equipped == null || targetPebble == null ) {
            //             if (raycast was a piece)
            //                 set the current piece as that piece
            //                 animationPlaying = true;
            //                 animtype = 0
            //             // else if a card
                        if (  hit.collider.CompareTag("PieceHit")) {
                            Piece aPiece = boardS.GetPieceByGameObject(hit.collider.gameObject); // dont forget to set to null when done
                            if (aPiece.belongsToPlayerA == gameStats.isPlayerATurn) {
                                equipped = aPiece;
                                targetPebble = boardS.GetPebbleByPiece( equipped);
                                animationPlaying = true;
                                animType = 0;
                                boardS.disableParticlesOf(availableMovement);
                            }
                        }
            //             else if (raycast was a pebble)
            //                 set the target pebble as that pebble
            //                 animationPlaying = true;
            //                 animType = 2
                        else if ( hit.collider.CompareTag("PebbleHit")) {
                            // if the pebble is an available move, do the following:
                            //      revert all pebble changed looks...
                            Pebble temp = boardS.GetPebbleByGameObject( hit.collider.gameObject);
                            if ( availableMovement != null && availableMovement.Contains( boardS.GetIndexOfPebble(temp))) {
                                targetPebble = temp;
                                animationPlaying = true;
                                animType = 2;
                            }
                            Debug.Log("a");
                        }
                    }
                }
            }
        }
        
        if (boardUpdate) {
            if ( isTurnEnd) {
                boardS.WaitStatusUpdate();
                Debug.Log("FFFFFFFFFFFFFFFFFFFFF");
                isTurnEnd = false;
            }
            if (boardS.BoardUpdate()) {
                Debug.Log("aaaaaaaaaa");
                boardUpdate = false;
                isTurnEnd = true;
            }
            //Debug.Log("kkkkkkkkkk");
            //boardS.WaitStatusUpdate();
        }
        
        // if (animationPlaying && player's turn)
        if (animationPlaying && gameStats.gameTurn % (gameStats.totalTurns * 2) < gameStats.totalTurns) {
            //Debug.Log("k");
            if (animType == 0) { // we selected a piece, UpwardsJump the piece in place.
                if (boardS.UpwardsJump(equipped, targetPebble.self.transform.position)) {
                    targetPebble.piece = equipped;
                    animationPlaying = false;
                    targetPebble = null;
                    availableMovement = null;
                    showPossibleMovement = true;
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
                    boardS.disableParticlesOf(availableMovement);
                    gameStats.isPlayerATurn = !gameStats.isPlayerATurn;
                    boardUpdate = true;
                            Debug.Log("c");
                }
            }
            else if ( animType == 3) { // we selected something to attack...
                            Debug.Log("e");

            }
        }
    }
}


