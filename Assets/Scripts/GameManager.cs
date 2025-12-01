using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // floor → remaining enemies
    private Dictionary<int, int> enemiesRemaining = new Dictionary<int, int>();

    // floor → door list
    private Dictionary<int, List<Door>> doorsPerFloor = new Dictionary<int, List<Door>>();

    /*private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }*/
    void Awake()
    {
        Instance = this;    // ← force assign, even if one already existed
        enemiesRemaining.Clear();
        doorsPerFloor.Clear();
    }


    // -----------------------------
    // DOOR REGISTRATION
    // -----------------------------
    public void RegisterDoor(int floor, Door door)
    {
        if (!doorsPerFloor.ContainsKey(floor))
            doorsPerFloor[floor] = new List<Door>();

        if (!doorsPerFloor[floor].Contains(door))
            doorsPerFloor[floor].Add(door);

        // Make sure the door starts closed
        door.SetOpen(false);
        Debug.Log($"[DEBUG] RegisterDoor floor={floor} door={door?.name}", door);

    }

    // -----------------------------
    // ENEMY REGISTRATION
    // -----------------------------
    public void RegisterEnemy(int floor)
    {
        if (!enemiesRemaining.ContainsKey(floor))
            enemiesRemaining[floor] = 0;

        enemiesRemaining[floor]++;
        //Debug.Log($"[GM] Enemy registered on floor {floor}. Total = {enemiesRemaining[floor]}");
    }

    // -----------------------------
    // ENEMY KILLED
    // -----------------------------
    public void EnemyKilled(int floor)
    {
        if (!enemiesRemaining.ContainsKey(floor))
            return;

        enemiesRemaining[floor]--;

        if (enemiesRemaining[floor] <= 0)
        {
            enemiesRemaining[floor] = 0;
            OpenAllDoorsOnFloor(floor);
        }
    }

    // -----------------------------
    // OPEN DOORS
    // -----------------------------
    private void OpenAllDoorsOnFloor(int floor)
    {
        if (!doorsPerFloor.ContainsKey(floor))
            return;

        foreach (Door door in doorsPerFloor[floor])
        {
            if (door != null)
                door.SetOpen(true);
        }
    }
}


/*using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<int, int> enemiesRemaining = new Dictionary<int, int>();
    private Dictionary<int, List<Door>> doorsPerFloor = new Dictionary<int, List<Door>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // optional but safe
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        enemiesRemaining.Clear();
        doorsPerFloor.Clear();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "DungeonScene")
        {
            enemiesRemaining.Clear();
            doorsPerFloor.Clear();
        }
    }

    public void RegisterDoor(int floor, Door door)
    {
        if (!doorsPerFloor.ContainsKey(floor))
            doorsPerFloor[floor] = new List<Door>();

<<<<<<< Updated upstream
        doorsPerFloor[floor].Add(door);
        Debug.Log("Registered door on floor " + floor);
=======
        if (!doorsPerFloor[floor].Contains(door))
            doorsPerFloor[floor].Add(door);
>>>>>>> Stashed changes
    }

    public void RegisterEnemy(int floor)
    {
        if (!enemiesRemaining.ContainsKey(floor))
            enemiesRemaining[floor] = 0;

        enemiesRemaining[floor]++;
        Debug.Log($"[GM] Enemy registered on floor {floor}. Count = {enemiesRemaining[floor]}");
    }

    public void EnemyKilled(int floor)
    {
        if (!enemiesRemaining.ContainsKey(floor))
        {
<<<<<<< Updated upstream
            enemiesRemaining.Add(floor, 0);
        }

        enemiesRemaining[floor] += amount;
        Debug.Log("Floor " + floor + ": now has " + enemiesRemaining[floor] + " enemies.");
    }

    // ------------------------------------------------------
    // CALLED BY ENEMY WHEN IT DIES
    // ------------------------------------------------------
    public void EnemyDied(int floor)
    {
        if (!enemiesRemaining.ContainsKey(floor))
        {
            Debug.LogWarning("Enemy died but floor " + floor + " is not registered!");
            return;
=======
            enemiesRemaining[floor] = 0;
            Debug.LogWarning($"EnemyKilled called for unregistered floor {floor}. Clamping to 0.");
>>>>>>> Stashed changes
        }

        enemiesRemaining[floor] = Mathf.Max(0, enemiesRemaining[floor] - 1);
        Debug.Log($"[GM] Enemy killed on floor {floor}. Remaining = {enemiesRemaining[floor]}");

        Debug.Log("Enemy on floor " + floor + " died. Remaining: " + enemiesRemaining[floor]);

        if (enemiesRemaining[floor] == 0)
        {
<<<<<<< Updated upstream
            Debug.Log("Floor " + floor + " cleared! Opening doors.");

=======
>>>>>>> Stashed changes
            if (doorsPerFloor.ContainsKey(floor))
            {
                foreach (Door d in doorsPerFloor[floor])
                    if (d != null)
                        d.SetOpen(true);
            }
            else
            {
                Debug.LogWarning("There are no doors registered for floor " + floor);
            }
        }
    }

    public bool IsFloorClear(int floor)
    {
        return enemiesRemaining.ContainsKey(floor) && enemiesRemaining[floor] == 0;
    }
}*/
