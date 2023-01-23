using UnityEngine;

public class Pebble : MonoBehaviour {
    public Piece piece;
    public GameObject self;
    public Pebble[] PebblesLinked;
    public bool isWaitPebble;
    public bool isPlayerAPebble = false;
    public ParticleSystem selectableParticles;
    private bool particlesAreOn = false;
    public Pebble( Pebble[] pebbles) {
        piece = null;
    }
    private void Start() {
        if (this.gameObject.transform.childCount > 0 && this.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>() != null) {
            selectableParticles = this.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        }
        
    }

    public bool putPiece( Piece toBePut) {
        piece = toBePut;
        return true;
        
    }
    public void removePiece() {
        piece = null;
    }

    public void ParticleSwitch() {
        if (selectableParticles == null) {
            return;
        }
        if ( particlesAreOn) {
            selectableParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            particlesAreOn = false;
        }
        else {
            selectableParticles.Play();
            particlesAreOn = true;
        }
    }
    public void ParticleDisable() {
        if (selectableParticles == null) {
            return;
        }
        if ( particlesAreOn) {
            selectableParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            particlesAreOn = false;
        }
    }
}