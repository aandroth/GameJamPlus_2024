using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWin : MonoBehaviour
{
    public GameObject m_goalFace;
    public GameObject m_face;
    public GameObject m_tower;
    public Material m_winMaterial;
    public void LevelWinStart()
    {
        StartCoroutine(LevelWinCoroutine());
    }
    private IEnumerator LevelWinCoroutine()
    {
        m_goalFace.SetActive(false);
        yield return new WaitForSeconds(2);
        m_face.SetActive(true);
        //yield return new WaitForSeconds(3);
        //m_tower.GetComponent<MeshRenderer>().material = m_winMaterial;
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync("StartScreen");
    }
}
