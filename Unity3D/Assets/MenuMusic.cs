using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dont destroy this objects period

public class MenuMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        /*
        GameObject MenuPointer, RealPointer;
        MenuPointer = GameObject.FindWithTag("CellphonePointer");
        RealPointer = GameObject.FindWithTag("RealPointer");

        DontDestroyOnLoad(MenuPointer);
        DontDestroyOnLoad(RealPointer);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
