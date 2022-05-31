
using UnityEngine;

public class Spawnscript : MonoBehaviour
{
    public GameObject prefab = null;
    public Transform spawn_point = null;
    public float min = 1.0f;
    public float max = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", Random.Range(min, max));
    }

    // Update is called once per frame
    private void Spawn()
    {
        GameObject obstacle = Instantiate(prefab);
        obstacle.transform.position = new Vector3(spawn_point.position.x,spawn_point.position.y,spawn_point.position.z);
        Invoke("Spawn", Random.Range(min, max));
    }
}
