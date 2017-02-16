using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SelectiveUI : UITemplate {

    // Different factors to be used

    public RenderTexture mMapCam;
    
    //Old GUI system, Obsolete
    //public GUISkin formationSkin, stanceSkin, uInfoSkin, minimapSkin;

    //Sizes for UI, mainly used in obsolete version of the UI
    private int FORMATIONS_BAR_WIDTH = Mathf.RoundToInt((Screen.width)*0.5f), MINIMAP_BAR_WIDTH = Mathf.RoundToInt((Screen.width) * 0.2f), STANCE_BAR_HEIGHT = 100, UINFO_BAR_WIDTH = Mathf.RoundToInt((Screen.width)- 200);

    //Minimap factors
    Texture2D sourceRender;
    Texture2D destinationRender;
    GameObject mCam;

    // String builder for Debugging
    StringBuilder sb;

    // Panels for ease of use
    GameObject IngameUI;
    GameObject[] UIPanels;

    //UI Layer
    private const int LayerUI = 5;
    
    

    // Use this for initialization
    void Start () {
        
        
        sb = new StringBuilder();

        this.transform.localPosition = new Vector3(0, 0, 0);
        
        mMapCam = new RenderTexture(Screen.width, Screen.height, 24);
        sourceRender = new Texture2D(MINIMAP_BAR_WIDTH, MINIMAP_BAR_WIDTH);
        mCam = CameraMinimap();
        RenderMinimap();
        CreateUI();
        Debug.Log("Start Loppuu");
    }

    private GameObject CreateUIBaseBox(RectTransform recT)
    {
        GameObject GameUIBox = new GameObject();
        GameUIBox.layer = LayerUI;

        recT = GameUIBox.AddComponent<RectTransform>();

        CanvasRenderer renderer = GameUIBox.AddComponent<CanvasRenderer>();

        return GameUIBox;
    }

    private GameObject TurnIntoObjInfoBox(GameObject objUI)
    {
        
        HorizontalLayoutGroup HLG = objUI.AddComponent<HorizontalLayoutGroup>();
        HLG.childForceExpandWidth = false;


        RectTransform recTTextsPanel = new RectTransform();
        GameObject ObjPanelTexts = new GameObject();
        ObjPanelTexts = CreatePanelUI(ObjPanelTexts ,objUI.transform, "DoublePanel");

        ObjPanelTexts.transform.SetParent(HLG.transform);        

        RectTransform recTObjName = new RectTransform();
        GameObject ObjPanelName = new GameObject();
        ObjPanelName = CreatePanelUI(ObjPanelName, ObjPanelTexts.transform, "Name");
        ObjPanelName = CreateTextBox(ObjPanelName.transform, 0, 0, (float)(Screen.height * 0.05), (float)(Screen.width * 0.8), "Set Text From Selected Unit", 28 );
        ObjPanelName.transform.SetAsFirstSibling();

        RectTransform recTDesc = new RectTransform();
        GameObject ObjPanelDesc = new GameObject();
        ObjPanelDesc = CreatePanelUI(ObjPanelDesc, ObjPanelTexts.transform, "Text");
        ObjPanelDesc = CreateTextBox(ObjPanelDesc.transform, 0, 0, (float)(Screen.height * 0.15), (float)(Screen.width * 0.8), "Set Text From Selected Unit", 24 );

        RectTransform recTImage = new RectTransform();
        GameObject ObjPanelPic = new GameObject();
        ObjPanelPic = CreatePanelUI(ObjPanelPic, objUI.transform, "Pic");
        Sprite spr = Resources.Load<Sprite>("UISquare");
        ObjPanelPic.transform.SetParent(objUI.transform);
        Image PicIMG = ObjPanelPic.AddComponent<Image>();
        PicIMG.sprite = spr;
        ObjPanelPic.transform.SetAsFirstSibling();

        VerticalLayoutGroup VLG = ObjPanelTexts.AddComponent<VerticalLayoutGroup>();
        

        
        

        return objUI;
    }

    private void CreateUI()
    {
        IngameUI = new GameObject("UI_BASE");
        IngameUI.layer = LayerUI;

        RectTransform rt = IngameUI.AddComponent<RectTransform>();
        
        rt.sizeDelta = new Vector2(0, 0);

        GameObject Panel01 = new GameObject("PanelObj");
        GameObject Panel02 = new GameObject("UnitInfo");
        GameObject Panel03 = new GameObject("UpperMenu");
        GameObject Panel04 = new GameObject("Minimap");
        
        IngameUI.transform.SetParent(this.transform);

        Canvas UICanvas = CreateCanvas(IngameUI.transform);
        GameObject UIEventSys = CreateEventSys(IngameUI.transform);

        Vector2 anchMin;
        Vector2 anchMax;
        Vector3 anchPos3D;
        Vector2 anchPos;
        Vector2 offMin;
        Vector2 offMax;
        Vector3 localPos;
        Vector2 sizeDelta;
        Vector3 localScale;
        Vector2 pivot;

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 3:
                    anchMin = new Vector2(0, 0);
                    anchMax = new Vector2(0, 0);
                    anchPos3D = new Vector3(0, 0, 0);
                    anchPos = new Vector2(0, 0);
                    offMin = new Vector2(0, 0);
                    offMax = new Vector2(0, 0);
                    localPos = new Vector3(0, 0, 0);
                    sizeDelta = new Vector2((float)(Screen.width * 0.2), (float)(Screen.height * 0.2));
                    localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    pivot = new Vector2(0, 0);

                    Panel04 = CreatePanelUI(Panel04, UICanvas.transform, "Opt4Round", anchMin, anchMax, anchPos, offMin, offMax, sizeDelta, anchPos3D, localPos, localScale, pivot);

                    CreateTextBox(Panel04.transform, 10, 10, 100, 50, "PO4", 32);

                    break;
                case 2:
                    anchMin = new Vector2(0, 1);
                    anchMax = new Vector2(0, 1);
                    anchPos3D = new Vector3(0, 0, 0);
                    anchPos = new Vector2(0, 0);
                    offMin = new Vector2(0, 0);
                    offMax = new Vector2(0, 0);
                    localPos = new Vector3(0, 0, 0);
                    sizeDelta = new Vector2((float)(Screen.width), (float)(Screen.height * 0.1));
                    localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    pivot = new Vector2(0, 1);

                    Panel03 = CreatePanelUI(Panel03, UICanvas.transform, "UpperMenu", anchMin, anchMax, anchPos, offMin, offMax, sizeDelta, anchPos3D, localPos, localScale, pivot);

                    CreateTextBox(Panel03.transform, 10, 10, 100, 50, "Upper Menu", 32);

                    break;
                case 1:
                    anchMin = new Vector2(1, 0);
                    anchMax = new Vector2(1, 0);
                    anchPos3D = new Vector3(0, 0, 0);
                    anchPos = new Vector2(0, 0);
                    offMin = new Vector2(1, 0);
                    offMax = new Vector2(1, 0);
                    localPos = new Vector3(0, 0, 0);
                    sizeDelta = new Vector2((float)(Screen.width * 0.8), (float)(Screen.height * 0.15));
                    localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    pivot = new Vector2(1.0f, 0);

                    Panel02 = CreatePanelUI(Panel02, UICanvas.transform, "UnitInfo", anchMin,anchMax,anchPos,offMin,offMax,sizeDelta,anchPos3D, localPos, localScale, pivot);

                    Panel02 = TurnIntoObjInfoBox(Panel02);

                    break;

                case 0:
                    Panel01 = CreatePanelUI(Panel01, UICanvas.transform, "Panel01");

                    CreateTextBox(Panel01.transform, 60, 250, 100, 50, "Objectives:", 32);
                    CreateTextBox(Panel01.transform, 60, 150, 50, 50, "It works", 24);
                    Panel01.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    //CreateButton(Panel01.transform, 60, 50, 100, 50, "Point", delegate { OnCancel(); });

                    break;
                default:
                    break;

            }
        }

    }

    private GameObject SetCanvasAsParent(Transform ingameUI, GameObject gObj)
    {
        gObj.transform.SetParent(ingameUI);
        return gObj;
    }

    private GameObject CreatePanelUI(GameObject panelObj, Transform ingameUI, String name)
    {
        
        panelObj.transform.SetParent(ingameUI);
        panelObj.layer = LayerUI;

        CanvasRenderer renderer = panelObj.AddComponent<CanvasRenderer>();
        RectTransform transRectPanel = panelObj.AddComponent<RectTransform>();
        
        
        
            
        transRectPanel.anchorMin = new Vector2(0, 0);
        transRectPanel.anchorMax = new Vector2(0, 0);
        transRectPanel.anchoredPosition3D = new Vector3(0, 0, 0);
        transRectPanel.anchoredPosition = new Vector2(0, 0);
        transRectPanel.offsetMin = new Vector2(0, 0);
        transRectPanel.offsetMax = new Vector2(0, 0);
        transRectPanel.localPosition.Set(0, 0, 0);
        transRectPanel.sizeDelta = new Vector2(0, 0);
        transRectPanel.pivot = new Vector2(0,0);
        transRectPanel.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        
        

        return panelObj;
    }

    private GameObject CreatePanelUI(GameObject panelObj, Transform ingameUI, String name, Vector2 anchMin, Vector2 anchMax, 
        Vector2 anchPos, Vector2 offMin, Vector2 offMax, Vector2 sDelta, Vector3 anchPos3d, Vector3 locPos, Vector3 locScale, Vector2 pivot)
    {
        Debug.Log("Fighting " + ingameUI.name.ToString() + " might work");
        

        CanvasRenderer renderer = panelObj.AddComponent<CanvasRenderer>();
        RectTransform transRectPanel = panelObj.AddComponent<RectTransform>();

        panelObj.transform.SetParent(ingameUI);
        panelObj.layer = LayerUI;

        if (anchMin == null)
            transRectPanel.anchorMin = new Vector2(0, 0);
        else
            transRectPanel.anchorMin = anchMin;
        if (anchMax == null)
            transRectPanel.anchorMax = new Vector2(1, 1);
        else
            transRectPanel.anchorMax = anchMax;
        if (anchPos3d == null)
            transRectPanel.anchoredPosition3D = new Vector3(0, 0, 0);
        else
            transRectPanel.anchoredPosition3D = anchPos3d;
        if (anchPos == null)
            transRectPanel.anchoredPosition = new Vector2(0, 0);
        else
            transRectPanel.anchoredPosition = anchPos;
        if (offMin == null)
            transRectPanel.offsetMin = new Vector2(0, 0);
        else
            transRectPanel.offsetMin = offMin;
        if (offMax == null)
            transRectPanel.offsetMax = new Vector2(0, 0);
        else
            transRectPanel.offsetMax = offMax;
        /*if (locPos == null)
            transRectPanel.localPosition = new Vector3(0, 0, 0);
        else
            transRectPanel.localPosition = locPos;*/
        if (sDelta == null)
            transRectPanel.sizeDelta = new Vector2(0, 0);
        else
            transRectPanel.sizeDelta = sDelta;
        if (locScale == null)
            transRectPanel.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else
            transRectPanel.localScale = locScale;
        if (pivot == null)
            transRectPanel.pivot = new Vector2(0.5f, 1.0f);
        else
            transRectPanel.pivot = pivot;

        Sprite spr = Resources.Load<Sprite>("UISquare");
        Image Img = panelObj.AddComponent<Image>();

        Img.sprite = spr;
        return panelObj;
    }

    private Canvas CreateCanvas(Transform ingameUI)
    {
        // create the canvas
        GameObject canvasObj = new GameObject("CanvasXO10");
        canvasObj.layer = LayerUI;

        RectTransform canvasTrans = canvasObj.AddComponent<RectTransform>();
        canvasTrans.localPosition = new Vector3(0,0,0);
        canvasTrans.position = new Vector3(0, 0, 0);
        canvasTrans.anchoredPosition = new Vector3(0, 0, 0);


        Canvas CanvasUI = canvasObj.AddComponent<Canvas>();
        CanvasUI.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasUI.pixelPerfect = true;

        CanvasScaler CanvasScale = canvasObj.AddComponent<CanvasScaler>();
        CanvasScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        CanvasScale.referenceResolution = new Vector2(800, 600);

        GraphicRaycaster canvasRayc = canvasObj.AddComponent<GraphicRaycaster>();

        canvasObj.transform.SetParent(ingameUI);

        return CanvasUI;
    }

    private GameObject CreateTextBox(Transform panel, float x, float y, float h, float w, String str, int size)
    {
        Debug.Log("WUTextBox 1");

        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(panel);

        textObject.layer = LayerUI;

        RectTransform trans = textObject.AddComponent<RectTransform>();
        trans.sizeDelta.Set(w, h);
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = new Vector2(x, y);
        trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        trans.localPosition.Set(0, 0, 0);

        CanvasRenderer renderer = textObject.AddComponent<CanvasRenderer>();

        Text text = textObject.AddComponent<Text>();
        text.supportRichText = true;
        text.text = str;
        text.fontSize = size;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.color = new Color(0, 0, 1);

        return textObject;
    }

    private GameObject CreateButton(Transform panel, float x, float y,
                                        float w, float h, string str,
                                        UnityAction eventListner)
    {
        GameObject buttonObject = new GameObject("Button");
        buttonObject.transform.SetParent(panel);

        buttonObject.layer = LayerUI;

        RectTransform trans = buttonObject.AddComponent<RectTransform>();
        SetSize(trans, new Vector2(w, h));
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = new Vector2(x, y);
        trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        trans.localPosition.Set(0, 0, 0);

        CanvasRenderer renderer = buttonObject.AddComponent<CanvasRenderer>();

        Image image = buttonObject.AddComponent<Image>();

        Texture2D tex = Resources.Load<Texture2D>("Background");
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                                  new Vector2(0.5f, 0.5f));

        Button button = buttonObject.AddComponent<Button>();
        button.interactable = true;
        button.onClick.AddListener(eventListner);

        GameObject textObject = CreateTextBox(buttonObject.transform, 0, 0, 0, 0,
                                                   str, 24);

        return buttonObject;
    }


    

    public override void LoadUI()
    {
        throw new NotImplementedException();
    }

    public override void UpdateUI()
    {
        throw new NotImplementedException();
    }

    public override void UsageUI()
    {
        throw new NotImplementedException();
    }
    

    public void UpdateUI(List<UnitTemplate> UnitList)
    {
        
    }

    

    private void RenderMinimap()
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = mMapCam;
        
        mCam.GetComponent<Camera>().Render();

        sourceRender.ReadPixels(new Rect(0.0f, 0.0f, mMapCam.width, mMapCam.height), 0, 0);
        sourceRender.Resize(MINIMAP_BAR_WIDTH, MINIMAP_BAR_WIDTH);
        
        sourceRender.Apply();

    }

    private void SetSize(RectTransform trans, Vector2 size)
    {
        Vector2 currSize = trans.rect.size;
        Vector2 sizeDiff = size - currSize;
        trans.offsetMin = trans.offsetMin -
                                  new Vector2(sizeDiff.x * trans.pivot.x,
                                      sizeDiff.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax +
                                  new Vector2(sizeDiff.x * (1.0f - trans.pivot.x),
                                      sizeDiff.y * (1.0f - trans.pivot.y));
    }



    private GameObject CameraMinimap()
    {
        GameObject minicamera = new GameObject("minimapCam", typeof(Camera));
        minicamera.GetComponent<Camera>().rect = new Rect(0.000f, 0.0f, 0.25f, 0.25f);//new Rect(0.005f, 0.73f, 0.25f, 0.25f);

        minicamera.transform.Translate(0, 20, 0);
        //minicamera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0, 0, 0));
        minicamera.transform.rotation = Quaternion.Euler(90, 0, 0);
        minicamera.GetComponent<Camera>().orthographic = true;
        minicamera.GetComponent<Camera>().depth = 1;
        minicamera.GetComponent<Camera>().orthographicSize = 20;
        minicamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;

        minicamera.GetComponent<Camera>().targetTexture = mMapCam;
        Debug.Log("Testi Mappi" + mMapCam.height + " / " + mMapCam.width);
        return minicamera;
    }

    private GameObject CreateEventSys(Transform ingameUI)
    {
        GameObject goEventuality = new GameObject("EventSystem");
        goEventuality.transform.SetParent(ingameUI);

        EventSystem eSystem = goEventuality.AddComponent<EventSystem>();
        eSystem.sendNavigationEvents = true;
        eSystem.pixelDragThreshold = 5;

        StandaloneInputModule stdaInput = goEventuality.AddComponent<StandaloneInputModule>();
        stdaInput.horizontalAxis = "Horizontal";
        stdaInput.verticalAxis = "Vertical";

        //TouchInputModule tInput = goEventuality.AddComponent<TouchInputModule>();

        goEventuality.transform.SetParent(ingameUI);

        return goEventuality;
    }

    // Update is called once per frame
    void OnGUI()
    {


    }

    //Old UI System, Obsolete, yet works
    /*public void LoadStanceGUI()
    {
        Debug.Log("Stance");
        GUI.skin = stanceSkin;
        GUI.BeginGroup(new Rect(0, Screen.height - STANCE_BAR_HEIGHT, FORMATIONS_BAR_WIDTH, Screen.height - STANCE_BAR_HEIGHT));
        GUI.Box(new Rect(0, 0, FORMATIONS_BAR_WIDTH, Screen.height - STANCE_BAR_HEIGHT), "");
        GUI.backgroundColor = Color.cyan;
        GUI.EndGroup();
    }
    public void LoadFormationGUI()
    {
        Debug.Log("Formation");
        GUI.skin = formationSkin;
        GUI.BeginGroup(new Rect(FORMATIONS_BAR_WIDTH, Screen.height - STANCE_BAR_HEIGHT, FORMATIONS_BAR_WIDTH, Screen.height - STANCE_BAR_HEIGHT));
        GUI.Box(new Rect(0, 0, FORMATIONS_BAR_WIDTH, Screen.height - STANCE_BAR_HEIGHT), "");
        GUI.backgroundColor = Color.gray;
        GUI.EndGroup();
    }
    public void LoadResourcesGUI()
    {

    }
    public void LoadUnitInfoGUI()
    {
        Debug.Log("Skin");
        GUI.skin = uInfoSkin;
        GUI.BeginGroup(new Rect(MINIMAP_BAR_WIDTH, 0, Screen.width - MINIMAP_BAR_WIDTH, STANCE_BAR_HEIGHT));
        GUI.Box(new Rect(0, 0, UINFO_BAR_WIDTH, STANCE_BAR_HEIGHT), "");
        GUI.backgroundColor = Color.cyan;
        GUI.EndGroup();

    }
    public void LoadMinimapGUI()
    {
        Debug.Log("Minimap");
        
        GUI.skin = uInfoSkin;
        GUI.BeginGroup(new Rect(0, 0, MINIMAP_BAR_WIDTH, MINIMAP_BAR_WIDTH));
        GUI.Box(new Rect(0, 0, MINIMAP_BAR_WIDTH, MINIMAP_BAR_WIDTH), "");
        GUI.DrawTexture(new Rect(0, 0, FORMATIONS_BAR_WIDTH, Screen.height - STANCE_BAR_HEIGHT), sourceRender, ScaleMode.ScaleAndCrop, true, 10.0f);
        GUI.backgroundColor = Color.green;
        GUI.EndGroup();
        
    }
    private void OnExit()
    {
        Application.Quit();
    }

    private void OnCancel()
    {
        GameObject.Destroy(this);
    }
    */
}
