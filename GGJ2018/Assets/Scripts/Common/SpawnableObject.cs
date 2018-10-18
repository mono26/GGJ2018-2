using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(CircleCollider2D))]
public class SpawnableObject : MonoBehaviour
{
    [Header("Spawnable settings")]
    [SerializeField]
    private float radius;
    public float GetRadius { get { return radius; } }

    private void Start() 
    {
        radius = GetComponent<CircleCollider2D>().radius;
        return;
    }
}
