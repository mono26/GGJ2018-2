using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [Header("Engine settings")]
    [SerializeField]
    protected float fuelLossRate = 1.0f;
    [SerializeField]
    protected float MaxFuel = 9999.0f;
    [SerializeField]
    protected float Thrust = 1.0f;

    [Header("Components")]
    [SerializeField]
    protected Ship ship;

    [Header("Editor debugging")]
    [SerializeField]
    protected float currentFuel;
    public float CurrentFuel { get { return currentFuel; } }

    // Event for fuel loss
    // Fijarse si el radar esta prendido
    // Event for recharging fuel
    public void ApplyForce(Vector3 _direction)
    {
        ship.Rigidbody2D.AddForce(_direction * Thrust);
        return;
    }

    public void RechargeFuel()
    {
        currentFuel = MaxFuel;
        return;
    }

    public void LoseFuel()
    {
        if (ship.Radar.IsRadarOn)
        {
            currentFuel -= 2 * fuelLossRate;
            return;
        }
        else
        {
            currentFuel -= 1 * fuelLossRate;
            return;
        }
    }

    public void RecieveDamage(int daño)
    {
        currentFuel -= daño;
        return;
    }
}
