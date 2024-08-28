using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float timer;
    private float timeRequiredToStartWindEffect = 0.1f;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeRequiredToStartWindEffect)
        {
            ApplyWindForce();
        }
    }

    private void ApplyWindForce()
    {
        transform.position += 10000 * Time.deltaTime * WindArea.Instance.GetWindForceVector();
    }
}
