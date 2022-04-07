using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.Models.Cards;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Test
{

    public class TestCardLogic
    {
        private Card _card1;
        private Card _card2;
        private Card _card3;
        private Card _card4;
        private Card _card5;
        private Card _card6;

        [SetUp]
        public void Setup()
        {
            _card1 = new(Guid.NewGuid(), "WaterGoblin", Guid.NewGuid(), 30.0, "Monster", "Water");
            _card2 = new(Guid.NewGuid(), "FireSpell", Guid.NewGuid(), 50.0, "Spell", "Fire");
            _card3 = new(Guid.NewGuid(), "Dragon", Guid.NewGuid(), 75.0, "Monster", "Regular");
            _card4 = new(Guid.NewGuid(), "WaterSpell", 80.0);
            _card5 = new(Guid.NewGuid(), "Ork", 40.0);
            _card6 = new(Guid.NewGuid(), "RegularSpell", 30.0);
            
        }

        [Test]
        public void Test_ValidateIfEnoughCardsToPurchasePackage()
        {
            List<Card> package = new();
            package.Add(_card1);
            package.Add(_card2);
            package.Add(_card3);            

            bool isEnough = PackageService.CheckForEnoughFreeCards(package.Count);
            Assert.IsFalse(isEnough);
        }

        [Test]
        public void Test_NewPackagesGetRealTimestampToSortOldestCards()
        {
            List<Card> package = new();
            package.Add(_card4);
            package.Add(_card5);

            var time4 = _card4.PackageTimestamp;
            var time5 = _card5.PackageTimestamp;
            
            package = PackageService.PrepareNewPackage(package);
            Assert.AreNotEqual(time4, package[0].PackageTimestamp);
            Assert.AreNotEqual(time5, package[1].PackageTimestamp);
        }

        [Test]
        public void Test_GetCorrectElementFromCardName()
        {
            var waterElement = PackageService.GetCardElementFromName(_card1.Name);
            var regularElement = PackageService.GetCardElementFromName(_card6.Name);
            Assert.AreEqual("Water", waterElement);
            Assert.AreEqual("Regular", regularElement);
        }

        [Test]
        public void Test_GetCorrectTypeFromCardName()
        {
            var monsterType = PackageService.GetCardTypeFromName(_card1.Name);
            var spellType = PackageService.GetCardTypeFromName(_card2.Name);
            Assert.AreEqual("Monster", monsterType);
            Assert.AreEqual("Spell", spellType);
        }

        [Test]
        public void Test_IfListOfCardsIsEmpty()
        {
            List<Card> cards = new();
            var isEmpty = DeckService.IsListEmpty(cards);
            Assert.IsTrue(isEmpty);
        }

        [Test]
        public void Test_IfCardsGetFilledWithProvidedGUID()
        {
            List<Card> cards = new();
            var guidType = Guid.NewGuid().GetType();
            List<String> cardIDs = new();
            cardIDs.Add("845f0dc7-37d0-426e-994e-43fc3ac83c08");
            cardIDs.Add("99f8f8dc-e25e-4a95-aa2c-782823f36e2a");
            cardIDs.Add("e85e3976-7c86-4d06-9a80-641c2019a79f");
            cardIDs.Add("1cb6ab86-bdb2-47e5-b6e4-68c5ab389334");

            cards = DeckService.PrepareCards(cardIDs);
            var id1 = cards[0].ID.GetType();
            var id2 = cards[0].ID.GetType();
            var id3 = cards[0].ID.GetType();
            var id4 = cards[0].ID.GetType();

            Assert.AreEqual(guidType, id1);
            Assert.AreEqual(guidType, id2);
            Assert.AreEqual(guidType, id3);
            Assert.AreEqual(guidType, id4);
        }


    }
}
