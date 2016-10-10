using UnityEngine;
using System;
using System.Collections;

public class StarControl : MonoBehaviour {

    public int Mass;
    public GameObject Monster;
    public GameObject GameControl;
    private SpriteRenderer image;
    private float radius;
    private float ImageObjectRatio = 5.5f;
	// Use this for initialization
	void Start () {
        Monster = GameObject.FindGameObjectWithTag("Player");
        GameControl = GameObject.FindGameObjectWithTag("GameController");
        int MonsterMass;
        try
        {
            MonsterMass = Monster != null ? Monster.GetComponent<Player>().Mass : 0;
            Mass = UnityEngine.Random.Range(GameConstant.MinimumStarMass, GameControl.GetComponent<GameController>().MassUppperLimit);
            radius = Mathf.Pow(Mass, 1f / 3f) / 10f;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, UnityEngine.Random.Range(-1, -5), 0);
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = UnityEngine.Random.Range(-100, 100);

            image = GetComponent<SpriteRenderer>();
            image.color = Mass <= MonsterMass ? Color.white : Color.grey;
            transform.localScale = new Vector3(radius / ImageObjectRatio, radius / ImageObjectRatio, 0);
            GetComponent<CircleCollider2D>().radius = radius + 0.4f;
        }
        catch (MissingReferenceException e)
        {
            Destroy(gameObject);
        }
        catch (NullReferenceException e)
        {
            Destroy(gameObject);
        }
	}

    void OnEnable(){
        Player.OnMassChange += UpdateHighlight;
    }

    void OnDisable()
    {
        Player.OnMassChange -= UpdateHighlight;
    }

    void UpdateHighlight()
    {
        image.color = Mass <= Monster.GetComponent<Player>().Mass ? Color.white : Color.gray;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shot"))
        {
            Mass -= GameConstant.FireDamage;
            UpdateHighlight();
            if (Mass < 0)
                Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("GameController"))
        {
            Destroy(gameObject);
        }
    }

}
