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

    public void RenderAndUpdatePropertyIfNeeded() {
        if (propertyObjectChanged) {
            UpdateGameObject();

            UpdateMeshIfNeeded();

            CacheProperty();
            propertyObjectChanged = false;
        } else if (Application.isEditor && GameObjectWasModified()) {
            SetProperty(GameObjectToShapeProperty());
            UpdateGameObject();

            UpdateMeshIfNeeded();

            CacheProperty();
            propertyObjectChanged = false;
        }
    }

    /*
     * Needs to be implemented in children
     */

    protected abstract void UpdateGameObject();

    protected abstract void UpdateMeshIfNeeded();

    protected abstract void CacheProperty();

    protected abstract void SetProperty(ShapeProperty property);

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
