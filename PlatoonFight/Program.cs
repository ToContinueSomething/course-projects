using System;
using System.Collections.Generic;

namespace PlatoonsFight
{
    class Program
    {
        static void Main(string[] args)
        {
            Battlefield battlefield = new Battlefield();
            battlefield.FightPlatoons();
        }
    }

    class Battlefield
    {
        private Platoon _firstPlatoon;
        private Platoon _secondPlatoon;

        public Battlefield()
        {
            _firstPlatoon = new Platoon();
            _secondPlatoon = new Platoon();
        }

        public void FightPlatoons()
        {
            Console.WriteLine("Бой!\n");

            while (_firstPlatoon.IsEmpty == false && _secondPlatoon.IsEmpty == false)
            {
                FightSoldiers();
            }

            ShowFightTotals();
        }

        private void ShowFightTotals()
        {
            if (_firstPlatoon.IsEmpty && _secondPlatoon.IsEmpty)
                Console.WriteLine("\nОба взвода пали!");
            else if (_firstPlatoon.IsEmpty)
                Console.WriteLine("\nПобедил второй взвод!");
            else if (_secondPlatoon.IsEmpty)
                Console.WriteLine("\nПобедил первый взвод!");
        }

        private void FightSoldiers()
        {
            while (_firstPlatoon.Soldier.IsDead == false && _secondPlatoon.Soldier.IsDead == false)
            {
                _firstPlatoon.Soldier.TakeDamage(_firstPlatoon.Soldier.Damage);
                _secondPlatoon.Soldier.TakeDamage(_secondPlatoon.Soldier.Damage);
            }

            ShowDeadSoldiers();

            _firstPlatoon.TryRemoveSoldier();
            _secondPlatoon.TryRemoveSoldier();
        }

        private void ShowDeadSoldiers()
        {
            if (_firstPlatoon.Soldier.IsDead && _secondPlatoon.Soldier.IsDead)
                Console.WriteLine("Погибли оба солдата");
            else if (_firstPlatoon.Soldier.IsDead)        
                Console.WriteLine("Погиб солдат первого взвода");        
            else if (_secondPlatoon.Soldier.IsDead)      
                Console.WriteLine("Погиб солдат второго взвода");       
        }
    }

    class Platoon
    {
        private List<Soldier> _soldiers;

        public Soldier Soldier => _soldiers[0];
        public bool IsEmpty => _soldiers.Count <= 0;

        public Platoon()
        {
            _soldiers = new List<Soldier>();
            int countSoldiers = 5;
            CreateNewSoldiers(countSoldiers);
        }

        public bool TryRemoveSoldier()
        {
            if (Soldier.IsDead)
            {
                _soldiers.Remove(Soldier);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CreateNewSoldiers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _soldiers.Add(new Soldier());
            }
        }
    }

    class Soldier
    {
        private static Random _random;
        private int _health;
        private int _armor;

        public bool IsDead => _health <= 0;
        public int Damage { get; private set; }

        static Soldier()
        {
            _random = new Random();
        }

        public Soldier()
        {
            int minValueHealth = 100;
            int maxValueHealth = 200;

            int minValueArmor = 25;
            int maxValueArmor = 30;

            int minValueDamage = 30;
            int maxValueDamage = 50;

            _health = _random.Next(minValueHealth, maxValueHealth);
            _armor = _random.Next(minValueArmor, maxValueArmor);
            Damage = _random.Next(minValueDamage, maxValueDamage);
        }

        public void TakeDamage(int damage)
        {
            _health -= Math.Abs(damage - _armor);

            int minArmor = 0;

            if (_armor <= minArmor)
                _armor = minArmor;
            else
                _armor -= damage;
        }
    }
}
