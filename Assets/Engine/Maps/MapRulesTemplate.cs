using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MapRulesTemplate : MonoBehaviour {
    // Public Unit List
	public List<UnitTemplate> UnitList = new List<UnitTemplate>();

	// Use this for initialization
	void Start () {
        GameObject GroundParent = new GameObject("GroundParent");
        CreateMap(GroundParent);
	}
	
	// Update is called once per frame
	void Update () {

	}
    // Unit List management
	public virtual void UpdateUnitList(List<UnitTemplate> uList)
	{
		UnitList = uList;
	}
    public virtual void RemoveUnitList(UnitTemplate uUnit)
    {
        UnitList.Remove(uUnit);
    }
    public virtual void UpdateUnitList(UnitTemplate uUnit)
	{
		UnitList.Add(uUnit);
	}
	public virtual void EmptyUnitList()
	{
		UnitList.Clear ();
	}
	public virtual List<UnitTemplate> GetUnitList()
	{
		return UnitList;
	} 

    // For child classes
	public abstract void VictoryCondition ();
	public abstract void MapFactors ();
    public abstract void CreateMap(GameObject GroundParent);
}
