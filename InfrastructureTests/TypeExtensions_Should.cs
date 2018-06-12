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
        public void IsEqualOrSubclassOf_ShouldReturnTrue_OnSelf()
        {
            typeof(A).IsEqualOrSubclassOf(typeof(A)).Should().BeTrue();
        }
        
        [Test]
        public void IsEqualOrSubclassOf_ShouldReturnFalse_OnChildren()
        {
            typeof(A).IsEqualOrSubclassOf(typeof(B)).Should().BeFalse();
        }

        [Test]
        public void IsEqualOrSubclassOf_ShouldReturnTrue_OnAncestor()
        {
            typeof(B).IsEqualOrSubclassOf(typeof(A)).Should().BeTrue();
        }

        [Test]
        public void IsEqualOrSubclassOf_ShouldReturnTrue_OnObjectType()
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