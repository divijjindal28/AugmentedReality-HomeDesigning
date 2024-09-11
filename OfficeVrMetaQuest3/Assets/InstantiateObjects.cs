using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjects : MonoBehaviour
{
    public GameObject Parent;
     
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Object Intantiated STarted");
    }

    public void InstantiatePrefab(GameObject prefab) {
        Debug.Log("Object Intantiated");

        GameObject instance = Instantiate(prefab,Parent.transform.position,Parent.transform.rotation);
        instance.transform.parent = Parent.transform;
        Debug.Log(Parent.transform.childCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
