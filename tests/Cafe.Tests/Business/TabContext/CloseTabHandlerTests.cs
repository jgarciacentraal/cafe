﻿using Cafe.Core.TabContext.Commands;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.TabContext
{
    public class CloseTabHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly TabTestsHelper _helper;

        public CloseTabHandlerTests()
        {
            _fixture = new SliceFixture();
            _helper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanCloseTab(Guid tabId, int tableNumber)
        {
            // Arrange
            await _helper.OpenTabOnTable(tabId, tableNumber);

            var closeTabCommand = new CloseTab
            {
                TabId = tabId,
                AmountPaid = 0 // This should be OK as we haven't ordered anything
            };

            // Act
            var result = await _fixture.SendAsync(closeTabCommand);

            // Assert
            await _helper.AssertTabExists(
                closeTabCommand.TabId,
                t => t.IsOpen == false &&
                     t.TipValue == 0);
        }
    }
}
