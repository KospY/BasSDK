using UnityEditor;

public class DisableModelMaterialImport : AssetPostprocessor
{
    public void OnPreprocessModel()
    {
        ModelImporter modelImporter = (ModelImporter)assetImporter;
        if (modelImporter.materialImportMode != ModelImporterMaterialImportMode.ImportViaMaterialDescription)
        {
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
        }
    }
}
