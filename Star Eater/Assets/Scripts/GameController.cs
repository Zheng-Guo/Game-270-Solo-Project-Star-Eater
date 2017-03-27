using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameConstant
{
    public static Vector3 MonsterInitialPosition = new Vector3(0, -4, 0);
    public static Vector3 BackgroundInitialPosition = new Vector3(0, 25, 0);
    public static float LeftBound = -11f;
    public static float RightBound = 11f;
    public static float TopBound = 3f;
    public static float BottomBound = -5f;
    public static int PlayerInitialMass = 400;
    public static int PlayerCriticalMass = 100;
    public static int PlayerFullStrengthMass = 4000;
    public static int AccelerationMassLosePercentage = 1;
    public static int FireMassLosePercentage = 2;
    public static int SuckingMassLoseRate = 20;
    public static int MinimumStarMass = 50;
    public static int MassThreshold1 = 1000;
    public static int MassThreshold2 = 5000;
    public static int MassThreshold3 = 10000;
    public static int MassThreshold4 = 20000;
    public static int MassThreshold5 = 1000000;
    public static int BlackholeFrequency1 = 20;
    public static int BlackholeFrequency2 = 10;
    public static int BlackholeFrequency3 = 5;
    public static int SupernovaFrequency1 = 32;
    public static int SupernovaFrequency2 = 16;
    public static int SupernovaFrequency3 = 8;
    public static int StarDensity = 30;
    public static int FireCost = 1000;
    public static int FireDamage = 4000;
    public static int GameDuration = 90;
    public static int BufferTime=100;
    public static int BackgroundLimit=-24;
    public static string Instruction1="Press Arrow Keys to control direction.\nPress Esc to quit.\nPress R to restart.\n";
    public static string Instruction2 = "nsufficient mass to accelerate. Need at least "+PlayerCriticalMass+".\n";
    public static string Instruction3 = "Press 'X' to accelerate(cost "+AccelerationMassLosePercentage+"% of the mass per second).\n";
    public static string Instruction4 = "Insufficient mass to fire. Need at least "+PlayerFullStrengthMass+".\n";
    public static string Instruction5 = "Press 'Z' to fire(cost "+FireCost+ " mass per shot).\n";
}

public class GameController : MonoBehaviour {

    public GameObject Star;
    public GameObject Blackhole;
    public GameObject Supernova;
    public GameObject Monster;
    public GameObject Background;
    public Vector3 spawnValues=new Vector3(10,7,0);
    private float SpawnWait=1f;
    private float StartWait=2f;
    private float WaveWait = 1f;
    public int MassUppperLimit;
    public Text StartingMessage;
    private int DisplayCount;
    private IEnumerator StarSpawn;

    void Start()
    {
        Monster = GameObject.FindGameObjectWithTag("Player");
        int MonsterMass = Monster.GetComponent<Player>().Mass;
        FindMassUpperLimit(MonsterMass);
        StarSpawn = SpawnWaves();
        //StartCoroutine(SpawnWaves());
        StartCoroutine(StarSpawn);
        DisplayCount = GameConstant.BufferTime;
    }

    void FixedUpdate(){
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("Star Eater");
        }
        if (DisplayCount > GameConstant.BufferTime/2)
        {
            DisplayCount--;
            StartingMessage.color = Color.red;
            StartingMessage.text = "Ready...";
        }
        else if (DisplayCount <= GameConstant.BufferTime / 2 && DisplayCount > 0)
        {
            DisplayCount--;
            StartingMessage.color = Color.green;
            StartingMessage.text = "Eat!!!";
        }else
            StartingMessage.text = "";
    }

    IEnumerator SpawnWaves(){
        yield return new WaitForSeconds(StartWait);
        Monster.GetComponent<Player>().GameStart = true;
        while (true)
        {
            int MonsterMass = Monster.GetComponent<Player>().Mass;
            FindMassUpperLimit(MonsterMass);
            for (int i = 0; i < GameConstant.StarDensity; i++)
            {
                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, 0);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(Star, spawnPosition, spawnRotation);
                spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, 0);
                if (MassUppperLimit > GameConstant.MassThreshold2 && MassUppperLimit <= GameConstant.MassThreshold3 && 0 == UnityEngine.Random.Range(0, GameConstant.BlackholeFrequency1))
                    Instantiate(Blackhole, spawnPosition, spawnRotation);
                if (MassUppperLimit > GameConstant.MassThreshold3 && MassUppperLimit <= GameConstant.MassThreshold4 && 0 == UnityEngine.Random.Range(0, GameConstant.BlackholeFrequency2))
                    Instantiate(Blackhole, spawnPosition, spawnRotation);
                if (MassUppperLimit > GameConstant.MassThreshold4 && 0 == UnityEngine.Random.Range(0, GameConstant.BlackholeFrequency3))
                    Instantiate(Blackhole, spawnPosition, spawnRotation);
                spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, 0);
                if (MassUppperLimit > GameConstant.MassThreshold2 && MassUppperLimit <= GameConstant.MassThreshold3 && 0 == UnityEngine.Random.Range(0, GameConstant.SupernovaFrequency1))
                    Instantiate(Supernova, spawnPosition, spawnRotation);
                if (MassUppperLimit > GameConstant.MassThreshold3 && MassUppperLimit <= GameConstant.MassThreshold4 && 0 == UnityEngine.Random.Range(0, GameConstant.SupernovaFrequency2))
                    Instantiate(Supernova, spawnPosition, spawnRotation);
                if (MassUppperLimit > GameConstant.MassThreshold4 && 0 == UnityEngine.Random.Range(0, GameConstant.SupernovaFrequency3))
                    Instantiate(Supernova, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(SpawnWait);
            }
        }
    }

    void FindMassUpperLimit(int MonsterMass)
    {
        if (MonsterMass < GameConstant.MassThreshold1)
            MassUppperLimit = GameConstant.MassThreshold1;
        else if (MonsterMass >= GameConstant.MassThreshold1 && MonsterMass < GameConstant.MassThreshold2)
            MassUppperLimit = GameConstant.MassThreshold2*2;
        else if (MonsterMass >= GameConstant.MassThreshold2 && MonsterMass < GameConstant.MassThreshold3)
            MassUppperLimit = GameConstant.MassThreshold3*2;
        else if (MonsterMass >= GameConstant.MassThreshold3 && MonsterMass < GameConstant.MassThreshold4)
            MassUppperLimit = GameConstant.MassThreshold4*2;
        else if (MonsterMass >= GameConstant.MassThreshold4)
            MassUppperLimit = GameConstant.MassThreshold4*2;
    }
}
