using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RMC.MyProject.Data
{
    /// <summary>
    /// Renders the <see cref="PlayerData"/> in the Unity Inspector Window.
    /// </summary>
    [CustomPropertyDrawer(typeof (PlayerData), true)]
    public class PlayerDataPropertyDrawer : PropertyDrawer 
    {
        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        private TemplateContainer _playerDataLayout;

        //  Unity Methods ---------------------------------
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Load the UXML layout
            var visualTree = Resources.Load<VisualTreeAsset>("Layouts/PlayerDataLayout");
            _playerDataLayout = visualTree.CloneTree();

            BindPlayerData(property);
            return _playerDataLayout;
        }
        
        
        //  Methods ---------------------------------------
        private void BindPlayerData(SerializedProperty property)
        {
            // Bind the serialized property to the UI fields
            _playerDataLayout.Q<FloatField>("MoveSpeed").BindProperty(property.FindPropertyRelative("MoveSpeed"));
            _playerDataLayout.Q<FloatField>("JumpSpeed").BindProperty(property.FindPropertyRelative("JumpSpeed"));
        }

        //  Event Handlers --------------------------------
    }
}