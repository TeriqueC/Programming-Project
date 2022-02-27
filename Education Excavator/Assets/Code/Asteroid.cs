using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EducationExcavator
{
    public class Asteroid : MonoBehaviour
    {
        private float asteriod_speed= 0.9f;
        private float asteriod_xpos;
        private float asteroid_ypos = 2f;

        public int asteroid_value;

        public static bool questionChange= false;
        
        public new Rigidbody2D rigidbody;
        //rigidbody allows physics to be aplied to gameobjects

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            move();
            //gets the component rigid body which applies physics to an object, attached to the gameobject
        }

        public int setValue(){
            bool correct = checkQuestion();
            if( correct == true){
                return asteroid_value;
            }
            return null;
        }

        public void move()
        {
            rigidbody.velocity = new Vector2(0, -1*asteriod_speed);
        }//moves asteroid downwards
        public void spawnPoint()
        {
            asteriod_xpos = Random.Range(-9f, 9f);//random spawn in any position on the x axis between the 2 numbers
            transform.position= new Vector3(asteriod_xpos, asteroid_ypos);
        }//spawns asteroid in new location once a collision has taken place

        public bool checkQuestion(){
          if(questionChange== true){
               questionChange=false;
                return true;
            }
            return false;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            spawnPoint();
            questionChange = true;
        }//manages collision between asteroid and player/boundary/bullet

    }
}