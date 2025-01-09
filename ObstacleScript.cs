using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] GameObject _obstacle;
    [SerializeField] GameObject _point;
    int _maxPoints; //the amount of point blocks that can spawn per row
    int _FPPspeed; //the speed of the blocks (FPP)
    int _TPPspeed; //the speed of the blocks (TPP)
    int _rowsSpawned; //the amount of rows spawned
    public static float _FPPspawnInterval, _TPPspawnInterval, _MAXFPPSpawnInterval, _MAXTPPSpawnInterval;
    GlobalGameHandlerScript _gGHS;

    Vector3[] _positions = new Vector3[] { new Vector3(-4, 1, 500), new Vector3(-2, 1, 500), new Vector3(0, 1, 500), new Vector3(2, 1,500), new Vector3(4, 1, 500) };
    // Start is called before the first frame update
    void Start()
    {
        _gGHS = GameObject.FindGameObjectWithTag("globalGameHandler").GetComponent<GlobalGameHandlerScript>(); //get the reference to the gameobject
        _maxPoints = 3;
        _FPPspeed = 100;
        _TPPspeed = 20;
        _FPPspawnInterval = _MAXFPPSpawnInterval = 1.5f;
        _TPPspawnInterval = _MAXTPPSpawnInterval = 1f; 

        StartCoroutine("StartWaves");
    }

    public IEnumerator StartWaves() {
            SpawnRow();
            if (IngameScript._perspective == 0) yield return new WaitForSeconds(_FPPspawnInterval);
            else yield return new WaitForSeconds(_TPPspawnInterval);
            StartCoroutine("StartWaves");
    }

    private void SpawnRow() {
        _rowsSpawned++;
        int numberOfPointsSpawned = 0; //the amount of points spawned
        GameObject blockInstance;
        string lastSpawned = null;
        for (int i = 0; i < 5; i++) {
            if (i < 4) { //if not on the last iteration
                bool spawnPoint = (0.5 > Random.Range(0f, 1f)); //flip a coin to decide if a piece needs to be spawned
                if (spawnPoint && numberOfPointsSpawned < _maxPoints && lastSpawned != "point") { //if a point can be spawned
                    blockInstance = Instantiate(_point, _positions[i], Quaternion.identity); //spawn a point
                    lastSpawned = "point"; //mark the last spawned block as a point
                    numberOfPointsSpawned++; //increment the number of points spawned
                }
                else {
                    blockInstance = Instantiate(_obstacle, _positions[i], Quaternion.identity); //spawn an obstacle
                    lastSpawned = "obstacle"; //mark the last spawned block as an obstacle
                }
            }
            else { //if on last iteration
                if (numberOfPointsSpawned == 0) blockInstance = Instantiate(_point, _positions[i], Quaternion.identity); //if no points have been spawned, spawn one
                else blockInstance = Instantiate(_obstacle, _positions[i], Quaternion.identity); //spawn an obstacle
            }
            if (IngameScript._perspective == 0) blockInstance.GetComponent<BlockScript>().SetSpeed(_FPPspeed); //change the speed of the block
            else blockInstance.GetComponent<BlockScript>().SetSpeed(_TPPspeed); //change the speed of the block
        }
    }

    public void UpdatePositionArray(float zValue) {
        _rowsSpawned = 0;
        _positions = new Vector3[] { new Vector3(-4, 1, zValue), new Vector3(-2, 1, zValue), new Vector3(0, 1, zValue), new Vector3(2, 1, zValue), new Vector3(4, 1, zValue) }; //set new position array
    }
}
