using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour 
{
	[Header("Initial values")]
	[Tooltip("Minimun distance to the center to spawn planets")]
	[SerializeField] int minDistanceToSpawn = 15;
	[Tooltip("Maximum distance to the center to spawn planets")]
	[SerializeField] int maxDistanceToSpawn = 500;

	[Header("Randomization values")]
	[Tooltip("Minimum distance separtion from spawn perimeters")]
	[SerializeField] int minDistanceIntervalToSpawn = 15;
	[Tooltip("Maximum distance separtion from spawn perimeters")]
	[SerializeField] int maxDistanceIntervalToSpawn = 25;
	[Tooltip("Minimum angle interval to add up and walk the perimeter")]
	[SerializeField] int minAngleIntervalToSpawn = 30;
	[Tooltip("Maximum angle interval to add up and walk the perimeter")]
	[SerializeField] int maxAngleIntervalToSpawn = 60;
	
	[Header("Layers to check for a free spot")]
	[SerializeField] LayerMask layersToCheck;

	[Header("Prefabs")]
	[SerializeField] Planet fuelPlanetPrefab;
	[SerializeField] Planet[] planetsPrefabs;
	[SerializeField] SpawnableObject asteroidWall;

	[Header("Containers")]
	[SerializeField] Transform planetsContainer = null;
	[SerializeField] Transform asteroidWallContainer = null;

	Vector2 lastFuelPlanetPosition;

	int GetRandomDistanceInterval { get { return Random.Range(minDistanceIntervalToSpawn, maxDistanceIntervalToSpawn + 1); } }
	int GetRandomAngleInterval { get { return Random.Range(minAngleIntervalToSpawn, maxAngleIntervalToSpawn + 1); } }
	int GetRandomeStartAngle { get { return Random.Range(0, 365); } }
	int GetRandomPlanetToSpawn { get { return Random.Range(0, planetsPrefabs.Length); } }
	int GetRandomScale { get { return Random.Range(1, 6); } }

	Vector3 GetRandomRotation
	{
		get
		{
			float z = Random.Range(0, 365);
			return new Vector3(0, 0 , z);
		}
	}

	void Awake()
	{
		if(planetsContainer == null)
		{
			planetsContainer = new GameObject("Planets").transform;
		}

		if(asteroidWallContainer == null)
		{
			asteroidWallContainer = new GameObject("AsteroidWall").transform;
		}

		GenerateLevel();
	}

	void GenerateLevel()
	{
		int totalDistance = minDistanceToSpawn;
		while (totalDistance <= maxDistanceToSpawn)
		{
			SpawnPerimeter(totalDistance);

			totalDistance += GetRandomDistanceInterval;
		}

		int asteroidWallSpawnDistance = maxDistanceToSpawn + maxDistanceIntervalToSpawn;
		SpawnAsteroidRing(asteroidWallSpawnDistance);
		SpawnAsteroidRing(asteroidWallSpawnDistance + 5);
		SpawnAsteroidRing(asteroidWallSpawnDistance + 10);
	}

	void SpawnPerimeter(int _distance)
	{
		int angleCount = 0;
		int spawnAngle = GetRandomeStartAngle;
		int randomAngleInterval;

		// Spawn one FuelPlanet per perimeter.
		Vector2 fuelPlanetPosition = CalculatePointInPerimeter(_distance, spawnAngle);
		fuelPlanetPosition = AdjustFuelPlanetPosition(fuelPlanetPosition);
		SpawnPlanet(fuelPlanetPrefab, fuelPlanetPosition);
		lastFuelPlanetPosition = fuelPlanetPosition;

		while (angleCount < 365)
		{
			randomAngleInterval = GetRandomAngleInterval;
			spawnAngle += randomAngleInterval;
			angleCount += randomAngleInterval;
			if (angleCount >= 365)
			{
				continue;
			}

			SpawnPlanet(planetsPrefabs[GetRandomPlanetToSpawn], CalculatePointInPerimeter(_distance, spawnAngle));
		}
	}

	Vector2 AdjustFuelPlanetPosition(Vector2 _fuelPlanetPosition)
	{
		Vector2 newFuelPlanetPosition = _fuelPlanetPosition;
		// 1 = Change only one axis, 2 = change both axis
		int numberOfAxisChanges = Random.Range(1, 2);
		// 1 = x, 2 = y
		int axisToChange = Random.Range(1, 2);
		if(lastFuelPlanetPosition.x >= 0 && _fuelPlanetPosition.x >= 0 || lastFuelPlanetPosition.x < 0 && _fuelPlanetPosition.x < 0)
		{
			if (numberOfAxisChanges == 2 || axisToChange == 1)
			{
				newFuelPlanetPosition.x *= -1;
			}
		}
		if(lastFuelPlanetPosition.y >= 0 && _fuelPlanetPosition.y >= 0 || lastFuelPlanetPosition.y < 0 && _fuelPlanetPosition.y < 0)
		{
			if (numberOfAxisChanges == 2 || axisToChange == 2)
			{
				newFuelPlanetPosition.y *= -1;
			}
		}

		return newFuelPlanetPosition;
	}

	Vector2 CalculatePointInPerimeter(int _distance, float _angle)
	{
		Vector3 pointInPerimeter = Vector2.zero;
		float x = Mathf.Cos(_angle) * _distance;
        float y = Mathf.Sin(_angle) * _distance;
		pointInPerimeter = new Vector3(x, y, 0);
		return pointInPerimeter;
	}

	void SpawnPlanet(Planet _planetToSpawn, Vector2 _positionToSpawn)
	{
		if(!CheckIfPositionIsFree(_positionToSpawn, _planetToSpawn.GetGravFieldRadius))
		{
			return;
		}

		Planet newPlanet = Instantiate(_planetToSpawn, _positionToSpawn, Quaternion.identity);
		newPlanet.Awake();
		newPlanet.transform.SetParent(planetsContainer);
	}

	bool CheckIfPositionIsFree(Vector2 _position, float _radius)
	{
		bool positionIsFree = true;	
		if(Physics2D.OverlapCircle(_position, _radius, layersToCheck) != null)
		{
			positionIsFree = false;
		}

		return positionIsFree;
	}

	void SpawnAsteroidRing(int _distance)
	{
		float angleCount = 0;

		while (angleCount < 365)
		{
			if (angleCount >= 365)
			{
				continue;
			}

			int randomScale = GetRandomScale;
			angleCount++;
			SpawnAsteroid(asteroidWall, CalculatePointInPerimeter(_distance, angleCount), randomScale);
		}
	}

	void SpawnAsteroid(SpawnableObject _asteroidWall, Vector2 _position, int _scale)
	{
		SpawnableObject asteroid = Instantiate(_asteroidWall, _position, Quaternion.Euler(GetRandomRotation));
		float zScale = asteroid.transform.localScale.z;
		asteroid.transform.localScale =  new Vector3(_scale, _scale, zScale);
		asteroid.transform.SetParent(asteroidWallContainer);
	}
}
