using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PieceStatus : MonoBehaviour
{
    public GameObject textM;
    public GameObject canvas;
    TMP_Text waitTextMesh;
    Canvas pieceCanvas;
    bool waitIsActive;
    private void Start() {
        pieceCanvas = canvas.GetComponent<Canvas>();
        waitTextMesh = textM.GetComponent<TMP_Text>();
        SetWaitUIActive(false);
        waitTextMesh.text = "0";
        waitIsActive = false;
    }
    private void SetWaitUIActive( bool isActive) {
        pieceCanvas.GetComponent<Transform>().Find("WaitStatus").gameObject.SetActive(isActive);
    }
    public void SetWaitText( int wait) {
        if ( wait > 0) {
            // set the text
            waitTextMesh.text = wait + "";
            // if wait is not active already, activate wait
            if ( !waitIsActive) {
                SetWaitUIActive(true);
                waitIsActive = true;
            }
        }
        else {
            if ( waitIsActive) {
                // set wait as inactive
                SetWaitUIActive(false);
                waitIsActive = false;
            }
        }
        
    }

}
