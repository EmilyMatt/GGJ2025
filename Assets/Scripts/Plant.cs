using UnityEngine;

public class Plant : MonoBehaviour
{
    public float pollutionDecrease;

    private void FixedUpdate()
    {
        GameManager.GetInstance().Pollute(-pollutionDecrease * Time.fixedDeltaTime);
    }
}