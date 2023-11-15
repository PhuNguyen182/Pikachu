using UnityEngine;

public class SimpleContainer : MonoBehaviour
{ }

public class SimpleContainer<T> : SimpleContainer where T : SimpleContainer
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject(typeof(T).Name).AddComponent<T>();

            return _instance;
        }
    }

    public static Transform InstanceTransform => Instance.transform;
}

public class CreatureCellContainer : SimpleContainer<CreatureCellContainer> { }

public class EffectContainer : SimpleContainer<EffectContainer> { }
