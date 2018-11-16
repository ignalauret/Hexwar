using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generador3d : MonoBehaviour {
	public GameObject man0;
	public GameObject man1;

	public GameObject hex1;
	public GameObject hex2;
	public GameObject hex3;
	public GameObject hex4;
	public GameObject hex5;

	public const float oddOffset = 0.645f; //Desfasaje de las lineas impares.
	public const float hexHight = 1f; //Altura de los Hex.
	public const float hexWidth = 1.29f; //Ancho de los Hex.
	public const float hightOffset = 0.457f; //Desfasaje de las elevaciones.

	const int mapWidth = 10;
	const int mapHight = 10;

	public int[,] alturas = new int[mapWidth,mapHight]; //Arreglo con las alturas.
	public GameObject[,] hexMap = new GameObject[mapWidth,mapHight]; // Arreglo con los Hex.

	float x = 0f;
	float y = 0f;

	void Start () {
		createHexMap();
		createBases();
	}

//Funcion que crea el mapa a partir del ancho y el alto
	void createHexMap()
	{
		for (int j=0; j< mapWidth; j++) {
			for (int i = 0; i< mapHight; i++) {
				//Elijo el color del Hex con una funcion "Random" por ahora;
				GameObject Temp = Instantiate(color((i+j)%5));
				hexMap[i,j] = Temp;
				alturas[i,j] = 0;
				float tempy = y;
				//En este if deberia preguntar si el generador dijo si alturas[i,j]=1, cuando haya generador...
				if(i+j>9 && i>0) {
					tempy = y+hightOffset;
					alturas[i,j] = 1;
				}

				Temp.transform.position = new Vector3(x,tempy,0);
				//Mientras mas arriba, menos sortingOrder asi no se tapan
				Temp.GetComponent<Renderer>().sortingOrder = mapWidth-j;
				x = x + hexWidth;


			}
			if(j%2 == 0) x = oddOffset;
			else x = 0f;
			y = y+hexHight;
		}
//Creo algunos soldados para probar (Temporal).
		GameObject TempMan0 = Instantiate(man0);
		GameObject TempMan1 = Instantiate(man1);
		GetComponent<Units>().moveUnitTo(TempMan0,new Vector3 (1,3,0));
		GetComponent<Units>().moveUnitTo(TempMan1,new Vector3 (2,4,0));

	}

	void createBases(){
		Random rnd = new Random();
		Debug.Log(rnd);
	}

//Funcion para elegir el color de los Hex.
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
