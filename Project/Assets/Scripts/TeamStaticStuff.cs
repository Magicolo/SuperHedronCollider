using UnityEngine;
using System.Collections;

public class TeamStaticStuff  {

	public static Color getColorForTeam(int playerId) {
		switch(playerId){
				case 0 : return Color.cyan;
				case 1 : return Color.magenta;
				case 2 : return Color.yellow;
				default : return Color.white;
		}
	}
}
