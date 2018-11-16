using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generador3d : MonoBehaviour {
	public GameObject man0;
	public GameObject man1;

	public GameObject hexPrefab;
	public GameObject hex1;
	public GameObject hex2;
	public GameObject hex3;
	public GameObject hex4;
	public GameObject hex5;

	public int[,] alturas = new int[10,10];
	public GameObject[,] hexMap = new GameObject[10,10];

	int mapWidth = 10;
	int mapHeight = 10;
	float x = 0f;
	float y = 0f;

	void Start () {
		createHexMap();
	}

	void createHexMap()
	{
		for (int j=0; j< mapWidth; j++) {
			for (int i = 0; i< mapHeight; i++) {
				GameObject Temp = Instantiate(color((i+j)%5));
				hexMap[i,j] = Temp;
				alturas[i,j] = 0;
				float tempy = y;
				if(i+j>9 && i>0) {
					tempy = y+0.457f;
					alturas[i,j] = 1;
				}
				Temp.transform.position = new Vector3(x,tempy,0);
				Temp.GetComponent<Renderer>().sortingOrder = mapWidth-j;
				x = x + (float)1.29f;


			}
			if(j%2 == 0) x = 0.645f;
			else x = 0f;
			y = y+1;
		}

		GameObject TempMan0 = Instantiate(man0);
		GameObject TempMan1 = Instantiate(man1);
		GetComponent<Units>().moveUnitTo(TempMan0,new Vector3 (1,3,0));
		GetComponent<Units>().moveUnitTo(TempMan1,new Vector3 (2,4,0));

	}

	GameObject color(int n){
		switch (n) {
			case 0 : return hex4; break;
			case 1 : return hex4; break;
			case 2 : return hex3; break;
			case 3 : return hex3; break;
			case 4 : return hex3; break;
			default : return hex5;
		}
	}

}
