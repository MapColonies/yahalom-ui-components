using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.CustomButtonExample
{
    public class CustomButton :Button
    {
        public new class UxmlFactory : UxmlFactory<CustomButton, UxmlTraits> { }

        public CustomButton()
        {
            AddToClassList("custom-button");
        }
    }
}