using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI locationNameDisplay;
    public TextMeshProUGUI locationDescriptionDisplay;
    public Location startingLocation;
    

    public Location currentLocation;

    public GameObject NorthButton;
    public GameObject EastButton;
    public GameObject SouthButton;
    public GameObject WestButton;

    public GameObject fireplace;
    public GameObject desk;
    public GameObject bath;
    public GameObject cabinent;
    public GameObject crack;
    public GameObject frontdoor;
    
    public GameManager instance;
    public bool hasPliers;
    public bool isCut;
    public bool hasWater;
    public bool hasFlashlight;
    public bool hasKey;

    public GameObject locationButton;
    
    // A dictionary to represent what items they have.
    private Dictionary<string, string> itemsOwned = new Dictionary<string, string>();
    
    //A dictionary that matches the location to buttons
    //***This ended up not being possible because I can't assign the game object button to the location
    //private Dictionary<Location, GameObject> locationButtons = new Dictionary<Location, GameObject>();
    
    // A dictionary that connections the location to the location's button text (Ex. "There's a fireplace here but I cant get to it.")
    //***this ended up not being totally necessary because im using scritable objects already
    private Dictionary<Location, string> buttonTexts = new Dictionary<Location, string>();
    
    // A dictionary that connections the location to the location's item's text (Ex. "Take Key?")
    //***this ended up not being totally necessary because im using scritable objects already
    private Dictionary<Location, string> takeItemTexts = new Dictionary<Location, string>();
    
    // A dictionary that connections the location to the location's hidden feature text (Ex. "FIREPLACE UNLOCKED")
    //***this ended up not being totally necessary because im using scritable objects already
    private Dictionary<Location, string> lockedButtonTexts = new Dictionary<Location, string>();
   
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (instance == null)
        {
            instance  = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
        //changes in code you make to SO persist after play mode
        Debug.Log("Current location:" + startingLocation);
        
       // locationNameDisplay.text = startingLocation.name;
       // locationDescriptionDisplay.text = startingLocation.description;
        
        startingLocation.UpdateLocationDisplay(this);
        currentLocation = startingLocation;
        AddLocation();

    }


    //key for which int matches with which direction (parameters for button to act upon)
    //North = 0
    //E = 1
    //W = 2
    //S = 3
    //LOCKED = 4
    //LOCKED2 = 5

    //what happens when you hit a direction button
    public void MoveDirection(int direction)
    {
       
        //ShowLocationButton(); 
        if (direction == 0) //north
        {
            //sets the south location of this location's north location to the current position
            currentLocation.northLocation.southLocation = currentLocation; 
            currentLocation = currentLocation.northLocation;
            
        }

        if (direction == 1) //east
        {
            currentLocation.eastLocation.westLocation = currentLocation;
            currentLocation = currentLocation.eastLocation;
        }

        if (direction == 2) //west
        {
            currentLocation.westLocation.eastLocation = currentLocation;
            currentLocation = currentLocation.westLocation;
        }
        if (direction == 3) //south
        {
            currentLocation.southLocation.northLocation = currentLocation;
            currentLocation = currentLocation.southLocation;
        }
        ReplaceLocation(); //replaces the current location set at start to the new location
        currentLocation.UpdateLocationDisplay(this); 
        currentLocation.ChangeCameraColor(); //calls on location to change the camera color based on the current location's values
        
    }
    
    //____________________BACKGROUND COLOR______________________________
    public void ChangeBackgroundColor() //calls a function in Location script that updates the background color
    {
        currentLocation.ChangeCameraColor();
    }
    
   
    //____________________DICTIONARY LOGIC________________________________
    
   
    
    
    //THIS IS REDUNDANT SINCE I DO THIS IN LOCATION
    // public void ShowLocationButton()
    // {
    //     
    //         if (currentLocation.isLivingroom)
    //         {
    //             fireplace.SetActive(true); //I tried to find a way to make the buttons part of a dictionary but I might have to list them out
    //         }
    //     
    // }
    
    public void AddLocation()
    {
        takeItemTexts.Add(currentLocation, currentLocation.takeItemText);
        buttonTexts.Add(currentLocation, currentLocation.buttonText);
    }

    public void ReplaceLocation()
    {
        takeItemTexts.Clear();
        buttonTexts.Clear();
        takeItemTexts.Add(currentLocation, currentLocation.takeItemText);
        buttonTexts.Add(currentLocation, currentLocation.buttonText);
    }
    
    //______________________BUTTON LOGIC________________________________________


    //player clicks button
    public void ButtonClick(string itemName)
    {
        if (ButtonLogic.isLocked)
        {
            UnlockButtonText(itemName);
        }
        else if (ButtonLogic.needsItem)
        {
            UnlockItem();
        }
        else
        {
            
        }
        
    }
    
    public void UnlockButtonText(string itemName)
    {
        bool hasItem = itemsOwned.ContainsKey(itemName);
        if (hasItem)
        {
            locationDescriptionDisplay.text = lockedButtonTexts[currentLocation];
        }
    }

    public void TakeItemButton(string itemName)
    {
        TakeItem(itemName);
    }
    
    //calls grabitem to add item to dictionary
    public void TakeItem(string itemName) 
        //***this as a separate function probably wasn't necessary, but maybe it would help if the itemText changes
    {
        //calls grab item and sets the parameter itemText to You Have:
        GrabItem(itemName, "You have: "); 
    } 
    
    //adds the item and its text to the dictionary
    public void GrabItem(string itemName,  string itemText) //adds
    {
        itemsOwned.Add(itemName, itemText); 
        //checks the item
        Debug.Log(itemName); 
        //changes the description display to "You Have: <item>"
        locationDescriptionDisplay.text = itemText + itemName; 
        // Debug.Log("You have" + itemName);
    }
    
    
    //What happens when the player clicks the UseItem button
    public void UseItemButton(string itemName)
    {
        //checks if we have that item
        bool hasItem = itemsOwned.ContainsKey(itemName);
        if (hasItem)
        {
            UnlockItem();
        }

        else
        {
            
        }
        
    }
    

    //THIS ONE DOES USE DICTIONARY LOGIC
    public void UnlockItem()
    {
        locationDescriptionDisplay.text = takeItemTexts[currentLocation];
        //TODO: ACTIVE YES OR NO BUTTON
    }

    


   
    

   

    // public void ShowFireText() //calls a function in Location script that updates the text
    // {
    //     //currentLocation.ActivateFirePlace(this);
    // }
    //
    // public void ShowDeskText() //calls a function in Location script that updates the text
    // {
    //     // currentLocation.ActivateDesk(this);
    //     // hasPliers = true;
    // }
    //
    // public void ShowWaterText() //calls a function in Location script that updates the text
    // {
    //     // currentLocation.ActivateWater(this);
    //     // if (isCut)
    //     // {
    //     // hasWater = true;
    //     // }
    // }
    //
    // public void ShowCabinentText() //calls a function in Location script that updates the text
    // {
    //     // currentLocation.ActivateCabinent(this);
    //     // if (hasKey) //checks if the player got the key
    //     // {
    //     //     hasFlashlight = true; //marks that the player now has a flashlight
    //     // }
    // }
    //
    // public void ShowCrackText()
    // {
    //     // if (hasFlashlight)
    //     // {
    //     //     currentLocation.ActivateCrack(this);
    //     // }
    //     //
    //     
    //
    // }
    //
    //
    // public void ShowDoorText()
    // {
    //   
    //     // currentLocation.ActivateDoor(this);
    //         
    // }
}
