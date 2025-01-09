using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;

public class BlockScript : MonoBehaviour
{
    int _speed;
    IngameScript _iGS;
    MeshRenderer _mS;

    private void Start() {
        _iGS = GameObject.FindGameObjectWithTag("gameHandler").GetComponent<IngameScript>(); //get instance of script
        _mS = GetComponent<MeshRenderer>(); //get the meshrenderer on the block
    }

    public void SetSpeed(int speed) {
        _speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z-_speed*Time.deltaTime); //constantly move
        if (transform.position.z < 0) {
            if (transform.childCount > 0 && transform.CompareTag("obstacle")) //if the current gameobject is an obstacle carrying the player off screen
                transform.GetChild(0).parent = null; //set the player's parent to null
            Destroy(gameObject); //destroy gameobject when far enough off screen
        }
    }

    //when colliding with objects
    private void OnTriggerEnter(Collider other) {
        if (transform.tag == "obstacle" && other.transform.CompareTag("Player")) { //if this gameobject is an obstacle
            _mS.enabled = false; //disable the mesh of the block
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
            //other.transform.parent = gameObject.transform; //adopt the player 
            _iGS.EndGame(); //call the endgame method
        }
    }
}
