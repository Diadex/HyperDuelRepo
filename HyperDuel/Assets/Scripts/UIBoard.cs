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
    //public GameStats gameStats;
    private Ray ray;
    private RaycastHit hit;
    //private int myLayerMask = 1 << 6;
    // or: private LayerMask _myLayerMask = 6;
    public bool isWaiting;
    public bool ableToMakeMoves;
    public BoardShadow boardS; 
    public Piece equipped;
    public Piece enemyEquipped;
    public Pebble targetPebble;
    public Pebble targetPebble2;

    private void Start()
    {
        equipped = null;
        mainCamera = Camera.main;
        targetPebble = null;
        targetPebble2 = null;
        isWaiting = false;
        ableToMakeMoves = true;
        animationPlaying = false;
        animType = 0;
    }
    
    List<int> availableMovement = null;
    bool showPossibleMovement = false;
    bool boardUpdate = false;
    bool isTurnEnd = false;
    int movementMode = 0;
    bool wasHitPiece = false;
    bool wasHitPebble = false;
    bool wasHitUI = false;
    bool isAttackAnim = false;

    private void animationInitiate0() {
        Piece aPiece = boardS.GetPieceByGameObject(hit.collider.gameObject);
        //if (aPiece.belongsToPlayerA == gameStats.isPlayerATurn) {
            equipped = aPiece;
            targetPebble = boardS.GetPebbleByPiece( equipped);
            boardS.disableParticlesOf(availableMovement);
            animType = 0;
            animationPlaying = true;
        //}
        setwasHitFalse();
    }
    private void setwasHitFalse() {
        wasHitPiece = false;
        wasHitPebble = false;
        wasHitUI = false;
    }
    private void Update()
    {
        
        
        // UI change, not animation...
        if (showPossibleMovement) { // && availableMovement == null && !animationPlaying && targetPebble == null && animType == 0 && equipped != null) { 
            if ( equipped.CanMove()) {
                // get available pebble locations since we know the equipped piece
                // for every location actually available, update the available moves
                // for every location, enable the particle system...
                availableMovement = boardS.getAllPebblesInRadius(equipped);
                boardS.enableDisableParticlesOf(availableMovement);
            }
            showPossibleMovement = false;
        }

        // get any mouseclicks
        if (Input.GetMouseButtonDown(0)) {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f)) {
                if (!animationPlaying) {
                    if (  hit.collider.CompareTag("PieceHit")) {
                        wasHitPiece = true;
                        wasHitPebble = false;
                        wasHitUI = false;
                    }
                    else if (  hit.collider.CompareTag("PebbleHit")) {
                        wasHitPiece = false;
                        wasHitPebble = true;
                        wasHitUI = false;
                    }
                }
                // if it is ui element...
            }
        }

        // any out of turn operations here
        if (wasHitUI) {
            //...
        }
        // if the turn is player1's
        if (!animationPlaying) { // && gameStats.gameTurn % (gameStats.totalTurns * 2) < gameStats.totalTurns) {
        // activate timer countdown for p1, set mode to mode 1
            // if the timer was inactive and movementmode was 0,
            if (movementMode == 0) {movementMode = 1;}
            if ( movementMode == 1) {
                if (wasHitPiece) {
                    animationInitiate0();
                }
            }
            else if ( movementMode == 2) {
                if (wasHitPiece) {
                    animationInitiate0();
                }
                else if (wasHitPebble) {
                    Pebble temp = boardS.GetPebbleByGameObject( hit.collider.gameObject);
                    if ( availableMovement != null && availableMovement.Contains( boardS.GetIndexOfPebble(temp))) {
                        targetPebble = temp;
                        animationPlaying = true;
                        animType = 2;
                        setwasHitFalse();
                    }
                }
            }
            else if ( movementMode == 3) {
                if (wasHitPiece) {
                    Piece aPiece = boardS.GetPieceByGameObject(hit.collider.gameObject);
                    //if (aPiece.belongsToPlayerA != gameStats.isPlayerATurn) {
                        equipped = aPiece;
                        targetPebble = boardS.GetPebbleByPiece( equipped);
                        boardS.disableParticlesOf(availableMovement);
                        animType = 0;
                        animationPlaying = true;
                        setwasHitFalse();
                    //}
                }
            }
            else if ( movementMode == -1) {
                boardS.WaitStatusUpdate();
                Debug.Log("FFFFFFFFFFFFFFFFFFFFF");
                boardUpdate = true;
                animationPlaying = true;
                animType = 4;
                setwasHitFalse();
            }
        }


        // if mode 0 && enturn clicked, endturn, mode set to 0, 
        // if mode 1,
        //      -----select piece mode
        //      if a piece was clicked,
        //          make the piece jump
        //              set mode to 2
        //      if endturn, set mode to 4
        // if mode 2,
        //      -----select piece or pebble mode
        //      if selected was a piece, make it jump
        //      if pebble, move the last selected piece to pebble
        //          set mode to 3
        //      if endturn, set mode to 4
        // if mode 3,
        //      -----select piece attack mode
        //      check any near  pieces,
        //      if there are none, set mode to 4
        //      if the clicked is any of the nearing enemy pieces,
        //          make the opposing piece jump,
        //          initiate attack sequence
        //              set mode to 4
        //      if endturn, set mode to 4
        // if mode 4, 
        //      update board
        //          set mode to 0
        
        
        // if (animationPlaying && player's turn)
        if (animationPlaying) { // && gameStats.gameTurn % (gameStats.totalTurns * 2) < gameStats.totalTurns) {
            //Debug.Log("k");
            if (animType == 0) { // we selected a piece, UpwardsJump the piece in place.
                if (boardS.UpwardsJump(equipped, targetPebble.self.transform.position)) {
                    if (isAttackAnim) {
                        movementMode = -1;
                        animationPlaying = true;
                    }
                    else {
                        targetPebble.piece = equipped;
                        animationPlaying = false;
                        targetPebble = null;
                        availableMovement = null;
                        showPossibleMovement = true;
                                Debug.Log("b");
                        movementMode = 2;
                    }
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
                    //gameStats.isPlayerATurn = !gameStats.isPlayerATurn;
                    boardUpdate = true;
                            Debug.Log("c");
                    movementMode = 3;
                }
            }
            else if ( animType == 3) { // we selected something to attack...
                            Debug.Log("e");

            }
            else if ( animType == 4) {
                            Debug.Log("f");

                if (boardUpdate) {
                    if (boardS.BoardUpdate()) {
                        boardUpdate = false;
                        animationPlaying = false;
                        movementMode = 0;
                    }

                }
            }
        }









        /*
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
                            else {
                                if (equipped != null && boardS.GetPebbleByPiece(equipped).HasPieceAsNeighbor( aPiece)) { // it is the last clicked's neighbor
                                    // is an attack to the piece.
                                    enemyEquipped = aPiece;
                                    targetPebble = boardS.GetPebbleByPiece( equipped);
                                    targetPebble2 = boardS.GetPebbleByPiece( enemyEquipped);
                                    animationPlaying = true;
                                    animType = 3;
                                }
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
                    if ( boardS.AttackIsPossible( )) {

                    }
                            Debug.Log("c");
                }
            }
            else if ( animType == 3) { // we selected something to attack...
                            Debug.Log("e");
                if (boardS.UpwardsJump(equipped, targetPebble.self.transform.position)) {
                    targetPebble.piece = equipped;
                    targetPebble = null;
                    availableMovement = null;
                }
                if (boardS.UpwardsJump(enemyEquipped, targetPebble2.self.transform.position)) {
                    targetPebble2.piece = enemyEquipped;
                    targetPebble2 = null;
                    availableMovement = null;
                }
                if ( targetPebble == null && targetPebble2 == null) {
                    animationPlaying = false;
                    gameStats.isPlayerATurn = !gameStats.isPlayerATurn;
                    boardUpdate = true;
                    // attack animation begins here.
                }

            }
        }*/
    }

    // private bool attackAnimPlaying() {} 
}


