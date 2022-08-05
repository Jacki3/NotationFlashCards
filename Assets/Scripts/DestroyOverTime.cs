using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float lifeTime = 1.5f;

    private float spawnTime;

    private void Awake()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        var progress = (Time.time - spawnTime) / lifeTime;

        if (progress < 1)
        {
            //can fade here
        }
        else
            Destroy(gameObject);
    }
}
