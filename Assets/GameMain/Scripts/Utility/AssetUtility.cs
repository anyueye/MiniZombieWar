
using GameFramework;

public class AssetUtility
{
    public static string GetDataTableAsset(string assetName, bool fromBytes)=>Utility.Text.Format("Assets/GameMain/DataTables/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
    public static string GetSceneAsset(string assetName)=> Utility.Text.Format("Assets/GameMain/Scenes/{0}.unity", assetName);
    public static string GetEntityAsset(string assetName) => Utility.Text.Format("Assets/GameMain/Entites/{0}.prefab",assetName);
}
