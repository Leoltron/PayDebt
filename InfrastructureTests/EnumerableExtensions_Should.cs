using System;
using System.Linq;
using FluentAssertions;
using Infrastructure;
using NUnit.Framework;

namespace InfrastructureTests
{
    [TestFixture]
    public class EnumerableExtensions_Should
    {
        [Test]
        public void GetAllStartsWith_ShouldAcceptAllByEmptyPrefix()
        {
            var strings = new[] {"string1", "other string", "!@$##%^some more"};

            strings.GetAllStartsWith("").Should().BeEquivalentTo(strings);
        }

        [Test]
        public void GetAllStartsWith_ShouldKeepOrder()
        {
            var strings = new[] {"string1", "other string", "!@$##%^some more"};

            strings.GetAllStartsWith("").Should().ContainInOrder(strings);
        }

        [Test]
        public void GetAllStartsWith_ShouldKeepElementsWithGivenPrefix()
        {
            var strings = new[] {"string1", "str - other string", "s t r", "!@$##%^some more"};

            strings.GetAllStartsWith("str").Should().Contain(strings.Take(2));
        }

        [Test]
        public void GetAllStartsWith_ShouldSkipElementsWithoutGivenPrefix()
        {
            var strings = new[] {"string1", "str - other string", "s t r", "!@$##%^some more"};

            strings.GetAllStartsWith("str").Should().NotContain(strings.Skip(2));
        }

        [Test]
        public void GetAllStartsWith_ShouldIgnoreCaseBeDefault()
        {
            var strings = new[] {"sTring1", "StR - other string", "s t r", "!@$##%^some more"};

            strings.GetAllStartsWith("str").Should().Contain(strings.Take(2));
        }

        [Test]
        public void GetAllStartsWith_ShouldConsiderCulture()
        {
            var strings = new[] {"sTring1", "stR - other string", "s t r", "!@$##%^some more"};

            strings.GetAllStartsWith("stR", StringComparison.InvariantCulture)
                .Should().Contain(strings.Skip(1).Take(1));
        }

        [Test]
        public void GetAllNonEmpty_ShouldKeepNonEmpty()
        {
            var strings = new[] {null, "non empty", "", "  \t\r   ", "other"};
            var expected = new[] {"non empty", "other"};

            strings.GetAllNonEmpty().Should().Contain(expected);
        }

        [Test]
        public void GetAllNonEmpty_ShouldNotKeepNull()
        {
            var strings = new[] {null, "non empty", "", "  \t\r   ", "other"};

            strings.GetAllNonEmpty().Should().NotContain(new string[]{null});
        }

        [Test]
        public void GetAllNonEmpty_ShouldNotKeepEmpty()
        {
            var strings = new[] {null, "non empty", "", "  \t\r   ", "other"};

            strings.GetAllNonEmpty().Should().NotContain("");
        }

        [Test]
        public void GetAllNonEmpty_ShouldNotKeepWhiteSpaceOnly()
        {
            var strings = new[] {null, "non empty", "", "  \t\r   ", "other"};

            strings.GetAllNonEmpty().Should().NotContain("  \t\r   ");
        }
    }
}