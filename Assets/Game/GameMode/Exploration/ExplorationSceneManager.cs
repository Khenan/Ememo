public class ExplorationSceneManager : GameSceneManager
{
    public override void StartScene()
    {
        base.StartScene();
        ExplorationManager.I?.StartExploration();
    }
}
