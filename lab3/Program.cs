using System;
using System.Collections.Generic;

/*
 * Lab 3
 * Variant 15
 * 
 * Реалізувати ігровий простір, 
 * що подає персонажа з наступними необхідними атрибутами 
 * (позиція персонажу, склад артефактів, рівень "здоров'я" тощо). 
 * Реалізувати механізм збереження/встановлення стану персонажа.
 */

/*
 * Memento is a behavioral design pattern 
 * which makes it possible to save and restore 
 * the previous state of an object without revealing the details of its implementation.
 */

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialization:

            Player player = new Player();
            SaveController saveController = new SaveController(player);

            // Display and save starting state:

            player.GetInfo();

            saveController.SaveGame();

            // Originator's state changes and is displayed:

            player.Play();
            player.Play();

            // Restore starting state and display it:

            saveController.LoadGame();

            player.GetInfo();

        }
    }

    // Memento
    // contains the infrastructure for storing the Originator's state,
    // IMemento has properties to provide a way to retrieve the memento's metadata.
    // However, it doesn't expose the Originator's state.

    public interface IMemento
    {
        float PosX { get; }
        float PosY { get; }
        int Health { get; }
        List<string> Items { get; }
    }

    class PlayerState : IMemento
    {
        public float PosX { get; }
        public float PosY { get; }
        public int Health { get; }
        public List<string> Items { get; }

        public PlayerState(float posX, float posY, int health, List<string> items)
        {
            PosX = posX;
            PosY = posY;
            Health = health;
            Items = new List<string>(items);
        }
    }

    // Caretaker
    // doesn't depend on the concrete Memento class. Therefore, it
    // doesn't have access to the originator's state, stored inside the memento.

    class SaveController
    {
        // Stack stores player states, also allowing to pop the last saved state

        private Stack<IMemento> PlayerStates { get; }
        private Player player = null;

        public SaveController(Player player)
        {
            PlayerStates = new Stack<IMemento>();
            this.player = player;
        }

        // Provides a common method to save important states in the game
        public void SaveGame()
        {
            Console.WriteLine("\n\nSaving...\n");
            PlayerStates.Push(player.SaveState());
        }

        // Provides a common method to restore important states in the game
        public void LoadGame()
        {
            Console.WriteLine("\n\nRestoring...\n");
            player.RestoreState((PlayerState)PlayerStates.Pop());
        }
    }

    // Originator
    // holds some important state that may change over time. 
    // It also defines a method for saving the state inside a memento 
    // and another method for restoring the state from it.

    class Player
    {
        // The Originator's state is stored inside these variables

        float posX;
        float posY;
        int health;
        List<string> items;
        
        public Player()
        {
            posX = 0;
            posY = 0;
            health = 100;
            items = new List<string>()
            {
                 "item1"
                ,"item2"
            };
        }

        // Saves the current state inside a memento.

        public PlayerState SaveState()
        {
            return new PlayerState(posX, posY, health, items);
        }

        // Restores the Originator's state from a memento object.

        public void RestoreState(PlayerState playerState)
        {
            posX = playerState.PosX;
            posY = playerState.PosY;
            health = playerState.Health;
            items = new List<string>(playerState.Items);
        }

        // The Originator's business logic affects its internal state.

        public void Play()
        {
            Console.WriteLine("\nPlaying...");

            posX += 2;
            posY += 1;
            health -= 5;
            items.Add("item3");

            GetInfo();
        }

        // Display Originator's current state

        public void GetInfo()
        {
            Console.WriteLine($"{posX,5}{posY,5}{health,5}");
            foreach(string i in items)
            {
                Console.Write($"{i,10}");
            }
        }
    }

}
