using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generador3d : MonoBehaviour {
	public GameObject man0;
	public GameObject man1;

	public GameObject basePrefab;
	public GameObject goldPf;

	public GameObject hex1;
	public GameObject hex2;
	public GameObject hex3;
	public GameObject hex4;
	public GameObject hex5;

	public const float oddOffset = 0.645f; //Desfasaje de las lineas impares.
	public const float hexHight = 0.95f; //Altura de los Hex.
	public const float hexWidth = 1.29f; //Ancho de los Hex.
	public const float hightOffset = 0.457f; //Desfasaje de las elevaciones.

	const int mapWidth = 10;
	const int mapHight = 10;

	public int[,] alturas = new int[mapWidth,mapHight]; //Arreglo con las alturas.
	public GameObject[,] hexMap = new GameObject[mapWidth,mapHight]; // Arreglo con los Hex.
	public int[,] recursos = new int [mapWidth,mapHight]; //Arreglo con los recursos

	float x = 0f;
	float y = 0f;
	bool CheckMoved;

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
		CheckMoved = GetComponent<Units>().moveUnitTo(TempMan0,new Vector3 (1,3,0),new Vector3(0,0,0));
		CheckMoved = GetComponent<Units>().moveUnitTo(TempMan1,new Vector3 (2,4,0),new Vector3(0,0,0));

	}

	void createBases(){
		System.Random rnd = new System.Random();
		for(int i=0;i<5;i++){
			int x = rnd.Next(1,10);
			int y = rnd.Next(1,10);
			GameObject base1 = Instantiate(basePrefab);
			CheckMoved = GetComponent<Units>().moveUnitTo(base1,new Vector3 (x,y,0),new Vector3 (0,0,0));
			if(!CheckMoved) i-=1;
		}
		GameObject gold = Instantiate(goldPf);
		CheckMoved = GetComponent<Units>().moveUnitTo(gold,new Vector3 (4,4,0),new Vector3 (0,0,0));
		gold.transform.Translate(new Vector3 (0,-0.3f,0));
		recursos[4,4] = 1;
		for(int i=0;i<3;i++){
			int x = rnd.Next(1,10);
			int y = rnd.Next(1,10);
			gold = Instantiate(goldPf);
			CheckMoved = GetComponent<Units>().moveUnitTo(gold,new Vector3 (x,y,0),new Vector3 (0,0,0));
			if(CheckMoved){
				gold.transform.Translate(new Vector3 (0,-0.3f,0));
				recursos[x,y] = 1;
			}
		}
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
