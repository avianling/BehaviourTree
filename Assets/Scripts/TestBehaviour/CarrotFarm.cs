using UnityEngine;
using System.Collections;

public class CarrotFarm : MonoBehaviour {

    public float size = 5f;

    public float frequency = 1f;

    IEnumerator Start()
    {
        while ( true )
        {
            CreateCarrot();
            yield return new WaitForSeconds(1f / frequency);
        }
    }

    void CreateCarrot()
    {
        Vector3 position = new Vector3(Random.Range(-size, size), 0, Random.Range(-size, size));
        Carrot carrot = AnimalFactory.Instance.CreateCarrot();
        carrot.transform.parent = transform;
        carrot.transform.localPosition = position;
    }

}
