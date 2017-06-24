using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class FunctionsHelper : NetworkBehaviour {

    public static GameObject GetNearestTargetWithTag(string tag, Vector3 searchingPosition)
    {
            return GameObject.FindGameObjectsWithTag(tag)
                .OrderBy(o => (o.transform.position - searchingPosition).sqrMagnitude)
                .FirstOrDefault();
    }

    public static UnitHealth GetNearestUnit(Vector3 searchingPosition, UnitHealth searchingUnit)
    {
        UnitHealth[] units = GameObject.FindObjectsOfType<UnitHealth>();

        // UnitHealth units = GameObject.FindObjectsOfType<UnitHealth>()

        units.OrderBy(o => (o.transform.position - searchingPosition).sqrMagnitude)
            .FirstOrDefault();

        return units[1];

    }


    public static Vector3 GetCursorPosition(int playerNumber)
    {
            Ray inputRay = GameManager.instance.players[playerNumber].playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                return hit.point;
            }
            else
            {
                Debug.Log("Raycast ain't hittinh shit");
                return Vector3.up;
            }

    }

    public static void CreateObject(GameObject obj, Vector3 position)
    {
        Instantiate(obj, position, Quaternion.identity);
    }

    public static void CreateAndDestroy(GameObject obj, Vector3 position, float time)
    {
        GameObject _obj = (GameObject)Instantiate(obj, position, Quaternion.identity);
        Destroy(_obj, time);
    }

    public static void AddPlayerTagToObject(Player player, GameObject obj)
    {
        obj.tag = GameManager.instance.playerTags[player.playerNumber];
    }

}
