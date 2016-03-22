using UnityEngine;
using System.Collections;

// A class to wrap the requirements of a targettable object.
public interface ITargettable {
    bool CanBeTargetted(Animal animal);
    void Targetted(Animal animal);
    void Untargetted(Animal animal);
    Vector3 GetPosition();
    bool IsAlive();
}
