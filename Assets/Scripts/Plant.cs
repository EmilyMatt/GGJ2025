using UnityEngine;

public class Plant : MonoBehaviour
{
    public float pollutionDecrease;

    private void Start()
    {
        Destroy(gameObject, Random.Range(15, 25));
    }

    private void FixedUpdate()
    {
        GameManager.GetInstance().Pollute(-pollutionDecrease * Time.fixedDeltaTime);
    }
}