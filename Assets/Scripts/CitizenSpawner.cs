using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour
{

    public GameObject citizen, zeroPatient;
    public int side;
    public float distance, altitude, force, freezeProbability;

    void Start()
    {   
        if (side > 0 && distance > 0)
        {
            int random = (int)Random.Range(1, side * side),
                zeroPatientZ = random / side, zeroPatientX = random % side;
            for (int z = 0; z < side; ++z)
                for (int x = 0; x < side; ++x)
                    if (z != 0 || x != 0)
                    {
                        Rigidbody rb = Instantiate(z == zeroPatientZ && x == zeroPatientX ? zeroPatient : citizen,
                                                    new Vector3(distance * x, altitude, distance * z),
                                                    Quaternion.identity,
                                                    transform).GetComponent<Rigidbody>();
                        if (Random.value > freezeProbability)
                            rb.AddForce(getRandomForce());
                        else
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                    }
        }
    }

    // inside force units circle contained in the xz plan
    private Vector3 getRandomForce()
    {
        Vector2 random = Random.insideUnitCircle;
        return force * new Vector3(random.x, 0, random.y);
    }
}
