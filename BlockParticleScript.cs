using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockParticleScript : MonoBehaviour
{
    bool _shouldParticleDiminish;
    bool _shouldWindTakeEffect;
    float _scaleMultiplier, _windMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        _shouldParticleDiminish = false; //dont start scaling down
        _scaleMultiplier = 0.001f;
        _windMultiplier = Random.Range(0.0001f,0.0003f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_shouldParticleDiminish) {
            transform.localScale = new Vector3(transform.localScale.x - _scaleMultiplier, transform.localScale.y - _scaleMultiplier, transform.localScale.z - _scaleMultiplier); //scale down
        }
        if (transform.localScale.x < 0) Destroy(gameObject); //if the scale is small enough, destroy self
        if (_shouldWindTakeEffect) {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - _windMultiplier * Time.deltaTime);
            _windMultiplier *= 1.075f;
        }
    }

    public void StartDiminish() {
        _shouldParticleDiminish = true; //set to diminish
        _shouldWindTakeEffect = true; //StartCoroutine("WindEffect");
    }
}
