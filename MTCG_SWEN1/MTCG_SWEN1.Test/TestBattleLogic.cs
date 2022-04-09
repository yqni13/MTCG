using MTCG_SWEN1.Models.Cards;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.Models;

namespace MTCG_SWEN1.Test
{    
    public class TestBattleLogic
    {
        private Card _userCard1;
        private Card _userCard2;
        private User _user1;
        private User _user2;

        [SetUp]
        public void Setup()
        {
            _userCard1 = new(Guid.NewGuid(), "WaterGoblin", Guid.NewGuid(), 30.0, "Monster", "Water");
            _userCard2 = new(Guid.NewGuid(), "FireSpell", Guid.NewGuid(), 80.0, "Spell", "Fire");
            _user1 = new(Guid.NewGuid(), "lukas", "varga", 20, 100, "if20b167", ":-)", 0, 0, 0);
            _user2 = new(Guid.NewGuid(), "sophie", "arcade", 20, 100, "angestellt", ";-)", 0, 0, 0);

        }

        [Test]
        public void Test_DamageAdvantageWaterMonsterVSFireSpell()
        {
            var cardDamages = BattleService.CalculateDamage(_userCard1, _userCard2);
            var damage1 = cardDamages[0];
            var damage2 = cardDamages[1];
            Assert.AreEqual(_userCard1.Damage * 2, damage1);
            Assert.AreEqual(_userCard2.Damage / 2, damage2);
        }

        [Test]
        public void Test_DamageDisadvantageRegularMonsterVSFireSpell()
        {
            _userCard1.ElementType = "Regular";
            _userCard1.Name = "Ork";

            var cardDamages = BattleService.CalculateDamage(_userCard2, _userCard1);
            var damage1 = cardDamages[0];
            var damage2 = cardDamages[1];
            Assert.AreEqual(_userCard2.Damage * 2, damage1);
            Assert.AreEqual(_userCard1.Damage / 2, damage2);
        }

        [Test]
        public void Test_DamageMonsterVSMonster()
        {
            _userCard1.Name = "Kraken";
            _userCard2.CardType = "Monster";
            _userCard2.ElementType = "Regular";

            var cardDamages = BattleService.CalculateDamage(_userCard1, _userCard2);
            var damage1 = cardDamages[0];
            var damage2 = cardDamages[1];
            Assert.AreEqual(_userCard1.Damage, damage1);
            Assert.AreEqual(_userCard2.Damage, damage2);
        }

        [Test]
        public void Test_DamageSpellImmunity()
        {
            _userCard1.Name = "Kraken";
            _userCard1.ElementType = "Regular";

            var cardDamages = BattleService.CalculateDamage(_userCard1, _userCard2);
            var damage1 = cardDamages[0];
            var damage2 = cardDamages[1];
            Assert.AreEqual(_userCard1.Damage, damage1);
            Assert.AreEqual(0.0, damage2);
        }

        [Test]
        public void Test_DamageSpellVSSpell()
        {
            _userCard1.Name = "RegularSpell";
            _userCard1.CardType = "Spell";
            _userCard1.ElementType = "Regular";

            _userCard2.Name = "WaterSpell";
            _userCard2.ElementType = "Water";

            var cardDamages = BattleService.CalculateDamage(_userCard1, _userCard2);
            var damage1 = cardDamages[0];
            var damage2 = cardDamages[1];
            Assert.AreEqual(_userCard1.Damage * 2, damage1);
            Assert.AreEqual(_userCard2.Damage / 2, damage2);
        }

        [Test]
        public void Test_DamageHistoryEffectMonsterVSMonster()
        {
            _userCard1.Damage = 80.0;

            _userCard2.Name = "Dragon";
            _userCard2.ElementType = "Regular";
            _userCard2.CardType = "Monster";
            _userCard2.Damage = 40.0;

            var cardDamages = BattleService.CalculateDamage(_userCard1, _userCard2);
            var damage1 = cardDamages[0];
            var damage2 = cardDamages[1];
            Assert.AreEqual(0.0, damage1);
            Assert.AreEqual(_userCard2.Damage, damage2);
        }

        [Test]
        public void Test_DamageDisadvantageKnight()
        {
            _userCard1.Name = "Knight";
            _userCard1.ElementType = "Regular";
            _userCard2.Name = "WaterSpell";
            _userCard2.ElementType = "Water";

            var cardDamages = BattleService.CalculateDamage(_userCard1, _userCard2);
            var damage1 = cardDamages[0];
            var damage2 = cardDamages[1];
            Assert.AreEqual(0.0, damage1);
            Assert.AreEqual(_userCard2.Damage, damage2);
        }

        [Test]
        public void Test_DamageDisadvantageDragonByHistory()
        {
            _userCard1.Name = "Dragon";
            _userCard1.Damage = 90.0;
            _userCard1.ElementType = "Regular";
            _userCard2.Name = "FireElves";
            _userCard2.CardType = "Monster";
            _userCard2.Damage = 15.0;

            var cardDamages = BattleService.CalculateDamage(_userCard1, _userCard2);
            var damage1 = cardDamages[0];
            var damage2 = cardDamages[1];
            Assert.AreEqual(0.0, damage1);
            Assert.AreEqual(_userCard2.Damage, damage2);
        }

        [Test]
        public void Test_UserStatsCalculationAfterWin()
        {
            var eloUser1 = _user1.ELO;
            var gamesUser1 = _user1.Games;
            var winUser1 = _user1.Wins;            

            var eloUser2 = _user2.ELO;
            var gamesUser2 = _user2.Games;
            var lossesUser2 = _user2.Losses;

            List<User> users = BattleService.CalculateUserStatsAfterWin(_user1, _user2);
            var winnerUser1 = users[0];
            var loserUser2 = users[1];

            Assert.AreEqual(eloUser1 + 3, winnerUser1.ELO);
            Assert.AreEqual(gamesUser1 + 1, winnerUser1.Games);
            Assert.AreEqual(winUser1 + 1, winnerUser1.Wins);
            Assert.AreEqual(eloUser2 - 5, loserUser2.ELO);
            Assert.AreEqual(gamesUser2 + 1, loserUser2.Games);
            Assert.AreEqual(lossesUser2 + 1, loserUser2.Losses);
        }

        [Test]
        public void Test_UserStatsCalculationAfterDraw()
        {
            var gamesUser1 = _user1.Games;
            var gamesUser2 = _user2.Games;

            List<User> users = BattleService.CalculatesUserStatsAfterDraw(_user1, _user2);
            var drawUser1 = users[0];
            var drawUser2 = users[1];

            Assert.AreEqual(gamesUser1 + 1, drawUser1.Games);
            Assert.AreEqual(gamesUser2 + 1, drawUser2.Games);            
        }


    }
}
