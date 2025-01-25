using UnityEngine;

public class Plant : MonoBehaviour
{
    private void FixedUpdate()
    {
        GameManager.GetInstance().Pollute(-Time.fixedDeltaTime * 0.1f);
    }
}