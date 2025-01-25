using UnityEngine;

public class Plant : MonoBehaviour
{
    private void FixedUpdate()
    {
        PlayerController.GetInstance().Pollute(-Time.fixedDeltaTime * 0.1f);
    }
}