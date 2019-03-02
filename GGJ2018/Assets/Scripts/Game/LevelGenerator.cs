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

	bool isInitialGeneration = true;

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

		if (isInitialGeneration)
		{
			GenerateLevel(Vector2.zero);
			isInitialGeneration = false;
		}
	}

	void GenerateLevel(Vector2 _perimeterFromWherePlayerCame)
	{
		int totalDistance = minDistanceToSpawn;
		Vector2 lastFuelPlanetPosition = Vector2.zero;
		while (totalDistance <= maxDistanceToSpawn)
		{
			SpawnPerimeter(totalDistance, lastFuelPlanetPosition, _perimeterFromWherePlayerCame);

			totalDistance += GetRandomDistanceInterval;
		}

		// int asteroidWallSpawnDistance = maxDistanceToSpawn + maxDistanceIntervalToSpawn;
		// SpawnAsteroidRing(asteroidWallSpawnDistance);
		// SpawnAsteroidRing(asteroidWallSpawnDistance + 5);
		// SpawnAsteroidRing(asteroidWallSpawnDistance + 10);
	}

	void SpawnPerimeter(int _distance, Vector2 _lastFuelPlanetPosition, Vector2 _perimeterFromWherePlayerCame)
	{
		int angleCount = 0;
		int spawnAngle = GetRandomeStartAngle;
		int randomAngleInterval;

		// Spawn one FuelPlanet per perimeter.
		Vector2 fuelPlanetPosition = CalculateLocalSpawnPoint(_distance, spawnAngle);
		fuelPlanetPosition = AdjustFuelPlanetPosition(fuelPlanetPosition, _lastFuelPlanetPosition);
		SpawnPlanet(fuelPlanetPrefab, fuelPlanetPosition, _perimeterFromWherePlayerCame);
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

			SpawnPlanet(planetsPrefabs[GetRandomPlanetToSpawn], CalculateLocalSpawnPoint(_distance, spawnAngle), _perimeterFromWherePlayerCame);
		}
	}

	/// <summary>
	/// Generates a local position spawn point inside a circle perimeter relative to the LevelGenerator.
	/// </summary>
	/// <param name="_distance"> Radius of the circle.</param>
	/// <param name="_angle"> Angle of the point in the perimeter.</param>
	/// <returns></returns>
	Vector2 CalculateLocalSpawnPoint(int _distance, float _angle)
	{
		Vector3 pointInPerimeter = Vector2.zero;
		float x = Mathf.Cos(_angle) * _distance;
        float y = Mathf.Sin(_angle) * _distance;
		pointInPerimeter = new Vector2(x, y);
		return pointInPerimeter;
	}

	Vector2 AdjustFuelPlanetPosition(Vector2 _fuelPlanetPosition, Vector2 lastFuelPlanetPosition)
	{
		Vector2 newFuelPlanetPosition = _fuelPlanetPosition;
		// 1 = Change only one axis, 2 = change both axis
		int numberOfAxisChanges = Random.Range(1, 2);
		// 1 = x, 2 = y
		int axisToChange = Random.Range(1, 2);
		if(lastFuelPlanetPosition.x > 0 && _fuelPlanetPosition.x > 0 || lastFuelPlanetPosition.x < 0 && _fuelPlanetPosition.x < 0)
		{
			if (numberOfAxisChanges == 2 || axisToChange == 1)
			{
				newFuelPlanetPosition.x *= -1;
			}
		}
		if(lastFuelPlanetPosition.y > 0 && _fuelPlanetPosition.y > 0 || lastFuelPlanetPosition.y < 0 && _fuelPlanetPosition.y < 0)
		{
			if (numberOfAxisChanges == 2 || axisToChange == 2)
			{
				newFuelPlanetPosition.y *= -1;
			}
		}

		return newFuelPlanetPosition;
	}

	void SpawnPlanet(Planet _planetToSpawn, Vector2 _localPositionToSpawn, Vector2 _perimeterFromWherePlayerCame)
	{
		if (!isInitialGeneration && CheckIfSameFromPerimeterOfLastSpawn(_localPositionToSpawn, _perimeterFromWherePlayerCame))
		{
			return;
		}

		if(!CheckIfPositionIsFree(_localPositionToSpawn, _planetToSpawn.GetGravFieldRadius))
		{
			return;
		}

		Vector3 worldPositionToSpawn = transform.position;
		worldPositionToSpawn.x += _localPositionToSpawn.x;
		worldPositionToSpawn.y += _localPositionToSpawn.y;
		Planet newPlanet = Instantiate(_planetToSpawn, worldPositionToSpawn, Quaternion.identity);
		newPlanet.Awake();
		newPlanet.transform.SetParent(planetsContainer);
	}

	bool CheckIfPositionIsFree(Vector2 _localPosition, float _radius)
	{
		bool positionIsFree = true;
		Vector3 positionToCheck = transform.position;
		positionToCheck.x += _localPosition.x;
		positionToCheck.y += _localPosition.y;
		if(Physics2D.OverlapCircle(positionToCheck, _radius + 10, layersToCheck) != null)
		{
			positionIsFree = false;
		}

		return positionIsFree;
	}

	bool CheckIfSameFromPerimeterOfLastSpawn(Vector2 _position, Vector2 _perimeterFromWherePlayerCame)
	{
		Vector2 matchedPerimeterToGenerationPosition = -_perimeterFromWherePlayerCame;
		bool sameXSide = false;
		bool sameYSide = false;
		if (matchedPerimeterToGenerationPosition.x > 0 && _position.x > 0 || matchedPerimeterToGenerationPosition.x < 0 && _position.x < 0)
		{
			sameXSide = true;
		}
		else if (matchedPerimeterToGenerationPosition.x > 0 && _position.x > 0 && matchedPerimeterToGenerationPosition.y == 0 && _position.y == 0)
		{
			return true;
		}
		else if (matchedPerimeterToGenerationPosition.x < 0 && _position.x < 0 && matchedPerimeterToGenerationPosition.x == 0 && _position.x == 0)
		{
			return true;
		}
		
		if (matchedPerimeterToGenerationPosition.y > 0 && _position.y > 0 || matchedPerimeterToGenerationPosition.y < 0 && _position.y < 0)
		{
			sameYSide = true;
		}
		else if (matchedPerimeterToGenerationPosition.x == 0 && _position.x == 0 && matchedPerimeterToGenerationPosition.y > 0 && _position.y > 0)
		{
			return true;
		}
		else if (matchedPerimeterToGenerationPosition.x == 0 && _position.x == 0 && matchedPerimeterToGenerationPosition.y < 0 && _position.y < 0)
		{
			return true;
		}

		return (sameXSide && sameYSide) ? true : false;
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
			SpawnAsteroid(asteroidWall, CalculateLocalSpawnPoint(_distance, angleCount), randomScale);
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

		Debug.LogError("Player exited the trigger");
		Vector2 playerPosition = other.transform.position;
		Vector2 playerLocalPosition = transform.InverseTransformPoint(playerPosition);
		Vector2 perimeterFromWherePlayerCame = GetPerimeterFromWherePlayerCameFrom(playerLocalPosition);
		Debug.LogError("Player came from perimeter: " + perimeterFromWherePlayerCame);
		Debug.LogError("Matched perimeter: " + -perimeterFromWherePlayerCame);
		transform.position = playerPosition;

		GenerateLevel(perimeterFromWherePlayerCame);
	}

	Vector2 GetPerimeterFromWherePlayerCameFrom(Vector2 _playerLocalPosition)
	{
		float xPosition = _playerLocalPosition.x;
		float yPosition = _playerLocalPosition.y;
		Vector2 perimeterFromWherePlayerCame = new Vector2(xPosition / Mathf.Abs(xPosition), yPosition / Mathf.Abs(yPosition));
		return perimeterFromWherePlayerCame;
	}
}
