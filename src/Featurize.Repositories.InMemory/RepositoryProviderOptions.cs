using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories.InMemory;
public static class RepositoryProviderOptionsExtentions
{
    public static RepositoryProviderOptions UseInMemory(this RepositoryProviderOptions options)
    {
        options.Provider = new InMemoryRepositoryProvider();
        return options;
    }

}
