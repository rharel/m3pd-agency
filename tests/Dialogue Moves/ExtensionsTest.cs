using NUnit.Framework;

namespace rharel.M3PD.Agency.Dialogue_Moves.Tests
{
    [TestFixture]
    public sealed class ExtensionsTest
    {
        [Test]
        public void Test_IsIdle()
        {
            Assert.IsTrue(Idle.Instance.IsIdle());
            Assert.IsTrue(DialogueMoves.Create("idle").IsIdle());
            Assert.IsFalse(DialogueMoves.Create("not-idle").IsIdle());
        }

        [Test]
        public void Test_GetAddressee()
        {
            Assert.IsFalse(
                DialogueMoves.Create("greeting")
                .GetAddressee().IsSome
            );
            Assert.IsTrue(
                DialogueMoves.Create("greeting", "alice")
                .GetAddressee().Contains("alice")
            );
            Assert.IsFalse(
                DialogueMoves.Create("greeting", "alice", "bob")
                .GetAddressee().IsSome
            );
        }
    }
}
