using UnityEngine;

public class Plant : MonoBehaviour
{
    public float pollutionDecrease;

    private void Start()
    {
        Destroy(gameObject, 20f);
    }

    private void FixedUpdate()
    {
        GameManager.GetInstance().Pollute(-pollutionDecrease * Time.fixedDeltaTime);
    }
}