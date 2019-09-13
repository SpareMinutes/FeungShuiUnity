using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapTransition : MonoBehaviour {
    [SerializeField]
    private MapRow[] MapGrid;
    [SerializeField]
    private int Row, Col;
    [SerializeField]
    private BoxCollider2D NorthBound, SouthBound, EastBound, WestBound;

    public void TransitionHorizontal(bool eastward) {
        int newCol = Col + (eastward ? 1 : -1);
        try {
            Debug.Log(MapGrid[Row].Maps[newCol]);
            SceneManager.LoadScene(MapGrid[Row].Maps[newCol]);
            transform.position = new Vector3(0 - gameObject.transform.position.x, gameObject.transform.position.y, 0);
        }catch (IndexOutOfRangeException e) {

        }catch(NullReferenceException e) {

        }
    }

    public void TransitionVertical(bool southward) {
        int newRow = Row + (southward ? 1 : -1);
        try {
            Debug.Log(MapGrid[Row].Maps[newRow]);
            SceneManager.LoadScene(MapGrid[newRow].Maps[Col]);
            transform.position = new Vector3(gameObject.transform.position.x, 0 -gameObject.transform.position.y, 0);
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
