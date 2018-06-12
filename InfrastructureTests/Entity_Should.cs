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
        public void Equals_ShouldReturnFalseOnNull()
        {
            new TestEntity(0).Equals(null).Should().BeFalse();
        }

        [Test]
        public void Equals_ShouldReturnTrueOnSelf()
        {
            var entity = new TestEntity(0);
            entity.Equals(entity).Should().BeTrue();
        }

        [Test]
        public void Equals_ShouldReturnTrueOnOtherOfSameId()
        {
            var entity = new TestEntity(0);
            var other = new TestEntity(0);
            entity.Equals(other).Should().BeTrue();
        }

        [Test]
        public void Equals_ShouldReturnFalseOnOtherOfDifferentId()
        {
            var entity = new TestEntity(0);
            var other = new TestEntity(1);
            entity.Equals(other).Should().BeFalse();
        }

        [Test]
        public void Equals_ShouldReturnFalseOnObjectsOfOtherTypes()
        {
            var entity = new TestEntity(0);
            var other = new object();
            entity.Equals(other).Should().BeFalse();
        }
    }

    [TestFixture]
    public class GetHashCode_Should
    {
        [Test]
        public void GetHashCode_ShouldReturnSameForSameIds()
        {
            for (var id = 0; id < 10; id++)
            {
                var one = new TestEntity(id);
                var other = new TestEntity(id);
                one.GetHashCode().Should().Be(other.GetHashCode());   
            }
        }
    }

    [TestFixture]
    public class ToString_Should
    {
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
