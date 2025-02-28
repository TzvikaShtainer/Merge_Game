namespace VisualLayer.MergeItems.MergeSystem
{
    public interface IMergeHandler
    {
        bool CanMerge(Item item1, Item item2);
        void Merge(Item item1, Item item2);
    }
}