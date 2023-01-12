using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoard : MonoBehaviour
{
    private Camera mainCamera;

    //private Renderer _renderer;

    private Ray ray;
    private RaycastHit hit;
    private int myLayerMask = 1 << 6;
    // or: private LayerMask _myLayerMask = 6;
    public bool isWaiting;
    public bool ableToMakeMoves = true;
    public BoardShadow boardS; 
    public Piece equipped;
    public Pebble targetPebble;
    private void Start()
    {
        equipped = null;
        mainCamera = Camera.main;
        isWaiting = false;
        //_renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        /*
        if (GameStats.gameTurn % GameStats.totalTurns * 2 < GameStats.totalTurns) { // it is your turn
            if ( ableToMakeMoves) {
                if (Input.GetMouseButtonDown(0))
                {
                    ray = new Ray(
                        mainCamera.ScreenToWorldPoint(Input.mousePosition),
                        mainCamera.transform.forward);

                    if (GameStats.gameTurn % GameStats.totalTurns == 0) {
                        if (Physics.Raycast(ray, out hit, 1000f, myLayerMask)) {
                            if (  hit.collider.CompareTag("PieceHit")) {
                                if (equipped == null) {
                                    equipped = boardS.GetPieceByGameObject(hit.collider.gameObject); // dont forget to set to null when done
                                }
                                if ( targetPebble == null) {
                                    targetPebble = boardS.GetPebbleByPiece( equipped);
                                }
                                boardS.UpwardsJump(equipped, targetPebble.self.transform.position);
                            }
                        }
                        // other stuff like picking cards etc.
                        
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

        }
        else {

        }
        */
    }
}
