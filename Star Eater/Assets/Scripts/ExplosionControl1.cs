using UnityEngine;
using System.Collections;

public class ExplosionControl1 : MonoBehaviour {

    private int ExplodeCount;
	// Use this for initialization
	void Start () {
        ExplodeCount = 50;
	}
	
	// Update is called once per frame
	void Update () {
        if (ExplodeCount > 0)
            ExplodeCount--;
        else
            Destroy(gameObject);
	}
}
