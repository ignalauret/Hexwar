using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour {

	public GameObject hitObject;
	public GameObject hitObject2;
	public GameObject man;

	Vector3 pos;
	Vector3 unitPos;
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
					unitPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				}

				//Si habia alguien seleccionado veo que hacer
				else if( hitObject.GetComponent<UnitInfo>().moved == false){
					hitObject2 = hitInfo.transform.root.gameObject;

					//Si son de distinto equipo
					if(hitObject.GetComponent<UnitInfo>().team != hitObject2.GetComponent<UnitInfo>().team){

						//Si tiene mas ataque
						if(hitObject.GetComponent<UnitInfo>().atack > hitObject2.GetComponent<UnitInfo>().atack){
							Destroy(hitObject2); //lo mato
							hitObject2 = null;
							pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
							moveUnitTo(hitObject,pos);
							hitObject.GetComponent<UnitInfo>().moved = true;
							hitObject = null;

						}

						//Si tenia menos ataque
						else {
							Debug.Log("Muy Debil");
							hitObject2 = null;
							hitObject = null;
						}
					}

					//Si son del mismo equipo
					else {
						Debug.Log("Mismo Equipo!");
						hitObject2 = null;
						hitObject = null;
					}
				} else hitObject = null;

				//Si esta vacia
			} else if(hitObject != null && hitObject.GetComponent<UnitInfo>().moved == false){
				//si se puede mover, lo muevo
				pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 tempos = getHexCoord(pos);
				Vector3 tempos2 = getHexCoord(unitPos);
				int deltax = Abs((int)tempos[0]-(int)tempos2[0]);
				int deltay = Abs((int)tempos[1]-(int)tempos2[1]);
				int speed = hitObject.GetComponent<UnitInfo>().speed;
				if(deltax <= speed && deltay <= speed && deltax + deltay < 2*speed ){
					//veo si esta dentro de su rango de movimiento
					moveUnitTo(hitObject,pos);
					hitObject.GetComponent<UnitInfo>().moved = true;
					hitObject = null;
				} else { //si no esta en su rango
					Debug.Log("Muy Lejos");
					hitObject = null;
				}
			}
		}

		//Boton derecho = crear unidad
		if(Input.GetMouseButtonDown(0)){
		  pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 coord = getHexCoord(pos);
			Debug.Log(coord);
      GameObject Temp2 = Instantiate(man);
			moveUnitTo(Temp2,pos);
			Temp2.GetComponent<Renderer>().sortingOrder = 20;
		}
	}

public void moveUnitTo(GameObject unit, Vector3 position){

	Vector3 hexPos = new Vector3(0,0,0);

	if((int)position[1]%2==1)position[0]-=0.645f;
	position[0]=(int)((position[0]+0.645f)/1.29f);
	hexPos[0] = position[0]*1.29f;
	if((int)position[1]%2==1)hexPos[0]+=0.645f;
	hexPos[1] = (int)position[1]+0.50f;
	if(GetComponent<generador3d>().alturas[(int)position[0],(int)position[1]] == 1)hexPos[1]+=0.457f;

	unit.transform.position = hexPos;

}

public Vector3 getHexCoord(Vector3 coord){
	if((int)coord[1]%2==1)coord[0]-=0.645f;
	coord[0]=(int)((coord[0]+0.645f)/1.29f);
	coord[1]=(int)coord[1];
	coord[2]=0f;
	return coord;

}
public int Abs(int n){
	if(n>0)return n;
	else return (-1)*n;
}
}
