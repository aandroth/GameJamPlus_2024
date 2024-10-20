using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject m_playerTarget;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, m_playerTarget.transform.position.y, transform.position.z);
    }
}
