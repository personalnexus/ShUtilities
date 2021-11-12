using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilitiesTest.Mocks;

namespace ShUtilitiesTest.Tests
{

    [TestClass]
    public class PropertyChangedTest
    {
        private PropertyChangedMock _mock;
        private int _callbackTriggered;

        [TestMethod]
        public void SetAndRaise()=> Execute(nameof(_mock.SetAndRaise), x => _mock.SetAndRaise = x, () => _mock.SetAndRaise);

        [TestMethod]
        public void SetAndRaiseWithReflection() => Execute(nameof(_mock.SetAndRaiseWithReflection), x => _mock.SetAndRaiseWithReflection = x, () => _mock.SetAndRaiseWithReflection);

        public void Execute(string expectedPropertyName, Action<long> setter, Func<long> getter)
        {
            _callbackTriggered = 0;
            _mock = new();
            _mock.PropertyChanged += (sender, e) => 
            {
                _callbackTriggered++;
                Assert.AreEqual(expectedPropertyName, e.PropertyName);
                Assert.AreSame(_mock, sender);
            };
            Assert.AreEqual(0, getter());
            Assert.AreEqual(0, _callbackTriggered);
            setter(2);
            Assert.AreEqual(2, getter());
            Assert.AreEqual(1, _callbackTriggered);
            setter(2);
            Assert.AreEqual(2, getter());
            Assert.AreEqual(1, _callbackTriggered);
            setter(3);
            Assert.AreEqual(3, getter());
            Assert.AreEqual(2, _callbackTriggered);
        }
    }
}