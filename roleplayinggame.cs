using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the RPG Game!");

        do
        {
            // Create a new character
            Character player = CreateCharacter();

            // Create enemies
            List<Enemy> enemies = CreateEnemies();

            // Game loop
            while (true)
            {
                // Player's turn
                Console.WriteLine("\nPlayer's Turn:");
                player.TakeTurn(enemies);

                // Check if all enemies are defeated
                if (enemies.Count == 0)
                {
                    Console.WriteLine("Congratulations! All enemies defeated. You win!");
                    break;
                }

                // Enemies' turn
                Console.WriteLine("\nEnemies' Turn:");
                enemies.ForEach(enemy => enemy.TakeTurn(player));

                // Check if the player is defeated
                if (player.Health <= 0)
                {
                    Console.WriteLine("Game Over! You were defeated.");
                    break;
                }
            }

            Console.Write("Do you want to restart the game? (yes/no): ");
        } while (Console.ReadLine().ToLower() == "yes");

        Console.WriteLine("Thanks for playing!");
    }

    static Character CreateCharacter()
    {
        Console.WriteLine("\nChoose your job:");
        Console.WriteLine("1. Archer");
        Console.WriteLine("2. Warrior");
        Console.WriteLine("3. Mage");
        Console.WriteLine("4. Assassin");

        int jobChoice = GetNumberInput(1, 4);
        string[] jobNames = { "Archer", "Warrior", "Mage", "Assassin" };
        string selectedJob = jobNames[jobChoice - 1];

        Console.Write($"Enter the name for your {selectedJob}: ");
        string playerName = Console.ReadLine();

        // Set initial stats based on the chosen job
        int initialHealth, initialDamage, initialSpeed;
        switch (selectedJob)
        {
            case "Archer":
                initialHealth = 80;
                initialDamage = 25;
                initialSpeed = 30;
                break;
            case "Warrior":
                initialHealth = 100;
                initialDamage = 20;
                initialSpeed = 20;
                break;
            case "Mage":
                initialHealth = 60;
                initialDamage = 30;
                initialSpeed = 25;
                break;
            case "Assassin":
                initialHealth = 70;
                initialDamage = 35;
                initialSpeed = 35;
                break;
            default:
                throw new InvalidOperationException("Invalid job selection.");
        }

        return new Character(playerName, selectedJob, initialHealth, initialDamage, initialSpeed);
    }

    static List<Enemy> CreateEnemies()
    {
        List<Enemy> enemies = new List<Enemy>
        {
            new Enemy("Goblin", 30, 15, 25),
            new Enemy("Skeleton", 40, 20, 20),
            new Enemy("Orc", 50, 25, 15),
            new Enemy("Dark Mage", 60, 30, 20),
            new Enemy("Dragon Boss", 100, 40, 30)
        };

        return enemies;
    }

    static int GetNumberInput(int minValue, int maxValue)
    {
        int userInput;
        while (true)
        {
            Console.Write($"Enter a number ({minValue}-{maxValue}): ");
            if (int.TryParse(Console.ReadLine(), out userInput) && userInput >= minValue && userInput <= maxValue)
            {
                return userInput;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }
}

class Character
{
    public string Name { get; private set; }
    public string Job { get; private set; }
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public int Speed { get; private set; }
    public int Exp { get; set; } // Set accessor made public

    public Character(string name, string job, int health, int damage, int speed)
    {
        Name = name;
        Job = job;
        Health = health;
        Damage = damage;
        Speed = speed;
        Exp = 0;
    }

    public void TakeTurn(List<Enemy> enemies)
    {
        Console.WriteLine($"{Name}'s turn:");

        // Sort enemies by speed in descending order
        enemies.Sort((a, b) => b.Speed.CompareTo(a.Speed));

        // Attack the enemy with the highest speed
        Enemy targetEnemy = enemies[0];
        Console.WriteLine($"Attacking {targetEnemy.Name}!");
        targetEnemy.TakeDamage(Damage, this);

        // Check if the enemy is defeated
        if (targetEnemy.Health <= 0)
        {
            Exp += 10;
            LevelUpIfNecessary();
            enemies.Remove(targetEnemy);
            Console.WriteLine($"You defeated {targetEnemy.Name}! Gained 10 EXP.");
        }
    }

    public void TakeDamage(int damage, Enemy enemy)
    {
        Health -= damage;
        Console.WriteLine($"{Name} took {damage} damage from {enemy.Name}. Remaining health: {Health}");

        // Check if the player is defeated
        if (Health <= 0)
        {
            Console.WriteLine($"{Name} was defeated by {enemy.Name}!");
        }
    }

    public void LevelUpIfNecessary() // Method made public
    {
        if (Exp >= 30)
        {
            Console.WriteLine("Level Up! Gained 2 points to all stats.");
            Health += 2;
            Damage += 2;
            Speed += 2;
            Exp = 0;
        }
    }
}

class Enemy
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public int Speed { get; private set; }

    public Enemy(string name, int health, int damage, int speed)
    {
        Name = name;
        Health = health;
        Damage = damage;
        Speed = speed;
    }

    public void TakeTurn(Character player)
    {
        Console.WriteLine($"{Name}'s turn:");

        Console.WriteLine($"Attacking {player.Name}!");
        player.TakeDamage(Damage, this);

        // Check if the player is defeated
        if (player.Health <= 0)
        {
            Console.WriteLine($"{player.Name} was defeated by {Name}!");
        }
    }

    public void TakeDamage(int damage, Character player)
    {
        Health -= damage;
        Console.WriteLine($"{Name} took {damage} damage. Remaining health: {Health}");

        // Check if the enemy is defeated
        if (Health <= 0)
        {
            player.Exp += 10;
            player.LevelUpIfNecessary();
            Console.WriteLine($"You defeated {Name}! Gained 10 EXP.");
        }
    }
}
