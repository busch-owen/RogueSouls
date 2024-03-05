using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    Dictionary<string, Stack<PoolObject>> stackDictionary = new Dictionary<string, Stack<PoolObject>>();
    // Start is called before the first frame update
    void Start()
    {
        PoolManager.Instance.Load();
    }

    private void Load()
    {
        PoolObject[] poolObjects = Resources.LoadAll<PoolObject>("PoolObjects");
        foreach(PoolObject poolObject in poolObjects)
        {
            Stack<PoolObject> objStack = new Stack<PoolObject>();
            objStack.Push(poolObject); //in a stack, we push something inside of it, and we pop it out
            stackDictionary.Add(poolObject.name, objStack); //the reason we are doing this is so that we know which stack to grab our particular object from
        }
    }

    public PoolObject Spawn(string name)
    {
        //first thing we need to do is reference the correct stack
        Stack<PoolObject> objStack = stackDictionary[name]; //grab the correct object
        //we have two situations when we are spawning an item: 1) we have only one item left, and if that's the case, we will instantiate a new object (this is to make sure that we still have one left all of the time)
        //situation 2: we have more than one object, so we simply pop it out and hand it to back to whomever made the request
        if(objStack.Count == 1) //check the number of items
        {
            PoolObject poolObject = objStack.Peek();
            PoolObject objectClone = Instantiate(poolObject);
            objectClone.name = poolObject.name;
            return objectClone;
        }
        PoolObject oldPoolObject = objStack.Pop();
        oldPoolObject.gameObject.SetActive(true);//when we despawn things, we make them inactive
        return oldPoolObject;
    }
    public void DeSpawn(PoolObject poolObject)
    {
        Stack<PoolObject> objStack = stackDictionary[poolObject.name];
        //disable that object and put in the stack
        poolObject.gameObject.SetActive(false);
        objStack.Push(poolObject);
    }
}
