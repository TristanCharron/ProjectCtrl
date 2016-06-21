using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjPooler
{
    Character holder;
    List<GameObject> list;
    public GameObject PooledObj { get { return getPooledObject(); } }
    public List<GameObject> PoolObjList { get { return getPooledObjectList(size, prefab); } }
    string prefab;
    int size;

    public ObjPooler(Character Holder, int Size, string Prefab)
    {
        holder = Holder;
        size = Size;
        prefab = Prefab;
        onStart();
    }

    public void onStart()
    {
        list = getPooledObjectList(size, prefab);
    }

    public List<GameObject> getPooledObjectList(int size, string prefabName)
    {
        if (list == null)
        {
            list = new List<GameObject>();
            for (int i = 0; i < size; i++)
            {
                list.Add(MonoBehaviour.Instantiate(Resources.Load(prefabName)) as GameObject);
                list[i].SetActive(false);
                list[i].transform.SetParent(holder.gameObject.transform);
            }
            return list;
        }
        else
            return list;
    }

    public GameObject getPooledObject()
    {

        if (list == null)
            list = getPooledObjectList(size, prefab);

        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeInHierarchy)
            {
                return list[i];
            }
        }
       
        return null;
    }

    public void onDestroy()
    {
        list.Clear();
    }


}
