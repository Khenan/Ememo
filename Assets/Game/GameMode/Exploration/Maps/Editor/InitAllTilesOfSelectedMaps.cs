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
        if (_asset is GameObject)
        {
            GameObject _gameObject = _asset as GameObject;
            if (_gameObject != null)
            {
                Map _map = _gameObject.GetComponent<Map>();
                if (_map != null)
                {
                    _map.InitAll();
                    EditorUtility.SetDirty(_map);
                }
            }
        }
    }
}
