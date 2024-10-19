using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTurning : MonoBehaviour
{
    public GameObject m_player;
    public float m_speed = 5f;
    public enum STATE { STATIC, TURNING}
    public STATE m_state = STATE.STATIC;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            TurnBuildingStart();
    }

    public void TurnBuildingStart()
    {
        StartCoroutine(TurnBuildingCoroutine());
        m_player.transform.parent = transform;
        m_player.GetComponent<PlayerController>().enabled = false;
    }

    public IEnumerator TurnBuildingCoroutine()
    {
        float angleChanged = 0;
        float angleOriginal = transform.rotation.eulerAngles.y;
        while (angleChanged < 90)
        {
            float change = m_speed * Time.deltaTime;

            angleChanged += change;
            transform.Rotate(new Vector3(0, change, 0));
            m_player.transform.rotation = Quaternion.identity;
            yield return null;
        }
        transform.eulerAngles = new Vector3(0, angleOriginal + 90, 0);
        m_player.transform.parent = null;
        m_player.GetComponent<PlayerController>().enabled = true;
    }
}
