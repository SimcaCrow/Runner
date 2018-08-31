using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsController : MonoBehaviour
{
    public Text coinsText;
    public AudioSource coinsSound;
    private int coinsNumber;
    private int coinValue;

    /** 
     *  Initialization
     */
    void Start()
    {
        coinsNumber = 0;
        coinValue = 1;
        DisplayCoins();
    }

    /** 
     *	Display the number of coin on screen 
	 */
    private void DisplayCoins()
    {
        coinsText.text = coinsNumber.ToString();
    }

    /** 
     *  Return the number of coins
     */
    public int GetCoinsNumber()
    {
        return coinsNumber;
    }

    /** 
     *  Return the value of a coin
     */
    public int GetCoinValue()
    {
        return coinValue;
    }

    /** 
     *  Return the score within coins number
     */
    public int CoinsScoring()
    {
        return coinsNumber * coinValue;
    }

    /** 
     *  Play action related to the coin collection
     */
    public void CollectCoin(GameObject coin)
    {
        coinsSound.Play();
        Destroy(coin);
        coinsNumber++;
        DisplayCoins();
    }
}
