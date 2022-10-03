using UnityEngine;

public class EntityDiedEvent
{
    public GameObject DiedObject { get; private set; }

    public EntityDiedEvent(GameObject gameObject) => DiedObject = gameObject;
}
