using Moq;
using System;
using AutoFixture;

namespace RivertyPaymentService.Core.Tests
{
    public class TestBase : IDisposable
    {
        private  MockRepository mockRepository { get; set; }
        protected Fixture fixture { get; private set; }
        public TestBase() 
        {
            mockRepository = new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Empty };
            fixture = new Fixture();
        }

        protected Mock<T> Mock<T>() where T : class
        {
            return mockRepository.Create<T>();
        }


        public void Dispose()
        {
            mockRepository.VerifyAll();
        }
    }
}
