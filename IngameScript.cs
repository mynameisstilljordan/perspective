using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using Lofelt.NiceVibrations;

public class IngameScript : MonoBehaviour {
    Camera _mainCamera;
    int _positionIndex;
    int[] _xPositions = new int[] { -4, -2, 0, 2, 4 };
    int[] _yPositions = new int[] { 5, 25 };
    int[] _zPositions = new int[] { 0, 20, 10 };
    float[] _xRotations = new float[] { 12.5f, 90f };
    [SerializeField] GameObject _player;
    [SerializeField] TMP_Text _scoreText, _endGameScoreText, _endGameHighscoreText;
    [SerializeField] Canvas _ingameCanvas, _endgameCanvas;
    public static int _score;
    float _animationSpeed = 0.1f;
    ObstacleScript _oS;
    int _minimumSwipeDistance = Screen.height / 15; //the distance required to swipe in order for an input to be registered (15% of screen height)
    Vector2 _startTouchPos, _endTouchPos; //for the start and end touch position
    bool _hasGameEnded;
    bool _canPlayerContinue;
    AnimationStateController _anim;
    GameObject _windParticles;

    private enum PlayerState{
        Running, Transforming
        }

    public enum Perspective {
        FirstPerson, TopDown
    }

    public static Perspective _perspective; 

    // Start is called before the first frame update
    void Start() {
        _windParticles = Camera.main.transform.GetChild(0).gameObject; //disable the wind particles
        _anim = _player.gameObject.GetComponent<AnimationStateController>(); //get the instance of the player animator
        _anim.PlayRunningAnimation(); //play the running animation of the player
        GlobalGameHandlerScript.IncrementAdCounter(); //increment the ad counter
        _score = 0; //set score to 0
        _perspective = Perspective.FirstPerson; //set perspective to 0 (first person)
        _oS = GameObject.FindGameObjectWithTag("gameHandler").GetComponent<ObstacleScript>(); //save the instance of the obstacle script
        _mainCamera = Camera.main; //get the reference to the main camera
        _positionIndex = 2; //set the position to the middle
        _hasGameEnded = false; //mark the game as not ended yet
        _canPlayerContinue = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.D)) Move(1);
        if (Input.GetKeyDown(KeyCode.A)) Move(-1);
        if (_scoreText.text != _score.ToString()) _scoreText.text = _score.ToString(); //if score text doesnt match the score, update

        //save the number of touches
        var _count = Input.touchCount;

        //if there is only 1 touch on the screen
        if (_count == 1) {
            var _touch = Input.GetTouch(0);

            //if the touch phase has began
            if (_touch.phase == TouchPhase.Began) {
                _startTouchPos = _touch.position; //save the starting position
            }

            //if the touch phase has ended
            if (_touch.phase == TouchPhase.Ended) {
                _endTouchPos = _touch.position; //save the ending position 

                //if the swipe was large enough to be considered a swipe
                if (Mathf.Abs(_startTouchPos.x - _endTouchPos.x) > _minimumSwipeDistance) {
                    if (_startTouchPos.x > _endTouchPos.x) Move(-1);
                    else Move(1);
                }

                //if the swipe was not large enough to be considered a swipe, treat it as a tap
                else {
                    if (_hasGameEnded && _canPlayerContinue) { //if the game has ended
                        Time.timeScale = 1f; //go back to regular time scale
                        SceneManager.LoadScene("menu"); //go back to the main menu
                    }
                    else { //if the game hasn't ended yet
                        if (_endTouchPos.x < Screen.width / 2) { //if the tap was on the left side of the screen
                            Move(-1); //move left
                        }
                        else if (_endTouchPos.x > Screen.width / 2) { //if the tap was on the right side of the screen
                            Move(1); //move right
                        }
                    }
                }
            }
        }
    }

    private void Move(int direction) {
        if (!_hasGameEnded) { //if the game hasn't ended
            if (_positionIndex + direction != -1 && _positionIndex + direction != 5) //if moving in the requested direction is valid
                _positionIndex += direction; //set the position to the new direction

            if (_perspective == Perspective.FirstPerson) { //if in 1st person perspective
                _player.transform.DORotate(new Vector3(0, 0, -10 * direction), _animationSpeed / 2) //shift the player
                    .SetEase(Ease.OutBounce)
                    .OnComplete(() => {
                        _player.transform.DORotate(new Vector3(0, 0, 0), _animationSpeed); //move player back to original rotation

                    });
            }
            _player.transform.DOMoveX(_xPositions[_positionIndex], _animationSpeed);
            if (_perspective == Perspective.FirstPerson) _mainCamera.transform.DOMoveX(_xPositions[_positionIndex], _animationSpeed * 2)
                    .SetEase(Ease.InFlash);
        }
    }

    public void TogglePerspective() {
        DestroyAllBlocks(); //destroy all blocks
        if (_perspective == 0) { //if perspective is 3d
            //_anim.PlayFlyingAnimation(); 
            _windParticles.SetActive(false); //disable the wind particles
            //_windParticles.transform.position = new Vector3(0, transform.position.y, transform.position.z); //set the wind particles
            _oS.UpdatePositionArray(40f);
            _mainCamera.orthographic = true; //set the camera to orthographic
            _perspective = Perspective.TopDown; //switch perspective to 3d
            _mainCamera.transform.DOMoveX(_xPositions[2], _animationSpeed);
            _mainCamera.transform.DOMoveY(_yPositions[1], _animationSpeed);
            _mainCamera.transform.DOMoveZ(_zPositions[1], _animationSpeed);
            _mainCamera.transform.DORotate(new Vector3(_xRotations[1], 0, 0), _animationSpeed);
        }
        else { //if perspective is 2d
            //_anim.PlayRunningAnimation();
            _oS.UpdatePositionArray(250f);
            _mainCamera.orthographic = false; //set the camera to perspective
            _perspective = Perspective.FirstPerson; //switch perspective to 2d
            _mainCamera.transform.DOMoveX(_xPositions[_positionIndex], _animationSpeed);
            _mainCamera.transform.DOMoveY(_yPositions[0], _animationSpeed);
            _mainCamera.transform.DOMoveZ(_zPositions[0], _animationSpeed);
            _mainCamera.transform.DORotate(new Vector3(_xRotations[0], 0, 0), _animationSpeed)
                .OnComplete(() => {
                    _windParticles.SetActive(true); //disable the wind particles
                });

        }
    }
    public void EndGame() {
        if (!_hasGameEnded) { //if the game hasnt ended already
            DestroyAllBlocks();
            _windParticles.SetActive(false); //disable the wind particles
            _anim.PlayDeathAnimation();
            _hasGameEnded = true; //mark hasgameended as true
            _ingameCanvas.enabled = false; //disable the ingame canvas
            _endgameCanvas.enabled = true; //enable the endgame canvas
            Time.timeScale = 0.1f; //slow down time
            _endGameScoreText.text = "SCORE: " + _score.ToString(); //show the score
            CheckForHighscore(); //check for and update highscore
            StartCoroutine("EnableContinue");
        }
    }

    //this coroutine enables the continue option of the player
    IEnumerator EnableContinue() {
        yield return new WaitForSeconds(.1f); //wait for 1 second
        _canPlayerContinue = true; //allow the player to continue
    }

    private void CheckForHighscore() { //this method checks for a highscore and updates it
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //vibrate
        if (_score > PlayerPrefs.GetInt("highScore")) { //if the score is higher than the highscore
            PlayerPrefs.SetInt("highScore", _score); //set the highscore to the score
        }
        _endGameHighscoreText.text = "BEST: " + PlayerPrefs.GetInt("highScore", 0).ToString();
    }

    //this game returns if the game has ended
    public bool HasGameEnded() {
        return _hasGameEnded; //return the bool
    }

    //this method destroys all blocks
    private void DestroyAllBlocks() {
        var allPoints = GameObject.FindGameObjectsWithTag("point"); //save all points to array
        var allBlocks = GameObject.FindGameObjectsWithTag("obstacle"); //save all blocks to array
        foreach (GameObject block in allBlocks) if (block.transform.childCount == 0) Destroy(block); //destroy all blocks on screen
            else {
                block.transform.GetChild(0).parent = null; //set the parent to null before deleting
                Destroy(block);
            }
        foreach (GameObject point in allPoints) Destroy(point); //destroy all blocks on screen
    }

    public void IncrementScore() {
        _score++;
        _anim.PlayJumpingAnimation();
    }
}
