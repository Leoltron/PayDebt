using FluentAssertions;
using Infrastructure;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace InfrastructureTests
{
    [TestFixture]
    public class Entity_Should
    {
        [Test]
        public void Equals_ShouldReturnFalse_OnNull()
        {
            new TestEntity(0).Equals(null).Should().BeFalse();
        }

        [Test]
        public void Equals_ShouldReturnTrue_OnSelf()
        {
            var entity = new TestEntity(0);
            entity.Equals(entity).Should().BeTrue();
        }

        [Test]
        public void Equals_ShouldReturnTrue_OnOtherOfSameId()
        {
            var entity = new TestEntity(0);
            var other = new TestEntity(0);
            entity.Equals(other).Should().BeTrue();
        }

        [Test]
        public void Equals_ShouldReturnFalse_OnOtherOfDifferentId()
        {
            var entity = new TestEntity(0);
            var other = new TestEntity(1);
            entity.Equals(other).Should().BeFalse();
        }

        [Test]
        public void Equals_ShouldReturnFalse_OnObjectsOfOtherTypes()
        {
            var entity = new TestEntity(0);
            var other = new object();
            entity.Equals(other).Should().BeFalse();
        }


        [Test]
        public void GetHashCode_ShouldReturnSame_ForSameIds()
        {
            for (var id = 0; id < 10; id++)
            {
                var one = new TestEntity(id);
                var other = new TestEntity(id);
                one.GetHashCode().Should().Be(other.GetHashCode());
            }
        }

        [Test]
        public void ToString_ShouldReturnStringOfCertainFormat()
        {
            for (var id = 0; id < 10; id++)
            {
                new TestEntity(id).ToString().Should().Be($"TestEntity(Id: {id})");
            }
        }
    }

    public class TestEntity : Entity<int>
    {
        public TestEntity(int id) : base(id)
        {
        }
    }
}