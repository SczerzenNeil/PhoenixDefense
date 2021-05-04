using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_Timer : MonoBehaviour
{
    public GameObject EMP;
    public GameObject EMP2;
    public GameObject EMP3;
    //  public bool canUse = true;
    public bool EMPactive = false;
    public float EMPeffect = 15f;
    public float _empTimer = 60f;
    private float returnTimer;
    private float returnEffectTimer;
    // Start is called before the first frame update
    void Start()
    {
        EMPactive = false;
        returnTimer = _empTimer;
        returnEffectTimer = EMPeffect;
    }

    // Update is called once per frame
    void Update()
    {

        if (EMPactive == false)
        {
            _empTimer -= Time.deltaTime;

            if (_empTimer <= 0)
            {
                EMPactive = true;
                EMPeffect = returnEffectTimer;
                return;
            }
            return;
        }
        else if (EMPactive == true)
        {
            EMP.SetActive(true);
            EMP2.SetActive(true);
            EMP3.SetActive(true);
            EMPeffect -= Time.deltaTime;

            if (EMPeffect <= 0)
            {
                EMPactive = false;
                _empTimer = returnTimer;
                EMP.SetActive(false);
                EMP2.SetActive(false);
                EMP3.SetActive(false);
                return;
            }
            return;
        }

    }
}
