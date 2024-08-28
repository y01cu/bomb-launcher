using UnityEngine;

public class WindArea : MonoBehaviour
{
    private Vector3 windForceVector;
    [SerializeField] private float windSpeed;

    private float timer = 0;
    private float interValForForceToChange = 1;

    public static WindArea Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interValForForceToChange)
        {
            windForceVector = new Vector3(Random.Range(-2 * windSpeed, 2 * windSpeed), Random.Range(windSpeed, 2 * windSpeed), Random.Range(windSpeed, 2 * windSpeed));
            timer = 0;
        }
    }

    public Vector3 GetWindForceVector()
    {
        return windForceVector;
    }
}
