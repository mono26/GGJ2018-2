using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : ShipComponent
{
    [Header("Engine settings")]
    [SerializeField]
    protected float fuelLossPerSecond = 1.0f;
    [SerializeField]
    protected float maxFuel = 9999.0f;
    [SerializeField]
    protected float thrust = 1.0f;

    [Header("Components ")]
    [SerializeField]
    protected FuelBar fuelDisplay;

    [Header("Editor debugging")]
    [SerializeField]
    protected float currentFuel;
    public float CurrentFuel { get { return currentFuel; } }

    protected void Start()
    {
        currentFuel = maxFuel;
        fuelDisplay.UpdateBar(currentFuel, maxFuel);
        return;
    }

    public override void EveryFrame()
    {
        base.EveryFrame();

        fuelDisplay.UpdateBar(currentFuel, maxFuel);

        if (currentFuel <= 0)
        {
            GameController.Instance.LoseGame();
        }
    }

    protected override void HandleInput()
    {
        // Input from the mouse VERTICAL
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // Check if there is enough fuel for moving
            if (currentFuel > 0)
            {
                ApplyForce(transform.up);
                LoseFuel();
            }
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // Check if there is enough fuel for moving
            if (currentFuel > 0)
            {
                ApplyForce(-transform.up);
                LoseFuel();
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (currentFuel > 0)
            {
                ApplyForce(-transform.right);
                LoseFuel();
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (currentFuel > 0)
            {
                ApplyForce(transform.right);
                LoseFuel();
            }
        }

        return;
    }

    // Event for fuel loss
    // Fijarse si el radar esta prendido
    // Event for recharging fuel
    public void ApplyForce(Vector3 _direction)
    {
        ship.ShipBody.AddForce(_direction * thrust);
        return;
    }

    public void RechargeFuel()
    {
        currentFuel = maxFuel;
        return;
    }

    public void LoseFuel()
    {
        currentFuel -= fuelLossPerSecond * Time.deltaTime;
        return;
    }

    public void RecieveDamage(int daño)
    {
        currentFuel -= daño;
        return;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Look if is a fuel planet or a signal planet
        if (collision.gameObject.CompareTag("Fuel Planet"))
        {
            RechargeFuel();
        }
        if (collision.gameObject.CompareTag("Asteroide"))
        {
            RecieveDamage(25);
            Debug.Log("Recibi daño de asteroide");
            collision.gameObject.GetComponent<Asteroide>().ReleaseAsteroid();
            //TODO: definir bien el daño del asteroide
            //TODO Enviar o destruir asteoroide
        }
    }

    protected void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Blackhole"))
        {
            Debug.Log("Recibi daño de Hoyo Negro");
            RecieveDamage(30);
        }
    }
}
