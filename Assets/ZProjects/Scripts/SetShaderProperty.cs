using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShaderProperty : MonoBehaviour {

    
    public string propertyName;
    public Transform moveObj;
    //public float depth = 0f;

    Material mat;

    void Start()
    {
        //
        mat = GetComponent<Renderer>().material;
        //mat.SetFloat("_MoveRate", depth);
        
    }

    // Update is called once per frame
    void Update () {

        if (moveObj != null){
            mat.SetVector(propertyName, moveObj.position);

        }else {

        }
	}
}
