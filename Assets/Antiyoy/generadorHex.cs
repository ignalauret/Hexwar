using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generadorHex : MonoBehaviour {

	public GameObject hexPrefab;
	public GameObject hex1;
	public GameObject hex2;
	public GameObject hex3;
	public GameObject hex4;
	public GameObject hex5;

	int mapWidth = 10;
	int mapHeight = 7;
	float x = 0f;
	float y = 0f;
	void Start () {
		createHexMap();
	}

	void createHexMap()
	{
		for (int j=0; j<= mapWidth; j++) {
			for (int i = 0; i<= mapHeight; i++) {
				GameObject Temp = Instantiate(color((i+j)%5));
				Temp.transform.position = new Vector3(x,y,0);
				y = y + (float)2.57;

			}
			if(j%2 == 0) y = 1.26f;
			else y = 0f;
			x = x+2.22f;
		}
	}

	GameObject color(int n){
		switch (n) {
			case 0 : return hex1; break;
			case 1 : return hex2; break;
			case 2 : return hex3; break;
			case 3 : return hex4; break;
			case 4 : return hex5; break;
			default : return hex5;
		}
	}

}
