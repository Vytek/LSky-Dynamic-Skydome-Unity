///////////////////////////////////
/// LSky
/// ===================
///
/// Description:
/// ==============
/// Resources for skydome.
////////////////////////////////////


using System.IO;
using UnityEngine;

namespace Rallec.LSky
{

	[CreateAssetMenu(fileName = "LSky_SkydomeResources", menuName = "Rallec/LSky/Resources", order = 1)]
	public class LSky_SkydomeResources : ScriptableObject
	{

		#region |Meshes|

        [Header("Sphere Meshes")]
        public Mesh sphereLOD0;
        public Mesh sphereLOD1;
        public Mesh sphereLOD2;
        public Mesh sphereLOD3;
        //==============================

        [Header("Atmosphere Meshes")]
        public Mesh atmosphereLOD0;
        public Mesh atmosphereLOD1;
        public Mesh atmosphereLOD2;
        //==============================

       // [Header("Quad Mesh")]
       // public Mesh quadMesh;
        //=============================

        [Header("Hemisphere")]
        public Mesh hemisphereLOD0;
		public Mesh hemisphereLOD1;
		public Mesh hemisphereLOD2;
        //=============================

        #endregion

        #region |Shaders|

        [Header("Deep Space Shaders")]
        public Shader galaxyBackgroundShader;
        public Shader starsFieldShader;


        [Header("Near Space Shaders")]
       // public Shader sunShader;
        public Shader moonShader;
        public Shader nearSpaceShader;
        //===================================

        [Header("Atmosphere Shaders")]
        public Shader atmosphereShader;
        public Shader skyboxShader;
        //===================================

        [Header("Clouds Shaders")]
        public Shader cloudsShader;
        //===================================

        #endregion

        #region |Materials|


        [Header("Deep Space Materials")]
        public Material galaxyBackgroundMaterial;
        public Material starsFieldMaterial;


        [Header("Near Space Materials")]
        public Material moonMaterial;
        public Material nearSpaceMaterial;
        //====================================

        [Header("Atmosphere Materials")]
        public Material atmosphereMaterial;
        public Material skyboxMaterial;
        //====================================

        [Header("Clouds Materials")]
        public Material cloudsMaterial;
        //====================================

        #endregion


	}

}