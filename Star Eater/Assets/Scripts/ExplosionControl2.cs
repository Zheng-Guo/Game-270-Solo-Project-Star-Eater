using UnityEngine;
using System.Collections;

public class ExplosionControl2 : MonoBehaviour {

    public GameObject Supernova;
    public int Mass;
    private int ExplodeCount;
    private int BlastRange;
    private float ImageObjectRatio = 30f;
    void Awake()
    {
        //Mass = Supernova.GetComponent<SupernovaControl>().Mass;
    }
	void Start () {
  //      Mass = 1000000;
        BlastRange=ExplodeCount = Mathf.CeilToInt(Mathf.Pow(Mass, 1f / 3f) / 2f);
        transform.localScale = new Vector3(1f / ImageObjectRatio, 1f / ImageObjectRatio, 0);	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (ExplodeCount > 0)
        {
            ExplodeCount--;
            transform.localScale = new Vector3(1f / ImageObjectRatio * (BlastRange - ExplodeCount), 1f / ImageObjectRatio * (BlastRange - ExplodeCount), 0);
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
