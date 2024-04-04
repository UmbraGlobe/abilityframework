using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Gameplay Ability System/Tag")]
public class TagSO : ScriptableObject
{
    [SerializeField]
    private TagSO _parent;
    public TagSO Parent { get { return _parent; } }


    public bool IsDescendantOf(TagSO other, int nSearchLimit = 4)
    {
        int i = 0;
        TagSO tag = Parent;
        while (nSearchLimit > i++)
        {
            // tag will be invalid once we are at the root ancestor
            if (!tag) return false;

            // Match found, so we can return true
            if (tag == other) return true;

            // No match found, so try again with the next ancestor
            tag = tag.Parent;
        }

        // If we've exhausted the search limit, no ancestor was found
        return false;
    }
}