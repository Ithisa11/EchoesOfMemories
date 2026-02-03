using UnityEngine;

public class PlayerSceneSpawn : MonoBehaviour
{
    public string defaultSpawnObjectName = "Spawn_Bedroom";
    public bool clearAfterSpawn = true;

    private void Start()
    {
        string wanted = SpawnRouter.nextSpawnObjectName;
        if (!string.IsNullOrEmpty(wanted))
        {
            var requested = GameObject.Find(wanted);
            if (requested != null)
            {
                transform.position = requested.transform.position;
                if (clearAfterSpawn) SpawnRouter.nextSpawnObjectName = "";
                return;
            }
        }

        var fallback = GameObject.Find(defaultSpawnObjectName);
        if (fallback != null)
        {
            transform.position = fallback.transform.position;
        }

        if (clearAfterSpawn) SpawnRouter.nextSpawnObjectName = "";
    }
}