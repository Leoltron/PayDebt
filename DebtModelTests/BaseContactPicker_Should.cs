using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtModel;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace InfrastructureTests
{
    [TestFixture]
    public class BaseContactPicker_Should
    {
        public Contact[] FirstProvision = {
            new Contact("Leonid"),
            new Contact("Alexander")
        };

        public Contact[] SecondProvision =  {
            new Contact("Aidar"),
            new Contact("Alexander")
        };

        private IContactProvider<Contact> Provider { get; set; }

        [SetUp]
        public void SetUp()
        {
            Provider = A.Fake<IContactProvider<Contact>>();
            A.CallTo(() => Provider.GetContactsAsync())
                .ReturnsNextFromSequence(
                    Task.FromResult<IEnumerable<Contact>>(FirstProvision), 
                    Task.FromResult<IEnumerable<Contact>>(SecondProvision));
        }
            
        [Test]
        public void Names_ShouldBeEmpty()
        {
            var contactPicker = new BaseContactPicker<Contact>(Provider);

            contactPicker.Names.Should().BeEmpty();
        }

        [Test]
        public void DisplayedContacts_ShouldBeEmpty()
        {
            var contactPicker = new BaseContactPicker<Contact>(Provider);

            contactPicker.DisplayedContacts.Should().BeEmpty();
        }

        [Test]
        public void DisplayedContacts_ShouldBeEquivalentToFirstProvision_AfterFirstUpdateContacts()
        {
            var contactPicker = new BaseContactPicker<Contact>(Provider);

            contactPicker.UpdateContactsAsync().Wait();

            contactPicker.DisplayedContacts.Should().BeEquivalentTo(FirstProvision);
        }

        [Test]
        public void DisplayedContacts_ShouldBeInSameOrderAsFirstProvision_AfterFirstUpdateContacts()
        {
            var contactPicker = new BaseContactPicker<Contact>(Provider);

            contactPicker.UpdateContactsAsync().Wait();

            contactPicker.DisplayedContacts.Should().ContainInOrder(FirstProvision);
        }

        [Test]
        public void DisplayedContacts_ShouldBeEquivalentToSecondProvision_AfterSecondUpdateContacts()
        {
            var contactPicker = new BaseContactPicker<Contact>(Provider);

            contactPicker.UpdateContactsAsync().Wait();
            contactPicker.UpdateContactsAsync().Wait();

            contactPicker.DisplayedContacts.Should().BeEquivalentTo(SecondProvision);
        }

        [Test]
        public void Indexer_ShouldBeEquivalentToDisplayedContactsIndexer()
        {
            var contactPicker = new BaseContactPicker<Contact>(Provider);

            contactPicker.UpdateContactsAsync().Wait();

            for (int i = 0; i < contactPicker.DisplayedContacts.Count; i++)
                contactPicker[i].Should().Be(contactPicker.DisplayedContacts[i]);
        }

        [Test]
        public void Filter_ShouldBeShouldLeaveInDisplayedContactsOnlyThoseWithGivenPrefix()
        {
            var contactPicker = new BaseContactPicker<Contact>(Provider);

            contactPicker.UpdateContactsAsync().Wait();
            contactPicker.FilterContacts("A");

            contactPicker.DisplayedContacts.Should().BeEquivalentTo(new Contact("Alexander"));
        }

        [Test]
        public void Names_ShouldBeUpdated_AfterUpdateContacts()
        {
            var contactPicker = new BaseContactPicker<Contact>(Provider);

            contactPicker.UpdateContactsAsync().Wait();
            
            contactPicker.Names.Should().BeEquivalentTo(FirstProvision.Select(c => c.Name));
        }
    }
}