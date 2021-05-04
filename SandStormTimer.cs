using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStormTimer : MonoBehaviour
{
    public GameObject sandStorm;
    //  public bool canUse = true;
    public bool sandStormActive = false; 
    public float sandstormEffect = 15f;
    public float _sandStormTimer = 60f;
    private float returnTimer;
    private float returnEffectTimer;
    // Start is called before the first frame update
    void Start()
    {
        sandStormActive = false;
        returnTimer = _sandStormTimer;
        returnEffectTimer = sandstormEffect;
    }

    // Update is called once per frame
    void Update()
    {

        if (sandStormActive == false)
        {
            _sandStormTimer -= Time.deltaTime;

            if (_sandStormTimer <= 0)
            {
                sandStormActive = true;
                sandstormEffect = returnEffectTimer;
                return;
            }
            return;
        }
        else if (sandStormActive == true)
        {
            sandStorm.SetActive(true);
            sandstormEffect -= Time.deltaTime;

            if (sandstormEffect <= 0)
            {
                sandStormActive = false;
                _sandStormTimer = returnTimer;
                sandStorm.SetActive(false);
                return;
            }
            return;
        }
        
    }
}
