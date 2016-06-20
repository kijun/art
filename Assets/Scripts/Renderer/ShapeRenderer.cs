using UnityEngine;

public abstract class ShapeRenderer : MonoBehaviour {

    private bool propertyObjectChanged;
    private ShapeProperty property;
    private ShapeProperty cachedProperty;

    /*
     * Update Methods
     */

    void Update() {
        RenderAndUpdatePropertyIfNeeded();
    }

    void RenderAndUpdatePropertyIfNeeded() {
        if (PropertyWasModified()) {
            UpdateGameObject();

            UpdateMeshIfNeeded();

            CacheProperty();
        }
        /* TODO enable only in editor mode
        else if (GameObjectWasModified()) {
            // update property
        }
        */
    }

    protected abstract void UpdateGameObject();

    protected abstract void UpdateMeshIfNeeded();

    protected void CacheProperty() {
        cachedProperty = property;
        // TODO should encapsulate
        propertyObjectChanged = false;
    }

    /*
     * Rerender criteria
     */

    protected bool PropertyWasModified() {
        return propertyObjectChanged && !property.Equals(cachedProperty);
    }

    protected abstract bool GameObjectWasModified();

    /*
     * Getters, Setters
     */
    protected abstract ShapeProperty GameObjectToShapeProperty();

    public ShapeProperty Property {
        get {
            return property;
        }
        set {
            propertyObjectChanged = true;
            property = value;
        }
    }
}
