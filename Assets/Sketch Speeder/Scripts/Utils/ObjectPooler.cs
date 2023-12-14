using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler<T> where T: Component
{
    private GameObject _prefab;
    private Transform _parent;
    private List<T> _pool;

    public ObjectPooler(GameObject prefab, Transform parent){
        _prefab = prefab;
        _parent = parent;

        _pool = new List<T>();

        GameObject obj = Object.Instantiate(prefab, parent);
        obj.SetActive(false);
        _pool.Add(obj.GetComponent<T>());
    }

    public T GetPooledObject(){
        foreach(T obj in _pool)
        {
            if(!obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        // If we've reached here, it means all lines in the pool are in use. So, increase pool size.
        GameObject newObj = Object.Instantiate(_prefab, _parent);
        T newObjComponent = newObj.GetComponent<T>();
        _pool.Add(newObjComponent);
        newObj.SetActive(true);
        return newObjComponent;
    }

    public void ClearPool()
    {
        foreach (var obj in _pool)
        {
            obj.gameObject.SetActive(false);
        }
    }
}