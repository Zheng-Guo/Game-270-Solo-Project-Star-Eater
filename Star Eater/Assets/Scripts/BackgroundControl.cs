using UnityEngine;
using System.Collections;

public class BackgroundControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate(){
        transform.position += new Vector3(0f, -0.01f, 0f);
        if(transform.position.y<GameConstant.BackgroundLimit)
            transform.position += GameConstant.BackgroundInitialPosition;
	}
}
