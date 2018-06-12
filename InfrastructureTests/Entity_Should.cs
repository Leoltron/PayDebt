using FluentAssertions;
using Infrastructure;
using NUnit.Framework;

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
    }

    public class TestEntity : Entity<int>
    {
        public TestEntity(int id) : base(id)
        {
        }
    }
}
