using System.Linq;
using FluentAssertions;
using Infrastructure;
using NUnit.Framework;

namespace InfrastructureTests
{
    [TestFixture]
    public class StaticStorage_Should
    {
        [Test]
        public void Constructors_ShouldBeProtectedOrPrivate()
        {
            typeof(StaticStorage<object, object>)
                .GetConstructors()
                .Where(c => c.IsPublic)
                .Should().BeEmpty();
        }

        [Test]
        public void All_ShouldNotContainPrivateFieldsValues()
        {
            TestStorage.All.Should().NotContain(0);
        }

        [Test]
        public void All_ShouldNotContainProtectedFieldsValues()
        {
            TestStorage.All.Should().NotContain(1);
        }

        [Test]
        public void All_ShouldContainPublicFieldsValues()
        {
            TestStorage.All.Should().Contain(new[] {2, 3, 4});
        }

        [Test]
        public void All_ShouldNotContainNonStaticFieldsValues()
        {
            TestStorage.All.Should().NotContain(new[] {5});
        }
    }

    public class TestStorage : StaticStorage<int, TestStorage>
    {
        private static int Private = 0;
        protected static int Protected = 1;
        public static int Collected1 = 2;
        public static int Collected2 = 3;
        public static int Collected3 = 4;

        public int NonStatic = 5;
    }
}