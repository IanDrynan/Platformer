using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    public GameManager gm;

	// Use this for initialization
	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gm.stopGame();
            gameObject.SetActive(false);
        }
    }
}
