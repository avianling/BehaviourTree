using UnityEngine;
using System.Collections;

public interface IUtilityBehaviour : IBehaviour {
    // Determine the utility of this option.
    float Utility();
}
