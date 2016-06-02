using DynamicTranslator.Core.DBReezeNoSQL.Uow;

namespace DynamicTranslator.Tests
{
    using System;
    using DynamicTranslator.Core.Domain.Uow;
    using Xunit;

    public class UnitOfWorkBaseTest
    {
        [Fact]
        public void Begin_IfOptionsIsNull_ThrowsArgumentNullException()
        {
            #region Arrange
            UnitOfWorkBase sut = new DbReezeUnitOfWork(null, null);
            #endregion

            #region Act & Assert
            Assert.Throws(typeof(ArgumentNullException), () => sut.Begin(null));
            #endregion
        }
    }
}
