using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField] GameObject goPrefab = null;

    List<Transform> enemyList = new List<Transform>();
    List<GameObject> hpBarList = new List<GameObject>();

    Camera camera;

    void Start()
    {
        camera = Camera.main;

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemy.Length; i++) 
        {
            enemyList.Add(enemy[i].transform);
            GameObject hp_bar = Instantiate(goPrefab, enemy[i].transform.position, Quaternion.identity, transform);
            hpBarList.Add(hp_bar);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hpBarList.Count; i++)
        {
            if (enemyList[i]==null)
            {
                Destroy(hpBarList[i]);
                hpBarList.RemoveAt(i);
                enemyList.RemoveAt(i);
            }
            hpBarList[i].transform.position = camera.WorldToScreenPoint(enemyList[i].position + new Vector3(0, 2.0f, 0));
        }
    }
}
