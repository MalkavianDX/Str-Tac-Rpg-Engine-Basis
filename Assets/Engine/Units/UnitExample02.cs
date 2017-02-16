using UnityEngine;
using System.Collections;
using System;

public class UnitExample02 : UnitTemplate
{
    float UnitSearchRadius;
    void Start()
    {
        UnitArmor = 1;
        UnitHP = 10;
        UnitSpeed = 10;
        UnitEnergy = 3;
        UnitTeam = 2;
        UnitSearchRadius = 30f;
    }

    void Update()
    {
        /*Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, UnitSearchRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].GetComponent<UnitTemplate>() != null)
            {
                var temp = (UnitExample01)hitColliders[i].GetComponent<UnitExample01>();
                if (temp.UnitTeam != this.UnitTeam)
                    temp.UnitGetHit();
                else
                    ;//ignorance is bliss
            }
            i++;
        }
        */
    }

    public void UnitGetHit()
    {
        Debug.Log("gethit");
    }

    public override void UnitHighlighted()
    {
        // Play sound etc.
    }
    /*
    public override void UnitMove()
    {
        throw new NotImplementedException();
    }

    public override void UnitShoot()
    {
        throw new NotImplementedException();
    }

    public override void UnitSkill()
    {
        throw new NotImplementedException();
    }*/
}
