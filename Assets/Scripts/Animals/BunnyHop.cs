using UnityEngine;
using System.Collections;

public class BunnyHop : MonoBehaviour {

    public AnimationCurve hop;

    private float time;
    private float speedModifier;

    void Start()
    {
        time = Random.Range(0f, 5f);
        speedModifier = Random.Range(0.8f, 1.2f);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 position = transform.position;
        position.y = hop.Evaluate(time);
        transform.position = position;
        time += Time.deltaTime * speedModifier;
    }
}
