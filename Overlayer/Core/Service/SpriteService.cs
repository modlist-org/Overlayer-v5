using Overlayer.Compat.Interface;
using Overlayer.UI.SpriteManage;

namespace Overlayer.Core.Service;

public sealed class SpriteService : IRuntimeService {
    public void Initialize() => SpriteDatabase.Initialize();
    public void Dispose() => SpriteDatabase.Dispose();
}