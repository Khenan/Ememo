using System;
using UnityEditor;
using UnityEngine;

public class SpellEditor : EditorWindow
{
    private SpellData spellData;
    private string newSpellName = "New Spell";
    private int damage;
    private int healing;
    
    [MenuItem("Tools/Spell Editor")]
    public static void ShowWindow()
    {
        GetWindow<SpellEditor>("Spell Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        // Button to create a new SpellData
        newSpellName = EditorGUILayout.TextField("New Spell Name", newSpellName);
        if (GUILayout.Button("Create New Spell"))
        {
            CreateNewSpell();
        }
        
        spellData = (SpellData)EditorGUILayout.ObjectField("Spell Data", spellData, typeof(SpellData), false);
        
        if (spellData)
        {
            spellData.spellName = EditorGUILayout.TextField("Name", spellData.spellName);
            spellData.description = EditorGUILayout.TextField("Description", spellData.description);

            // Spell Scaling Section
            GUILayout.Label("Spell Scaling", EditorStyles.boldLabel);
            spellData.scalingAttribute = (AttributeType)EditorGUILayout.EnumPopup("Scaling Attribute", spellData.scalingAttribute);
            spellData.scalingFactor = EditorGUILayout.FloatField("Scaling Factor", spellData.scalingFactor);

            // Targeting System Section
            GUILayout.Label("Targeting System", EditorStyles.boldLabel);
            spellData.targetType = (TargetType)EditorGUILayout.EnumPopup("Target Type", spellData.targetType);

            // Range & Cooldown
            spellData.rangeMin = EditorGUILayout.IntField("RangeMin", spellData.rangeMin);
            spellData.rangeMax = EditorGUILayout.IntField("RangeMax", spellData.rangeMax);
            spellData.cooldown = EditorGUILayout.IntField("Cooldown", spellData.cooldown);
            spellData.isLignOfSight = EditorGUILayout.Toggle("isLightOfSight", spellData.isLignOfSight);
            spellData.isChangeableRange = EditorGUILayout.Toggle("isChangableRange", spellData.isChangeableRange);
            spellData.isTargetRequired = EditorGUILayout.Toggle("isTargetRequired", spellData.isTargetRequired);

            // Handle the spell costs (AP or other costs)
            spellData.apCost = EditorGUILayout.IntField("AP Cost", spellData.apCost);
            EditorGUILayout.LabelField("Extra Costs");
            for (int i = 0; i < spellData.extraCosts.Count; i++)
            {
                var cost = spellData.extraCosts[i];
                EditorGUILayout.BeginHorizontal();
                cost.type = (SpellResourceType)EditorGUILayout.EnumPopup("Resource Type", cost.type);
                cost.amount = EditorGUILayout.IntField("Amount", cost.amount);
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    spellData.extraCosts.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add New Cost"))
            {
                spellData.extraCosts.Add(new SpellCost { type = SpellResourceType.AP, amount = 0 });
            }

            // Display and modify spell effects
            GUILayout.Label("Effects", EditorStyles.boldLabel);

            // for (int i = 0; i < spellData.effects.Count; i++)
            // {
            //     SpellEffect effect = spellData.effects[i];

            //     if (effect is DamageEffect damageEffect)
            //     {
            //         damageEffect.value = EditorGUILayout.IntField("Damage Amount", damageEffect.value);
            //     }
            //     else if (effect is HealEffect healEffect)
            //     {
            //         healEffect.value = EditorGUILayout.IntField("Healing Amount", healEffect.value);
            //     }

            //     // Remove effect button
            //     if (GUILayout.Button("Remove Effect"))
            //     {
            //         spellData.effects.RemoveAt(i);
            //     }
            // }

            // Adding new effect options
            // GUILayout.Label("Add New Effect", EditorStyles.boldLabel);
            // damage = EditorGUILayout.IntField("Damage Amount", damage);
            // healing = EditorGUILayout.IntField("Healing Amount", healing);

            // if (GUILayout.Button("Add Damage Effect"))
            // {
            //     spellData.effects.Add(new DamageEffect(damage));
            // }

            // if (GUILayout.Button("Add Healing Effect"))
            // {
            //     spellData.effects.Add(new HealEffect(healing));
            // }
        }
        
        if (GUILayout.Button("Save Spell"))
        {
            EditorUtility.SetDirty(spellData);
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndVertical();
    }

    private void CreateNewSpell()
    {
        if (string.IsNullOrEmpty(newSpellName))
        {
            Debug.LogWarning("Spell name cannot be empty.");
            return;
        }        
        // Define the path and name for the new spell
        string path = $"Assets/Game/GameMode/Fight/Spells/Common/SpellData_{newSpellName}.asset";
        string assetPath = AssetDatabase.GenerateUniqueAssetPath(path);

        // Create the new SpellData instance
        SpellData newSpell = ScriptableObject.CreateInstance<SpellData>();
        newSpell.spellName = newSpellName;

        // Save the new spell to the Assets folder
        AssetDatabase.CreateAsset(newSpell, assetPath);
        AssetDatabase.SaveAssets();

        // Automatically select the new spell in the editor
        spellData = newSpell;

        EditorGUIUtility.PingObject(newSpell);
        newSpellName = "New Spell";
    }
}