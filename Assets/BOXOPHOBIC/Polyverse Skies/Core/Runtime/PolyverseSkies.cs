// Cristian Pop - https://boxophobic.com/

using System;
using UnityEngine;
using Boxophobic.StyledGUI;

namespace PolyverseSkiesAsset
{
    // 禁止在同一GameObject上附加多个此组件
    [DisallowMultipleComponent]
    // 在编辑模式下也执行此脚本
    [ExecuteInEditMode]
    public class PolyverseSkies : StyledMonoBehaviour
    {
        // 使用StyledGUI显示自定义横幅，包含颜色、标题和文档链接
        [StyledBanner(0.968f, 0.572f, 0.890f, "Polyverse Skies", "", "https://docs.google.com/document/d/1z7A_xKNa2mXhvTRJqyu-ZQsAtbV32tEZQbO1OmPS_-s/edit?usp=sharing")]
        public bool styledBanner;

        // "Scene"分类标签，带有序号和缩进参数
        [StyledCategory("Scene", 5, 10)]
        public bool categoryScene;

        // 太阳方向控制对象
        public GameObject sunDirection;
        // 月亮方向控制对象
        public GameObject moonDirection;

        // "Time Of Day"分类标签
        [StyledCategory("Time Of Day")]
        public bool categoryTime;

        // 时间系统信息提示框
        [StyledMessage("Info", "The Time Of Day feature will interpolate between two Polyverse Skies materials. Please note that material properties such as textures and keywords will not be interpolated! You will need to enable the same features on both materials in order for the interpolation to work! Toggle Update Lighting to enable Unity's realtime environment lighting! ", 0, 10)]
        public bool categoryTimeMessage = true;

        // 白天天空盒材质
        public Material skyboxDay;
        // 夜晚天空盒材质
        public Material skyboxNight;
        // 时间控制参数（0=白天，1=夜晚）
        [Range(0, 1)]
        public float timeOfDay = 0;

        // 间距
        [Space(10)]
        // 是否更新实时照明
        public bool updateLighting = false;

        // 自定义间距
        [StyledSpace(5)]
        public bool styledSpace0;

        // 用于插值的天空盒材质实例
        Material skyboxMaterial;

        // 单例实例
        public static PolyverseSkies instance;

        // 唤醒时调用
        void Awake()
        {
            // 设置单例实例
            instance = this;
        }

        // 开始时调用
        void Start()
        {
            // 如果白天和夜晚材质都已设置，创建新的材质实例
            if (skyboxDay != null && skyboxNight != null)
            {
                skyboxMaterial = new Material(skyboxDay);
            }
        }

        // 每帧更新
        void Update()
        {
            // 设置全局太阳方向向量
            if (sunDirection != null)
            {
                Shader.SetGlobalVector("GlobalSunDirection", -sunDirection.transform.forward);
            }
            else
            {
                Shader.SetGlobalVector("GlobalSunDirection", Vector3.zero);
            }

            // 设置全局月亮方向向量
            if (moonDirection != null)
            {
                Shader.SetGlobalVector("GlobalMoonDirection", -moonDirection.transform.forward);
            }
            else
            {
                Shader.SetGlobalVector("GlobalMoonDirection", Vector3.zero);
            }

            // 在白天和夜晚材质之间进行插值
            if (skyboxDay != null && skyboxNight != null)
            {
                // 根据时间参数在两种材质间插值
                skyboxMaterial.Lerp(skyboxDay, skyboxNight, timeOfDay);
                // 设置渲染天空盒为插值后的材质
                RenderSettings.skybox = skyboxMaterial;
            }

            // 如果需要更新照明，更新环境光照
            if (updateLighting)
            {
                DynamicGI.UpdateEnvironment();
            }
        }

        // 仅在Unity编辑器下编译
#if UNITY_EDITOR
        // 当脚本值在检查器中更改时调用
        void OnValidate()
        {
            // 在编辑器模式下验证材质并创建新实例
            if (skyboxDay != null && skyboxNight != null)
            {
                skyboxMaterial = new Material(skyboxDay);
            }
        }
#endif
    }
}