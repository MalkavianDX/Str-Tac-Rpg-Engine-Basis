using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapRulesExample1 : MapRulesTemplate
{


    public override void CreateMap (GameObject parent)
    {
        GameObject FirstSlot = GameObject.Find("GroundCube");
        GameObject Block = new GameObject();
        for (int i = 0; i < 60; i++)
        {
            if(i!=0)
                Block = Instantiate(FirstSlot, new Vector3(FirstSlot.transform.position.x - (3*i),FirstSlot.transform.position.y,FirstSlot.transform.position.z), new Quaternion()) as GameObject;
            Block.transform.SetParent(parent.transform);
            for(int j = 1; j < 61; j++)
            {
                Block = Instantiate(FirstSlot, new Vector3(FirstSlot.transform.position.x - (3 * i), FirstSlot.transform.position.y, FirstSlot.transform.position.z - (3*j)), new Quaternion()) as GameObject;
                Block.transform.SetParent(parent.transform);
            }
        }
    }

	public override void VictoryCondition ()
	{
		throw new System.NotImplementedException ();
	}

	public override void MapFactors ()
	{
		throw new System.NotImplementedException ();
	}

}


