using UnityEngine;

public class BombLauncher : MonoBehaviour
{
    [SerializeField] private Bomb bombPrefab;
    [SerializeField] private Transform missleSpawnParentTransform;
    [SerializeField] private float BombSpeed;

    private void Start()
    {
        Fire();
    }
    private void Fire()
    {
        var newMissle = Instantiate(bombPrefab, missleSpawnParentTransform.position, Quaternion.identity);
        newMissle.transform.Rotate(Vector3.forward * (transform.localRotation.eulerAngles.z - 90));
        newMissle.AssignSpeed(BombSpeed);
        Debug.Log($"local scale {newMissle.transform.localScale} | loosy scale {newMissle.transform.lossyScale}");


        // var missleScale = newMissle.transform.localScale;
        // newMissle.transform.localScale = new Vector3(missleScale.x / parentScale.x, missleScale.y / parentScale.y, missleScale.z / parentScale.z);
    }
}
