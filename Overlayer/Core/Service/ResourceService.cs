using Overlayer.Compat.Interface;
using Overlayer.Resource;

namespace Overlayer.Core.Service;

public sealed class ResourceService : IRuntimeService {
    public void Initialize() =>  ResourceManager.Initialize();
    public void Dispose() => ResourceManager.Dispose();
}