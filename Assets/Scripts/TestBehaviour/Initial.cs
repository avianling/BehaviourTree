using UnityEngine;
using System.Collections;

public class Initial : MonoBehaviour {

    public int numBunnies;
    public int numFoxes;
    public int numShepards;

	// Use this for initialization
	IEnumerator Start () {
	    for ( int i=0; i < numBunnies; i++ )
        {
            AnimalFactory.Instance.CreateAnimal(AnimalTypes.Bunny).transform.position = transform.position;
            yield return new WaitForSeconds(0.2f);
        }
        yield return 0f;

        for (int i = 0; i < numFoxes; i++)
        {
            AnimalFactory.Instance.CreateAnimal(AnimalTypes.Fox).transform.position = transform.position;
            yield return new WaitForSeconds(1f);
        }
        yield return 0f;

        for (int i = 0; i < numShepards; i++)
        {
            AnimalFactory.Instance.CreateAnimal(AnimalTypes.Shepard).transform.position = transform.position;
            yield return new WaitForSeconds(1f);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
