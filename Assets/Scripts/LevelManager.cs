using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    //the amount of seconds to respawn the player
    public float respawnTimer;
    //the player
    public PlayerController player;

    // Empty heart sprite.
    public Sprite heartEmpty;
    // Full heart sprite.
    public Sprite heartFull;
    // The life holder.
    public Transform lifeHolder;
    // The heart prefab.
    public Image heartPrefab;
    // The player's maximum life count.
    public int lifeCountMax;
    // The player's life count.
    public int lifeCount;

    //the sound manager
    public SoundManager sm;
    //the canvas with panel (UI)
    public Canvas canvas;
    public Image loseScreen;
    public Button playAgainBtn;
    public Text scoreTxt;
    //the health bar
    public Slider healthBar;

    //the dictionary for particle effects
    private Dict dict;
    private List<Image> heartList = new List<Image>();

    //if other classes wants to use the dictionary, return it
    public Dict Dict {
        get { return dict; }
    }

    // Use this for initialization
    void Start () {
        //find the player
        player = FindObjectOfType<PlayerController>();
        dict = this.GetComponent<Dict>();
        lifeCount = lifeCountMax;


        // Creating the life prefabs
        for (int i = 0; i < lifeCountMax; i++) {
            Image life = Instantiate<Image>(heartPrefab);
            life.rectTransform.SetParent(lifeHolder);

            float posX = 40 + 34 * i;
            float posY = -65;
            life.rectTransform.localPosition = new Vector2(posX, posY);
            life.rectTransform.localScale = Vector3.one;
            heartList.Add(life);
        }

        playAgainBtn.onClick.AddListener(ResetGame);
	}

    public void UpdateScore() {
        scoreTxt.text = string.Format("Score: {0}", player.score);
    }

    public void ResetGame() {
        SceneManager.LoadScene("Scene 1", LoadSceneMode.Single);
    }

    public void ShowLoseScreen() {
        loseScreen.gameObject.SetActive(true);
    }

    public void HideLoseScreen() {
        loseScreen.gameObject.SetActive(false);
    }

    //fade the canvas out
    public void FadeUI(float time = 0.5f) {
        //get the canvas group
        CanvasGroup cg = canvas.GetComponent<CanvasGroup>();
        //set the alpha (opacity) to 100%
        cg.alpha = 1;

        //start the fade function over duration (time)
        StartCoroutine(StartFade(cg, time));
    }

    IEnumerator StartFade(CanvasGroup cg, float time) {
        //when the canvas is still visible
        while (cg.alpha > 0) {
            //decrease the opacity over time
            cg.alpha -= Time.deltaTime / time;
            //updates per frame
            yield return null;
        }

        //tell the coroutine it has finished fading
        yield return null;
    }

    public void UpdateHealth() {
        //update the healthbar value. 1 = 100%
        healthBar.value = player.health / player.maxHealth;
    }

    public void UpdateLifeCount() {
        // Iterate through the maximum life the player has.
        // For each iteration, if the iteration is less than the life count of the player, set the iteration's sprite to full. (Default is full)
        for (int i = 0; i < lifeCountMax; i++) {
            // If the current iteration is less than the amount of life the player has, set the current sprite to full heart.
            if (i < lifeCount) {
                heartList[i].sprite = heartFull;
            } else {
                // Otherwise, set it to empty.
                heartList[i].sprite = heartEmpty;
            }
        }
    }

    public void Respawn()
    {
        if (--lifeCount > 0) {
            // Update life UI.
            UpdateLifeCount();

            // Start the respawn function.
            StartCoroutine(RespawnCo());
        } else {
            // Update life UI.
            UpdateLifeCount();

            // Start the respawn function.
            StartCoroutine(RespawnCo());

            // Show the losing screen.
            ShowLoseScreen();
        }
    }

    private IEnumerator RespawnCo()
    {
        //disable the player object
        player.gameObject.SetActive(false);

        GameObject deathSplosion = dict.particleEffects["death01"];
        //play the deathsplosion animation
        Instantiate(deathSplosion, player.transform.position, player.transform.rotation);

        //play the death sound effect
        sm.PlaySound("death01", 0.5f);

        //random death sound effect of player
        List<string> playerDeathSfx = new List<string>() {
            "player_death01",
            "player_death02",
            "player_death03",
            "player_death04"
        };
        //play a random sound in the list above
        sm.PlayRandomSound(playerDeathSfx);


        //resume the function in x seconds
        yield return new WaitForSeconds(respawnTimer);

        if (lifeCount > 0) {
            //enable the player object
            player.gameObject.SetActive(true);
            //position the player back to his respawn position
            player.transform.position = player.respawnPosition;
            //restore the player's health
            player.health = player.maxHealth;
            //update health
            this.UpdateHealth();
        }
    }

}
