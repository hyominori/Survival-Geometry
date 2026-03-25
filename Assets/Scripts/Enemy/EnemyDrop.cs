using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    public GameObject expOrbPrefab;
    public GameObject healOrbPrefab;

    public int expAmount = 1;

    [Range(0f, 1f)]
    public float expDropChance = 0.8f;

    [Range(0f, 1f)]
    public float healDropChance = 0.2f;

    public void Drop()
    {
        if (Random.value < expDropChance)
        {
            GameObject expOrb = Instantiate(expOrbPrefab, transform.position, Quaternion.identity);
            expOrb.GetComponent<ExpOrb>().Init(expAmount);
        }
        if (Random.value < healDropChance)
        {
            Instantiate(healOrbPrefab, transform.position, Quaternion.identity);
        }
    }
}