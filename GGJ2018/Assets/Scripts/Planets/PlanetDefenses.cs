using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDefenses : MonoBehaviour 
{
	[Header("Settings")]
    [SerializeField] int minNumberOfWeapons = 2;
    [SerializeField] int maxNumberOfWeapons = 5;

	List<PlanetWeapon> weapons;

	void Awake() 
	{
		SpawnWeapons();
	}

    public void SpawnWeapons()
    {
        SpawnWeaponsInPlanet();
		Planet planet = GetComponent<Planet>();
	    LocateWeapons(planet);
        AlignWeaponsTowarsCenter();
    }

	void SpawnWeaponsInPlanet()
    {
		weapons = new List<PlanetWeapon>();

		int numberOfWeapons = Random.Range(minNumberOfWeapons, maxNumberOfWeapons + 1);

        for (int i = 0; i < numberOfWeapons; i++)
        {
            PlanetWeapon weapon = PoolsManager.Instance.GetObjectFromPool<PlanetWeapon>();
            weapon.transform.SetParent(transform);
			weapons.Add(weapon);
        }
    }

    protected void LocateWeapons(Planet _planet)
    {
		// To distribute weapons evenly
		float angle = Random.Range(0, 360);
		float angleIncrease = (float)360.0f / (float)weapons.Count;

        foreach (PlanetWeapon weapon in weapons)
        {
			angle += angleIncrease;
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * (_planet.GetRadius + 0.3f);
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * (_planet.GetRadius + 0.3f);
            weapon.transform.position = _planet.GetCenterPosition + new Vector2(x, y);
            Debug.DrawLine(_planet.GetCenterPosition, _planet.GetCenterPosition + new Vector2(x, y), Color.red, 3.0f);
        }
    }

    private void AlignWeaponsTowarsCenter()
    {
        foreach (PlanetWeapon weapon in weapons)
        {
            weapon.transform.up = (weapon.transform.position - transform.position).normalized;
        }
    }

	void OnDestroy() 
	{
		foreach (PlanetWeapon weapon in weapons)
		{
			Destroy(weapon.gameObject);
		}

		weapons.Clear();
	}
}
