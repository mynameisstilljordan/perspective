using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    ParticleSystem _pSPrimary, _pSSecondary, _pSExplosion, _pSObstacleExplosion;
    IngameScript _iGS;
    //MeshRenderer _mR;

    // Start is called before the first frame update
    void Start()
    {
        //_mR = GetComponent<MeshRenderer>(); //get the mesh renderer of the player

        _pSPrimary = transform.GetChild(1).GetComponent<ParticleSystem>(); //get the particlesystem on the player 
        _pSSecondary = transform.GetChild(2).GetComponent<ParticleSystem>(); //get the particlesystem on the player 
        _pSExplosion = transform.GetChild(3).GetComponent<ParticleSystem>(); //get the explosion particle system
        _pSObstacleExplosion = transform.GetChild(4).GetComponent<ParticleSystem>(); //get the explosion particle system for the block
        _iGS = GameObject.FindGameObjectWithTag("gameHandler").GetComponent<IngameScript>(); //get the reference to the script

        _pSPrimary.Stop(); //stop primary
        _pSSecondary.Stop(); //stop secondary
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("point")) { //if player hit a point
            PointHit(); //call this method if a point was hit
            Destroy(other.transform.gameObject); //destroy the point
        }
        else if (other.transform.CompareTag("obstacle")) { //if player hit an obstacle
            //_mR.enabled = false; //disable the mesh renderer
            _pSExplosion.Play(); //play the explosion particle effect
            _pSObstacleExplosion.Play(); //play the explosion particle effect
            transform.GetChild(0).gameObject.SetActive(false); //destroy the trail of the player
        }
    }

    //this method is called when the player hits a point
    void PointHit() {
        IngameScript._score++; //increment the score

        if (IngameScript._score % 10 == 0)   //if the current score is divisible by 10
            _iGS.TogglePerspective(); //toggle the perspective

        if (IngameScript._perspective == 0) //if in first person
            _pSPrimary.Play(); //play the particles
        

        else //if top down
            _pSSecondary.Play(); //play the secondary particle 
    }
}
