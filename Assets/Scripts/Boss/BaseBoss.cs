using UnityEngine;

public abstract class BossBase : MonoBehaviour
{
    protected Transform player;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        BossUpdate();
    }

    protected abstract void BossUpdate();
}