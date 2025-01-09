using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    BlockParticleScript[] _bs;
    MeshRenderer[] _ms;
    Transform[] _t;
    IngameScript _iGS; 
    // Start is called before the first frame update
    void Start()
    {
        _iGS = GameObject.FindGameObjectWithTag("gameHandler").GetComponent<IngameScript>(); 
        _ms = GetComponentsInChildren<MeshRenderer>();
        _bs = GetComponentsInChildren<BlockParticleScript>();
        _t = GetComponentsInChildren<Transform>();
        ToggleChildrenRigidbodies(false); //turn off children rigidbodies
    }

    //when the score block collides with something
    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player")) {
            if (!_iGS.HasGameEnded()) //if the game hasn't ended yet
            Explode(); //if made contact with player, explode
        }
    }

    //explode the point into small pieces
    private void Explode() {
        _iGS.IncrementScore(); //increment the score

        if (IngameScript._score % 20 == 0) { //on every 20 points
            if (ObstacleScript._FPPspawnInterval > 0.5f) { //if the interval is above the minimum interval
                ObstacleScript._FPPspawnInterval -= (0.05f * ObstacleScript._MAXFPPSpawnInterval); //remove 5% from the total time
                ObstacleScript._TPPspawnInterval -= (0.05f * ObstacleScript._MAXTPPSpawnInterval); //remove 5% from the total time
            }
        }

        if (IngameScript._score % 10 == 0) GameObject.FindGameObjectWithTag("gameHandler").GetComponent<IngameScript>().TogglePerspective(); //if the score is evenly divisible by 10
        else {
            ToggleChildrenRigidbodies(true); //turn on all child rigidbodies

            for (int i = 0; i < transform.childCount; i++) { //for all children
                _bs[i].StartDiminish(); //start diminish in the current script
                if (IngameScript._perspective == 0) _t[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-500, 500), Random.Range(0, 600), Random.Range(1000, 1800))); //add force in direction
                else _t[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-500, 500), Random.Range(0, 600), Random.Range(-500, 500))); //add force in direction
                _t[i].GetComponent<Rigidbody>().useGravity = true;
                _t[i].parent = null;
            }
        }
        Destroy(gameObject); //destroy this

    }

    public void UpdateChildrenColor(Color32 color) {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    private void ToggleChildrenRigidbodies(bool toggle) {
        for (int i = 0; i < transform.childCount; i++) { //for all children
            transform.GetChild(i).GetComponent<Rigidbody>().detectCollisions = toggle; //set the rigidbody to the parameter
        }
    }
}
