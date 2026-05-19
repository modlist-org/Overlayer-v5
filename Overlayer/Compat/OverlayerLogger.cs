using Overlayer.Compat.Interface;

namespace Overlayer.Compat;

public sealed class OverlayerLogger(IOverlayerLogger logger) {
    public void Msg(string msg) => logger.OverlayerMsg(msg);
    public void Wrn(string msg) => logger.OverlayerWrn(msg);
    public void Err(string msg) => logger.OverlayerErr(msg);
}