using UnityEngine;
using UnityEditor;

public class InitAllTilesOfSelectedMaps : MonoBehaviour
{
    // Add the method to the context menu for selected assets in the Project window
    [MenuItem("Assets/Apply Tool to Selected Assets", false, 10)]  // This makes the menu appear when you right-click on assets
    static void ApplyToolToSelectedAssets()
    {
        // Get all selected assets in the Project window
        Object[] _selectedAssets = Selection.objects;

        if (_selectedAssets.Length == 0)
        {
            Debug.LogWarning("No assets selected!");
            return;
        }

        foreach (Object _asset in _selectedAssets)
        {
            ApplyMethod(_asset);
        }
    }

    // Validates whether the menu item should be enabled
    [MenuItem("Assets/Apply Tool to Selected Assets", true)]
    static bool ValidateApplyToolToSelectedAssets()
    {
        // Ensure that at least one asset is selected
        return Selection.objects.Length > 0;
    }

    // Method to apply on each asset
    static void ApplyMethod(Object _asset)
    {
        Debug.Log("Applying method to asset: " + _asset.name);

        // Example: If the asset is a GameObject (like a prefab), do something specific
        if (_asset is GameObject)
        {
            GameObject _gameObject = _asset as GameObject;
            if (_gameObject != null)
            {
                Debug.Log("_asset is GameObject");
                Map _map = _gameObject.GetComponent<Map>();
                if (_map != null)
                {
                    Debug.Log("_asset is Map");
                    _map.InitAll();
                    EditorUtility.SetDirty(_map);
                }
            }
        }
    }
}
