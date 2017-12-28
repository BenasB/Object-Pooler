# Object-Pooler
A simple object pooler for Unity game engine.

Features (For now):
* Create Pool
* Expand Pool
* Reuse Object

Note that this system uses object's InstanceID as the dictionary key, therefore when reusing/expanding you must pass in the same GameObject (prefab) which was used to create the pool.
