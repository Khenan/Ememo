using UnityEngine;

public class SpriteManager : Singleton<SpriteManager>
{
    [System.Serializable]
    public class Tips { 
        public Sprite Default;
        public Sprite Blind;
    }
    public Tips TipsSprite = new();
    public Sprite Default;
}