using HeroesGame.Constant;
using HeroesGame.Contract;
using HeroesGame.Implementation.Hero;
using HeroesGame.Implementation.Monster;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesGame.Tests
{
    public class CombatProcessorTest
    {
        private CombatProcessor _cp;
        private IHero hero;
        [SetUp]
        public void SetUp()
        {
            this._cp = new CombatProcessor(new Hunter());
            hero = new Hunter();

        }
        [Test]
        public void InitializeCorrectly()
        {
            Assert.That(this._cp.Hero, Is.Not.Null);
            Assert.That(this._cp.Logger, Is.Not.Null);
            Assert.That(this._cp.Logger, Is.Empty);
        }
        [Test]
        public void FightCorrently_WeakerEnemy()
        {
            IMonster moster = new MedusaTheGorgon(1);
            this.LevelUp(50);

            this._cp.Fight(moster);
            var logger = this._cp.Logger;

            Assert.That(logger.Count, Is.EqualTo(8));
            Assert.That(logger, Does.Contain("The Hunter hits the MedusaTheGorgon dealing 10 damage to it.")
                .And.Contain("The MedusaTheGorgon hits the Hunter dealing 10 damage to it.")
                .And.Contain("The Hunter hits the MedusaTheGorgon dealing 10 damage to it.")
                .And.Contain("The MedusaTheGorgon hits the Hunter dealing 10 damage to it.")
                .And.Contain("The Hunter hits the MedusaTheGorgon dealing 10 damage to it.")
                .And.Contain("The MedusaTheGorgon hits the Hunter dealing 10 damage to it.")
                .And.Contain("The Hunter hits the MedusaTheGorgon dealing 10 damage to it.")
                .And.Contain("The monster dies. (4 XP gained.)"));

            //"The Hunter hits the MedusaTheGorgon dealing 10 damage to it.", 
            //    "The MedusaTheGorgon hits the Hunter dealing 10 damage to it.",
            //    "The Hunter hits the MedusaTheGorgon dealing 10 damage to it.",
            //    "The MedusaTheGorgon hits the Hunter dealing 10 damage to it.",
            //    "The Hunter hits the MedusaTheGorgon dealing 10 damage to it.",
            //    "The MedusaTheGorgon hits the Hunter dealing 10 damage to it.",
            //    "The Hunter hits the MedusaTheGorgon dealing 10 damage to it.",
            //    "The monster dies. (4 XP gained.)"
        }
        [Test]
        public void FightCorrectlyAndRepeatedly_StrongerEnemy()
        {
            IMonster monster = new MedusaTheGorgon(50);
            this._cp.Fight(monster);
            var logger = this._cp.Logger;

            Assert.That(logger, Has.Count.EqualTo(12));
            Assert.That(logger, Does.Contain("The hero dies on level 1 after 4 fights."));


        }
        private void LevelUp(int levels)
        {

            for (int i = 0; i < levels; i++)
            {
                hero.GainExperience(HeroConstants.MaximumExperience);
            }

        }
    }
}
