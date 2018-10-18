using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Factories;
using System.ComponentModel;
using Engine.EventArgs;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;


        #region Properties
        private Monster _currentMonster;
        private Location _currentLocation;
        private Trader _currentTrader;
        private Player _currentPlayer;
        private Potion _currentPotion;
        public string Name { get; set; }

        public World CurrentWorld { get; set; }
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLevelUp -= OnCurrentPlayerLevelUp;
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                }
                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLevelUp += OnCurrentPlayerLevelUp;
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                }
            }
        }
        public Location CurrentLocation
        { get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));
                OnPropertyChanged(nameof(CurrentLocation.Description));

                GivePlayerQuestsAtLocation();
                GetMonsterAtLocation();
                CompleteQuestsAtLocation();
                CurrentTrader = CurrentLocation.TraderHere;
            }
        }



        public Monster CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                if (_currentMonster != null)
                {
                    _currentMonster.OnKilled -= OnCurrentMonsterKilled;
                }

                _currentMonster = value;

                if (_currentMonster != null)
                {
                    _currentMonster.OnKilled += OnCurrentMonsterKilled;

                    RaiseMessage("");
                    RaiseMessage($"Vidis {CurrentMonster.Name} ovdje!");
                }

                OnPropertyChanged(nameof(CurrentMonster));
                OnPropertyChanged(nameof(HasMonster));
            }
        }

        public Trader CurrentTrader
        { get { return _currentTrader; }
            set
            {
                _currentTrader = value;
                OnPropertyChanged(nameof(CurrentTrader));
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        public Weapon CurrentWeapon { get; set; }

        public Potion CurrentPotion
        { get { return _currentPotion; }
            set
            {
                _currentPotion = value;
                OnPropertyChanged(nameof(CurrentPotion));
                OnPropertyChanged(nameof(HasPotion));
            }
        }

        public bool HasLocationToNorth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;

        public bool HasLocationToWest =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;

        public bool HasLocationToEast =>
             CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;

        public bool HasLocationToSouth =>
             CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;


        public bool HasMonster => CurrentMonster != null;

        public bool HasTrader => CurrentTrader != null;

        public bool HasPotion => CurrentPotion != null;


        #endregion


        public async void CoolDown()
        {
            await Task.Delay(500);
            RaiseMessage(".");
            await Task.Delay(500);
            RaiseMessage(".");
        }

        public void CreateChar(string name)
        {

        }

        public GameSession(string characterName)
        {
            CurrentPlayer = new Player(characterName, "Hardcoded", 0, 10, 10, 1000000);

            if (!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }

            CurrentWorld = WorldFactory.CreateWorld(); // Stvori world
            CurrentLocation = CurrentWorld.LocationAt(0, 0); // Trenutna lokacija

        }


        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            }
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID &&
                                                             !q.IsCompleted);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        // Removaj quest completion iteme igracu
                        foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                        {
                            for (int i = 0; i < itemQuantity.Quantity; i++)
                            {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                            }
                        }

                        RaiseMessage("");
                        RaiseMessage($"Zavrsio si '{quest.Name}' quest");

                        // Daj playeru reward
                        RaiseMessage($"Dobio si {quest.RewardExperiencePoints} xp-a");
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);

                        RaiseMessage($"Dobio si {quest.RewardGold} golda");
                        CurrentPlayer.RecieveGold(quest.RewardGold);

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);

                            RaiseMessage($"Dobio si {rewardItem.Name}");
                            CurrentPlayer.AddItemToInventory(rewardItem);
                        }

                        // Markuj quest kao compelted
                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }


        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    RaiseMessage("");
                    RaiseMessage($"Dobio si '{quest.Name}' quest");
                    RaiseMessage(quest.Description);

                    RaiseMessage("");
                    RaiseMessage("Vrati se sa:");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage($"{itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                    RaiseMessage("");
                    RaiseMessage("Nagrade za quest:");
                    RaiseMessage($"{quest.RewardExperiencePoints} xp-a");
                    RaiseMessage($"{quest.RewardGold} golda");
                    foreach (ItemQuantity itemQuantiy in quest.RewardItems)
                    {
                        RaiseMessage($"{itemQuantiy.Quantity} {ItemFactory.CreateGameItem(itemQuantiy.ItemID).Name}");
                    }
                }
            }
        }

        public void DrinkCurrentPotion()
        {
            if (CurrentPotion == null)
            {
                RaiseMessage("Moras odabrat potion debilu");
                return;
            }
            else
            {
                if (CurrentPlayer.CurrentHitPoints == CurrentPlayer.MaximumHitPoints)
                {
                    RaiseMessage($"Pun ti je hp debilu");
                    return;
                }
                else
                {
                    RaiseMessage($"Healan si za {CurrentPotion.Heal}!");
                    CurrentPlayer.Heal(CurrentPotion.Heal);
                    CurrentPlayer.RemoveItemFromInventory(CurrentPotion);
                }
            }

        }
        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        public void AttackCurrentMonster()
        {
            if (CurrentWeapon == null)
            {
                RaiseMessage("Moras odabrat weapon da udaras");
                return;
            }

            int damageToMonster = RandomNumberGenerator.NumberBetween(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage);

            if (damageToMonster == 0)
            {
                RaiseMessage($"Promasio si {CurrentMonster.Name}.");
            }
            else
            {
                RaiseMessage("");
                RaiseMessage($"Udario si {CurrentMonster.Name} za {damageToMonster}.");
                CurrentMonster.TakeDamage(damageToMonster);
            }

            if (CurrentMonster.IsDead)
            {
                GetMonsterAtLocation();
            }
            else
            {
                int damageToPlayer = RandomNumberGenerator.NumberBetween(CurrentMonster.MinimumDamage, CurrentMonster.MaximumDamage);

                if (damageToPlayer == 0)
                {
                    RaiseMessage("Monster je promasio hehe gade srecni");
                }
                else
                {
                    RaiseMessage($"{CurrentMonster.Name} te udario za {damageToPlayer}");
                    CurrentPlayer.TakeDamage(damageToPlayer);
                }
            }
        }

        public void OnCurrentPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage($"");
            RaiseMessage($"{CurrentMonster.Name} te ubio.");
            RaiseMessage("Teleportiram te kuci");

            CurrentLocation = CurrentWorld.LocationAt(0, -1); // Vracas se kuci
            CurrentPlayer.CompletylHeal();
        }

        public void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"Pobjedio si {CurrentMonster.Name}!");

            RaiseMessage($"Dobio si {CurrentMonster.RewardExperiencePoints} xp-a!");
            CurrentPlayer.AddExperience(CurrentMonster.RewardExperiencePoints);

            RaiseMessage($"Dobio si {CurrentMonster.Gold} golda!");
            CurrentPlayer.RecieveGold(CurrentMonster.Gold);

            foreach (GameItem gameItem in CurrentMonster.Inventory)
            {
                RaiseMessage($"Dobio si jedan {gameItem.Name}.");
                CurrentPlayer.AddItemToInventory(gameItem);
            }
        }

        private void OnCurrentPlayerLevelUp(object sender, System.EventArgs e)
        {
            RaiseMessage($"Sada si level {CurrentPlayer.Level}!");
        }

        public void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }

    }
}
