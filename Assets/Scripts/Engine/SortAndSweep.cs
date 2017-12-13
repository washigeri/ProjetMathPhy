using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal enum Axis
{
    X = 0,
    Y = 1,
    Z = 2
}

public class SortAndSweep
{
    internal const Axis SEARCH_AXIS = Axis.X;

    internal struct GameObjectsPair
    {
        internal GameObject gameObject1;
        internal GameObject gameObject2;

        internal CollisionInfo AreColliding()
        {
            CustomCollider collider1 = gameObject1.GetComponent<CustomCollider>();
            CustomCollider collider2 = gameObject2.GetComponent<CustomCollider>();
            return collider1.IsColliding(collider2);
        }
    }

    internal struct SearchNode
    {
        internal float startValue;
        internal float endValue;
        internal GameObject attachedGameobject;

        internal SearchNode(float startValue, float endValue, GameObject gameObject)
        {
            this.startValue = startValue;
            this.endValue = endValue;
            this.attachedGameobject = gameObject;
        }
    }

    private List<GameObject> gameObjects;

    public SortAndSweep(List<GameObject> gameObjects)
    {
        this.gameObjects = gameObjects;
    }

    internal List<GameObjectsPair> CheckForPossibleCollisions()
    {
        List<GameObjectsPair> result = new List<GameObjectsPair>();
        List<SearchNode> searchList = new List<SearchNode>();
        foreach (GameObject gameObject in gameObjects)
        {
            CustomCollider collider = gameObject.GetComponent<CustomCollider>();
            searchList.Add(new SearchNode(collider.GetMinXYZ((int)SEARCH_AXIS), collider.GetMaxXYZ((int)SEARCH_AXIS), gameObject));
        }
        searchList = searchList.OrderBy(o => o.startValue).ToList();
        for (int i = 0; i < searchList.Count; i++)
        {
            SearchNode searchNodeI = searchList[i];
            for (int j = i + 1; j < searchList.Count; j++)
            {
                SearchNode searchNodeJ = searchList[j];
                if (IsOverlapping(searchNodeI.startValue, searchNodeI.endValue, searchNodeJ.startValue, searchNodeJ.endValue))
                {
                    result.Add(new GameObjectsPair
                    {
                        gameObject1 = searchNodeI.attachedGameobject,
                        gameObject2 = searchNodeJ.attachedGameobject
                    });
                }
            }
        }
        return result;
    }

    private static bool IsOverlapping(float s1, float e1, float s2, float e2)
    {
        bool res = false;
        res = res || (s2 >= s1 && s2 <= e1);
        res = res || (e2 >= s1 && e2 <= e1);
        res = res || (s1 >= s2 && s1 <= e2);
        res = res || (e1 >= s2 && e1 <= e2);
        return res;
    }
}