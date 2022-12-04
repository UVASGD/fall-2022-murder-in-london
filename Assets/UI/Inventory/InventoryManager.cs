using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    void Awake(){
        if(Instance != null){
            Destroy(this.gameObject);
        }else{
            Instance = this;
        }
    }
    // parent of the whole inventory, used to show/hide the inventory
    public GameObject inventoryMenu;

    //list of all evidence you can possibly get
    public List<Evidence> inventory;

    // references to the UI for the currently focused item
    public Image focusImage;
    public TextMeshProUGUI focusTitle;
    public TextMeshProUGUI focusDescription;

    //color for the icons that are selected or not
    public Color iconColor;
    public Color selectedColor;
    
    // the index of the currently selected icon (index in the evidenceIcons list)
    private int selectedIcon;

    // used to get a list of the evidence Icons
    public GameObject iconParent;
    // list of the evidence Icons in the UI
    private Image[] evidenceIcons;

    // Press Z to close/ press Z to present text
    public TextMeshProUGUI instructions;

    //used to get a list of the evidence Icon backgrounds
    public GameObject iconBackgroundParent;
    // list of the evidence Icon backgrounds
    private Image[] iconBackgrounds;

    // list to convert from a given inventory icon (using the index of the icon) to the evidence object contained within
    private Evidence[] iconToEvidence = {null, null, null, null, null, null, null, null};

    // Start is called before the first frame update
    void Start()
    {
        // set up evidence icon lists
        evidenceIcons = iconParent.GetComponentsInChildren<Image>();
        iconBackgrounds = iconBackgroundParent.GetComponentsInChildren<Image>();
        //set up the icon colors
        int iconMax = evidenceIcons.Length;
        // set up the colors of all of the evidence Icons
        for(int i = 0; i<iconMax; i++){
            evidenceIcons[i].color = iconColor;
            iconBackgrounds[i].color = iconColor;
            iconToEvidence[i] = null;
        }
        // update the menu to reflect the currently found items
        updateInventoryDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        // check if in the viewing inventory state
        if(InteractionManager.Instance.GetInteractionState() == InteractionManager.InteractionState.viewInventory || InteractionManager.Instance.GetInteractionState() == InteractionManager.InteractionState.presentEvidence){
            // if so, show the inventory and allow inputs
            inventoryMenu.SetActive(true);
            if(Input.GetKeyDown(KeyCode.A)){
            updateSelected(selectedIcon - 1);
            }
            if(Input.GetKeyDown(KeyCode.D)){
                updateSelected( selectedIcon + 1);
            }
            if(Input.GetKeyUp(KeyCode.Z) ){
                InteractionManager.Instance.SetToPlayerMovement();
            }
            if(InteractionManager.Instance.GetInteractionState() == InteractionManager.InteractionState.viewInventory){
                instructions.text = "";
            }else{
                instructions.text = "press Z to present this evidence";
            }
        }else{
            // if not, hide the inventory
            inventoryMenu.SetActive(false);
        }
        
    }

    // update which evidence should show up in the menu (called when opening menu)
    public void updateInventoryDisplay(){
        int iconMax = evidenceIcons.Length;
        // reset all of the evidence icons
        for(int i = 0; i<iconMax; i++){
            evidenceIcons[i].color = iconColor;
            iconBackgrounds[i].color = iconColor;
            iconToEvidence[i] = null;
        }

        // go through progress manager, and if any progress shares a name with any evidence, then make sure that evidence is marked as discorvered
        foreach(Evidence e in inventory){
            if(ProgressManager.Instance.sceneProgressList.Contains(e.name) || ProgressManager.Instance.gameProgressList.Contains(e.name)){
                e.discovered = true;
            }
        }

        // go through the evidence, and for all discovered evidence assign it an icon and update that icon
        int iconCurrentIndex = 0;
        foreach( Evidence e in inventory){
            if(iconCurrentIndex >= iconMax){
                break;
            }
            if(e.discovered){
                evidenceIcons[iconCurrentIndex].sprite = e.icon;
                evidenceIcons[iconCurrentIndex].color = Color.white;
                iconToEvidence[iconCurrentIndex] = e;
                iconCurrentIndex++;
            }
        }
        // update which icon is selected (default to 0 here when opening the menu)
        updateSelected(0);
    }

    // update which evidence is currently selected
    public void updateSelected(int num){
        // check if that number would be out of scope or if it isn't assigned an evidence yet
        if(num > 7 || num < 0 || iconToEvidence[num] == null){
            // if so do nothing and return
            return;
        }
        // otherwise, update the old selected UI to not be selected anymore
        iconBackgrounds[selectedIcon].color = iconColor;

        // update the UI to reflect the new selection
        iconBackgrounds[num].color = selectedColor;
        focusImage.sprite = iconToEvidence[num].icon;
        focusDescription.text = iconToEvidence[num].description;
        focusTitle.text = iconToEvidence[num].name;
        // keep track of the new selection
        selectedIcon = num;

        return;
    }

    // get the name of the last selected item
    public string getSelected(){
        return iconToEvidence[selectedIcon].name;
    }
}
