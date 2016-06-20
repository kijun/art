using UnityEngine;

public abstract class ShapeRenderer : MonoBehaviour {

    private bool propertyObjectChanged;
    private IShapeProperty property;
    private IShapeProperty cachedProperty;

    /*
     * Update Methods
     */

    void Update() {
        RenderAndUpdatePropertyIfNeeded();
    }

    void RenderAndUpdatePropertyIfNeeded() {
        if (PropertyWasModified()) {
            UpdateGameObject();

            if (MeshNeedsUpdate()) {
                GenerateMesh();
            }

            CacheProperty();
        }
        /* TODO enable only in editor mode
        else if (GameObjectWasModified()) {
            // update property
        }
        */
    }

    protected abstract void UpdateGameObject();

    protected abstract void GenerateMesh();

    protected void CacheProperty() {
        cachedProperty = property;
    }

    /*
     * Rerender criteria
     */

    protected bool PropertyWasModified() {
        return !property.Equals(cachedProperty);
    }

    protected abstract bool MeshNeedsUpdate();

    protected abstract bool GameObjectWasModified();

    /* Getters, Setters */
    protected abstract IShapeProperty GameObjectToShapeProperty();

    public IShapeProperty Property {
        get {
            return property;
        }
        set {
            propertyObjectChanged = true;
            property = value;
        }
    }
}
