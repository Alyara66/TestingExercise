using HeroesGame.Constant;
using HeroesGame.Implementation.Weapon;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesGame.Tests
{
    internal class BaseWeaponShould
    {
        private Mock<BaseWeapon> _weaponMock;

        [SetUp]
        public void Setup()
        {
            // Mock базовия клас, за да се тества абстрактната функционалност.
            _weaponMock = new Mock<BaseWeapon> { CallBase = true };
        }

        [Test]
        public void HaveCorrectInitialValues()
        {
            var weapon = _weaponMock.Object;

            Assert.That(weapon.Damage, Is.EqualTo(WeaponConstants.InitialDamage));
            Assert.That(weapon.Level, Is.EqualTo(WeaponConstants.InitialLevel));
        }

        [Test]
        public void IncreaseDamageAndLevelOnLevelUp()
        {
            var weapon = _weaponMock.Object;

            weapon.LevelUp();

            Assert.That(weapon.Level, Is.EqualTo(WeaponConstants.InitialLevel + 1));
            Assert.That(weapon.Damage, Is.EqualTo(WeaponConstants.InitialDamage + WeaponConstants.DamagePerLevel));
        }

        [Test]
        public void ReturnCorrectArmorPenetrationValue_WhenOverridden()
        {
            // Симулираме конкретна реализация на ArmorPenetration.
            const int expectedPenetration = 15;
            _weaponMock.Setup(w => w.ArmorPenetration()).Returns(expectedPenetration);

            var armorPenetration = _weaponMock.Object.ArmorPenetration();

            Assert.That(armorPenetration, Is.EqualTo(expectedPenetration));
        }

        [Test]
        public void AllowDamageModification()
        {
            var weapon = _weaponMock.Object;

            weapon.Damage = 20;

            Assert.That(weapon.Damage, Is.EqualTo(20));
        }

        [Test]
        public void AllowLevelModification()
        {
            var weapon = _weaponMock.Object;

            weapon.Level = 3;

            Assert.That(weapon.Level, Is.EqualTo(3));
        }
    }
}


