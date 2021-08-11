using Unity;
using UnityEngine;

using Shower;

namespace Scripts.Helper
{
    public class UISettingLoader : MonoBehaviour
    {
        public PersistentObjectManager persistentObjectManager;
        public TextBox textBox;

        private void Start()
        {
            persistentObjectManager.DefaultUIShowerSetting = new persistentObject.DefaultUIShowerSetting(textBox, 0.1f);
        }

        //在这里Load进UI的TextBox对象等, 用于简化Task
    }

}
