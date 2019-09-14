using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapTransition : MonoBehaviour {
    [SerializeField]
    private float offset;
    [SerializeField]
    private MapRow[] MapGrid;
    [SerializeField]
    private int Row, Col;
    [SerializeField]
    private BoxCollider2D NorthBound, SouthBound, EastBound, WestBound;

    public void TransitionHorizontal(bool eastward) {
        //keep the same y but the x position is the other side of the map
        PersistentStats.SceneChangePosY = gameObject.transform.position.y;
        PersistentStats.SceneChangePosX = eastward ? (-(1600 - offset)) : (1600 - offset);
        /*if (eastward) {
            PersistentStats.SceneChangePosX = -(1600 - offset);
        } else {
            PersistentStats.SceneChangePosX = 1600 - offset;
        }*/
        PersistentStats.SceneChanged = true;

        int newCol = Col + (eastward ? 1 : -1);
        try {
            Debug.Log(MapGrid[Row].Maps[newCol]);
            SceneManager.LoadScene(MapGrid[Row].Maps[newCol]);
            //transform.position = new Vector3(0 - gameObject.transform.position.x, gameObject.transform.position.y, 0);
        }catch (IndexOutOfRangeException e) {

        }catch(NullReferenceException e) {

        }
    }

    public void TransitionVertical(bool southward) {
        //x stays the same and y inverts
        PersistentStats.SceneChangePosX = gameObject.transform.position.x;
        PersistentStats.SceneChangePosX = southward ? (1600 - offset) : (-(1600 - offset));
        PersistentStats.SceneChanged = true;

        int newRow = Row + (southward ? 1 : -1);
        try {
            Debug.Log(MapGrid[Row].Maps[newRow]);
            SceneManager.LoadScene(MapGrid[newRow].Maps[Col]);
            //transform.position = new Vector3(gameObject.transform.position.x, 0 -gameObject.transform.position.y, 0);
        } catch (IndexOutOfRangeException e) {

        } catch (NullReferenceException e) {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
    
        try {
            if (collision.Equals(NorthBound))
                TransitionVertical(false);
            if (collision.Equals(SouthBound))
                TransitionVertical(true);
            if (collision.Equals(EastBound))
                TransitionHorizontal(true);
            if (collision.Equals(WestBound))
                TransitionHorizontal(false);
        } catch(NullReferenceException e) {

        }
    }

    [System.Serializable]
    public class MapRow {
        public String[] Maps;
    }
}
