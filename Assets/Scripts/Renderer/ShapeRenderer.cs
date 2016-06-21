using UnityEngine;

public abstract class ShapeRenderer : MonoBehaviour {

    protected bool propertyObjectChanged;

    /*
     * Update Methods
     */

    void Update() {
    //    Debug.Log("update " + this);
        RenderAndUpdatePropertyIfNeeded();
    }

    void RenderAndUpdatePropertyIfNeeded() {
        if (propertyObjectChanged) {
            Debug.Log("property modified");
            UpdateGameObject();

            UpdateMeshIfNeeded();

            CacheProperty();
            propertyObjectChanged = false;
        }
        /* TODO enable only in editor mode
        else if (GameObjectWasModified()) {
            // update property
        }
        */
    }

    /*
     * Needs to be implemented in children
     */

    protected abstract void UpdateGameObject();

    protected abstract void UpdateMeshIfNeeded();

    protected abstract void CacheProperty();

    /*
     * Rerender criteria
     */

    protected abstract bool GameObjectWasModified();

    /*
     * Getters, Setters
     */
    protected abstract ShapeProperty GameObjectToShapeProperty();

    /*
    public abstract ShapeProperty property {
        get; set;
    }
    */

    /*
    public ShapeProperty property {
        get {
            return _property;
        }
        set {
            propertyObjectChanged = true;
            _property = value;
        }
    }
    */
}
