namespace ServiceLayer.SaveSystem
{
    [System.Serializable]
    public class MergeItemSaveData
    {
        public string typeId;
        public SerializableTypes.SerializableVector2 position;
        public SerializableTypes.SerializableVector2 velocity;
        public SerializableTypes.SerializableQuaternion  rotation;
    }
}