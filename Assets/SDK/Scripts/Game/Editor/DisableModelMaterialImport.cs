using UnityEditor;
 
public class DisableModelMaterialImport : AssetPostprocessor
{
    public void OnPreprocessModel()
    {
        ModelImporter modelImporter = (ModelImporter)assetImporter;
        modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
    }
}
