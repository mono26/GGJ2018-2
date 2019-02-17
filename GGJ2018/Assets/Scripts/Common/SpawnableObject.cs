using UnityEngine;

[System.Serializable]
public class SpawnableObject : MonoBehaviour
{
    [Header("Spawnable settings")]
    [SerializeField]
    private float radius;
    public float GetRadius { get { return radius; } }
}
