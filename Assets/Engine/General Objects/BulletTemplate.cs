using UnityEngine;
using System.Collections;

public class BulletTemplate : MonoBehaviour {

    public Vector3 Targeted { get; set; }
    public float speed { get; set; }
    public float BulletHitRadius { get; set; }
    GameObject Origin;


    
    public virtual void Initialize(GameObject origin, GameObject target)
    {
        this.transform.position = origin.transform.position;
        Origin = origin;
        AddTarget(target);
    }

	// Use this for initialization
	void Start () {
        speed = 3;
        BulletHitRadius = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {

        StrikeAtThee();
        int i = 0;
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, BulletHitRadius);
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].GetComponent<UnitTemplate>() != null && hitColliders[i].gameObject != Origin)
            {
                var temp = (UnitExample01)hitColliders[i].GetComponent<UnitExample01>();

                temp.UnitGetHit();
                Destroy(this.gameObject);
            }
            i++;
        }

        if (this.transform.position == Targeted)
        {
            Destroy(this.gameObject);
        }
        
	}

    public virtual void AddTarget(GameObject target)
    {
        Targeted = target.transform.position;
    }

    public virtual void StrikeAtThee()
    {
        if (Targeted != null)
            transform.position = Vector3.MoveTowards(transform.position, Targeted, Time.deltaTime * speed);
    }

    void OnCollionEnter(Collision collision)
    {
        if (this.gameObject.GetComponent<Collider>() != null) {
            GameObject coll = this.gameObject.GetComponent<Collider>().gameObject;
            if (coll.GetComponent<UnitTemplate>() != null)
            {
                UnitExample01 unit = coll.GetComponent<UnitExample01>();
                unit.UnitGetHit();
                Destroy(this.gameObject);
            }
        }
    }
}
