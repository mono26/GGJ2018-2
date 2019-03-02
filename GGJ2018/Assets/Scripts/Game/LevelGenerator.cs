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
	[SerializeField] GameObject spawnedZoneMark;

	[Header("Containers")]
	[SerializeField] Transform planetsContainer = null;
	[SerializeField] Transform asteroidWallContainer = null;
	[SerializeField] Transform marksContainer;

	[Header("Clear parameters")]
	[SerializeField] int numberOfSpawnsToClearAZone = 4;

	bool isInitialGeneration = true;
	int numberOfSpawns = 0;
	Vector2 _lastFuelPlanetPosition;
	List<GameObject> spawnedZones;

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

		if(marksContainer == null)
		{
			marksContainer = new GameObject("SpawnedMarks").transform;
		}


		if (isInitialGeneration)
		{
			spawnedZones = new List<GameObject>();
			GenerateLevel();
			isInitialGeneration = false;
		}
	}

	void GenerateLevel()
	{
		int totalDistance = minDistanceToSpawn;
		Vector2 lastFuelPlanetPosition = Vector2.zero;
		while (totalDistance <= maxDistanceToSpawn)
		{
			SpawnPerimeter(totalDistance);

			totalDistance += GetRandomDistanceInterval;
		}

		numberOfSpawns++;

		// int asteroidWallSpawnDistance = maxDistanceToSpawn + maxDistanceIntervalToSpawn;
		// SpawnAsteroidRing(asteroidWallSpawnDistance);
		// SpawnAsteroidRing(asteroidWallSpawnDistance + 5);
		// SpawnAsteroidRing(asteroidWallSpawnDistance + 10);

		GameObject newMark = Instantiate(spawnedZoneMark, transform.position, Quaternion.identity);
		newMark.GetComponent<CircleCollider2D>().radius = maxDistanceToSpawn - maxDistanceIntervalToSpawn - minDistanceIntervalToSpawn;
		newMark.transform.SetParent(marksContainer);
		spawnedZones.Add(newMark);

		if (numberOfSpawns % numberOfSpawnsToClearAZone == 0)
		{
			GameObject markedZone = spawnedZones[0];
			spawnedZones.RemoveAt(0);
			Destroy(markedZone);
		}
	}

	void SpawnPerimeter(int _distance)
	{
		int angleCount = 0;
		int spawnAngle = GetRandomeStartAngle;
		int randomAngleInterval;

		// Spawn one FuelPlanet per perimeter.
		Vector2 fuelPlanetPosition = CalculateSpawnPointRelativeToGenerator(_distance, spawnAngle);
		fuelPlanetPosition = AdjustFuelPlanetPosition(fuelPlanetPosition, _lastFuelPlanetPosition);
		SpawnPlanet(fuelPlanetPrefab, fuelPlanetPosition);
		_lastFuelPlanetPosition = fuelPlanetPosition;

		while (angleCount < 365)
		{
			randomAngleInterval = GetRandomAngleInterval;
			spawnAngle += randomAngleInterval;
			angleCount += randomAngleInterval;
			if (angleCount >= 365)
			{
				continue;
			}

			Vector2 spawnPositionRelativeToGenerator = CalculateSpawnPointRelativeToGenerator(_distance, spawnAngle);
			SpawnPlanet(planetsPrefabs[GetRandomPlanetToSpawn], spawnPositionRelativeToGenerator);
		}
	}

	/// <summary>
	/// Generates a local position spawn point inside a circle perimeter relative to the LevelGenerator.
	/// </summary>
	/// <param name="_distance"> Radius of the circle.</param>
	/// <param name="_angle"> Angle of the point in the perimeter.</param>
	/// <returns></returns>
	Vector2 CalculateSpawnPointRelativeToGenerator(int _distance, float _angle)
	{
		Vector3 pointInPerimeter = Vector2.zero;
		float x = Mathf.Cos(_angle) * _distance;
        float y = Mathf.Sin(_angle) * _distance;
		pointInPerimeter = new Vector2(x, y);
		return pointInPerimeter;
	}

	Vector2 AdjustFuelPlanetPosition(Vector2 _fuelPlanetPosition, Vector2 _lastFuelPlanetPosition)
	{
		Vector2 newFuelPlanetPosition = _fuelPlanetPosition;
		// 1 = Change only one axis, 2 = change both axis
		int numberOfAxisChanges = Random.Range(1, 2);
		// 1 = x, 2 = y
		int axisToChange = Random.Range(1, 2);
		if(_lastFuelPlanetPosition.x > 0 && _fuelPlanetPosition.x > 0 || _lastFuelPlanetPosition.x < 0 && _fuelPlanetPosition.x < 0)
		{
			if (numberOfAxisChanges == 2 || axisToChange == 1)
			{
				newFuelPlanetPosition.x *= -1;
			}
		}
		if(_lastFuelPlanetPosition.y > 0 && _fuelPlanetPosition.y > 0 || _lastFuelPlanetPosition.y < 0 && _fuelPlanetPosition.y < 0)
		{
			if (numberOfAxisChanges == 2 || axisToChange == 2)
			{
				newFuelPlanetPosition.y *= -1;
			}
		}

		return newFuelPlanetPosition;
	}

	void SpawnPlanet(Planet _planetToSpawn, Vector2 _positionRelativeToGenerator)
	{
		if (!isInitialGeneration && CheckIfSameFromPerimeterOfLastSpawn(_positionRelativeToGenerator, _planetToSpawn.GetPlanetRadius))
		{
			return;
		}

		if(!CheckIfPositionIsFree(_positionRelativeToGenerator, _planetToSpawn.GetGravFieldRadius))
		{
			return;
		}

		Vector3 worldPositionToSpawn = transform.position;
		worldPositionToSpawn.x += _positionRelativeToGenerator.x;
		worldPositionToSpawn.y += _positionRelativeToGenerator.y;
		Planet newPlanet = Instantiate(_planetToSpawn, worldPositionToSpawn, Quaternion.identity);
		newPlanet.Awake();
		newPlanet.transform.SetParent(planetsContainer);
	}

	bool CheckIfPositionIsFree(Vector2 _positionRelativeToGenerator, float _radius)
	{
		bool positionIsFree = true;
		Vector3 positionToCheck = transform.position;
		positionToCheck.x += _positionRelativeToGenerator.x;
		positionToCheck.y += _positionRelativeToGenerator.y;
		if(Physics2D.OverlapCircle(positionToCheck, _radius, layersToCheck) != null)
		{
			positionIsFree = false;
		}

		return positionIsFree;
	}

	bool CheckIfSameFromPerimeterOfLastSpawn(Vector2 _positionRelativeToGenerator, float _radius)
	{
		bool inAlreadySpawnZone = false;
		Vector3 positionToCheck = transform.position;
		positionToCheck.x += _positionRelativeToGenerator.x;
		positionToCheck.y += _positionRelativeToGenerator.y;
		if(Physics2D.OverlapCircle(positionToCheck, _radius + 10, 1 << 17) != null)
		{
			inAlreadySpawnZone = true;
		}

		return inAlreadySpawnZone;
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
			SpawnAsteroid(asteroidWall, CalculateSpawnPointRelativeToGenerator(_distance, angleCount), randomScale);
		}
	}

	void SpawnAsteroid(SpawnableObject _asteroidWall, Vector2 _position, int _scale)
	{
		SpawnableObject asteroid = Instantiate(_asteroidWall, _position, Quaternion.Euler(GetRandomRotation));
		float zScale = asteroid.transform.localScale.z;
		asteroid.transform.localScale =  new Vector3(_scale, _scale, zScale);
		asteroid.transform.SetParent(asteroidWallContainer);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (!other.CompareTag("Player"))
		{
			return;
		}

		Vector2 playerPosition = other.transform.position;
		transform.position = playerPosition;

		GenerateLevel();
	}
}
