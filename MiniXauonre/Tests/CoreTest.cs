using MiniXauonre.Controller;
using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using NUnit.Framework;

namespace MiniXauonre.Tests
{
    [TestFixture]
    public class CoreTest
    {
        [Test]
        public void HeroHPManipulations()
        {
            var hero = new Hero();
            Assert.AreEqual(hero.GetHp(), hero.GetMaxHp());
            hero.SetMaxHp(100);
            hero.SetHp(hero.GetMaxHp());
            Assert.AreEqual(hero.GetHp(), hero.GetMaxHp());
            var bonus = 1;
            hero.AddMaxHp(bonus);
            var maxHp = hero.GetMaxHp();
            Assert.AreEqual(maxHp, hero.GetHp());
            hero.AddHp(-bonus);
            Assert.AreEqual(maxHp, hero.GetHp() + bonus);
            hero.AddMaxHp(-bonus);
            Assert.AreEqual(hero.GetMaxHp(), hero.GetHp());
            hero.AddMaxHp(-bonus);
            Assert.AreEqual(hero.GetMaxHp(), hero.GetHp());
        }

        [Test]
        public void HeroEnergyManipulations()
        {
            var hero = new Hero();
            Assert.AreEqual(hero.GetEnergy(), hero.GetMaxEnergy());
            hero.SetMaxEnergy(100);
            hero.SetEnergy(hero.GetMaxEnergy());
            Assert.AreEqual(hero.GetEnergy(), hero.GetMaxEnergy());
            var bonus = 1;
            hero.AddMaxEnergy(bonus);
            var maxEnergy = hero.GetMaxEnergy();
            Assert.AreEqual(maxEnergy, hero.GetEnergy());
            hero.AddEnergy(-bonus);
            Assert.AreEqual(maxEnergy, hero.GetEnergy() + bonus);
            hero.AddMaxEnergy(-bonus);
            Assert.AreEqual(hero.GetMaxEnergy(), hero.GetEnergy());
            hero.AddMaxEnergy(-bonus);
            Assert.AreEqual(hero.GetMaxEnergy(), hero.GetEnergy());
        }

        [Test]
        public void HeroDamageGetting()
        {
            var hero = new Hero();
            var attaker = new Hero();
            var game = new Game(new GameRules());
            attaker.P = new Player(game, "player");
            var armor = 10;
            var resist = 20;
            hero.SetArmor(armor);
            hero.SetResist(resist);
            var hp = hero.GetHp();
            var physDamage = 20;
            var spellDamage = 40;
            var pureDamage = 20;
            var heal = 1000;
            var attack = new Damage(attaker, attaker.P, phys: physDamage);

            Assert.AreEqual(hero.GetHp(), hero.GetMaxHp());
            hero.GetDamage(attack);
            Assert.AreEqual(hero.GetHp(), hero.GetMaxHp() - (physDamage - armor));
            hero.GetHeal(heal);
            Assert.AreEqual(hero.GetHp(), hero.GetMaxHp());

            attack.Phys = 0;
            attack.Magic = spellDamage;
            hero.GetDamage(attack);
            Assert.AreEqual(hero.GetHp(), hero.GetMaxHp() - (spellDamage - resist));
            hero.GetHeal(heal);
            Assert.AreEqual(hero.GetHp(), hero.GetMaxHp());


            attack.Pure = pureDamage;
            attack.Magic = 0;
            hero.GetDamage(attack);
            Assert.AreEqual(hero.GetHp(), hero.GetMaxHp() - pureDamage);
            hero.GetHeal(heal);
            Assert.AreEqual(hero.GetHp(), hero.GetMaxHp());
        }

    }
}
