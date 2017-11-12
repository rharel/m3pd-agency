using NUnit.Framework;
using System;
using System.Linq;

namespace rharel.M3PD.Agency.Dialogue_Moves.Tests
{
    [TestFixture]
    public sealed class DialogueMoveTest
    {
        private class Foo { }
        private sealed class FooDerived: Foo { }
        private sealed class Bar { }

        private static readonly string TYPE = "mock_type";
        private static readonly Foo PROPERTIES = new Foo();
        private static readonly string ADDRESSEE = "alice";
        private static readonly string[] ADDRESSEES =
        {
            "alice", "bob", "charlie"
        };

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => DialogueMoves.Create(null, PROPERTIES)
            );
            Assert.Throws<ArgumentException>(
                () => DialogueMoves.Create(" ", PROPERTIES)
            );
            Assert.Throws<ArgumentException>(
                () => DialogueMoves.Create(TYPE, " ")
            );
        }
        [Test]
        public void Test_Constructor_WithAddresee()
        {
            var move = DialogueMoves.Create(TYPE, ADDRESSEE);

            Assert.AreEqual(TYPE, move.Type);
            Assert.IsTrue(move.AddresseeIDs.Contains(ADDRESSEE));
        }
        [Test]
        public void Test_Constructor_WithAddresees()
        {
            var move = DialogueMoves.Create(TYPE, ADDRESSEES);

            Assert.AreEqual(TYPE, move.Type);
            Assert.IsTrue(ADDRESSEES.All(
                value => move.AddresseeIDs.Contains(value)
            ));
        }
        [Test]
        public void Test_Constructor_WithoutProperties()
        {
            var move = DialogueMoves.Create(TYPE);

            Assert.AreEqual(TYPE, move.Type);
            Assert.IsTrue(move.Properties.IsNone);
        }
        [Test]
        public void Test_Constructor_WithProperties()
        { 
            var move = DialogueMoves.Create(TYPE, PROPERTIES);

            Assert.AreEqual(TYPE, move.Type);
            Assert.IsTrue(move.Properties.Contains(PROPERTIES));
        }

        [Test]
        public void Test_Cast_ToInvalidType()
        {
            Assert.Throws<InvalidCastException>(
                () => DialogueMoves.Create(TYPE, new Foo()).Cast<Bar>()
            );
        }
        [Test]
        public void Test_Cast_Up()
        {
            var source = DialogueMoves.Create(TYPE, new FooDerived());
            var target = source.Cast<Foo>();

            Assert.AreEqual(TYPE, target.Type);
            Assert.AreEqual(target.Properties, source.Properties);
        }
        [Test]
        public void Test_Cast_Down()
        {
            var source = DialogueMoves.Create(TYPE, new FooDerived());
            var target = source.Cast<FooDerived>();

            Assert.AreEqual(TYPE, target.Type);
            Assert.AreEqual(target.Properties, source.Properties);
        }

        [Test]
        public void Test_Equality()
        {
            var original = DialogueMoves.Create(TYPE, PROPERTIES);
            var good_copy = DialogueMoves.Create(
                original.Type, original.Properties.Unwrap()
            );
            var flawed_type_copy = (
                DialogueMoves.Create(
                    $"other {original.Type}", 
                    original.Properties.Unwrap()
                )
            );
            var flawed_properties_copy = (
                DialogueMoves.Create(
                    original.Type,
                    $"other {original.Properties.Unwrap()}"
                )
            );

            Assert.AreNotEqual(original, null);
            Assert.AreNotEqual(original, "incompatible type");
            Assert.AreNotEqual(original, flawed_type_copy);
            Assert.AreNotEqual(original, flawed_properties_copy);

            Assert.AreEqual(original, original);
            Assert.AreEqual(original, good_copy);
        }
        [Test]
        public void Test_Equality_WhenCast()
        {
            Assert.AreEqual(
                DialogueMoves.Create(TYPE, PROPERTIES),
                DialogueMoves.Create(TYPE, PROPERTIES)
            );
        }
    }
}
