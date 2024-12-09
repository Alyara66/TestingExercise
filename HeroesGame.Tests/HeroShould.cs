using HeroesGame.Constant;

using HeroesGame.Contract;
using HeroesGame.Implementation.Hero;
using HeroesGame.Implementation.Monster;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RangeAttribute = NUnit.Framework.RangeAttribute;

namespace HeroesGame.Tests
{
    public class HeroShould
    {
        [Test]
        public void HaveCorrectInitialValues()
        {
            var hero = new Mage();

            Assert.That(hero.Level, Is.EqualTo(HeroConstants.InitialLevel));
            Assert.That(hero.Experience, Is.EqualTo(HeroConstants.InitialExperience));
            Assert.That(hero.MaxHealth, Is.EqualTo(HeroConstants.InitialMaxHealth));
            Assert.That(hero.Health, Is.EqualTo(HeroConstants.InitialMaxHealth));
            Assert.That(hero.Armor, Is.EqualTo(HeroConstants.InitialArmor));
            Assert.That(hero.Weapon, Is.Not.Null);

        }

        [Test]
        public void TakeHitCorrectli()
        {
            var hero = new Warrior();

            var damage = 50;
            hero.TakeHit(damage);

            Assert.That(hero.Health, Is.EqualTo(HeroConstants.InitialMaxHealth - damage + HeroConstants.InitialArmor));
        }

       // private IHero _hero;
        private Mock<Mage> hero;

        [SetUp]
        public void Setup()
        {
            //this._hero = new Mage();
            hero = new Mock<Mage>();
            hero.Protected()
                .Setup("LevelUp")
                .CallBase();
        }
        [Test]
        public void ThrowExceptionForNegativeTakeHitValue()
        {
            var damage = -50;
            Assert.Throws<ArgumentException>(() => hero.Object.TakeHit(damage),"Damage value cannot be negative");

        }
        [Test]
        [TestCase(arg: 10)]
        [TestCase(arg: 20)]
        [TestCase(arg: 30)]

        public void TakeHitCorrectly_TestCase(int damage)
        {
            hero.Object.TakeHit(damage);

            Assert.That(hero.Object.Health, Is.EqualTo(HeroConstants.InitialMaxHealth - damage + HeroConstants.InitialArmor));
        }
        [Test]

        public void TakeHitCorrectly_Combinatorial([Values(40, 50, 60)] int damage)
        {
            hero.Object.TakeHit(damage);

            Assert.That(hero.Object.Health, Is.EqualTo(HeroConstants.InitialMaxHealth - damage + HeroConstants.InitialArmor));
        }

        [Test]
        public void TakeHitCorrectly_Range([Range(70, 100, 10)] int damage)
        {
            hero.Object.TakeHit(damage);

            Assert.That(hero.Object.Health, Is.EqualTo(HeroConstants.InitialMaxHealth - damage + HeroConstants.InitialArmor));
        }
        [Test]
        public void GainExperienceCorrectly([Range(25, 500, 25)] double xp)
        {
            hero.Object.GainExperience(xp);
            if (xp >= HeroConstants.MaximumExperience)
            {
                var expectedXp = (HeroConstants.InitialExperience + xp) % HeroConstants.MaximumExperience;
                Assert.That(hero.Object.Experience, Is.EqualTo(expectedXp));
                Assert.That(hero.Object.Level, Is.EqualTo(HeroConstants.InitialLevel + 1));
            }
            else
            {
                Assert.That(hero.Object.Experience, Is.EqualTo(HeroConstants.InitialExperience + xp));

            }

        }
        [Test]
        public void HealCorrectly([Range(5, 25, 1)] int level, [Range(25, 500, 25)] int damage)
        {
            this.LevelUp(level);
            double totalDamage = HeroConstants.InitialMaxHealth + damage;
            totalDamage = hero.Object.TakeHit(totalDamage);
            hero.Object.Heal();

            var healValue = hero.Object.Level * HeroConstants.HealPerLevel;
            var expectedHealth = (hero.Object.MaxHealth - totalDamage) + healValue;

            if (expectedHealth > hero.Object.MaxHealth)
                expectedHealth = hero.Object.MaxHealth;

            Assert.That(hero.Object.Health, Is.EqualTo(expectedHealth));
        }

        private void LevelUp(int levels)
        {

            for (int i = 0; i < levels; i++)
            {
                hero.Object.GainExperience(HeroConstants.MaximumExperience);
            }

        }
        [Test]
        public void NotBeBornDead()
        {
            var isDead = hero.Object.IsDead();
            Assert.That(isDead, Is.False);   
        }
        [Test]
        public void BeDeadWhenCriticallyHit([Range(50, 150, 25)] double damage)
        {
            damage = hero.Object.TakeHit(damage);

            if (damage >= hero.Object.MaxHealth)
            {
                Assert.That(hero.Object.IsDead);
            }
            else
            {
                Assert.That(hero.Object.IsDead, Is.False);
            }
        }
       

        [Test]
        public void TakeHitCorrectly()
        {
            var damage = 50;
            hero.Object.TakeHit(damage);

            Assert.That(hero.Object.Health, Is.EqualTo(HeroConstants.InitialMaxHealth - damage + HeroConstants.InitialArmor));
            
        }

      
    }
}  
