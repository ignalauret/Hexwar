using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour {

	public GameObject hitObject;
	public GameObject hitObject2;
	public GameObject man;

	public const float oddOffset = 0.645f;
	public const float hexHight = 0.95f;
	public const float hexWidth = 1.29f;
	public const float hightOffset = 0.457f;

	Vector3 pos;
	Vector3 unitPos;
	Vector3 unitPos2;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	 void Update () {
		 //Boton Izquierdo = seleccionar y mover
		if(Input.GetMouseButtonDown(1)){

			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition);
			RaycastHit hitInfo;

			//Si le doy a una unidad
			if(Physics.Raycast( ray, out hitInfo)) {

				//Si no habia alguien seleccionado lo selecciono
				if(hitObject == null){
					hitObject = hitInfo.transform.root.gameObject;

					if(hitObject.GetComponent<UnitInfo>().team == GetComponent<TurnController>().playerTurn){
						unitPos = getHexCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition));
					} else {
						hitObject = null;
						Debug.Log("No es tu turno");
					}

				}

				//Si habia alguien seleccionado veo que hacer
				else if( hitObject.GetComponent<UnitInfo>().moved == false){
					hitObject2 = hitInfo.transform.root.gameObject;
					unitPos2 = getHexCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition));

					//Si son de distinto equipo
					if(hitObject.GetComponent<UnitInfo>().team != hitObject2.GetComponent<UnitInfo>().team){

							atacar(hitObject,unitPos,hitObject2,unitPos2,false);
							hitObject = null;
							hitObject2 = null;
					}

					//Si son del mismo equipo
					else {
						Debug.Log("Mismo Equipo!");
						hitObject2 = null;
						hitObject = null;
					}
				//Si ya se movio, no hago nada
				} else hitObject = null;

				//Si esta vacia
			} else if(hitObject != null && hitObject.GetComponent<UnitInfo>().moved == false){
				//Si se puede mover, lo muevo
				pos = getHexCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition));

				int deltax = Abs((int)pos[0]-(int)unitPos[0]);
				int deltay = Abs((int)pos[1]-(int)unitPos[1]);
				int speed = hitObject.GetComponent<UnitInfo>().speed;
				if(deltax <= speed && deltay <= speed && deltax + deltay < 2*speed ){
					//Veo si esta dentro de su rango de movimiento
					moveUnitTo(hitObject,pos,unitPos);
					hitObject.GetComponent<UnitInfo>().moved = true;
					//Pongo el hexagono del equipo del soldado
					GameObject hex = GetComponent<generador3d>().hexMap[(int)pos[0],(int)pos[1]];
					hex.GetComponent<HexInfo>().team = hitObject.GetComponent<UnitInfo>().team;
					hitObject = null;
					hex = null;
				} else { //si no esta en su rango
					Debug.Log("Muy Lejos");
					hitObject = null;
				}
			}
		}

		//Boton derecho = crear unidad
		if(Input.GetMouseButtonDown(0)){
		  pos = getHexCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition));
      GameObject Temp2 = Instantiate(man);
			moveUnitTo(Temp2,pos,new Vector3 (0,0,0));
			Temp2.GetComponent<Renderer>().sortingOrder = 20;
		}
	}

public void moveUnitTo(GameObject unit, Vector3 objectivePosition, Vector3 actualPosition){


	GameObject hex = GetComponent<generador3d>().hexMap[(int)objectivePosition[0],(int)objectivePosition[1]];
//Si no hay una unidad en ese Hex...
	if(hex.GetComponent<HexInfo>().isUnit == false){

		Vector3 hexPos = objectivePosition;

		hexPos[0] = objectivePosition[0]*hexWidth;
		if((int)hexPos[1]%2==1) hexPos[0]+=oddOffset;
		//Corrijo la altura si esta elevado
		if(GetComponent<generador3d>().alturas[(int)objectivePosition[0],(int)objectivePosition[1]] == 1) hexPos[1]+=hightOffset;
		//Hago desfasaje por la altura de la unidad
		hexPos[1] = hexPos[1]+0.40f;

		unit.transform.position = hexPos;
		hex.GetComponent<HexInfo>().isUnit = true;
		//Seteo que no hay unidad en el Hex donde estaba
		GameObject lastHex = GetComponent<generador3d>().hexMap[(int)actualPosition[0],(int)actualPosition[1]];
		lastHex.GetComponent<HexInfo>().isUnit = false;

	} else Destroy(unit);
}

//Funcion que le das un punto en la pantalla y te dice a que hexagono pertenece.
public Vector3 getHexCoord(Vector3 coord){
	if((int)coord[1]%2==1)coord[0]-=oddOffset;
	coord[0]=(int)((coord[0]+oddOffset)/hexWidth);
	coord[1]=(int)coord[1];
	coord[2]=0f;
	return coord;
}

//C# no tiene Abs...
public int Abs(int n){
	if(n>0)return n;
	else return (-1)*n;
}


void atacar(GameObject unit1, Vector3 Upos1, GameObject unit2, Vector3 Upos2, bool contrataque){

	if(contrataque){
		//Solo puede contratacar una vez por turno.
		unit1.GetComponent<UnitInfo>().contrataque = true;
	}

	int range = unit1.GetComponent<UnitInfo>().range;

	int deltax = Abs((int)Upos1[0]-(int)Upos2[0]);
	int deltay = Abs((int)Upos1[1]-(int)Upos2[1]);

	if(deltax <= range && deltay <= range && deltax + deltay < 2*range ){
		//Si alcanza al objetivo...
		int vida = unit2.GetComponent<UnitInfo>().life;
		int ataque = unit1.GetComponent<UnitInfo>().atack;
		vida = vida - ataque; //Hago el daño.
		Debug.Log(vida);

		if(vida <= 0){
			//Si murio...
			Destroy(unit2); //Lo mato y pongo su hex como vacio
			GameObject posHex = GetComponent<generador3d>().hexMap[(int)Upos2[0],(int)Upos2[1]];
			posHex.GetComponent<HexInfo>().isUnit = false;
			if(!contrataque){
				//Muevo la unidad atacante a su lugar
				moveUnitTo(unit1,Upos2,Upos1);
				unit1.GetComponent<UnitInfo>().moved = true;
			}

		} else {
			//Si no murio...
			unit2.GetComponent<UnitInfo>().life = vida;
			if(!unit2.GetComponent<UnitInfo>().contrataque){
				//Si todavia no contratacó, largo contrataque.
				atacar(unit2,Upos2,unit1,Upos1,true);
			}
		}

	}
}

}
