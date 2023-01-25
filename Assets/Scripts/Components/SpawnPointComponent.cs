using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnPoint
{
    Vector3 GetPosition();

    Quaternion GetRotation();
}

public class SpawnPointComponent : MonoBehaviour, ISpawnPoint
{
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }
}
