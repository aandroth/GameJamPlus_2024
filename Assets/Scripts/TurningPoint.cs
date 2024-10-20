using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPoint : MonoBehaviour
{
    public WorldTurning m_parentBuilding;

    public void TurnParentBuilding()
    {
        m_parentBuilding.TurnBuildingStart(true);
    }
}
