using HeroesGame.Constant;
using HeroesGame.Contract;
using HeroesGame.Implementation.Monster;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesGame.Tests
{
    internal class BaseMonsterTest
    {
        private Mock<BaseMonster> _monsterMock;

        [SetUp]
        public void Setup()
        {
            // Mock базовия клас, за да тестваме абстрактната функционалност.
            _monsterMock = new Mock<BaseMonster>(10) { CallBase = true }; // Ниво 10 за примера
        }

        [Test]
        public void HaveCorrectInitialValues()
        {
            var monster = _monsterMock.Object;

            Assert.That(monster.Level, Is.EqualTo(10));
            Assert.That(monster.Health, Is.EqualTo(MonsterConstants.MaxHealthPerLevel * 10));
        }

        [Test]
        public void DealCorrectDamage_WhenOverridden()
        {
            const double expectedDamage = 50.0;
            _monsterMock.Setup(m => m.Damage()).Returns(expectedDamage);

            var damage = _monsterMock.Object.Damage();

            Assert.That(damage, Is.EqualTo(expectedDamage));
        }

        [Test]
        public void GiveCorrectExperience_WhenOverridden()
        {
            const double expectedExperience = 100.0;
            _monsterMock.Setup(m => m.Experience()).Returns(expectedExperience);

            var experience = _monsterMock.Object.Experience();

            Assert.That(experience, Is.EqualTo(expectedExperience));
        }

        [Test]
        public void ReturnCorrectArmorValue_WhenOverridden()
        {
            const int expectedArmor = 20;
            _monsterMock.Setup(m => m.Armor()).Returns(expectedArmor);

            var armor = _monsterMock.Object.Armor();

            Assert.That(armor, Is.EqualTo(expectedArmor));
        }

        [Test]
        public void TakeDamageCorrectly()
        {
            var weaponMock = new Mock<IWeapon>();
            weaponMock.Setup(w => w.Damage).Returns(50);
            weaponMock.Setup(w => w.ArmorPenetration()).Returns(10);
            _monsterMock.Setup(m => m.Armor()).Returns(5);

            var monster = _monsterMock.Object;
            double damageTaken = monster.TakeHit(weaponMock.Object);

            Assert.That(damageTaken, Is.EqualTo(50 + 10 - 5)); // Final damage: 55
            Assert.That(monster.Health, Is.EqualTo((MonsterConstants.MaxHealthPerLevel * 10) - 55));
        }

        [Test]
        public void HitHeroCorrectly()
        {
            var heroMock = new Mock<IHero>();
            heroMock.Setup(h => h.TakeHit(It.IsAny<double>())).Returns(30);

            _monsterMock.Setup(m => m.Damage()).Returns(30);

            var monster = _monsterMock.Object;
            double damageDealt = monster.Hit(heroMock.Object);

            Assert.That(damageDealt, Is.EqualTo(30));
            heroMock.Verify(h => h.TakeHit(30), Times.Once);
        }

        [Test]
        public void DieWhenHealthIsZeroOrLess()
        {
            var monster = _monsterMock.Object;

            monster.Health = 0;

            Assert.That(monster.IsDead(), Is.True);
        }

        [Test]
        public void StayAliveWhenHealthIsAboveZero()
        {
            var monster = _monsterMock.Object;

            monster.Health = 50;

            Assert.That(monster.IsDead(), Is.False);
        }
    }
}

