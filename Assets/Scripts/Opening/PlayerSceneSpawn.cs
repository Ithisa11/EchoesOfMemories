using UnityEngine;

public class PlayerSceneSpawn : MonoBehaviour
{
    [Tooltip("Used if SpawnRouter.nextSpawnObjectName is empty or not found.")]
    public string defaultSpawnObjectName = "Spawn_Bedroom";

    [Tooltip("If true, clears SpawnRouter after spawning.")]
    public bool clearAfterSpawn = true;

    private void Start()
    {
        // If something else spawns/moves the player first, Start() is a good place to override it.
        string wanted = SpawnRouter.nextSpawnObjectName;

        // Try requested spawn first
        if (!string.IsNullOrEmpty(wanted))
        {
            var requested = GameObject.Find(wanted);
            if (requested != null)
            {
                transform.position = requested.transform.position;
                if (clearAfterSpawn) SpawnRouter.nextSpawnObjectName = "";
                return;
            }
            else
            {
                Debug.LogWarning($"PlayerSceneSpawn: Requested spawn '{wanted}' not found. Falling back to default.");
            }
        }

        // Fall back to default
        var fallback = GameObject.Find(defaultSpawnObjectName);
        if (fallback != null)
        {
            transform.position = fallback.transform.position;
        }
        else
        {
            Debug.LogWarning($"PlayerSceneSpawn: Default spawn '{defaultSpawnObjectName}' not found. Player stays where it is.");
        }

        if (clearAfterSpawn) SpawnRouter.nextSpawnObjectName = "";
    }
}