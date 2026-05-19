namespace Overlayer.Compat.Interface;

public interface IOverlayerLogger {
    void OverlayerMsg(string msg);
    void OverlayerWrn(string msg);
    void OverlayerErr(string msg);
}