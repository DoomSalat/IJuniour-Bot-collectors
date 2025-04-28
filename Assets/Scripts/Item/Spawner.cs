using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
	private const float MaxQuantenion = 360;
	private const float Half = 0.5f;

	[SerializeField] private PooledObject _prefab;
	[SerializeField][Min(0)] private int _maxPool = 10;
	[SerializeField][Min(0)] private int _defaultCapacity = 5;

	[Header("Timer")]
	[SerializeField][Min(0)] private float _spawnTime = 5;

	private ObjectPool<PooledObject> _pool;

	private BoxCollider _boxCollider;

	private void Awake()
	{
		_boxCollider = GetComponent<BoxCollider>();

		_pool = new ObjectPool<PooledObject>(
			createFunc: CreatePooledItem,
			actionOnGet: OnTakeFromPool,
			actionOnRelease: OnReturnedToPool,
			actionOnDestroy: OnDestroyPoolObject,
			collectionCheck: true,
			defaultCapacity: _defaultCapacity,
			maxSize: _maxPool
		);
	}

	private void Start()
	{
		StartCoroutine(LoopSpawn());
	}

	private PooledObject CreatePooledItem()
	{
		PooledObject instance = Instantiate(_prefab);
		instance.Returned += () => _pool.Release(instance);
		instance.gameObject.SetActive(false);

		return instance;
	}

	private void OnTakeFromPool(PooledObject pooledObject)
	{
		pooledObject.gameObject.SetActive(true);
		pooledObject.transform.position = transform.position;
	}

	private void OnReturnedToPool(PooledObject pooledObject)
	{
		pooledObject.gameObject.SetActive(false);
		pooledObject.transform.SetParent(null);
	}

	private void OnDestroyPoolObject(PooledObject pooledObject)
	{
		Destroy(pooledObject.gameObject);
	}

	private IEnumerator LoopSpawn()
	{
		var waitFreeSpace = new WaitUntil(() => _pool.CountActive < _maxPool);
		var delaySpawn = new WaitForSeconds(_spawnTime);

		bool isWork = true;

		while (isWork)
		{
			yield return waitFreeSpace;
			yield return delaySpawn;

			PooledObject pooled = _pool.Get();
			pooled.transform.position = RandomPosition();
			pooled.transform.rotation = RandomRotate();
		}
	}

	private Vector3 RandomPosition()
	{
		Vector3 center = _boxCollider.center;
		Vector3 size = _boxCollider.size;

		Vector3 randomLocalPoint = new Vector3(
			(Random.value - Half) * size.x,
			(Random.value - Half) * size.y,
			(Random.value - Half) * size.z
		);

		Vector3 worldPoint = transform.TransformPoint(center + randomLocalPoint);

		return worldPoint;
	}

	private Quaternion RandomRotate()
	{
		float randomX = Random.Range(0f, MaxQuantenion);
		float randomY = Random.Range(0f, MaxQuantenion);
		float randomZ = Random.Range(0f, MaxQuantenion);

		return Quaternion.Euler(randomX, randomY, randomZ);
	}
}