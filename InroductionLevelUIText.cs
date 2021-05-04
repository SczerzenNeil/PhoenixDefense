using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InroductionLevelUIText : MonoBehaviour
{
   // public GameObject UI;
    public Text Instructions;
    public string InstructionsString;
    public string[] InstructionsArray;
    private int InstructionIndex;
    public NS_BuildManager buildmanager;
    public GameObject DialogSystem;
  
   public string[] DialogInstructions = { };

    public void Start()
    {
        DialogSystem = FindObjectOfType<MB_DialogSystem>().gameObject;
        buildmanager = GameObject.Find("GameManager").GetComponent<NS_BuildManager>();

        // This was because dialog system's start function is broken.
        DialogSystem.GetComponent<MB_DialogSystem>().WaveSpawner = GameObject.FindObjectOfType<NS_WaveSpawner>().gameObject;

        Instructions.text = InstructionsString;
        InstructionsArray = DialogInstructions; 
        InstructionsString = InstructionsArray[0];

        //var dialog = DialogSystem.GetComponent<MB_DialogSystem>();
        //StartCoroutine(dialog.ShowReconDialog(InstructionsString));

        StartCoroutine(Init());
    }

    private void Update()
    {
        var dialog = DialogSystem.GetComponent<MB_DialogSystem>();

        if (Input.GetKeyDown(KeyCode.Space) && dialog.IsShowingReconText == false)
        {
            Debug.Log(InstructionIndex);
            InstructionIndex++;

            if(InstructionIndex >= InstructionsArray.Length)
            {
                enabled = false;
                return;
            } 
             InstructionsString = InstructionsArray[InstructionIndex];
             StartCoroutine(dialog.ShowReconDialog(InstructionsString));

        }
    }

    public IEnumerator Init() // To prevent the dialog system to be called before the system knows what the wave spawner is
    {
        yield return new WaitForSecondsRealtime(1);
        var dialog = DialogSystem.GetComponent<MB_DialogSystem>();
        StartCoroutine(dialog.ShowReconDialog(InstructionsString));
        yield break;
    }
}
