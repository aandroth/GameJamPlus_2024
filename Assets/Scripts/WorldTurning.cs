using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTurning : MonoBehaviour
{
    public GameObject m_player;
    public float m_speed = 5f;
    public int m_state = 0;
    public List<BoxCollider2D> sideA_Colliders;
    public List<BoxCollider2D> sideB_Colliders;
    public List<BoxCollider2D> sideC_Colliders;
    public List<BoxCollider2D> sideD_Colliders;
    private bool m_isTurning = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            TurnBuildingStart();
    }

    public void TurnBuildingStart(bool clockwiseTurn = true)
    {
        if (!m_isTurning)
        {
            m_isTurning = true;
            StartCoroutine(TurnBuildingCoroutine(clockwiseTurn));
        }
    }

    public IEnumerator TurnBuildingCoroutine(bool clockwiseTurn)
    {
        float angleChanged = 0;
        float angleOriginal = transform.rotation.eulerAngles.y;
        m_player.transform.parent = transform;
        m_player.GetComponent<PlayerController>().enabled = false;
        TurnOffColliders();
        while (angleChanged < 90)
        {
            float change = m_speed * Time.deltaTime;

            angleChanged += change;
            transform.Rotate(new Vector3(0, change, 0));
            m_player.transform.rotation = Quaternion.identity;
            yield return null;
        }
        TurnOnNewCollidersSide(clockwiseTurn);
        transform.eulerAngles = new Vector3(0, angleOriginal + 90, 0);
        m_player.transform.parent = null;
        m_player.GetComponent<PlayerController>().enabled = true;
        m_player.GetComponent<PlayerController>().FinishBuildingTurn();
        m_isTurning = false;

    }

    public void TurnOffColliders()
    {
        switch (m_state)
        {
            case 0:
                foreach (BoxCollider2D b in sideA_Colliders)
                    b.enabled = false;
                break;
            case 1:
                foreach (BoxCollider2D b in sideB_Colliders)
                    b.enabled = false;
                break;
            case 2:
                foreach (BoxCollider2D b in sideC_Colliders)
                    b.enabled = false;
                break;
            case 3:
                foreach (BoxCollider2D b in sideD_Colliders)
                    b.enabled = false;
                break;
            default:
                break;
        }
    }

    public void TurnOnNewCollidersSide(bool clockwiseTurn)
    {

        if (clockwiseTurn)
            m_state = (m_state + 1) % 4;
        else
            m_state = (m_state - 1) % 4;

        switch (m_state)
        {
            case 0:
                foreach (BoxCollider2D b in sideA_Colliders)
                    b.enabled = true;
                break;
            case 1:
                foreach (BoxCollider2D b in sideB_Colliders)
                    b.enabled = true;
                break;
            case 2:
                foreach (BoxCollider2D b in sideC_Colliders)
                    b.enabled = true;
                break;
            case 3:
                foreach (BoxCollider2D b in sideD_Colliders)
                    b.enabled = true;
                break;
            default:
                break;
        }
    }
}
