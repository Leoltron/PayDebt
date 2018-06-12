using System;
using FluentAssertions;
using Infrastructure;
using NUnit.Framework;

namespace InfrastructureTests
{
    [TestFixture]
    public class TypeExtensions_Should
    {
        [Test]
        public void IsEqualOrSubclassOf_ShouldReturnTrueOnSelf()
        {
            typeof(A).IsEqualOrSubclassOf(typeof(A)).Should().BeTrue();
        }
        
        [Test]
        public void IsEqualOrSubclassOf_ShouldReturnFalseOnChildren()
        {
            typeof(A).IsEqualOrSubclassOf(typeof(B)).Should().BeFalse();
        }

        [Test]
        public void IsEqualOrSubclassOf_ShouldReturnTrueOnAncestor()
        {
            typeof(B).IsEqualOrSubclassOf(typeof(A)).Should().BeTrue();
        }

        [Test]
        public void IsEqualOrSubclassOf_ShouldReturnTrueOnObjectType()
        {
            typeof(B).IsEqualOrSubclassOf(typeof(object)).Should().BeTrue();
        }
    }

    public class A
    {
    }

    public class B : A
    {
    }
}