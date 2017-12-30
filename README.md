# Object-Pooler
A simple object pooler for Unity game engine.

Features (For now):
* Create Pool
* Expand Pool
* Reuse Object

When reusing the object you can implement the IPoolable interface and specify what needs to be reset in ResetPoolObject() method. For example this will set the scale to Vector3.one (1,1,1):

```csharp
using UnityEngine;

public class MyClass : MonoBehaviour, IPoolable
{
  public void ResetPoolObject()
  {
    transform.localScale = Vector3.one;
  }
}
```

Note that this system uses object's InstanceID as the dictionary key, therefore when reusing/expanding you must pass in the same GameObject (prefab) which was used to create the pool.
