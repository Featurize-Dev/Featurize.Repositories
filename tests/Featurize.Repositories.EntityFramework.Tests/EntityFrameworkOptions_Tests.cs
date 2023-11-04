using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories.EntityFramework.Tests;

public class EntityFrameworkOptions_Tests
{
    public class GetContext
    {
        [Test]
        public void returns_ContextType()
        {
            var options = new RepositoryOptions();
            
            options.UseContext<TestContext>();

            options.Should().ContainKey("UseContext");

            options["UseContext"].Should().Be(typeof(TestContext));
        }
    }
}
