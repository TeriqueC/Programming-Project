using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EducationExcavator{

    public class GameController : MonoBehaviour
    {
    public Player player;
    public Asteroid asteroid;
    public GameObject canvas;
    public GameObject PauseMenu;
    public GameObject questionPopup;
    public GameObject questionBox;
    public GameObject health;

    QuestionGenerator generator = new QuestionGenerator();

    public static bool isGamePaused = false;
        // Start is called before the first frame update
        void Start()
        {
            generator.generateQuestions();
            updateQuestion();
            canvas.SetActive(false);
            player.GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            player.movement();
            player.shoot();
            checkHealth();
            bool change = asteroid.checkQuestion();
            if(change == true){
                updateQuestion();
            }
        }

        public void checkHealth(){
            string lives = player.checkLives().ToString();
            health.GetComponent<Text>().text= lives;
        }

        public void pause()
        {
            canvas.SetActive(true);
            questionPopup.SetActive(false);
            Time.timeScale= 0f;
            isGamePaused = true;
        }

        public void resume(){
            canvas.SetActive(false);
            Time.timeScale= 1f;
            isGamePaused = false;
        }

        public void updateQuestion(){
            string newQuestion = generator.updateQuestion();
            questionBox.GetComponent<Text>().text= newQuestion;
        }
    }
}