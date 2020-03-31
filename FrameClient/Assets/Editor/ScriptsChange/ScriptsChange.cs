using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptsChange 
{
    static GameObject  walls;

    [MenuItem("Tools/Prefab/Change")]
    static void Change()
    {
        GameObject ooo = new GameObject();
        for (int i = 1; i < 81; i++)
        {
            GameObject resObj = Resources.Load<GameObject>(PathUtils.levelPre + "level"+i);

            GameObject sceneObj = GameObject.Instantiate(resObj);

            sceneObj.name = "level" + i;

            walls = sceneObj.transform.Find("walls").gameObject;

            BoxCollider[] boxs = walls.GetComponentsInChildren<BoxCollider>();

            foreach (BoxCollider item in boxs)
            {
                Vector3 size = item.size;
                Vector3 center = item.center;
                Vector3 pos = item.transform.position;
                Quaternion rot = item.transform.localRotation;
                string name = item.gameObject.name;

                GameObject obj2d = new GameObject();
                BoxCollider2D box2d = obj2d.AddComponent<BoxCollider2D>();
                box2d.transform.parent = walls.transform;
                box2d.size = new Vector2(size.x, size.y);
                box2d.offset = new Vector2(center.x, center.y);
                box2d.transform.position = pos;
                box2d.transform.localRotation = rot;
                box2d.gameObject.name = name;
            }


            for (int j = 0; j < boxs.Length; j++)
            {
                boxs[j].transform.parent = ooo.transform;
            }
        }

       
    }


    [MenuItem("Tools/Prefab/Delete")]
    static void Delete()
    {

        GameObject ooo = GameObject.Find("ooo");

        for (int i = 1; i < 81; i++)
        {

            GameObject walls = ooo.transform.Find("walls" + i).gameObject;


            GameObject level = GameObject.Find("Level"+i);

            walls.name = "walls";

            walls.transform.parent = level.transform;
        }



    }


}
