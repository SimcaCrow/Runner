using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingsController : MonoBehaviour {

	public AudioSource selectSound;
	
	private GameObject playerMenu;
	private GameObject playerText;
	private GameObject dropdownElements;
	private GameObject inputField;
	private GameObject colorPicker;

	private GameObject addPlayerScreenButton;
	private GameObject addPlayerButton;
	private GameObject setPlayerScreenButton;
	private GameObject setPlayerButton;
	private GameObject deletePlayerButton;

	private Dropdown dropdown;
	protected DrawGUIOrderColorPickerController GUIColorPickerController;

	private string defaultPlayer; 

	/** 
     *  Initialization
     */
	void Start () {
		playerMenu = GameObject.Find("Canvas").transform.Find("PlayerMenu").gameObject;		
		playerText = playerMenu.transform.Find("PlayerText").gameObject;
		dropdownElements = GameObject.Find("Canvas").transform.Find("Dropdown").gameObject;
		inputField = playerMenu.transform.Find("InputField").gameObject;
		colorPicker = playerMenu.transform.Find("ColorPicker").gameObject;
		
		addPlayerScreenButton = playerMenu.transform.Find("AddPlayerScreenButton").gameObject;
		addPlayerButton = playerMenu.transform.Find("AddPlayerButton").gameObject;
		setPlayerScreenButton = playerMenu.transform.Find("SetPlayerScreenButton").gameObject;
		setPlayerButton = playerMenu.transform.Find("SetPlayerButton").gameObject;
		deletePlayerButton = playerMenu.transform.Find("DeletePlayerButton").gameObject;
		
		dropdown = dropdownElements.GetComponent<Dropdown>();
		GUIColorPickerController = colorPicker.GetComponent<DrawGUIOrderColorPickerController>();

		defaultPlayer = "Default";
	}
	
	/** 
     * Return the default player name
     */
	public string GetDefaultPlayer(){
		return defaultPlayer;
	}

	/** 
     *	Set the player name in the PlayerPrefs
     */
	public void SetPlayerName(string name){
		PlayerPrefs.SetString("PlayerName", name);
	}
	
	/** 
     * Return True if the player is the Default Player
     */
	private bool isDefaultPlayer(string player){
		return player == defaultPlayer;
	}

	/** 
     *	Delete a player in the csv file and go back to player selection screen
     */
	public void DeletePlayer(){
		selectSound.Play();
		string player = dropdown.options[dropdown.value].text;
		if(!isDefaultPlayer(player)){
			ColorFileManager.RemovePlayer(player);
		}
		DisabledPlayersSettingsScreen();
		EnabledPlayerMenu();
	}
	
	/** 
     *	Display the player settings screen
     */
	public void SetPlayerScreen(){
		selectSound.Play();
		string player = dropdown.options[dropdown.value].text;
		playerText.GetComponent<Text>().text = player;
		Dictionary<string, Color> playerColor = ColorFileManager.FindColors(player);

        SetPlayerMenuElementState(false);
		playerText.SetActive(true);
		colorPicker.SetActive(true);
		setPlayerButton.SetActive(true);

		GUIColorPickerController.SetEmotionColors(playerColor);
	}

	/** 
     *	Set colors for the player in the csv file
     */
	public void SetPlayer(){
		selectSound.Play();
		Dictionary<string, Color> playerColor = GUIColorPickerController.GetEmotionColors();
        PlayerColor newPlayer = new PlayerColor(dropdown.options[dropdown.value].text, playerColor["Anger"], playerColor["Surprise"], playerColor["Joy"], playerColor["Sadness"]);
        ColorFileManager.EditPlayer(newPlayer);
		DisabledPlayersSettingsScreen();
		EnabledPlayerMenu();
	}
	
	/** 
     *	Display the player addition menu
     */
	public void AddPlayerScreen(){
		selectSound.Play();
		SetPlayerMenuElementState(false);
		inputField.SetActive(true);
		colorPicker.SetActive(true);
		addPlayerButton.SetActive(true);

		Dictionary<string, Color> playerColor = ColorFileManager.FindColors(defaultPlayer);
		GUIColorPickerController.SetEmotionColors(playerColor);
	}

	/** 
     *	Add a new player with his colors in the csv file
     */
	public void AddPlayer(){
		selectSound.Play();
		string player = inputField.GetComponent<InputField>().text;
		if(player.Length != 0){
			Dictionary<string, Color> playerColor = GUIColorPickerController.GetEmotionColors();
			PlayerColor newPlayer = new PlayerColor(player, playerColor["Anger"], playerColor["Surprise"], playerColor["Joy"], playerColor["Sadness"]);
			ColorFileManager.EditPlayer(newPlayer);
		}
		DisabledPlayersSettingsScreen();
		EnabledPlayerMenu();
	}
	
	/** 
     *	Put the default player at the first position of the list
     */
	private List<string> sortPlayersList(List<string> playersList){
		string temp = playersList[0];
		for (int i = 0; i < playersList.Count; i++){
			if(isDefaultPlayer(playersList[i])){
				playersList[0] = playersList[i];
				playersList[i] = temp;
				break;
			}
		}
		return playersList;
	}
	
	/** 
     *	Return the index of the default player
     */
	private int searchDefaultIndex(){
		List<Dropdown.OptionData> options = dropdown.options;
		for (int i = 0; i < options.Count; i++){
			if(isDefaultPlayer(options[i].text)){
				return i;
			}
		}
		return 0;
	}

	/** 
     *	Update the dropdown element
     */
	public void UpdateDropdown(){
		dropdown.ClearOptions();
		dropdown.AddOptions(sortPlayersList(ColorFileManager.ListOfPlayers()));
		dropdown.value = searchDefaultIndex();
	}

	/** 
     *	Change the player setting menu element state
     */
	private void SetPlayerMenuElementState(bool state){
		dropdownElements.SetActive(state);
		deletePlayerButton.SetActive(state);
		addPlayerScreenButton.SetActive(state);
		setPlayerScreenButton.SetActive(state);
	}

	/** 
     *	Disabled player setting screens elements
     */
	public void DisabledPlayersSettingsScreen(){
		playerText.SetActive(false);
		inputField.SetActive(false);
		colorPicker.SetActive(false);
		addPlayerButton.SetActive(false);
		setPlayerButton.SetActive(false);
	}
		
	/** 
     *	Enabled player settings menu
     */
	public void EnabledPlayerMenu(){
		inputField.GetComponent<InputField>().text = "";
		UpdateDropdown();
		SetPlayerMenuElementState(true);
	}

}
