using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NS_Node : MonoBehaviour
{
    public Color hoverColor;
    private Renderer rend;
    [HideInInspector]
    public Color startColor;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;


    public Vector3 positionOffset;
    public bool notActive = false;
    MB_MoneySystem money;


    NS_BuildManager Buildmanager;
    public AudioSource a_Audio;
    public AudioClip turretplaced, error, upgradeMade, sold, repair;
    public GameObject upgradeParticle;
    public GameObject sellParticle;
    public GameObject repairParticle;

    public GameObject Currency;
    public bool _canBuild = true;

    public GameObject AchievmentThingy;
    public GameObject pause;
  //  public GameObject DebugObject;
    public float turretHealth, turretStartingHealth;
    public Material inActiveMat;
    public Material startMat;


    void Start()
    {

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        startMat = rend.material;
        Buildmanager = NS_BuildManager.instance;
        pause = FindObjectOfType<NWB_PauseGame>().gameObject;
        AchievmentThingy = FindObjectOfType<MB_AchievementObserver>().gameObject; // Tells the script what the achievement observer is...

      //  DebugObject = FindObjectOfType<NWB_DebugMenu>().gameObject; // Debug Menu.
       // var DebugMenu = DebugObject.GetComponent<NWB_DebugMenu>();

      //  DebugMenu.nodes.Add(this.gameObject); // Added this node to the DebugMenu's list....

        if (notActive == true)
        {
           // rend.material.color = Color.black;
              rend.material = inActiveMat;
        }
    }

    public void DisableInput()
    {
        this.enabled = false; //Disabling this script 
    }



    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    public void Update()
    {
        if (notActive == true)
        {
           // rend.material.color = Color.black;
            rend.material = inActiveMat;
        }
        

        if (turret != null)
        {
            turretStartingHealth = turret.GetComponent<NS_Turret>().startHealth;
            turretHealth = turret.GetComponent<NS_Turret>().health / turret.GetComponent<NS_Turret>().startHealth;
        }

        if (turret == null) //fixing issue with upgrading turret being destroyed and not being able to upgrade a turret placed on destroyed spot 
            isUpgraded = false;
    }

    void OnMouseDown()
    {
        
        if (notActive == true)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null)
        {
            Buildmanager.SelectNode(this);
            return;
        }

        BuildTurret(Buildmanager.GetTuuretToBuild());

        if (Buildmanager.CanBuild)
            return;
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
      //  if (blueprint.prefab != null)
      //  {
            var achievement = AchievmentThingy.GetComponent<MB_AchievementObserver>(); // Initializes the achievment observer...

            if (blueprint == null)
                return;

            if (NS_GameManager.Money < blueprint.cost)
            {
                a_Audio.clip = error;
                a_Audio.PlayOneShot(error);
                Debug.Log("Not enough money");
                return;
            }

            NS_GameManager.Money -= blueprint.cost;
            NS_GameManager.MoneyChanged(); //matthews function addition for money UI

            GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
            turret = _turret;

            turretHealth = turret.GetComponent<NS_Turret>().health; // _turret.GetComponent<NS_Turret>().startHealth;


            turretBlueprint = blueprint;
            achievement.TurretAchievementNum++;

            if (achievement.HasUnlockedTurretAchivement == false)
            {
                achievement.CheckAchievements(); // Checks achievement progress...
            }

            a_Audio.clip = turretplaced;
            a_Audio.PlayOneShot(turretplaced);
            Debug.Log("turret built");

       // }
    }
    public void RepairTurret()
    {
        if (turretBlueprint == null)
            return;

        if (NS_GameManager.Money < turretBlueprint.GetRepairAmount() || turretHealth == 1) //>= turret.GetComponent<NS_Turret>().startHealth)
        {
            a_Audio.clip = error;
            a_Audio.PlayOneShot(error);
            Debug.Log("Not enough money");
            return;
        }




        if (turretHealth < 1 && turretHealth > 0.5f)
        {
            NS_GameManager.Money -= turretBlueprint.GetRepairAmount();
            NS_GameManager.MoneyChanged();

            turret.GetComponent<NS_Turret>().health = turretStartingHealth;

            a_Audio.clip = upgradeMade;
            a_Audio.PlayOneShot(upgradeMade);

            GameObject repairEffect = (GameObject)Instantiate(repairParticle, GetBuildPosition(), Quaternion.identity);
            Destroy(repairEffect, 3f);

        }

        else if (turretHealth < 0.5f)
        {
            NS_GameManager.Money -= turretBlueprint.GetRepairAmount() * 2;
            NS_GameManager.MoneyChanged();

            turret.GetComponent<NS_Turret>().health = turretStartingHealth;

            a_Audio.clip = upgradeMade;
            a_Audio.PlayOneShot(upgradeMade);

            GameObject repairEffect = (GameObject)Instantiate(repairParticle, GetBuildPosition(), Quaternion.identity);
            Destroy(repairEffect, 3f);

        }

    }
    public void UpgradeTurret()
    {
        if (turretBlueprint == null)
            return;

        if (NS_GameManager.Money < turretBlueprint.upgradecost)
        {
            a_Audio.clip = error;
            a_Audio.PlayOneShot(error);
            Debug.Log("Not enough money");
            return;
        }

        NS_GameManager.Money -= turretBlueprint.upgradecost;
        NS_GameManager.MoneyChanged(); //matthews function addition for money UI

        //deleting old turret
        Destroy(turret);

        //building new turret 
        GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        turretHealth = _turret.GetComponent<NS_Turret>().health / _turret.GetComponent<NS_Turret>().startHealth;
        a_Audio.clip = upgradeMade;
        a_Audio.PlayOneShot(upgradeMade);

        GameObject upgradeEffect = (GameObject)Instantiate(upgradeParticle, GetBuildPosition(), Quaternion.identity);
        Destroy(upgradeEffect, 3f);
        isUpgraded = true;
        Debug.Log("turret upgraded");
    }

    public void SellTurret()
    {
        Scene CurrentScene = SceneManager.GetActiveScene(); // Gets the current scene
        if (CurrentScene.name == "NS_ProtoType 05" || CurrentScene.name == "MeteorLV2" || CurrentScene.name == "NS_MeteorxSandstorm") // Are we on the Meteor level?
        {
            if (turret.tag == "Turret")
            {
                var MeatyOre = GameObject.FindObjectOfType<NS_MeteorMovement>(); // Reference to the meteor script
                MeatyOre.Turrets.Remove(gameObject); // Removes self from turrets
            }
        }
        if (isUpgraded == false)
        {
            NS_GameManager.Money += turretBlueprint.GetSellAmount();
            NS_GameManager.MoneyChanged(); //matthews function addition for money UI
        }
        else if (isUpgraded == true)
        {
            NS_GameManager.Money += turretBlueprint.GetSellAmount() * 2;
            NS_GameManager.MoneyChanged(); //matthews function addition for money UI
        }

        GameObject sellEffect = (GameObject)Instantiate(sellParticle, GetBuildPosition(), Quaternion.identity);
        Destroy(sellEffect, 3f);
        a_Audio.clip = sold;
        a_Audio.PlayOneShot(sold);
        Destroy(turret);
        isUpgraded = false;
        turretBlueprint = null;
    }


    void OnMouseEnter()
    {
        if (notActive == true)
            return;
        if (pause.GetComponent<NWB_PauseGame>().GameIsPaused == true)
            return;

        if (Buildmanager.CanBuild)
        {
            rend.material.color = hoverColor;
            return;
        }

    }

    void OnMouseExit()
    {
        if (notActive == true)
            return;
        if (pause.GetComponent<NWB_PauseGame>().GameIsPaused == true)
            return;

        rend.material.color = startColor;
    }


    
}
