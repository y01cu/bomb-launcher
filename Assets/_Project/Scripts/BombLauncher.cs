using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLauncher : MonoBehaviour
{
    [Header("Line renderer veriables")]
    public LineRenderer line;
    [Range(2, 50)]
    public int resolution;

    [Header("Formula variables")]
    public Vector3 velocity;
    public float yLimit;
    private float gravity;

    [Header("Linecast variables")]
    [Range(2, 50)]
    public int linecastResolution;
    public LayerMask canHit;
    [SerializeField] private Bomb bombPrefab;
    [SerializeField] private float prefabSpeed = 1f;
    [SerializeField] private Camera camera;
    [SerializeField] float sensitivity = 1f;

    [SerializeField] private WindArea windArea;

    [SerializeField] private GameObject hitAreaObject;

    private void Start()
    {
        gravity = Mathf.Abs(Physics.gravity.y);
        LineRendererManager.Setup(line);
        hitAreaObject.SetActive(true);
    }
    private void Update()
    {
        RenderArc();
        TurnMousePointToVelocity();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            StartCoroutine(FollowArc(bomb));
        }
        hitAreaObject.transform.position = HitPosition();
    }
    private IEnumerator FollowArc(Bomb bomb)
    {
        float totalTime = MaxTimeY();
        float elapsedTime = 0f;

        Vector3 currentVelocityVector = velocity;

        while (elapsedTime < 10f)
        {
            float t = elapsedTime / totalTime;
            Vector3 position = CalculateLinePoint(currentVelocityVector, t);
            bomb.transform.position = position;
            elapsedTime += Time.deltaTime * prefabSpeed;
            yield return null;
        }

        Destroy(bomb.gameObject);
    }


    private void TurnMousePointToVelocity()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = camera.nearClipPlane;
        // Vector3 worldPosition = camera.ScreenToWorldPoint(mousePosition);



        Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -mousePosition.z));
        // Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x * 1.2f, mousePosition.y * 2f, -mousePosition.z));

        velocity = (transform.position - worldPosition) * sensitivity;
    }

    private void RenderArc()
    {
        line.positionCount = resolution + 1;
        line.SetPositions(CalculateLineArray());
    }

    private Vector3[] CalculateLineArray()
    {
        Vector3[] lineArray = new Vector3[resolution + 1];

        var lowestTimeValueX = MaxTimeX() / resolution;
        var lowestTimeValueZ = MaxTimeZ() / resolution;
        var lowestTimeValue = lowestTimeValueX > lowestTimeValueZ ? lowestTimeValueZ : lowestTimeValueX;

        for (int i = 0; i < lineArray.Length; i++)
        {
            var t = lowestTimeValue * i;
            lineArray[i] = CalculateLinePoint(t);
        }

        return lineArray;
    }

    private Vector3 HitPosition()
    {
        var lowestTimeValue = MaxTimeY() / linecastResolution;

        for (int i = 0; i < linecastResolution + 1; i++)
        {
            RaycastHit rayHit;

            var t = lowestTimeValue * i;
            var tt = lowestTimeValue * (i + 1);

            if (Physics.Linecast(CalculateLinePoint(t), CalculateLinePoint(tt), out rayHit, canHit))
                return rayHit.point;
        }

        return CalculateLinePoint(MaxTimeY());
    }

    private Vector3 CalculateLinePoint(float time)
    {
        float x = velocity.x * time;
        float z = velocity.z * time;
        float y = (velocity.y * time) - (gravity * Mathf.Pow(time, 2) / 2);
        return new Vector3(x + transform.position.x, y + transform.position.y, z + transform.position.z);
    }

    private Vector3 CalculateLinePoint(Vector3 velocity, float time)
    {
        float x = velocity.x * time + WindArea.Instance.GetWindForceVector().x * time;
        float z = velocity.z * time;
        float y = (velocity.y * time) - (gravity * Mathf.Pow(time, 2) / 2);
        return new Vector3(x + transform.position.x, y + transform.position.y, z + transform.position.z);
    }

    private float MaxTimeY()
    {
        var yVelocity = velocity.y;
        var yVelocitySquare = yVelocity * yVelocity;
        var time = (yVelocity + Mathf.Sqrt(yVelocitySquare + 2 * gravity * (transform.position.y - yLimit))) / gravity;
        return time;
    }

    private float MaxTimeX()
    {
        if (IsValueAlmostZero(velocity.x))
            SetValueToAlmostZero(ref velocity.x);

        var xVelocity = velocity.x;

        var time = (HitPosition().x - transform.position.x) / xVelocity;
        return time;
    }

    private float MaxTimeZ()
    {
        if (IsValueAlmostZero(velocity.z))
            SetValueToAlmostZero(ref velocity.z);

        var zVelocity = velocity.z;

        var time = (HitPosition().z - transform.position.z) / zVelocity;
        return time;
    }

    private bool IsValueAlmostZero(float value)
    {
        return value < 0.0001f && value > -0.0001f;
    }

    private void SetValueToAlmostZero(ref float value)
    {
        value = 0.0001f;
    }

    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }
}