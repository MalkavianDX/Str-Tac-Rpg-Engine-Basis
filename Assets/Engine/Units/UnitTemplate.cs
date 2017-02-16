using UnityEngine;
using System.Collections;
using System;

abstract public class UnitTemplate : MonoBehaviour, IHighlightable {

    //Flags (boolean)
	protected Boolean flag_isMoving = false;
	protected Boolean flag_isSelected = false;

    //Variable for setting unit Destination
	protected Vector3 UnitDestination;
    
    //variables + get & set
	public float UnitHP { get; set; }
	public float UnitEnergy { get; set; } 
	public float UnitArmor { get; set; }
    public int UnitTeam { get; set; }
    public float UnitSpeed { get; set; }
    public float Timer_Shooter { get; set; }
    public float UnitReloadSpeed { get; set; }
    public float UnitSearchRadius { get; set; }
    public bool flag_UnitSighted { get; set; }
    public GameObject Target_Nearest { get; set; }
    public float target_distance { get; set; }
    public float next_distance { get; set; }

    //Mark highlight object for unit
    public GameObject selectHighlight;

    // Use this for initialization
    void Start () {
        //Set destination as the starting location to keep unit still
		UnitDestination = this.transform.position;
        //Set variables (10 = default)
        /*UnitHP = 10;
        UnitEnergy = 10;
        UnitArmor = 10;
        UnitSpeed = 10;
        UnitTeam = 0;
        UnitReloadSpeed = 500.0f;
        Timer_Shooter = 0;
        UnitSearchRadius = 15f;
        UnitReloadSpeed = 5.0f;
        target_distance = -255;
        next_distance = -255;*/
        //Find and update Unit List of the map
        MapRulesTemplate MapObject = FindObjectOfType<MapRulesTemplate>();
        MapObject.UpdateUnitList(this);
	}

	// FixedUpdate is called on fixed intervals, Use for regular updates such as physics
	void FixedUpdate () {
        // Is unit supposed to move
		if (flag_isMoving) {
            // Is unit on the Destination
			if (transform.position.Equals(UnitDestination)) {
                //Unit does not move anymore
				flag_isMoving = false;
			}
            // If unit is not on Destination, Move
			else {
				// Vector will do a linear interpolation between our current position and our destination
				Vector3 v;
				v = Vector3.Lerp(transform.position,UnitDestination,Time.fixedDeltaTime * UnitSpeed);
				
				// set our position to the new destination
				transform.position = v;
			}
		}
        if (UnitHP <= 0)
        {
            Destroy(this.selectHighlight);
            Destroy(this.gameObject);
            MapRulesTemplate MapObject = FindObjectOfType<MapRulesTemplate>();
            MapObject.RemoveUnitList(this);
        }
    }
    //Shoot Methods
    //Automatically Check radius for enemy units and shoot the closest. TODO Keep 1 target till changed by player or target dead.
    public virtual void UnitShoot()
    {
        //Check around set radius for objects
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, UnitSearchRadius);
        int i = 0;
        Debug.Log("Next is While : " + i + " Unit Team: " + this.UnitTeam);
        // Go through the results
        while (i < hitColliders.Length)
        {
            // Check Object has Code Component
            if (hitColliders[i].GetComponent<UnitTemplate>() != null)
            {
                var temp = hitColliders[i].GetComponent<UnitTemplate>();
                // set distance to be checked
                next_distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                // Check if unit has different team
                if (temp.UnitTeam != this.UnitTeam)
                {
                    // Is there a target
                    if (target_distance != -255)
                    {
                        Debug.Log("t_Distance != -255, is : " + target_distance);
                        // next on the list farther than target right now

                        if (target_distance < next_distance)
                        {
                            //Shoot current target if it still exists
                            if (Target_Nearest != null)
                                this.UnitShoot(Target_Nearest);
                        }
                        //If not, then it is closer
                        else
                        {
                            //Change target and fire
                            target_distance = next_distance;
                            Target_Nearest = hitColliders[i].gameObject;
                            this.UnitShoot(Target_Nearest);
                        }
                    }
                    // If there is no target, set first as one
                    else
                    {
                        target_distance = next_distance;
                        Target_Nearest = hitColliders[i].gameObject;
                        this.UnitShoot(Target_Nearest);
                    }

                }
                else
                    ;//For future uses
            }
            i++;
            Debug.Log(" " + Target_Nearest);
        }
    }
    // Shoot Target enemy
    public virtual void UnitShoot(GameObject enemy)
    {
        Timer_Shooter = Timer_Shooter - Time.deltaTime;
        //Can shoot?
        if (Timer_Shooter <= 0)
        {
            //Set Timers
            Timer_Shooter = UnitReloadSpeed;
            
            Debug.Log(" " + UnitReloadSpeed);
            Debug.Log(Timer_Shooter + "<-TS TdT->" + Time.deltaTime);
            
            //Shoot with Target
            if (enemy.GetComponent<UnitTemplate>() != null)
            {
                //Create Bullet
                GameObject Bullet = Instantiate(Resources.Load("SphereBullet")) as GameObject;
                //Set Target //Path Bullet
                if (Bullet != null)
                    Bullet.GetComponent<SimpleBullet>().Initialize(this.gameObject, enemy);
            }
        }
    }
    //Skill Methods
    public virtual void UnitSkill()
    {
        // Use Skill(s)
    }
    //Move Methods
	public virtual void UnitMove()
    {
        // Move
    }
    public virtual void UnitMove(Vector3 movPos)
    {
        // Set Destination
        UnitDestination = movPos;
        // Unit moves
        flag_isMoving = true;
    }
    // Unit Got Hit
    public virtual void UnitGetHit()
    {
        // Do armor dmg magic
        Debug.Log("gethit" + (1 * (1 - 0.1f * UnitArmor)));
        UnitHP = UnitHP - 1;
    }

    //Abstract method for future use --> Check if unit Highlighted
    public abstract void UnitHighlighted();

    //Highlight Unit
    public void Highlight()
    {
        //Create Projector
        Projector highProject = new Projector();
        //Create Shader
        Shader highShader = new Shader();

    }
    
	
}
