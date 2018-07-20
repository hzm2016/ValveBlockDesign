using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using stdole;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal class BlockFormRequest:ValveBlockDesign.ChangeRequest
    {
        //Inventor application
        private Inventor.Application m_inventorApplication;
        //BlockForm command parameters
        private double m_length;
        private double m_width;
        private double m_height;
        private string m_ClientId = "c29d5be2-c9f7-4783-9191-5070d4944568";
        private System.Reflection.Assembly assembly;
        string[] resources;

        public BlockFormRequest(Inventor.Application application, double length, double width, double height)
        {
            m_inventorApplication = application;
            m_length = length;
            m_width = width;
            m_height = height;
        }

        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveEditObject;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;
            bool asset = true;
            foreach (Asset asset1 in oPartDoc.Assets)
            {
                if (asset1.DisplayName == "NET1"||asset1.DisplayName=="NET2"||asset1.DisplayName=="NET3")
                {
                    asset = false;
                }
            }
            if (asset == true)
            {
                Addassets();
            }
            //改变当前文件的ActiveAppearance
            Asset localAsset;
            bool flag = false;
            int t;
            int index = 0;
            for (t = 1; t <= oPartDoc.Assets.Count; t++)
            {
                if (oPartDoc.Assets[t].Name == "Generic-027")
                {
                    flag = true;
                    index = t;
                }
            }

            try
            {
                if (flag)
                {
                    localAsset = oPartDoc.Assets[index];
                }
                else
                {
                    AssetLibraries assetLibs = m_inventorApplication.AssetLibraries;
                    Asset libAsset = assetLibs["AFEFC330-5E61-4E24-814F-AE810148B79D"].AppearanceAssets["Generic-027"];
                    localAsset = libAsset.CopyTo(oPartDoc);
                }

                oPartDoc.ActiveAppearance = localAsset;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //添加新的草图
            PlanarSketch oSketch;
            oSketch = oPartDoc.ComponentDefinition.Sketches.Add(oPartDoc.ComponentDefinition.WorkPlanes[3], false);

            //创建临时几何图形对象，以便后续程序进行引用
            TransientGeometry oTransGeo;
            oTransGeo = m_inventorApplication.TransientGeometry;

            //画一个矩形
            oSketch.SketchLines.AddAsTwoPointRectangle(oTransGeo.CreatePoint2d(0, 0), oTransGeo.CreatePoint2d(m_length, m_width));

            //创建轮廓
            Profile oProfile;
            oProfile = oSketch.Profiles.AddForSolid(true, null, null);

            System.Array Line = System.Array.CreateInstance(typeof(SketchLine), 6);
            SketchLine[] line = Line as SketchLine[];

            //创建拉伸特征
            ExtrudeFeature oFirstExtrude;
            oFirstExtrude = oPartDoc.ComponentDefinition.Features.ExtrudeFeatures.AddByDistanceExtent(oProfile, m_height, PartFeatureExtentDirectionEnum.kPositiveExtentDirection, PartFeatureOperationEnum.kJoinOperation, null);

            //Create FaceCollection Object
            FaceCollection oFaceCollection;
            oFaceCollection = m_inventorApplication.TransientObjects.CreateFaceCollection();

            //Put faces into FaceCollection
            Faces oStartFaces;
            oStartFaces = oFirstExtrude.StartFaces;
            
            int oStFNomb = oStartFaces.Count;

            for (int i = 1; i <= oStFNomb; i++)
            {
                oFaceCollection.Add(oStartFaces[i]);
            }

            Faces oSideFaces;
            oSideFaces = oFirstExtrude.SideFaces;
            int oSiFNomb = oSideFaces.Count;

            for (int j = 1; j <= oSiFNomb; j++)
            {
                oFaceCollection.Add(oSideFaces[j]);
            }

            Faces oEndFaces;
            oEndFaces = oFirstExtrude.EndFaces;
            int oEdFNomb = oEndFaces.Count;
            
            for (int k = 1; k <= oEdFNomb; k++)
            {
                oFaceCollection.Add(oEndFaces[k]);
            }
            UserCoordinateSystems m_UCSs;
            m_UCSs = oPartCompDef.UserCoordinateSystems;
            foreach (UserCoordinateSystem ucs in m_UCSs)
            {
                ucs.Delete();
            }
            CreateUCS(oFaceCollection[6].Edges[3], oFaceCollection[6].Edges[4]);
            CreateUCS(oFaceCollection[5].Edges[3], oFaceCollection[5].Edges[4]);
            CreateUCS(oFaceCollection[4].Edges[4], oFaceCollection[4].Edges[1]);
            CreateUCS(oFaceCollection[3].Edges[3], oFaceCollection[3].Edges[4]);
            CreateUCS(oFaceCollection[2].Edges[3], oFaceCollection[2].Edges[4]);
            CreateUCS(oFaceCollection[1].Edges[3], oFaceCollection[1].Edges[4]);

            Document oDoc = default(Document);
            oDoc = m_inventorApplication.ActiveDocument;

            BrowserPanes oPanes = default(BrowserPanes);
            oPanes = oDoc.BrowserPanes;
            ClientNodeResources oRscs = oPanes.ClientNodeResources;
            if (oRscs.Count == 0)
            {
                AddTreeBrowersPane();
            }
        }

        private void CreateUCS(Edge edge1, Edge edge2)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveEditObject;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkPoint oUCSOrigin;
            oUCSOrigin = oPartCompDef.WorkPoints.AddByTwoLines(edge1, edge2, true);

            UserCoordinateSystemDefinition oUCSDef;
            oUCSDef = oPartCompDef.UserCoordinateSystems.CreateDefinition();

            oUCSDef.SetByThreePoints(oUCSOrigin, edge1, edge2);

            UserCoordinateSystem oUCS;
            oUCS = oPartCompDef.UserCoordinateSystems.Add(oUCSDef);
            oUCS.Visible = false;
        }

        private void IconPictureInitial()//获得系统资源图片
        {
            //get names of all the sources in the assenbly
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            resources = assembly.GetManifestResourceNames();
        }

        private void AddTreeBrowersPane()
        {
            IconPictureInitial();
            Document oDoc = default(Document);
            oDoc = m_inventorApplication.ActiveDocument;

            BrowserPanes oPanes = default(BrowserPanes);
            oPanes = oDoc.BrowserPanes;

            System.IO.Stream oStream_Block = assembly.GetManifestResourceStream("ValveBlockDesign.resources.BlockStandard.ico");
            System.Drawing.Icon oIcon_Block = new System.Drawing.Icon(oStream_Block);

            System.IO.Stream oStream_NET1 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET1.ico");
            System.Drawing.Icon oIcon_NET1 = new System.Drawing.Icon(oStream_NET1);
            System.IO.Stream oStream_NET2 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET2.ico");
            System.Drawing.Icon oIcon_NET2 = new System.Drawing.Icon(oStream_NET2);
            System.IO.Stream oStream_NET3 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET3.ico");
            System.Drawing.Icon oIcon_NET3 = new System.Drawing.Icon(oStream_NET3);
            System.IO.Stream oStream_NET4 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET4.ico");
            System.Drawing.Icon oIcon_NET4 = new System.Drawing.Icon(oStream_NET4);
            System.IO.Stream oStream_NET5 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET5.ico");
            System.Drawing.Icon oIcon_NET5 = new System.Drawing.Icon(oStream_NET5);
            System.IO.Stream oStream_NET6 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET6.ico");
            System.Drawing.Icon oIcon_NET6 = new System.Drawing.Icon(oStream_NET6);
            System.IO.Stream oStream_NET7 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET7.ico");
            System.Drawing.Icon oIcon_NET7 = new System.Drawing.Icon(oStream_NET7);
            System.IO.Stream oStream_NET8 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET8.ico");
            System.Drawing.Icon oIcon_NET8 = new System.Drawing.Icon(oStream_NET8);
            System.IO.Stream oStream_NET9 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET9.ico");
            System.Drawing.Icon oIcon_NET9 = new System.Drawing.Icon(oStream_NET9);
            System.IO.Stream oStream_NET10 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET10.ico");
            System.Drawing.Icon oIcon_NET10 = new System.Drawing.Icon(oStream_NET10);
            System.IO.Stream oStream_NET11 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET11.ico");
            System.Drawing.Icon oIcon_NET11 = new System.Drawing.Icon(oStream_NET11);
            System.IO.Stream oStream_NET12 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET12.ico");
            System.Drawing.Icon oIcon_NET12 = new System.Drawing.Icon(oStream_NET12);
            System.IO.Stream oStream_NET13 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET13.ico");
            System.Drawing.Icon oIcon_NET13 = new System.Drawing.Icon(oStream_NET13);
            System.IO.Stream oStream_NET14 = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NET14.ico");
            System.Drawing.Icon oIcon_NET14 = new System.Drawing.Icon(oStream_NET14);
            System.IO.Stream oStream_NULL = assembly.GetManifestResourceStream("ValveBlockDesign.resources.NULL.ico");
            System.Drawing.Icon oIcon_NULL = new System.Drawing.Icon(oStream_NULL);

            System.IO.Stream oStream_Cavity = assembly.GetManifestResourceStream("ValveBlockDesign.resources.Cavity.ico");
            System.Drawing.Icon oIcon_Cavity = new System.Drawing.Icon(oStream_Cavity);
            System.IO.Stream oStream_Footprint = assembly.GetManifestResourceStream("ValveBlockDesign.resources.Footprint.ico");
            System.Drawing.Icon oIcon_Footprint = new System.Drawing.Icon(oStream_Footprint);
            
            //This is the icon that will be displayed at this node. Add the IPictureDisp to the client node resource.

            ClientNodeResources oRscs = oPanes.ClientNodeResources;
            
            stdole.IPictureDisp clientNodeIcon = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_Block.ToBitmap());
            ClientNodeResource oRsc = oRscs.Add(m_ClientId, 1, clientNodeIcon);

            stdole.IPictureDisp clientNodeIcon1 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET1.ToBitmap());
            ClientNodeResource oRsc1 = oRscs.Add(m_ClientId, 2, clientNodeIcon1);

            stdole.IPictureDisp clientNodeIcon2 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET2.ToBitmap());
            ClientNodeResource oRsc2 = oRscs.Add(m_ClientId, 3, clientNodeIcon2);

            stdole.IPictureDisp clientNodeIcon3 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET3.ToBitmap());
            ClientNodeResource oRsc3 = oRscs.Add(m_ClientId, 4, clientNodeIcon3);

            stdole.IPictureDisp clientNodeIcon4 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET4.ToBitmap());
            ClientNodeResource oRsc4 = oRscs.Add(m_ClientId, 5, clientNodeIcon4);
            stdole.IPictureDisp clientNodeIcon5 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET5.ToBitmap());
            ClientNodeResource oRsc5 = oRscs.Add(m_ClientId, 6, clientNodeIcon5);
            stdole.IPictureDisp clientNodeIcon6 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET6.ToBitmap());
            ClientNodeResource oRsc6 = oRscs.Add(m_ClientId, 7, clientNodeIcon6);
            stdole.IPictureDisp clientNodeIcon7 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET7.ToBitmap());
            ClientNodeResource oRsc7 = oRscs.Add(m_ClientId, 8, clientNodeIcon7);
            stdole.IPictureDisp clientNodeIcon8 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET8.ToBitmap());
            ClientNodeResource oRsc8 = oRscs.Add(m_ClientId, 9, clientNodeIcon8);
            stdole.IPictureDisp clientNodeIcon9 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET9.ToBitmap());
            ClientNodeResource oRsc9 = oRscs.Add(m_ClientId, 10, clientNodeIcon9);
            stdole.IPictureDisp clientNodeIcon10 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET10.ToBitmap());
            ClientNodeResource oRsc10 = oRscs.Add(m_ClientId, 11, clientNodeIcon10);
            stdole.IPictureDisp clientNodeIcon11 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET11.ToBitmap());
            ClientNodeResource oRsc11 = oRscs.Add(m_ClientId, 12, clientNodeIcon11);
            stdole.IPictureDisp clientNodeIcon12 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET12.ToBitmap());
            ClientNodeResource oRsc12 = oRscs.Add(m_ClientId, 13, clientNodeIcon12);

            stdole.IPictureDisp clientNodeIcon13 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_Cavity.ToBitmap());
            ClientNodeResource oRsc13 = oRscs.Add(m_ClientId, 14, clientNodeIcon13);
            stdole.IPictureDisp clientNodeIcon14 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_Footprint.ToBitmap());
            ClientNodeResource oRsc14 = oRscs.Add(m_ClientId, 15, clientNodeIcon14);
            stdole.IPictureDisp clientNodeIcon15 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NULL.ToBitmap());
            ClientNodeResource oRsc15 = oRscs.Add(m_ClientId, 16, clientNodeIcon15);
            stdole.IPictureDisp clientNodeIcon16 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET13.ToBitmap());
            ClientNodeResource oRsc16 = oRscs.Add(m_ClientId, 17, clientNodeIcon16);
            stdole.IPictureDisp clientNodeIcon17 = ValveBlockDesign.StandardAddInServer.AxHostConverter.ImageToPictureDisp(oIcon_NET14.ToBitmap());
            ClientNodeResource oRsc17 = oRscs.Add(m_ClientId, 18, clientNodeIcon17);

            BrowserNodeDefinition oDef = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("网络清单", 1, oRsc);

            Inventor.BrowserPane oPane = oPanes.AddTreeBrowserPane("油路", m_ClientId, oDef);
            Inventor.BrowserNode topNode = oPanes["油路"].TopNode;
            BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET1", 2, oRsc1);
            topNode.AddChild(oDef1);
            BrowserNodeDefinition oDef2 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET2", 3, oRsc2);
            topNode.AddChild(oDef2);
            BrowserNodeDefinition oDef3 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET3", 4, oRsc3);
            topNode.AddChild(oDef3);
            BrowserNodeDefinition oDef4 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET4", 5, oRsc4);
            topNode.AddChild(oDef4);
            BrowserNodeDefinition oDef5 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET5", 6, oRsc5);
            topNode.AddChild(oDef5);
            BrowserNodeDefinition oDef6 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET6", 7, oRsc6);
            topNode.AddChild(oDef6);
            BrowserNodeDefinition oDef7 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET7", 8, oRsc7);
            topNode.AddChild(oDef7);
            BrowserNodeDefinition oDef8 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET8", 9, oRsc8);
            topNode.AddChild(oDef8);
            BrowserNodeDefinition oDef9 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET9", 10, oRsc9);
            topNode.AddChild(oDef9);
            BrowserNodeDefinition oDef10 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET10", 11, oRsc10);
            topNode.AddChild(oDef10);
            BrowserNodeDefinition oDef11 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET11", 12, oRsc11);
            topNode.AddChild(oDef11);
            BrowserNodeDefinition oDef12 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET12", 13, oRsc12);
            topNode.AddChild(oDef12);
            BrowserNodeDefinition oDef13 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET13", 14, oRsc16);
            topNode.AddChild(oDef13);
            BrowserNodeDefinition oDef14 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NET14", 15, oRsc17);
            topNode.AddChild(oDef14);
            BrowserNodeDefinition oDef15 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("NULLNET", 16, oRsc15);

            topNode.AddChild(oDef15);
            oPanes["油路"].Update();
            oPanes["模型"].Activate();
        }//创建浏览器节点

        private void Addassets()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveEditObject;
            
            //创建网络1的红色外观
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;
            
            Asset assetNet1;
            assetNet1 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET1", "NET1");

            ColorAssetValue colorred;
            colorred = (ColorAssetValue)assetNet1["generic_diffuse"];
            colorred.Value = m_inventorApplication.TransientObjects.CreateColor(255, 0, 0);
            
            //创建网络2的蓝色外观
            Asset assetNet2;
            assetNet2 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET2", "NET2");

            ColorAssetValue colorblue;
            colorblue = (ColorAssetValue)assetNet2["generic_diffuse"];
            colorblue.Value = m_inventorApplication.TransientObjects.CreateColor(0, 0, 255);
            
            //创建网络3的绿色外观
            Asset assetNet3;
            assetNet3 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET3", "NET3");

            ColorAssetValue colorgreen;
            colorgreen = (ColorAssetValue)assetNet3["generic_diffuse"];
            colorgreen.Value = m_inventorApplication.TransientObjects.CreateColor(0, 255, 0);
            //创建网络4的黄色外观
            Asset assetNet4;
            assetNet4 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET4", "NET4");

            ColorAssetValue coloryellow;
            coloryellow = (ColorAssetValue)assetNet4["generic_diffuse"];
            coloryellow.Value = m_inventorApplication.TransientObjects.CreateColor(255, 255, 0);
            //创建网络5的粉色外观
            Asset assetNet5;
            assetNet5 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET5", "NET5");

            ColorAssetValue colorpink;
            colorpink = (ColorAssetValue)assetNet5["generic_diffuse"];
            colorpink.Value = m_inventorApplication.TransientObjects.CreateColor(255, 0, 255);
            //创建网络6的天蓝色外观
            Asset assetNet6;
            assetNet6 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET6", "NET6");

            ColorAssetValue colorskyblue;
            colorskyblue = (ColorAssetValue)assetNet6["generic_diffuse"];
            colorskyblue.Value = m_inventorApplication.TransientObjects.CreateColor(0, 255, 255);
            //创建网络7的深蓝色色外观
            Asset assetNet7;
            assetNet7 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET7", "NET7");

            ColorAssetValue colordarkblue;
            colordarkblue = (ColorAssetValue)assetNet7["generic_diffuse"];
            colordarkblue.Value = m_inventorApplication.TransientObjects.CreateColor(40, 35, 117);
            //创建网络8的紫色外观
            Asset assetNet8;
            assetNet8 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET8", "NET8");

            ColorAssetValue colorpurple;
            colorpurple = (ColorAssetValue)assetNet8["generic_diffuse"];
            colorpurple.Value = m_inventorApplication.TransientObjects.CreateColor(119, 0, 255);
            //创建网络9的墨绿色外观
            Asset assetNet9;
            assetNet9 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET9", "NET9");

            ColorAssetValue colordarkgreen;
            colordarkgreen = (ColorAssetValue)assetNet9["generic_diffuse"];
            colordarkgreen.Value = m_inventorApplication.TransientObjects.CreateColor(28, 84, 41);
            //创建网络10的黑色外观
            Asset assetNet10;
            assetNet10 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET10", "NET10");

            ColorAssetValue colordark;
            colordark = (ColorAssetValue)assetNet10["generic_diffuse"];
            colordark.Value = m_inventorApplication.TransientObjects.CreateColor(0, 0, 0);
            //创建网络11的棕色外观
            Asset assetNet11;
            assetNet11 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET11", "NET11");

            ColorAssetValue colorbrown;
            colorbrown = (ColorAssetValue)assetNet11["generic_diffuse"];
            colorbrown.Value = m_inventorApplication.TransientObjects.CreateColor(104, 38, 39);
            //创建网络12的卡其色外观
            Asset assetNet12;
            assetNet12 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET12", "NET12");

            ColorAssetValue colorkhaqi;
            colorkhaqi = (ColorAssetValue)assetNet12["generic_diffuse"];
            colorkhaqi.Value = m_inventorApplication.TransientObjects.CreateColor(73, 71, 28);
            //创建网络13的外观
            Asset assetNet13;
            assetNet13 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET13", "NET13");

            ColorAssetValue colordarkorange;
            colordarkorange = (ColorAssetValue)assetNet13["generic_diffuse"];
            colordarkorange.Value = m_inventorApplication.TransientObjects.CreateColor(247, 149, 37);
            //创建网络14的外观
            Asset assetNet14;
            assetNet14 = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NET14", "NET14");

            ColorAssetValue colordarkgreen2;
            colordarkgreen2 = (ColorAssetValue)assetNet14["generic_diffuse"];
            colordarkgreen2.Value = m_inventorApplication.TransientObjects.CreateColor(16, 104, 60);

            //创建默认网络外观
            Asset assetNetNull;
            assetNetNull = oPartDoc.Assets.Add(AssetTypeEnum.kAssetTypeAppearance, "Generic", "NULLNET", "NULLNET");

            ColorAssetValue colornull;
            colornull = (ColorAssetValue)assetNetNull["generic_diffuse"];
            colornull.Value = m_inventorApplication.TransientObjects.CreateColor(255, 255, 255);

        }
    }
}
