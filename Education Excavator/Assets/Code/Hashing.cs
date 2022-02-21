using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;
using System.Data;

namespace EducationExcavator
{
    class Hashing
    {
        string dbName = "URI=file:C:Education database - Copy.db";
        string sql;

        int hashValue = 0;
        int hashedValue;
        public int counter;

        public int hash(string word)
        {
            int length = word.Length;
            for(int i = 0; i < length; i = i+1)
            {
                char wordChar = word[i];
                int charValue = Convert.ToInt32(wordChar);
                hashValue = hashValue + charValue;
            }

            int finalValue = hashValue % 51;
            return finalValue;
        }

        public bool checkLength(string word)
        {
            bool size;
            int length = word.Length;
            if (length > 8){
                size = false;
                return size;
            }
           size = true;
           return size;
        }

        public bool[] insertDetails(string userName, string password)
        {
            setCounter();
            bool len = checkLength(password);
            bool userCollision = usernameCollision(userName);
            bool[] confirm = new bool[2];
            if(len == true && userCollision==false)
            {
                hashedValue = hash(password);
                bool collision = passwordCollision(hashedValue);
                if (collision == false)
                {
                    SqliteConnection connection = new SqliteConnection(dbName);
                    connection.Open();
                    SqliteCommand Command = connection.CreateCommand();

                    sql = "INSERT INTO Users(player_name, hashed_password) VALUES('" + userName + "', '" + hashedValue + "')";

                    Command.CommandText = sql;
                    Command.ExecuteNonQuery();
                    connection.Close();
                    confirm[0] = true;
                    confirm[1] = true;
                    return confirm;
                }
                confirm[0]= true;
                confirm[1] = false;
                return confirm;
            }
            confirm[0]= false;
            confirm[1] = false;
            return confirm;         
        }

        public bool CheckDetails(string userName, string password)
        {
           setCounter();
           int code = hash(password);
           if (usernameCollision(userName) == true)
           {
                SqliteConnection connection = new SqliteConnection(dbName);
                connection.Open();
                SqliteCommand Command = connection.CreateCommand();

                sql = "SELECT hashed_password FROM Users WHERE player_name ='"+userName+"'";

                Command.CommandText = sql;
                SqliteDataReader reader = Command.ExecuteReader();
                reader.Read();
                int currentPassword = reader.GetInt32(0);
                if(currentPassword == code)
                {
                    return true;
                }
                return false;
           }
           return false;
        }
        
         public int retrieveId(string username)
        {
            SqliteConnection connection = new SqliteConnection(dbName);
            connection.Open();
            SqliteCommand Command = connection.CreateCommand();

            sql = "SELECT player_id FROM Users WHERE player_name ='" + username + "'";

            Command.CommandText = sql;
            SqliteDataReader reader = Command.ExecuteReader();
            reader.Read();
            int playerId = reader.GetInt32(0);
            return playerId;
        }

        public void setCounter()
        {
            SqliteConnection connection = new SqliteConnection(dbName);
            connection.Open();
            SqliteCommand Command = connection.CreateCommand();

            sql = "SELECT COUNT(player_id) FROM Users";

            Command.CommandText = sql;
            SqliteDataReader reader = Command.ExecuteReader();
            reader.Read();
            counter = reader.GetInt32(0);
            connection.Close();
        }
        public bool usernameCollision(string newUsername)
        {
            bool collision = false;
            for (int i = 1; i <= counter; i++)
            {
                SqliteConnection connection = new SqliteConnection(dbName);
                connection.Open();
                SqliteCommand Command = connection.CreateCommand();

                sql = "SELECT player_name FROM Users WHERE player_id =" + i + "";

                Command.CommandText = sql;
                SqliteDataReader reader = Command.ExecuteReader();
                reader.Read();
                string currentUsername = reader.GetString(0);

                if (newUsername == currentUsername)
                {
                    collision = true;
                    connection.Close();
                }
                connection.Close();
            }
            return collision;
        }

        public bool passwordCollision(int newPassword)
        {
            bool collision = false;
            for (int i = 1; i <= counter; i++)
            {
                SqliteConnection connection = new SqliteConnection(dbName);
                connection.Open();
                SqliteCommand Command = connection.CreateCommand();

                sql = "SELECT hashed_password FROM Users WHERE player_id ="+i+"";

                Command.CommandText = sql;
                SqliteDataReader reader = Command.ExecuteReader();
                reader.Read();
                int currentPassword = reader.GetInt32(0);

                if (newPassword == currentPassword)
                {
                    collision = true;
                    connection.Close();
                }
                connection.Close();
            }
            return collision;
        }
    }
}
