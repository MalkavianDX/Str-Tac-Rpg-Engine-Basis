using UnityEngine;
using System.Collections;
using System;

public class UnitExample01 : UnitTemplate
{
    float UnitSearchRadius;
    bool flag_UnitSighted;
    public int UnitTeam = 1;

    void Start()
    {
        UnitArmor = 1;
        UnitHP = 10;
        UnitSpeed = 5;
        UnitEnergy = 3;
        UnitTeam = 1;
        UnitSearchRadius = 15f;
        UnitReloadSpeed = 5.0f;
    }

    void Update()
    {
        //Check for target in radius
        UnitShoot();
    }

    public override void UnitHighlighted()
    {
        // Play sound etc.
    }

}
